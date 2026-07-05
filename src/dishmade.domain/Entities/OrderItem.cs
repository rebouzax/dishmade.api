using dishmade.domain.Common;

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
    }

    public void AddOption(
        Guid dishOptionId,
        string optionName,
        decimal additionalPrice)
    {
        var option = new OrderItemOption(
            Id,
            dishOptionId,
            optionName,
            additionalPrice);

        Options.Add(option);
        SetUpdatedAt();
    }

    public decimal GetOptionsTotal()
    {
        return Options.Sum(option => option.AdditionalPrice);
    }

    public decimal GetTotal()
    {
        return (UnitPrice + GetOptionsTotal()) * Quantity;
    }

    private static string? NormalizeNotes(string? notes)
    {
        return string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();
    }
}