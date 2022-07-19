using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace TimingJob
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).ConfigureLogging((host, logBuilder) =>
            {
                if (host.HostingEnvironment.IsProduction())
                {
                    logBuilder.ClearProviders();
                }
                logBuilder.AddNLog("NLog.config");
            }).Build().Run();
            NLog.LogManager.Shutdown();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
