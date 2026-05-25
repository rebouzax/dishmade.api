namespace dishmade.application.Features.Admin.Clients.Queries.GetClients;

public sealed record ClientResponse(
    Guid UserId,
    string UserName,
    string Email,
    bool IsActive,
    Guid? RestaurantId,
    string? RestaurantName,
    string? RestaurantDocument,
    bool? RestaurantIsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);