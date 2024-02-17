using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using API.Errors;
using Core.DTOs.CategoryDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/categories")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        this.categoriesService = categoriesService;
    }


    [HttpGet]
    [ProducesResponseType(typeof(ReadOnlyCollection<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
        var result = await categoriesService.GetCategories();

        return Ok(result);
    }


    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var result = await categoriesService.GetCategory(id);

        return Ok(result);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the created category.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddCategory([FromForm] CategoryAddRequest categoryAddRequest, [Required] IFormFile icon)
    {
        var categoryIconAsBytes = await ConvertFormFileToByteArray(icon);

        var createdCategory = await categoriesService.AddCategory(categoryAddRequest, categoryIconAsBytes);

        return CreatedAtAction(nameof(GetCategory), new { Id = createdCategory.Id }, createdCategory);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the updated category.</response>
    /// <response code="404">If the category you are trying to update is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromForm] CategoryUpdateRequest categoryUpdateRequest, IFormFile? newIcon)
    {
        byte[]? newCategoryIconAsBytes = null;

        if (newIcon != null)
            newCategoryIconAsBytes = await ConvertFormFileToByteArray(newIcon);

        var updatedCategory = await categoriesService.UpdateCategory(id, categoryUpdateRequest, newCategoryIconAsBytes);

        return Ok(updatedCategory);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="204">If the category has been deleted sucessfully.</response>
    /// <response code="404">If the category you are trying to delete is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
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
