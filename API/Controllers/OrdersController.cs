using Core.DTOs.QueryParametersDTOs;
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
}
