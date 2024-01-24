using System.ComponentModel.DataAnnotations;
using Core.DTOs.BrandDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly IBrandsService BrandsService;

    public BrandsController(IBrandsService BrandsService)
    {
        this.BrandsService = BrandsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var result = await BrandsService.GetBrands();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrand(int id)
    {
        var result = await BrandsService.GetBrand(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrand([FromForm] BrandForAddingDto BrandToAdd, [Required] IFormFile icon)
    {
        var BrandIconAsBytes = await ConvertFormFileToByteArray(icon);

        var BrandId = await BrandsService.AddBrand(BrandToAdd, BrandIconAsBytes);

        return CreatedAtAction(nameof(GetBrand), new { Id = BrandId }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandForUpdatingDto updatedProduct, IFormFile? newIcon)
    {
        byte[]? newBrandIconAsBytes = null;

        if (newIcon != null)
            newBrandIconAsBytes = await ConvertFormFileToByteArray(newIcon);

        await BrandsService.UpdateBrand(id, updatedProduct, newBrandIconAsBytes);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        await BrandsService.DeleteBrand(id);

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
