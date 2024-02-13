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
    private readonly IUnitOfWork unitOfWork;

    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly string categoriesIconsFolder;
    private readonly int maxAllowedImageSizeInBytes;

    public CategoriesService(IUnitOfWork unitOfWork, IFileService fileService, IConfiguration configuration, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.fileService = fileService;
        this.mapper = mapper;
        this.categoriesIconsFolder = configuration["ResourcesStorage:CategoriesIconsFolder"];
        this.maxAllowedImageSizeInBytes = int.Parse(configuration["ResourcesStorage:MaxAllowedImageSizeInBytes"]);
    }

    public async Task<IReadOnlyCollection<CategoryResponse>> GetCategories()
    {
        var categoriesEntities = await unitOfWork.Categories.GetCategories();

        var categoriesDtos = mapper.Map<IReadOnlyCollection<Category>, IReadOnlyCollection<CategoryResponse>>(categoriesEntities);

        return categoriesDtos;
    }

    public async Task<CategoryResponse> GetCategory(Guid id)
    {
        var categoryEntity = await unitOfWork.Categories.GetCategory(id);

        if (categoryEntity is null)
            throw new NotFoundException($"Category not found.");

        var categoryDto = mapper.Map<Category?, CategoryResponse>(categoryEntity);

        return categoryDto;
    }

    public async Task<Guid> AddCategory(CategoryAddRequest categoryAddRequest, byte[] categoryIcon)
    {
        await ValidateUploadedIcon(categoryIcon);

        var categoryEntity = mapper.Map<CategoryAddRequest, Category>(categoryAddRequest);

        categoryEntity.IconPath = await fileService.SaveFile(categoriesIconsFolder, categoryIcon); ;

        unitOfWork.Categories.AddCategory(categoryEntity);

        await unitOfWork.SaveChanges();

        return categoryEntity.Id;
    }

    public async Task UpdateCategory(Guid id, CategoryUpdateRequest categoryUpdateRequest, byte[]? newCategoryIcon)
    {
        var category = await unitOfWork.Categories.GetCategory(id);
        if (category is null)
            throw new NotFoundException($"Category not found.");

        if (newCategoryIcon != null && newCategoryIcon?.Length > 0)
            await ValidateUploadedIcon(newCategoryIcon);

        var categoryEntity = mapper.Map(categoryUpdateRequest, category);

        //update icon if there is a new one uploaded
        if (newCategoryIcon != null && newCategoryIcon?.Length > 0)
        {
            fileService.DeleteFile(categoryEntity.IconPath);
            categoryEntity.IconPath = await fileService.SaveFile(categoriesIconsFolder, newCategoryIcon);
        }

        unitOfWork.Categories.UpdateCategory(categoryEntity);

        await unitOfWork.SaveChanges();
    }

    public async Task DeleteCategory(Guid id)
    {
        var category = await unitOfWork.Categories.GetCategory(id);
        if (category is null)
            throw new NotFoundException($"Category not found.");

        unitOfWork.Categories.DeleteCategory(category);

        await unitOfWork.SaveChanges();

        fileService.DeleteFile(category.IconPath);
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
