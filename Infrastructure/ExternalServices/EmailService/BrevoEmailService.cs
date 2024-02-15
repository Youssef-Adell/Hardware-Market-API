using Core.Interfaces.IExternalServices;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.ExternalServices.EmailService;


public class BrevoEmailService : IEmailService
{
    public async Task SendConfirmationEmail(string userEmail, string confirmationLink)
    {
        Configuration.Default.ApiKey["api-key"] = Environment.GetEnvironmentVariable("BREVO_API_KEY");
        var apiInstance = new TransactionalEmailsApi();

        var to = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(userEmail) };

        var sendSmtpEmail = new SendSmtpEmail(templateId: 1, to: to, _params: new { ConfirmationLink = confirmationLink });

        await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
    }

    public async Task SendResetPasswordEmail(string userEmail, string passowrdResetPageLink)
    {
        Configuration.Default.ApiKey["api-key"] = Environment.GetEnvironmentVariable("BREVO_API_KEY");
        var apiInstance = new TransactionalEmailsApi();

        var to = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(userEmail) };

        var sendSmtpEmail = new SendSmtpEmail(templateId: 2, to: to, _params: new { PassowrdResetPageLink = passowrdResetPageLink });

        await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
    }
}

