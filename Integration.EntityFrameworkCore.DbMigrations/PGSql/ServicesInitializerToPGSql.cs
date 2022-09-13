using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.EntityFrameworkCore.DbMigrations.PGSql
{
    public class ServicesInitializerToPGSql
    {
        public static void Register(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<SelfPGSqlDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PGSqlConnection"));
            });

        }
    }
}
