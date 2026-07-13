using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class OrderPaymentMapping : IEntityTypeConfiguration<OrderPayment>
{
    public void Configure(EntityTypeBuilder<OrderPayment> builder)
    {
        builder.ToTable("OrderPayments");

        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Id)
            .ValueGeneratedNever();

        builder.Property(payment => payment.RestaurantId)
            .IsRequired();

        builder.HasIndex(payment => payment.RestaurantId);

        builder.Property(payment => payment.OrderId)
            .IsRequired();

        builder.HasIndex(payment => payment.OrderId);

        builder.Property(payment => payment.Method)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(payment => payment.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(payment => payment.Amount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(payment => payment.Notes)
            .HasMaxLength(500);

        builder.Property(payment => payment.CreatedAt)
            .IsRequired();

        builder.Property(payment => payment.UpdatedAt);

        builder.HasOne(payment => payment.Order)
            .WithMany(order => order.Payments)
            .HasForeignKey(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}