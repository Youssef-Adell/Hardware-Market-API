using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces;

public interface IProductsService
{
    Task<PagedResult<ProductForListDto>> GetProducts(SpecificationParameters specsParams);
}
