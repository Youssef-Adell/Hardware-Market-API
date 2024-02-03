using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;
using Core.Interfaces.IDomainServices;
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
    public async Task<IActionResult> GetOrders([FromQuery] OrderQueryParameters queryParams)
    {
        var result = await ordersService.GetOrders(queryParams);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await ordersService.GetOrder(id);

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatusDto orderStatusDto)
    {
        await ordersService.UpdateOrderStatus(id, orderStatusDto.Status);

        return NoContent();
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
                var orderId = int.Parse(paymentIntent.Metadata["orderId"]);
                await ordersService.UpdateOrderStatus(orderId, OrderStatus.Received);
                break;

            case Events.PaymentIntentPaymentFailed:
                paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                orderId = int.Parse(paymentIntent.Metadata["orderId"]);
                await ordersService.UpdateOrderStatus(orderId, OrderStatus.Failed);
                break;
        }

        return Ok();
    }
}
