using dishmade.domain.Entities;

namespace dishmade.application.Features.Kitchen;

public static class KitchenOrderRealtimeMapper
{
    public static KitchenOrderRealtimeResponse ToResponse(Order order)
    {
        return new KitchenOrderRealtimeResponse(
            order.Id,
            order.RestaurantId,
            order.TableId,
            order.Table.Number,
            order.Status,
            order.PaymentStatus,
            order.GetTotal(),
            order.CreatedAt,
            order.UpdatedAt,
            order.Items
                .Select(item => new KitchenOrderItemRealtimeResponse(
                    item.Id,
                    item.DishId,
                    item.Dish?.Name ?? "Prato não carregado",
                    item.Quantity,
                    item.UnitPrice,
                    item.GetOptionsTotal(),
                    item.GetTotal(),
                    item.Notes,
                    item.Options
                        .Select(option => new KitchenOrderItemOptionRealtimeResponse(
                            option.DishOptionId,
                            option.OptionName,
                            option.AdditionalPrice))
                        .ToList()))
                .ToList());
    }
}