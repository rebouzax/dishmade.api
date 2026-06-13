namespace dishmade.application.Features.Tables.MenuQrCode;

public sealed record TableMenuQrCodeResponse(
    Guid TableId,
    int TableNumber,
    Guid RestaurantId,
    string RestaurantName,
    string RestaurantSlug,
    bool IsEnabled,
    string? MenuUrl,
    string? QrCodeImageUrl
);