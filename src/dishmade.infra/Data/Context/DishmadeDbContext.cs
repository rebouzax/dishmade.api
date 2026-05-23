using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Data.Context;

public sealed class DishmadeDbContext : DbContext
{
    public DishmadeDbContext(DbContextOptions<DishmadeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<RestaurantTable> RestaurantTables => Set<RestaurantTable>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DishmadeDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}