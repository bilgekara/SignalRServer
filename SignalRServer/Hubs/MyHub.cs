using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalRServer.interfaces;
using SignalRServer.TimerFeatures;

namespace SignalRServer.Hubs
{
    public class MyHub : Hub  // <IMessageClient>
    {

        static List<string> clients = new List<string>();


        public override async Task OnConnectedAsync()
        {
            clients.Add(Context.ConnectionId);
            await Clients.All.SendAsync("clients", clients);
            await Clients.All.SendAsync("userJoined", Context.ConnectionId);
            //await Clients.All.Clients(clients);
            //await Clients.All.UserJoined(Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            clients.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("clients", clients);
            await Clients.All.SendAsync("userLeaved", Context.ConnectionId);
            //await Clients.All.Clients(clients);
            //await Clients.All.UserLeaved(Context.ConnectionId);
        }

        public async Task SendMessageAsync(string usrName, string password, string urlx, string connectionId)
        {
            //#region definition
            //url = urlx;
            //username = usrName;
            //passw = password;
            //#endregion
            await Clients.All.SendAsync("pullMessage", urlx, ".");
            //await _hubContext.Groups.AddToGroupAsync(connId, urlx);
            await Groups.AddToGroupAsync(connectionId, urlx);

        }

        public async Task addGroup(string connectionId, string groupName)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }
        public async Task removeGroup(string connectionId, string groupName)
        {
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
            await Clients.All.SendAsync("pullMessage", groupName, ".");

        }
        public async Task mqtt()
        {
            var timerManager = new TimerManager(() => Clients.All.SendAsync("transferchartdata", "kjj"));

        }
    }
}


