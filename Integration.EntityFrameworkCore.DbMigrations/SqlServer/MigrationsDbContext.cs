using Entity;
using Integration.EntityFrameworkCore.DbMigrations.SqlServer.TypeConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.EntityFrameworkCore.DbMigrations.SqlServer
{
    public class MigrationsDbContext: DbContext
    {
        public MigrationsDbContext(DbContextOptions<MigrationsDbContext> options) : base(options)
        {
            
        }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberTypeConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
