using dishmade.domain.Enums;
using dishmade.application.Features.Orders.Payments;

namespace dishmade.application.Features.Orders.Receipts;

public sealed record OrderReceiptResponse(
    Guid OrderId,
    Guid RestaurantId,
    Guid TableId,
    int TableNumber,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal ServiceFeeAmount,
    decimal FinalTotal,
    decimal PaidAmount,
    decimal RemainingAmount,
    DateTime? ClosedAt,
    DateTime? PaidAt,
    DateTime CreatedAt,
    IReadOnlyList<OrderReceiptItemResponse> Items,
    IReadOnlyList<OrderPaymentResponse> Payments
);

public sealed record OrderReceiptItemResponse(
    Guid Id,
    Guid DishId,
    string DishName,
    int Quantity,
    decimal UnitPrice,
    decimal OptionsTotal,
    decimal Total,
    string? Notes,
    IReadOnlyList<OrderReceiptItemOptionResponse> Options
);

public sealed record OrderReceiptItemOptionResponse(
    Guid DishOptionId,
    string Name,
    decimal AdditionalPrice
);