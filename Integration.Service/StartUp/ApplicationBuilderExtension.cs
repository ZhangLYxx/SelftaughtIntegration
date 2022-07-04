using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.StartUp
{
    public static class ApplicationBuilderExtension
    {
        public static void UseGateSwagger(this IApplicationBuilder app, string serverRoute)
        {
            app.UseSwagger(opt =>
            {
                opt.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    if (httpReq.Headers.TryGetValue("_API_GATEWAY_", out _))
                    {
                        var paths = swaggerDoc.Paths;
                        var newPaths = new OpenApiPaths();
                        foreach (var apiPath in paths)
                        {
                            newPaths.Add($"/{serverRoute}{apiPath.Key}", apiPath.Value);
                        }
                        swaggerDoc.Paths = newPaths;
                        if (httpReq.Headers.ContainsKey("Referer"))
                        {
                            var uri = new Uri(httpReq.Headers["Referer"].ToString());
                            swaggerDoc.Servers.Insert(0, new OpenApiServer { Url = $"{uri.Scheme}://{uri.Authority}", Description = "测试地址" });
                        }
                    }
                });
            });

            app.UseSwaggerUI();
        }
    }
}
