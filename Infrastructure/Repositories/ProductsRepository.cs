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

    public async Task<PagedResult<Product>> GetProductsForCategoryAsync(int categoryId, ProductsSpecificationParameters specsParams)
    {
        //build a query of filtered products
        var query = appDbContext.Products.Include(p => p.Brand).Include(p => p.Category)
                            .Where(p => p.CategoryId == categoryId)
                            //search (Short Circuit if no value in search)
                            .Where(p => string.IsNullOrEmpty(specsParams.Search) || p.Name.ToLower().Contains(specsParams.Search.ToLower()))
                            //filter (Short circuit if no value for categoryId & brandId)
                            .Where(p => p.Price >= specsParams.MinPrice && p.Price <= specsParams.MaxPrice)
                            .Where(p => specsParams.BrandId == null || p.BrandId == specsParams.BrandId);

        //sort and paginate the above query then execute it
        var pagedProductsData = await query
                            .Sort(specsParams.SortBy, specsParams.SortDirection)
                            .Paginate(specsParams.Page, specsParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total count of filterd products
        var totalProductsCount = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<Product>(pagedProductsData, specsParams.Page, specsParams.PageSize, totalProductsCount);
    }

    public Task<Product?> GetProduct(int categoryId, int productId)
    {
        var product = appDbContext.Products
                            .Include(p => p.Brand).Include(p => p.Category)
                            .FirstOrDefaultAsync(p => p.Id == productId && p.CategoryId == categoryId);
        return product;
    }
}
