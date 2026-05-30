using dishmade.application.Abstractions.Auth;
using dishmade.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Data.Context;

public sealed class DishmadeDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public DishmadeDbContext(
        DbContextOptions<DishmadeDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishImage> DishImages => Set<DishImage>();
    public DbSet<RestaurantTable> RestaurantTables => Set<RestaurantTable>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DishmadeDbContext).Assembly);

        ApplyTenantFilters(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ApplyTenantFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasQueryFilter(category =>
                _currentUserService.RestaurantId.HasValue &&
                category.RestaurantId == _currentUserService.RestaurantId.Value);

        modelBuilder.Entity<Dish>()
            .HasQueryFilter(dish =>
                !dish.IsDeleted &&
                _currentUserService.RestaurantId.HasValue &&
                dish.RestaurantId == _currentUserService.RestaurantId.Value);
        
        modelBuilder.Entity<DishImage>()
        .HasQueryFilter(image =>
            _currentUserService.RestaurantId.HasValue &&
            image.RestaurantId == _currentUserService.RestaurantId.Value);


        modelBuilder.Entity<RestaurantTable>()
            .HasQueryFilter(table =>
                !table.IsDeleted &&
                _currentUserService.RestaurantId.HasValue &&
                table.RestaurantId == _currentUserService.RestaurantId.Value);

        modelBuilder.Entity<Order>()
            .HasQueryFilter(order =>
                _currentUserService.RestaurantId.HasValue &&
                order.RestaurantId == _currentUserService.RestaurantId.Value);
    }
}