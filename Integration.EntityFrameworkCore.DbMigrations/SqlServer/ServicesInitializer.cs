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

        }
    }
}
