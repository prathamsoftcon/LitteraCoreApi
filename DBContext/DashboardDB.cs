using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LitteraCore.DBContext
{
    public class DashboardDB
    {
        private readonly IConfiguration _configuration;
        public DashboardDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DBAnalytics Get_Admin_DB_Analytics(string usertype, string userid, DateTime startdate, DateTime enddate, string banchid)
        {
            DBAnalytics c = new DBAnalytics();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_dashboard_analytics", con))
                {
                 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usertype", usertype);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@startdate", startdate);
                    cmd.Parameters.AddWithValue("@enddate", enddate);
                    cmd.Parameters.AddWithValue("@branchid", banchid);

                    cmd.Connection = con;
                    cmd.CommandTimeout = 5000;
                     if (con.State == ConnectionState.Open) { con.Close();}con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Convert.ToString(reader["total_participant"]) != "")
                            {
                                c.total_participant = Convert.ToInt32(reader["total_participant"]);
                            }
                           
                            if (Convert.ToString(reader["active_learners"]) != "")
                            {
                                c.active_learners = Convert.ToInt32(reader["active_learners"]);
                            }
                           
                            if(Convert.ToString(reader["avg_learning_time_therory"]) != "")
                            {
                                c.avg_learning_time_therory = Convert.ToInt32(reader["avg_learning_time_therory"]);
                            }
                            if (Convert.ToString(reader["avg_learning_time_practical"]) != "")
                            {
                                c.avg_learning_time_practical = Convert.ToInt32(reader["avg_learning_time_practical"]);
                            }

                           

                        }

                    }
                       
                 
                }


            }
           
          return c;

        }


        public List<TRG_FEEDBACK_DATA> Get_Trg_Feedback_Data(string finyear)
        {
            List<TRG_FEEDBACK_DATA> c = new List<TRG_FEEDBACK_DATA>();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_latest_avg_rating_per_training", con))
                {
                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@finyear", finyear);
                  

                    cmd.Connection = con;
                    cmd.CommandTimeout = 5000;
                     if (con.State == ConnectionState.Open) { con.Close();}con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TRG_FEEDBACK_DATA tfd = new TRG_FEEDBACK_DATA();
                            tfd.trainingid = Convert.ToString(reader["ttbfcr_Training_id"]);
                            tfd.trg_rating = Convert.ToDecimal(reader["ttbfcr_avg_rating"]);
                            tfd.no_of_response = Convert.ToInt32(reader["ttbfcr_total_rows"]);
                            c.Add(tfd);

                        }

                    }


                }


            }

            return c;

        }
    }
}
