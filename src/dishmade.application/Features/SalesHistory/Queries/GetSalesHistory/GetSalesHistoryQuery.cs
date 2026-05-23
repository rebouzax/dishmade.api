using MediatR;

namespace dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;

public sealed record GetSalesHistoryQuery(
    DateTime? StartDate,
    DateTime? EndDate
) : IRequest<IReadOnlyList<SalesHistoryOrderResponse>>;