using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dashboard.Queries.GetRestaurantDashboard;

public sealed class GetRestaurantDashboardQueryHandler
    : IRequestHandler<GetRestaurantDashboardQuery, RestaurantDashboardResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetRestaurantDashboardQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<RestaurantDashboardResponse> Handle(
        GetRestaurantDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetDeliveredOrdersAsync(
            request.StartDate,
            request.EndDate,
            cancellationToken);

        var totalOrders = orders.Count;

        var totalRevenue = orders.Sum(order => order.GetTotal());

        var totalItemsSold = orders
            .SelectMany(order => order.Items)
            .Sum(item => item.Quantity);

        var averageTicket = totalOrders == 0
            ? 0
            : totalRevenue / totalOrders;

        var topDishes = orders
            .SelectMany(order => order.Items)
            .GroupBy(item => new
            {
                item.DishId,
                item.Dish.Name
            })
            .Select(group => new TopDishResponse(
                group.Key.DishId,
                group.Key.Name,
                group.Sum(item => item.Quantity),
                group.Sum(item => item.GetTotal())))
            .OrderByDescending(dish => dish.QuantitySold)
            .ThenByDescending(dish => dish.Revenue)
            .Take(5)
            .ToList();

        return new RestaurantDashboardResponse(
            totalRevenue,
            totalOrders,
            totalItemsSold,
            averageTicket,
            topDishes);
    }
}