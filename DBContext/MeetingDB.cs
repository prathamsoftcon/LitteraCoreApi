using LitteraCore.BLContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.DBContext
{
    public class MeetingDB
    {
        private readonly IConfiguration _configuration;
        public MeetingDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Meeting> Get_Meetings(string finyear,string branchid,string usertype,string userid, PaginationParam param)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_lms_get_meeting_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@finyear", finyear);
            cmd.Parameters.AddWithValue("@branchid", branchid);
            cmd.Parameters.AddWithValue("@loginuserid", userid);
            cmd.Parameters.AddWithValue("@Usertype", usertype);
            if (param.PageSize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", param.PageNumber);
                cmd.Parameters.AddWithValue("@PageSize", param.PageSize);
            }
           

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Meeting> LI = new List<Meeting>();
            foreach (DataRow row in dt.Rows)
            {
                Meeting exp = new Meeting();
                exp.ttlm_id = Convert.ToString(row["ttlm_id"]);
                exp.ttlm_zid = Convert.ToString(row["ttlm_zid"]);
                exp.ttlm_host_id = Convert.ToString(row["ttlm_host_id"]);
                exp.ttlm_ttttt_session_id = Convert.ToString(row["ttlm_ttttt_session_id"]);
                exp.ttlm_title = Convert.ToString(row["ttlm_title"]);
                exp.ttlm_agenda = Convert.ToString(row["ttlm_agenda"]);
                exp.ttlm_date = Convert.ToDateTime(row["meeting_date"]);
                exp.ttlm_time = Convert.ToString(row["ttlm_time"]);
                exp.ttlm_duration = Convert.ToInt16(row["ttlm_duration"]);

                exp.ttlm_z_join_link = Convert.ToString(row["ttlm_z_join_link"]);
                exp.ttlm_z_start_link = Convert.ToString(row["ttlm_z_start_link"]);


                exp.training_code = Convert.ToString(row["Trainingcode"]);
                exp.training_title = Convert.ToString(row["T_Name"]);
                exp.trainingid = Convert.ToString(row["trainingplanid"]);
                exp.course_director_id = Convert.ToString(row["coursedirector"]);
                exp.associate_course_director_id = Convert.ToString(row["AssociateDirector"]);
                exp.tttlm_ttlms_LitteraMeetingID = Convert.ToString(row["tttlm_ttlms_LitteraMeetingID"]);

                LI.Add(exp);

            }



            return LI;
        }

        public bool Delete_Meeting(string meetingid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_lms_delete_meeting", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttlm_id", meetingid);
           
            cmd.ExecuteNonQuery();


            return true;
        }


        public List<Meeting> Get_Trg_Meetings(string trainingid)
        {

            List<Meeting> LI = new List<Meeting>();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_lms_get_trg_meeting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@trainingid", trainingid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Meeting exp = new Meeting
                        {
                            ttlm_id = Convert.ToString(reader["ttlm_id"]),
                            ttlm_zid = Convert.ToString(reader["ttlm_zid"]),
                            ttlm_host_id = Convert.ToString(reader["ttlm_host_id"]),
                            ttlm_ttttt_session_id = Convert.ToString(reader["ttlm_ttttt_session_id"]),
                            ttlm_title = Convert.ToString(reader["ttlm_title"]),
                            ttlm_agenda = Convert.ToString(reader["ttlm_agenda"]),
                            ttlm_date = Convert.ToDateTime(reader["ttlm_date"]),
                            ttlm_time = Convert.ToString(reader["ttlm_time"]),
                            ttlm_duration = Convert.ToInt16(reader["ttlm_duration"]),
                            ttlm_z_join_link = Convert.ToString(reader["ttlm_z_join_link"]),
                            ttlm_z_start_link = Convert.ToString(reader["ttlm_z_start_link"]),
                            training_code = Convert.ToString(reader["Trainingcode"]),
                            training_title = Convert.ToString(reader["T_Name"]),
                            trainingid = Convert.ToString(reader["trainingplanid"]),
                            course_director_id = Convert.ToString(reader["coursedirector"]),
                            associate_course_director_id = Convert.ToString(reader["AssociateDirector"]),
                            tttlm_ttlms_LitteraMeetingID = Convert.ToString(reader["tttlm_ttlms_LitteraMeetingID"])
                        };

                        LI.Add(exp);
                    }
                }
            }

            return LI;

        }


    }
}
