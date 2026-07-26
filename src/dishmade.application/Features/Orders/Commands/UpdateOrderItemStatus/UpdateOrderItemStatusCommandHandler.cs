using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Kitchen;
using dishmade.application.Features.Orders.Queries;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.UpdateOrderItemStatus;

public sealed class UpdateOrderItemStatusCommandHandler
    : IRequestHandler<UpdateOrderItemStatusCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderItemStatusCommandHandler(
        IOrderRepository orderRepository,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse> Handle(
        UpdateOrderItemStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        order.UpdateItemStatus(
            request.OrderItemId,
            request.Status);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedOrder = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (updatedOrder is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        await _kitchenRealtimeNotifier.NotifyOrderItemStatusChangedAsync(
            updatedOrder.RestaurantId,
            KitchenOrderRealtimeMapper.ToResponse(updatedOrder),
            cancellationToken);

        return OrderMapper.ToResponse(updatedOrder);
    }
}