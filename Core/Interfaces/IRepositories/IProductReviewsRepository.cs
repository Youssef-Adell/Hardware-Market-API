using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductReviewsRepository
{
    Task<PagedResult<ProductReview>> GetProductReviews(int productId, SpecificationParameters specsParams);
}
