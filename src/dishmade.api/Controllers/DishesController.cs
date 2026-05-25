using dishmade.application.Features.Dishes.Commands.CreateDish;
using dishmade.application.Features.Dishes.Commands.DeleteDish;
using dishmade.application.Features.Dishes.Commands.UpdateDish;
using dishmade.application.Features.Dishes.Queries.GetDishById;
using dishmade.application.Features.Dishes.Queries.GetDishes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using dishmade.domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/dishes")]
[Authorize(Roles = Roles.Client)]
public sealed class DishesController : ControllerBase
{
    private readonly ISender _sender;

    public DishesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDishRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDishCommand(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            request.RestaurantId);

        var dishId = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = dishId },
            new { id = dishId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] string? search,
    [FromQuery] Guid? categoryId,
    [FromQuery] bool? isAvailable,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var dishes = await _sender.Send(
            new GetDishesQuery(search, categoryId, isAvailable, pageNumber, pageSize),
            cancellationToken);

        return Ok(dishes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var dish = await _sender.Send(new GetDishByIdQuery(id), cancellationToken);

        return Ok(dish);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDishRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDishCommand(
            id,
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            request.RestaurantId);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteDishCommand(id), cancellationToken);

        return NoContent();
    }
}

public sealed record CreateDishRequest(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    Guid RestaurantId
);

public sealed record UpdateDishRequest(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    Guid RestaurantId
);