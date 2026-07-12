using MediatR;

namespace dishmade.application.Features.Public.Orders.Queries.GetCurrentPublicOrderByTable;

public sealed record GetCurrentPublicOrderByTableQuery(
    string RestaurantSlug,
    int TableNumber,
    string AccessCode
) : IRequest<PublicOrderResponse>;