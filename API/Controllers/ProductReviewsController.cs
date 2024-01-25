using Core.DTOs.ProductReviewDTOs;
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

    [HttpPost]
    public async Task<IActionResult> AddProductReview(int productId, ProductReviewForAddingDto reviewToAdd, [FromQuery] string customerEmail)
    {
        var reviewId = await productReviewsService.AddProductReview(customerEmail, productId, reviewToAdd);

        return CreatedAtAction(nameof(GetProductReview), new { productId = productId, id = reviewId }, null);
    }
}