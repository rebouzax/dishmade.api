using dishmade.domain.Enums;

namespace dishmade.application.Features.Orders.Queries;

public sealed record OrderItemResponse(
    Guid Id,
    Guid DishId,
    string DishName,
    int Quantity,
    decimal UnitPrice,
    decimal OptionsTotal,
    decimal Total,
    string? Notes,
    OrderItemStatus Status,
    IReadOnlyList<OrderItemOptionResponse> Options
);
