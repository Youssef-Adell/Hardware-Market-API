using Core.DTOs.ProductDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductsService
{
    Task<PagedResult<ProductForListDto>> GetProducts(ProductQueryParameters queryParams);
    Task<ProductDetailsDto> GetProduct(int id);
    Task<int> AddProduct(ProductForAddingDto productToAdd, List<byte[]> productImages);
    Task UpdateProduct(int productId, ProductForUpdatingDto updatedProduct, List<byte[]> imagesToAdd);
    Task DeleteProduct(int id);
}
