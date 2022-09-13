using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integration.EntityFrameworkCore.DbMigrations.MySql.TypeConfigurations;
using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
using Microsoft.EntityFrameworkCore;
using Self.MySql.Entity;

namespace Integration.EntityFrameworkCore.DbMigrations.MySql
{
    public class SelfMySqlDbContext:DbContext
    {
        public SelfMySqlDbContext(DbContextOptions<SelfMySqlDbContext> options) : base(options)
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
