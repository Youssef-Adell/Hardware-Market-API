using Core.DTOs.QueryParametersDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductsRepository
{
    Task<PagedResult<Product>> GetProducts(ProductQueryParameters queryParams);
    Task<List<Product>?> GetProductsCollection(IEnumerable<Guid> ids);
    Task<Product?> GetProduct(Guid id);
    Task<bool> ProductExists(Guid id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    void DeleteProductImage(ProductImage productImage);
}
