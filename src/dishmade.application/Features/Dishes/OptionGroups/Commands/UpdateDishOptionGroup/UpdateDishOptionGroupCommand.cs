using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.UpdateDishOptionGroup;

public sealed record UpdateDishOptionGroupCommand(
    Guid DishId,
    Guid OptionGroupId,
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection
) : IRequest<DishOptionGroupResponse>;