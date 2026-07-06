using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.UpdateDishOption;

public sealed record UpdateDishOptionCommand(
    Guid DishId,
    Guid OptionGroupId,
    Guid OptionId,
    string Name,
    decimal AdditionalPrice
) : IRequest<DishOptionResponse>;