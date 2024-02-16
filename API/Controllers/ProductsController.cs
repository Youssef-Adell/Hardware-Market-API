using System.ComponentModel.DataAnnotations;
using Core.DTOs.ProductDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters queryParams)
    {
        var result = await productsService.GetProducts(queryParams);
        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await productsService.GetProduct(id);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddProduct([FromForm] ProductAddRequest productAddRequest, [Required] IFormFileCollection images)
    {
        //Convert to list<byte[]> to make the service layer not depends on IFormFileCollection which is conisderd infrastructure details
        var productImagesAsBytes = await ConvertFormFilesToByteArrays(images);

        var createdProduct = await productsService.AddProduct(productAddRequest, productImagesAsBytes);

        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] ProductUpdateRequest productUpdateRequest, IFormFileCollection imagesToAdd)
    {
        //Convert to list<byte[]> to make the service layer not depends on IFormFileCollection which is conisderd infrastructure details
        var productImagesAsBytes = await ConvertFormFilesToByteArrays(imagesToAdd);

        var UpdateProduct = await productsService.UpdateProduct(id, productUpdateRequest, productImagesAsBytes);

        return Ok(UpdateProduct);
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await productsService.DeleteProduct(id);

        return NoContent();
    }

    [HttpPatch("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProductQuntity(Guid id, ProductQuntityUpdateRequest productQuntityUpdateRequest)
    {
        var updatedProduct = await productsService.UpdateProductQuntity(id, productQuntityUpdateRequest.Quantity);

        return Ok(updatedProduct);
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
}
