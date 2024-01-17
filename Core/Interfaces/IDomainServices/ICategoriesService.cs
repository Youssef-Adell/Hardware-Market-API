using Core.DTOs.CategoryDTOs;

namespace Core.Interfaces.IDomainServices;

public interface ICategoriesService
{
    Task<IReadOnlyCollection<CategoryDto>> GetCategories();
}
