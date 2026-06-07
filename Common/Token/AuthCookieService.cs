namespace LitteraCore.Common.Token
{
    public sealed class AuthCookieService
    {
        private readonly AuthSecuritySettings _settings;

        public AuthCookieService(AuthSecuritySettings settings)
        {
            _settings = settings;
        }

        public void Append(HttpResponse response, string token)
        {
            response.Cookies.Append(
                AuthSecuritySettings.CookieName,
                token,
                CreateCookieOptions(DateTimeOffset.UtcNow.Add(_settings.SessionLifetime)));
        }

        public void Delete(HttpResponse response)
        {
            response.Cookies.Delete(
                AuthSecuritySettings.CookieName,
                CreateCookieOptions(DateTimeOffset.UnixEpoch));
        }

        private static CookieOptions CreateCookieOptions(DateTimeOffset expires)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                IsEssential = true,
                Expires = expires
            };
        }
    }
}
