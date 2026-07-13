using dishmade.application.Features.Orders.Receipts;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.RegisterOrderPayment;

public sealed record RegisterOrderPaymentCommand(
    Guid OrderId,
    PaymentMethod Method,
    decimal Amount,
    string? Notes
) : IRequest<OrderReceiptResponse>;