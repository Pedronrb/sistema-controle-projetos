using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace sistemadecontrole.Server.Utils
{
    public static class JwtUtil
    {
        private static readonly string SecretKey = "minha-chave-secreta-minha-chave-secreta"; // Use a chave diretamente aqui, ou pegue do arquivo de configuração
        private static readonly int ExpirationMinutes = 60;

        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public static string? GetClaim(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal.FindFirst(claimType)?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
