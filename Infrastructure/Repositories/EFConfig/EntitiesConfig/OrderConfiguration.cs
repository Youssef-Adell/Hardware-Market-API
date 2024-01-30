using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.EFConfig.EntitiesConfig;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.WithOwner();
            address.Property(a => a.AddressLine).HasColumnName("AddressLine");
            address.Property(a => a.City).HasColumnName("City");
        });

        builder.Property(o => o.Status).HasConversion(
            statusObj => statusObj.ToString(), //delegate to convert from OrderStatus enum to column value (string)
            statusColumn => Enum.Parse<OrderStatus>(statusColumn) //delegate to convert from column value (string) to OrderStatus enum
        );

        builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade).IsRequired();
    }

}
