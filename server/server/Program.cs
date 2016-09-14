using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace server
{
    class Server
    {
        private async void StartListener()
        {
            var tcpListener = TcpListener.Create(1234);
            tcpListener.Start();
            //var tcpClient = await tcpListener.AcceptTcpClientAsync();
            //Console.WriteLine("[Server] Client has connected");
            while (true) // тут какое-то разумное условие выхода
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                processClientTearOff(tcpClient); // await не нужен
            }
        }
        async Task processClientTearOff(TcpClient c)
        {
            using (var client = new Client(c))
                await client.ProcessAsync();
        }
        public Server() //main
        {
            StartListener();
            Console.ReadLine();
            /* // Создаем "слушателя" для указанного порта
            var tcpListener = TcpListener.Create(1234);
            tcpListener.Start();
            var tcpClient = await tcpListener.AcceptTcpClientAsync();
            processClientTearOff(tcpClient); // await не нужен*/

        }

        

        static void Main(string[] args)
        {
            new Server();
        }
    }
}
