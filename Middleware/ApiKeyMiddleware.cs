namespace LitteraCore.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyName = "ApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint().ToString();
            if (!endpoint.Contains("Get_Activity_Token_Info") && !endpoint.Contains("Littera_Events") && !endpoint.Contains("User_Session_Details"))
            {
                string apiKey = context.Request.Headers[ApiKeyName].FirstOrDefault();

                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

                var validapiKey = appSettings.GetValue<string>(ApiKeyName);




                if (string.IsNullOrEmpty(apiKey) || !IsValidApiKey(apiKey, validapiKey))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid API key.");
                    return;
                }
            }
            

            await _next.Invoke(context);
        }

        private bool IsValidApiKey(string apiKey, string validapiKey)
        {
            // Implement your logic to validate API keys here (e.g., check against a database)
            // For simplicity, let's assume we have a list of valid API keys stored in a configuration
            //var validApiKeys = new List<string> { "your-api-key-1", "your-api-key-2" };


            return validapiKey == apiKey;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }


}
