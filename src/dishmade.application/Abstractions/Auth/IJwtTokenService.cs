using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Auth;

public interface IJwtTokenService
{
    string GenerateToken(AppUser user);

    DateTime GetExpirationDate();
}