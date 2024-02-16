using System.Security.Claims;
using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    private readonly IOrdersService ordersService;

    public CustomerController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }

    [HttpGet("orders")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetCustomerOrders([FromQuery] PaginationQueryParameters queryParams)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await ordersService.GetCustomerOrders(customerId, queryParams);

        return Ok(result);
    }

    [HttpGet("orders/{id:Guid}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetCustomerOrder(Guid id)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await ordersService.GetCustomerOrder(customerId, id);

        return Ok(result);
    }

    [HttpPost("orders")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateCustomerOrder(OrderAddRequest orderAddRequest)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var createdOrder = await ordersService.CreateOrder(customerId, orderAddRequest);

        return CreatedAtAction(nameof(GetCustomerOrder), new { id = createdOrder.Id }, createdOrder);
    }
}
