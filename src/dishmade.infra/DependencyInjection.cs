using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.infra.Auth;
using dishmade.infra.Data.Context;
using dishmade.infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dishmade.infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DishmadeDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();

        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IRestaurantTableRepository, RestaurantTableRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}