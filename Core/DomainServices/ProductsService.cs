using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using FileSignatures;
using FileSignatures.Formats;

namespace Core.DomainServices;

public class ProductsService : IProductsService
{
    private readonly IMapper mapper;
    private readonly IProductsRepository productsRepository;
    private readonly ICategoriesRepository categoriesRepository;
    private readonly IBrandsRepository brandsRepository;
    private readonly IFileService fileService;
    private readonly IFileFormatInspector fileFormatInspector;

    public ProductsService(IMapper mapper, IProductsRepository productsRepository, ICategoriesRepository categoriesRepository, IBrandsRepository brandsRepository, IFileService fileService, IFileFormatInspector fileFormatInspector)
    {
        this.mapper = mapper;
        this.productsRepository = productsRepository;
        this.categoriesRepository = categoriesRepository;
        this.brandsRepository = brandsRepository;
        this.fileService = fileService;
        this.fileFormatInspector = fileFormatInspector;
    }

    public async Task<PagedResult<ProductForListDto>> GetProducts(ProductsSpecificationParameters specsParams)
    {
        //check for category existence
        if (specsParams.CategoryId != null)
        {
            var category = await categoriesRepository.GetCategory(specsParams.CategoryId.GetValueOrDefault());
            if (category is null)
                throw new BadRequestException($"No category with id: {specsParams.CategoryId} to filter by.");
        }

        //check for brands existence
        if (specsParams.BrandId?.Any() ?? false)
        {
            var brandsIdsAvailable = (await brandsRepository.GetBrands()).Select(b => b.Id);
            if (!specsParams.BrandId.All(id => brandsIdsAvailable.Contains(id)))
                throw new BadRequestException($"No brands with these ids to filter by.");
        }

        //get the products
        var pageOfProductEntities = await productsRepository.GetProducts(specsParams);

        var pageOfProductDtos = mapper.Map<PagedResult<Product>, PagedResult<ProductForListDto>>(pageOfProductEntities);
        return pageOfProductDtos;
    }

    public async Task<ProductDetailsDto> GetProduct(int id)
    {
        var productEntity = await productsRepository.GetProduct(id);
        if (productEntity is null)
            throw new NotFoundException($"The product with id: {id} not found.");

        var productDto = mapper.Map<Product, ProductDetailsDto>(productEntity);
        return productDto;
    }

    public async Task<int> AddProduct(ProductForAddingDto productForAddingDto)
    {
        //check for category existence
        var category = await categoriesRepository.GetCategory(productForAddingDto.CategoryId);
        if (category is null)
            throw new BadRequestException($"No category with id: {productForAddingDto.CategoryId}.");

        //check for brand existence
        var brand = await brandsRepository.GetBrand(productForAddingDto.BrandId);
        if (brand is null)
            throw new BadRequestException($"No brand with id: {productForAddingDto.BrandId}.");

        //create the product
        var productEntity = mapper.Map<ProductForAddingDto, Product>(productForAddingDto);

        await productsRepository.AddProduct(productEntity);

        return productEntity.Id;
    }

    public async Task AddImagesForProduct(int id, IEnumerable<byte[]> images)
    {
        //ensure that there are images uploaded
        if (images is null || !images.Any())
            throw new BadRequestException("No images uploaded.");

        //ensure that all images has a valid image type and doesnt exceed the maxSizeAllowed 
        const int maxSizeAllowedForImage = 2 * 1024 * 1024; // 2MB
        if (!await AreValidImageFiles(images, maxSizeAllowedForImage))
            throw new BadRequestException($"One or more image has invalid type or exceded the maximum size allowed: {maxSizeAllowedForImage / (1024 * 1024)}MB.");

        //check for product existence
        var product = await productsRepository.GetProduct(id);
        if (product is null)
            throw new NotFoundException($"The product with id: {id} not found.");

        //save the images and assign them to the specified product 
        foreach (var image in images)
        {
            var imagePathForDb = await fileService.SaveFile("Images/Products", image);

            if (!string.IsNullOrEmpty(imagePathForDb))
                product?.Images?.Add(new ProductImage() { Path = imagePathForDb });
        }

        await productsRepository.UpdateProduct(product);
    }

    private async Task<bool> AreValidImageFiles(IEnumerable<byte[]> files, int maxSizeAllowedForImage)
    {
        foreach (var file in files)
        {
            //check for the file size
            if (file.Length > maxSizeAllowedForImage)
                return false;

            //check for the file type
            using (var memoryStream = new MemoryStream())
            {
                await memoryStream.WriteAsync(file);
                var fileFormat = fileFormatInspector.DetermineFileFormat(memoryStream);
                if (fileFormat is not Image)
                    return false;
            }
        }

        return true;
    }

}
