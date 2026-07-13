using dishmade.application.Features.Orders.Payments;
using dishmade.domain.Entities;

namespace dishmade.application.Features.Orders.Receipts;

public static class OrderReceiptMapper
{
    public static OrderReceiptResponse ToResponse(Order order)
    {
        var items = order.Items
            .Select(item => new OrderReceiptItemResponse(
                item.Id,
                item.DishId,
                item.Dish?.Name ?? "Prato não carregado",
                item.Quantity,
                item.UnitPrice,
                item.GetOptionsTotal(),
                item.GetTotal(),
                item.Notes,
                item.Options
                    .Select(option => new OrderReceiptItemOptionResponse(
                        option.DishOptionId,
                        option.OptionName,
                        option.AdditionalPrice))
                    .ToList()))
            .ToList();

        var payments = order.Payments
            .Select(payment => new OrderPaymentResponse(
                payment.Id,
                payment.Method,
                payment.Status,
                payment.Amount,
                payment.Notes,
                payment.CreatedAt,
                payment.UpdatedAt))
            .ToList();

        return new OrderReceiptResponse(
            order.Id,
            order.RestaurantId,
            order.TableId,
            order.Table.Number,
            order.Status,
            order.PaymentStatus,
            order.GetSubtotal(),
            order.DiscountAmount,
            order.ServiceFeeAmount,
            order.GetFinalTotal(),
            order.GetPaidAmount(),
            order.GetRemainingAmount(),
            order.ClosedAt,
            order.PaidAt,
            order.CreatedAt,
            items,
            payments);
    }
}