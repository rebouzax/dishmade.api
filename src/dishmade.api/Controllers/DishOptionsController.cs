using dishmade.application.Features.Dishes.OptionGroups.Commands.CreateDishOptionGroup;
using dishmade.application.Features.Dishes.OptionGroups.Queries.GetDishOptionGroups;
using dishmade.application.Features.Dishes.Options.Commands.CreateDishOption;
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

    [HttpPost("{optionGroupId:guid}/options")]
    public async Task<IActionResult> CreateOption(
        Guid dishId,
        Guid optionGroupId,
        [FromBody] CreateDishOptionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateDishOptionCommand(
                dishId,
                optionGroupId,
                request.Name,
                request.AdditionalPrice),
            cancellationToken);

        return Created(string.Empty, response);
    }
}

public sealed record CreateDishOptionGroupRequest(
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection
);

public sealed record CreateDishOptionRequest(
    string Name,
    decimal AdditionalPrice
);