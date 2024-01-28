using Core.DTOs.CategoryDTOs;
using Core.Entities.ProductAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly AppDbContext appDbContext;

    public CategoriesRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<IReadOnlyCollection<ProductCategory>> GetCategories()
    {
        var categories = await appDbContext.ProductCategories
                            .AsNoTracking()
                            .ToListAsync();

        return categories;
    }

    public async Task<ProductCategory?> GetCategory(int id)
    {
        var category = await appDbContext.ProductCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);

        return category;
    }

    public void AddCategory(ProductCategory category)
    {
        appDbContext.ProductCategories.Add(category);
    }

    public void UpdateCategory(ProductCategory category)
    {
        appDbContext.ProductCategories.Update(category);
    }

    public void DeleteCategory(ProductCategory category)
    {
        appDbContext.ProductCategories.Remove(category);
    }
}
