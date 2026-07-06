using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.DeleteDishOptionGroup;

public sealed record DeleteDishOptionGroupCommand(
    Guid DishId,
    Guid OptionGroupId
) : IRequest;