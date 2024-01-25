using AutoMapper;
using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class ProductReviewsService : IProductReviewsService
{
    private readonly IProductReviewsRepository productReviewsRepository;
    private readonly IMapper mapper;

    public ProductReviewsService(IProductReviewsRepository productReviewsRepository, IMapper mapper)
    {
        this.productReviewsRepository = productReviewsRepository;
        this.mapper = mapper;
    }

    public async Task<PagedResult<ProductReviewDto>> GetProductReviews(int productId, SpecificationParameters specsParams)
    {
        var pageOfReviewsEntities = await productReviewsRepository.GetProductReviews(productId, specsParams);

        var pageOfReviewstDtos = mapper.Map<PagedResult<ProductReview>, PagedResult<ProductReviewDto>>(pageOfReviewsEntities);

        return pageOfReviewstDtos;
    }

}
