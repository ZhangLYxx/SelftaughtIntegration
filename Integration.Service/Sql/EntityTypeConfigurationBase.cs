using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.Sql
{
    public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (typeof(IEntity).IsAssignableFrom(builder.Metadata.ClrType))
            {
                var p1 = builder.Property<long>("Id");
                if (p1 != null)
                {
                    p1.IsRequired().ValueGeneratedNever();
                    builder.HasKey("Id");
                }
            }

            if (typeof(ISoftDelete).IsAssignableFrom(builder.Metadata.ClrType))
            {
                var p1 = builder.Property<bool>("IsDeleted");
                if (p1 != null)
                {
                    p1.IsRequired();
                    //Expression<Func<ISoftDelete, bool>> deletedFilter = _ => _.IsDeleted == false;

                    //builder.HasQueryFilter(GetSoftDeleteFilter<TEntity>());
                    builder.Metadata.AddSoftDeleteQueryFilter();
                }
            }

            if (typeof(ITenantEntity).IsAssignableFrom(builder.Metadata.ClrType))
            {
                var p1 = builder.Property<long>("TenantId");
                p1?.IsRequired();
            }

            if (typeof(IAuditEntity).IsAssignableFrom(builder.Metadata.ClrType))
            {
                builder.Property<long>("CreateBy")?.IsRequired();
                builder.Property<string>("CreateName")?.IsRequired().IsUnicode().HasMaxLength(50);
                builder.Property<DateTime>("CreateTime")?.IsRequired();
                builder.Property<string>("LastUpdateName")?.IsUnicode().HasMaxLength(50);
            }
        }

        /*
        private static LambdaExpression GetSoftDeleteFilter<T>() where T : class
        {
            var typeP = Expression.Parameter(typeof(T), "_");
            var access = Expression.Property(typeP, "IsDeleted");
            var const1 = Expression.Constant(false);
            var body = Expression.Equal(access, const1);
            return Expression.Lambda<Func<T, bool>>(body, typeP);
 
        }
        */
    }


    public static class EntityTypeBuilderExtension
    {
        public static void AddSoftDeleteQueryFilter(
            this IMutableEntityType entityData)
        {
            var methodToCall = typeof(EntityTypeBuilderExtension)
                .GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);
            if (methodToCall is null)
            {
                return;
            }
            var filter = methodToCall.Invoke(null, new object[] { });
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => x.IsDeleted == false;
            return filter;
        }
    }
}
