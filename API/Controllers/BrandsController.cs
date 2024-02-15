using System.ComponentModel.DataAnnotations;
using Core.DTOs.BrandDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly IBrandsService brandsService;

    public BrandsController(IBrandsService brandsService)
    {
        this.brandsService = brandsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var result = await brandsService.GetBrands();

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetBrand(Guid id)
    {
        var result = await brandsService.GetBrand(id);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddBrand([FromForm] BrandAddRequest brandAddRequest, [Required] IFormFile icon)
    {
        var BrandIconAsBytes = await ConvertFormFileToByteArray(icon);

        var BrandId = await brandsService.AddBrand(brandAddRequest, BrandIconAsBytes);

        return CreatedAtAction(nameof(GetBrand), new { Id = BrandId }, null);
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBrand(Guid id, [FromForm] BrandUpdateRequest brandUpdateRequest, IFormFile? newIcon)
    {
        byte[]? newBrandIconAsBytes = null;

        if (newIcon != null)
            newBrandIconAsBytes = await ConvertFormFileToByteArray(newIcon);

        await brandsService.UpdateBrand(id, brandUpdateRequest, newBrandIconAsBytes);

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
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
