using dishmade.application.Features.Kitchen;
using dishmade.application.Features.Public.ServiceRequests;

namespace dishmade.application.Abstractions.Realtime;

public interface IKitchenRealtimeNotifier
{
    Task NotifyOrderCreatedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default);

    Task NotifyOrderItemAddedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default);

    Task NotifyOrderItemStatusChangedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default);

    Task NotifyOrderStatusChangedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default);

    Task NotifyOrderCanceledAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default);

    Task NotifyOrderDeliveredAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default);

    Task NotifyServiceRequestCreatedAsync(
        Guid restaurantId,
        ServiceRequestResponse serviceRequest,
        CancellationToken cancellationToken = default);

    Task NotifyServiceRequestUpdatedAsync(
        Guid restaurantId,
        ServiceRequestResponse serviceRequest,
        CancellationToken cancellationToken = default);
}