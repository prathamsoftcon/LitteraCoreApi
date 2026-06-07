using Azure.Core;
using LitteraCore.Common;
using LitteraCore.Common.EmailService;
using LitteraCore.Common.OTP;
using LitteraCore.Common.SmsService;
using LitteraCore.Common.Token;
using LitteraCore.DBContext;
using LitteraCore.Middleware;
using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
bool swaggerEnabled =
    builder.Configuration.GetValue<bool>("Swagger:Enabled");

#region Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override(
        "Microsoft.AspNetCore.Hosting.Diagnostics",
        LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/error.log",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Error,
        outputTemplate:
            "Date - {Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}]\n" +
            "File: {ExceptionFile} | Line: {ExceptionLineNumber}\n" +
            "RequestId: {RequestId}\n" +
            "RequestMethod:{RequestMethod}\n" +
            "URL: {RequestScheme}://{RequestHost}/{RequestPath}?{RequestParams}\n" +
            "BodyParameter : {BodyParam}\n" +
            "{Message:lj}\n{Exception}\n{Properties:j}\n")
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region CORS
var allowedOrigins =
    builder.Configuration.GetSection("CORSHost").Get<string[]>()
    ?? Array.Empty<string>();

if (allowedOrigins.Any(origin =>
        string.IsNullOrWhiteSpace(origin)
        || origin.Contains('*')
        || !Uri.TryCreate(origin, UriKind.Absolute, out var uri)
        || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
{
    throw new InvalidOperationException(
        "CORSHost must contain only exact HTTP or HTTPS origins without wildcards.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowedOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
#endregion

#region Controllers (SECURED BY DEFAULT)
builder.Services.AddControllers(options =>
{
    //ALL APIs REQUIRE AUTHENTICATION BY DEFAULT
    options.Filters.Add(new AuthorizeFilter());

    options.AllowEmptyInputInBodyModelBinding = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Encoder =
        System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

    options.JsonSerializerOptions.Converters.Add(
        new JsonDateTimeConverter("yyyy-MM-dd HH:mm:ss"));
});
#endregion

#region Services
builder.Services.AddSingleton<OtpManager>();
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
builder.Services.AddTransient<ISmsService, SmsService>();
var authSecuritySettings =
    AuthSecuritySettings.FromConfiguration(builder.Configuration);
builder.Services.AddSingleton(authSecuritySettings);
builder.Services.AddSingleton<AppAuthService>();
builder.Services.AddSingleton<IAppAuthService>(
    services => services.GetRequiredService<AppAuthService>());
builder.Services.AddSingleton<AuthCookieService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
#endregion

#region JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authorizationHeader =
                    context.Request.Headers.Authorization.ToString();
                var hasBearerHeader =
                    authorizationHeader.StartsWith(
                        "Bearer ",
                        StringComparison.OrdinalIgnoreCase);

                if (!hasBearerHeader
                    && context.Request.Cookies.TryGetValue(
                        AuthSecuritySettings.CookieName,
                        out var cookieToken))
                {
                    context.Token = cookieToken;
                    context.HttpContext.Items[
                        AuthSecuritySettings.AuthenticationSourceItem] =
                        AuthSecuritySettings.CookieAuthenticationSource;
                }

                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters =
            authSecuritySettings.CreateTokenValidationParameters();
    });

builder.Services.AddAuthorization();
#endregion

#region Swagger
if (swaggerEnabled)
{
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Littera.Core",
            Version = "v1"
        });

        c.OperationFilter<AddRequiredHeaderParameter>();
        c.EnableAnnotations();

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {your JWT token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            },
        });
    });
}
#endregion

var app = builder.Build();

#region Middleware Pipeline
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("allowedOrigins");
#region Request Logging Middleware
app.Use(async (context, next) =>
{
    LogContext.PushProperty("RequestId", context.TraceIdentifier);
    LogContext.PushProperty("RequestPath", context.Request.Path);
    LogContext.PushProperty("RequestMethod", context.Request.Method);
    LogContext.PushProperty("RequestScheme", context.Request.Scheme);
    LogContext.PushProperty("RequestHost", context.Request.Host.Host);
    LogContext.PushProperty("RequestProtocol", context.Request.Protocol);
    LogContext.PushProperty("RemoteIpAddress", context.Connection.RemoteIpAddress);

    var sensitiveNameFragments =
        new[] { "token", "authorization", "password", "otp", "secret", "apikey" };
    var queryParams = string.Join("&",
        context.Request.Query.Select(q =>
        {
            var normalizedName = new string(
                q.Key.Where(char.IsLetterOrDigit).ToArray());
            var isSensitive = sensitiveNameFragments.Any(fragment =>
                normalizedName.Contains(
                    fragment,
                    StringComparison.OrdinalIgnoreCase));
            return $"{q.Key}={(isSensitive ? "[REDACTED]" : q.Value.ToString())}";
        }));
    LogContext.PushProperty("RequestParams", queryParams);

    context.Request.EnableBuffering();

    using (var reader = new StreamReader(
        context.Request.Body, Encoding.UTF8, leaveOpen: true))
    {
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        if (!string.IsNullOrWhiteSpace(body))
        {
            LogContext.PushProperty("BodyParam", "[REDACTED]");
        }
    }


#endregion


    try
    {
        await next();
        LogContext.PushProperty("ResponseStatusCode", context.Response.StatusCode);

    }

    catch (Exception ex)
    {
        var frame = new StackTrace(ex, true)
            .GetFrames()?
            .FirstOrDefault(f => f.GetFileLineNumber() > 0);

        Log.ForContext("ExceptionFile", Path.GetFileName(frame?.GetFileName()))
           .ForContext("ExceptionLineNumber", frame?.GetFileLineNumber())
           .Error(ex, "Unhandled exception");

        throw;
    }
});
#endregion

app.UseAuthentication();
app.UseCookieOriginValidation();
app.UseAuthorization();

// API Key middleware now acts as SECONDARY / INTERNAL protection
app.UseApiKeyMiddleware();

app.MapControllers();
app.Run();

#region JsonDateTimeConverter
public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format;

    public JsonDateTimeConverter(string format)
    {
        _format = format;
    }

    public override DateTime Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var s = reader.GetString();
        if (string.IsNullOrEmpty(s))
            throw new JsonException($"Expected date string in format {_format}");
        return DateTime.ParseExact(s, _format, null);
    }

    public override void Write(Utf8JsonWriter writer,
        DateTime value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
#endregion
