using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace SchedulerJob
{
    public class StartLogInfoJob : IJob
    {

        private readonly ILogger _logger;
        public StartLogInfoJob(ILogger<StartLogInfoJob> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Start();
        }
        public async Task Start()
        {
            _logger.LogInformation("调度打印Debug");
        }
    }


    //public class StartLogDebugJob : IJob
    //{
    //    public async Task Execute(IJobExecutionContext context)
    //    {
    //        await Start();
    //    }
    //    public async Task Start()
    //    {
    //        _.Info("调度打印Info");
    //    }
    //}
}
