using AutoMapper;
using Core.DTOs.BrandDTOs;
using Core.DTOs.CategoryDTOs;
using Core.DTOs.CouponDTOs;
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
        CreateMap<Product, ProductForListResponse>()
            .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
            .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
            .ForMember(d => d.ImageUrl, options => options.MapFrom<PreviewImageUrlResolver>());

        CreateMap<PagedResult<Product>, PagedResult<ProductForListResponse>>();

        CreateMap<Product, ProductResponse>()
            .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
            .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Images, options => options.MapFrom<ImagesUrlsResolver>());

        CreateMap<ProductAddRequest, Product>();
        CreateMap<ProductUpdateRequest, Product>();


        //---Categories Mapping---
        CreateMap<Category, CategoryResponse>()
            .ForMember(d => d.IconUrl, options => options.MapFrom<IconUrlResolver>());
        CreateMap<CategoryAddRequest, Category>();
        CreateMap<CategoryUpdateRequest, Category>();

        //---Brands Mapping---
        CreateMap<Brand, BrandResponse>()
            .ForMember(d => d.LogoUrl, options => options.MapFrom<LogoUrlResolver>());
        CreateMap<BrandAddRequest, Brand>();
        CreateMap<BrandUpdateRequest, Brand>();

        //---ProductReviews Mapping---
        CreateMap<ProductReview, ProductReviewResponse>()
            .ForMember(d => d.CustomerName, options => options.MapFrom(s => s.Customer.Name));
        CreateMap<PagedResult<ProductReview>, PagedResult<ProductReviewResponse>>();
        CreateMap<ProductReviewAddRequest, ProductReview>();
        CreateMap<ProductReviewUpdateRequest, ProductReview>();

        //---Orders Mapping---
        CreateMap<Order, OrderForAdminListResponse>()
            .ForMember(d => d.CustomerName, options => options.MapFrom(s => s.Customer.Name))
            .ForMember(d => d.CustomerEmail, options => options.MapFrom(s => s.Customer.Email))
            .ForMember(d => d.City, options => options.MapFrom(s => s.ShippingAddress.City));
        CreateMap<PagedResult<Order>, PagedResult<OrderForAdminListResponse>>();
        CreateMap<OrderLine, OrderLineResponse>();
        CreateMap<Order, OrderResponse>()
            .ForMember(d => d.CustomerName, options => options.MapFrom(s => s.Customer.Name))
            .ForMember(d => d.CustomerEmail, options => options.MapFrom(s => s.Customer.Email));
        CreateMap<Order, OrderForCustomerListResponse>();
        CreateMap<PagedResult<Order>, PagedResult<OrderForCustomerListResponse>>();

        //---Coupons Mapping---
        CreateMap<Coupon, CouponResponse>();
        CreateMap<CouponAddRequest, Coupon>();
        CreateMap<CouponUpdateRequest, Coupon>();
    }
}



// ---Resolvers---
public class PreviewImageUrlResolver : IValueResolver<Product, ProductForListResponse, string?>
{
    private readonly IConfiguration configration;
    public PreviewImageUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(Product source, ProductForListResponse destination, string? destMember, ResolutionContext context)
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

public class ImagesUrlsResolver : IValueResolver<Product, ProductResponse, List<ProductImageResponse>?>
{
    private readonly IConfiguration configration;
    public ImagesUrlsResolver(IConfiguration configration) => this.configration = configration;

    public List<ProductImageResponse>? Resolve(Product source, ProductResponse destination, List<ProductImageResponse>? destMember, ResolutionContext context)
    {
        List<ProductImageResponse>? imagesDtos = new();

        source?.Images?.ForEach(image =>
        {
            var hostUrl = new Uri(configration["ResourcesStorage:HostUrl"]);
            var imageUrl = new Uri(hostUrl, image.Path).ToString();

            imagesDtos.Add(new ProductImageResponse() { Id = image.Id, Url = imageUrl });
        }
        );

        return imagesDtos.Count != 0 ? imagesDtos : null;
    }
}

public class IconUrlResolver : IValueResolver<Category, CategoryResponse, string?>
{
    private readonly IConfiguration configration;
    public IconUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(Category source, CategoryResponse destination, string? destMember, ResolutionContext context)
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

public class LogoUrlResolver : IValueResolver<Brand, BrandResponse, string?>
{
    private readonly IConfiguration configration;
    public LogoUrlResolver(IConfiguration configration) => this.configration = configration;

    public string? Resolve(Brand source, BrandResponse destination, string? destMember, ResolutionContext context)
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