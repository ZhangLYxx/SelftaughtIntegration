using System.Diagnostics;

namespace RunKafkaService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker测试 服务开启了2");

            stoppingToken.Register(() =>
            {
                _logger.LogInformation("Worker测试 服务正在停止2");
                _logger.LogInformation("Worker测试 服务已停止2");
            });

            Start(_logger);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker测试 服务在运行中2");

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }


        static void Start(ILogger logger)
        {
            RunJps(logger);
        }

        public static void RunZookeeper(ILogger logger)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;       //是否重定向错误
            startInfo.RedirectStandardInput = true;       //是否重定向输入   是则不能在cmd命令行中输入
            startInfo.RedirectStandardOutput = true;      //是否重定向输出,是则不会在cmd命令行中输出
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(@"E: ");
            process.StandardInput.WriteLine(@"cd E:\kafka_2.13-3.2.0 ");
            process.StandardInput.WriteLine(@"bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties");

            RunJps(logger);
        }

        public static void RunKafka(ILogger logger)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(@"E: ");
            process.StandardInput.WriteLine(@"cd E:\kafka_2.13-3.2.0 ");
            process.StandardInput.WriteLine(@"bin\windows\kafka-server-start.bat .\config\server.properties");

            RunJps(logger);
        }

        public static void RunJps(ILogger logger)
        {
            var output = string.Empty;
            var msg = string.Empty;
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(@"E: ");
            process.StandardInput.WriteLine(@"cd E:\kafka_2.13-3.2.0 ");
            process.StandardInput.WriteLine(@"jps");
            //io流需要手动关闭输入流
            process.StandardInput.Close();
            //获取返回值
            output = process.StandardOutput.ReadToEnd();
            if (output.IndexOf("QuorumPeerMain") > -1 && output.IndexOf("Kafka") > -1)
            {
                logger.LogInformation("zookeeper，kafka均已开启");
                RunKafkaeagle();
            }
            else if (output.IndexOf("QuorumPeerMain") > -1 && output.IndexOf("Kafka") < 0)
            {
                logger.LogWarning("zookeeper已开启，kafka未开启");
                RunKafka(logger);
            }
            else if (output.IndexOf("QuorumPeerMain") == -1)
            {
                logger.LogWarning("zookeeper未开启，重启所有服务");
                RunZookeeper(logger);
            }
        }

        public static void RunKafkaeagle()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(@"E: ");
            process.StandardInput.WriteLine(@"cd E:\kafkaeagle\kafka-eagle-bin-3.0.1\efak-web-3.0.1\bin ");
            process.StandardInput.WriteLine(@"ke.bat /k");
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }
    }
}