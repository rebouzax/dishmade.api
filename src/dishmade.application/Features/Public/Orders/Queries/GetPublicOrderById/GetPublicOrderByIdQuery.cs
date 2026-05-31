using MediatR;

namespace dishmade.application.Features.Public.Orders.Queries.GetPublicOrderById;

public sealed record GetPublicOrderByIdQuery(
    Guid OrderId,
    string AccessCode
) : IRequest<PublicOrderResponse>;