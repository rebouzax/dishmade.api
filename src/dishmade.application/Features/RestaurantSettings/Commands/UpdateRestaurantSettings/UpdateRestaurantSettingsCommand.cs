using MediatR;

namespace dishmade.application.Features.RestaurantSettings.Commands.UpdateRestaurantSettings;

public sealed record UpdateRestaurantSettingsCommand(
    decimal DefaultServiceFeePercentage,
    bool AcceptsQrCodeOrders,
    bool AcceptsWaiterCall
) : IRequest<RestaurantSettingsResponse>;