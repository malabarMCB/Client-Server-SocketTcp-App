using System;

namespace TcpSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter server`s IP address");
            var serverIP = Console.ReadLine();
            var client = new Client(serverIP);

            while (true)
            {
                Console.Write("Enter your message: ");
                var message = Console.ReadLine();

                try
                {
                    client.Run();
                    client.SendMessage(message);
                    var response = client.GetResponse();
                    Console.WriteLine(response);
                }
                catch (ClientException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
