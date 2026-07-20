using System.Security.Claims;
using dishmade.application.Abstractions.Realtime;
using dishmade.domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace dishmade.api.Hubs;

[Authorize(Roles = Roles.Client)]
public sealed class KitchenHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var restaurantIdValue = Context.User?.FindFirstValue("restaurant_id");

        if (!Guid.TryParse(restaurantIdValue, out var restaurantId))
        {
            Context.Abort();
            return;
        }

        var groupName = KitchenRealtimeGroups.Restaurant(restaurantId);

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            groupName);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var restaurantIdValue = Context.User?.FindFirstValue("restaurant_id");

        if (Guid.TryParse(restaurantIdValue, out var restaurantId))
        {
            var groupName = KitchenRealtimeGroups.Restaurant(restaurantId);

            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                groupName);
        }

        await base.OnDisconnectedAsync(exception);
    }
}