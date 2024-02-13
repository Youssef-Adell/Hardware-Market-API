using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductReviewsService
{
    Task<PagedResult<ProductReviewResponse>> GetProductReviews(Guid productId, PaginationQueryParameters queryParams);
    Task<ProductReviewResponse> GetProductReview(Guid productId, Guid reviewId);
    Task<ProductReviewResponse> GetProductReview(Guid productId, string customerEmail);
    Task<Guid> AddProductReview(string customerEmail, Guid productId, ProductReviewAddRequest productReviewAddRequest);
    Task UpdateProductReview(string customerEmail, Guid productId, Guid reviewId, ProductReviewUpdateRequest ProductReviewUpdateRequest);
    Task DeleteProductReview(string customerEmail, Guid productId, Guid reviewId);
}