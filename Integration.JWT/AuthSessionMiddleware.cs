using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    internal class AuthSessionMiddleware
    {

        private readonly RequestDelegate _next;


        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User is not null && httpContext.User.Identity.IsAuthenticated)
            {
                var claims = httpContext.User.Claims;
                if (claims is not null)
                {
                    var c = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
                    if (c is not null && !string.IsNullOrEmpty(c.Value))
                    {
                        if (httpContext.User.IsInRole("Manager"))
                        {
                            var session = JsonConvert.DeserializeObject<ManagerSession>(c.Value);
                            httpContext.Features.Set(session);
                        }
                        else
                        {
                            var session = JsonConvert.DeserializeObject<MemberSession>(c.Value);
                            httpContext.Features.Set(session);
                        }                        
                    }

                }
            }

            await _next(httpContext);
        }
    }
    public static class AuthSessionMiddlewareExtensions
    {
        public static void UseAuthSession(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}
