using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Integration.BackgroundJobs.Quartz
{
    public class WorkAbnormalAppService : IJob
    {
        /// <summary>
        /// 实现定时接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            //你需要定时执行的任务 
            //SendEmail email = new SendEmail();
            //string[] emailList = new string[] { "1586693437@qq.com" };
            //foreach (var item in emailList)
            //{
            //    email.Send_Email("定时邮件提醒", "BUG处理", item);
            //}
        }

    }
}
