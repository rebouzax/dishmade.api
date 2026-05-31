using MediatR;

namespace dishmade.application.Features.Public.Orders.Commands.CreatePublicOrder;

public sealed record CreatePublicOrderCommand(
    string RestaurantSlug,
    int TableNumber
) : IRequest<PublicOrderResponse>;