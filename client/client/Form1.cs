using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace client
{
    public partial class Form1 : Form
    {
        
        IPHostEntry ipHostInfo;
        IPAddress ipAddress;
        IPEndPoint remoteEP;
        Socket ssender;
        public Form1()
        {
            InitializeComponent();
            
        }
        private void SendMsg(int oper)
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[2048];
            string cType;
            string cMsg = "";
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                ipHostInfo = Dns.Resolve(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
                int port;
                switch (oper)
                {
                    case 0: //login server
                        port = 1234;
                        break;
                    default: //game server
                        port = 1235;
                        break;

                }
                remoteEP = new IPEndPoint(ipAddress, port);
                // Create a TCP/IP  socket.
                ssender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    ssender.Connect(remoteEP);
                    //         Console.WriteLine("Socket connected to {0}", ssender.RemoteEndPoint.ToString());
                    // Encode the data string into a byte array.
                    
                    byte[] msg;
                    int bytesSent;
                    switch (oper)
                    {
                        case 0: // login (command 10)
                            cMsg = textBox1.Text + "@" + textBox2.Text;
                            msg = Encoding.ASCII.GetBytes("10" + cMsg);
                            bytesSent = ssender.Send(msg);
                            break;
                        case 1: //login command 1000 (button toGameServer)
                            // cMsg = "123456";  //auth_id
                            cMsg = textBox_auth_key.Text;  //auth_id
                            msg = Encoding.ASCII.GetBytes("1000" + cMsg);
                            bytesSent = ssender.Send(msg);
                            break;
                        case 2: //go to game command 1001 (button toGameWorld) char slot '1';
                            cMsg = "1";  //character slot
                            msg = Encoding.ASCII.GetBytes("1001" + cMsg);
                            bytesSent = ssender.Send(msg);
                            break;

                    }
                    // Send the data through the socket.
                    while (true)
                    {
                        // Receive the response from the remote device.
                        int bytesRec = ssender.Receive(bytes);
                        cType = Encoding.ASCII.GetString(bytes, 0, 2);
                        cMsg = Encoding.ASCII.GetString(bytes, 2, bytesRec - 2);

                        if (true)//cType == "11")
                        {

                            this.Invoke((MethodInvoker)delegate ()
                            {
                                richTextBox1.AppendText(cType+cMsg + "\n");
                                richTextBox1.AppendText("bytesRec: " + bytesRec.ToString() + "\n");
                            });
                            //break;

                        }
                        if (cMsg.IndexOf("<EOF>") > -1) break;
                    }
                    /*          });
                              MyThread1.Start();
                              MyThread1.Join();*/

                    // Release the socket.
                    //   ssender.Shutdown(SocketShutdown.Both);
                    //       ssender.Close();


                }
                catch (ArgumentNullException ane)
                {
                }
                catch (SocketException se)
                {
                }
                catch (Exception e)
                {
                }

                }
                catch (Exception e)
                {
                }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread MyThread1 = new System.Threading.Thread(delegate ()
            { SendMsg(0); });
            MyThread1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Threading.Thread MyThread1 = new System.Threading.Thread(delegate ()
            { SendMsg(1); });
            MyThread1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
      /*      System.Threading.Thread MyThread1 = new System.Threading.Thread(delegate ()
            {*/
                var cMsg = "1";  //character slot
                var msg = Encoding.ASCII.GetBytes("1020" + cMsg);
                ssender.Send(msg);

/*
            });
            MyThread1.Start();*/
        }

        private void btnCreateChar_Click(object sender, EventArgs e)
        {
                //create character "race,name,sex,face";
                var cMsg = "1$"+ tbNickName.Text + "$1${1,1,1}";  //character slot
                var msg = Encoding.ASCII.GetBytes("1010" + cMsg);
                ssender.Send(msg);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
