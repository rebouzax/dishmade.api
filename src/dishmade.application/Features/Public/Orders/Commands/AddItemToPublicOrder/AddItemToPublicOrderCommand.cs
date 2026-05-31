using MediatR;

namespace dishmade.application.Features.Public.Orders.Commands.AddItemToPublicOrder;

public sealed record AddItemToPublicOrderCommand(
    Guid OrderId,
    string AccessCode,
    Guid DishId,
    int Quantity
) : IRequest<PublicOrderResponse>;