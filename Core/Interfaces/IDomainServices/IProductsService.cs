using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductsService
{
    Task<PagedResult<ProductForListDto>> GetProducts(ProductsSpecificationParameters specsParams);
    Task<ProductDetailsDto> GetProduct(int productId);
}
