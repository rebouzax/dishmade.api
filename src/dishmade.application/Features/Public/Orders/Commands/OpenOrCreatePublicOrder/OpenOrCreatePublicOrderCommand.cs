using MediatR;

namespace dishmade.application.Features.Public.Orders.Commands.OpenOrCreatePublicOrder;

public sealed record OpenOrCreatePublicOrderCommand(
    string RestaurantSlug,
    int TableNumber,
    string? AccessCode
) : IRequest<PublicOrderSessionResponse>;