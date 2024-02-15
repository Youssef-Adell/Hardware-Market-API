


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.DTOs.AuthDTOs;
using Core.Entities.UserAggregate;
using Core.Exceptions;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.ExternalServices.AuthService.EFConfig;

namespace Infrastructure.ExternalServices.AuthService;


public class IdentityAuthService : IAuthService
{
    private readonly UserManager<Account> accountManager;
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration configuration;
    private readonly IEmailService emailService;

    public IdentityAuthService(UserManager<Account> accountManager, IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration configuration)
    {
        this.accountManager = accountManager;
        this.unitOfWork = unitOfWork;
        this.emailService = emailService;
        this.configuration = configuration;
    }

    public async Task Register(RegisterRequest registerRequest, string role = "Customer")
    {
        //create user to be saved in our business database
        var user = new User { Id = Guid.NewGuid(), Name = registerRequest.UserName, Email = registerRequest.Email };

        //create account in identity database and link it to the created user
        var account = new Account(registerRequest.UserName, registerRequest.Email, user.Id);

        var creationResult = await accountManager.CreateAsync(account, registerRequest.Password);

        if (!creationResult.Succeeded)
            throw new BadRequestException(creationResult.Errors.Humanize(e => $"{e.Description}"));

        try
        {
            var addingRolesResult = await accountManager.AddToRoleAsync(account, role); //it throws exception if the role doesnt exist so we wrap it with try block.

            if (!addingRolesResult.Succeeded)
                throw new BadRequestException(creationResult.Errors.Humanize(e => $"{e.Description}"));
        }
        catch (Exception ex)
        {
            await accountManager.DeleteAsync(account);
            throw new BadRequestException(ex.Message);
        }


        //save the user into business database if the account created successfully
        try
        {
            unitOfWork.Users.AddUser(user);
            await unitOfWork.SaveChanges();
        }
        catch
        {
            await accountManager.DeleteAsync(account);
            throw;
        }
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        var account = await accountManager.FindByEmailAsync(loginRequest.Email);
        if (account is null)
            throw new UnauthorizedException("Invalid email or password.");

        if (await accountManager.IsLockedOutAsync(account))
            throw new UnauthorizedException("The account has been temporarily locked due to several failed attempts to login.");

        if (!await accountManager.CheckPasswordAsync(account, loginRequest.Password))
        {
            await accountManager.AccessFailedAsync(account);
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!account.EmailConfirmed)
            throw new ForbiddenException("The email is not confirmed.");


        var roles = await accountManager.GetRolesAsync(account);
        var accessToken = await CreateAccessToken(account, roles);

        var loginResponse = new LoginResponse
        {
            UserId = account.OwnerId,
            UserEmail = account.Email!,
            UserName = account.UserName!,
            UserRole = roles.First(),
            AccessToken = accessToken.token,
            ExpiresIn = accessToken.expiresIn,
            RefreshToken = await CreateRefreshToken(account),
        };

        return loginResponse;
    }

    public async Task<LoginResponse> Refresh(string refreshToken)
    {
        var account = accountManager.Users.SingleOrDefault(account => account.RefreshToken == refreshToken);
        if (account is null)
            throw new UnauthorizedException("Invalid refresh token.");


        var roles = await accountManager.GetRolesAsync(account);
        var accessToken = await CreateAccessToken(account, roles);

        var loginResponse = new LoginResponse
        {
            UserId = account.OwnerId,
            UserEmail = account.Email!,
            UserName = account.UserName!,
            UserRole = roles.First(),
            AccessToken = accessToken.token,
            ExpiresIn = accessToken.expiresIn,
            RefreshToken = await CreateRefreshToken(account),
        };

        return loginResponse;
    }

    public async Task SendConfirmationEmail(string email, string confirmationEndpointUrl)
    {
        var account = await accountManager.FindByEmailAsync(email);

        if (account != null && !account.EmailConfirmed)
        {
            var token = await accountManager.GenerateEmailConfirmationTokenAsync(account);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmationLink = $"{confirmationEndpointUrl}?accountId={account.Id}&token={token}";

            await emailService.SendConfirmationEmail(email, confirmationLink);
        }
    }

    public async Task<bool> ConfirmEmail(string accountId, string token)
    {
        var account = await accountManager.FindByIdAsync(accountId);

        if (account == null)
            return false;

        if (!account.EmailConfirmed)
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await accountManager.ConfirmEmailAsync(account, token);

            if (!result.Succeeded)
                return false;
        }

        return true;
    }

    public async Task SendPasswordResetEmail(string email, string passwordResetPageUrl)
    {
        var account = await accountManager.FindByEmailAsync(email);

        if (account != null && account.EmailConfirmed)
        {
            var token = await accountManager.GeneratePasswordResetTokenAsync(account);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var passowrdResetPageLink = $"{passwordResetPageUrl}?accountId={account.Id}&token={token}";

            await emailService.SendResetPasswordEmail(email, passowrdResetPageLink);
        }
    }

    public async Task ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        var account = await accountManager.FindByIdAsync(resetPasswordRequest.AccountId);

        if (account != null && account.EmailConfirmed)
        {
            resetPasswordRequest.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.Token));

            var result = await accountManager.ResetPasswordAsync(account, resetPasswordRequest.Token, resetPasswordRequest.NewPassword);

            if (!result.Succeeded)
                throw new UnauthorizedException(result.Errors.Humanize(e => $"{e.Description}"));
        }
    }

    public async Task ConfirmAdminEmail(string adminEmail)
    {
        var adminAccount = await accountManager.FindByEmailAsync(adminEmail);
        if (adminAccount == null)
            return;

        if (!adminAccount.EmailConfirmed)
        {
            var confirmationToken = await accountManager.GenerateEmailConfirmationTokenAsync(adminAccount);
            await accountManager.ConfirmEmailAsync(adminAccount, confirmationToken);
        }
    }


    private async Task<string> CreateRefreshToken(Account account)
    {
        var randomNumber = new Byte[32];

        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            // fills the byte array(randomNumber) with a cryptographically strong random sequence of values.
            randomNumberGenerator.GetBytes(randomNumber);
        }

        var refreshToken = Convert.ToBase64String(randomNumber);  // Converting the byte array to a base64 string to ensures that the refresh token is in a format that can be easily transmitted  or stored in the database

        //save the refresh token into the account record in the db to use it to get the account when the consumer tries to refresh the tokens later
        account.RefreshToken = refreshToken;

        await accountManager.UpdateAsync(account);

        return refreshToken;
    }

    private async Task<(string token, double expiresIn)> CreateAccessToken(Account account, IEnumerable<string> roles)
    {
        var jwtToken = new JwtSecurityToken(
            claims: await GetClaims(account, roles),
            signingCredentials: GetSigningCredentials(),
            expires: DateTime.Now.AddSeconds(Convert.ToDouble(configuration["JwtSettings:AccessTokenExpirationTimeInSec"]))
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var expiresIn = Convert.ToDouble(configuration["JwtSettings:AccessTokenExpirationTimeInSec"]);

        return (token, expiresIn);
    }

    private async Task<List<Claim>> GetClaims(Account account, IEnumerable<string> roles)
    {
        //JwtRegisteredClaimNames vs ClaimTypes see this https://stackoverflow.com/questions/50012155/jwt-claim-names , https://stackoverflow.com/questions/68252520/httpcontext-user-claims-doesnt-match-jwt-token-sub-changes-to-nameidentifie, https://stackoverflow.com/questions/57998262/why-is-claimtypes-nameidentifier-not-mapping-to-sub
        var claims = new List<Claim>
        {
            // new Claim(JwtRegisteredClaimNames.Sub, account.Id),
            new Claim("AccountId", account.OwnerId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, account.OwnerId.ToString()),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")));
        return new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
    }
}
