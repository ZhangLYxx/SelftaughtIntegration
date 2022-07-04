using Entity;
using Integration.Service.Sql;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Integration.EntityFrameworkCore.DbMigrations.SqlServer.TypeConfigurations
{
    public class MemberTypeConfig: EntityTypeConfigurationBase<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("UC_Member");
            builder.HasKey(it => it.Id);
            builder.Property(it => it.Name).IsRequired().IsUnicode().HasMaxLength(50).HasColumnName("姓名");
            builder.Property(it => it.Age).IsRequired().HasColumnName("年龄");
            builder.Property(it => it.Birthday).IsRequired().HasColumnName("生日");
            builder.Property(it => it.PhoneNumber).IsRequired().IsUnicode().HasMaxLength(50).HasColumnName("手机号");
            base.Configure(builder);
        }
    }
}
