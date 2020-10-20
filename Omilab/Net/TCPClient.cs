using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Omilab.Net
{
    public class TCPClient
    {
        #region "Private Variables"        
        private Socket clientSocket;
        private byte[] buffer;
        #endregion

        #region "Events"

        public delegate void OnConnectDelegate(EndPoint serverEndPoint);
        public event OnConnectDelegate OnConnect;

        public delegate void OnReceiveDelegate(EndPoint serverEndPoint, string receiveData);
        public event OnReceiveDelegate OnReceive;

        public delegate void OnSendDelegate(EndPoint serverEndPoint);
        public event OnSendDelegate OnSend;

        public delegate void OnCloseDelegate();
        public event OnCloseDelegate OnClose;

        public delegate void OnExceptionDelegate(Exception ex);
        public event OnExceptionDelegate OnException;

        #endregion

        #region "Constructor"

        public TCPClient()
        {

            buffer = new byte[1024];
        }
        #endregion

        #region "Connects"
        public void Connect(string ipaddress, int port)
        {
            IPAddress address = IPAddress.Parse(ipaddress);

            clientSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);


            clientSocket.BeginConnect(address, port, new AsyncCallback(ConnectCallBack), clientSocket);


        }

        public void ConnectCallBack(IAsyncResult asyncResult)
        {

            try
            {
                clientSocket.EndConnect(asyncResult);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
                return;
            }

            OnConnect?.Invoke(clientSocket.RemoteEndPoint);

            WaitData();
        }
        #endregion

        #region "Receive Data from Server"
        private void WaitData()
        {
            clientSocket.BeginReceive(buffer, 0, buffer.Length,
                                SocketFlags.None,
                                new AsyncCallback(ReceiveCallBack), clientSocket);

        }
        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;
            int receiveBytes = 0;

            try
            {
                receiveBytes = socket.EndReceive(asyncResult);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
                return;
            }

            byte[] receiveBuffer = new byte[receiveBytes];
            Array.Copy(buffer, receiveBuffer, receiveBytes);

            string receiveData = Encoding.ASCII.GetString(receiveBuffer);

            try
            {
                OnReceive?.Invoke(socket.RemoteEndPoint, receiveData);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
                return;
            }

            WaitData();
        }
        #endregion

        #region "Send Data"
        public void SendData(string data)
        {
            byte[] sendBufer = Encoding.ASCII.GetBytes(data);

            try
            {
                clientSocket.BeginSend(sendBufer, 0, sendBufer.Length,
                            SocketFlags.None,
                            new AsyncCallback(SendCallBack), clientSocket);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
                return;
            }

        }

        private void SendCallBack(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;
            socket.EndSend(asyncResult);
            OnSend?.Invoke(socket.RemoteEndPoint);
        }

        #endregion

        #region "Disconnect"
        public void Close()
        {
            clientSocket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), clientSocket);
        }
        private void DisconnectCallBack(IAsyncResult asyncResult)
        {
            clientSocket.EndDisconnect(asyncResult);
            OnClose?.Invoke();
        }

        #endregion

    }//end class

}//end namespace

