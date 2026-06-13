using dishmade.domain.Entities;
using Microsoft.Extensions.Configuration;

namespace dishmade.application.Features.Tables.MenuQrCode;

public static class TableMenuQrCodeResponseFactory
{
    public static TableMenuQrCodeResponse Create(
        RestaurantTable table,
        Restaurant restaurant,
        IConfiguration configuration)
    {
        var baseUrl = configuration["PublicMenu:BaseUrl"]?.TrimEnd('/')
            ?? "http://localhost:3000/menu";

        var menuUrl = table.IsMenuQrCodeEnabled
            ? $"{baseUrl}/{restaurant.Slug}?table={table.Number}"
            : null;

        var qrCodeImageUrl = table.IsMenuQrCodeEnabled
            ? $"/api/tables/{table.Id}/menu-qr-code/image"
            : null;

        return new TableMenuQrCodeResponse(
            table.Id,
            table.Number,
            restaurant.Id,
            restaurant.Name,
            restaurant.Slug,
            table.IsMenuQrCodeEnabled,
            menuUrl,
            qrCodeImageUrl);
    }
}