using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading;
using MQTTnet.Client.Subscribing;
using MQTTnet;
using SignalRServer.interfaces;

namespace SignalRServer.Hubs
{
    public class MyHub : Hub  // <IMessageClient>
    {

        #region definition

        static List<string> clients = new List<string>();

        static List<string> topics = new List<string>();

        static List<string> grup = new List<string>();
        public static string url = "";
        public static string username = "";
        public static string passw = "";

        #endregion

        // bakelor/pr14/prod/+/s
        // "drony"

        #region MQTT

        static IMqttClient mqttClient = null;
        public static string topic = "";

        public static void ConnectClient()
        {
            var options = new MqttClientOptionsBuilder().WithTcpServer("mqtt.bakelor.com").WithCredentials(username, passw).WithCleanSession().Build();
            var factory = new MqttFactory();

            mqttClient = factory.CreateMqttClient();
            mqttClient.UseConnectedHandler(ConnectionHandler);
            mqttClient.UseApplicationMessageReceivedHandler(MessageHandler);

            var ct = new CancellationToken();
            var result = mqttClient.ConnectAsync(options, ct).GetAwaiter().GetResult();
        }

        static Task ConnectionHandler(MQTTnet.Client.Connecting.MqttClientConnectedEventArgs arg)
        {
            mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(url).Build()).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }

        static async Task MessageHandler(MqttApplicationMessageReceivedEventArgs e)
        {
            await Task.Run(() =>
           {
               topics.Add($"+ Topic = {e.ApplicationMessage.Topic}");
           });
        }

        #endregion

        

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

        public async Task SendMessageAsync(string usrName, string password) // burasi silincek
        {
            #region definition
            //url = urlx;
            username = usrName;
            passw = password; 
            #endregion

            //ConnectClient();
            //await Clients.All.SendAsync("pullMessage", topics, "Topics");
            topics.Clear();
        }


        public async Task addGroup(string connectionId, string groupName) 
        {
            grup.Add(groupName);
            await Clients.All.SendAsync("pullMessage", "grup", "oldu mu", "birdaha");

            //await Groups.AddToGroupAsync(connectionId, groupName);
        }
    }
}
