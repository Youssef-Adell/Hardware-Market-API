using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductsService
{
    Task<PagedResult<ProductForListDto>> GetProducts(ProductsSpecificationParameters specsParams);
    Task<ProductDetailsDto> GetProduct(int id);
    Task<int> AddProduct(ProductForAddingDto product, List<byte[]> productImages);
    Task UpdateProduct(int productId, ProductForUpdatingDto updatedProduct, List<byte[]> imagesToAdd);
    Task DeleteProduct(int id);
}
