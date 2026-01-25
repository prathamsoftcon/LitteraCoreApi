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
            // ✅ 1. If JWT already authenticated → SKIP ApiKey
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();

            //  2. If endpoint explicitly allows anonymous → SKIP ApiKey
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value?.ToLower() ?? "";

            //  Explicit public APIs (login / otp / config)
            if (path.Contains("/api/gettoken")
                || path.Contains("/api/generateotp")
                || path.Contains("/api/verifyotp")
                || path.Contains("/api/send_otp")
                || path.Contains("/api/verifyotpwithlogin")
                || path.Contains("/api/get_react_app_configuration")
                || path.Contains("/api/Get_Activity_Token_Info")
                || path.Contains("/api/Littera_Events")
                || path.Contains("/api/User_Session_Details")
               || path.Contains("/api/trainingplan")
                || path.Contains("/api/UserInfo_wk")
                || path.Contains("/api/GenerateOTP_wk")
                || path.Contains("/api/VerifyOTP_wk")
                || path.Contains("/api/Participants_training_wk")
                || path.Contains("/api/TRG_PARTICIPANT_DETAILS_wk")
                || path.Contains("/api/GET_CONTENT_DETAILS_wk")
                || path.Contains("/api/GenerateActivityToken_wk")
                || path.Contains("/api/GET_REACT_APP_CONFIGURATION_wk")
                || path.Contains("/api/Check_First_Login_wk")
                || path.Contains("/api/SAVE_USER_LOG_wk")
                || path.Contains("/api/Save_Audit_Trail_wk")
                || path.Contains("/api/Learning_Time_wk")
                || path.Contains("/api/check_content_learning_exist_wk") 
                || path.Contains("/api/Update_Session_Status_wk")

                )
            {
                await _next(context);
                return;
            }

            //  Enforce API key
            string apiKey = context.Request.Headers[ApiKeyName].FirstOrDefault();
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

        private bool IsValidApiKey(string apiKey, string validApiKey)
        {
            if (string.IsNullOrWhiteSpace(validApiKey))
                return false;

            // Direct match
            if (apiKey == validApiKey)
                return true;

            // Base64 decoded match (optional backward compatibility)
            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(apiKey));
                return decoded == validApiKey;
            }
            catch
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
