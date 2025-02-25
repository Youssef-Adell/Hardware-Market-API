using Core.DTOs.AuthDTOs;

namespace Core.Interfaces.IExternalServices;

public interface IAuthService
{
    Task Register(RegisterRequest registerRequest, string role = "Customer");
    Task<LoginResponse> Login(LoginRequest loginRequest);
    Task<LoginResponse> Refresh(string refreshToken);
    Task SendConfirmationEmail(string email, string confirmationEndpointUrl);
    Task<bool> ConfirmEmail(string accountId, string token);
    Task SendPasswordResetEmail(string email, string passwordResetPageUrl);
    Task ResetPassword(ResetPasswordRequest resetPasswordRequest);

    /// <summary>
    /// This method must be called after each admin registeration to confirm the registerd email because no confirmation email will be sent to admins automatically like the normal registeration via register endpoint.
    /// </summary>
    /// <param name="adminEmail">the admin email to be confirmed</param>
    Task ConfirmAdminEmail(string adminEmail);
}