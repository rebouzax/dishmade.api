using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class DishOptionGroupMapping : IEntityTypeConfiguration<DishOptionGroup>
{
    public void Configure(EntityTypeBuilder<DishOptionGroup> builder)
    {
        builder.ToTable("DishOptionGroups");

        builder.HasKey(group => group.Id);

        builder.Property(group => group.Id)
            .ValueGeneratedNever();

        builder.Property(group => group.RestaurantId)
            .IsRequired();

        builder.HasIndex(group => group.RestaurantId);

        builder.Property(group => group.DishId)
            .IsRequired();

        builder.HasIndex(group => group.DishId);

        builder.Property(group => group.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(group => group.IsRequired)
            .IsRequired();

        builder.Property(group => group.MinSelection)
            .IsRequired();

        builder.Property(group => group.MaxSelection)
            .IsRequired();

        builder.Property(group => group.IsActive)
            .IsRequired();

        builder.Property(group => group.IsDeleted)
            .IsRequired();

        builder.Property(group => group.CreatedAt)
            .IsRequired();

        builder.Property(group => group.UpdatedAt);

        builder.HasOne(group => group.Dish)
            .WithMany()
            .HasForeignKey(group => group.DishId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}