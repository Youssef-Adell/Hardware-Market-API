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

    public async Task<PagedResult<ProductReviewDto>> GetProductReviews(int productId, ReviewQueryParameters queryParams)
    {
        var pageOfReviewsEntities = await unitOfWork.ProductReviews.GetProductReviews(productId, queryParams);

        var pageOfReviewstDtos = mapper.Map<PagedResult<ProductReview>, PagedResult<ProductReviewDto>>(pageOfReviewsEntities);

        return pageOfReviewstDtos;
    }

    public async Task<ProductReviewDto> GetProductReview(int productId, int reviewId)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var reviewEntity = await unitOfWork.ProductReviews.GetProductReview(productId, reviewId);
        if (reviewEntity is null)
            throw new NotFoundException($"Review not found.");

        var reviewDto = mapper.Map<ProductReview?, ProductReviewDto>(reviewEntity);

        return reviewDto;
    }

    public async Task<int> AddProductReview(string customerEmail, int productId, ProductReviewForAddingDto reviewToAdd)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        if (await unitOfWork.ProductReviews.HasCustomerReviewedProduct(productId, customerEmail))
            throw new ConfilctException("You already reviewed this product.");

        var reviewEntity = mapper.Map<ProductReviewForAddingDto, ProductReview>(reviewToAdd);
        reviewEntity.CustomerEmail = customerEmail;
        reviewEntity.ProductId = productId;

        unitOfWork.ProductReviews.AddProductReview(reviewEntity);

        await unitOfWork.SaveChanges();

        return reviewEntity.Id;
    }

    public async Task UpdateProductReview(string customerEmail, int productId, int reviewId, ProductReviewForUpdatingDto updatedReview)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var review = await unitOfWork.ProductReviews.GetProductReview(productId, reviewId);
        if (review is null)
            throw new NotFoundException($"Review not found.");

        if (review.CustomerEmail != customerEmail)
            throw new ForbiddenException("You are not authorized to edit this review.");

        var reviewEntity = mapper.Map(updatedReview, review);

        unitOfWork.ProductReviews.UpdateProductReview(reviewEntity);

        await unitOfWork.SaveChanges();
    }

    public async Task DeleteProductReview(string customerEmail, int productId, int reviewId)
    {
        var product = await unitOfWork.Products.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var review = await unitOfWork.ProductReviews.GetProductReview(productId, reviewId);
        if (review is null)
            throw new NotFoundException($"Review not found.");

        if (review.CustomerEmail != customerEmail)
            throw new ForbiddenException("You are not authorized to delete this review.");

        unitOfWork.ProductReviews.DeleteProductReview(review);

        await unitOfWork.SaveChanges();
    }
}
