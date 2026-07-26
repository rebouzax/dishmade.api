using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.Property(item => item.OrderId)
            .IsRequired();

        builder.Property(item => item.DishId)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(item => item.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(item => item.Notes)
            .HasMaxLength(500);

        builder.Property(item => item.CreatedAt)
            .IsRequired();

        builder.Property(item => item.UpdatedAt);

        builder.HasOne(item => item.Order)
            .WithMany(order => order.Items)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Dish)
            .WithMany()
            .HasForeignKey(item => item.DishId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}