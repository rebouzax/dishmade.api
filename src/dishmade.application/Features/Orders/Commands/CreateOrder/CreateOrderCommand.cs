using MediatR;

namespace dishmade.application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(Guid TableId) : IRequest<Guid>;