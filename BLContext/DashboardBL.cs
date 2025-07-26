using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.BLContext
{
    public class DashboardBL
    {
        private readonly IConfiguration _configuration;
        public DashboardBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<TRG_FEEDBACK_DATA> Get_Trg_Feedback_Data(string finyear)
        {
            DashboardDB db = new DashboardDB(_configuration);
            List<TRG_FEEDBACK_DATA> c = new List<TRG_FEEDBACK_DATA>();
            c = db.Get_Trg_Feedback_Data(finyear);
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");

            return c;

        }

        public static string GetFinancialYear(DateTime fromDate, DateTime toDate)
        {
            // Validate the range (optional, depending on your needs)
            if (fromDate > toDate)
                throw new ArgumentException("From date cannot be later than to date.");

            // Determine the financial year based on the fromDate
            int startYear;
            if (fromDate.Month >= 4)
            {
                startYear = fromDate.Year;
            }
            else
            {
                startYear = fromDate.Year - 1;
            }

            return $"{startYear}-{startYear + 1}";
        }


        public  bool validate_external_user_key(string secretKey)
        {
            string Server_secretKey = _configuration["secretKey"];
            if (secretKey == Server_secretKey)
            {
                return true;
            }
            else
            {
                return false;
            }
         

           
        }

    }
}
