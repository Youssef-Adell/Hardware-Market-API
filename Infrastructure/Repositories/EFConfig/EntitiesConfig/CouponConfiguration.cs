using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.EFConfig.EntitiesConfig;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.Property(o => o.Type).HasConversion(
            typeObj => typeObj.ToString(), //delegate to convert from CouponType enum to column value (string)
            typeColumn => Enum.Parse<DiscountType>(typeColumn) //delegate to convert from column value (string) to DiscountType enum
        );
    }

}
