namespace dishmade.application.Abstractions.Realtime;

public static class KitchenRealtimeEvents
{
    public const string OrderCreated = "KitchenOrderCreated";
    public const string OrderItemAdded = "KitchenOrderItemAdded";
    public const string OrderStatusChanged = "KitchenOrderStatusChanged";
    public const string OrderCanceled = "KitchenOrderCanceled";
    public const string OrderDelivered = "KitchenOrderDelivered";
    public const string ServiceRequestCreated = "KitchenServiceRequestCreated";
    public const string ServiceRequestUpdated = "KitchenServiceRequestUpdated";
}