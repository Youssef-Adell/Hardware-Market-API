using Core.DTOs.BrandDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IBrandsService
{
    Task<IReadOnlyCollection<BrandDto>> GetBrands();
    Task<BrandDto> GetBrand(int id);
    Task<int> AddBrand(BrandForAddingDto BrandToAdd, byte[] BrandLogo);
    Task UpdateBrand(int BrandId, BrandForUpdatingDto updatedBrand, byte[]? newBrandLogo);
    Task DeleteBrand(int id);
}
