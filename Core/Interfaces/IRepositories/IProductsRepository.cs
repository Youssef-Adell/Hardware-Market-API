using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IProductsRepository
{
    Task<PagedResult<Product>> GetProducts(ProductsSpecificationParameters specsParams);
    Task<Product?> GetProduct(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    void DeleteProductImage(ProductImage productImage);
    Task SaveChanges();
}
