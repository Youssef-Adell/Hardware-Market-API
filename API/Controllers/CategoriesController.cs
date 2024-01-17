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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var result = await categoriesService.GetCategory(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromForm] CategoryForAddingDto categoryToAdd, [Required] IFormFile icon)
    {
        var categoryIconAsBytes = await ConvertFormFileToByteArray(icon);

        var categoryId = await categoriesService.AddCategory(categoryToAdd, categoryIconAsBytes);

        return CreatedAtAction(nameof(GetCategory), new { Id = categoryId }, null);
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
