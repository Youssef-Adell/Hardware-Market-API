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
    public async Task<IActionResult> GetProductReviews(Guid productId, [FromQuery] PaginationQueryParameters queryParams)
    {
        var result = await productReviewsService.GetProductReviews(productId, queryParams);

        return Ok(result);
    }

    [HttpGet("current-customer-review")]
    public async Task<IActionResult> GetProductReviews(Guid productId, string customerEmail)
    {
        var result = await productReviewsService.GetProductReview(productId, customerEmail);

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProductReview(Guid productId, Guid id)
    {
        var result = await productReviewsService.GetProductReview(productId, id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductReview(Guid productId, ProductReviewAddRequest productReviewAddRequest, [FromQuery] string customerEmail)
    {
        var reviewId = await productReviewsService.AddProductReview(customerEmail, productId, productReviewAddRequest);

        return CreatedAtAction(nameof(GetProductReview), new { productId = productId, id = reviewId }, null);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateProductReview(Guid productId, Guid id, ProductReviewUpdateRequest productReviewUpdateRequest, [FromQuery] string customerEmail)
    {
        await productReviewsService.UpdateProductReview(customerEmail, productId, id, productReviewUpdateRequest);

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteProductReview(Guid productId, Guid id, [FromQuery] string customerEmail) //customerEmail param would be removed later when adding the authentication
    {
        await productReviewsService.DeleteProductReview(customerEmail, productId, id);

        return NoContent();
    }
}