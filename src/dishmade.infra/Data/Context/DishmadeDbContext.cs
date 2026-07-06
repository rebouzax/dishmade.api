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
    public DbSet<DishOptionGroup> DishOptionGroups => Set<DishOptionGroup>();
    public DbSet<DishOption> DishOptions => Set<DishOption>();
    public DbSet<OrderItemOption> OrderItemOptions => Set<OrderItemOption>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

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

        modelBuilder.Entity<DishOptionGroup>()
            .HasQueryFilter(group =>
                !group.IsDeleted &&
                _currentUserService.RestaurantId.HasValue &&
                group.RestaurantId == _currentUserService.RestaurantId.Value);

        modelBuilder.Entity<DishOption>()
            .HasQueryFilter(option =>
               !option.IsDeleted &&
               _currentUserService.RestaurantId.HasValue &&
               option.RestaurantId == _currentUserService.RestaurantId.Value);
        modelBuilder.Entity<ServiceRequest>()
            .HasQueryFilter(request =>
               _currentUserService.RestaurantId.HasValue &&
               request.RestaurantId == _currentUserService.RestaurantId.Value);
    }
}