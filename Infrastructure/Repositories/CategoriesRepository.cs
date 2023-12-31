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

    public async Task<ProductCategory?> GetCategory(int categoryId)
    {
        var category = await appDbContext.ProductCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == categoryId);

        return category;
    }

}
