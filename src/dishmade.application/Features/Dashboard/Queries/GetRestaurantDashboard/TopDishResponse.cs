namespace dishmade.application.Features.Dashboard.Queries.GetRestaurantDashboard;

public sealed record TopDishResponse(
    Guid DishId,
    string DishName,
    int QuantitySold,
    decimal Revenue
);