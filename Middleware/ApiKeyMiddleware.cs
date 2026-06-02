using System.Text;

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
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next.Invoke(context);
                return;
            }

            var path = context.Request.Path.Value ?? string.Empty;

            if (!IsExcludedPath(path))
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

        private bool IsExcludedPath(string path)
        {
            var excludedPaths = new[]
            {
                "Get_Activity_Token_Info",
                "Littera_Events",
                "User_Session_Details",
                "trainingplan",
                "UserInfo_wk",
                "GenerateOTP_wk",
                "VerifyOTP_wk",
                "Participants_training_wk",
                "TRG_PARTICIPANT_DETAILS_wk",
                "GET_CONTENT_DETAILS_wk",
                "GenerateActivityToken_wk",
                "GET_REACT_APP_CONFIGURATION_wk",
                "Check_First_Login_wk",
                "SAVE_USER_LOG_wk",
                "Save_Audit_Trail_wk",
                "Learning_Time_wk",
                "check_content_learning_exist_wk",
                "Update_Session_Status_wk"
            };

            return excludedPaths.Any(excludedPath =>
                path.Contains(excludedPath, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidApiKey(string apiKey, string validapiKey)
        {
            // Implement your logic to validate API keys here (e.g., check against a database)
            // For simplicity, let's assume we have a list of valid API keys stored in a configuration
            //var validApiKeys = new List<string> { "your-api-key-1", "your-api-key-2" };


            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(validapiKey))
            {
                return false;
            }

            if (validapiKey == apiKey)
            {
                return true;
            }

            try
            {
                return validapiKey == Encoding.UTF8.GetString(Convert.FromBase64String(apiKey));
            }
            catch (FormatException)
            {
                return false;
            }
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
