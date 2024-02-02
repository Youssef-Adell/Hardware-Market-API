using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;
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
    public async Task<IActionResult> GetProductReviews(int productId, [FromQuery] PaginationQueryParameters queryParams)
    {
        var result = await productReviewsService.GetProductReviews(productId, queryParams);

        return Ok(result);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetProductReviews(int productId, string customerEmail)
    {
        var result = await productReviewsService.GetProductReview(productId, customerEmail);

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductReview(int productId, int id, ProductReviewForUpdatingDto updatedReview, [FromQuery] string customerEmail)
    {
        await productReviewsService.UpdateProductReview(customerEmail, productId, id, updatedReview);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductReview(int productId, int id, [FromQuery] string customerEmail) //customerEmail param would be removed later when adding the authentication
    {
        await productReviewsService.DeleteProductReview(customerEmail, productId, id);

        return NoContent();
    }
}