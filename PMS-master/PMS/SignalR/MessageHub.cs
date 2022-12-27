using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.SignalR
{
    public class MessageHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId,group);
        }

        //public async Task SendMessage(string sender, string message)
        //{
        //    await Clients.All.SendAsync("SendMessage", sender, message);
        //}

        public Task SendMessageToGroup(string groupname, string sender, string message)
        {
            return Clients.Group(groupname).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
