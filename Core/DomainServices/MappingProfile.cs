using AutoMapper;
using Core.DTOs.BrandDTOs;
using Core.DTOs.CategoryDTOs;
using Core.DTOs.OrderDTOs;
using Core.DTOs.ProductDTOs;
using Core.DTOs.ProductReviewDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;
using Core.Entities.ProductAggregate;
using Microsoft.Extensions.Configuration;

namespace Core.DomainServices;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //---Product Mapping---
        CreateMap<Product, ProductForListDto>()
            .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
            .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
            .ForMember(d => d.ImageUrl, options => options.MapFrom<PreviewImageUrlResolver>());

        CreateMap<PagedResult<Product>, PagedResult<ProductForListDto>>();

        CreateMap<Product, ProductDetailsDto>()
            .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
            .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Images, options => options.MapFrom<ImagesUrlsResolver>());

        CreateMap<ProductForAddingDto, Product>();
        CreateMap<ProductForUpdatingDto, Product>()
            .ForMember(d => d.Brand, options => options.MapFrom(s => (object)null)) //if we didnt set the value of Brand, Category objects to null ef core will use them and ignore the new brandId and CategoryId so they wont be updated in UpdateProduct method
            .ForMember(d => d.Category, options => options.MapFrom(s => (object)null));


        //---Categories Mapping---
        CreateMap<ProductCategory, CategoryDto>()
            .ForMember(d => d.IconUrl, options => options.MapFrom<IconUrlResolver>());
        CreateMap<CategoryForAddingDto, ProductCategory>();
        CreateMap<CategoryForUpdatingDto, ProductCategory>();

        //---Brands Mapping---
        CreateMap<ProductBrand, BrandDto>()
            .ForMember(d => d.LogoUrl, options => options.MapFrom<LogoUrlResolver>());
        CreateMap<BrandForAddingDto, ProductBrand>();
        CreateMap<BrandForUpdatingDto, ProductBrand>();

        //---ProductReviews Mapping---
        CreateMap<ProductReview, ProductReviewDto>();
        CreateMap<PagedResult<ProductReview>, PagedResult<ProductReviewDto>>();
        CreateMap<ProductReviewForAddingDto, ProductReview>();
        CreateMap<ProductReviewForUpdatingDto, ProductReview>();

        //---Orders Mapping---
        CreateMap<Order, OrderForAdminListDto>();
        CreateMap<PagedResult<Order>, PagedResult<OrderForAdminListDto>>();
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ImageUrl, Options => Options.MapFrom<OrderItemUrlResolver>());
        CreateMap<Order, OrderDetailsDto>();
        CreateMap<Order, OrderForCustomerListDto>();
        CreateMap<PagedResult<Order>, PagedResult<OrderForCustomerListDto>>();
    }
}



// ---Resolvers---
public class PreviewImageUrlResolver : IValueResolver<Product, ProductForListDto, string?>
{
    private readonly IConfiguration configration;
    public PreviewImageUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(Product source, ProductForListDto destination, string? destMember, ResolutionContext context)
    {
        //here we took the first image in the product images and return it to be assigned for the preiview image for the product which will be displayed in the list
        var pathOfFirstImage = source?.Images?.FirstOrDefault()?.Path;
        if (!string.IsNullOrEmpty(pathOfFirstImage))
        {
            var hostUrl = new Uri(configration["ResourcesStorage:HostUrl"]);
            var ImageUrl = new Uri(hostUrl, pathOfFirstImage).ToString();
            return ImageUrl;
        }

        return null;
    }
}

public class ImagesUrlsResolver : IValueResolver<Product, ProductDetailsDto, List<ProductImageDto>?>
{
    private readonly IConfiguration configration;
    public ImagesUrlsResolver(IConfiguration configration) => this.configration = configration;

    public List<ProductImageDto>? Resolve(Product source, ProductDetailsDto destination, List<ProductImageDto>? destMember, ResolutionContext context)
    {
        List<ProductImageDto>? imagesDtos = new();

        source?.Images?.ForEach(image =>
        {
            var hostUrl = new Uri(configration["ResourcesStorage:HostUrl"]);
            var imageUrl = new Uri(hostUrl, image.Path).ToString();

            imagesDtos.Add(new ProductImageDto() { Id = image.Id, Url = imageUrl });
        }
        );

        return imagesDtos.Count != 0 ? imagesDtos : null;
    }
}

public class IconUrlResolver : IValueResolver<ProductCategory, CategoryDto, string?>
{
    private readonly IConfiguration configration;
    public IconUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(ProductCategory source, CategoryDto destination, string? destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.IconPath))
        {
            var hostUrl = new Uri(configration["ResourcesStorage:HostUrl"]);
            var iconUrl = new Uri(hostUrl, source.IconPath).ToString();
            return iconUrl;
        }

        return null;
    }
}

public class LogoUrlResolver : IValueResolver<ProductBrand, BrandDto, string?>
{
    private readonly IConfiguration configration;
    public LogoUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(ProductBrand source, BrandDto destination, string? destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.LogoPath))
        {
            var hostUrl = new Uri(configration["ResourcesStorage:HostUrl"]);
            var iconUrl = new Uri(hostUrl, source.LogoPath).ToString();
            return iconUrl;
        }

        return null;
    }
}

public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string?>
{
    private readonly IConfiguration configration;
    public OrderItemUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(OrderItem source, OrderItemDto destination, string? destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ImagePath))
        {
            var hostUrl = new Uri(configration["ResourcesStorage:HostUrl"]);
            var imageUrl = new Uri(hostUrl, source.ImagePath).ToString();
            return imageUrl;
        }

        return null;
    }
}