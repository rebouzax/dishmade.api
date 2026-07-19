using dishmade.application.Features.Orders.Receipts;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.CloseOrderAccount;

public sealed record CloseOrderAccountCommand(
    Guid OrderId,
    decimal DiscountAmount,
    decimal? ServiceFeeAmount,
    bool UseDefaultServiceFee
) : IRequest<OrderReceiptResponse>;