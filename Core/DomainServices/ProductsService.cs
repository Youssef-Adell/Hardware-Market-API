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

    public async Task<int> AddProduct(ProductForAddingDto product, List<byte[]> productImages)
    {
        //check for category existence
        var category = await categoriesRepository.GetCategory(product.CategoryId);
        if (category is null)
            throw new BadRequestException($"No category with id: {product.CategoryId}.");

        //check for brand existence
        var brand = await brandsRepository.GetBrand(product.BrandId);
        if (brand is null)
            throw new BadRequestException($"No brand with id: {product.BrandId}.");

        //ensure that all images has a valid image type and doesnt exceed the maxSizeAllowed 
        const int maxSizeAllowedForImage = 2 * 1024 * 1024; // 2MB
        if (!await AreValidImageFiles(productImages, maxSizeAllowedForImage))
            throw new BadRequestException($"One or more image has invalid type or exceded the maximum size allowed: {maxSizeAllowedForImage / (1024 * 1024)}MB.");

        //save the images and get their relative paths
        var imagesPaths = new List<string>();
        foreach (var image in productImages)
            imagesPaths.Add(await fileService.SaveFile("Images/Products", image));

        //assign the images paths to the product then add it to the database
        var productEntity = mapper.Map<ProductForAddingDto, Product>(product);
        productEntity.Images = imagesPaths?.Select(path => new ProductImage { Path = path }).ToList();

        await productsRepository.AddProduct(productEntity);

        return productEntity.Id;
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

    public async Task UpdateProduct(int productId, ProductForUpdatingDto updatedProduct, List<byte[]> imagesToAdd)
    {
        //check for product existence
        var product = await productsRepository.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"The product with id: {productId} not found.");

        //ensure that all images has a valid image type and doesnt exceed the maxSizeAllowed 
        const int maxSizeAllowedForImage = 2 * 1024 * 1024; // 2MB
        if (!await AreValidImageFiles(imagesToAdd, maxSizeAllowedForImage))
            throw new BadRequestException($"One or more image has invalid type or exceded the maximum size allowed: {maxSizeAllowedForImage / (1024 * 1024)}MB.");

        //save the new images and get their relative paths
        var imagesPaths = new List<string>();
        foreach (var image in imagesToAdd)
            imagesPaths.Add(await fileService.SaveFile("Images/Products", image));

        //remove the images that selected to be removed
        if (updatedProduct?.IdsOfImagesToRemove != null)
        {
            var imagesToRemove = product.Images?
                                .Where(image => updatedProduct.IdsOfImagesToRemove.Contains(image.Id))
                                .ToList();

            /*
            imagesToRemove?.ForEach(async image =>
            {
                await productsRepository.DeleteProductImage(image);
                fileService.DeleteFile(image.Path);
            });

            the above code gives an exception says "A second operation was started on this context instance before a previous
            operation completed and Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance"
            To address this issue, you should make sure that each operation using the DbContext is awaited before starting the next one.
            The ForEach method with an async lambda (the code above) doesn't wait for each iteration to complete before moving on to the next one.
            see https://go.microsoft.com/fwlink/?linkid=2097913 for more details.
            
            so i used the normal foreach instead because it ensures that each deletion operation is awaited before moving on to the next one,
            preventing the concurrent use of the DbContext.
            */
            if (imagesToRemove != null)
                foreach (var image in imagesToRemove)
                {
                    await productsRepository.DeleteProductImage(image);
                    fileService.DeleteFile(image.Path);
                }

        }

        //update the product
        var productEntity = mapper.Map<ProductForUpdatingDto, Product>(updatedProduct);
        productEntity.Id = productId;
        productEntity.Images = imagesPaths?.Select(path => new ProductImage { Path = path }).ToList();

        await productsRepository.UpdateProduct(productEntity);
    }

    public async Task DeleteProduct(int id)
    {
        //check for product existence
        var product = await productsRepository.GetProduct(id);
        if (product is null)
            throw new NotFoundException($"The product with id: {id} not found.");

        await productsRepository.DeleteProduct(product);

        //delete the images from the storage
        product.Images?.ForEach(image => fileService.DeleteFile(image.Path));
    }
}
