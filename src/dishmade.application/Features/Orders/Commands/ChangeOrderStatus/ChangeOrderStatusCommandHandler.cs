using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Kitchen;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.ChangeOrderStatus;

public sealed class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;

    public ChangeOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;
    }

    public async Task Handle(
        ChangeOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        switch (request.Status)
        {
            case OrderStatus.InPreparation:
                order.StartPreparation();
                break;

            case OrderStatus.Ready:
                order.MarkAsReady();
                break;

            case OrderStatus.Delivered:
                order.Deliver();
                order.Table.Release();
                break;

            default:
                throw new InvalidOperationException("Status inválido para alteração.");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedOrder = await _orderRepository.GetByIdAsync(order.Id, cancellationToken);

        if (updatedOrder is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        var payload = KitchenOrderRealtimeMapper.ToResponse(updatedOrder);

        if (updatedOrder.Status == OrderStatus.Canceled)
        {
            await _kitchenRealtimeNotifier.NotifyOrderCanceledAsync(
                updatedOrder.RestaurantId,
                payload,
                cancellationToken);

            return;
        }

        if (updatedOrder.Status == OrderStatus.Delivered)
        {
            await _kitchenRealtimeNotifier.NotifyOrderDeliveredAsync(
                updatedOrder.RestaurantId,
                payload,
                cancellationToken);

            return;
        }

        await _kitchenRealtimeNotifier.NotifyOrderStatusChangedAsync(
            updatedOrder.RestaurantId,
            payload,
            cancellationToken);
    }
}