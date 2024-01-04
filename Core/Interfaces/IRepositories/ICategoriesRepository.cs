using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface ICategoriesRepository
{
    Task<ProductCategory?> GetCategory(int id);
}
