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
    private readonly IBrandsRepository brandsRepository;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly string brandsLogosFolder;
    private readonly int maxAllowedImageSizeInBytes;

    public BrandsService(IBrandsRepository brandsRepository, IFileService fileService, IConfiguration configuration, IMapper mapper)
    {
        this.brandsRepository = brandsRepository;
        this.fileService = fileService;
        this.mapper = mapper;
        brandsLogosFolder = configuration["ResourcesStorage:BrandsLogosFolder"];
        maxAllowedImageSizeInBytes = int.Parse(configuration["ResourcesStorage:MaxAllowedImageSizeInBytes"]);
    }

    public async Task<IReadOnlyCollection<BrandDto>> GetBrands()
    {
        var brandsEntities = await brandsRepository.GetBrands();

        var brandsDtos = mapper.Map<IReadOnlyCollection<ProductBrand>, IReadOnlyCollection<BrandDto>>(brandsEntities);

        return brandsDtos;
    }

    public async Task<BrandDto> GetBrand(int id)
    {
        var brandEntity = await brandsRepository.GetBrand(id);

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

        brandsRepository.AddBrand(brandEntity);

        await brandsRepository.SaveChanges();
        return brandEntity.Id;
    }

    public async Task UpdateBrand(int brandId, BrandForUpdatingDto updatedBrand, byte[]? newBrandLogo)
    {
        var brand = await brandsRepository.GetBrand(brandId);
        if (brand is null)
            throw new NotFoundException($"Brand not found.");

        if (newBrandLogo != null && newBrandLogo?.Length > 0)
            await ValidateUploadedLogo(newBrandLogo);

        var brandEntity = mapper.Map(updatedBrand, brand);

        //update logo if there is a new one uploaded
        if (newBrandLogo != null && newBrandLogo?.Length != 0)
        {
            fileService.DeleteFile(brandEntity.LogoPath);
            brandEntity.LogoPath = await fileService.SaveFile(brandsLogosFolder, newBrandLogo);
        }

        brandsRepository.UpdateBrand(brandEntity);

        await brandsRepository.SaveChanges();
    }

    public async Task DeleteBrand(int id)
    {
        //check brand exsitence
        var brand = await brandsRepository.GetBrand(id);
        if (brand is null)
            throw new NotFoundException($"Brand not found.");

        brandsRepository.DeleteBrand(brand);
        await brandsRepository.SaveChanges();

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
