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

    public ProductsService(IMapper mapper, IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
    {
        this.mapper = mapper;
        this.productsRepository = productsRepository;
        this.categoriesRepository = categoriesRepository;
    }
    public async Task<PagedResult<ProductForListDto>> GetProducts(ProductsSpecificationParameters specsParams)
    {
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
        var productEntity = mapper.Map<ProductForAddingDto, Product>(productForAddingDto);

        await productsRepository.AddProduct(productEntity);

        return productEntity.Id;
    }
}
