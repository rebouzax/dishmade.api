using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.SetDishOptionAvailability;

public sealed record SetDishOptionAvailabilityCommand(
    Guid DishId,
    Guid OptionGroupId,
    Guid OptionId,
    bool IsAvailable
) : IRequest<DishOptionResponse>;