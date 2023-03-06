using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Server
{
    public class ChatHub : Hub
    {
        private static List<string> list_UserName = new List<string>();

        public override async Task OnConnectedAsync()
        {
            var msg = $"New comer is add, ConnectionId= {Context.ConnectionId}, UserName= {Context}";
            Console.WriteLine(msg);
            await Clients.All.SendAsync(msg);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var msg = $"ConnectionId: {Context.ConnectionId} is disconnect!";
            Console.WriteLine(msg);
            await Clients.All.SendAsync(msg);
        }

        public async Task SendMsg(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}
