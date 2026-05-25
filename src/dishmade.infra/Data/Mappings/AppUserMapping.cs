using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class AppUserMapping : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .ValueGeneratedNever();

        builder.Property(user => user.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .IsRequired();

        builder.Property(user => user.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.RestaurantId);

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt);

        builder.HasOne(user => user.Restaurant)
            .WithMany(restaurant => restaurant.Users)
            .HasForeignKey(user => user.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}