using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using API.Errors;
using Core.DTOs.BrandDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/brands")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class BrandsController : ControllerBase
{
    private readonly IBrandsService brandsService;

    public BrandsController(IBrandsService brandsService)
    {
        this.brandsService = brandsService;
    }



    [HttpGet]
    [ProducesResponseType(typeof(ReadOnlyCollection<BrandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands()
    {
        var result = await brandsService.GetBrands();

        return Ok(result);
    }


    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBrand(Guid id)
    {
        var result = await brandsService.GetBrand(id);

        return Ok(result);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the created brand.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddBrand([FromForm] BrandAddRequest brandAddRequest, [Required] IFormFile icon)
    {
        var brandIconAsBytes = await ConvertFormFileToByteArray(icon);

        var createdBrand = await brandsService.AddBrand(brandAddRequest, brandIconAsBytes);

        return CreatedAtAction(nameof(GetBrand), new { Id = createdBrand.Id }, createdBrand);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the updated brand.</response>
    /// <response code="404">If the brand you are trying to update is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateBrand(Guid id, [FromForm] BrandUpdateRequest brandUpdateRequest, IFormFile? newIcon)
    {
        byte[]? newBrandIconAsBytes = null;

        if (newIcon != null)
            newBrandIconAsBytes = await ConvertFormFileToByteArray(newIcon);

        var updatedBrand = await brandsService.UpdateBrand(id, brandUpdateRequest, newBrandIconAsBytes);

        return Ok(updatedBrand);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="204">If the brand has been deleted sucessfully.</response>
    /// <response code="404">If the brand you are trying to delete is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteBrand(Guid id)
    {
        await brandsService.DeleteBrand(id);

        return NoContent();
    }


    private async Task<Byte[]> ConvertFormFileToByteArray(IFormFile formFile)
    {
        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
