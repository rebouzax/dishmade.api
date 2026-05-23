using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;

public sealed record GetSalesHistoryQuery(
    DateTime? StartDate,
    DateTime? EndDate,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<SalesHistoryOrderResponse>>;