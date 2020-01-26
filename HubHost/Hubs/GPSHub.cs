using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub.Hubs
{
    public class GPSHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendGroupMessage(string user, string message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessage(string connectionId, string user, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId}", "Has Connected: ", groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId}", "Has Disconnected: ", groupName);
        }

        public async Task SendCoordinatesToHubAsync(double latitude, double longitude)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveCoordinates", latitude, longitude);
            //await Clients.All.SendAsync("ReceiveCoordinates", latitude, longitude);
        }
    }
}
