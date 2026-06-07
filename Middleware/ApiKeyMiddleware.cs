using System.Text;
using Microsoft.AspNetCore.Authorization;

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
                await _next(context);
                return;
            }

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value ?? string.Empty;
            if (IsExcludedPath(path))
            {
                await _next(context);
                return;
            }

            string? apiKey =
                context.Request.Headers[ApiKeyName].FirstOrDefault();
            var config = context.RequestServices.GetRequiredService<IConfiguration>();
            var validApiKey = config.GetValue<string>(ApiKeyName);

            if (string.IsNullOrWhiteSpace(apiKey) || !IsValidApiKey(apiKey, validApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }

            await _next(context);
        }

        private static bool IsExcludedPath(string path)
        {
            var excludedPaths = new[]
            {
                "/api/gettoken",
                "/api/generateotp",
                "/api/verifyotp",
                "/api/send_otp",
                "/api/verifyotpwithlogin",
                "/api/get_react_app_configuration",
                "Get_Activity_Token_Info",
                "Littera_Events",
                "User_Session_Details",
                "trainingplan",
                "GenerateOTP_wk",
                "VerifyOTP_wk",
                "Participants_training_wk",
                "TRG_PARTICIPANT_DETAILS_wk",
                "GET_CONTENT_DETAILS_wk",
                "GET_REACT_APP_CONFIGURATION_wk",
                "Check_First_Login_wk",
                "SAVE_USER_LOG_wk",
                "Save_Audit_Trail_wk",
                "Learning_Time_wk",
                "check_content_learning_exist_wk",
                "Update_Session_Status_wk",
                "GetClientData",
                "country",
                "Finacial_year",
                "Get_Application_Setting",
                "Check_Payment_Gateway_Available"
            };

            return excludedPaths.Any(excludedPath =>
                path.Contains(excludedPath, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsValidApiKey(
            string apiKey,
            string? validApiKey)
        {
            if (string.IsNullOrWhiteSpace(validApiKey))
                return false;

            // Direct match
            if (apiKey == validApiKey)
                return true;

            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(apiKey));
                return decoded == validApiKey;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
