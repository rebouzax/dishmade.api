using dishmade.domain.Entities;

namespace dishmade.application.Features.Orders.Queries;

public static class OrderMapper
{
    public static OrderResponse ToResponse(Order order)
    {
        var items = order.Items
            .Select(item => new OrderItemResponse(
                item.Id,
                item.DishId,
                item.Dish?.Name ?? "Prato não carregado",
                item.Quantity,
                item.UnitPrice,
                item.GetOptionsTotal(),
                item.GetTotal(),
                item.Notes,
                item.Status,
                item.Options
                    .Select(option => new OrderItemOptionResponse(
                        option.DishOptionId,
                        option.OptionName,
                        option.AdditionalPrice))
                    .ToList()))
            .ToList();

        return new OrderResponse(
            order.Id,
            order.TableId,
            order.Table.Number,
            order.Status,
            order.GetTotal(),
            order.PaymentStatus,
            order.GetSubtotal(),
            order.DiscountAmount,
            order.ServiceFeeAmount,
            order.GetFinalTotal(),
            order.GetPaidAmount(),
            order.GetRemainingAmount(),
            order.ClosedAt,
            order.PaidAt,
            items,
            order.CreatedAt,
            order.UpdatedAt,
            order.DeliveredAt);
    }
}