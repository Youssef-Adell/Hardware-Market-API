using System.ComponentModel.DataAnnotations;
using Core.DTOs.BrandDTOs;
using Core.Interfaces.IDomainServices;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrand(int id)
    {
        var result = await brandsService.GetBrand(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrand([FromForm] BrandForAddingDto BrandToAdd, [Required] IFormFile icon)
    {
        var BrandIconAsBytes = await ConvertFormFileToByteArray(icon);

        var BrandId = await brandsService.AddBrand(BrandToAdd, BrandIconAsBytes);

        return CreatedAtAction(nameof(GetBrand), new { Id = BrandId }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandForUpdatingDto updatedBrand, IFormFile? newIcon)
    {
        byte[]? newBrandIconAsBytes = null;

        if (newIcon != null)
            newBrandIconAsBytes = await ConvertFormFileToByteArray(newIcon);

        await brandsService.UpdateBrand(id, updatedBrand, newBrandIconAsBytes);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrand(int id)
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
