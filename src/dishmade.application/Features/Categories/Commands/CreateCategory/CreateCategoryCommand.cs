using MediatR;

namespace dishmade.application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    string? Description
) : IRequest<Guid>;
