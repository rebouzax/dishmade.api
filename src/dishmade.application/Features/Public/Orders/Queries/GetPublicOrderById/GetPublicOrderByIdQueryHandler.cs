using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Public.Orders.Queries.GetPublicOrderById;

public sealed class GetPublicOrderByIdQueryHandler
    : IRequestHandler<GetPublicOrderByIdQuery, PublicOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantRepository _restaurantRepository;

    public GetPublicOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository)
    {
        _orderRepository = orderRepository;
        _restaurantRepository = restaurantRepository;
    }

    public async Task<PublicOrderResponse> Handle(
        GetPublicOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetPublicByIdAndAccessCodeAsync(
            request.OrderId,
            request.AccessCode,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        var restaurant = await _restaurantRepository.GetByIdAsync(
            order.RestaurantId,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        return PublicOrderMapper.ToResponse(order, restaurant);
    }
}