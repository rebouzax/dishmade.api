using dishmade.application.Features.Dishes.Commands.CreateDish;
using dishmade.application.Features.Dishes.Commands.DeleteDish;
using dishmade.application.Features.Dishes.Commands.DeleteDishImage;
using dishmade.application.Features.Dishes.Commands.UpdateDish;
using dishmade.application.Features.Dishes.Commands.UploadDishImage;
using dishmade.application.Features.Dishes.Queries.GetDishById;
using dishmade.application.Features.Dishes.Queries.GetDishes;
using dishmade.application.Features.Dishes.Queries.GetDishImage;
using dishmade.domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            request.CategoryId);

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
            request.CategoryId);

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

    [HttpPost("{id:guid}/image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadImage(
    Guid id,
    IFormFile file,
    CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "A imagem é obrigatória." });

        await using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream, cancellationToken);

        var command = new UploadDishImageCommand(
            id,
            file.FileName,
            file.ContentType,
            file.Length,
            memoryStream.ToArray());

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/image")]
    public async Task<IActionResult> GetImage(
        Guid id,
        CancellationToken cancellationToken)
    {
        var image = await _sender.Send(
            new GetDishImageQuery(id),
            cancellationToken);

        return File(
            image.Data,
            image.ContentType,
            image.FileName);
    }

    [HttpDelete("{id:guid}/image")]
    public async Task<IActionResult> DeleteImage(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteDishImageCommand(id),
            cancellationToken);

        return NoContent();
    }
}

public sealed record CreateDishRequest(
        string Name,
        string? Description,
        decimal Price,
        Guid CategoryId
    );
public sealed record UpdateDishRequest(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId
);