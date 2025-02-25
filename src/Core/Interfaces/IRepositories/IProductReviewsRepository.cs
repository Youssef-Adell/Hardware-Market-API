using Core.DTOs.QueryParametersDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductReviewsRepository
{
    Task<PagedResult<ProductReview>> GetProductReviews(Guid productId, PaginationQueryParameters queryParams);
    Task<ProductReview?> GetProductReview(Guid productId, Guid reviewId);
    Task<ProductReview?> GetCustomerProductReview(Guid customerId, Guid productId);
    Task<bool> HasCustomerReviewedProduct(Guid customerId, Guid productId);
    Task<double> CalculateAvgRatingForProduct(Guid productId);
    void AddProductReview(ProductReview review);
    void UpdateProductReview(ProductReview review);
    void DeleteProductReview(ProductReview review);
}
