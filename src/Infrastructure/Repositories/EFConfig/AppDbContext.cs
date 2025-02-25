using System.Reflection;
using Core.Entities.OrderAggregate;
using Core.Entities.ProductAggregate;
using Core.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFConfig;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
