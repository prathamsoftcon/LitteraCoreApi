using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace LitteraCore.DBContext
{
    public class InterviewDB
    {
        private readonly IConfiguration _configuration;
        public InterviewDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<interviewQuestion> Get_Interview_Questions()
        {

            List<interviewQuestion> IQ = new List<interviewQuestion>();

            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Interview.json"));
            IQ = JsonConvert.DeserializeObject<List<interviewQuestion>>(jsontxt);





            return IQ;
        }


        //public List<interviewQuestion> Get_Interview_Data(string tpad_id)
        //{
           
        //    string connectionString = _configuration.GetConnectionString("LitteraDatabase");
        //    SqlConnection con = new SqlConnection(connectionString);
          
        //    string query = "SELECT * FROM trainingplan.tbl_tp_activity_data WHERE tpad_id = @TpadId";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    using (SqlCommand command = new SqlCommand(query, connection))
        //    {
        //        // Add parameter with value
        //        command.Parameters.AddWithValue("@TpadId", tpad_id);

        //        connection.Open();

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                // Process each row
        //                Console.WriteLine(reader["tpad_id"]); // example
        //            }
        //        }
        //    }

        //    return IQ;
        //}




    }
}
