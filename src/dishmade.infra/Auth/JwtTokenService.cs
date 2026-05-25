using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dishmade.application.Abstractions.Auth;
using dishmade.domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dishmade.infra.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(AppUser user)
    {
        var secretKey = _configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey não configurado.");

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience não configurado.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        if (user.RestaurantId.HasValue)
        {
            claims.Add(new Claim("restaurant_id", user.RestaurantId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: GetExpirationDate(),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetExpirationDate()
    {
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes");

        if (expirationMinutes <= 0)
            expirationMinutes = 120;

        return DateTime.UtcNow.AddMinutes(expirationMinutes);
    }
}