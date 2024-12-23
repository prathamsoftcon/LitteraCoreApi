using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.DBContext
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            return new SqlConnection(connectionString);
        }
    }
}
