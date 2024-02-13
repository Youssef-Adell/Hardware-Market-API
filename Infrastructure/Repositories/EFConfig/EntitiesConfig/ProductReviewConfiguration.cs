// using Core.Entities.ProductAggregate;
// using Microsoft.EntityFrameworkCore;

// namespace Infrastructure.Repositories.EFConfig.EntitiesConfig;

// public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
// {
//     public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductReview> builder)
//     {
//         // to solve the problem of returning int from the function that compute AverageRating column at the DB and make it returns float 
//         builder.Property(r => r.Rating).HasColumnType("real");
//     }

// }
