using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrders([FromQuery] OrderQueryParameters queryParams)
    {
        var result = await ordersService.GetOrders(queryParams);

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var result = await ordersService.GetOrder(id);

        return Ok(result);
    }

    [HttpPatch("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, OrderStatusUpdateRequest orderStatusUpdateRequest)
    {
        var updatedOrder = await ordersService.UpdateOrderStatus(id, orderStatusUpdateRequest.Status);

        return Ok(updatedOrder);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("payment-status-webhook")]
    public async Task<IActionResult> PaymentStatusStripeWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], Environment.GetEnvironmentVariable("STRIPE_PAYMEMT_STATUS_WEBHOOK_SECRET"));

        switch (stripeEvent.Type)
        {
            case Events.PaymentIntentSucceeded:
                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                var orderId = Guid.Parse(paymentIntent.Metadata["orderId"]);
                await ordersService.UpdateOrderStatus(orderId, OrderStatus.Orderd);
                break;

            case Events.PaymentIntentPaymentFailed:
                paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                orderId = Guid.Parse(paymentIntent.Metadata["orderId"]);
                await ordersService.UpdateOrderStatus(orderId, OrderStatus.Failed);
                break;
        }

        return NoContent();
    }
}
