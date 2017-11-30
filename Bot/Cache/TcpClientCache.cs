using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Bot
{
    public static class TcpClientCache
    {
        public static TcpClient Cache { get; set; }
    }
}
