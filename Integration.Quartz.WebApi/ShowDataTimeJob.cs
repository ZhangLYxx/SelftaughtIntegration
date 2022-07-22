using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Integration.Quartz.WebApi
{
    public class ShowDataTimeJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync($"现在时间 {DateTime.Now}");
        }
    }
}
