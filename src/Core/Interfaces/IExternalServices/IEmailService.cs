namespace Core.Interfaces.IExternalServices;

public interface IEmailService
{
    Task SendConfirmationEmail(string userEmail, string confirmationLink);
    Task SendResetPasswordEmail(string userEmail, string passowrdResetPageLink);
}
