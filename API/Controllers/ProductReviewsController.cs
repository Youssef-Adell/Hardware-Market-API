using System.Security.Claims;
using API.Errors;
using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/products/{productId}/reviews")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class ProductReviewsController : ControllerBase
{
    private readonly IProductReviewsService productReviewsService;

    public ProductReviewsController(IProductReviewsService productReviewsService)
    {
        this.productReviewsService = productReviewsService;
    }


    /// <response code="404">If the product is not found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductReviews(Guid productId, [FromQuery] PaginationQueryParameters queryParams)
    {
        var result = await productReviewsService.GetProductReviews(productId, queryParams);

        return Ok(result);
    }


    /// <response code="404">If the product is not found or the current customer has no review for this product.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not a customer.</response>
    [HttpGet("current-customer-review")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(ProductReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCurrentCustomerReview(Guid productId)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await productReviewsService.GetCustomerProductReview(customerId, productId);

        return Ok(result);
    }


    /// <response code="200">ٌReturns the created review.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="409">If the customer has already reviewd this product.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not a customer.</response>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(ProductReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddProductReview(Guid productId, ProductReviewAddRequest productReviewAddRequest)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var createdReview = await productReviewsService.AddProductReview(customerId, productId, productReviewAddRequest);

        return CreatedAtAction(nameof(GetCurrentCustomerReview), new { productId = productId }, createdReview);
    }


    /// <response code="404">If the product is not found or the current customer has no review for this product to update.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not a customer.</response>
    [HttpPut("current-customer-review")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(ProductReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProductReview(Guid productId, ProductReviewUpdateRequest productReviewUpdateRequest)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var updatedReview = await productReviewsService.UpdateCustomerProductReview(customerId, productId, productReviewUpdateRequest);

        return Ok(updatedReview);
    }


    /// <response code="204">If the product is deleted successfully.</response>
    /// <response code="404">If the product is not found or the current customer has no review for this product to delete.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not a customer.</response>
    [HttpDelete("current-customer-review")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProductReview(Guid productId)
    {
        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await productReviewsService.DeleteCustomerProductReview(customerId, productId);

        return NoContent();
    }
}