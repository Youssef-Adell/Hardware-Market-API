using Core.DTOs.OrderDTOs;
using Core.DTOs.SpecificationDTOs;
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
    public async Task<IActionResult> GetCustomerOrders([FromQuery] SpecificationParameters specsParams, string customerEmail)
    {
        var result = await ordersService.GetCustomerOrders(customerEmail, specsParams);

        return Ok(result);
    }

    [HttpGet("orders/{id}")]
    public async Task<IActionResult> GetCustomerOrder(int id, [FromQuery] string customerEmail)
    {
        var result = await ordersService.GetCustomerOrder(customerEmail, id);

        return Ok(result);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateUserOrder(OrderForCreatingDto order, [FromQuery] string customerEmail)
    {
        var orderId = await ordersService.CreateOrder(customerEmail, order);

        return CreatedAtAction(nameof(GetCustomerOrder), new { id = orderId }, null);
    }
}
