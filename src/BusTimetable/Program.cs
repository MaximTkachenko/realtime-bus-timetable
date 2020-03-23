using System.Net;
using BusTimetable.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Serilog;
using Serilog.Events;

namespace BusTimetable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Orleans", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog()
                        .UseUrls("http://*:5005");
                })
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IBusStop).Assembly))
                        .UseDashboard(options => {
                            options.Host = "*";
                            options.Port = 5006;
                            options.HostSelf = true;
                            options.CounterUpdateIntervalMs = 2000;
                        })
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = "dev";
                            opts.ServiceId = nameof(BusTimetable);
                        })
                        .Configure<EndpointOptions>(opts =>
                        {
                            opts.AdvertisedIPAddress = IPAddress.Loopback;
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.AddSerilog();
                        });
                });
    }
}
