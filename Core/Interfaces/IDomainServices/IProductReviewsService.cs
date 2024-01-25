using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductReviewsService
{
    Task<PagedResult<ProductReviewDto>> GetProductReviews(int productId, SpecificationParameters specsParams);
}
