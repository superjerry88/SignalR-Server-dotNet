using System;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalRServerdotNet
{
    class Program
    {
        static IDisposable SignalR;

        static void Main(string[] args)
        {
            SignalR = WebApp.Start<Startup>("http://+:8000");
            Console.WriteLine("AAS Server started successfully");
            Console.ReadKey();
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);
                app.Map("/signalr", map =>
                {
                    map.UseCors(CorsOptions.AllowAll);
                    var hubConfiguration = new HubConfiguration
                    {
                    };
                    map.RunSignalR(hubConfiguration);
                });
                //idk what is this but just leave it for future incase more error
                /*  CAMEL CASE & JSON DATE FORMATTING
                 use SignalRContractResolver from
                https://stackoverflow.com/questions/30005575/signalr-use-camel-case

                var settings = new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };

                settings.ContractResolver = new SignalRContractResolver();
                var serializer = JsonSerializer.Create(settings);
                GlobalHost.DependencyResolver.Register(typeof(JsonSerializer),  () => serializer);                

                 */
                app.MapSignalR();
            }
        }

        [HubName("MyHub")]
        public class MyHub : Hub
        {
            public void Send(string name, string message)
            { 
                //Reply to all connected senders
                //Clients.All.addMessage(name, message);

                //Reply to specific caller
                Clients.Caller.addMessage(name, message);


                Console.WriteLine($"{name} - {message}");
            }
        }

        [HubName("Function1")]
        public class Function1 : Hub
        {
            public void Send(string name, string message)
            {
                //Reply to all connected senders
                Clients.All.addMessage(name, message);

                //Reply to specific caller
                //Clients.Caller.addMessage(name, $"Message received at {DateTime.Now:G}");

                Console.WriteLine($"[Function 1]{name} - {message}");
            }
        }

    }
}
