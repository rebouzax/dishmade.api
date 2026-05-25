using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class RestaurantMapping : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurants");

        builder.HasKey(restaurant => restaurant.Id);

        builder.Property(restaurant => restaurant.Id)
            .ValueGeneratedNever();

        builder.Property(restaurant => restaurant.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(restaurant => restaurant.Document)
            .HasMaxLength(30);

        builder.Property(restaurant => restaurant.IsActive)
            .IsRequired();

        builder.Property(restaurant => restaurant.CreatedAt)
            .IsRequired();

        builder.Property(restaurant => restaurant.UpdatedAt);
    }
}