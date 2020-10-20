using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Omilab.Net
{
    //To check if you're connected or not:
    //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
    //AddressFamily.InterNetworkV6 para ipv6
    //AddressFamily.InterNetwork para ipv4
    //https://stackoverflow.com/questions/6803073/get-local-ip-address

    public class TCPServer
    {
        #region "Variables"
        public Dictionary<EndPoint, TCPConnection> ClientSockets { get; set; }
        protected Socket serverSocket;
        public bool IsRunning { get; set; }
        #endregion
               
        #region "Events & Delegates"
        public delegate void OnStartDelegate(string serverIP, int port);
        public event OnStartDelegate OnStart;

        public delegate void OnclientConnectedDelegate(EndPoint clientEndPoint);
        public event OnclientConnectedDelegate OnClientConnected;

        public delegate void OnReceiveDelegate(EndPoint clientEndPoint, string requestData);
        public event OnReceiveDelegate OnReceive;

        public delegate void OnSendDelegate(EndPoint clientEndPoint, string responseData);
        public event OnSendDelegate OnSend;

        public delegate void OnStopDelegate();
        public event OnStopDelegate OnStop;

        public delegate void OnExceptionDelegate(Exception ex);
        public event OnExceptionDelegate OnException;

        public delegate void OnClientCloseDelegate(EndPoint clientEndPoint);
        public event OnClientCloseDelegate OnClientClose;

        public delegate void OnErrorDelegate(string msg);
        public event OnErrorDelegate OnError;


        #endregion               

        #region "Constructor"

        public TCPServer()
        {
            //Running = false;
            // clientSockets = new Dictionary<EndPoint, Socket>();            
            //buffer = new byte[1024];
        }
        #endregion

        #region "Accept Connections"


        public void Start(int port)
        {
            string ipaddress = this.GetLocalIPAddress();

            if (ipaddress != "")
                Start(ipaddress, port);
            else
                OnError?.Invoke("Error: Server can't detect Ip Address.");
            
        }

        public void Start(string ipaddress, int port)
        {

            IsRunning = false;
            ClientSockets = new Dictionary<EndPoint, TCPConnection>();
            serverSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);
            IsRunning = true;

            OnStart?.Invoke(ipaddress, port); //esto significa (if(OnStart != null) OnStart();

            IPAddress address = IPAddress.Parse(ipaddress);
            serverSocket.Bind(new IPEndPoint(address, port));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private void AcceptCallBack(IAsyncResult asyncResult)
        {
            if (!IsRunning)
                return;

            TCPConnection client = new TCPConnection();

            client.Socket = serverSocket.EndAccept(asyncResult);

            if (!client.Socket.Connected)
                return;


            ClientSockets.Add(client.Socket.RemoteEndPoint, client);

            OnClientConnected?.Invoke(client.Socket.RemoteEndPoint);

            serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), client);
        }
        #endregion

        #region "Receive Data from Clients"
        public void WaitData(EndPoint clientEndPoint)
        {
            if (!ClientSockets.ContainsKey(clientEndPoint))
                return;

            TCPConnection client = ClientSockets[clientEndPoint];

            if (!client.Socket.Connected)
                return;


            try
            {
                client.Socket.BeginReceive(client.ReceiveBuffer, 0, client.ReceiveBuffer.Length,
                                SocketFlags.None,
                                new AsyncCallback(ReceiveCallBack), client);
            }
            catch (Exception ex)
            {
                //aqui es donde me da error constantemente
                OnException?.Invoke(ex); //Server Exception: An existing connection was forcibly closed by the remote host

            }



        }
        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            //Socket socket = (Socket)asyncResult.AsyncState;
            TCPConnection client = (TCPConnection)asyncResult.AsyncState;

            if (!client.Socket.Connected)
                return;


            int receiveBytes = 0;

            try
            {
                receiveBytes = client.Socket.EndReceive(asyncResult);
            }
            catch (SocketException sex)
            {
                if (sex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    OnClientClose?.Invoke(client.Socket.RemoteEndPoint);
                    ClientSockets.Remove(client.Socket.RemoteEndPoint);
                }
                else
                {
                    OnException?.Invoke( sex);
                }
            }
            catch (Exception ex)
            {

                OnException?.Invoke( ex);
                return;
            }



            if (receiveBytes == 0)
                return;


            byte[] receiveBuffer = new byte[receiveBytes];
            Array.Copy(client.ReceiveBuffer, receiveBuffer, receiveBytes);

            string receiveData = Encoding.ASCII.GetString(receiveBuffer);

            try
            {
                OnReceive?.Invoke(client.Socket.RemoteEndPoint, receiveData);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
            }

        }
        #endregion             

        private string responseData = "";

        public void SendData(EndPoint clientEndPoint, string data)
        {
            responseData = data;

            if (!ClientSockets.ContainsKey(clientEndPoint))
                return;

            if (data == "")
                return;


            //Socket socket = ClientSockets[clientEndPoint];
            TCPConnection client = ClientSockets[clientEndPoint];

            if (!client.Socket.Connected)
                return;


            //byte[] sendBufer = Encoding.ASCII.GetBytes(data);
            client.SendBufer = Encoding.ASCII.GetBytes(data);

            try
            {
                client.Socket.BeginSend(client.SendBufer, 0, client.SendBufer.Length,
                             SocketFlags.None,
                             new AsyncCallback(SendCallBack), client);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
            }

        }

        private void SendCallBack(IAsyncResult asyncResult)
        {
            //Socket socket = (Socket)asyncResult.AsyncState;
            TCPConnection client = (TCPConnection)asyncResult.AsyncState;

            if (!client.Socket.Connected)
                return;

            client.Socket.EndSend(asyncResult);

            //if(OnSend != null)
            OnSend?.Invoke(client.Socket.RemoteEndPoint, responseData);
        }

        #region "Stop the server"
        public void Stop()
        {
            IsRunning = false;
            //serverSocket.Shutdown(SocketShutdown.Both);


            foreach (KeyValuePair<EndPoint, TCPConnection> kvp in ClientSockets)
            {
                kvp.Value.Socket.Close();
            }


            serverSocket.Close();
            ClientSockets.Clear();

            OnStop?.Invoke();
        }
        #endregion


        
        public string GetLocalIPAddress()
        {
            string ipAddr = "";

            var host = Dns.GetHostEntry(Dns.GetHostName());
            try
            {
                foreach (var ip in host.AddressList)
                {
                    //AddressFamily.InterNetworkV6 para ipv6
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddr = ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
                return "";
            }


            return ipAddr;
        }

    }//end class

}//end namespace
