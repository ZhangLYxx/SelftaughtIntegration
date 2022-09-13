using Integration.Application.Contracts.SecondHandCar;
using Integration.Application.SecondHandCar;
using Integration.BackgroundJobs.Quartz;
using Integration.JWT;
using Integration.Kafka;
using Integration.MediatR;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.StartUp
{
    /// <summary>
    /// 依赖项
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 添加依赖
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDependency(IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).GetTypeInfo().Assembly);
            services.AddScoped<IMember, MemberApplication>();
            services.AddTransient<IKafkaService, KafkaService>();
            services.AddScoped<IIdentityParser, JwtIdentityParser>();
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="services"></param>
        public static void AddPolicy(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {

                options.AddPolicy(PermissionConstantKey.UploadPolicy, p =>
                {
                    p.RequireAuthenticatedUser().RequireClaim("Roles", PermissionConstantKey.UploadPolicy);
                });

                options.AddPolicy(PermissionConstantKey.ExaminePolicy, p =>
                {
                    p.RequireAuthenticatedUser().RequireClaim("Roles", PermissionConstantKey.ExaminePolicy);
                });

                options.AddPolicy(PermissionConstantKey.LikesPolicy, p =>
                {
                    p.RequireAuthenticatedUser().RequireClaim("Roles", PermissionConstantKey.LikesPolicy);
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("DEV_CORS", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                        .WithExposedHeaders("content-disposition", "Content-Disposition")
                        .SetPreflightMaxAge(TimeSpan.FromHours(15));
                });

                options.AddPolicy("CORS", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                        .WithExposedHeaders("content-disposition", "Content-Disposition")
                        .SetPreflightMaxAge(TimeSpan.FromHours(10));
                });
            });

        }

    }
}
