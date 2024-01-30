using Core.DTOs.QueryParametersDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductsRepository
{
    Task<PagedResult<Product>> GetProducts(ProductQueryParameters queryParams);
    Task<List<Product>?> GetProductsCollection(IEnumerable<int> ids);
    Task<Product?> GetProduct(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    void DeleteProductImage(ProductImage productImage);
}
