using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Threading;
namespace TestProj
{
    class Key
    {
        public static string pass;
        public Key()
        {
            pass = get_new_key();
        }

        private string charDelete(string a)
        {
            a = a.Trim(new char[] { '\r', '\n', 'О', '╩', '©' });
            return a;
        }



        private string get_new_key() {
            const int PORT = 2013;
            const string HOST = "88.212.241.115";
            IPAddress ip = IPAddress.Parse(HOST);
            TcpClient client = new TcpClient();
            Client MyClass = new Client();
            client.Connect(HOST, PORT);
            MyClass.sendMsg("Register\n", client.GetStream());
            Key.pass = MyClass.getMsg(client, client.GetStream());
            
            return charDelete(Key.pass);
        }

        public void Start()
        {
            while (true)
            {
                Thread.Sleep(15000);
                checkKey();
            }
        }

        private void checkKey()
        {
            Client MyClass = new Client();
            const int PORT = 2013;
            const string HOST = "88.212.241.115";
            IPAddress ip = IPAddress.Parse(HOST);
            
            while (true)
            {
                string respounce = null;
                int[] param = null;
                do
                {
                    TcpClient client = new TcpClient();
                    client.Connect(HOST, PORT);
                    NetworkStream stream = client.GetStream();

                    
                    MyClass.sendMsg(pass + "|1\n", stream);
                    respounce = MyClass.getMsg(client, stream);
                    if (respounce != null)
                    {
                        param = MyClass.check_right(respounce);
                        if (param[0] == 1)
                            respounce = null;
                    }
                } while (respounce == null);
                if (param[2] == 1) Thread.Sleep(20000);
                if (param[1] == 1) { Key.pass = get_new_key(); break; }
                if (param[3] == 1) { break; }
            }

        }

    }
}
