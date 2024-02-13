using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface ICategoriesRepository
{
    Task<IReadOnlyCollection<Category>> GetCategories();
    Task<Category?> GetCategory(Guid id);
    Task<bool> CategoryExists(Guid id);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(Category category);
}
