using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class DishImageMapping : IEntityTypeConfiguration<DishImage>
{
    public void Configure(EntityTypeBuilder<DishImage> builder)
    {
        builder.ToTable("DishImages");

        builder.HasKey(image => image.Id);

        builder.Property(image => image.Id)
            .ValueGeneratedNever();

        builder.Property(image => image.RestaurantId)
            .IsRequired();

        builder.HasIndex(image => image.RestaurantId);

        builder.Property(image => image.DishId)
            .IsRequired();

        builder.HasIndex(image => image.DishId)
            .IsUnique();

        builder.Property(image => image.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(image => image.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(image => image.SizeInBytes)
            .IsRequired();

        builder.Property(image => image.Data)
            .IsRequired()
            .HasColumnType("varbinary(max)");

        builder.Property(image => image.CreatedAt)
            .IsRequired();

        builder.Property(image => image.UpdatedAt);

        builder.HasOne(image => image.Dish)
            .WithOne()
            .HasForeignKey<DishImage>(image => image.DishId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}