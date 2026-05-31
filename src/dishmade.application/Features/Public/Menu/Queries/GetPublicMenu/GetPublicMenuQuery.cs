using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;

public sealed record GetPublicMenuQuery(string Slug) : IRequest<PublicMenuResponse>;