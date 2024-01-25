using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductReviewsRepository
{
    Task<PagedResult<ProductReview>> GetProductReviews(int productId, SpecificationParameters specsParams);
    Task<ProductReview?> GetProductReview(int productId, int reviewId);
    void AddProductReview(ProductReview reviewToAdd);
    Task<bool> HasCustomerReviewedProduct(int productId, string customerEmail);
    Task SaveChanges();
}
