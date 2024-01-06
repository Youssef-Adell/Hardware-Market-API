using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductsService
{
    Task<PagedResult<ProductForListDto>> GetProducts(ProductsSpecificationParameters specsParams);
    Task<ProductDetailsDto> GetProduct(int id);
    Task<int> AddProduct(ProductForAddingDto productToAdd);
    Task AddImagesForProduct(int id, IEnumerable<byte[]> images);
}
