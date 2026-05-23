using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.ChangeOrderStatus;

public sealed record ChangeOrderStatusCommand(
    Guid OrderId,
    OrderStatus Status
) : IRequest;