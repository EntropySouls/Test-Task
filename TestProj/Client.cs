using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TestProj
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
            try
            {
                Byte[] msg = Encoding.ASCII.GetBytes(a);
                stream.Write(msg, 0, msg.Length);
                stream.Flush();
            }
            catch {
                Thread.Sleep(3000);
            }

        }
        public string Main() {
            const int PORT = 2013;
            const string HOST = "88.212.241.115";
            IPAddress ip = IPAddress.Parse(HOST);
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(ip, PORT);
                NetworkStream stream = client.GetStream();
                string nubmer = Key.pass + "|" + this.i.ToString() + "\n";
                sendMsg(nubmer , stream);
                string alldata = getMsg(client, stream);
                return alldata;
            }
            catch (Exception ex)
            {
                Thread.Sleep(300);
            }
            return null;
        }
        public int GetNumb() {
            try
            {
                string data = null;
                do
                {
                    data = Main();
                    if (data != null) { 
                        int[] param = check_right(data);  
                        if(param[0] == 1 || param[1] == 1) 
                            data = null; 
                        if (param[1] == 1) 
                            Thread.Sleep(5000); }
                    
                } while (data == null);
                string result = new string(data.Where(x => char.IsDigit(x) == true).ToArray());
                int numb = Convert.ToInt32(result);
                Console.WriteLine(numb + ":" + this.i);
                return numb;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString() +"\t:" + i);
            }
            return -1;
        }

        public int[] check_right(string alldata)
        {
            int[] param = new int[4];
            for (int j = 0; j < param.Length; j++) param[j] = 0;
            Regex regex = new Regex(@"О╩.О╩.О╩.О╩.О╩.О╩.");
            Regex regex2 = new Regex("Key");
            Regex regex1 = new Regex("Rate");
            Regex regex3 = new Regex(@"\d");
            if (regex.IsMatch(alldata) || (alldata[0] == '\n')) { param[0] = 1; }
            if (regex2.IsMatch(alldata)) {  param[1] = 1; }
            if (regex1.IsMatch(alldata)) { param[2] = 1; }
            if (regex3.IsMatch(alldata)) { param[3] = 1; }
            return param; 
        }

        public string getMsg(TcpClient client, NetworkStream stream) {
            try
            {
                int Count_of_times_down = 1;
                string alldata = "";
                
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
                        stream.Close();
                        return alldata;
                    }
                }
            } catch {
                Thread.Sleep(300);
                return null;
            }
            return null;
        }

    }
}
