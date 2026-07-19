using dishmade.domain.Entities;

namespace dishmade.application.Features.RestaurantSettings;

public static class RestaurantSettingsMapper
{
    public static RestaurantSettingsResponse ToResponse(Restaurant restaurant)
    {
        return new RestaurantSettingsResponse(
            restaurant.Id,
            restaurant.Name,
            restaurant.Slug,
            restaurant.DefaultServiceFeePercentage,
            restaurant.AcceptsQrCodeOrders,
            restaurant.AcceptsWaiterCall,
            restaurant.CreatedAt,
            restaurant.UpdatedAt);
    }
}