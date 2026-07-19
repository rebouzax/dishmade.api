using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.RestaurantSettings.Commands.UpdateRestaurantSettings;

public sealed class UpdateRestaurantSettingsCommandHandler
    : IRequestHandler<UpdateRestaurantSettingsCommand, RestaurantSettingsResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRestaurantSettingsCommandHandler(
        ICurrentUserService currentUserService,
        IRestaurantRepository restaurantRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _restaurantRepository = restaurantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RestaurantSettingsResponse> Handle(
        UpdateRestaurantSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var restaurant = await _restaurantRepository.GetByIdAsync(
            restaurantId,
            cancellationToken);

        if (restaurant is null)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        restaurant.UpdateOperationalSettings(
            request.DefaultServiceFeePercentage,
            request.AcceptsQrCodeOrders,
            request.AcceptsWaiterCall);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RestaurantSettingsMapper.ToResponse(restaurant);
    }
}