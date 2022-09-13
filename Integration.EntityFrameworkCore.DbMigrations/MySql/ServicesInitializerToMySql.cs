using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.EntityFrameworkCore.DbMigrations.MySql
{
    public class ServicesInitializerToMySql
    {
        public static void Register(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<SelfMySqlDbContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("MySqlConnection"));
            });

        }
    }
}
