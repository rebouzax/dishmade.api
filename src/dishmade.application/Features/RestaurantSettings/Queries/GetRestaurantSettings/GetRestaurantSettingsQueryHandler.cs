using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.RestaurantSettings.Queries.GetRestaurantSettings;

public sealed class GetRestaurantSettingsQueryHandler
    : IRequestHandler<GetRestaurantSettingsQuery, RestaurantSettingsResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRestaurantRepository _restaurantRepository;

    public GetRestaurantSettingsQueryHandler(
        ICurrentUserService currentUserService,
        IRestaurantRepository restaurantRepository)
    {
        _currentUserService = currentUserService;
        _restaurantRepository = restaurantRepository;
    }

    public async Task<RestaurantSettingsResponse> Handle(
        GetRestaurantSettingsQuery request,
        CancellationToken cancellationToken)
    {
        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var restaurant = await _restaurantRepository.GetByIdAsync(
            restaurantId,
            cancellationToken);

        if (restaurant is null)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        return RestaurantSettingsMapper.ToResponse(restaurant);
    }
}