using dishmade.application.Abstractions.Auth;
using dishmade.domain.Constants;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dishmade.infra.Data.Seed;

public static class AdminUserSeeder
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DishmadeDbContext>();
        var passwordHashService = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var adminEmail = configuration["AdminUser:Email"]?.Trim().ToLowerInvariant();
        var adminName = configuration["AdminUser:Name"]?.Trim();
        var adminPassword = configuration["AdminUser:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminName) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var admin = await context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(user => user.Email == adminEmail);

        if (admin is null)
        {
            admin = AppUser.CreatePlatformAdmin(adminName, adminEmail);

            var passwordHash = passwordHashService.HashPassword(admin, adminPassword);

            admin.SetPasswordHash(passwordHash);

            await context.Users.AddAsync(admin);
        }
        else
        {
            if (admin.Role != Roles.PlatformAdmin)
                throw new InvalidOperationException("O e-mail configurado para ADM já pertence a um usuário que não é ADM.");

            var passwordHash = passwordHashService.HashPassword(admin, adminPassword);

            admin.SetPasswordHash(passwordHash);

            if (!admin.IsActive)
                admin.Activate();
        }

        await context.SaveChangesAsync();
    }
}