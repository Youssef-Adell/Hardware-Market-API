using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.Entities.ProductAggregate;

namespace Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductForListDto>()
        .ForMember(d=>d.ImageUrl, options=>options.MapFrom(s=>s.ImageUrls.ElementAt(0)))
        .ForMember(d=>d.Brand, options=>options.MapFrom(s=>s.Brand.Name))
        .ForMember(d=>d.Category, options=>options.MapFrom(s=>s.Category.Name))
        .ForMember(d=>d.InStock, options=>options.MapFrom(s=>s.Quantity>0));
    }
}
