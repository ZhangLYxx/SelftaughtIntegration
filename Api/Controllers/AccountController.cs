using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
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
        private readonly MigrationsDbContext _dbContext;

        public AccountController(MigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<string> Login([FromQuery]  [FromServices] IIdentityParser identityParser)
        {
           //var row=_dbContex

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
