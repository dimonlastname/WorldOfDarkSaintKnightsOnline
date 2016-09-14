using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace servergame
{
    class GameServer
    {
        Sql sql = new Sql();
        TcpListener tcpListener;
        List<Client> OnlineList = new List<Client>();
       // Thread th;
        private void  ClientThread(Object Client)
        {
            TcpClient client = (TcpClient)Client;
            ClientCreator(client);                  //connections filter            
        }
        private void ClientCreator(TcpClient client)                                    //fake client checker for not create a Client class;
        {
            try
            {          
                var buffer = new byte[1130];
                var byteCount = client.GetStream().Read(buffer, 0, buffer.Length);
                var request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                var cType = request.Substring(0, 4);
                request = request.Substring(4);
                Console.WriteLine("[Preserver] " + cType + request);
                if (cType == "1000")                                                        // if game client conecting
                {
                    Client user = new Client(client, sql, ref OnlineList, cType+request);   //create it
                    OnlineList.Add(user);
                }
                else                                // if someone left client
                {   
                    client.Close();                 //disconnect&kill this shit
                    client = null;
                    //th.Abort();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Preserver] Error: " + e.ToString());
            }


        }
        private GameServer() //main
        {
            tcpListener = TcpListener.Create(1235);
            tcpListener.Start();
      //      sql.Connect();
            while (true)
            {
                TcpClient Client = tcpListener.AcceptTcpClient();                       //await connections
                ClientThread(Client);
          /*      th = new Thread(new ParameterizedThreadStart(ClientThread));   //go new thread for new one connect
                try
                {
                    th.Start(Client);
                }
                catch(Exception e)
                {
                    Console.WriteLine("[Preserver] Error: " + e.ToString());
                }*/
            }
        }
        static void Main(string[] args) //start point
        {
            new GameServer();
        }
    }
}
