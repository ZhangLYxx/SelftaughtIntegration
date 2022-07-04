using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Autofac;
namespace Integration.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, string[] xmlnames, string title)
        {
            var assemnly = Assembly.GetEntryAssembly();
            if (assemnly != null)
            {
                var defaultxml = $"{assemnly.GetName().Name}.xml";
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = title,
                        Version = "v1"
                    });
                    //获取应用程序目录,然后使用反射获取xml文件，并构造出文件的路径
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, defaultxml);
                    //启用xml注释，该方法第二个参数启用控制器的注释，默认为false
                    c.IncludeXmlComments(xmlPath);
                    if (xmlnames != null)
                    {
                        foreach (var xml in xmlnames)
                        {
                            if (xml.Equals(defaultxml))
                            {
                                continue;
                            }
                            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xml));
                        }
                    }

                    //参数 首字符小写
                    c.DescribeAllParametersInCamelCase();
                    c.EnableAnnotations();
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Bearer JWT",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                    });

                    var requirement = new OpenApiSecurityRequirement();
                    var key = new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    requirement.Add(key, Array.Empty<string>());
                    c.AddSecurityRequirement(requirement);
                });
            }



        }

    }
}
