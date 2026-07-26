using dishmade.application.Features.Orders.Queries;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.UpdateOrderItemStatus;

public sealed record UpdateOrderItemStatusCommand(
    Guid OrderId,
    Guid OrderItemId,
    OrderItemStatus Status
) : IRequest<OrderResponse>;