using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductReviewsService
{
    Task<PagedResult<ProductReviewResponse>> GetProductReviews(Guid productId, PaginationQueryParameters queryParams);
    Task<ProductReviewResponse> GetProductReview(Guid productId, Guid reviewId);
    Task<ProductReviewResponse> GetCustomerProductReview(Guid customerId, Guid productId);
    Task<Guid> AddProductReview(Guid customerId, Guid productId, ProductReviewAddRequest productReviewAddRequest);
    Task UpdateProductReview(Guid customerId, Guid productId, Guid reviewId, ProductReviewUpdateRequest ProductReviewUpdateRequest);
    Task DeleteProductReview(Guid customerId, Guid productId, Guid reviewId);
}