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
                item.Dish.Name,
                item.Quantity,
                item.UnitPrice,
                item.GetTotal()))
            .ToList();

        return new OrderResponse(
            order.Id,
            order.TableId,
            order.Table.Number,
            order.Status,
            order.GetTotal(),
            items,
            order.CreatedAt,
            order.UpdatedAt,
            order.DeliveredAt);
    }
}