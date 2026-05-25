using MediatR;

namespace dishmade.application.Features.Dishes.Commands.CreateDish;

public sealed record CreateDishCommand(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    Guid RestaurantId
) : IRequest<Guid>;
