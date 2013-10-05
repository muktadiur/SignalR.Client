using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace SignalR.Client
{
    public class SignaRClient
    {

        private static string endPoint = ConfigurationManager.AppSettings.Get("SignalREndPoint");
        private static string hubProxyName = ConfigurationManager.AppSettings.Get("HubProxyName");
        private static HubConnection connection; 
        private static IHubProxy hub;
        private static readonly SignaRClient simpleClient = new SignaRClient();
        public int ThreadSleepTime { get; set; }

        private SignaRClient() {
            connection = new HubConnection(endPoint);
            hub = connection.CreateHubProxy(hubProxyName);
            ThreadSleepTime = 500;
        }
        static SignaRClient() { }

        public static SignaRClient Instance {
            get { return simpleClient; }
        }
        
        public void Join(string name)
        {
            //Remove(name);
            connection.Start().ContinueWith(t => 
            {
                hub.Invoke("Join", name);
            });
        }

        public void Join(string name, Action<string,string> callBack)
        {
            //Remove(name);
            hub.On("Show", callBack);
            connection.Start().ContinueWith(t =>
            {
                hub.Invoke("Join", name);
            });
        }

        public void Invoke(params string[] messages)
        {
            Validate();
            Thread.Sleep(ThreadSleepTime);
            if (connection.State == ConnectionState.Connected)
                hub.Invoke("Send", messages);
            else {
                connection.Start().ContinueWith(t =>
                {
                    hub.Invoke("Send", messages);
                });
            }
        }
        
        public void Remove(string name)
        {
            if (connection.State == ConnectionState.Connected)
                hub.Invoke("Remove", name).Wait();
            else
            {
                connection.Start().ContinueWith(t =>
                {
                    hub.Invoke("Remove", name).Wait();
                });
            }
            
        }

        private  void Validate()
        {
            if (string.IsNullOrEmpty(endPoint))
                throw new ArgumentNullException("SignalREndPoint", "End Point URL not set in app config");
            if (string.IsNullOrEmpty(hubProxyName))
                throw new ArgumentNullException("HubProxyName", "Hub Proxy Name not set in app config");
        }



    }
}
