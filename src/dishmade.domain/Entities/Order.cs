using dishmade.domain.Common;
using dishmade.domain.Enums;

namespace dishmade.domain.Entities;

public sealed class Order : BaseEntity
{
    public Guid TableId { get; private set; }
    public RestaurantTable Table { get; private set; } = null!;

    public OrderStatus Status { get; private set; } = OrderStatus.Created;

    public ICollection<OrderItem> Items { get; private set; } = [];

    private Order()
    {
    }

    public Order(Guid tableId)
    {
        TableId = tableId;
    }

    public void AddItem(Guid dishId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("O preço unitário deve ser maior que zero.", nameof(unitPrice));

        Items.Add(new OrderItem(Id, dishId, quantity, unitPrice));
        SetUpdatedAt();
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
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem ser cancelados.");

        Status = OrderStatus.Canceled;
        SetUpdatedAt();
    }

    public decimal GetTotal()
    {
        return Items.Sum(item => item.GetTotal());
    }
}