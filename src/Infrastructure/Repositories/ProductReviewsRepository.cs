using Core.DTOs.QueryParametersDTOs;
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

    public async Task<PagedResult<ProductReview>> GetProductReviews(Guid productId, PaginationQueryParameters queryParams)
    {
        //build a query to get reveiws of this product
        var query = appDbContext.ProductReviews.Where(r => r.ProductId == productId).Include(r => r.Customer);

        //sort and paginate the above query then execute it
        var pagedReviewssData = await query
                            .Sort("Id", SortDirection.Descending) //to put the latest created reviews at the top
                            .Paginate(queryParams.Page, queryParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total reviews count for this product
        var totalReviewsForThisProduct = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<ProductReview>(pagedReviewssData, queryParams.Page, queryParams.PageSize, totalReviewsForThisProduct);
    }

    public async Task<ProductReview?> GetProductReview(Guid productId, Guid reviewId)
    {
        var review = await appDbContext.ProductReviews
                        .Include(r => r.Customer)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.Id == reviewId && r.ProductId == productId);

        return review;
    }

    public async Task<ProductReview?> GetCustomerProductReview(Guid customerId, Guid productId)
    {
        var review = await appDbContext.ProductReviews
                        .Include(r => r.Customer)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ProductId == productId);

        return review;
    }

    public async Task<bool> HasCustomerReviewedProduct(Guid customerId, Guid productId)
    {
        var hasReviewd = await appDbContext.ProductReviews.AnyAsync(r => r.ProductId == productId && r.CustomerId == customerId);

        return hasReviewd;
    }

    public async Task<double> CalculateAvgRatingForProduct(Guid productId)
    {
        if (await appDbContext.ProductReviews.AnyAsync(r => r.ProductId == productId))
            return await appDbContext.ProductReviews.Where(r => r.ProductId == productId).AverageAsync(r => r.Rating);
        else
            return 0;
    }

    public void AddProductReview(ProductReview review)
    {
        appDbContext.ProductReviews.Add(review);
    }

    public void UpdateProductReview(ProductReview review)
    {
        appDbContext.ProductReviews.Update(review);
    }

    public void DeleteProductReview(ProductReview review)
    {
        appDbContext.ProductReviews.Remove(review);
    }
}
