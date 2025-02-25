using API.Errors;
using Core.DTOs.AuthDTOs;
using Core.Interfaces.IExternalServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly LinkGenerator linkGenerator;

    public AuthController(IAuthService authService, LinkGenerator linkGenerator)
    {
        this.authService = authService;
        this.linkGenerator = linkGenerator;
    }


    /// <response code="204">If the the account is created successfully. (a confirmation email will be sent in this case)</response>
    /// <response code="400">If the registeration info does not satisfy the requirments or either the username or email is taken by another user.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        await authService.Register(registerRequest);

        var confirmationEmailRequest = new ConfirmationEmailRequest { Email = registerRequest.Email };

        return await SendConfirmationEmail(confirmationEmailRequest);
    }


    /// <response code="200">If the email and password are valid.</response>
    /// <response code="401">If the email or password is invalid or the account is locked.</response>
    /// <response code="403">If the email is not confirmed yet.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var loginResponse = await authService.Login(loginRequest);

        return Ok(loginResponse);
    }


    /// <response code="200">If the refresh token is valid.</response>
    /// <response code="401">If the refresh token is invalid.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(RefreshRequest refreshRequest)
    {
        var loginResponse = await authService.Refresh(refreshRequest.RefreshToken);

        return Ok(loginResponse);
    }


    //when the user hit this endpoint there is message contains a link to the "confirm-email" endpoint with (accountId, token) in the query parameters will be sent to his email
    //and when the user hit this link a get request will be sent to the "confirm-email" endpoint which will take the token in the query param tp validate it and return an html page to indicate if the email confirmed or not

    /// <response code="204">If the the confirmation email is sent successfully.</response>
    [HttpPost("resend-confirmation-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SendConfirmationEmail(ConfirmationEmailRequest confirmationEmailRequest)
    {
        await authService.SendConfirmationEmail(confirmationEmailRequest.Email, linkGenerator.GetUriByAction(HttpContext, nameof(ConfirmEmail))!);

        return NoContent();
    }


    [HttpGet("confirm-email")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ContentResult> ConfirmEmail(string accountId, string token)
    {

        var isConfirmed = await authService.ConfirmEmail(accountId, token);

        string html;

        if (isConfirmed)
            html = await System.IO.File.ReadAllTextAsync(@"./wwwroot/Pages/confirmation-succeeded.html");
        else
            html = await System.IO.File.ReadAllTextAsync(@"./wwwroot/pages/confirmation-faild.html");

        return Content(html, "text/html");
    }


    //when the user enter his email and hit the "forget-password" endpoint there is message contains a link to the "reset-password-page" endpoint with (accountId, token) in the query parameters will be sent to it
    //the link will return an html page that has a form to set a new password and as i said there is accountId and resetCode included in the query parameter by the previous endpoint
    //when the user press the submit button to submit the new password there is a post request contains (accountId, token, newPassword) will be sent to "reset-password" endpoint

    /// <response code="204">If the the password-reset email is sent successfully.</response>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordRequest forgetPasswordRequest)
    {
        await authService.SendPasswordResetEmail(forgetPasswordRequest.Email, linkGenerator.GetUriByAction(HttpContext, nameof(GetPasswordResetPage))!);

        return NoContent();
    }


    [HttpGet("password-reset-page")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ContentResult> GetPasswordResetPage(string accountId, string token)
    {
        var html = await System.IO.File.ReadAllTextAsync(@"./wwwroot/Pages/reset-password.html");

        html = html.Replace("{{resetPasswordEndpointUrl}}", linkGenerator.GetUriByAction(HttpContext, nameof(ResetPassword)));

        return Content(html, "text/html");
    }


    [HttpPost("reset-password")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        await authService.ResetPassword(resetPasswordRequest);

        return NoContent();
    }
}
