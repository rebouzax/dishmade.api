using dishmade.application.Common.Pagination;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(
    OrderStatus? Status,
    Guid? TableId,
    DateTime? StartDate,
    DateTime? EndDate,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<OrderResponse>>;