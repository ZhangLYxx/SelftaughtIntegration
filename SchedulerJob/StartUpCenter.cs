using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace SchedulerJob
{
    public class StartUpCenter
    {
        private IScheduler scheduler;
        /// <summary>

        /// 创建调度任务的入口

        /// </summary>

        /// <returns></returns>

        public async Task Start()
        {
            await StartJob();
        }

        /// <summary>
        /// 创建调度任务的公共调用中心
        /// </summary>
        /// <returns></returns>
        public async Task StartJob()
        {
            //创建一个工厂
            NameValueCollection param = new NameValueCollection()
            {
                { "testJob","test"}
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(param);
            //创建一个调度器
            scheduler = await factory.GetScheduler();
            //开始调度器
            await scheduler.Start();

            //每三秒打印一个info日志
            await CreateJob<StartLogInfoJob>("_StartLogInfoJob", "_StartLogInfoJob", " 0/3 * * * * ? ");

            //每五秒打印一个debug日志
            //await CreateJob<StartLogDebugJob>("_StartLogDebugJob", "_StartLogDebugJob", " 0/5 * * * * ? ");

            //调度器时间生成地址--    http://cron.qqe2.com

        }

        /// <summary>
        /// 停止调度器      
        /// </summary>
        public void Stop()
        {
            scheduler.Shutdown();
            scheduler = null;
        }

        /// <summary>
        /// 创建运行的调度器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <param name="cronTime"></param>
        /// <returns></returns>
        public async Task CreateJob<T>(string name, string group, string cronTime) where T : IJob
        {
            //创建一个作业
            var job = JobBuilder.Create<T>()
                .WithIdentity("name" + name, "gtoup" + group)
                .Build();

            //创建一个触发器
            var tigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("name" + name, "group" + group)
                .StartNow()
                .WithCronSchedule(cronTime)
                .Build();

            //把作业和触发器放入调度器中
            await scheduler.ScheduleJob(job, tigger);
        }
    }
}