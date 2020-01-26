using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClientLibrary.Clients
{
    public class GPSClient
    {
        private HubConnection connection;

        public GPSClient()
        {
            InitializeHub().GetAwaiter().GetResult();
        }

        private async Task InitializeHub() //Jacob ToDo: Add some OnConnected validation?
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44358/gpsHub", options => //Jacob ToDo: probably need some url helper where the base url is fetched from a config file
                {

                })
                .Build();

            #region Subscribe to Hub Events

            connection.Closed += async (error) => //Attempt to reconnect if closed
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<double, double>("ReceiveCoordinates", (latitude, longitude) =>
            {
                var newMessage = $"{latitude}: {longitude}";
                Console.WriteLine(newMessage + "\n");

            });

            #endregion Subscribe to Hub Events

            await connection.StartAsync(new CancellationToken()).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                    return;
                }
                else
                {
                    Console.WriteLine("Connected to Server...\n");

                }
            });
        }

        #region Methods

        public async Task SendCoordinatesToHubAsync(double latitude, double longitude)
        {
            await connection.InvokeAsync("SendCoordinatesToHubAsync", latitude, longitude);
        }

        #endregion Methods
    }
}
