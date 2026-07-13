using dishmade.domain.Enums;

namespace dishmade.application.Features.Orders.Payments;

public sealed record OrderPaymentResponse(
    Guid Id,
    PaymentMethod Method,
    PaymentStatus Status,
    decimal Amount,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);