using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            Console.CancelKeyPress += (x, y) =>
            {
                quit = true;
            };

            string gatewayUrl = Gateway.GetUrl();
            Console.WriteLine("Gateway: " + gatewayUrl);

            while (!quit)
            {
            }
        }
    }
}
