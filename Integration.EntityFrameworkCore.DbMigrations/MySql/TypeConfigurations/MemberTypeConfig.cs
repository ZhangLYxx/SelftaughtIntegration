using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Self.MySql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integration.EntityFrameworkCore.DbMigrations.MySql.TypeConfigurations
{
    public class MemberTypeConfig : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("m_member");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Name).IsRequired();
            builder.Property(x=>x.Type).IsRequired();
            builder.Property(x=>x.State).IsRequired();
            builder.Property(x=>x.Role).IsRequired();
        }
    }
}
