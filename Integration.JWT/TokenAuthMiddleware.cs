using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenAuthMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue(RequestUtility.TokenHeader, out var token) && !string.IsNullOrEmpty(token))
            {
                var cacheString = string.Empty;
                var identityParser = httpContext.RequestServices.GetService<IIdentityParser>();
                var body = identityParser?.Parse(token);
                if (body != null && body.UserId > 0)
                {
                    cacheString = JsonConvert.SerializeObject(body);
                }
                if (!string.IsNullOrEmpty(cacheString))
                {
                    httpContext.Request.Headers.Add(RequestUtility.JWTAuthHeader,
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(cacheString)));
                    httpContext.Request.Headers.Remove(RequestUtility.TokenHeader);
                }
            }
            httpContext.Request.Headers.TryAdd("_API_GATEWAY_", "OCELOT");

            await _next(httpContext);
        }
    }

    public static class TokenCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenAuthMiddleware>();
        }
    }
}
