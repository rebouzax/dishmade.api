using dishmade.application.Features.Public.Orders.Commands.AddItemToPublicOrder;
using dishmade.application.Features.Public.Orders.Commands.CreatePublicOrder;
using dishmade.application.Features.Public.Orders.Commands.OpenOrCreatePublicOrder;
using dishmade.application.Features.Public.Orders.Queries.GetCurrentPublicOrderByTable;
using dishmade.application.Features.Public.Orders.Queries.GetPublicOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/public/orders")]
[AllowAnonymous]
public sealed class PublicOrdersController : ControllerBase
{
    private readonly ISender _sender;

    public PublicOrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePublicOrderRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreatePublicOrderCommand(
                request.RestaurantSlug,
                request.TableNumber),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                id = response.OrderId,
                accessCode = response.AccessCode
            },
            response);
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid id,
        [FromBody] AddItemToPublicOrderRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new AddItemToPublicOrderCommand(
                id,
                request.AccessCode,
                request.DishId,
                request.Quantity,
                request.Notes,
                request.OptionIds ?? []),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] string accessCode,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetPublicOrderByIdQuery(id, accessCode),
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("open-or-create")]
    public async Task<IActionResult> OpenOrCreate(
    [FromBody] OpenOrCreatePublicOrderRequest request,
    CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new OpenOrCreatePublicOrderCommand(
                request.RestaurantSlug,
                request.TableNumber,
                request.AccessCode),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("~/api/public/restaurants/{slug}/tables/{tableNumber:int}/current-order")]
    public async Task<IActionResult> GetCurrentOrderByTable(
    string slug,
    int tableNumber,
    [FromQuery] string accessCode,
    CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetCurrentPublicOrderByTableQuery(
                slug,
                tableNumber,
                accessCode),
            cancellationToken);

        return Ok(response);
    }
}

public sealed record CreatePublicOrderRequest(
    string RestaurantSlug,
    int TableNumber
);

public sealed record AddItemToPublicOrderRequest(
    string AccessCode,
    Guid DishId,
    int Quantity,
    string? Notes,
    IReadOnlyList<Guid>? OptionIds
);

public sealed record OpenOrCreatePublicOrderRequest(
    string RestaurantSlug,
    int TableNumber,
    string? AccessCode
);