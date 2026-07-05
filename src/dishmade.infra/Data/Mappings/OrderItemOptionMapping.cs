using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class OrderItemOptionMapping : IEntityTypeConfiguration<OrderItemOption>
{
    public void Configure(EntityTypeBuilder<OrderItemOption> builder)
    {
        builder.ToTable("OrderItemOptions");

        builder.HasKey(option => option.Id);

        builder.Property(option => option.Id)
            .ValueGeneratedNever();

        builder.Property(option => option.OrderItemId)
            .IsRequired();

        builder.HasIndex(option => option.OrderItemId);

        builder.Property(option => option.DishOptionId)
            .IsRequired();

        builder.Property(option => option.OptionName)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(option => option.AdditionalPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(option => option.CreatedAt)
            .IsRequired();

        builder.Property(option => option.UpdatedAt);

        builder.HasOne(option => option.OrderItem)
            .WithMany(item => item.Options)
            .HasForeignKey(option => option.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(option => option.DishOption)
            .WithMany()
            .HasForeignKey(option => option.DishOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}