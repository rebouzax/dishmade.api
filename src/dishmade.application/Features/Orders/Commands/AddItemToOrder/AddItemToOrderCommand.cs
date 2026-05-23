using MediatR;

namespace dishmade.application.Features.Orders.Commands.AddItemToOrder;

public sealed record AddItemToOrderCommand(
    Guid OrderId,
    Guid DishId,
    int Quantity
) : IRequest;