using System.ComponentModel.DataAnnotations;
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

    [NonAction]
    private async Task<List<Byte[]>> ConvertFormFilesToByteArrays(IFormFileCollection formFiles)
    {
        var imagesAsBytes = new List<byte[]>();

        foreach (var formFile in formFiles)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                imagesAsBytes.Add(memoryStream.ToArray());
            }
        }

        return imagesAsBytes;
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
    public async Task<IActionResult> AddProduct([FromForm] ProductForAddingDto productToAdd, [Required] IFormFileCollection images)
    {
        //Convert to list<byte[]> to make the service layer not depends on IFormFileCollection which is conisderd infrastructure details
        var productImagesAsBytes = await ConvertFormFilesToByteArrays(images);

        var productId = await productsService.AddProduct(productToAdd, productImagesAsBytes);

        return CreatedAtAction(nameof(GetProduct), new { id = productId }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductForUpdatingDto updatedProduct, [Required] IFormFileCollection imagesToAdd)
    {
        //Convert to list<byte[]> to make the service layer not depends on IFormFileCollection which is conisderd infrastructure details
        var productImagesAsBytes = await ConvertFormFilesToByteArrays(imagesToAdd);

        await productsService.UpdateProduct(id, updatedProduct, productImagesAsBytes);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await productsService.DeleteProduct(id);

        return NoContent();
    }

}
