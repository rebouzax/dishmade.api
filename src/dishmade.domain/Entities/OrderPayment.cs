using dishmade.domain.Common;
using dishmade.domain.Enums;

namespace dishmade.domain.Entities;

public sealed class OrderPayment : RestaurantScopedEntity
{
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Paid;

    public decimal Amount { get; private set; }
    public string? Notes { get; private set; }

    private OrderPayment()
    {
    }

    public OrderPayment(
        Guid orderId,
        Guid restaurantId,
        PaymentMethod method,
        decimal amount,
        string? notes)
    {
        SetRestaurantId(restaurantId);

        if (amount <= 0)
            throw new ArgumentException("O valor do pagamento deve ser maior que zero.", nameof(amount));

        OrderId = orderId;
        Method = method;
        Amount = amount;
        Notes = NormalizeNotes(notes);
    }

    public void Cancel()
    {
        if (Status == PaymentStatus.Canceled)
            throw new InvalidOperationException("O pagamento já está cancelado.");

        if (Status == PaymentStatus.Refunded)
            throw new InvalidOperationException("Pagamentos estornados não podem ser cancelados.");

        Status = PaymentStatus.Canceled;
        SetUpdatedAt();
    }

    public void Refund()
    {
        if (Status == PaymentStatus.Refunded)
            throw new InvalidOperationException("O pagamento já está estornado.");

        if (Status == PaymentStatus.Canceled)
            throw new InvalidOperationException("Pagamentos cancelados não podem ser estornados.");

        Status = PaymentStatus.Refunded;
        SetUpdatedAt();
    }

    private static string? NormalizeNotes(string? notes)
    {
        return string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();
    }
}