using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Integration.JWT
{
    public class JwtIdentityParser : IIdentityParser
    {
        public string Parse(UserSession session)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(session))
            };
            claims.AddRange(session.Roles.Select(role => new Claim("Roles", role)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstKeys.SECURITY_SECRET));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: ConstKeys.VALID_ISSUER,
                audience: ConstKeys.VALID_AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }
        public UserSession Parse(string identityStr)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstKeys.SECURITY_SECRET));

            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireSignedTokens = true,
                ValidIssuer = ConstKeys.VALID_ISSUER,
                ValidAudience = ConstKeys.VALID_AUDIENCE,
                IssuerSigningKeys = new[] { key }
            };

            try
            {
                var jwtObj = new JwtSecurityTokenHandler().ValidateToken(identityStr, validationParameters, out _);
                if (jwtObj?.Claims != null)
                {
                    var userDataClaim = jwtObj.Claims.FirstOrDefault(it => it.Type == ClaimTypes.UserData);
                    if (userDataClaim == null)
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<UserSession>(userDataClaim.Value);
                }
                return null;
            }
            catch
            {
                return null;

            }
        }
    }
}
