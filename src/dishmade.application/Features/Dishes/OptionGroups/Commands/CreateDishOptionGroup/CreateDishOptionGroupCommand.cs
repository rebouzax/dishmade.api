using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.CreateDishOptionGroup;

public sealed record CreateDishOptionGroupCommand(
    Guid DishId,
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection
) : IRequest<DishOptionGroupResponse>;