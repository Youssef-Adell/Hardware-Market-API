using Core.DTOs.BrandDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IBrandsService
{
    Task<IReadOnlyCollection<BrandResponse>> GetBrands();
    Task<BrandResponse> GetBrand(Guid id);
    Task<Guid> AddBrand(BrandAddRequest brandAddRequest, byte[] BrandLogo);
    Task UpdateBrand(Guid id, BrandUpdateRequest brandUpdateRequest, byte[]? newBrandLogo);
    Task DeleteBrand(Guid id);
}
