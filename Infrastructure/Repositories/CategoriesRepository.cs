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

    public async Task<IReadOnlyCollection<Category>> GetCategories()
    {
        var categories = await appDbContext.Categories
                            .AsNoTracking()
                            .ToListAsync();

        return categories;
    }

    public async Task<Category?> GetCategory(Guid id)
    {
        var category = await appDbContext.Categories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);

        return category;
    }

    public void AddCategory(Category category)
    {
        appDbContext.Categories.Add(category);
    }

    public void UpdateCategory(Category category)
    {
        appDbContext.Categories.Update(category);
    }

    public void DeleteCategory(Category category)
    {
        appDbContext.Categories.Remove(category);
    }
}
