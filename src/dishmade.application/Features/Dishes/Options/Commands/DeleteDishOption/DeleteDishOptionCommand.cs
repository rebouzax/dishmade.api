using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.DeleteDishOption;

public sealed record DeleteDishOptionCommand(
    Guid DishId,
    Guid OptionGroupId,
    Guid OptionId
) : IRequest;