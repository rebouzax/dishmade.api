namespace dishmade.application.Features.Orders.Queries;

public sealed record OrderItemResponse(
    Guid Id,
    Guid DishId,
    string DishName,
    int Quantity,
    decimal UnitPrice,
    decimal Total,
    string? Notes
);