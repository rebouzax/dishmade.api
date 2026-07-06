using dishmade.application.Features.Dishes.OptionGroups.Commands.CreateDishOptionGroup;
using dishmade.application.Features.Dishes.OptionGroups.Commands.DeleteDishOptionGroup;
using dishmade.application.Features.Dishes.OptionGroups.Commands.UpdateDishOptionGroup;
using dishmade.application.Features.Dishes.OptionGroups.Queries.GetDishOptionGroups;
using dishmade.application.Features.Dishes.Options.Commands.CreateDishOption;
using dishmade.application.Features.Dishes.Options.Commands.DeleteDishOption;
using dishmade.application.Features.Dishes.Options.Commands.SetDishOptionAvailability;
using dishmade.application.Features.Dishes.Options.Commands.UpdateDishOption;
using dishmade.domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/dishes/{dishId:guid}/option-groups")]
[Authorize(Roles = Roles.Client)]
public sealed class DishOptionsController : ControllerBase
{
    private readonly ISender _sender;

    public DishOptionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups(
        Guid dishId,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetDishOptionGroupsQuery(dishId),
            cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup(
        Guid dishId,
        [FromBody] CreateDishOptionGroupRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateDishOptionGroupCommand(
                dishId,
                request.Name,
                request.IsRequired,
                request.MinSelection,
                request.MaxSelection),
            cancellationToken);

        return Created(string.Empty, response);
    }

    [HttpPut("{groupId:guid}")]
    public async Task<IActionResult> UpdateGroup(
        Guid dishId,
        Guid groupId,
        [FromBody] UpdateDishOptionGroupRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new UpdateDishOptionGroupCommand(
                dishId,
                groupId,
                request.Name,
                request.IsRequired,
                request.MinSelection,
                request.MaxSelection),
            cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{groupId:guid}")]
    public async Task<IActionResult> DeleteGroup(
        Guid dishId,
        Guid groupId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteDishOptionGroupCommand(
                dishId,
                groupId),
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{groupId:guid}/options")]
    public async Task<IActionResult> CreateOption(
        Guid dishId,
        Guid groupId,
        [FromBody] CreateDishOptionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateDishOptionCommand(
                dishId,
                groupId,
                request.Name,
                request.AdditionalPrice),
            cancellationToken);

        return Created(string.Empty, response);
    }

    [HttpPut("{groupId:guid}/options/{optionId:guid}")]
    public async Task<IActionResult> UpdateOption(
        Guid dishId,
        Guid groupId,
        Guid optionId,
        [FromBody] UpdateDishOptionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new UpdateDishOptionCommand(
                dishId,
                groupId,
                optionId,
                request.Name,
                request.AdditionalPrice),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{groupId:guid}/options/{optionId:guid}/available")]
    public async Task<IActionResult> SetOptionAvailable(
        Guid dishId,
        Guid groupId,
        Guid optionId,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new SetDishOptionAvailabilityCommand(
                dishId,
                groupId,
                optionId,
                true),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{groupId:guid}/options/{optionId:guid}/unavailable")]
    public async Task<IActionResult> SetOptionUnavailable(
        Guid dishId,
        Guid groupId,
        Guid optionId,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new SetDishOptionAvailabilityCommand(
                dishId,
                groupId,
                optionId,
                false),
            cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{groupId:guid}/options/{optionId:guid}")]
    public async Task<IActionResult> DeleteOption(
        Guid dishId,
        Guid groupId,
        Guid optionId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteDishOptionCommand(
                dishId,
                groupId,
                optionId),
            cancellationToken);

        return NoContent();
    }
}

public sealed record CreateDishOptionGroupRequest(
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection
);

public sealed record UpdateDishOptionGroupRequest(
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection
);

public sealed record CreateDishOptionRequest(
    string Name,
    decimal AdditionalPrice
);

public sealed record UpdateDishOptionRequest(
    string Name,
    decimal AdditionalPrice
);