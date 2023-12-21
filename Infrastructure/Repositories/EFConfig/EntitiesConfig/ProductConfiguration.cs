using Core.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.EFConfig.EntitiesConfig;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // we solved the problem of calculated AverageRating by delegating the task to the DB but the issue is our app now is dependent on details at Db level so if we changed our DB we have to implement dbo.CalculateProductRate function in the new DB
        builder.Property(p => p.AverageRating).HasComputedColumnSql("dbo.CalculateProductRate([Id])");
    }

}
