using Core.DTOs.CategoryDTOs;
using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface ICategoriesRepository
{
    Task<IReadOnlyCollection<ProductCategory>> GetCategories();
    Task<ProductCategory?> GetCategory(int id);
}
