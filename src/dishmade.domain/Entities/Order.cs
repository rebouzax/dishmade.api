using dishmade.domain.Common;
using dishmade.domain.Enums;

namespace dishmade.domain.Entities;

public sealed class Order : RestaurantScopedEntity
{
    public Guid TableId { get; private set; }
    public RestaurantTable Table { get; private set; } = null!;

    public OrderStatus Status { get; private set; } = OrderStatus.Created;

    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;

    public decimal DiscountAmount { get; private set; }
    public decimal ServiceFeeAmount { get; private set; }

    public DateTime? ClosedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    public string? PublicAccessCode { get; private set; }

    public ICollection<OrderItem> Items { get; private set; } = [];
    public ICollection<OrderPayment> Payments { get; private set; } = [];

    private Order()
    {
    }

    public Order(Guid tableId, Guid restaurantId)
    {
        SetRestaurantId(restaurantId);
        TableId = tableId;
    }

    public OrderItem AddItem(
        Guid dishId,
        int quantity,
        decimal unitPrice,
        string? notes = null)
    {
        EnsureCanBeChanged();

        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

        if (unitPrice <= 0)
            throw new ArgumentException("O preço unitário deve ser maior que zero.", nameof(unitPrice));

        var item = new OrderItem(
            Id,
            dishId,
            quantity,
            unitPrice,
            notes);

        Items.Add(item);
        SetUpdatedAt();

        return item;
    }

    public void CloseAccount(
        decimal discountAmount,
        decimal serviceFeeAmount)
    {
        if (Status == OrderStatus.Canceled)
            throw new InvalidOperationException("Pedidos cancelados não podem ser fechados.");

        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem ser fechados novamente.");

        if (Status != OrderStatus.Ready)
            throw new InvalidOperationException("Somente pedidos prontos podem ter a conta fechada.");

        if (!Items.Any())
            throw new InvalidOperationException("Não é possível fechar uma conta sem itens.");

        if (discountAmount < 0)
            throw new ArgumentException("O desconto não pode ser negativo.", nameof(discountAmount));

        if (serviceFeeAmount < 0)
            throw new ArgumentException("A taxa de serviço não pode ser negativa.", nameof(serviceFeeAmount));

        var subtotalWithServiceFee = GetSubtotal() + serviceFeeAmount;

        if (discountAmount > subtotalWithServiceFee)
            throw new InvalidOperationException("O desconto não pode ser maior que o subtotal somado à taxa de serviço.");

        DiscountAmount = discountAmount;
        ServiceFeeAmount = serviceFeeAmount;
        ClosedAt = DateTime.UtcNow;

        UpdatePaymentStatus();
        SetUpdatedAt();
    }

    public OrderPayment RegisterPayment(
        PaymentMethod method,
        decimal amount,
        string? notes = null)
    {
        if (Status == OrderStatus.Canceled)
            throw new InvalidOperationException("Pedidos cancelados não podem receber pagamento.");

        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem receber novo pagamento.");

        if (ClosedAt is null)
            throw new InvalidOperationException("A conta precisa ser fechada antes de registrar pagamento.");

        if (amount <= 0)
            throw new ArgumentException("O valor do pagamento deve ser maior que zero.", nameof(amount));

        var payment = new OrderPayment(
            Id,
            RestaurantId,
            method,
            amount,
            notes);

        Payments.Add(payment);

        UpdatePaymentStatus();
        SetUpdatedAt();

        return payment;
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

        foreach (var item in Items.Where(item => item.Status != OrderItemStatus.Canceled))
        {
            item.MarkAsDelivered();
        }

        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem ser cancelados.");

        if (Status == OrderStatus.Canceled)
            throw new InvalidOperationException("O pedido já está cancelado.");

        Status = OrderStatus.Canceled;
        PaymentStatus = PaymentStatus.Canceled;

        SetUpdatedAt();
    }

    public void SetPublicAccessCode(string accessCode)
    {
        if (string.IsNullOrWhiteSpace(accessCode))
            throw new ArgumentException("O código público do pedido é obrigatório.", nameof(accessCode));

        PublicAccessCode = accessCode;
        SetUpdatedAt();
    }

    public decimal GetSubtotal()
    {
        return Items.Sum(item => item.GetTotal());
    }

    public decimal GetPaidAmount()
    {
        return Payments
            .Where(payment => payment.Status == PaymentStatus.Paid)
            .Sum(payment => payment.Amount);
    }

    public decimal GetRemainingAmount()
    {
        var remaining = GetFinalTotal() - GetPaidAmount();

        return remaining <= 0 ? 0 : remaining;
    }

    public decimal GetFinalTotal()
    {
        var total = GetSubtotal() + ServiceFeeAmount - DiscountAmount;

        return total <= 0 ? 0 : total;
    }

    public decimal GetTotal()
    {
        return GetFinalTotal();
    }

    public bool IsFullyPaid()
    {
        return GetPaidAmount() >= GetFinalTotal();
    }

    public void MarkAsPaidIfFullyPaid()
    {
        UpdatePaymentStatus();

        if (PaymentStatus == PaymentStatus.Paid)
        {
            PaidAt ??= DateTime.UtcNow;
            SetUpdatedAt();
        }
    }

    private void UpdatePaymentStatus()
    {
        var paidAmount = GetPaidAmount();
        var finalTotal = GetFinalTotal();

        if (finalTotal <= 0)
        {
            PaymentStatus = PaymentStatus.Pending;
            return;
        }

        if (paidAmount <= 0)
        {
            PaymentStatus = PaymentStatus.Pending;
            return;
        }

        if (paidAmount < finalTotal)
        {
            PaymentStatus = PaymentStatus.PartiallyPaid;
            return;
        }

        PaymentStatus = PaymentStatus.Paid;
        PaidAt ??= DateTime.UtcNow;
    }

    private void EnsureCanBeChanged()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Pedidos entregues não podem ser alterados.");

        if (Status == OrderStatus.Canceled)
            throw new InvalidOperationException("Pedidos cancelados não podem ser alterados.");

        if (ClosedAt is not null)
            throw new InvalidOperationException("Pedidos com conta fechada não podem receber novos itens.");
    }

    private void SyncOrderStatusFromItems()
    {
        if (Status == OrderStatus.Delivered || Status == OrderStatus.Canceled)
            return;

        if (!Items.Any())
        {
            Status = OrderStatus.Created;
            return;
        }

        var activeItems = Items
            .Where(item => item.Status != OrderItemStatus.Canceled)
            .ToList();

        if (activeItems.Count == 0)
        {
            Status = OrderStatus.Canceled;
            return;
        }

        if (activeItems.All(item =>
                item.Status == OrderItemStatus.Ready ||
                item.Status == OrderItemStatus.Delivered))
        {
            Status = OrderStatus.Ready;
            return;
        }

        if (activeItems.Any(item =>
                item.Status == OrderItemStatus.InPreparation ||
                item.Status == OrderItemStatus.Ready ||
                item.Status == OrderItemStatus.Delivered))
        {
            Status = OrderStatus.InPreparation;
            return;
        }

        Status = OrderStatus.Created;
    }
    public void UpdateItemStatus(
    Guid orderItemId,
    OrderItemStatus status)
    {
        EnsureCanBeChanged();

        var item = Items.FirstOrDefault(item => item.Id == orderItemId);

        if (item is null)
            throw new KeyNotFoundException("Item do pedido não encontrado.");

        switch (status)
        {
            case OrderItemStatus.Created:
                throw new InvalidOperationException("Não é possível voltar o item para criado.");

            case OrderItemStatus.InPreparation:
                item.StartPreparation();
                break;

            case OrderItemStatus.Ready:
                item.MarkAsReady();
                break;

            case OrderItemStatus.Delivered:
                item.MarkAsDelivered();
                break;

            case OrderItemStatus.Canceled:
                item.Cancel();
                break;

            default:
                throw new InvalidOperationException("Status do item inválido.");
        }

        SyncOrderStatusFromItems();

        SetUpdatedAt();
    }
}