using dishmade.domain.Entities;

namespace dishmade.application.Features.Public.Orders;

public static class PublicOrderMapper
{
    public static PublicOrderResponse ToResponse(Order order, Restaurant restaurant)
    {
        var items = order.Items
            .Select(item => new PublicOrderItemResponse(
                item.Id,
                item.DishId,
                item.Dish?.Name ?? "Prato não carregado",
                item.Quantity,
                item.UnitPrice,
                item.GetTotal()))
            .ToList();

        return new PublicOrderResponse(
            order.Id,
            order.PublicAccessCode ?? string.Empty,
            restaurant.Id,
            restaurant.Name,
            order.TableId,
            order.Table.Number,
            order.Status,
            order.GetTotal(),
            items,
            order.CreatedAt);
    }
}