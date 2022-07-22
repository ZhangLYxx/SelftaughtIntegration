using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Integration.BackgroundJobs.Quartz
{
    public class QuartzManager
    {
        public async static void Init()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            //创建一个Scheduler任务调度容器
            IScheduler scheduler = await factory.GetScheduler();

            //指定具体执行的任务Job
            IJobDetail sendEmailJob = JobBuilder.Create<SendMailJob>()
                .WithIdentity("sendEmailJob", "sendEmailJobGrop")
                .WithDescription("定时发送邮件").Build();

            //设置触发条件为五秒执行一次
            ITrigger sendEmailTrigger = TriggerBuilder.Create()
                .WithIdentity("sendEmailTrigger", "sendEmailJobGrop")
                    .WithDescription("QuartZ")
                    .WithCronSchedule("3/5 * * * * ?")
                    .Build();

            //把策略和任务放入到Scheduler
            await scheduler.ScheduleJob(sendEmailJob, sendEmailTrigger);
            //执行任务
            await scheduler.Start();
        }
    }


    //增加特性保证任务不会重叠执行
    [DisallowConcurrentExecution]
    public class SendMailJob : IJob
    {
        //Job类
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                //doSomthing
                Console.WriteLine($"开始发送邮件{DateTime.Now}");
            });
        }
    }
}
