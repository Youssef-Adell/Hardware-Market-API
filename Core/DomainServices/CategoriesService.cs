using AutoMapper;
using Core.DTOs.CategoryDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;

namespace Core.DomainServices;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository categoriesRepository;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly string categoriesIconsFolder;
    private readonly int maxAllowedImageSizeInBytes;

    public CategoriesService(ICategoriesRepository categoriesRepository, IFileService fileService, IConfiguration configuration, IMapper mapper)
    {
        this.categoriesRepository = categoriesRepository;
        this.fileService = fileService;
        this.mapper = mapper;
        categoriesIconsFolder = configuration["ResourcesStorage:CategoriesIconsFolder"];
        maxAllowedImageSizeInBytes = int.Parse(configuration["ResourcesStorage:MaxAllowedImageSizeInBytes"]);
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetCategories()
    {
        var categoriesEntities = await categoriesRepository.GetCategories();

        var categoriesDtos = mapper.Map<IReadOnlyCollection<ProductCategory>, IReadOnlyCollection<CategoryDto>>(categoriesEntities);

        return categoriesDtos;
    }

    public async Task<CategoryDto> GetCategory(int id)
    {
        var categoryEntity = await categoriesRepository.GetCategory(id);

        if (categoryEntity is null)
            throw new NotFoundException($"Category not found.");

        var categoryDto = mapper.Map<ProductCategory?, CategoryDto>(categoryEntity);

        return categoryDto;
    }

    public async Task<int> AddCategory(CategoryForAddingDto categoryToAdd, byte[] categoryIcon)
    {
        await ValidateUploadedIcon(categoryIcon);

        var iconPath = await fileService.SaveFile(categoriesIconsFolder, categoryIcon);

        //map the dto to an entity then assign the path of uploaded icon to it
        var categoryEntity = mapper.Map<CategoryForAddingDto, ProductCategory>(categoryToAdd);
        categoryEntity.IconPath = iconPath;

        categoriesRepository.AddCategory(categoryEntity);

        await categoriesRepository.SaveChanges();
        return categoryEntity.Id;
    }

    public async Task UpdateCategory(int categoryId, CategoryForUpdatingDto updatedCategory, byte[]? newCategoryIcon)
    {
        //check category exsitence
        var category = await categoriesRepository.GetCategory(categoryId);
        if (category is null)
            throw new NotFoundException($"Category not found.");

        //map the dto to an entity
        var categoryEntity = mapper.Map(updatedCategory, category);

        //update icon if there is a new one uploaded
        if (newCategoryIcon != null && newCategoryIcon?.Length != 0)
        {
            await ValidateUploadedIcon(newCategoryIcon);
            fileService.DeleteFile(categoryEntity.IconPath);
            categoryEntity.IconPath = await fileService.SaveFile(categoriesIconsFolder, newCategoryIcon);
        }

        categoriesRepository.UpdateCategory(categoryEntity);

        await categoriesRepository.SaveChanges();
    }

    private async Task ValidateUploadedIcon(byte[] icon)
    {
        //ensure that the uploaded image has a valid image type and doesnt exceed the maxSizeAllowed 
        if (!await fileService.IsFileOfTypeImage(icon))
            throw new UnprocessableEntityException($"Icon has invalid format.");

        if (fileService.IsFileSizeExceedsLimit(icon, maxAllowedImageSizeInBytes))
            throw new UnprocessableEntityException($"Icon exceeds the maximum allowed size of {maxAllowedImageSizeInBytes / 1024} KB.");
    }
}
