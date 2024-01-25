using Core.DTOs.SpecificationDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/products/{productId}/reviews")]
public class ProductReviewsController : ControllerBase
{
    private readonly IProductReviewsService productReviewsService;

    public ProductReviewsController(IProductReviewsService productReviewsService)
    {
        this.productReviewsService = productReviewsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductReviews(int productId, [FromQuery] SpecificationParameters specsParams)
    {
        var result = await productReviewsService.GetProductReviews(productId, specsParams);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductReview(int productId, int id)
    {
        var result = await productReviewsService.GetProductReview(productId, id);

        return Ok(result);
    }
}