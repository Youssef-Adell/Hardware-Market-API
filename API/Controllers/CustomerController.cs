using System.Collections.ObjectModel;
using System.Security.Claims;
using API.Errors;
using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="401">If there is no access token has been provided with the request.</response>
/// <response code="403">If the access token has been provided but the user is not a customer.</response>
/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/customer")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
public class CustomerController : ControllerBase
{
    private readonly IOrdersService ordersService;

    public CustomerController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }


    [HttpGet("orders")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(ReadOnlyCollection<OrderForCustomerListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerOrders([FromQuery] PaginationQueryParameters queryParams)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await ordersService.GetCustomerOrders(customerId, queryParams);

        return Ok(result);
    }


    [HttpGet("orders/{id:Guid}")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerOrder(Guid id)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await ordersService.GetCustomerOrder(customerId, id);

        return Ok(result);
    }


    /// <response code="201">Returns the created order.</response>
    /// <response code="422">If there is insufficient stock of one or more product.</response>
    [HttpPost("orders")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateCustomerOrder(OrderAddRequest orderAddRequest)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var createdOrder = await ordersService.CreateOrder(customerId, orderAddRequest);

        return CreatedAtAction(nameof(GetCustomerOrder), new { id = createdOrder.Id }, createdOrder);
    }
}