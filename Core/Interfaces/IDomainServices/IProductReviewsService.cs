using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductReviewsService
{
    Task<PagedResult<ProductReviewResponse>> GetProductReviews(Guid productId, PaginationQueryParameters queryParams);
    Task<ProductReviewResponse> AddProductReview(Guid customerId, Guid productId, ProductReviewAddRequest productReviewAddRequest);
    Task<ProductReviewResponse> GetCustomerProductReview(Guid customerId, Guid productId);
    Task<ProductReviewResponse> UpdateCustomerProductReview(Guid customerId, Guid productId, ProductReviewUpdateRequest ProductReviewUpdateRequest);
    Task DeleteCustomerProductReview(Guid customerId, Guid productId);
}