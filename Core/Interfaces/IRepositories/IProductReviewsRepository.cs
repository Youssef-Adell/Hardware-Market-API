using Core.DTOs.QueryParametersDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductReviewsRepository
{
    Task<PagedResult<ProductReview>> GetProductReviews(int productId, PaginationQueryParameters queryParams);
    Task<ProductReview?> GetProductReview(int productId, int reviewId);
    Task<ProductReview?> GetProductReview(int productId, string customerEmail);
    Task<bool> HasCustomerReviewedProduct(int productId, string customerEmail);
    void AddProductReview(ProductReview review);
    void UpdateProductReview(ProductReview review);
    void DeleteProductReview(ProductReview review);
}
