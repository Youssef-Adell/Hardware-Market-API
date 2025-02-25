using System.Collections.ObjectModel;
using API.Errors;
using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
/// <response code="401">If there is no access token has been provided with the request.</response>
/// <response code="403">If the access token has been provided but the user is not an admin .</response>
[ApiController]
[Route("api/orders")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ReadOnlyCollection<OrderForAdminListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders([FromQuery] OrderQueryParameters queryParams)
    {
        var result = await ordersService.GetOrders(queryParams);

        return Ok(result);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    [HttpGet("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var result = await ordersService.GetOrder(id);

        return Ok(result);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="400">If the status value is invalid.</response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
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
            case EventTypes.PaymentIntentSucceeded:
                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                var orderId = Guid.Parse(paymentIntent.Metadata["orderId"]);
                await ordersService.UpdateOrderStatus(orderId, OrderStatus.Orderd);
                break;

            case EventTypes.PaymentIntentPaymentFailed:
                paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                orderId = Guid.Parse(paymentIntent.Metadata["orderId"]);
                await ordersService.UpdateOrderStatus(orderId, OrderStatus.Failed);
                break;
        }

        return NoContent();
    }
}
