using LitteraCore.Common.Token;

namespace LitteraCore.Middleware
{
    public sealed class CookieOriginValidationMiddleware
    {
        private static readonly HashSet<string> SafeMethods =
            new(StringComparer.OrdinalIgnoreCase)
            {
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Trace
            };

        private readonly RequestDelegate _next;
        private readonly HashSet<string> _allowedOrigins;

        public CookieOriginValidationMiddleware(
            RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _allowedOrigins = configuration
                .GetSection("CORSHost")
                .Get<string[]>()?
                .Select(NormalizeOrigin)
                .Where(origin => origin != null)
                .Cast<string>()
                .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public async Task Invoke(HttpContext context)
        {
            var usesCookieAuthentication =
                string.Equals(
                    context.Items[AuthSecuritySettings.AuthenticationSourceItem] as string,
                    AuthSecuritySettings.CookieAuthenticationSource,
                    StringComparison.Ordinal);

            if (usesCookieAuthentication
                && context.User.Identity?.IsAuthenticated == true
                && !SafeMethods.Contains(context.Request.Method)
                && !IsAllowedOrigin(context.Request.Headers.Origin))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(
                    "Cookie-authenticated write requests require an allowed Origin header.");
                return;
            }

            await _next(context);
        }

        private bool IsAllowedOrigin(string originHeader)
        {
            var origin = NormalizeOrigin(originHeader);
            return origin != null && _allowedOrigins.Contains(origin);
        }

        private static string? NormalizeOrigin(string? origin)
        {
            if (string.IsNullOrWhiteSpace(origin)
                || !Uri.TryCreate(origin, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return null;
            }

            return uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        }
    }

    public static class CookieOriginValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieOriginValidation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieOriginValidationMiddleware>();
        }
    }
}
