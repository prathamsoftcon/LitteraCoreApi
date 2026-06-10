using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace LitteraCore.Common.Token
{
    public static class ApiKeyAuthenticationDefaults
    {
        public const string AuthenticationScheme = "ApiKey";
        public const string PolicyName = "PublicApiKey";
        public const string HeaderName = "ApiKey";
    }

    public sealed class ApiKeyAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var suppliedApiKey =
                Request.Headers[ApiKeyAuthenticationDefaults.HeaderName]
                    .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(suppliedApiKey))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var configuredApiKey =
                Environment.GetEnvironmentVariable("API_KEY")
                ?? _configuration[ApiKeyAuthenticationDefaults.HeaderName];

            if (string.IsNullOrWhiteSpace(configuredApiKey))
            {
                Logger.LogError("API key authentication is not configured.");
                return Task.FromResult(
                    AuthenticateResult.Fail(
                        "API key authentication is not configured."));
            }

            if (!KeysMatch(suppliedApiKey, configuredApiKey))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Invalid API key."));
            }

            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "react-proxy"),
                    new Claim(ClaimTypes.Name, "React Proxy")
                },
                ApiKeyAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(
                principal,
                ApiKeyAuthenticationDefaults.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private static bool KeysMatch(string suppliedKey, string configuredKey)
        {
            var suppliedBytes = Encoding.UTF8.GetBytes(suppliedKey);
            var configuredBytes = Encoding.UTF8.GetBytes(configuredKey);

            return suppliedBytes.Length == configuredBytes.Length
                && CryptographicOperations.FixedTimeEquals(
                    suppliedBytes,
                    configuredBytes);
        }
    }
}
