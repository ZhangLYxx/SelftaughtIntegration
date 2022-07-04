using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Integration.JWT
{
    public static class Authenticationconfig
    {
        public static void AddIntegrationAuth(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateLifetime = false,
                           ValidateAudience = true,
                           ValidateIssuer = true,
                           RequireSignedTokens = true,
                           ValidIssuer = ConstKeys.VALID_ISSUER,
                           ValidAudience = ConstKeys.VALID_AUDIENCE,
                           IssuerSigningKeys = new[] { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstKeys.SECURITY_SECRET)) },
                           ClockSkew = TimeSpan.FromMinutes(1)
                       };
                   });
        }
    }
}