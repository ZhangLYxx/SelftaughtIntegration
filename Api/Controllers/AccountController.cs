using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
using Integration.JWT;
using Integration.Service.StartUp;
using Integration.ToolKits;
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
        private readonly MigrationsDbContext _dbContext;

        public AccountController(MigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ApiResult<string> Login([FromServices] IIdentityParser identityParser)
        {
            var roles = new MemberSession
            {
                UserId = 1L,
                UserName = "张政",
                Roles = new [] { PermissionConstantKey.ExaminePolicy }
            };

            var token = identityParser.Parse(roles);
            return ApiResult.Ok($"Bearer {token}");
        }
    }
}
