using NLog.Web;
using System.Runtime.InteropServices;

namespace RunKafkaService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService()
                .UseNLog()
                .Build();
                host.Run();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }            
        }
    }
}