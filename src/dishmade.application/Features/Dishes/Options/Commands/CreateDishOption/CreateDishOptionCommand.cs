using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.CreateDishOption;

public sealed record CreateDishOptionCommand(
    Guid DishId,
    Guid OptionGroupId,
    string Name,
    decimal AdditionalPrice
) : IRequest<DishOptionResponse>;