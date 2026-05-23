using MediatR;

namespace dishmade.application.Features.Dashboard.Queries.GetRestaurantDashboard;

public sealed record GetRestaurantDashboardQuery(
    DateTime? StartDate,
    DateTime? EndDate
) : IRequest<RestaurantDashboardResponse>;