using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TcpSocketClient
{

    interface IClient
    {
        void Run();
        void SendMessage(string message);
        string GetResponse();
    }

    class Client:IClient
    {
        private Socket socket;
        private readonly IPEndPoint ipPoint;

        public Client(string serverIP, int port)
        {
            ipPoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
        }

        public Client(string serverIP)
        {
            ipPoint = new IPEndPoint(IPAddress.Parse(serverIP), 8005);
        }

        public void Run()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
            }
            catch (SocketException)
            {
                throw new ClientException();
            }
        }

        public void SendMessage(string message)
        {
            var data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
        }

        public string GetResponse()
        {
            var buffer = new byte[256];
            var dataBuilder = new StringBuilder();
            var bytes = 0;

            do
            {
                bytes = socket.Receive(buffer);
                dataBuilder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));

            } while (socket.Available > 0);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            return "Response from server: " + dataBuilder;
        }
    }
}
