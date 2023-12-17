using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Interfaces;
using Infrastructure.Persistence.AppData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Infrastructure.Services;

public class ProductsService : IProductsService
{
    private readonly AppDbContext appDbContext;
    private readonly IMapper mapper;

    public ProductsService(AppDbContext appDbContext, IMapper mapper)
    {
        this.appDbContext = appDbContext;
        this.mapper = mapper;
    }
    public async Task<PagedResult<ProductForListDto>> GetProducts(SpecificationParameters specsParams)
    {   
        //Filtering Query
        var query = appDbContext.Products
                            .Include(p=>p.Brand).Include(p=>p.Category).Include(p=>p.Reviews);

        //Get a page of filterd data
        var productsEntities = await query
                            .OrderBy(p=>p.Name)
                            .Skip((specsParams.Page-1)*specsParams.PageSize)
                            .Take(specsParams.PageSize)
                            .ToListAsync();

        //Get count of filterd data
        var totalProductsCount = await query.CountAsync();

        //Get count of products in the current page
        var productCountInCurrentPage = productsEntities.Count();

        //Map entities to dtos
       var productsDtos = mapper.Map<List<Product>, List<ProductForListDto>>(productsEntities);

        //return page of dto
       return new PagedResult<ProductForListDto>(productsDtos, specsParams.Page, specsParams.PageSize, totalProductsCount);
    }

}
