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
        OrderId = orderId;
        DishId = dishId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Notes = NormalizeNotes(notes);
    }

    public decimal GetTotal()
    {
        return Quantity * UnitPrice;
    }

    private static string? NormalizeNotes(string? notes)
    {
        return string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();
    }
}