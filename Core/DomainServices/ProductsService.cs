using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;

namespace Core.DomainServices;

public class ProductsService : IProductsService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly string productsImagesFolder;
    private readonly int maxAllowedImageSizeInBytes;

    public ProductsService(IUnitOfWork unitOfWork, IFileService fileService, IMapper mapper, IConfiguration configuration)
    {
        this.unitOfWork = unitOfWork;
        this.fileService = fileService;
        this.mapper = mapper;
        this.productsImagesFolder = configuration["ResourcesStorage:ProductsImagesFolder"];
        this.maxAllowedImageSizeInBytes = int.Parse(configuration["ResourcesStorage:MaxAllowedImageSizeInBytes"]);
    }

    public async Task<PagedResult<ProductForListDto>> GetProducts(ProductQueryParameters queryParams)
    {
        var pageOfProductEntities = await unitOfWork.Products.GetProducts(queryParams);

        var pageOfProductDtos = mapper.Map<PagedResult<Product>, PagedResult<ProductForListDto>>(pageOfProductEntities);
        return pageOfProductDtos;
    }

    public async Task<ProductDetailsDto> GetProduct(int id)
    {
        var productEntity = await unitOfWork.Products.GetProduct(id);
        if (productEntity is null)
            throw new NotFoundException($"Product not found.");

        var productDto = mapper.Map<Product, ProductDetailsDto>(productEntity);
        return productDto;
    }

    public async Task<int> AddProduct(ProductForAddingDto productToAdd, List<byte[]> productImages)
    {
        await ValidateCategory(productToAdd.CategoryId);
        await ValidateBrand(productToAdd.BrandId);

        if (productImages != null && productImages.Count > 0)
            await ValidateUploadedImages(productImages);

        var productEntity = mapper.Map<ProductForAddingDto, Product>(productToAdd);

        //add uploaded images to the product (if there are any)
        if (productImages != null && productImages.Count > 0)
        {
            var pathsOfSavedImages = await fileService.SaveFiles(productsImagesFolder, productImages);
            productEntity.Images = pathsOfSavedImages?.Select(path => new ProductImage { Path = path }).ToList();
        }

        unitOfWork.Products.AddProduct(productEntity);

        await unitOfWork.SaveChanges();

        return productEntity.Id;
    }

    public async Task UpdateProduct(int productId, ProductForUpdatingDto updatedProduct, List<byte[]> imagesToAdd)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        await ValidateCategory(updatedProduct.CategoryId);
        await ValidateBrand(updatedProduct.BrandId);

        if (imagesToAdd != null && imagesToAdd?.Count > 0)
            await ValidateUploadedImages(imagesToAdd);

        var productEntity = mapper.Map(updatedProduct, product);

        //remove the images that selected to be removed (if there are any)
        if (updatedProduct?.IdsOfImagesToRemove != null && updatedProduct.IdsOfImagesToRemove.Count > 0)
        {
            var imagesToRemove = productEntity.Images?
                                                .Where(image => updatedProduct.IdsOfImagesToRemove.Contains(image.Id))
                                                .ToList();

            imagesToRemove?.ForEach(image =>
            {
                unitOfWork.Products.DeleteProductImage(image);
                fileService.DeleteFile(image.Path);
            });
            /*
            imagesToRemove?.ForEach(async image =>
            {
                await unitOfWork.Products.DeleteProductImage(image);
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
                    unitOfWork.Products.DeleteProductImage(image);
                    fileService.DeleteFile(image.Path);
                }

            UPDATE:
            i used first code again and it doesnt give any errors now because i made the DeleteProductImage sync
            and extract the async saving intoa seperate method in the repo (to reduce the no of requests to the database)
            */
        }

        //add new uploaded images to the product (if there are any)
        if (imagesToAdd != null && imagesToAdd?.Count > 0)
        {
            var pathsOfSavedImages = await fileService.SaveFiles(productsImagesFolder, imagesToAdd);
            productEntity?.Images?.AddRange(pathsOfSavedImages.Select(path => new ProductImage { Path = path }));
        }

        unitOfWork.Products.UpdateProduct(productEntity);

        await unitOfWork.SaveChanges();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await unitOfWork.Products.GetProduct(id);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        unitOfWork.Products.DeleteProduct(product);

        await unitOfWork.SaveChanges();

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
        var category = await unitOfWork.Categories.GetCategory(categoryId);
        if (category is null)
            throw new UnprocessableEntityException($"Invalid category id.");
    }

    private async Task ValidateBrand(int brandId)
    {
        var brand = await unitOfWork.Brands.GetBrand(brandId);
        if (brand is null)
            throw new UnprocessableEntityException($"Invalid brand id.");
    }

}