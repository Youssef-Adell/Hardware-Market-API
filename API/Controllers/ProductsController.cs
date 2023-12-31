using Core.DTOs.SpecificationDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/categories/{categoryId}/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;

    public ProductsController(IProductsService productsService)
    {
        this.productsService = productsService;
    }


    [HttpGet]
    public async Task<IActionResult> GetProductsForCategory(int categoryId, [FromQuery] ProductsSpecificationParameters specsParams)
    {
        var result = await productsService.GetProductsForCategory(categoryId, specsParams);
        return Ok(result);
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetProductsForCategory(int categoryId, int productId)
    {
        var result = await productsService.GetProduct(categoryId, productId);
        return Ok(result);
    }
}
