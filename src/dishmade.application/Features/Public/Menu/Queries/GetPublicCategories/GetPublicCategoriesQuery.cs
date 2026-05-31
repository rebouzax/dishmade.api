using dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicCategories;

public sealed record GetPublicCategoriesQuery(string Slug)
    : IRequest<IReadOnlyList<PublicCategoryResponse>>;