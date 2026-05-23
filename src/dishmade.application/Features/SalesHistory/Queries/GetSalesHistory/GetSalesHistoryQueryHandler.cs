using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;

public sealed class GetSalesHistoryQueryHandler
    : IRequestHandler<GetSalesHistoryQuery, PagedResponse<SalesHistoryOrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetSalesHistoryQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResponse<SalesHistoryOrderResponse>> Handle(
        GetSalesHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _orderRepository.GetDeliveredOrdersPagedAsync(
            request.StartDate,
            request.EndDate,
            pageNumber,
            pageSize,
            cancellationToken);

        var sales = result.Items
            .Select(order => new SalesHistoryOrderResponse(
                order.Id,
                order.TableId,
                order.Table.Number,
                order.GetTotal(),
                order.DeliveredAt ?? order.UpdatedAt ?? order.CreatedAt,
                order.Items
                    .Select(item => new SalesHistoryItemResponse(
                        item.DishId,
                        item.Dish.Name,
                        item.Quantity,
                        item.UnitPrice,
                        item.GetTotal()))
                    .ToList()))
            .ToList();

        return new PagedResponse<SalesHistoryOrderResponse>(
            sales,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}