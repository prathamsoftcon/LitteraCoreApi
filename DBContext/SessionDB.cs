using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace LitteraCore.DBContext
{
    public class SessionDB
    {
        private readonly IConfiguration _configuration;
        public SessionDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Session> Get_Session_Data_By_Trg(string trainingid)
        {
           
            List<Session> sessiondata = new List<Session>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("select * from  trainingplan.Vw_tp_trg_time_table where TrainingId='" + trainingid + "'", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "ttttt_session_id is not null";
            dt = dt.DefaultView.ToTable();
            //Condition to sort data on basis of session no in case of self paced training
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["trg_type"].ToString() == "2")
                {
                    dt.Columns.Add("session_no_int", typeof(Int32), "ttttt_session_no");
                    dt.DefaultView.Sort = "session_no_int";
                    dt = dt.DefaultView.ToTable();
                }
                else
                {

                    dt.Columns.Add("session_no_int", typeof(Int32), "ttttt_session_no");

                    dt.Columns.Add("ttttt_session_dt_date", typeof(DateTime), "ttttt_session_dt");
                    dt.Columns.Add("ttttt_session_time_time", typeof(DateTime), "ttttt_session_time");

                    DataTable dtselfpacesession = new DataTable();
                    DataTable dtOtherThanselfpacesession = new DataTable();
                    dt.DefaultView.RowFilter = "ttttt_type=11";
                    dtselfpacesession = dt.DefaultView.ToTable();
                    dtselfpacesession.DefaultView.Sort = "session_no_int asc";
                    dtselfpacesession = dtselfpacesession.DefaultView.ToTable();






                    dt.DefaultView.RowFilter = "ttttt_type <> 11";
                    dtOtherThanselfpacesession = dt.DefaultView.ToTable();

                    dtOtherThanselfpacesession.DefaultView.Sort = "ttttt_session_dt_date,ttttt_session_time_time asc";
                    dtOtherThanselfpacesession = dtOtherThanselfpacesession.DefaultView.ToTable();

                    DataTable dtallsession = new DataTable();
                    dtallsession.Merge(dtselfpacesession);
                    dtallsession.AcceptChanges();

                    dtallsession.Merge(dtOtherThanselfpacesession);
                    dtallsession.AcceptChanges();

                    dt = dtallsession;
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                Session vw = new Session();
                vw.trainingid = Convert.ToString(row["trainingid"]);
                vw.ttttt_session_row_no = Convert.ToString(row["ttttt_session_row_no"]);
                vw.ttttt_session_id = Convert.ToString(row["ttttt_session_id"]);
                vw.ttttt_timetableid = Convert.ToString(row["ttttt_timetableid"]);
                vw.ttttt_facultyid = Convert.ToString(row["ttttt_facultyid"]);
                vw.ttttt_content_desc = Convert.ToString(row["ttttt_content_desc"]);
                vw.ttttt_session_dt = Convert.ToDateTime(row["ttttt_session_dt"]).ToString("yyyy/MM/dd");
                vw.ttttt_session_time = Convert.ToString(row["ttttt_session_time"]);
                vw.ttttt_session_duration = Convert.ToString(row["ttttt_session_duration"]);
                vw.ttttt_session_day = Convert.ToInt32(row["ttttt_session_day"]);
                vw.ttttt_is_joint_session = Convert.ToString(row["ttttt_is_joint_session"]);
                vw.ttttt_session_end_time = Convert.ToDateTime(row["ttttt_session_end_time"]).ToString("yyyy/MM/dd hh:mm:ss");
                vw.ttttt_session_no = Convert.ToInt32(row["ttttt_session_no"]);
                vw.ttttt_status = Convert.ToString(row["ttttt_status"]);
                vw.tttttf_status = Convert.ToString(row["tttttf_status"]);
                vw.ttttt_remark = Convert.ToString(row["ttttt_remark"]);
                vw.ttttt_session_week = Convert.ToInt32(row["ttttt_session_week"]);
                vw.ttttt_type = Convert.ToInt32(row["ttttt_type"]);
                if (row["ttttt_session_duration_type"] != DBNull.Value)
                {
                    vw.ttttt_session_duration_type = Convert.ToInt32(row["ttttt_session_duration_type"]);
                }

                if (Convert.ToString(row["ttttt_complimentory"]) != "")
                {
                    vw.ttttt_complimentory = Convert.ToInt32(row["ttttt_complimentory"]);
                }
                else
                {
                    vw.ttttt_complimentory = 0;
                }

                vw.ttttt_tag = Convert.ToString(row["ttttt_tag"]);
                vw.ttttt_subject = Convert.ToString(row["ttttt_subject"]);
                vw.participant_seession_required = Convert.ToString(row["participant_seession_required"]);

                vw.facultyname = Convert.ToString(row["facultyname"]);
                vw.hfacultyname = Convert.ToString(row["hfacultyname"]);
                if (Convert.ToString(row["ttttt_facultyid"]) != "")
                {
                    if (Convert.ToString(row["facultyimgpath"]) != "")
                    {
                        vw.facultyimgpath = Convert.ToString(row["facultyimgpath"]);
                    }
                    else
                    {
                        vw.facultyimgpath = null;
                    }
                }

                vw.Attendance = Convert.ToString(row["Attendance"]);
                if (row["ttttt_session_duration_type"] != DBNull.Value)
                {
                    vw.ttttt_session_duration_type_name = ((Common.CommonEnum.SessionDurationType)Convert.ToInt32(row["ttttt_session_duration_type"])).ToString();
                }

                vw.Session_type_icon = Session.Get_Session_Icon(vw.ttttt_type);
                vw.Session_type_name = Session.Get_Session_Type_Name(vw.ttttt_type);
                if (row["ttttt_module_no"].ToString() != "")
                {
                    vw.module = Convert.ToInt32(row["ttttt_module_no"].ToString());
                }

                if (row["ttttt_module_no"].ToString() != "")
                {
                    vw.modulename = Get_Session_Module_Name_by_id(row["ttttt_module_no"].ToString());
                }


                if (row["ttttt_completion_type"] != null)
                {
                    if (row["ttttt_completion_type"].ToString() != "")
                    {
                        vw.completiontype = JsonConvert.DeserializeObject<CompletionType>(row["ttttt_completion_type"].ToString());
                    }
                }



                DataTable dtfaulties = new DataTable();
                dt.DefaultView.RowFilter = "ttttt_session_id='" + Convert.ToString(row["ttttt_session_id"]) + "'";
                dtfaulties = dt.DefaultView.ToTable();
                List<SessionFaculties> lsf = new List<SessionFaculties>();
                foreach (DataRow drf1 in dtfaulties.Rows)
                {
                    if (Convert.ToString(drf1["ttttt_facultyid"]) != "")
                    {
                        string imgpath = "";
                        if (Convert.ToString(drf1["facultyimgpath"]) != "")
                        {
                            imgpath = Convert.ToString(drf1["facultyimgpath"]);
                        }
                        else
                        {
                            imgpath = null;
                        }
                        lsf.Add(new SessionFaculties { facultyid = Convert.ToString(drf1["ttttt_facultyid"]), facultyname = Convert.ToString(drf1["facultyname"]), hfacultyname = Convert.ToString(drf1["hfacultyname"]), facultuimg = imgpath });
                    }

                }

                vw.sessionFaculties = lsf.ToArray();

                sessiondata.Add(vw);
            }



            sessiondata = sessiondata.Where(o => o.ttttt_timetableid != null).ToList();

            return sessiondata;
        }

        public static string Get_Session_Module_Name_by_id(string id)
        {
            string modulename = "";
            List<SessionModule> M = new List<SessionModule>();
            M = GET_SESSION_MODULE();
            modulename = M.Where(o => o.ID.ToString() == id.ToString()).FirstOrDefault().DisplayName;


            return modulename;
        }

        public static List<SessionModule> GET_SESSION_MODULE()
        {
            //string usertype = Get_Login_User_type();
            List<SessionModule> ct = new List<SessionModule>();
            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Modules.json"));
            ct = JsonConvert.DeserializeObject<List<SessionModule>>(jsontxt);

            return ct;
        }


        public bool Save_Session_Notes(Notes note)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("[trainingplan].[proc_tp_ins_upd_session_notes]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttsn_id", note.ttsn_id);

            cmd.Parameters.AddWithValue("@ttsn_training_id", note.ttsn_training_id);
            cmd.Parameters.AddWithValue("@ttsn_session_id", note.ttsn_session_id);
            string p1 = JsonConvert.SerializeObject(note.ttsn_notes);
            cmd.Parameters.AddWithValue("@ttsn_notes", p1);
            cmd.Parameters.AddWithValue("@ttsn_created_by", note.ttsn_created_by);
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }


        public List<Notes> Get_Session_Notes(string userid, string trainingid = null, string sessionid = null)
        {
            List<Notes> notes = new List<Notes>();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("[trainingplan].[proc_tp_get_session_notes]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@ttsn_training_id", trainingid);
            }
            if (sessionid != null)
            {
                cmd.Parameters.AddWithValue("@ttsn_session_id", sessionid);
            }
            cmd.Parameters.AddWithValue("@ttsn_created_by", userid);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Notes N = new Notes();
                N.ttsn_id = Convert.ToString(dr["ttsn_id"]);
                N.ttsn_notes = JsonConvert.DeserializeObject<notes_detail[]>(Convert.ToString(dr["ttsn_notes"]));
                N.ttsn_session_id = Convert.ToString(dr["ttsn_session_id"]);
                N.ttsn_training_id = Convert.ToString(dr["ttsn_training_id"]);
                N.ttsn_created_by = Convert.ToString(dr["ttsn_createdon"]);
                N.ttsn_createdon = Convert.ToDateTime(dr["ttsn_createdon"]).ToString("yyyy/MM/dd hh:mm:ss");
                notes.Add(N);

            }
            notes = notes.OrderByDescending(o => o.ttsn_createdon).ToList();
            return notes;

        }

        public List<TrgComment> Get_Trg_Comments(string trainingid, string Sessionid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); 
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_trg_comment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            if (Sessionid != null)
            {
                cmd.Parameters.AddWithValue("@sessionid", Sessionid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@sessionid", DBNull.Value);
            }




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<TrgComment> LI = new List<TrgComment>();
            foreach (DataRow row in dt.Rows)
            {
                TrgComment exp = new TrgComment();
                exp.tttcm_commentid = Convert.ToString(row["tttcm_commentid"]);
                exp.tttcm_trg_id = Convert.ToString(row["tttcm_trg_id"]);
                exp.tttcm_session_id = Convert.ToString(row["tttcm_session_id"]);
                exp.tttcm_created_by = Convert.ToString(row["tttcm_created_by"]);
                exp.tttcm_comment = Convert.ToString(row["tttcm_comment"]);
                exp.tttcm_is_delete = Convert.ToInt32(row["tttcm_is_delete"]);
                exp.tttcm_createdon = Convert.ToDateTime(row["tttcm_createdon"]);
                exp.AgencyName = Convert.ToString(row["AgencyName"]);
                List<TrgComment_reply> c = new List<TrgComment_reply>();
                if (row["comment"].ToString() != "")
                {
                    c = JsonConvert.DeserializeObject<List<TrgComment_reply>>(row["comment"].ToString());
                }

                exp.comments = c.ToArray();
                LI.Add(exp);

            }



            return LI;
        }

        public bool Save_Trg_Comment(TrgComment c)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); 
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_insert_trg_comment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tttcm_commentid", c.tttcm_commentid);
            cmd.Parameters.AddWithValue("@tttcm_trg_id", c.tttcm_trg_id);
            if (c.tttcm_session_id != null)
            {
                cmd.Parameters.AddWithValue("@tttcm_session_id", c.tttcm_session_id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tttcm_session_id", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@tttcm_created_by", c.tttcm_created_by);
            cmd.Parameters.AddWithValue("@tttcm_comment", c.tttcm_comment);
            cmd.ExecuteNonQuery();

            con.Close();

            return true;
        }

        public bool Save_Trg_Comment_reply(TrgComment_reply r)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_insert_trg_comment_reply", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tttcr_tttcm_commentid", r.tttcr_tttcm_commentid);
            cmd.Parameters.AddWithValue("@tttcr_replied_by", r.tttcr_replied_by);
            cmd.Parameters.AddWithValue("@tttcr_comment", r.tttcr_comment);

            cmd.ExecuteNonQuery();

            con.Close();

            return true;
        }

        public bool Save_Session_Content_Feedback(string trainingid, string sessionid, string loginagencyid, SessionFeedback[] Feedback, SqlTransaction transaction = null)
        {
            bool issaved = false;
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            cmd = new SqlCommand("Trainingplan.proc_tp_insert_content_feedback", con);
            cmd.Parameters.AddWithValue("@ttcf_id", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("@ttcf_Participant_id", loginagencyid);
            cmd.Parameters.AddWithValue("@ttcf_Training_id", trainingid);
            cmd.Parameters.AddWithValue("@ttcf_Session_id", sessionid);
            string p1 = JsonConvert.SerializeObject(Feedback);
            cmd.Parameters.AddWithValue("@feedbackjson", p1);
            cmd.Parameters.AddWithValue("@ttcf_created_by", loginagencyid);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }

        public bool Save_Session_Faculty_Feedback(string trainingid, string sessionid, string loginagencyid, SessionFeedback[] Feedback, SqlTransaction transaction = null)
        {
            bool issaved = false;
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            cmd = new SqlCommand("Trainingplan.proc_tp_insert_faculty_feedback", con);
            cmd.Parameters.AddWithValue("@ttff_id", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("@ttff_Participant_id", loginagencyid);
            cmd.Parameters.AddWithValue("@ttff_Training_id", trainingid);
            cmd.Parameters.AddWithValue("@ttff_Session_id", sessionid);
            string p1 = JsonConvert.SerializeObject(Feedback);
            cmd.Parameters.AddWithValue("@feedbackjson", p1);
            cmd.Parameters.AddWithValue("@ttff_created_by", loginagencyid);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }

        public bool Save_Session_Content_Faculty_Feedback(string trainingid, string sessionid, string loginagencyid, SessionFeedback[] Feedback)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlTransaction st = con.BeginTransaction();
            try
            {
                List<SessionFeedback> Feedback_content = new List<SessionFeedback>();
                Feedback_content = Feedback.Where(o => o.contentid != null).ToList();
                Save_Session_Content_Feedback(trainingid, sessionid, loginagencyid, Feedback_content.ToArray(), st);
                List<SessionFeedback> Feedback_Faculty = new List<SessionFeedback>();
                Feedback_Faculty = Feedback.Where(o => o.facultyid != null).ToList();
                Save_Session_Faculty_Feedback(trainingid, sessionid, loginagencyid, Feedback_Faculty.ToArray(), st);

                st.Commit();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return false;
            }
            finally
            {
                con.Close();

            }
        }

        public bool Update_Session_Status(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("Trainingplan.proc_update_participant_session_status", con);
            cmd.Parameters.AddWithValue("@ttpss_participant_id", Participantid);
            cmd.Parameters.AddWithValue("@ttpss_session_id", Sessionid);
            cmd.Parameters.AddWithValue("@ttpss_onscreen_time", timeonsession);
            cmd.Parameters.AddWithValue("@ttpss_status", status);
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            cmd.Parameters.AddWithValue("@BranchId", branchid);



            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.ExecuteNonQuery();

            return true;
        }



        public bool Check_Content_Feedback_Exists(string userid, string trainingid, string sessionid)
        {
            bool isexist = false;
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("trainingplan.proc_tp_chk_content_feedback", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ttcf_Training_id", trainingid);
            cmd.Parameters.AddWithValue("@ttcf_Session_id", sessionid);
            cmd.Parameters.AddWithValue("@ttcf_Participant_id", userid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            DataTable dtfacultyfeedback= Get_Faculty_Feedback(userid,trainingid,sessionid);


            if (dt.Rows.Count > 0 || dtfacultyfeedback.Rows.Count>0)
            {
                isexist = true;
            }
            else
            {
                isexist = false;
            }


            return isexist;
        }


        public DataTable Get_Faculty_Feedback(string userid, string trainingid, string sessionid)
        {
            bool isexist = false;
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("trainingplan.proc_tp_chk_faculty_feedback", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ttff_Training_id", trainingid);
            cmd.Parameters.AddWithValue("@ttff_Session_id", sessionid);
            cmd.Parameters.AddWithValue("@ttff_Participant_id", userid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            
            return dt;
        }

        public List<user_session_status> Get_Participant_session_status(string Participantid, string trainingid = null, string sessionid = null)
        {

            List<user_session_status> trgdata = new List<user_session_status>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_get_participant_session_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@trainingid", trainingid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);

            }
            if (sessionid != null)
            {
                cmd.Parameters.AddWithValue("@sessionid", sessionid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@sessionid", DBNull.Value);

            }
            cmd.Parameters.AddWithValue("@participantid", Participantid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                user_session_status vw = new user_session_status();
                vw.trainingid = Convert.ToString(row["ttttt_trainingid"]);
                vw.status = Convert.ToString(row["ttpss_status"]);
                vw.sessionid = Convert.ToString(row["ttpss_session_id"]);
                vw.userid = Convert.ToString(row["ttpss_participant_id"]);
                trgdata.Add(vw);
            }





            return trgdata;
        }


        public List<Meeting> Get_Session_Meetings(string sessionid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_lms_get_meeting", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@session_id", sessionid);
           
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Meeting> LI = new List<Meeting>();
            foreach (DataRow row in dt.Rows)
            {
                Meeting exp = new Meeting();
                exp.ttlm_id = Convert.ToString(row["ttlm_id"]);
                exp.ttlm_host_id = Convert.ToString(row["ttlm_host_id"]);
                exp.ttlm_ttttt_session_id = Convert.ToString(row["ttlm_ttttt_session_id"]);
                exp.ttlm_title = Convert.ToString(row["ttlm_title"]);
                exp.ttlm_agenda = Convert.ToString(row["ttlm_agenda"]);
                exp.ttlm_date = Convert.ToDateTime(row["ttlm_date"]);
                exp.ttlm_time = Convert.ToString(row["ttlm_time"]);
                exp.ttlm_duration = Convert.ToInt16(row["ttlm_duration"]);

                exp.ttlm_z_join_link = Convert.ToString(row["ttlm_z_join_link"]);
                exp.ttlm_z_start_link = Convert.ToString(row["ttlm_z_start_link"]);
                exp.ttlm_duration = Convert.ToInt16(row["ttlm_duration"]);
                LI.Add(exp);

            }



            return LI;
        }

        public static int Get_Session_Group(int sessiontype)
        {
            int group = 0;
            var group1 = new Session_Study_Group();
            var group2 = new Session_Sport_Group();
            var group3 = new Session_Evaluation_Group();
            var group4 = new Session_Breaks_Group();
            var group5 = new Session_Tours_Group();
            var group6 = new Session_Self_Paced();

            foreach (var value in group1.Values)
            {
                if ((int)value == sessiontype)
                {
                    return (int)CommonEnum.SessionGroup.Study_Group;
                }
            }
            foreach (var value in group2.Values)
            {
                if ((int)value == sessiontype)
                {
                    return (int)CommonEnum.SessionGroup.Sport_Group;
                }
            }
            foreach (var value in group3.Values)
            {
                if ((int)value == sessiontype)
                {
                    return (int)CommonEnum.SessionGroup.Evaluation_Group;
                }
            }
            foreach (var value in group4.Values)
            {
                if ((int)value == sessiontype)
                {
                    return (int)CommonEnum.SessionGroup.Breaks_Group;
                }
            }
            foreach (var value in group5.Values)
            {
                if ((int)value == sessiontype)
                {
                    return (int)CommonEnum.SessionGroup.Tours_Group;
                }
            }
            foreach (var value in group6.Values)
            {
                if ((int)value == sessiontype)
                {
                    return (int)CommonEnum.SessionGroup.Self_paced;
                }
            }


            return 0;
        }

        public class Session_Study_Group
        {
            public List<CommonEnum.SESSION_TYPE> Values { get; } = new List<CommonEnum.SESSION_TYPE> { CommonEnum.SESSION_TYPE.Personled, CommonEnum.SESSION_TYPE.Group_discussion, CommonEnum.SESSION_TYPE.Presentation, CommonEnum.SESSION_TYPE.Practical };
        }
        public class Session_Sport_Group
        {
            public List<CommonEnum.SESSION_TYPE> Values { get; } = new List<CommonEnum.SESSION_TYPE> { CommonEnum.SESSION_TYPE.Physical_training, CommonEnum.SESSION_TYPE.Sport };
        }
        public class Session_Evaluation_Group
        {
            public List<CommonEnum.SESSION_TYPE> Values { get; } = new List<CommonEnum.SESSION_TYPE> { CommonEnum.SESSION_TYPE.Test, CommonEnum.SESSION_TYPE.Assignment };
        }
        public class Session_Breaks_Group
        {
            public List<CommonEnum.SESSION_TYPE> Values { get; } = new List<CommonEnum.SESSION_TYPE> { CommonEnum.SESSION_TYPE.Breaks };
        }
        public class Session_Tours_Group
        {
            public List<CommonEnum.SESSION_TYPE> Values { get; } = new List<CommonEnum.SESSION_TYPE> { CommonEnum.SESSION_TYPE.Tour };
        }

        public class Session_Self_Paced
        {
            public List<CommonEnum.SESSION_TYPE> Values { get; } = new List<CommonEnum.SESSION_TYPE> { CommonEnum.SESSION_TYPE.Self_paced };
        }

        public List<Session> Get_Trg_Progress_Data(string trainingid, string participantid)
        {
           
            List<Session> sessiondata = new List<Session>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("trainingplan.proc_session_completion_report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@participantid", participantid);
            }


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "ttttt_session_id is not null";
            dt = dt.DefaultView.ToTable();
            dt.Columns.Add("ttttt_session_no_int", typeof(int), "ttttt_session_no");
            dt.DefaultView.Sort = "ttttt_session_no_int asc";
            dt = dt.DefaultView.ToTable();

            ParticipantDB WDB = new ParticipantDB(_configuration);
            List<Participant> trgparticipants = new List<Participant>();
            trgparticipants = WDB.Get_TRG_PARTICIPANT_Data(trainingid);



            foreach (DataRow row in dt.Rows)
            {
                Session vw = new Session();
                vw.trainingid = Convert.ToString(row["trainingid"]);
                // vw.ttttt_session_row_no = Convert.ToString(row["ttttt_session_row_no"]);
                vw.ttttt_session_id = Convert.ToString(row["ttttt_session_id"]);
                // vw.ttttt_timetableid = Convert.ToString(row["ttttt_timetableid"]);
                // vw.ttttt_facultyid = Convert.ToString(row["ttttt_facultyid"]);
                vw.ttttt_content_desc = Convert.ToString(row["ttttt_content_desc"]);
                vw.ttttt_session_dt = Convert.ToDateTime(row["ttttt_session_dt"]).ToString("yyyy/MM/dd");
                vw.ttttt_session_time = Convert.ToString(row["ttttt_session_time"]);
                vw.ttttt_session_duration = Convert.ToString(row["ttttt_session_duration"]);
                vw.ttttt_session_day = Convert.ToInt32(row["ttttt_session_day"]);
                // vw.ttttt_is_joint_session = Convert.ToString(row["ttttt_is_joint_session"]);
                vw.ttttt_session_end_time = Convert.ToString(row["ttttt_session_end_time"]);
                vw.ttttt_session_no = Convert.ToInt32(row["ttttt_session_no"]);
                //vw.ttttt_status = Convert.ToString(row["ttttt_status"]);
                //vw.tttttf_status = Convert.ToString(row["tttttf_status"]);
                //vw.ttttt_remark = Convert.ToString(row["ttttt_remark"]);
                vw.ttttt_session_week = Convert.ToInt32(row["ttttt_session_week"]);
                if (Convert.ToString(row["ttttt_module_no"]) != "")
                {
                    vw.module = Convert.ToInt32(row["ttttt_module_no"]);
                }
                else
                {
                    vw.module = 0;
                }

                vw.ttttt_type = Convert.ToInt32(row["ttttt_type"]);
                //if (row["ttttt_session_duration_type"] != DBNull.Value)
                //{
                //    vw.ttttt_session_duration_type = Convert.ToInt32(row["ttttt_session_duration_type"]);
                //}

                //vw.ttttt_tag = Convert.ToString(row["ttttt_tag"]);
                //vw.ttttt_subject = Convert.ToString(row["ttttt_subject"]);
                //vw.participant_seession_required = Convert.ToString(row["participant_seession_required"]);

                vw.facultyname = Convert.ToString(row["facultyname"]);
                //vw.hfacultyname = Convert.ToString(row["hfacultyname"]);
                //if (Convert.ToString(row["facultyimgpath"]) != "")
                //{
                //    vw.facultyimgpath = up.Get_Agency_Photo_Path() + Convert.ToString(row["facultyimgpath"]);
                //}
                //else
                //{
                //    vw.facultyimgpath = up.Get_Agency_Default_Photo_Path();
                //}
                //vw.Attendance = Convert.ToString(row["Attendance"]);
                //if (row["ttttt_session_duration_type"] != DBNull.Value)
                //{
                //    vw.ttttt_session_duration_type_name = ((Common.CommonEnum.SessionDurationType)Convert.ToInt32(row["ttttt_session_duration_type"])).ToString();
                //}

                //vw.Session_type_icon = CommonEnum.Get_Session_Icon(vw.ttttt_type);
                //vw.Session_type_name = CommonEnum.Get_Session_Type_Name(vw.ttttt_type);
                vw.noofcompletion = Convert.ToInt32(row["noofpersons"]);
                vw.ttttt_complimentory = Convert.ToInt32(row["ttttt_complimentory"]);
                //dt.DefaultView.RowFilter = "ttttt_session_id='" + Convert.ToString(row["ttttt_session_id"]) + "'";
                //DataTable dtfilterdata = dt.DefaultView.ToTable();
                List<completionDetail> cp = new List<completionDetail>();
                // foreach (DataRow dr1 in dtfilterdata.Rows)
                //{
                //    cp.Add(new completionDetail { agencyid = dr1["tta_agency_id"].ToString(), agencyname = dr1["AgencyName"].ToString() });
                //}
                //vw.completiondetail = cp.ToArray();

                foreach (Participant p in trgparticipants)
                {
                    dt.DefaultView.RowFilter = "ttttt_session_id='" + Convert.ToString(row["ttttt_session_id"]) + "' and tta_agency_id='" + p.ParticipantId + "'";
                    DataTable dtfilterdata1 = dt.DefaultView.ToTable();
                    if (dtfilterdata1.Rows.Count > 0)
                    {
                        cp.Add(new completionDetail { agencyid = p.ParticipantId, agencyname = p.ParticipantName, status = "Completed", emailid = p.email, mobileno = p.mobileno });
                    }
                    else
                    {
                        cp.Add(new completionDetail { agencyid = p.ParticipantId, agencyname = p.ParticipantName, status = "Pending", emailid = p.email, mobileno = p.mobileno });
                    }


                }

                vw.completiondetail = cp.ToArray();





                sessiondata.Add(vw);
            }


            // sessiondata = sessiondata.Where(o => o.ttttt_timetableid != null).ToList();

            return sessiondata;
        }

        public SessionRestriction GET_SESSION_RESTRICTION_INFO(string trainingid)
        {

        
            SessionRestriction restrinction =new SessionRestriction();

            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            if (trgdetail.trg_Setting != null)
            {
              
                restrinction= trgdetail.trg_Setting.Session.SessionRestriction;

            }
            else
            {
                TrainingSettings TS = new TrainingSettings();
                string Foldername = CommonEnum.GET_JSON_FOLDER();
                string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "TrainingSettings.json"));
                TS = JsonConvert.DeserializeObject<TrainingSettings>(jsontxt);
                restrinction.isrestricted = Convert.ToInt16(TS.SessionAccessibility.Restricted);
                restrinction.restrictionon=TS.SessionAccessibility.RestrictOn;

            }


            //string Foldername = CommonEnum.GET_JSON_FOLDER();
            //string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "TrainingSettings.json"));
            //TS = JsonConvert.DeserializeObject<TrainingSettings>(jsontxt);
           return restrinction;
        }


        public List<Session> Get_Session_Data(DateTime fromdate, DateTime todate, DateTime? Sessiondt = null)
        {
          
            List<Session> sessiondata = new List<Session>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            string sessionstartdate = "";
            if (Sessiondt.HasValue)
            {
                sessionstartdate = Sessiondt.HasValue ? Sessiondt.Value.ToString("yyyy/MM/dd") : string.Empty;
            }


            if (Sessiondt.HasValue == true)
            {
                cmd = new SqlCommand("select * from  trainingplan.Vw_tp_trg_time_table where  (T_StartDate >= '" + fromdate.ToString("yyyy/MM/dd") + "' or T_ClosingDate>='" + fromdate.ToString("yyyy/MM/dd") + "') and T_StartDate <='" + todate.ToString("yyyy/MM/dd") + "' and ttttt_session_dt='" + sessionstartdate + "' and ttttt_timetableid is not null", con);
            }
            else
            {
                cmd = new SqlCommand("select * from  trainingplan.Vw_tp_trg_time_table where  (T_StartDate >= '" + fromdate.ToString("yyyy/MM/dd") + "' or T_ClosingDate>='" + fromdate.ToString("yyyy/MM/dd") + "') and T_StartDate <='" + todate.ToString("yyyy/MM/dd") + "' and ttttt_timetableid is not null", con);
            }

            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                Session vw = new Session();
                vw.trainingid = Convert.ToString(row["trainingid"]);
                vw.trainingcode= Convert.ToString(row["trainingcode"]);
                vw.training_title = Convert.ToString(row["T_Name"]);
                vw.ttttt_session_row_no = Convert.ToString(row["ttttt_session_row_no"]);
                vw.ttttt_session_id = Convert.ToString(row["ttttt_session_id"]);
                vw.ttttt_timetableid = Convert.ToString(row["ttttt_timetableid"]);
                vw.ttttt_facultyid = Convert.ToString(row["ttttt_facultyid"]);
                vw.ttttt_content_desc = Convert.ToString(row["ttttt_content_desc"]);
                vw.ttttt_session_dt = Convert.ToDateTime(row["ttttt_session_dt"]).ToString("yyyy/MM/dd");
                vw.ttttt_session_time = Convert.ToString(row["ttttt_session_time"]);
                vw.ttttt_session_duration = Convert.ToString(row["ttttt_session_duration"]);
                vw.ttttt_session_day = Convert.ToInt32(row["ttttt_session_day"]);
                vw.ttttt_is_joint_session = Convert.ToString(row["ttttt_is_joint_session"]);
                vw.ttttt_session_end_time = Convert.ToString(row["ttttt_session_end_time"]);
                vw.ttttt_session_no = Convert.ToInt32(row["ttttt_session_no"]);
                vw.ttttt_status = Convert.ToString(row["ttttt_status"]);
                vw.tttttf_status = Convert.ToString(row["tttttf_status"]);
                vw.ttttt_remark = Convert.ToString(row["ttttt_remark"]);
                vw.ttttt_session_week = Convert.ToInt32(row["ttttt_session_week"]);
                vw.ttttt_type = Convert.ToInt32(row["ttttt_type"]);
                if (row["ttttt_session_duration_type"] != DBNull.Value)
                {
                    vw.ttttt_session_duration_type = Convert.ToInt32(row["ttttt_session_duration_type"]);
                }

                vw.ttttt_tag = Convert.ToString(row["ttttt_tag"]);
                vw.ttttt_subject = Convert.ToString(row["ttttt_subject"]);
                vw.participant_seession_required = Convert.ToString(row["participant_seession_required"]);

                vw.facultyname = Convert.ToString(row["facultyname"]);
                vw.hfacultyname = Convert.ToString(row["hfacultyname"]);
                if (Convert.ToString(row["facultyimgpath"]) != "")
                {
                    vw.facultyimgpath = Convert.ToString(row["facultyimgpath"]);
                }
                else
                {
                    vw.facultyimgpath = null;
                }
                vw.Attendance = Convert.ToString(row["Attendance"]);
                if (row["ttttt_session_duration_type"] != DBNull.Value)
                {
                    vw.ttttt_session_duration_type_name = ((Common.CommonEnum.SessionDurationType)Convert.ToInt32(row["ttttt_session_duration_type"])).ToString();
                }

                vw.Session_type_icon = Session.Get_Session_Icon(vw.ttttt_type);
                vw.Session_type_name = Session.Get_Session_Type_Name(vw.ttttt_type);
                sessiondata.Add(vw);
            }



            sessiondata = sessiondata.Where(o => o.ttttt_timetableid != null).ToList();
            sessiondata = sessiondata.Where(o => o.tttttf_status != Convert.ToString(CommonEnum.Session_Status.Delete)).ToList();
            return sessiondata;
        }


        public List<SessionCompletionStatus> Get_Session_Status(string trainingid, string LoginUserType, string LoginUserID, string fromdt, string todt)
        {

            List<SessionCompletionStatus> attendancedata = new List<SessionCompletionStatus>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("Trainingplan.proc_tp_get_session_completion_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@TrainingId", trainingid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TrainingId", DBNull.Value);
            }

            if (LoginUserID != null)
            {
                cmd.Parameters.AddWithValue("@loginuserid", LoginUserID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@loginuserid", DBNull.Value);
            }
            if (LoginUserType != null)
            {
                cmd.Parameters.AddWithValue("@loginusertype", LoginUserType);
            }
            else
            {
                cmd.Parameters.AddWithValue("@loginusertype", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@frmdt", fromdt);
            cmd.Parameters.AddWithValue("@todt", todt);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                SessionCompletionStatus vw = new SessionCompletionStatus();
                vw.tttttm_training_id = Convert.ToString(row["tttttm_training_id"]);
                vw.ttttt_session_id = Convert.ToString(row["ttttt_session_id"]);
                if (Convert.ToString(row["iscompleted"]) != "")
                {
                    vw.iscompleted = Convert.ToInt32(row["iscompleted"]);
                }
                else
                {
                    vw.iscompleted = 0;
                }
                if (Convert.ToString(row["totalparticipant"]) != "")
                {
                    vw.totalparticipant = Convert.ToInt32(row["totalparticipant"]);
                }
                else
                {
                    vw.totalparticipant = 0;
                }
                if (Convert.ToString(row["percentcomplete"]) != "")
                {
                    vw.percentcomplete = Convert.ToDecimal(row["percentcomplete"]);
                }
                else
                {
                    vw.percentcomplete = 0;
                }


                attendancedata.Add(vw);
            }

            return attendancedata;
        }

        public bool Save_Session(CreateSessionDTO session, SqlTransaction transaction = null)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            cmd = new SqlCommand("Trainingplan.proc_tp_ins_upd_session", con);
            cmd.Parameters.AddWithValue("@Trainingid", session.trainingid);
            cmd.Parameters.AddWithValue("@Timetableid", session.time_table_Id);
            cmd.Parameters.AddWithValue("@createdby", session.createdby);
            cmd.Parameters.AddWithValue("@createdon", session.createdon.ToString("yyyy/MM/dd hh:mm:ss"));
            cmd.Parameters.AddWithValue("@branchid", session.branchid);
            cmd.Parameters.AddWithValue("@ttttt_session_id", session.sessionid);
            if (session.faculties != null)
            {
                cmd.Parameters.AddWithValue("@ttttt_facultyid", string.Join(",", session.faculties));
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttttt_facultyid", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@ttttt_content_desc", session.subject);
            cmd.Parameters.AddWithValue("@ttttt_subject", session.description);

            cmd.Parameters.AddWithValue("@ttttt_type", session.sessiontype);
            cmd.Parameters.AddWithValue("@ttttt_session_dt", session.date?.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@ttttt_session_day", session.day);
            cmd.Parameters.AddWithValue("@ttttt_session_week", session.week);
            cmd.Parameters.AddWithValue("@ttttt_session_duration", session.duration);
            cmd.Parameters.AddWithValue("@ttttt_session_row_no", session.rowno);
            cmd.Parameters.AddWithValue("@ttttt_is_joint_session", session.isjointSession);
            cmd.Parameters.AddWithValue("@ttttt_session_no", session.sessionno);

            cmd.Parameters.AddWithValue("@ttttt_session_time", session.sessionTime);
            cmd.Parameters.AddWithValue("@ttttt_session_end_time", session.End_date_time.ToString("yyyy/MM/dd hh:mm:ss"));
            cmd.Parameters.AddWithValue("@ttttt_status", session.status);
            if (session.remark == null)
            {
                cmd.Parameters.AddWithValue("@ttttt_remark", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttttt_remark", session.remark);
            }

            cmd.Parameters.AddWithValue("@TTTTT_SESSION_DURATION_TYPE", session.duration_type);
            if (session.tags != null)
            {
                cmd.Parameters.AddWithValue("@tag", string.Join(",", session.tags));
            }
            else
            {
                cmd.Parameters.AddWithValue("@tag", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@iscomplimentory", session.iscomplementory);

            //******** Column increased for module and completiontype
            if (session.module != null)
            {
                cmd.Parameters.AddWithValue("@ttttt_module_no", session.module);
            }
            //
            if (session.completiontype != null)
            {
                string p1 = JsonConvert.SerializeObject(session.completiontype);
                cmd.Parameters.AddWithValue("@ttttt_completion_type", p1);
            }



            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }


        public static bool SESSION_DISPLAY_ACTION(string usertype, int trainingtype, int sessiontype, int sessionstatus, int ActionFor, int iscomplementory, int ismeetingavailable, int iscdlogin, int participantstatus, string testparticipant = "", string session_completion_type = null, decimal completionpercentage = 0)
        {

            bool isdisplay = false;
            if (testparticipant == null)
            {
                testparticipant = "";
            }


            if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.LITTERA_ROOM)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    // Depend on If Participant is Approved in training or Session is complementory
                    if (iscomplementory == 1)
                    {
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                        {
                            isdisplay = false;
                        }

                    }
                    else
                    {
                        if (participantstatus == 1)
                        {
                            if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                            {
                                isdisplay = true;
                            }
                            if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                            {
                                isdisplay = false;
                            }
                            if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                            {
                                isdisplay = false;
                            }
                            if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                            {
                                isdisplay = true;
                            }
                            if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                            {
                                isdisplay = false;
                            }
                            if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                            {
                                isdisplay = true;
                            }

                        }
                        else
                        {
                            isdisplay = false;
                        }
                    }

                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = false;
                    }


                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = false;
                    }


                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = false;
                    }


                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {

                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = false;
                    }
                    if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = false;
                    }

                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.SHARE_FEEDBACK)
            {

                isdisplay = false;
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.CHECK_COMPETENCY)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    if (participantstatus == 1 || iscomplementory == 1)
                    {

                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                        {
                            isdisplay = false;
                        }

                        if (session_completion_type == "2")
                        {
                            isdisplay = true;
                        }
                        else
                        {
                            isdisplay = false;
                        }
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.GIVE_FEEDBACK)
            {
                isdisplay = false;
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.EDIT_SESSION)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if ((Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Test) && (Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Assignment))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }

                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if ((Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Test) && (Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Assignment))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if ((Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Test) && (Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Assignment))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.DELETE_SESSION)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if ((Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Test) && (Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Assignment))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if ((Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Test) && (Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Assignment))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if ((Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Test) && (Convert.ToInt16(sessiontype.ToString()) != (int)CommonEnum.SESSION_TYPE.Assignment))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.CONTENT_LIBRARY)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {

                    isdisplay = true;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    isdisplay = true;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    isdisplay = true;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    isdisplay = true;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = true;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.CREATE_MEETING)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if (ismeetingavailable == 0)
                    {

                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                        {
                            isdisplay = false;
                        }


                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if (ismeetingavailable == 0)
                    {
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                        {
                            isdisplay = false;
                        }


                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if (ismeetingavailable == 0)
                    {
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                        {
                            isdisplay = true;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                        {
                            isdisplay = false;
                        }
                        if (Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                        {
                            isdisplay = false;
                        }

                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.START_MEETING)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if ((iscdlogin == 1) && (ismeetingavailable == 1))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if ((iscdlogin == 1) && (ismeetingavailable == 1))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if ((iscdlogin == 1) && (ismeetingavailable == 1))
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.JOIN_MEETING)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    if (ismeetingavailable == 1 && iscdlogin == 0)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if (ismeetingavailable == 1 && iscdlogin == 0)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if (ismeetingavailable == 1 && iscdlogin == 0)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if (ismeetingavailable == 1 && iscdlogin == 0)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.SHARE_MEETING)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    if (ismeetingavailable == 1)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    if (ismeetingavailable == 1)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    if (ismeetingavailable == 1)
                    {
                        isdisplay = true;
                    }
                    else
                    {
                        isdisplay = false;
                    }
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.RUN_TEST)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    if (sessiontype == (int)CommonEnum.SessionType.Test)
                    {
                        if (testparticipant.ToString() == "")
                        {
                            isdisplay = true;
                        }


                    }

                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.View_TEST_RESULT)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    if (sessiontype == (int)CommonEnum.SessionType.Test)
                    {
                        if (testparticipant.ToString() != "")
                        {
                            isdisplay = true;
                        }


                    }

                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.Admin)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.CD)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.FACULTY)
                {
                    isdisplay = false;
                }
                else if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.DEPT)
                {
                    isdisplay = false;
                }
            }
            else if ((int)ActionFor == (int)CommonEnum.SESSION_LIST_ACTIONS.Complete_Session)
            {
                if (Convert.ToInt16(usertype.ToString()) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    if (session_completion_type == "1")
                    {
                        if (completionpercentage < 100)
                        {
                            isdisplay = true;
                        }


                    }

                }
                else
                {
                    isdisplay = false;
                }
            }

            return isdisplay;
        }
       

    }

}
