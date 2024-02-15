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
}