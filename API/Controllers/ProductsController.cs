using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductForAddingDto productForAddingDto)
    {
        var productId = await productsService.AddProduct(productForAddingDto);

        //pass productId as a value for route data and pass null as a value for body cause we dont need to a body in the GetProduct Action
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, null);
    }
}
