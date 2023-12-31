using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.SpecificationDTOs;
using Core.Entities.ProductAggregate;
using Microsoft.Extensions.Configuration;

namespace Core.DomainServices;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductForListDto>()
        .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
        .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
        .ForMember(d => d.InStock, options => options.MapFrom(s => s.Quantity > 0))
        .ForMember(d => d.ImageUrl, options => options.MapFrom<PreviewImageUrlResolver>());

        CreateMap<PagedResult<Product>, PagedResult<ProductForListDto>>();

        CreateMap<Product, ProductDetailsDto>()
        .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
        .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
        .ForMember(d => d.InStock, options => options.MapFrom(s => s.Quantity > 0))
        .ForMember(d => d.ImageUrls, options => options.MapFrom<ImageUrlsResolver>());

    }
}


// ---Resolvers---
public class PreviewImageUrlResolver : IValueResolver<Product, ProductForListDto, string>
{
    private readonly IConfiguration configration;

    public PreviewImageUrlResolver(IConfiguration configration) => this.configration = configration;

    public string Resolve(Product source, ProductForListDto destination, string destMember, ResolutionContext context)
    {
        var previewImagePath = source.ImageUrls.ElementAt(0);
        if (!string.IsNullOrEmpty(previewImagePath))
            return $"{configration["ApiUrl"]}/{previewImagePath}";

        return null;
    }
}

public class ImageUrlsResolver : IValueResolver<Product, ProductDetailsDto, List<string>>
{
    private readonly IConfiguration configration;

    public ImageUrlsResolver(IConfiguration configration) => this.configration = configration;

    public List<string> Resolve(Product source, ProductDetailsDto destination, List<string> destMember, ResolutionContext context)
    {
        List<string> imageUrls = new();

        foreach (var imageUrl in source.ImageUrls)
            if (!string.IsNullOrEmpty(imageUrl))
                imageUrls.Add($"{configration["ApiUrl"]}/{imageUrl}");

        return imageUrls;
    }
}