using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Public.Orders.Commands.AddItemToPublicOrder;

public sealed class AddItemToPublicOrderCommandHandler
    : IRequestHandler<AddItemToPublicOrderCommand, PublicOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToPublicOrderCommandHandler(
        IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository,
        IDishRepository dishRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _restaurantRepository = restaurantRepository;
        _dishRepository = dishRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PublicOrderResponse> Handle(
        AddItemToPublicOrderCommand request,
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

        var dish = await _dishRepository.GetPublicAvailableByIdAsync(
            request.DishId,
            order.RestaurantId,
            cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        order.AddItem(
            dish.Id,
            request.Quantity,
            dish.Price);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedOrder = await _orderRepository.GetPublicByIdAndAccessCodeAsync(
            request.OrderId,
            request.AccessCode,
            cancellationToken);

        if (updatedOrder is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        return PublicOrderMapper.ToResponse(updatedOrder, restaurant);
    }
}