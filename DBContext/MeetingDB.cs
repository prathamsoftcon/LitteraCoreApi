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
            if (con.State != ConnectionState.Open) { con.Open(); }
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
            if (con.State != ConnectionState.Open) { con.Open(); }
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
                if (con.State != ConnectionState.Open) { con.Open(); }
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

        // ------------------------------------------------------------------
        // Added 2026-09-25 - frm_session_meeting.aspx -> React migration.
        // The old page reached all three procedures below through the
        // generic /TrainingAPI/Save_Data and /TrainingAPI/Get_Data
        // dispatchers of API_ERP_TRAINING (TRAININGAPIController.vb L963 /
        // Datamanager.vb Save_Common_Data L677). None of the three procedures
        // was called anywhere in LitteraCoreReactAPI\DBContext before this
        // (checked every *.cs file outside DBContext\old\).
        // ------------------------------------------------------------------

        // Reproduces Save_Common_Data's parameter rules exactly: an empty
        // value is NOT sent at all (the proc's own default applies), and the
        // literal string "NULL" is sent as DBNull.
        private static void AddIfPresent(SqlCommand cmd, string name, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.ToUpperInvariant() == "NULL")
            {
                cmd.Parameters.AddWithValue(name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(name, value);
            }
        }

        public bool Save_Meeting(MeetingSaveRequest m)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_lms_save_meeting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

                AddIfPresent(cmd, "@ttlm_id", m.ttlm_id);
                AddIfPresent(cmd, "@ttlm_host_id", m.ttlm_host_id);
                AddIfPresent(cmd, "@ttlm_ttttt_session_id", m.ttlm_ttttt_session_id);
                AddIfPresent(cmd, "@ttlm_title", m.ttlm_title);
                AddIfPresent(cmd, "@ttlm_agenda", m.ttlm_agenda);
                AddIfPresent(cmd, "@ttlm_date", m.ttlm_date);
                AddIfPresent(cmd, "@ttlm_time", m.ttlm_time);
                AddIfPresent(cmd, "@ttlm_duration", m.ttlm_duration);
                AddIfPresent(cmd, "@ttlm_zid", m.ttlm_zid);
                AddIfPresent(cmd, "@ttlm_zencpwd", m.ttlm_zencpwd);
                AddIfPresent(cmd, "@ttlm_zpwd", m.ttlm_zpwd);
                AddIfPresent(cmd, "@ttlm_z_join_link", m.ttlm_z_join_link);
                AddIfPresent(cmd, "@ttlm_z_start_link", m.ttlm_z_start_link);
                AddIfPresent(cmd, "@ttlm_z_hostVideo", m.ttlm_z_hostVideo);
                AddIfPresent(cmd, "@ttlm_z_participantVideo", m.ttlm_z_participantVideo);
                AddIfPresent(cmd, "@ttlm_z_mute_upon_entry", m.ttlm_z_mute_upon_entry);
                AddIfPresent(cmd, "@ttlm_z_watermark", m.ttlm_z_watermark);
                AddIfPresent(cmd, "@ttlm_z_approval_type", m.ttlm_z_approval_type);
                AddIfPresent(cmd, "@ttlm_z_registration_type", m.ttlm_z_registration_type);
                AddIfPresent(cmd, "@ttlm_z_audio", m.ttlm_z_audio);
                AddIfPresent(cmd, "@ttlm_z_recording", m.ttlm_z_recording);
                AddIfPresent(cmd, "@ttlm_z_joinBeforeHost", m.ttlm_z_joinBeforeHost);
                AddIfPresent(cmd, "@ttlm_z_hostemail", m.ttlm_z_hostemail);
                AddIfPresent(cmd, "@ttlm_z_alt_hostemail", m.ttlm_z_alt_hostemail);
                AddIfPresent(cmd, "@ttlm_z_response_data", m.ttlm_z_response_data);
                AddIfPresent(cmd, "@tttlm_type", m.tttlm_type);
                AddIfPresent(cmd, "@tttlm_isenable", m.tttlm_isenable);
                AddIfPresent(cmd, "@tttlm_ttlms_LitteraMeetingID", m.tttlm_ttlms_LitteraMeetingID);

                // Old page passed CreatedOnParameter="@ttlm_created_on", which
                // the dispatcher filled with the web server's current local
                // time as a "yyyy/MM/dd HH:mm:ss" string
                // (Datamanager.vb Get_CreatedOn_Server) - same here.
                cmd.Parameters.AddWithValue("@ttlm_created_on", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        // Reads a column by name without throwing when the procedure doesn't
        // return it (DataColumnCollection.Contains is case-insensitive).
        private static string? Col(DataRow row, string name)
        {
            if (!row.Table.Columns.Contains(name) || row[name] == DBNull.Value)
            {
                return null;
            }
            return Convert.ToString(row[name]);
        }

        // Normalises a date column to yyyy-MM-dd whether the procedure
        // returns a real datetime or a string.
        private static string? DateCol(DataRow row, string name)
        {
            if (!row.Table.Columns.Contains(name) || row[name] == DBNull.Value)
            {
                return null;
            }
            object value = row[name];
            if (value is DateTime dt)
            {
                return dt.ToString("yyyy-MM-dd");
            }
            string raw = Convert.ToString(value) ?? "";
            if (DateTime.TryParse(raw, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsed))
            {
                return parsed.ToString("yyyy-MM-dd");
            }
            return raw;
        }

        public MeetingEditData? Get_Meeting_Data(string meetingid)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_lms_get_meeting_data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@tttlm_id", meetingid);

                // Old page read TableIndex=0.
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0) { dt = ds.Tables[0]; }
            }

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            return new MeetingEditData
            {
                ttlm_id = Col(row, "ttlm_id"),
                ttlm_zid = Col(row, "ttlm_zid"),
                ttlm_host_id = Col(row, "ttlm_host_id"),
                ttlm_ttttt_session_id = Col(row, "ttlm_ttttt_session_id"),
                ttlm_title = Col(row, "ttlm_title"),
                ttlm_agenda = Col(row, "ttlm_agenda"),
                ttlm_date = DateCol(row, "ttlm_date"),
                ttlm_time = Col(row, "ttlm_time"),
                ttlm_duration = Col(row, "ttlm_duration"),
                ttlm_zpwd = Col(row, "ttlm_zpwd"),
                ttlm_z_hostvideo = Col(row, "ttlm_z_hostvideo"),
                ttlm_z_participantvideo = Col(row, "ttlm_z_participantvideo"),
                ttlm_z_mute_upon_entry = Col(row, "ttlm_z_mute_upon_entry"),
                ttlm_z_jointimebeforehost = Col(row, "ttlm_z_jointimebeforehost"),
                ttlm_z_recording = Col(row, "ttlm_z_recording"),
                ttlm_z_hostemail = Col(row, "ttlm_z_hostemail"),
                ttlm_z_alt_hostemail = Col(row, "ttlm_z_alt_hostemail"),
                tttlm_ttlms_litterameetingid = Col(row, "tttlm_ttlms_litterameetingid"),
            };
        }

        public MeetingSessionDetail? Get_Meeting_Session_Detail(string sessionid)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("TrainingPlan.Proc_tp_get_session_detail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@sessionid", sessionid);

                // Old page read TableIndex=0.
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0) { dt = ds.Tables[0]; }
            }

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            return new MeetingSessionDetail
            {
                session_id = sessionid,
                t_name = Col(row, "t_name"),
                t_code = Col(row, "t_code"),
                subject = Col(row, "subject"),
                session_dt = DateCol(row, "session_dt"),
                starttime = Col(row, "starttime"),
                duration = Col(row, "duration"),
                cd_mailid = Col(row, "cd_mailid"),
                acd_mailid = Col(row, "acd_mailid"),
                faculty_mailid = Col(row, "faculty_mailid"),
            };
        }

    }
}
