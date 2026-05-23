using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using dishmade.application.Features.Orders.Queries;
using MediatR;

namespace dishmade.application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, PagedResponse<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResponse<OrderResponse>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _orderRepository.GetPagedAsync(
            request.Status,
            request.TableId,
            request.StartDate,
            request.EndDate,
            pageNumber,
            pageSize,
            cancellationToken);

        var orders = result.Items
            .Select(OrderMapper.ToResponse)
            .ToList();

        return new PagedResponse<OrderResponse>(
            orders,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}