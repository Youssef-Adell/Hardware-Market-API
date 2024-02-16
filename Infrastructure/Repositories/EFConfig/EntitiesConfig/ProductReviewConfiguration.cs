using Core.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.EFConfig.EntitiesConfig;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        //if the customer deleted his reviews wont be deleted and the customerId wil be assigned to null
        //note that you may run into NullReferenceException issues because this so try to make your code protective (to do later)
        builder.HasOne(r => r.Customer).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
    }
}
