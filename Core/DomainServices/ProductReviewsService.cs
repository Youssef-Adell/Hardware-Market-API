using AutoMapper;
using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class ProductReviewsService : IProductReviewsService
{
    private readonly IProductReviewsRepository productReviewsRepository;
    private readonly IProductsRepository productsRepository;
    private readonly IMapper mapper;

    public ProductReviewsService(IProductReviewsRepository productReviewsRepository, IProductsRepository productsRepository, IMapper mapper)
    {
        this.productReviewsRepository = productReviewsRepository;
        this.productsRepository = productsRepository;
        this.mapper = mapper;
    }

    public async Task<PagedResult<ProductReviewDto>> GetProductReviews(int productId, SpecificationParameters specsParams)
    {
        var pageOfReviewsEntities = await productReviewsRepository.GetProductReviews(productId, specsParams);

        var pageOfReviewstDtos = mapper.Map<PagedResult<ProductReview>, PagedResult<ProductReviewDto>>(pageOfReviewsEntities);

        return pageOfReviewstDtos;
    }

    public async Task<ProductReviewDto> GetProductReview(int productId, int reviewId)
    {
        var product = await productsRepository.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var reviewEntity = await productReviewsRepository.GetProductReview(productId, reviewId);
        if (reviewEntity is null)
            throw new NotFoundException($"Review not found.");

        var reviewDto = mapper.Map<ProductReview?, ProductReviewDto>(reviewEntity);

        return reviewDto;
    }

    public async Task<int> AddProductReview(string customerEmail, int productId, ProductReviewForAddingDto reviewToAdd)
    {
        var product = await productsRepository.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        if (await productReviewsRepository.HasCustomerReviewedProduct(productId, customerEmail))
            throw new ConfilctException("Customer has already reviewed this product.");

        var reviewEntity = mapper.Map<ProductReviewForAddingDto, ProductReview>(reviewToAdd);
        reviewEntity.CustomerEmail = customerEmail;
        reviewEntity.ProductId = productId;

        productReviewsRepository.AddProductReview(reviewEntity);
        await productReviewsRepository.SaveChanges();

        return reviewEntity.Id;
    }

    public async Task UpdateProductReview(string customerEmail, int productId, int reviewId, ProductReviewForUpdatingDto updatedReview)
    {
        var product = await productsRepository.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var review = await productReviewsRepository.GetProductReview(productId, reviewId);
        if (review is null)
            throw new NotFoundException($"Review not found.");

        if (review.CustomerEmail != customerEmail)
            throw new ForbiddenException("The review does not belong to the customer to edit it.");

        var reviewEntity = mapper.Map(updatedReview, review);

        productReviewsRepository.UpdateProductReview(reviewEntity);
        await productReviewsRepository.SaveChanges();
    }

    public async Task DeleteProductReview(string customerEmail, int productId, int reviewId)
    {
        var product = await productsRepository.GetProduct(productId);
        if (product is null)
            throw new NotFoundException($"Product not found.");

        var review = await productReviewsRepository.GetProductReview(productId, reviewId);
        if (review is null)
            throw new NotFoundException($"Review not found.");

        if (review.CustomerEmail != customerEmail)
            throw new ForbiddenException("The review does not belong to the customer to delete it.");

        productReviewsRepository.DeleteProductReview(review);
        await productReviewsRepository.SaveChanges();
    }
}
