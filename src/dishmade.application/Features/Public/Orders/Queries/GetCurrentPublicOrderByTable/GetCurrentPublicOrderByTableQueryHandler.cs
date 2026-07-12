using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Public.Orders.Queries.GetCurrentPublicOrderByTable;

public sealed class GetCurrentPublicOrderByTableQueryHandler
    : IRequestHandler<GetCurrentPublicOrderByTableQuery, PublicOrderResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IOrderRepository _orderRepository;

    public GetCurrentPublicOrderByTableQueryHandler(
        IRestaurantRepository restaurantRepository,
        IRestaurantTableRepository tableRepository,
        IOrderRepository orderRepository)
    {
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
        _orderRepository = orderRepository;
    }

    public async Task<PublicOrderResponse> Handle(
        GetCurrentPublicOrderByTableQuery request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetBySlugAsync(
            request.RestaurantSlug,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        var table = await _tableRepository.GetPublicByRestaurantIdAndNumberAsync(
            restaurant.Id,
            request.TableNumber,
            cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        var order = await _orderRepository.GetOpenDetailsByRestaurantIdAndTableIdAndAccessCodeAsync(
            restaurant.Id,
            table.Id,
            request.AccessCode,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido aberto não encontrado.");

        return PublicOrderMapper.ToResponse(order, restaurant);
    }
}