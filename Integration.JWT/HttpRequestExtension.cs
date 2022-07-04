using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// 获取会员 Session
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static UserSession GetMemberSession(this HttpContext httpContext)
        {
            return httpContext.Features.Get<UserSession>() ?? new UserSession();
        }
    }
}
