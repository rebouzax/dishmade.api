namespace dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;

public sealed record SalesHistoryOrderResponse(
    Guid OrderId,
    Guid TableId,
    int TableNumber,
    decimal Total,
    DateTime SaleDate,
    IReadOnlyList<SalesHistoryItemResponse> Items
);