using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LitteraCore.Common.Token
{
    public sealed class AuthSecuritySettings
    {
        public const string CookieName = "Auth_token";
        public const string AuthenticationSourceItem = "AuthenticationSource";
        public const string CookieAuthenticationSource = "Cookie";

        public AuthSecuritySettings(
            string signingKey,
            string issuer,
            string audience,
            TimeSpan sessionLifetime)
        {
            SigningKey = signingKey;
            Issuer = issuer;
            Audience = audience;
            SessionLifetime = sessionLifetime;
        }

        public string SigningKey { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public TimeSpan SessionLifetime { get; }

        public SymmetricSecurityKey CreateSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        }

        public TokenValidationParameters CreateTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = CreateSecurityKey(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        }

        public static AuthSecuritySettings FromConfiguration(IConfiguration configuration)
        {
            var signingKey =
                Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? configuration["JWT:Key"];
            var issuer =
                Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? configuration["JWT:Issuer"];
            var audience =
                Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                ?? configuration["JWT:Audience"];

            if (string.IsNullOrWhiteSpace(signingKey))
            {
                throw new InvalidOperationException(
                    "JWT signing key is required. Set JWT_SECRET or JWT:Key.");
            }

            if (Encoding.UTF8.GetByteCount(signingKey) < 32)
            {
                throw new InvalidOperationException(
                    "JWT signing key must be at least 32 bytes.");
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new InvalidOperationException(
                    "JWT issuer is required. Set JWT_ISSUER or JWT:Issuer.");
            }

            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException(
                    "JWT audience is required. Set JWT_AUDIENCE or JWT:Audience.");
            }

            var sessionHours = configuration.GetValue("JWT:SessionHours", 8);
            if (sessionHours <= 0 || sessionHours > 24)
            {
                throw new InvalidOperationException(
                    "JWT:SessionHours must be between 1 and 24.");
            }

            return new AuthSecuritySettings(
                signingKey,
                issuer,
                audience,
                TimeSpan.FromHours(sessionHours));
        }
    }
}
