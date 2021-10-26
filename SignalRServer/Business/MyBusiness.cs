using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using SignalRServer.Hubs;
using SignalRServer.TimerFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Business
{
    public class MyBusiness
    {

        #region definition
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
            var options = new MqttClientOptionsBuilder().WithTcpServer("mqtt.bakelor.com").WithCredentials("drony", "drony").WithCleanSession().Build();
            var factory = new MqttFactory();

            mqttClient = factory.CreateMqttClient();
            mqttClient.UseConnectedHandler(ConnectionHandler);
            mqttClient.UseApplicationMessageReceivedHandler(MessageHandler);

            var ct = new CancellationToken();
            var result = mqttClient.ConnectAsync(options, ct).GetAwaiter().GetResult();
        }

        static Task ConnectionHandler(MQTTnet.Client.Connecting.MqttClientConnectedEventArgs arg)
        {
            mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter("bakelor/pr14/prod/+/s").Build()).GetAwaiter().GetResult();
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
        readonly IHubContext<MyHub> _hubContext;

        public MyBusiness(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task SendMessageAsync(string usrName, string password, string urlx) 
        {
            #region definition
            url = urlx;
            username = usrName;
            passw = password;
            #endregion
            await _hubContext.Clients.All.SendAsync("pullMessage", urlx, ".");
            //await _hubContext.Groups.AddToGroupAsync(connId, urlx);
        }

        public async Task SendTopicAsync()
        {
            ConnectClient();
            await _hubContext.Clients.All.SendAsync("transferchartdata", topics);
            topics.Clear();

        }

        //public async Task removeGroup(string connectionId, string groupName)
        //{
        //    await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
        //}

    }
}