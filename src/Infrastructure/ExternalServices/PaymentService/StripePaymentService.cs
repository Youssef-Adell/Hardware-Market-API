using Core.Interfaces.IExternalServices;
using Stripe;

namespace Infrastructure.ExternalServices.PaymentService;

public class StripePaymentService : IPaymentService
{
    public async Task<string> CreatePaymentIntent(Guid orderId, double orderTotalAmount)
    {
        StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)orderTotalAmount * 100,
            Currency = "egp",
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string> { { "orderId", orderId.ToString() } }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        return paymentIntent.ClientSecret;
    }
}
