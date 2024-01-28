using Core.DTOs.OrderDTOs;
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


    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderForCreatingDto order, [FromQuery] string customerEmail)
    {
        var orderId = await ordersService.CreateOrder(customerEmail, order);

        return Ok();
    }
}
