using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly AppDbContext appDbContext;


    public ProductsRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<PagedResult<Product>> GetProductsWithSpecsAsync(SpecificationParameters specsParams)
    {
        //query to filter the products
        var query = appDbContext.Products
                            .Include(p=>p.Brand).Include(p=>p.Category).Include(p=>p.Reviews);

        //Get a page of filtered products
        var pagedProductsData = await query
                            .OrderBy(p=>p.Name)
                            .Skip((specsParams.Page-1)*specsParams.PageSize)
                            .Take(specsParams.PageSize)
                            .ToListAsync();

        //Get total count of filterd products
        var totalProductsCount = await query.CountAsync();

        //return page of products with pagination metadata
       return new PagedResult<Product>(pagedProductsData, specsParams.Page, specsParams.PageSize, totalProductsCount);    }
}
