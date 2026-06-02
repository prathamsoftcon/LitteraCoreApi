using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Middleware;
using Microsoft.OpenApi.Models;
using Serilog; 
using Serilog.Context;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Text.Json;
//using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using LitteraCore.Common.OTP;
using LitteraCore.Common.EmailService;
using LitteraCore.Common.SmsService;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using Azure.Core;
using MailKit;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
bool swaggerEnabled =
    builder.Configuration.GetValue<bool>("Swagger:Enabled");

//var columnOptions = new ColumnOptions
//{
//    AdditionalColumns = new Collection<SqlColumn>
//    {
//        new SqlColumn { ColumnName = "RequestId", DataType = SqlDbType.NVarChar, DataLength = 128 },
//        new SqlColumn { ColumnName = "RequestPath", DataType = SqlDbType.NVarChar, DataLength = 256 },
//        new SqlColumn { ColumnName = "RequestMethod", DataType = SqlDbType.NVarChar, DataLength = 10 },
//        new SqlColumn { ColumnName = "RequestScheme", DataType = SqlDbType.NVarChar, DataLength = 10 },
//        new SqlColumn { ColumnName = "RequestHost", DataType = SqlDbType.NVarChar, DataLength = 256 },
//        new SqlColumn { ColumnName = "RequestProtocol", DataType = SqlDbType.NVarChar, DataLength = 10 },
//        new SqlColumn { ColumnName = "RemoteIpAddress", DataType = SqlDbType.NVarChar, DataLength = 45 },
//        new SqlColumn { ColumnName = "Exception", DataType = SqlDbType.NVarChar, DataLength = 45 },
//        new SqlColumn { ColumnName = "CreatedOn", DataType = SqlDbType.NVarChar, DataLength = 45 }
//    }
//};

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
     .WriteTo.File("logs/error.log",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
        //outputTemplate: "Date - {Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {NewLine} RequestId: {RequestId} {NewLine} RequestMethod:{RequestMethod} URL: {RequestScheme}://{RequestHost}/{RequestPath}?{RequestParams} {NewLine} BodyParameter : {BodyParam}  {NewLine} {Message:lj}{NewLine}{Exception}"
        outputTemplate: "Date1 - {Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {NewLine}" +
                "File: {ExceptionFile} | Line: {ExceptionLineNumber}{NewLine}" +
                "RequestId: {RequestId} {NewLine} RequestMethod:{RequestMethod} " +
                "URL: {RequestScheme}://{RequestHost}/{RequestPath}?{RequestParams} {NewLine}" +
                "BodyParameter : {BodyParam}  {NewLine} {Message:lj}{NewLine}{Exception} {NewLine} {Properties:j}  {NewLine}")
    .CreateLogger();

//.WriteTo.MSSqlServer(
//    connectionString: builder.Configuration.GetConnectionString("LitteraDatabase"),
//    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
//    restrictedToMinimumLevel: LogEventLevel.Error, // Only log errors
//    columnOptions: columnOptions) // Custom column options
//.CreateLogger();
builder.Host.UseSerilog();
// Read allowed origins from configuration
var allowedOrigins = builder.Configuration.GetSection("CORSHost").Get<string[]>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowedOrigins",
        policyBuilder =>
        {
            policyBuilder.WithOrigins(allowedOrigins)
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials(); // Use if you need to allow credentials
        });
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.AllowEmptyInputInBodyModelBinding = true;
});
builder.Services.AddControllers()
 .AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
     options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
     options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
     options.JsonSerializerOptions.WriteIndented = true;
     options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

     // Set the default DateTime format
     options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter("yyyy-MM-dd HH:mm:ss"));

 });


builder.Services.AddSingleton<OtpManager>();
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
builder.Services.AddTransient<ISmsService, SmsService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Littera.Core", Version = "v1" });

//     c.OperationFilter<AddRequiredHeaderParameter>();
//     c.EnableAnnotations();

// });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    });
}
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
// app.UseSwagger();
// app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("allowedOrigins");
app.UseAuthorization();
app.UseApiKeyMiddleware();
app.MapControllers();

app.Use(async (context, next) =>
{
    // Enrich Serilog log context with HTTP request details
    LogContext.PushProperty("RequestId", context.TraceIdentifier);
    LogContext.PushProperty("RequestPath", context.Request.Path);
    LogContext.PushProperty("RequestMethod", context.Request.Method);
    LogContext.PushProperty("RequestScheme", context.Request.Scheme);
    LogContext.PushProperty("RequestHost", context.Request.Host.Host);
    LogContext.PushProperty("RequestProtocol", context.Request.Protocol);
    LogContext.PushProperty("RemoteIpAddress", context.Connection.RemoteIpAddress);
   

    string str = "";
    foreach (var queryParam in context.Request.Query)
    {
        str += queryParam.Key + "=" + queryParam.Value + "&";

    }

    LogContext.PushProperty("RequestParams", str);

    context.Request.EnableBuffering();

  

    // Read the request body to a string
    var bodyparam = "";
    using (var reader = new StreamReader(
        context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
    {
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        if (!string.IsNullOrEmpty(body))
        {
            // Assuming JSON body, you can deserialize to a dynamic object or a specific type
            var bodyParams = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
            LogContext.PushProperty("BodyParam", bodyParams);
        }
    }


    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var frame = new StackTrace(ex, true)
           .GetFrames()?
           .FirstOrDefault(f => f.GetFileLineNumber() > 0 && f.GetFileName() != null);

        string fileName = Path.GetFileName(frame?.GetFileName());
        int? lineNumber = frame?.GetFileLineNumber() ?? 0;

        // Attach props directly to event
        Log.ForContext("ExceptionFile", fileName)
           .ForContext("ExceptionLineNumber", lineNumber)
           .Error(ex, "Unhandled exception in {ExceptionFile} at line {ExceptionLineNumber}");

        throw;
    }

});



app.Run();

public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format;
    public JsonDateTimeConverter(string format)
    {
        _format = format;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), _format, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
   

}

