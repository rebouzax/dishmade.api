using System.Security.Claims;
using dishmade.application.Abstractions.Auth;

namespace dishmade.api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var userId)
                ? userId
                : Guid.Empty;
        }
    }

    public Guid? RestaurantId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirstValue("restaurant_id");

            return Guid.TryParse(value, out var restaurantId)
                ? restaurantId
                : null;
        }
    }

    public string Role =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

    public Guid GetRequiredRestaurantId()
    {
        return RestaurantId
            ?? throw new UnauthorizedAccessException("Usuário não possui restaurante vinculado.");
    }
}