using dishmade.application.Features.RestaurantSettings.Commands.UpdateRestaurantSettings;
using dishmade.application.Features.RestaurantSettings.Queries.GetRestaurantSettings;
using dishmade.domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/restaurant-settings")]
[Authorize(Roles = Roles.Client)]
public sealed class RestaurantSettingsController : ControllerBase
{
    private readonly ISender _sender;

    public RestaurantSettingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetRestaurantSettingsQuery(),
            cancellationToken);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateRestaurantSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new UpdateRestaurantSettingsCommand(
                request.DefaultServiceFeePercentage,
                request.AcceptsQrCodeOrders,
                request.AcceptsWaiterCall),
            cancellationToken);

        return Ok(response);
    }
}

public sealed record UpdateRestaurantSettingsRequest(
    decimal DefaultServiceFeePercentage,
    bool AcceptsQrCodeOrders,
    bool AcceptsWaiterCall
);