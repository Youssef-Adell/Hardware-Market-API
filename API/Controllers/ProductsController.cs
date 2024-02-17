using System.ComponentModel.DataAnnotations;
using API.Errors;
using Core.DTOs.ProductDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/products")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;

    public ProductsController(IProductsService productsService)
    {
        this.productsService = productsService;
    }


    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductForListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters queryParams)
    {
        var result = await productsService.GetProducts(queryParams);
        return Ok(result);
    }


    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await productsService.GetProduct(id);
        return Ok(result);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the created product.</response>
    /// <response code="422">If either the categoryId, brandId or the uploaded images are invalid.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin .</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddProduct([FromForm] ProductAddRequest productAddRequest, [Required] IFormFileCollection images)
    {
        //Convert to list<byte[]> to make the service layer not depends on IFormFileCollection which is conisderd infrastructure details
        var productImagesAsBytes = await ConvertFormFilesToByteArrays(images);

        var createdProduct = await productsService.AddProduct(productAddRequest, productImagesAsBytes);

        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="422">If either the categoryId, brandId or the uploaded images are invalid.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin .</response>
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] ProductUpdateRequest productUpdateRequest, IFormFileCollection imagesToAdd)
    {
        //Convert to list<byte[]> to make the service layer not depends on IFormFileCollection which is conisderd infrastructure details
        var productImagesAsBytes = await ConvertFormFilesToByteArrays(imagesToAdd);

        var UpdateProduct = await productsService.UpdateProduct(id, productUpdateRequest, productImagesAsBytes);

        return Ok(UpdateProduct);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="204">If the product is deleted successfully.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin .</response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await productsService.DeleteProduct(id);

        return NoContent();
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin .</response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
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
