using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class DishOptionMapping : IEntityTypeConfiguration<DishOption>
{
    public void Configure(EntityTypeBuilder<DishOption> builder)
    {
        builder.ToTable("DishOptions");

        builder.HasKey(option => option.Id);

        builder.Property(option => option.Id)
            .ValueGeneratedNever();

        builder.Property(option => option.RestaurantId)
            .IsRequired();

        builder.HasIndex(option => option.RestaurantId);

        builder.Property(option => option.OptionGroupId)
            .IsRequired();

        builder.HasIndex(option => option.OptionGroupId);

        builder.Property(option => option.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(option => option.AdditionalPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(option => option.IsAvailable)
            .IsRequired();

        builder.Property(option => option.IsDeleted)
            .IsRequired();

        builder.Property(option => option.CreatedAt)
            .IsRequired();

        builder.Property(option => option.UpdatedAt);

        builder.HasOne(option => option.OptionGroup)
            .WithMany(group => group.Options)
            .HasForeignKey(option => option.OptionGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}