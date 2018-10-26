using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Fanview.API
{
    public class Program
    {
        static string SpecialFileName
        {
            get
            {
                return string.Format("Fanview-Api-log-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            }
        }
        public static void Main(string[] args)
        {
            var fileName = Path.Combine(Path.GetFullPath(@"../../../../../" + "FanViewAPILog"), SpecialFileName);

            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.File(fileName)
           .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
                //return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                //return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
