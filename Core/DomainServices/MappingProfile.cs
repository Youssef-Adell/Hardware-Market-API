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
        .ForMember(d => d.Images, options => options.MapFrom<ImagesUrlsResolver>());

        CreateMap<ProductForAddingDto, Product>();
        CreateMap<ProductForUpdatingDto, Product>();
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
            var hostUrl = new Uri(configration["ApiUrl"]);
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
            var hostUrl = new Uri(configration["ApiUrl"]);
            var imageUrl = new Uri(hostUrl, image.Path).ToString();

            imagesDtos.Add(new ProductImageDto() { Id = image.Id, Url = imageUrl });
        }
        );

        return imagesDtos.Count != 0 ? imagesDtos : null;
    }
}