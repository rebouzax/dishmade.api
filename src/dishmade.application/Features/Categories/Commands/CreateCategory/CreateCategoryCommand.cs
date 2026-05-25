using MediatR;

namespace dishmade.application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    string? Description,
    Guid RestaurantId
) : IRequest<Guid>;
