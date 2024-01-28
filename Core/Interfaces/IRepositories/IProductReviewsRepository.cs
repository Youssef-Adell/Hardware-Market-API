using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductReviewsRepository
{
    Task<PagedResult<ProductReview>> GetProductReviews(int productId, SpecificationParameters specsParams);
    Task<ProductReview?> GetProductReview(int productId, int reviewId);
    Task<bool> HasCustomerReviewedProduct(int productId, string customerEmail);
    void AddProductReview(ProductReview review);
    void UpdateProductReview(ProductReview review);
    void DeleteProductReview(ProductReview review);
}
