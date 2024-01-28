using AutoMapper;
using Core.DTOs.BrandDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;

namespace Core.DomainServices;

public class BrandsService : IBrandsService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly string brandsLogosFolder;
    private readonly int maxAllowedImageSizeInBytes;

    public BrandsService(IUnitOfWork unitOfWork, IFileService fileService, IConfiguration configuration, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.fileService = fileService;
        this.mapper = mapper;
        this.brandsLogosFolder = configuration["ResourcesStorage:BrandsLogosFolder"];
        this.maxAllowedImageSizeInBytes = int.Parse(configuration["ResourcesStorage:MaxAllowedImageSizeInBytes"]);
    }

    public async Task<IReadOnlyCollection<BrandDto>> GetBrands()
    {
        var brandsEntities = await unitOfWork.Brands.GetBrands();

        var brandsDtos = mapper.Map<IReadOnlyCollection<ProductBrand>, IReadOnlyCollection<BrandDto>>(brandsEntities);

        return brandsDtos;
    }

    public async Task<BrandDto> GetBrand(int id)
    {
        var brandEntity = await unitOfWork.Brands.GetBrand(id);

        if (brandEntity is null)
            throw new NotFoundException($"Brand not found.");

        var brandDto = mapper.Map<ProductBrand?, BrandDto>(brandEntity);

        return brandDto;
    }

    public async Task<int> AddBrand(BrandForAddingDto brandToAdd, byte[] brandLogo)
    {
        await ValidateUploadedLogo(brandLogo);

        var brandEntity = mapper.Map<BrandForAddingDto, ProductBrand>(brandToAdd);

        brandEntity.LogoPath = await fileService.SaveFile(brandsLogosFolder, brandLogo); ;

        unitOfWork.Brands.AddBrand(brandEntity);

        await unitOfWork.SaveChanges();
        return brandEntity.Id;
    }

    public async Task UpdateBrand(int brandId, BrandForUpdatingDto updatedBrand, byte[]? newBrandLogo)
    {
        var brand = await unitOfWork.Brands.GetBrand(brandId);
        if (brand is null)
            throw new NotFoundException($"Brand not found.");

        if (newBrandLogo != null && newBrandLogo?.Length > 0)
            await ValidateUploadedLogo(newBrandLogo);

        var brandEntity = mapper.Map(updatedBrand, brand);

        //update logo if there is a new one uploaded
        if (newBrandLogo != null && newBrandLogo?.Length > 0)
        {
            fileService.DeleteFile(brandEntity.LogoPath);
            brandEntity.LogoPath = await fileService.SaveFile(brandsLogosFolder, newBrandLogo);
        }

        unitOfWork.Brands.UpdateBrand(brandEntity);

        await unitOfWork.SaveChanges();
    }

    public async Task DeleteBrand(int id)
    {
        var brand = await unitOfWork.Brands.GetBrand(id);
        if (brand is null)
            throw new NotFoundException($"Brand not found.");

        unitOfWork.Brands.DeleteBrand(brand);

        await unitOfWork.SaveChanges();

        fileService.DeleteFile(brand.LogoPath);
    }

    private async Task ValidateUploadedLogo(byte[] logo)
    {
        //ensure that the uploaded image has a valid image type and doesnt exceed the maxSizeAllowed 
        if (!await fileService.IsFileOfTypeImage(logo))
            throw new UnprocessableEntityException($"Logo has invalid format.");

        if (fileService.IsFileSizeExceedsLimit(logo, maxAllowedImageSizeInBytes))
            throw new UnprocessableEntityException($"Logo exceeds the maximum allowed size of {maxAllowedImageSizeInBytes / 1024} KB.");
    }
}
