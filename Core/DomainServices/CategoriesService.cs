using AutoMapper;
using Core.DTOs.CategoryDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository categoriesRepository;
    private readonly IMapper mapper;


    public CategoriesService(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        this.categoriesRepository = categoriesRepository;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetCategories()
    {
        var categoriesEntities = await categoriesRepository.GetCategories();

        var categoriesDtos = mapper.Map<IReadOnlyCollection<ProductCategory>, IReadOnlyCollection<CategoryDto>>(categoriesEntities);
    
        return categoriesDtos;
    }

    public async Task<CategoryDto> GetCategory(int id){
        var categoryEntity = await categoriesRepository.GetCategory(id);

        if(categoryEntity is null)
            throw new NotFoundException($"Category not found.");

        var categoryDto = mapper.Map<ProductCategory?, CategoryDto>(categoryEntity);

        return categoryDto;
    }

}
