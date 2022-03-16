using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TestProj2
{
    class Client
    {
        int i;
        public Client()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public Client(int i)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.i = i;
        }
        public void sendMsg(string a, NetworkStream stream)
        {

            Byte[] msg = Encoding.ASCII.GetBytes(a);
            stream.Write(msg, 0, msg.Length);
            stream.Flush();

        }
        private string Main()
        {
            const int PORT = 2013;
            const string HOST = "88.212.241.115";
            IPAddress ip = IPAddress.Parse(HOST);
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(ip, PORT);
                NetworkStream stream = client.GetStream();
                string nubmer = this.i.ToString() + "\n";
                sendMsg(nubmer, stream);
                string alldata = getMsg(client, stream);
                return alldata;
            }
            catch (Exception ex)
            {
                Thread.Sleep(300);
            }
            return null;
        }
        public int GetNumb()
        {
            try
            {
                string data = null;
                do
                {
                    data = Main();

                } while (data == null);
                string result = new string(data.Where(x => char.IsDigit(x) == true).ToArray());
                int numb = Convert.ToInt32(result);
                Console.WriteLine(numb + ":" + this.i);
                return numb;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "\t:" + i);
            }
            return -1;
        }

        public string getMsg(TcpClient client, NetworkStream stream)
        {
            try
            {
                int Count_of_times_down = 1;
                string alldata = "";
                Regex regex = new Regex(@"О╩.О╩.О╩.О╩.О╩.О╩.");
                while (client != null && client.Connected)
                {
                    Byte[] readingData = new Byte[256];
                    String responseData = String.Empty;
                    StringBuilder completeMessage = new StringBuilder();
                    int numberOfBytesRead = 0;
                    if (!stream.DataAvailable)
                    {
                        if (Count_of_times_down > 5) throw new Exception();
                        Thread.Sleep(2500 * Count_of_times_down);
                        Count_of_times_down++;
                    }
                    else Count_of_times_down = 1;
                    do
                    {
                        numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
                        completeMessage.AppendFormat("{0}", Encoding.GetEncoding(20866).GetString(readingData, 0, numberOfBytesRead));
                    }
                    while (stream.DataAvailable);
                    
                    responseData = completeMessage.ToString();
                    alldata += responseData;

                    if (alldata[alldata.Length - 1] == '\n')
                    {
                        if (regex.IsMatch(alldata) || (alldata[0] == '\n')) throw new Exception();
                        stream.Close();
                        return alldata;
                    }

                }
            }
            catch
            {
                Thread.Sleep(300);
                return null;
            }
            return null;
        }

    }
}
