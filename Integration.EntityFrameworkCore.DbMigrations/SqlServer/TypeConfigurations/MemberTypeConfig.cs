using Entity;
using Integration.Service.Sql;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Integration.EntityFrameworkCore.DbMigrations.SqlServer.TypeConfigurations
{
    public class MemberTypeConfig: IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("UC_Member");
            builder.HasKey(it => it.Id);
            builder.Property(it => it.Name).IsRequired().IsUnicode().HasMaxLength(50);
            builder.Property(it => it.Age).IsRequired();
            builder.Property(it => it.PhoneNumber).IsRequired().IsUnicode().HasMaxLength(50);
        }
    }
}
