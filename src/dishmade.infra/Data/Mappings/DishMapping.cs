using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class DishMapping : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.ToTable("Dishes");

        builder.HasKey(dish => dish.Id);

        builder.Property(dish => dish.Id)
            .ValueGeneratedNever();

        builder.Property(dish => dish.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(dish => dish.Description)
            .HasMaxLength(1000);

        builder.Property(dish => dish.Price)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(dish => dish.IsAvailable)
            .IsRequired();

        builder.Property(dish => dish.IsDeleted)
            .IsRequired();

        builder.Property(dish => dish.CategoryId)
            .IsRequired();

        builder.Property(dish => dish.RestaurantId)
            .IsRequired();

        builder.HasIndex(dish => dish.RestaurantId);

        builder.Property(dish => dish.CreatedAt)
            .IsRequired();

        builder.Property(dish => dish.UpdatedAt);

        builder.HasQueryFilter(dish => !dish.IsDeleted);
    }
}