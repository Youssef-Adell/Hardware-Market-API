using Core.DTOs.ProductDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductsService
{
    Task<PagedResult<ProductForListResponse>> GetProducts(ProductQueryParameters queryParams);
    Task<ProductResponse> GetProduct(Guid id);
    Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest, List<byte[]> productImages);
    Task<ProductResponse> UpdateProduct(Guid id, ProductUpdateRequest productUpdateRequest, List<byte[]> imagesToAdd);
    Task DeleteProduct(Guid id);
    Task<ProductResponse> UpdateProductQuntity(Guid id, int newQuntity);

}
