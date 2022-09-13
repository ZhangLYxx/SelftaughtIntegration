using Integration.EntityFrameworkCore.DbMigrations.PGSql.TypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Self.MySql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.EntityFrameworkCore.DbMigrations.PGSql
{
    public class SelfPGSqlDbContext : DbContext
    {
        public SelfPGSqlDbContext(DbContextOptions<SelfPGSqlDbContext> options) : base(options)
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
