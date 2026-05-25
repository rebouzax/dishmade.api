using dishmade.domain.Common;
using dishmade.domain.Enums;

namespace dishmade.domain.Entities;

public sealed class Order : RestaurantScopedEntity
{
    public Guid TableId { get; private set; }
    public RestaurantTable Table { get; private set; } = null!;

    public OrderStatus Status { get; private set; } = OrderStatus.Created;

    public DateTime? DeliveredAt { get; private set; }

    public ICollection<OrderItem> Items { get; private set; } = [];

    private Order()
    {
    }

    public Order(Guid tableId, Guid restaurantId)
    {
        SetRestaurantId(restaurantId);
        TableId = tableId;
    }

    public OrderItem AddItem(Guid dishId, int quantity, decimal unitPrice)
    {
        EnsureCanBeChanged();

        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("O preço unitário deve ser maior que zero.", nameof(unitPrice));

        var item = new OrderItem(Id, dishId, quantity, unitPrice);

        Items.Add(item);
        SetUpdatedAt();

        return item;
    }

    public void StartPreparation()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Somente pedidos criados podem entrar em preparo.");

        Status = OrderStatus.InPreparation;
        SetUpdatedAt();
    }

    public void MarkAsReady()
    {
        if (Status != OrderStatus.InPreparation)
            throw new InvalidOperationException("Somente pedidos em preparo podem ficar prontos.");

        Status = OrderStatus.Ready;
        SetUpdatedAt();
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Ready)
            throw new InvalidOperationException("Somente pedidos prontos podem ser entregues.");

        Status = OrderStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem ser cancelados.");

        if (Status == OrderStatus.Canceled)
            throw new InvalidOperationException("O pedido já está cancelado.");

        Status = OrderStatus.Canceled;
        SetUpdatedAt();
    }

    public decimal GetTotal()
    {
        return Items.Sum(item => item.GetTotal());
    }

    private void EnsureCanBeChanged()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem ser alterados.");

        if (Status == OrderStatus.Canceled)
            throw new InvalidOperationException("Pedidos cancelados não podem ser alterados.");
    }
}