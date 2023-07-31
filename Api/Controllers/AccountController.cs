using Integration.EntityFrameworkCore.DbMigrations.MySql;
using Integration.EntityFrameworkCore.DbMigrations.PGSql;
using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
using Integration.JWT;
using Integration.Service.StartUp;
using Integration.ToolKits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Self.MySql.Entity;
using System.Text.RegularExpressions;

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
        private readonly SelfPGSqlDbContext _pgdbContext;
        private readonly SelfMySqlDbContext _mySqlContext;
        public AccountController(MigrationsDbContext dbContext, SelfMySqlDbContext mySqlContext,SelfPGSqlDbContext selfPGSqlDbContext)
        {
            _dbContext = dbContext;
            _mySqlContext = mySqlContext;
            _pgdbContext = selfPGSqlDbContext;
        }

        [HttpGet]
        public string Login([FromServices] IIdentityParser identityParser)
        {
            var token = identityParser.Parse(new ManagerSession { Roles = new[] { "Manager" }, UserId = 101, UserName = "na" });
            return $"Bearer {token}";
        }

        [HttpGet("[action]")]
        [ManagerAuthorize]
        public ManagerSession Test()
        {
            return HttpContext.GetManagerSession();
        }
    }
}
