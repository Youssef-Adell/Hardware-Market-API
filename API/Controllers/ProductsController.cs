using Core.Interfaces;
using Infrastructure.Persistence.AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;

    public ProductsController(IProductsService productsService)
    {
        this.productsService = productsService;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts(){
        var result = await productsService.GetProducts();
        return Ok(result);
    }
}
