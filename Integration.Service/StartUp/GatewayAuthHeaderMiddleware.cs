using Integration.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.StartUp
{
    public class GatewayAuthHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public GatewayAuthHeaderMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes)
        {
            _next = next;
            Schemes = schemes;
        }
        public IAuthenticationSchemeProvider Schemes { get; set; }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = httpContext.Request.Path,
                OriginalPathBase = httpContext.Request.PathBase
            });

            if (httpContext.Request.Headers.TryGetValue(RequestUtility.GatewayAuthHeader, out var baseStr) && !StringValues.IsNullOrEmpty(baseStr))
            {
                var jsonStr = Encoding.UTF8.GetString(Convert.FromBase64String(baseStr.ToString()));
                var userInfo = JsonConvert.DeserializeObject<UserSession>(jsonStr);
                if (userInfo != null)
                {
                    var claims = new List<Claim>();
                    if (userInfo.Roles != null)
                    {
                        claims.AddRange(userInfo.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    claims.Add(new Claim(ClaimTypes.Sid, userInfo.UserId.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));
                    var identity = new ClaimsIdentity(claims, RequestUtility.GatewayAuthHeader);
                    var principal = new ClaimsPrincipal(identity);
                    httpContext.User = principal;
                    await httpContext.AuthenticateAsync(RequestUtility.GatewayAuthHeader);

                }
            }

            await _next.Invoke(httpContext);
        }
    }
    public static class GatewayAuthorizationMiddlewareExtensions
    {
        /// <summary>
        /// 使用网关头部认证中间件
        /// </summary>
        /// <param name="builder"></param>
        public static void UseGatewayAuthHeader(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GatewayAuthHeaderMiddleware>();
        }
    }
}
