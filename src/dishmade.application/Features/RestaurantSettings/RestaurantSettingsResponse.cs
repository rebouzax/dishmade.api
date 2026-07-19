namespace dishmade.application.Features.RestaurantSettings;

public sealed record RestaurantSettingsResponse(
    Guid RestaurantId,
    string RestaurantName,
    string RestaurantSlug,
    decimal DefaultServiceFeePercentage,
    bool AcceptsQrCodeOrders,
    bool AcceptsWaiterCall,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);