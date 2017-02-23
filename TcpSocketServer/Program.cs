using System;

namespace TcpSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter server`s IP");
            var serverIP = Console.ReadLine();
            var server = new Server(serverIP);

            var serverStatus=server.Run();
            Console.WriteLine(serverStatus);

            while (server.IsWorking)
            {
                var messgae=server.Listen();
                Console.WriteLine(messgae);

                server.SendResponse("Your message has been delivered");
            }

            Console.ReadKey();
        }
    }
}
