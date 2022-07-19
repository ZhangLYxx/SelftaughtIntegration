using Microsoft.Extensions.Logging;
using Quartz;

namespace Integration.BackgroundJobs.Quartz
{
    public class QuartzJob : IJob
    {
        private readonly ILogger<QuartzJob> _logger;
        public QuartzJob(ILogger<QuartzJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobKey = context.JobDetail.Key;//获取job信息
            var triggerKey = context.Trigger.Key;//获取trigger信息

            _logger.LogInformation($"{DateTime.Now} QuartzJob:==>>自动执行.{jobKey.Name}|{triggerKey.Name}");
            await Task.CompletedTask;
        }
    }
}
