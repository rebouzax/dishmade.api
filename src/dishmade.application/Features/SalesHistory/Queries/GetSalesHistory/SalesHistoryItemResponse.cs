namespace dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;

public sealed record SalesHistoryItemResponse(
    Guid DishId,
    string DishName,
    int Quantity,
    decimal UnitPrice,
    decimal Total
);