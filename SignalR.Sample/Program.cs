using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SignalR.Client;

namespace SignalR.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var msg = string.Empty;
            Console.Write("Please enter Name : ");
            var name = Console.ReadLine();
            SignaRClient client = SignaRClient.Instance;
            Action<string, string, string, string, string, string, string> act = (a, b, c, d, e, f, g) => Show(a, b, c, d, e, f, g);
            client.Join(name, act);
            client.ThreadSleepTime = 10;
            while (msg != "quit")
            {
                msg = Console.ReadLine();
                new Thread(delegate()
                {
                    for (int i = 0; i < 100; i++)
                    {
                        client.Invoke(name, i.ToString(), "a", "b", "c", "d", "e");
                    }
                }).Start();


                new Thread(delegate()
                {
                    for (int i = 100; i < 200; i++)
                    {
                        client.Invoke(name, i.ToString(), "a", "b", "c", "d", "e");
                    }
                }).Start();
            }
            
        }
        
        public static string Show(string name, string msg, string a, string b, string c, string d,string e) {
            var message = string.Format("{0} | {1} |  {2} | {3} | {4} | {5} | {6}", name, msg, a, b, c, d, e);
            Console.WriteLine(message);
            return message;
        }
    }
}
