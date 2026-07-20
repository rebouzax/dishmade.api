namespace dishmade.application.Abstractions.Realtime;

public static class KitchenRealtimeGroups
{
    public static string Restaurant(Guid restaurantId)
    {
        return $"restaurant:{restaurantId}";
    }
}