using System.ComponentModel.DataAnnotations;
using Core.DTOs.CategoryDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        this.categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await categoriesService.GetCategories();

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var result = await categoriesService.GetCategory(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromForm] CategoryAddRequest categoryAddRequest, [Required] IFormFile icon)
    {
        var categoryIconAsBytes = await ConvertFormFileToByteArray(icon);

        var categoryId = await categoriesService.AddCategory(categoryAddRequest, categoryIconAsBytes);

        return CreatedAtAction(nameof(GetCategory), new { Id = categoryId }, null);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromForm] CategoryUpdateRequest categoryUpdateRequest, IFormFile? newIcon)
    {
        byte[]? newCategoryIconAsBytes = null;

        if (newIcon != null)
            newCategoryIconAsBytes = await ConvertFormFileToByteArray(newIcon);

        await categoriesService.UpdateCategory(id, categoryUpdateRequest, newCategoryIconAsBytes);

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await categoriesService.DeleteCategory(id);

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
