using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductReviewsService
{
    Task<PagedResult<ProductReviewDto>> GetProductReviews(int productId, PaginationQueryParameters queryParams);
    Task<ProductReviewDto> GetProductReview(int productId, int reviewId);
    Task<int> AddProductReview(string customerEmail, int productId, ProductReviewForAddingDto reviewToAdd);
    Task UpdateProductReview(string customerEmail, int productId, int reviewId, ProductReviewForUpdatingDto updatedReview);
    Task DeleteProductReview(string customerEmail, int productId, int reviewId);
}
