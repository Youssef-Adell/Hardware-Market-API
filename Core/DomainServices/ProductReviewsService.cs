using AutoMapper;
using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class ProductReviewsService : IProductReviewsService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductReviewsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;

        this.mapper = mapper;
    }

    public async Task<PagedResult<ProductReviewResponse>> GetProductReviews(Guid productId, PaginationQueryParameters queryParams)
    {
        var productExists = await unitOfWork.Products.ProductExists(productId);
        if (!productExists)
            throw new NotFoundException($"Product not found.");

        var pageOfReviewsEntities = await unitOfWork.ProductReviews.GetProductReviews(productId, queryParams);

        var pageOfReviewstDtos = mapper.Map<PagedResult<ProductReview>, PagedResult<ProductReviewResponse>>(pageOfReviewsEntities);

        return pageOfReviewstDtos;
    }

    public async Task<ProductReviewResponse> GetCustomerProductReview(Guid customerId, Guid productId)
    {
        var productExists = await unitOfWork.Products.ProductExists(productId);
        if (!productExists)
            throw new NotFoundException($"Product not found.");

        var reviewEntity = await unitOfWork.ProductReviews.GetCustomerProductReview(customerId, productId);
        if (reviewEntity is null)
            throw new NotFoundException($"You don't have a review for this product.");

        var reviewDto = mapper.Map<ProductReview?, ProductReviewResponse>(reviewEntity);

        return reviewDto;
    }

    public async Task<ProductReviewResponse> AddProductReview(Guid customerId, Guid productId, ProductReviewAddRequest productReviewAddRequest)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        if (await unitOfWork.ProductReviews.HasCustomerReviewedProduct(customerId, productId))
            throw new ConfilctException("You already reviewed this product.");

        var reviewEntity = mapper.Map<ProductReviewAddRequest, ProductReview>(productReviewAddRequest);
        reviewEntity.CustomerId = customerId;
        reviewEntity.ProductId = productId;

        unitOfWork.ProductReviews.AddProductReview(reviewEntity);
        await unitOfWork.SaveChanges();

        await UpdateProductAverageRating(product);

        var reviewDto = mapper.Map<ProductReview?, ProductReviewResponse>(reviewEntity);

        return reviewDto;
    }

    public async Task<ProductReviewResponse> UpdateCustomerProductReview(Guid customerId, Guid productId, ProductReviewUpdateRequest productReviewUpdateRequest)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var review = await unitOfWork.ProductReviews.GetCustomerProductReview(customerId, productId);
        if (review is null)
            throw new NotFoundException($"You don't have a review for this product to update.");

        var reviewEntity = mapper.Map(productReviewUpdateRequest, review);

        unitOfWork.ProductReviews.UpdateProductReview(reviewEntity);
        await unitOfWork.SaveChanges();

        await UpdateProductAverageRating(product);

        var reviewDto = mapper.Map<ProductReview?, ProductReviewResponse>(reviewEntity);

        return reviewDto;
    }

    public async Task DeleteCustomerProductReview(Guid customerId, Guid productId)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var review = await unitOfWork.ProductReviews.GetCustomerProductReview(customerId, productId);
        if (review is null)
            throw new NotFoundException($"You don't have a review for this product to delete.");

        unitOfWork.ProductReviews.DeleteProductReview(review);
        await unitOfWork.SaveChanges();

        await UpdateProductAverageRating(product);
    }

    private async Task UpdateProductAverageRating(Product product)
    {
        product.AverageRating = await unitOfWork.ProductReviews.CalculateAvgRatingForProduct(product.Id);

        unitOfWork.Products.UpdateProduct(product);

        await unitOfWork.SaveChanges();
    }
}
