using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace Glimesh_OBS_Ingest_Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 60;
            Console.Title = "Unofficial Glimesh OBS Ingest Pinger";
            GetPing();
        }

        public static void GetPing()
        {
            try
            {
                var json = new WebClient().DownloadString("https://glimesh-static-assets.nyc3.digitaloceanspaces.com/obs-glimesh-service.json");
                JObject root = JObject.Parse(json);
                var server_url = root["servers"].Children()["url"];

                List<Task<PingReply>> pingTasks = new List<Task<PingReply>>();
                foreach (var address in server_url)
                {
                    pingTasks.Add(PingAsync(address.ToString()));
                }

                //Wait for all the tasks to complete
                Task.WaitAll(pingTasks.ToArray());

                //Now you can iterate over your list of pingTasks
                int x = -1;
                foreach (var pingTask in pingTasks)
                {
                    //pingTask.Result is whatever type T was declared in PingAsync
                    x = x + 1;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" Server: {0}", root["servers"][x]["name"]);
                    Console.WriteLine(" Url: {0}", root["servers"][x]["url"]);
                    Console.WriteLine(" Status: {0}", pingTask.Result.Status);
                    Console.WriteLine(" Roundtrip Time: {0}", pingTask.Result.RoundtripTime);
                    Console.WriteLine(" Time to live: {0}", pingTask.Result.Options.Ttl);
                    Console.WriteLine(" Buffer size: {0}", pingTask.Result.Buffer.Length);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                Console.WriteLine(" Scroll Up To See All Servers.");
                Console.WriteLine();
                Console.WriteLine(" Press [Space] key to close.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                /*
                 * Disabled for production due to
                 * An error that occurs randomly.
                 * 
                var ST = new StackTrace(e, true);
                var STframe = ST.GetFrame(0);
                var STLine = STframe.GetFileLineNumber();
                Console.WriteLine("An error occured.");
                Console.WriteLine("Message: {0}", e.Message);
                Console.WriteLine("Stack Trace: {0}", e.StackTrace);
                Console.WriteLine("Source: {0}", e.Source);
                Console.WriteLine("TargetSite: {0}", e.TargetSite);
                Console.WriteLine("InnerException: {0}", e.InnerException);
                Console.WriteLine("Stack Trace Line Number: {0}", STLine);
                Console.WriteLine();*/
            }
            Console.ReadKey();
        }

        static Task<PingReply> PingAsync(string address)
        {
            var tcs = new TaskCompletionSource<PingReply>();
            Ping ping = new Ping();
            PingOptions options = new PingOptions();
            options.Ttl = 128;
            string data = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            ping.PingCompleted += (obj, sender) =>
            {
                tcs.SetResult(sender.Reply);
            };
            ping.SendAsync(address, 120, buffer, options);
            return tcs.Task;
        }
    }
}
