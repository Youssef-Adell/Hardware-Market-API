using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
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
    public async Task<IActionResult> GetCustomerOrders([FromQuery] PaginationQueryParameters queryParams, string customerEmail)
    {
        var result = await ordersService.GetCustomerOrders(customerEmail, queryParams);

        return Ok(result);
    }

    [HttpGet("orders/{id:Guid}")]
    public async Task<IActionResult> GetCustomerOrder(Guid id, [FromQuery] string customerEmail)
    {
        var result = await ordersService.GetCustomerOrder(customerEmail, id);

        return Ok(result);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateUserOrder(OrderAddRequest orderAddRequest, [FromQuery] string customerEmail)
    {
        var orderId = await ordersService.CreateOrder(customerEmail, orderAddRequest);

        return CreatedAtAction(nameof(GetCustomerOrder), new { id = orderId }, null);
    }
}
