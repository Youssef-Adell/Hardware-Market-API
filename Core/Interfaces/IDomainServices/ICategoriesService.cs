using Core.DTOs.CategoryDTOs;

namespace Core.Interfaces.IDomainServices;

public interface ICategoriesService
{
    Task<IReadOnlyCollection<CategoryResponse>> GetCategories();
    Task<CategoryResponse> GetCategory(Guid id);
    Task<Guid> AddCategory(CategoryAddRequest categoryAddRequest, byte[] categoryIcon);
    Task UpdateCategory(Guid id, CategoryUpdateRequest categoryUpdateRequest, byte[]? newCategoryIcon);
    Task DeleteCategory(Guid id);
}
