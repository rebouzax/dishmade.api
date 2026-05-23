using dishmade.domain.Enums;

namespace dishmade.application.Features.Orders.Queries;

public sealed record OrderResponse(
    Guid Id,
    Guid TableId,
    int TableNumber,
    OrderStatus Status,
    decimal Total,
    IReadOnlyList<OrderItemResponse> Items,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);