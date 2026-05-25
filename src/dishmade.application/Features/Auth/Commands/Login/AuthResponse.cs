namespace dishmade.application.Features.Auth.Commands.Login;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAt,
    UserAuthResponse User
);

public sealed record UserAuthResponse(
    Guid Id,
    string Name,
    string Email,
    string Role,
    Guid? RestaurantId,
    string? RestaurantName
);