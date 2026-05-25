namespace dishmade.application.Abstractions.Auth;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid? RestaurantId { get; }
    string Role { get; }
    bool IsAuthenticated { get; }

    Guid GetRequiredRestaurantId();
}