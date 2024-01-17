using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;

namespace Core.DomainServices;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository productsRepository;
    private readonly ICategoriesRepository categoriesRepository;
    private readonly IBrandsRepository brandsRepository;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly string productsImagesFolder;
    private readonly int maxAllowedImageSizeInBytes;
    public ProductsService(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository, IBrandsRepository brandsRepository, IFileService fileService, IMapper mapper, IConfiguration configuration)
    {
        this.productsRepository = productsRepository;
        this.categoriesRepository = categoriesRepository;
        this.brandsRepository = brandsRepository;
        this.fileService = fileService;
        this.mapper = mapper;
        productsImagesFolder = configuration["ResourcesStorage:ProductsImagesFolder"];
        maxAllowedImageSizeInBytes =  int.Parse(configuration["ResourcesStorage:MaxAllowedImageSizeInBytes"]);
    }

    public async Task<PagedResult<ProductForListDto>> GetProducts(ProductsSpecificationParameters specsParams)
    {
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

    public async Task<int> AddProduct(ProductForAddingDto productToAdd, List<byte[]> productImages)
    {
        await ValidateCategory(productToAdd.CategoryId);
        await ValidateBrand(productToAdd.BrandId);
        await ValidateUploadedImages(productImages);

        var imagesPaths = await fileService.SaveFiles(productsImagesFolder, productImages);

        //map the dto to an entity then assign the paths of the uploaded images to it
        var productEntity = mapper.Map<ProductForAddingDto, Product>(productToAdd);
        productEntity.Images = imagesPaths?.Select(path => new ProductImage { Path = path }).ToList();

        productsRepository.AddProduct(productEntity);

        await productsRepository.SaveChanges();
        return productEntity.Id;
    }

    public async Task UpdateProduct(int productId, ProductForUpdatingDto updatedProduct, List<byte[]> imagesToAdd)
    {
        var product = await productsRepository.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"The product with id: {productId} not found.");

        await ValidateCategory(updatedProduct.CategoryId);
        await ValidateBrand(updatedProduct.BrandId);
        await ValidateUploadedImages(imagesToAdd);

        var imagesPaths = await fileService.SaveFiles(productsImagesFolder, imagesToAdd);

        //remove the images that selected to be removed
        if (updatedProduct?.IdsOfImagesToRemove != null)
        {
            var imagesToRemove = product.Images?.Where(image => updatedProduct.IdsOfImagesToRemove.Contains(image.Id)).ToList();

            imagesToRemove?.ForEach(image =>
            {
                productsRepository.DeleteProductImage(image);
                fileService.DeleteFile(image.Path);
            });

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

            so i used the normal foreach below instead because it ensures that each deletion operation is awaited before moving on to the next one,
            preventing the concurrent use of the DbContext.

            if (imagesToRemove != null)
                foreach (var image in imagesToRemove)
                {
                    productsRepository.DeleteProductImage(image);
                    fileService.DeleteFile(image.Path);
                }

            UPDATE:
            i used first code again and it doesnt give any errors now because i made the DeleteProductImage sync
            and extract the async saving intoa seperate method in the repo (to reduce the no of requests to the database)
            */
        }

        //map the dto to an entity then assign the paths of the uploaded images to it
        var productEntity = mapper.Map<ProductForUpdatingDto, Product>(updatedProduct);
        productEntity.Images = imagesPaths?.Select(path => new ProductImage { Path = path }).ToList();

        //to make it recognizable by the DB to perform the update
        productEntity.Id = productId;

        productsRepository.UpdateProduct(productEntity);
        await productsRepository.SaveChanges();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await productsRepository.GetProduct(id);
        if (product is null)
            throw new NotFoundException($"The product with id: {id} not found.");

        productsRepository.DeleteProduct(product);
        await productsRepository.SaveChanges();

        product.Images?.ForEach(image => fileService.DeleteFile(image.Path));
    }

    private async Task ValidateUploadedImages(List<byte[]> images)
    {
        //ensure that all images has a valid image type and doesnt exceed the maxSizeAllowed 
        foreach (var image in images)
        {
            if (!await fileService.IsFileOfTypeImage(image))
                throw new UnprocessableEntityException($"One or more images has invalid format.");

            if (fileService.IsFileSizeExceedsLimit(image, maxAllowedImageSizeInBytes))
                throw new UnprocessableEntityException($"One or more images exceed the maximum allowed size of {maxAllowedImageSizeInBytes / 1024} KB.");
        }
    }

    private async Task ValidateCategory(int categoryId)
    {
        var category = await categoriesRepository.GetCategory(categoryId);
        if (category is null)
            throw new UnprocessableEntityException($"Invalid category id.");
    }

    private async Task ValidateBrand(int brandId)
    {
        var brand = await brandsRepository.GetBrand(brandId);
        if (brand is null)
            throw new UnprocessableEntityException($"Invalidd brand id.");
    }

}