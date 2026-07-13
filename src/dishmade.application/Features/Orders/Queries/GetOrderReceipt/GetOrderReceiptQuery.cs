using dishmade.application.Features.Orders.Receipts;
using MediatR;

namespace dishmade.application.Features.Orders.Queries.GetOrderReceipt;

public sealed record GetOrderReceiptQuery(Guid OrderId)
    : IRequest<OrderReceiptResponse>;