using Core.DTOs.CategoryDTOs;

namespace Core.Interfaces.IDomainServices;

public interface ICategoriesService
{
    Task<IReadOnlyCollection<CategoryDto>> GetCategories();
    Task<CategoryDto> GetCategory(int id);
}
