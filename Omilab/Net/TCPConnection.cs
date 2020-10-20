using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Omilab.Net
{
    public class TCPConnection
    {
        public Socket Socket { get; set; }
        public byte[] ReceiveBuffer { get; set; }
        public byte[] SendBufer { get; set; }
        //public int ReceiveBytes { get; set; }
        public TCPConnection()
        {
            // ReceiveBytes = 0;
            ReceiveBuffer = new byte[1024];
            Socket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);
        }
    }
}
