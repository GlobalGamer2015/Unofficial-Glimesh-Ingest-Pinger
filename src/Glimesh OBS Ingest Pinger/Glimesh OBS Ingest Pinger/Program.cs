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

        protected static List<Task<PingReply>> pingTasks = new List<Task<PingReply>>();

        public static void GetPing()
        {
            try
            {
                var json = new WebClient()?.DownloadString("https://glimesh-static-assets.nyc3.digitaloceanspaces.com/obs-glimesh-service.json");
                JObject root = JObject.Parse(json);

                var server_url = root?["servers"]?.Children()["url"];

                foreach (var address in server_url)
                {
                    pingTasks.Add(PingAsync(address?.ToString()));
                }

                //Wait for all the tasks to complete
                Task.WaitAll(pingTasks?.ToArray());

                //Now you can iterate over your list of pingTasks
                int x = -1;
                foreach (var pingTask in pingTasks)
                {
                    //pingTask.Result is whatever type T was declared in PingAsync
                    x = x + 1;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" Server: {0}", root?["servers"]?[x]?["name"]);
                    Console.WriteLine(" Url: {0}", root?["servers"]?[x]?["url"]);
                    if(pingTask?.Result?.Status == IPStatus.Success)
                    {
                        Console.WriteLine(" Status: {0}", pingTask?.Result?.Status); // No color change
                    }
                    if(pingTask?.Result?.Status != IPStatus.Success)
                    {
                        Console.Write($" Status: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(pingTask?.Result?.Status);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    //Console.WriteLine(" Status: {0}", pingTask.Result.Status);
                    
                    if(pingTask?.Result?.RoundtripTime == 0)
                    {
                        Console.Write(" Roundtrip Time: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(pingTask?.Result?.RoundtripTime);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if(pingTask?.Result?.RoundtripTime >= 1)
                    {
                        Console.WriteLine(" Roundtrip Time: {0}", pingTask?.Result?.RoundtripTime);
                    }

                    if (pingTask?.Result?.Options?.Ttl == 0)
                    {
                        Console.Write(" Time to live: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(pingTask?.Result?.Options?.Ttl);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (pingTask?.Result?.Options?.Ttl >= 1)
                    {
                        Console.WriteLine(" Time to live: {0}", pingTask?.Result?.Options?.Ttl);
                    }

                    if (pingTask?.Result?.Buffer?.Length == 0)
                    {
                        Console.Write(" Buffer size: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(pingTask?.Result?.Buffer?.Length);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (pingTask?.Result?.Buffer?.Length >= 1)
                    {
                        Console.WriteLine(" Buffer size: {0}", pingTask?.Result?.Buffer?.Length);
                    }

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
                var ST = new StackTrace(e, true);
                var STframe = ST.GetFrame(0);
                var STLine = STframe.GetFileLineNumber();
                throw new Exception(
                "An error occured.\n" +
                $"Message: {e.Message}\n" +
                $"Stack Trace: {e.StackTrace}\n" +
                $"Source: {e.Source}\n" +
                $"TargetSite: {e.TargetSite}\n" +
                $"InnerException: {e.InnerException}\n" +
                $"Stack Trace Line Number: {STLine}\n");
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
                tcs?.SetResult(sender?.Reply);
            };
            ping.SendAsync(address, 120, buffer, options);
            return tcs?.Task;
        }
    }
}
