using Quartz;
using Quartz.Impl;

namespace Integration.Quartz.WebApi
{
    public class Program
    {
        static void Main(string[] args)
        {
            // trigger async evaluation
            RunProgram().GetAwaiter().GetResult();
        }

        private static async Task RunProgram()
        {
            try
            {
                // 建立 scheduler
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();

                // 建立 Job
                IJobDetail job = JobBuilder.Create<ShowDataTimeJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // 建立 Trigger，每秒跑一次
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();

                // 加入 ScheduleJob 中
                await scheduler.ScheduleJob(job, trigger);

                // 启动
                await scheduler.Start();

                // 执行
                await Task.Delay(TimeSpan.FromMinutes(1));

                // say goodbye
                await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}