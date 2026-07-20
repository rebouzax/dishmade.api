using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Security;
using dishmade.application.Features.Kitchen;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Public.Orders.Commands.CreatePublicOrder;

public sealed class CreatePublicOrderCommandHandler
    : IRequestHandler<CreatePublicOrderCommand, PublicOrderResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;

    public CreatePublicOrderCommandHandler(
        IRestaurantRepository restaurantRepository,
        IRestaurantTableRepository tableRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier)
    {
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;

    }

    public async Task<PublicOrderResponse> Handle(
        CreatePublicOrderCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetBySlugAsync(
            request.RestaurantSlug,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        if (!restaurant.AcceptsQrCodeOrders)
            throw new InvalidOperationException("Este restaurante não aceita pedidos pelo QR Code no momento.");

        var table = await _tableRepository.GetPublicByRestaurantIdAndNumberAsync(
            restaurant.Id,
            request.TableNumber,
            cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        if (table.IsOccupied)
            throw new InvalidOperationException("Essa mesa já possui um pedido aberto.");

        var order = new Order(table.Id, restaurant.Id);

        order.SetPublicAccessCode(PublicAccessCodeGenerator.Generate());

        table.Occupy();

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdOrder = await _orderRepository.GetPublicDetailsByIdAsync(order.Id, cancellationToken);

        if (createdOrder is null)
            throw new KeyNotFoundException("Pedido criado não encontrado.");

        var kitchenPayload = KitchenOrderRealtimeMapper.ToResponse(createdOrder);

        await _kitchenRealtimeNotifier.NotifyOrderCreatedAsync(
            restaurant.Id,
            kitchenPayload,
            cancellationToken);

        return PublicOrderMapper.ToResponse(createdOrder, restaurant);
    }
}