using System;
using SignalRClientLibrary.Clients;
using System.Runtime.InteropServices;

namespace DemoApp
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        static int _x, _y;

        private static GPSClient _client;

        public struct POINT
        {
            public int X;
            public int Y;
        }

        static void Main(string[] args)
        {

            _client = new GPSClient();

            Console.WriteLine("Press any key to exit.");

            while (!Console.KeyAvailable)
            {
                ShowMousePosition();
            }

        }

        private static void ShowMousePosition()
        {
            POINT point;
            if (GetCursorPos(out point) && point.X != _x && point.Y != _y)
            {             
                _x = point.X;
                _y = point.Y;
                _client.SendCoordinatesToHubAsync(_x, _y).GetAwaiter().GetResult();
            }
        }
    }
}
