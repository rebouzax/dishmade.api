using dishmade.domain.Enums;

namespace dishmade.application.Features.ServiceRequests;

public sealed record ServiceRequestResponse(
    Guid Id,
    Guid RestaurantId,
    Guid TableId,
    int TableNumber,
    ServiceRequestType Type,
    ServiceRequestStatus Status,
    string? Message,
    DateTime CreatedAt,
    DateTime? StartedAt,
    DateTime? ResolvedAt,
    DateTime? CanceledAt,
    DateTime? UpdatedAt
);