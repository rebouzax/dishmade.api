using MediatR;

namespace dishmade.application.Features.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery : IRequest<IReadOnlyList<OrderResponse>>;