using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.NewFolder
{
    public class Jwt(IConfiguration configuration)
    {
        private readonly string _secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT 'Secret' not found.");


        public string GenerateJwtToken(ClaimsIdentity identity)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: identity.Claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["Jwt:ExpirationTime"])),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ClaimsPrincipal? ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }

                throw new SecurityTokenException("Invalid token");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

    }
}
