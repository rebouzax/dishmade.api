using dishmade.domain.Enums;

namespace dishmade.application.Features.Kitchen;

public sealed record KitchenOrderRealtimeResponse(
    Guid OrderId,
    Guid RestaurantId,
    Guid TableId,
    int TableNumber,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    decimal Total,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<KitchenOrderItemRealtimeResponse> Items
);

public sealed record KitchenOrderItemRealtimeResponse(
    Guid Id,
    Guid DishId,
    string DishName,
    int Quantity,
    decimal UnitPrice,
    decimal OptionsTotal,
    decimal Total,
    string? Notes,
    OrderItemStatus Status,
    IReadOnlyList<KitchenOrderItemOptionRealtimeResponse> Options
);

public sealed record KitchenOrderItemOptionRealtimeResponse(
    Guid DishOptionId,
    string Name,
    decimal AdditionalPrice
);