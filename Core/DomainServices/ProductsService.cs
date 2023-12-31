using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class ProductsService : IProductsService
{
    private IMapper mapper;
    private IProductsRepository productsRepository;

    public ProductsService(IProductsRepository productsRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.productsRepository = productsRepository;
    }
    public async Task<PagedResult<ProductForListDto>> GetProductsForCategory(int categoryId, ProductsSpecificationParameters specsParams)
    {
        var pageOfProductEntities = await productsRepository.GetProductsForCategoryAsync(categoryId, specsParams);

        //Map page of product entities to page of product dtos
        var pageOfProductDtos = mapper.Map<PagedResult<Product>, PagedResult<ProductForListDto>>(pageOfProductEntities);

        return pageOfProductDtos;
    }

}
