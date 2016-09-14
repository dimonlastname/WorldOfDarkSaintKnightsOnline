using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MySql.Data.Entity;
using MySql.Fabric;
using MySql.Web;
using MySql.Data.MySqlClient;



namespace server
{
    class Client : IDisposable
    {
        NetworkStream s;
        Sql sql;
        public Client(TcpClient Client, Sql sqlconn)
        {
            s = Client.GetStream();
            sql = sqlconn;
        }
        public void Dispose()
        {
            s.Dispose();
        }
        
        public async Task ProcessAsync()
        {
            var buffer = new byte[1024];
            var byteCount = await s.ReadAsync(buffer, 0, buffer.Length);
            var request = Encoding.UTF8.GetString(buffer, 0, byteCount); //5 is '<eof>'
            var cType = request.Substring(0, 2);
            request = request.Substring(2);
            string ServerResponseString = "";
            Console.WriteLine("[Server] Client wrote: " + request);
            switch (cType)
            {
                case "10": //is loging
                    var user = request.Split('@');
                    Console.WriteLine("[Server] Client login: " + user[0] + ", password: " + user[1]);
                    string req = sql.sqlrequest(user);
                    if (req != null)
                    {
                        ServerResponseString = "11" + req;
                    }
                    else {
                        ServerResponseString = "10";
                    }
                    break;
                case "11":
                    ServerResponseString = "11";
                    break;
                default:
                    Console.WriteLine("[Server] Fake client connect, message: \n" + Encoding.UTF8.GetString(buffer, 0, byteCount));
                    ServerResponseString = "1000";
                    break;
            }
            byte[] ServerResponseBytes = Encoding.UTF8.GetBytes(ServerResponseString);
            await s.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
            Console.WriteLine("[Server] Response has been written");
        }
    }
}
