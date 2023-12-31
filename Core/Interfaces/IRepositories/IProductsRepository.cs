using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductsRepository
{
    Task<PagedResult<Product>> GetProductsForCategory(int categoryId, ProductsSpecificationParameters specsParams);
    Task<Product?> GetProduct(int categoryId, int productId);
}
