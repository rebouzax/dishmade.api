namespace dishmade.application.Features.Dashboard.Queries.GetRestaurantDashboard;

public sealed record RestaurantDashboardResponse(
    decimal TotalRevenue,
    int TotalOrders,
    int TotalItemsSold,
    decimal AverageTicket,
    IReadOnlyList<TopDishResponse> TopDishes
);