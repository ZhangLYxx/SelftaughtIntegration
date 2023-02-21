using System.Diagnostics;

namespace RunCmdService
{
    public class Program
    {
        static void Main()
        {
            RunZookeeper();
            RunKafka();
            RunJps();
        }
        public static void RunZookeeper()
        {
            var msg = string.Empty;
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
            process.StandardInput.WriteLine(@"bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties /k");
        }

        public static void RunKafka()
        {
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
            process.StandardInput.WriteLine(@"bin\windows\kafka-server-start.bat config\server.properties /k");            
        }

        public static void RunJps()
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
                RunKafkaeagle();
            }
            else if(output.IndexOf("QuorumPeerMain")>-1 && output.IndexOf("Kafka") == -1)
            {
                RunKafka();
            }
            else if (output.IndexOf("QuorumPeerMain") == -1)
            {
                Main();
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
            process.WaitForExit();            
        }
    }
}