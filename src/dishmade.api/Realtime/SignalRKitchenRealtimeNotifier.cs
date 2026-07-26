using dishmade.api.Hubs;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Features.Kitchen;
using dishmade.application.Features.Public.ServiceRequests;
using Microsoft.AspNetCore.SignalR;

namespace dishmade.api.Realtime;

public sealed class SignalRKitchenRealtimeNotifier : IKitchenRealtimeNotifier
{
    private readonly IHubContext<KitchenHub> _hubContext;

    public SignalRKitchenRealtimeNotifier(IHubContext<KitchenHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyOrderCreatedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.OrderCreated,
            order,
            cancellationToken);
    }

    public async Task NotifyOrderItemAddedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.OrderItemAdded,
            order,
            cancellationToken);
    }

    public async Task NotifyOrderStatusChangedAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.OrderStatusChanged,
            order,
            cancellationToken);
    }

    public async Task NotifyOrderCanceledAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.OrderCanceled,
            order,
            cancellationToken);
    }

    public async Task NotifyOrderDeliveredAsync(
        Guid restaurantId,
        KitchenOrderRealtimeResponse order,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.OrderDelivered,
            order,
            cancellationToken);
    }

    public async Task NotifyServiceRequestCreatedAsync(
        Guid restaurantId,
        ServiceRequestResponse serviceRequest,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.ServiceRequestCreated,
            serviceRequest,
            cancellationToken);
    }

    public async Task NotifyServiceRequestUpdatedAsync(
        Guid restaurantId,
        ServiceRequestResponse serviceRequest,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.ServiceRequestUpdated,
            serviceRequest,
            cancellationToken);
    }

    private async Task SendAsync(
        Guid restaurantId,
        string eventName,
        object payload,
        CancellationToken cancellationToken)
    {
        var groupName = KitchenRealtimeGroups.Restaurant(restaurantId);

        await _hubContext.Clients
            .Group(groupName)
            .SendAsync(
                eventName,
                payload,
                cancellationToken);
    }

    public async Task NotifyOrderItemStatusChangedAsync(
    Guid restaurantId,
    KitchenOrderRealtimeResponse order,
    CancellationToken cancellationToken = default)
    {
        await SendAsync(
            restaurantId,
            KitchenRealtimeEvents.OrderItemStatusChanged,
            order,
            cancellationToken);
    }
}