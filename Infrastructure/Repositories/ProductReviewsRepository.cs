using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductReviewsRepository : IProductReviewsRepository
{
    private readonly AppDbContext appDbContext;

    public ProductReviewsRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<PagedResult<ProductReview>> GetProductReviews(int productId, SpecificationParameters specsParams)
    {
        //build a query to get reveiws of this product
        var query = appDbContext.ProductReviews.Where(r => r.ProductId == productId);

        //sort and paginate the above query then execute it
        var pagedReviewssData = await query
                            .Sort(specsParams.SortBy, specsParams.SortDirection)
                            .Paginate(specsParams.Page, specsParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total reviews count for this product
        var totalReviewsForThisProduct = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<ProductReview>(pagedReviewssData, specsParams.Page, specsParams.PageSize, totalReviewsForThisProduct);
    }

    public async Task<ProductReview?> GetProductReview(int productId, int reviewId)
    {
        var review = await appDbContext.ProductReviews
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.Id == reviewId && r.ProductId == productId);

        return review;
    }

    public void AddProductReview(ProductReview reviewToAdd)
    {
        appDbContext.ProductReviews.Add(reviewToAdd);
    }

    public async Task<bool> HasCustomerReviewedProduct(int productId, string customerEmail)
    {
        var hasReviewd = await appDbContext.ProductReviews.AnyAsync(r => r.ProductId == productId && r.CustomerEmail == customerEmail);

        return hasReviewd;
    }

    public async Task SaveChanges()
    {
        await appDbContext.SaveChangesAsync();
    }
}
