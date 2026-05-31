using dishmade.application.Features.Public.Menu.Queries.GetPublicCategories;
using dishmade.application.Features.Public.Menu.Queries.GetPublicDishImage;
using dishmade.application.Features.Public.Menu.Queries.GetPublicDishes;
using dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/public")]
[AllowAnonymous]
public sealed class PublicMenuController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IConfiguration _configuration;

    public PublicMenuController(
        ISender sender,
        IConfiguration configuration)
    {
        _sender = sender;
        _configuration = configuration;
    }

    [HttpGet("restaurants/{slug}/menu")]
    public async Task<IActionResult> GetMenu(
        string slug,
        CancellationToken cancellationToken)
    {
        var menu = await _sender.Send(
            new GetPublicMenuQuery(slug),
            cancellationToken);

        return Ok(menu);
    }

    [HttpGet("restaurants/{slug}/categories")]
    public async Task<IActionResult> GetCategories(
        string slug,
        CancellationToken cancellationToken)
    {
        var categories = await _sender.Send(
            new GetPublicCategoriesQuery(slug),
            cancellationToken);

        return Ok(categories);
    }

    [HttpGet("restaurants/{slug}/dishes")]
    public async Task<IActionResult> GetDishes(
        string slug,
        [FromQuery] Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var dishes = await _sender.Send(
            new GetPublicDishesQuery(slug, categoryId),
            cancellationToken);

        return Ok(dishes);
    }

    [HttpGet("dishes/{id:guid}/image")]
    public async Task<IActionResult> GetDishImage(
        Guid id,
        CancellationToken cancellationToken)
    {
        var image = await _sender.Send(
            new GetPublicDishImageQuery(id),
            cancellationToken);

        return File(
            image.Data,
            image.ContentType,
            image.FileName);
    }

    [HttpGet("restaurants/{slug}/qr-code")]
    public IActionResult GetRestaurantQrCode(string slug)
    {
        var baseUrl = _configuration["PublicMenu:BaseUrl"]?.TrimEnd('/')
            ?? "http://localhost:3000/menu";

        var menuUrl = $"{baseUrl}/{slug}";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(
            menuUrl,
            QRCodeGenerator.ECCLevel.Q);

        var pngQrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = pngQrCode.GetGraphic(20);

        return File(qrCodeBytes, "image/png", $"{slug}-qrcode.png");
    }
}