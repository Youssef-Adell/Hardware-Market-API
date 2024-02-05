using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface ICategoriesRepository
{
    Task<IReadOnlyCollection<ProductCategory>> GetCategories();
    Task<ProductCategory?> GetCategory(int id);
    Task<bool> CategoryExists(int id);
    void AddCategory(ProductCategory category);
    void UpdateCategory(ProductCategory category);
    void DeleteCategory(ProductCategory category);
}
