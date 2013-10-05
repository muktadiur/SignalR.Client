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
            Action<string, string> act = (id, message) => Show(id, message);
            client.Join(name, act);
            
            while (msg != "quit")
            {
                msg = Console.ReadLine();
                new Thread(delegate()
                {
                    for (int i = 0; i < 100; i++)
                    {
                        client.Invoke(name, i.ToString());
                    }
                }).Start();


                
            }
            
        }
        
        public static string Show(string name, string msg) {
            var message = string.Format("{0} | {1} | ", name, msg);
            Console.WriteLine(message);
            return message;
        }
    }
}
