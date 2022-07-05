using Dto;
using Entity;
using Integration.JWT;
using Integration.Service.StartUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionConstantKey.ExaminePolicy)]
        public  string Login()
        {
            var session = HttpContext.GetMemberSession();

            return $"{session.UserId}------{session.UserName}------{session.Roles[0]}";
        }
    }
}
