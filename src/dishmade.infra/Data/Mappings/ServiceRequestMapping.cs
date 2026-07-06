using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dishmade.infra.Data.Mappings;

public sealed class ServiceRequestMapping : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("ServiceRequests");

        builder.HasKey(request => request.Id);

        builder.Property(request => request.Id)
            .ValueGeneratedNever();

        builder.Property(request => request.RestaurantId)
            .IsRequired();

        builder.HasIndex(request => request.RestaurantId);

        builder.Property(request => request.TableId)
            .IsRequired();

        builder.HasIndex(request => request.TableId);

        builder.Property(request => request.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(request => request.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasIndex(request => request.Status);

        builder.Property(request => request.Message)
            .HasMaxLength(500);

        builder.Property(request => request.StartedAt);
        builder.Property(request => request.ResolvedAt);
        builder.Property(request => request.CanceledAt);

        builder.Property(request => request.CreatedAt)
            .IsRequired();

        builder.Property(request => request.UpdatedAt);

        builder.HasOne(request => request.Table)
            .WithMany()
            .HasForeignKey(request => request.TableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}