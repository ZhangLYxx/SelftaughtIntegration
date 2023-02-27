using Integration.EntityFrameworkCore.DbMigrations.MySql;
using Integration.EntityFrameworkCore.DbMigrations.PGSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.EntityFrameworkCore.DbMigrations.SqlServer
{
    public static class ServicesInitializer
    {
        public static void Register(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<MigrationsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });
            service.AddDbContext<SelfMySqlDbContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("MySqlConnection"));
            });
            service.AddDbContext<SelfPGSqlDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PGSqlConnection"));
            });
        }   
    }
}
