namespace Core.Interfaces.IExternalServices;


public interface IPaymentService
{
    /// <summary>
    /// Creates a payment intent for an order.
    /// </summary>
    /// <param name="orderId">the order id which will be linked to this payment intent and it will be used later to update the order status when the customer payment either successeded or faild.</param>
    /// <param name="orderTotalAmount">the total amount of the order the customer have to pay.</param>
    /// <returns>a string represents the client secret for the payment intent that will be used later by frontend to redirect the customer to the payment page.</returns>
    Task<string> CreatePaymentIntent(Guid orderId, double orderTotalAmount);
}