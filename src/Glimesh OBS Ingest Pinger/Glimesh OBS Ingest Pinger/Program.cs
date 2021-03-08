using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

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
                //var json = new WebClient().DownloadString("https://glimesh-static-assets.nyc3.digitaloceanspaces.com/obs-glimesh-service.json");
                var json = File.ReadAllText(@"obs-glimesh-service.json");
                JObject root = JObject.Parse(json);
                var server_url = root["servers"].Children()["url"];

                List<Task<PingReply>> pingTasks = new List<Task<PingReply>>();
                foreach (var address in server_url)
                {
                    pingTasks.Add(PingAsync(address.ToString()));
                    string url = address.ToString();
                }

                //Wait for all the tasks to complete
                Task.WaitAll(pingTasks.ToArray());

                //Now you can iterate over your list of pingTasks
                int x = 0;
                foreach (var pingTask in pingTasks)
                {
                    //pingTask.Result is whatever type T was declared in PingAsync
                    int server = (x = x + 1);
                    if(server == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: North America - Chicago, United States");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: North America - New York, United States");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: North America - San Francisco, United States");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: North America - Toronto, Canada");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: Europe - Amsterdam, Netherlands");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 6)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: Europe - Frankfurt, Germany");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: Europe - London, United Kingdom");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 8)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: Asia - Bangalore, India");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    if (server == 9)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Server: Asia - Singapore");
                        Console.WriteLine(" Status: " + pingTask.Result.Status);
                        Console.WriteLine(" Roundtrip Time: " + pingTask.Result.RoundtripTime);
                        Console.WriteLine(" Time to live: " + pingTask.Result.Options.Ttl);
                        Console.WriteLine(" Buffer size: " + pingTask.Result.Buffer.Length);
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(" Scroll Up To See All Servers.");
                Console.WriteLine();
                Console.WriteLine(" Press any key to close.");
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("An error has occured.");
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
