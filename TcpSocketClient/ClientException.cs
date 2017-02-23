using System;

namespace TcpSocketClient
{
    class ClientException:Exception
    {
        public override string Message => "Incorrect IP or port";
    }
}
