using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductsRepository
{
    Task<PagedResult<Product>> GetProducts(ProductsSpecificationParameters specsParams);
    Task<Product?> GetProduct(int id);
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
}
