using Core.DTOs.SpecificationDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;

    public ProductsController(IProductsService productsService)
    {
        this.productsService = productsService;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductsSpecificationParameters specsParams)
    {
        var result = await productsService.GetProducts(specsParams);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var result = await productsService.GetProduct(id);
        return Ok(result);
    }
}
