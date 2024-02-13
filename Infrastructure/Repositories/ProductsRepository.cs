using Core.DTOs.QueryParametersDTOs;
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

    public async Task<PagedResult<Product>> GetProducts(ProductQueryParameters queryParams)
    {
        //build a query of filtered products
        var query = appDbContext.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Images)
                            //search (Short Circuit if no value in search)
                            .Where(p => string.IsNullOrEmpty(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower()))
                            //filter (Short circuit if no value for categoryId & brandId)
                            .Where(p => p.Price >= queryParams.MinPrice && p.Price <= queryParams.MaxPrice)
                            .Where(p => queryParams.CategoryId == null || queryParams.CategoryId == p.CategoryId)
                            .Where(p => queryParams.BrandId == null || queryParams.BrandId.Contains(p.BrandId));

        //sort and paginate the above query then execute it
        var pagedProductsData = await query
                            .Sort(queryParams.SortBy, queryParams.SortDirection)
                            .Paginate(queryParams.Page, queryParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total count of filterd products
        var totalProductsCount = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<Product>(pagedProductsData, queryParams.Page, queryParams.PageSize, totalProductsCount);
    }

    public async Task<List<Product>?> GetProductsCollection(IEnumerable<Guid> ids)
    {
        var productsCollection = await appDbContext.Products.Where(p => ids.Contains(p.Id))
                                    .Include(p => p.Images)
                                    .AsNoTracking()
                                    .ToListAsync();

        return productsCollection;
    }

    public Task<Product?> GetProduct(Guid id)
    {
        var product = appDbContext.Products
                            .Include(p => p.Brand).Include(p => p.Category).Include(p => p.Images)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<bool> ProductExists(Guid id)
    {
        var exists = await appDbContext.Products.AnyAsync(product => product.Id == id);

        return exists;
    }

    public void AddProduct(Product product)
    {
        appDbContext.Products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        appDbContext.Products.Update(product);
    }

    public void DeleteProduct(Product product)
    {
        appDbContext.Products.Remove(product);
    }

    public void DeleteProductImage(ProductImage productImage)
    {
        appDbContext.ProductImages.Remove(productImage);
    }
}
