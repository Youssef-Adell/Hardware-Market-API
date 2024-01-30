using Core.DTOs.OrderDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetOrders([FromQuery] OrdersSpecificationParameters specsParams)
    {
        var result = await ordersService.GetOrders(specsParams);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await ordersService.GetOrder(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderForCreatingDto order, [FromQuery] string customerEmail)
    {
        var orderId = await ordersService.CreateOrder(customerEmail, order);

        return CreatedAtAction(nameof(GetOrder), new { id = orderId }, null);
    }
}
