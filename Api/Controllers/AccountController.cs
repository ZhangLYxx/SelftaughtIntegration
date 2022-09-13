﻿using Integration.EntityFrameworkCore.DbMigrations.MySql;
using Integration.EntityFrameworkCore.DbMigrations.PGSql;
using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
using Integration.JWT;
using Integration.Service.StartUp;
using Integration.ToolKits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Self.MySql.Entity;
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
        public Member[] Login()
        {
            var c = _mySqlContext.Members.Where(c => c.Id == 1).ToArray();
            return c;
        }

        [HttpGet("[action]")]
        public Member[] Test()
        {
            var result = _pgdbContext.Members.AsNoTracking().ToArray();
            
            return result;
        }
    }
}
