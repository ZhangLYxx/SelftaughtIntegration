using Integration.JWT;
using Integration.Service.StartUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UCmember.Api.Controllers
{
    /// <summary>
    /// 账号
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]  
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Login([FromServices] IIdentityParser identityParser)
        {
            var roles = new MemberSession
            {
                UserId = 1l,
                UserName = "张政",
                Roles = new [] { PermissionConstantKey.ExaminePolicy }
            };

            var token = identityParser.Parse(roles);
            return $"Bearer {token}";
        }

    }
}
