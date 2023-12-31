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
    private IMapper mapper;
    private IProductsRepository productsRepository;
    private readonly ICategoriesRepository categoriesRepository;

    public ProductsService(IMapper mapper, IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
    {
        this.mapper = mapper;
        this.productsRepository = productsRepository;
        this.categoriesRepository = categoriesRepository;
    }
    public async Task<PagedResult<ProductForListDto>> GetProductsForCategory(int categoryId, ProductsSpecificationParameters specsParams)
    {
        //check for category existence
        var category = await categoriesRepository.GetCategory(categoryId);
        if (category is null)
            throw new NotFoundException($"The category with id: {categoryId} not found.");

        var pageOfProductEntities = await productsRepository.GetProductsForCategory(categoryId, specsParams);

        var pageOfProductDtos = mapper.Map<PagedResult<Product>, PagedResult<ProductForListDto>>(pageOfProductEntities);
        return pageOfProductDtos;
    }

    public async Task<ProductDetailsDto> GetProduct(int categoryId, int productId)
    {
        //check for category existence
        var category = await categoriesRepository.GetCategory(categoryId);
        if (category is null)
            throw new NotFoundException($"The category with id: {categoryId} not found.");

        var productEntity = await productsRepository.GetProduct(categoryId, productId);
        if (productEntity is null)
            throw new NotFoundException($"The product with id: {productId} not found.");

        var productDto = mapper.Map<Product, ProductDetailsDto>(productEntity);
        return productDto;
    }

}
