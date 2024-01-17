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
    public async Task<IActionResult> GetCategories(){
        var result = await categoriesService.GetCategories();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id){
        var result = await categoriesService.GetCategory(id);

        return Ok(result);
    }
}
