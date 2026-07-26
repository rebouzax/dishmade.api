using dishmade.domain.Enums;

namespace dishmade.application.Features.Orders.Queries;

public sealed record OrderResponse(
    Guid Id,
    Guid TableId,
    int TableNumber,
    OrderStatus Status,
    decimal Total,
    PaymentStatus PaymentStatus,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal ServiceFeeAmount,
    decimal FinalTotal,
    decimal PaidAmount,
    decimal RemainingAmount,
    DateTime? ClosedAt,
    DateTime? PaidAt,
    IReadOnlyList<OrderItemResponse> Items,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeliveredAt
);

public sealed record OrderItemOptionResponse(
    Guid DishOptionId,
    string Name,
    decimal AdditionalPrice
);