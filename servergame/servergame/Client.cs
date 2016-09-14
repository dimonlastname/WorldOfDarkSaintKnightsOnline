using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using MySql.Data.MySqlClient;
namespace servergame
{
    public class Client //: IDisposable
    {
        //debug
        float h = 0f;
        float v = 0f;
        //player
        public int id;                                      //user id   
        public Character character;// = new Character();
        //system
        public NetworkStream s;
        public Thread thread;
        static Sql sql;
        public List<Client> OnlineList;
        string firstRequest;
        Thread innerThread;

        TcpClient tcp;
        Request action;
        bool first = true;
        public Client(TcpClient Client, Sql sqlconn, ref List<Client> list, string req)
        {
            tcp = Client;
            s = Client.GetStream();
            //thread = tr;
            sql = sqlconn;
            OnlineList = list;
            firstRequest = req;
            ClientStarter();
        }
        ~Client()
        {      
        }
        public void CharMove(string direction)
        {
            float d = 0f;
           
            switch (direction)
            {
                case "1": //left
                    h -=0.1f;
                    d = h;
                    direction = "h";
                    break;
                case "2":  //right
                    h+= 0.1f;
                    d = h;
                    direction = "h";
                    break;
                case "3":  //top
                    v+= 0.1f;
                    d = v;
                    direction = "v";
                    break;
                case "4":  //bot
                    v-= 0.1f;
                    d = v;
                    direction = "v";
                    break;
            }
            string hui = "0001" + direction + d.ToString().Replace(",", ".");
            byte[] ServerResponseBytes = Encoding.UTF8.GetBytes(hui);
            s.Write(ServerResponseBytes, 0, ServerResponseBytes.Length);
            Console.WriteLine("[Server] Response to client='" + id + "', msg: '" + hui + "'");
          //  return d;
        }

        public void ClientKiller(Client c)
        {
            short tries = 0;
      
            if (sql.SaveCharacter(c.character) ||  tries > 10)
            {
                c.tcp.Close();
                c.innerThread.Abort();
            //    c.thread.Abort();
                c = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            } else if (tries > 10)
            {
                Console.WriteLine("[Server] [ClientKiller] Unable to save character");
            }
            else
            {
                tries++;
                ClientKiller(c);
            }
                
        }

        private void ClientStarter()
        {
            innerThread = new Thread(ClientListener);
            innerThread.Start();
        }
        private void ClientListener()
        {           
            var buffer = new byte[1024];
            action = new Request(this);
            try
            {
                while (tcp.Client.Connected) //s.CanRead
                {
                    string request;
                    string cType;
                    if (first)
                    {
                        request = firstRequest;
                        first = false;
                    }

                    else
                    {
                        int byteCount = s.Read(buffer, 0, buffer.Length);
                        request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    }
                    Console.WriteLine("[Client] " + request);
                    cType = request.Substring(0, 4);
                    request = request.Substring(4);
                    Commands.Execute(action, this, cType, request);

                }

            }

            catch (ThreadAbortException)
            {
                Console.WriteLine("[Server] Connection lost: --CLIENT '"+id+"' THREAD KILLED--");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] Connection lost: " + e.ToString());
                ClientKiller(this);
            }

        }
        private void SendMsg(string msg)
        {
            byte[] ServerResponseBytes = Encoding.UTF8.GetBytes(msg);
            s.Write(ServerResponseBytes, 0, ServerResponseBytes.Length);
        }

    }
}
