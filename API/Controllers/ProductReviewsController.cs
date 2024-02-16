using System.Security.Claims;
using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProductReview(Guid productId, Guid id)
    {
        var result = await productReviewsService.GetProductReview(productId, id);

        return Ok(result);
    }

    [HttpGet("current-customer-review")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetCurrentCustomerReview(Guid productId)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await productReviewsService.GetCustomerProductReview(customerId, productId);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddProductReview(Guid productId, ProductReviewAddRequest productReviewAddRequest)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var createdReview = await productReviewsService.AddProductReview(customerId, productId, productReviewAddRequest);

        return CreatedAtAction(nameof(GetProductReview), new { productId = productId, id = createdReview.Id }, createdReview);
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateProductReview(Guid productId, Guid id, ProductReviewUpdateRequest productReviewUpdateRequest)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var updatedReview = await productReviewsService.UpdateProductReview(customerId, productId, id, productReviewUpdateRequest);

        return Ok(updatedReview);
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> DeleteProductReview(Guid productId, Guid id)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await productReviewsService.DeleteProductReview(customerId, productId, id);

        return NoContent();
    }
}