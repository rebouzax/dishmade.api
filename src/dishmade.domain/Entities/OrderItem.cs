using dishmade.domain.Common;
using dishmade.domain.Enums;

namespace dishmade.domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Guid DishId { get; private set; }
    public Dish Dish { get; private set; } = null!;

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Notes { get; private set; }

    public OrderItemStatus Status { get; private set; } = OrderItemStatus.Created;

    public ICollection<OrderItemOption> Options { get; private set; } = [];

    private OrderItem()
    {
    }

    public OrderItem(
        Guid orderId,
        Guid dishId,
        int quantity,
        decimal unitPrice,
        string? notes = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("O preço unitário deve ser maior que zero.", nameof(unitPrice));

        OrderId = orderId;
        DishId = dishId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Notes = NormalizeNotes(notes);
        Status = OrderItemStatus.Created;
    }

    public void AddOption(
        Guid dishOptionId,
        string optionName,
        decimal additionalPrice)
    {
        EnsureCanBeChanged();

        var option = new OrderItemOption(
            Id,
            dishOptionId,
            optionName,
            additionalPrice);

        Options.Add(option);
        SetUpdatedAt();
    }

    public void StartPreparation()
    {
        EnsureCanBeChanged();

        if (Status != OrderItemStatus.Created)
            throw new InvalidOperationException("Somente itens criados podem entrar em preparo.");

        Status = OrderItemStatus.InPreparation;
        SetUpdatedAt();
    }

    public void MarkAsReady()
    {
        EnsureCanBeChanged();

        if (Status != OrderItemStatus.Created &&
            Status != OrderItemStatus.InPreparation)
        {
            throw new InvalidOperationException("Somente itens criados ou em preparo podem ficar prontos.");
        }

        Status = OrderItemStatus.Ready;
        SetUpdatedAt();
    }

    public void MarkAsDelivered()
    {
        if (Status == OrderItemStatus.Canceled)
            throw new InvalidOperationException("Itens cancelados não podem ser entregues.");

        if (Status == OrderItemStatus.Delivered)
            return;

        Status = OrderItemStatus.Delivered;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == OrderItemStatus.Delivered)
            throw new InvalidOperationException("Itens entregues não podem ser cancelados.");

        if (Status == OrderItemStatus.Canceled)
            throw new InvalidOperationException("O item já está cancelado.");

        Status = OrderItemStatus.Canceled;
        SetUpdatedAt();
    }

    public decimal GetOptionsTotal()
    {
        return Options.Sum(option => option.AdditionalPrice);
    }

    public decimal GetTotal()
    {
        if (Status == OrderItemStatus.Canceled)
            return 0;

        return (UnitPrice + GetOptionsTotal()) * Quantity;
    }

    private void EnsureCanBeChanged()
    {
        if (Status == OrderItemStatus.Delivered)
            throw new InvalidOperationException("Itens entregues não podem ser alterados.");

        if (Status == OrderItemStatus.Canceled)
            throw new InvalidOperationException("Itens cancelados não podem ser alterados.");
    }

    private static string? NormalizeNotes(string? notes)
    {
        return string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();
    }
}