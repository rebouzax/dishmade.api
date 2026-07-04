using dishmade.domain.Enums;

namespace dishmade.application.Features.Public.Orders;

public sealed record PublicOrderResponse(
    Guid OrderId,
    string AccessCode,
    Guid RestaurantId,
    string RestaurantName,
    Guid TableId,
    int TableNumber,
    OrderStatus Status,
    decimal Total,
    IReadOnlyList<PublicOrderItemResponse> Items,
    DateTime CreatedAt
);

public sealed record PublicOrderItemResponse(
    Guid Id,
    Guid DishId,
    string DishName,
    int Quantity,
    decimal UnitPrice,
    decimal Total,
    string? Notes
);