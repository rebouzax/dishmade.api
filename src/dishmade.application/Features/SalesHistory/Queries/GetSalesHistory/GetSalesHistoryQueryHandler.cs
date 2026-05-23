using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;

public sealed class GetSalesHistoryQueryHandler
    : IRequestHandler<GetSalesHistoryQuery, IReadOnlyList<SalesHistoryOrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetSalesHistoryQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<SalesHistoryOrderResponse>> Handle(
        GetSalesHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetDeliveredOrdersAsync(
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return orders
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
    }
}