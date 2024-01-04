using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class ProductsService : IProductsService
{
    private readonly IMapper mapper;
    private readonly IProductsRepository productsRepository;
    private readonly ICategoriesRepository categoriesRepository;
    private readonly IBrandsRepository brandsRepository;

    public ProductsService(IMapper mapper, IProductsRepository productsRepository, ICategoriesRepository categoriesRepository, IBrandsRepository brandsRepository)
    {
        this.mapper = mapper;
        this.productsRepository = productsRepository;
        this.categoriesRepository = categoriesRepository;
        this.brandsRepository = brandsRepository;
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
}
