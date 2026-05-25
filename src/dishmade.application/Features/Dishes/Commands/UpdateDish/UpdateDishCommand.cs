using MediatR;

namespace dishmade.application.Features.Dishes.Commands.UpdateDish;

public sealed record UpdateDishCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    Guid RestaunratId
) : IRequest;