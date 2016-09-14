using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servergame
{
    public class Request
    {
        private Client c;
        private Sql sql = new Sql();
        public Request(Client client)
        {
            c = client;
        }
        private void SendMsg(string msg)
        {
            byte[] ServerResponseBytes = Encoding.UTF8.GetBytes(msg);
            c.s.Write(ServerResponseBytes, 0, ServerResponseBytes.Length);
        }
        private void CheckConnected(int id)  // if id already connected
        {
            for (int i = 0; i < c.OnlineList.Count; i++)
            {
                Client cli = c.OnlineList[i];
                if ((cli.id == id || cli.id == 0) && (cli != c))
                {
                    c.ClientKiller(c.OnlineList[i]);          //kill last connected thread/object
                    c.OnlineList.Remove(c.OnlineList[i]);
                    Console.WriteLine("[Server] Client id =" + id + " has been killed");
                    // break;
                }

            }
        }
        public void GameClientConnecting(string request)
        {
            SW sw = new SW();
            string serverResponse = "";
           // int id = 0;
            Console.WriteLine("[Server] Client auth_id: " + request);
            string userid;
            /* lock (this)
             {*/
            sql.Connect();
            userid = sql.GetIdByAuth_id(request);           //get user id by auth_key
            // }

            if (userid != null)                             //if user found by auth_key
            {
                c.id = Convert.ToInt32(userid);               
                CheckConnected(c.id);                         //check user for already online
                string CharsPack = sql.GetCharacters(c.id);   //get chars list
                if (CharsPack != null)                      //if have at least 1 char
                    serverResponse = "1001" + CharsPack;    //user ok and chars list
                else
                    serverResponse = "1001";                //user ok bit no chars
            }
            else
            {
                serverResponse = "1000";                   //user fail(invalid auth_key)
            }
            SendMsg(serverResponse);
            Console.WriteLine("[Server] Response to client='" + c.id + "', msg: '" + serverResponse + "', " + sw.stop());
            //return c.id;
            sql.Close();
        }
        public void GameStarting(string char_slot)
        {
            c.character = new Character();
           // sql.LoadCharacter(ref c.character);
            Console.WriteLine("[Server] game is starting, okay?");
            //Console.WriteLine("[Server] " + ti.ToString());
        }
        public void CreateCharacter(string request)
        {
            SW sw = new SW();
            sql.Connect();
            if (sql.CreateCharacter(request, c.id) > 0)
            {
                string CharsPack = sql.GetCharacters(c.id);
                SendMsg("10111" + CharsPack);
            }
            else if (sql.CreateCharacter(request, c.id) == 0)
            {
                string CharsPack = sql.GetCharacters(c.id);
                SendMsg("101101"); //already exist
            }

            else SendMsg("101100"); //another error
            sql.Close();
            Console.WriteLine("[Server] [CreateCharacter] time: " + sw.stop());
        }
    }
}

