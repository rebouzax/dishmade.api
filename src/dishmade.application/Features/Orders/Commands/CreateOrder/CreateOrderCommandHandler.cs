using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Kitchen;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier)
    {
        _orderRepository = orderRepository;
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.TableId, cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        if (table.IsOccupied)
            throw new InvalidOperationException("Não é possível criar pedido para uma mesa ocupada.");

        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var order = new Order(
            table.Id,
            restaurantId);

        table.Occupy();

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdOrder = await _orderRepository.GetByIdAsync(
            order.Id,
            cancellationToken);

        if (createdOrder is null)
            throw new KeyNotFoundException("Pedido criado não encontrado.");

        await _kitchenRealtimeNotifier.NotifyOrderCreatedAsync(
            createdOrder.RestaurantId,
            KitchenOrderRealtimeMapper.ToResponse(createdOrder),
            cancellationToken);

        return order.Id;
    }
}