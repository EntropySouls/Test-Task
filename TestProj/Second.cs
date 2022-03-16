using System.Threading.Tasks;
using System;
using System.Threading;

namespace TestProj
{
    class Second
    {
        static void Main(string[] args)
        {
            var mc = new Second();
            int[] answer_mass = new int[2018];
            
            mc.recur(answer_mass);
            Console.ReadKey();
            
        }

        private void recur(int[] answer_mass)
        {
            try
            {
                Key key = new Key();
                
                int k = 11;
                Task[] active_tasks = new Task[k];

                for (int i = 0; i < k; i++)
                {
                    active_tasks[i] = new Task(() =>
                        {
                            Client client = new Client(i + 1);
                            answer_mass[i] = client.GetNumb();
                        });
                    active_tasks[i].Start();
                    Thread.Sleep(100);
                }

                Task Check_Key = null;

                int j = k;
                Check_Key = new Task(() =>
                {
                    Thread.Sleep(10000);
                    key.Start();
                });
                Check_Key.Start();
                while (j < 2018)
                {
                    for (int i = 0; i < k; i++)
                    {
                        if (active_tasks[i].IsCompletedSuccessfully && j < 2017)
                        {
                            active_tasks[i] = new Task(() =>
                                {
                                    Client client = new Client(j+1);
                                    answer_mass[j] = client.GetNumb();
                                });
                            j++;
                            active_tasks[i].Start();
                            Thread.Sleep(300);
                        }
                    }
                    Thread.Sleep(5000);
                       
                }
                var t = Task.WhenAll(active_tasks);
                t.Wait();
                
                Array.Sort(answer_mass);
                double Solve = (double)((answer_mass[1008] + answer_mass[1009]) / (double)2);
                Console.WriteLine(Solve);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        
    }

}