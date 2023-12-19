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
        .ForMember(d=>d.Brand, options=>options.MapFrom(s=>s.Brand.Name))
        .ForMember(d=>d.Category, options=>options.MapFrom(s=>s.Category.Name))
        .ForMember(d=>d.InStock, options=>options.MapFrom(s=>s.Quantity>0))
        .ForMember(d=>d.ImageUrl, options=>options.MapFrom<ImageUrlResolver<ProductForListDto>>());

        CreateMap<PagedResult<Product>, PagedResult<ProductForListDto>>();
    }
}


// ---Resolvers---
public class ImageUrlResolver<TDestination> : IValueResolver<Product, TDestination, string>
{
    private readonly IConfiguration configration;

    public ImageUrlResolver(IConfiguration configration)=>this.configration = configration;

    public string Resolve(Product source, TDestination destination, string destMember, ResolutionContext context)
    {
        var previewImagePath = source.ImageUrls.ElementAt(0);
        if(!string.IsNullOrEmpty(previewImagePath))
            return $"{configration["ApiUrl"]}/{previewImagePath}";
        
        return null;
    }
}