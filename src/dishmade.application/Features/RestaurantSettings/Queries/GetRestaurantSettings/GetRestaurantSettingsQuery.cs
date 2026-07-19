using MediatR;

namespace dishmade.application.Features.RestaurantSettings.Queries.GetRestaurantSettings;

public sealed record GetRestaurantSettingsQuery
    : IRequest<RestaurantSettingsResponse>;