using Core.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.EFConfig.EntitiesConfig;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.AverageRating)
        // we solved the problem of calculated AverageRating by delegating the task to the DB but the issue is our app now is dependent on details at Db level so if we changed our DB we have to implement dbo.CalculateProductRate function in the new DB

        .HasComputedColumnSql("dbo.CalculateProductRate([Id])")
        //to stop returning this property in the OUTPUT Clause when inserting a new entity in the DB beacuse if we didnt it will rhrow an error while saving because this column is computedColumn and its value cannot be calculated and returned immediataly after saving unlike Id property
        .ValueGeneratedNever()
        //to ignore the value of AverageRating property and not save it while saving a new entity to the DB because if we diidnt it will throw an error while saving because this column is computedColumn and cannot be modified or updated explicitly
        .Metadata.SetBeforeSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        //to ignore the value of AverageRating property and not update it while updating the entity in the DB
        builder.Property(p => p.AverageRating)
        .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
    }

}
