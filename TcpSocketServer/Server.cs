using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TcpSocketServer
{

    interface IServer
    {
        string Run();
        string Listen();
        void SendResponse(string message);
    }

    internal class Server:IServer
    {
        private Socket listenSocket;
        private readonly IPEndPoint ipPoint;
        private Socket handler;
        private byte[] buffer;

        public bool IsWorking { get; private set; }

        public Server(string ip, int port)
        {
            ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public Server(string ip)
        {
            ipPoint = new IPEndPoint(IPAddress.Parse(ip), 8005);
        }

        public string Run()
        {
            var message = "";
            try
            {
                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                message = "Server is working";
                this.IsWorking = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        public string Listen()
        {
            handler = listenSocket.Accept();
            var dataBuilder = new StringBuilder();
            int bytes=0;
            buffer = new byte[256];
            do
            {
                bytes = handler.Receive(buffer);
                dataBuilder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
            } while (handler.Available > 0);

            return $"\nIP: {handler.RemoteEndPoint}\n" +
                $"Time: {DateTime.Now:H:mm:ss}\n" +
                $"Message: {dataBuilder}\n" +
                $"{new string('_', 10)}";
        }

        public void SendResponse(string message)
        {
            buffer = Encoding.Unicode.GetBytes(message);
            handler.Send(buffer);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        public void CloseConnection()
        {
            listenSocket.Shutdown(SocketShutdown.Both);
            listenSocket.Close();
        }
    }
}
