using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class RestaurantTableMapping : IEntityTypeConfiguration<RestaurantTable>
{
    public void Configure(EntityTypeBuilder<RestaurantTable> builder)
    {
        builder.ToTable("RestaurantTables");

        builder.HasKey(table => table.Id);

        builder.Property(table => table.Id)
            .ValueGeneratedNever();

        builder.Property(table => table.Number)
            .IsRequired();

        builder.Property(table => table.RestaurantId)
            .IsRequired();

        builder.HasIndex(table => new { table.RestaurantId, table.Number })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.Property(table => table.IsOccupied)
            .IsRequired();

        builder.Property(table => table.IsDeleted)
            .IsRequired();

        builder.Property(table => table.CreatedAt)
            .IsRequired();

        builder.Property(table => table.UpdatedAt);

        builder.HasQueryFilter(table => !table.IsDeleted);
    }
}