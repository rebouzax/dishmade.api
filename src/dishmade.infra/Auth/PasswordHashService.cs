using dishmade.application.Abstractions.Auth;
using dishmade.domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace dishmade.infra.Auth;

public sealed class PasswordHashService : IPasswordHashService
{
    private readonly PasswordHasher<AppUser> _passwordHasher = new();

    public string HashPassword(AppUser user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(AppUser user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password);

        return result != PasswordVerificationResult.Failed;
    }
}