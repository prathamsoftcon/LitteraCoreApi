using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using LitteraCore.Controllers;
public abstract class BaseRepository
{
    protected readonly IConfiguration _configuration;
    

    protected BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
       
    }

    protected void LogSqlQuery(SqlCommand cmd, string methodName = null)
    {
        if (_configuration.GetSection("ApiKey").Value == "1")
        {
            string query = cmd.CommandText;

            foreach (SqlParameter param in cmd.Parameters)
            {
                query += $" | {param.ParameterName} = {param.Value}";
            }
            ILogger logger =new ILogger(methodName);
            _logger.LogError($"[SQL LOG] {methodName ?? "UnknownMethod"} => {query}");
        }
    }

    protected SqlDataReader ExecuteReader(SqlCommand cmd, string methodName = null)
    {
        LogSqlQuery(cmd, methodName);
        return cmd.ExecuteReader();
    }

    protected int ExecuteNonQuery(SqlCommand cmd, string methodName = null)
    {
        LogSqlQuery(cmd, methodName);
        return cmd.ExecuteNonQuery();
    }

    protected object ExecuteScalar(SqlCommand cmd, string methodName = null)
    {
        LogSqlQuery(cmd, methodName);
        return cmd.ExecuteScalar();
    }
}
