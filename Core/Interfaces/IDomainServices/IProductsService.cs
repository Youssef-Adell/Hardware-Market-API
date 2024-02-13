using Core.DTOs.ProductDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IProductsService
{
    Task<PagedResult<ProductForListResponse>> GetProducts(ProductQueryParameters queryParams);
    Task<ProductResponse> GetProduct(Guid id);
    Task<Guid> AddProduct(ProductAddRequest productAddRequest, List<byte[]> productImages);
    Task UpdateProduct(Guid id, ProductUpdateRequest productUpdateRequest, List<byte[]> imagesToAdd);
    Task DeleteProduct(Guid id);
    Task UpdateProductQuntity(Guid id, int newQuntity);

}
