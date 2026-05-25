using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Auth;

public interface IPasswordHashService
{
    string HashPassword(AppUser user, string password);

    bool VerifyPassword(AppUser user, string password);
}