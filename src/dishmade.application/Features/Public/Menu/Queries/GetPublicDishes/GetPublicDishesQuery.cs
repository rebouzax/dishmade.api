using dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicDishes;

public sealed record GetPublicDishesQuery(
    string Slug,
    Guid? CategoryId
) : IRequest<IReadOnlyList<PublicDishResponse>>;