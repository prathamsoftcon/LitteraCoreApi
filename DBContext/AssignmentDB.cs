using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace LitteraCore.DBContext
{
    public class AssignmentDB
    {
        private readonly IConfiguration _configuration;
        public AssignmentDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Assignment> Get_Assignment_Data(string assignmentid = null)
        {

            List<Assignment> assingdata = new List<Assignment>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_get_assignment_list_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (assignmentid != null)
            {
                cmd.Parameters.AddWithValue("@assignmentid", assignmentid);
            }
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                Assignment ass = new Assignment();
                ass.AssignmentID = Convert.ToString(row["AssignmentID"]);
                ass.Instructions = Convert.ToString(row["Instructions"]);
                ass.Tag = Convert.ToString(row["Tag"]);
                ass.AssesmentQuestions = Convert.ToString(row["AssesmentQuestions"]);
                ass.FacultyID_Json = Convert.ToString(row["FacultyID_Json"]);
                ass.AttachmentsID_Json = Convert.ToString(row["AttachmentsID_Json"]);
                //ass.AssignmentType = Convert.ToString(row["AssignmentType"]);
                ass.GradeApplicable = Convert.ToInt32(row["GradeApplicable"]);
                ass.AssignmentName = Convert.ToString(row["AssignmentName"]);
                ass.isOpenended = Convert.ToInt32(row["DeadlineType"]);
                //ass.createdon = Convert.ToDateTime(row["createdon"]);
                ass.createdby = Convert.ToString(row["createdby"]);
                ass.status = Convert.ToInt32(row["tdds_status"]);
                //ass.session = Convert.ToString(row["session"]);
                ass.ttttt_session_id = Convert.ToString(row["SessionID"]);
                if (row["MaxMarks"].ToString() != "")
                {
                    ass.MaxMarks = Convert.ToInt32(row["MaxMarks"]);
                }
                else
                {
                    ass.MaxMarks = 0;
                }

                ass.uploadpath = Convert.ToString("");
                // ass.training = Convert.ToString(row["training"]);

                ass.facultyname = Convert.ToString(row["faculty"]);
                ass.AssignmentTypeName = Convert.ToString(row["AssignmentType"]);
                ass.TrainingCode = Convert.ToString(row["TrainingCode"]);
                ass.Trainingid = Convert.ToString(row["ttttt_trainingid"]);

                DateTime sessionDate = Convert.ToDateTime(row["ttttt_session_dt"]);
                string sessionTime = Convert.ToString(row["ttttt_session_time"]);

                DateTime combinedDateTime = DateTime.Parse(sessionDate.ToString("yyyy-MM-dd") + " " + sessionTime);

                ass.ttttt_session_dt = combinedDateTime;
                ass.ttttt_session_time = Convert.ToString(row["ttttt_session_time"]);
                ass.ttttt_session_duration = Convert.ToInt32(row["ttttt_session_duration"]);
                ass.tdds_doc_no = Convert.ToString(row["tdds_doc_no"]);
                ass.ttttt_session_end_time = Convert.ToDateTime(row["ttttt_session_end_time"]);
                if (Convert.ToString(row["question_max_marks"]) != "")
                {
                    ass.AssignmentQuestionsMarks = JsonConvert.DeserializeObject<List<AssignmentQuestions>>(Convert.ToString(row["question_max_marks"]));
                }

                // ass.taau_status= Convert.ToString(row["taau_status"]);

                if (Convert.ToString(row["trg_setting"]) != "")
                {
                    try
                    {
                        Trg_Setting p = new Trg_Setting();
                        p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(row["trg_setting"]));
                        ass.trg_Setting = p;
                        if (p.displaycontrols != null)
                        {
                            if (p.displaycontrols.Where(o => o.id == 9).ToList().Count() > 0)
                            {
                                if (p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext != "")
                                {
                                    ass.TrainingCode = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                    
                                }
                            }
                        }



                    }
                    catch
                    {
                        ass.trg_Setting = null;

                    }

                }
                else
                {
                    ass.trg_Setting = null;
                }
                assingdata.Add(ass);
            }





            return assingdata;
        }

        public List<AssignmentValuation> Get_Valuation(string assignmentid, string participantid = null)
        {

            List<AssignmentValuation> assingvaluation = new List<AssignmentValuation>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_assignment_valuation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssignmentID", assignmentid);
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@participantid", participantid);
            }
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                AssignmentValuation ass = new AssignmentValuation();
                ass.taav_id = Convert.ToString(row["taav_id"]);
                ass.taav_assignmentid = Convert.ToString(row["taav_assignmentid"]);
                ass.taav_valuation_json = JsonConvert.DeserializeObject<List<valuation_json>>(Convert.ToString(row["taav_valuation_json"]));
                ass.taav_createdby = Convert.ToString(row["taav_createdby"]);
                ass.taav_createdon = Convert.ToDateTime(row["taav_createdon"]);

                assingvaluation.Add(ass);
            }





            return assingvaluation;
        }

        public List<proc_ass_get_assignment_comment> Get_assignment_Comments(string assignmentid, string participantid)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_assignment_comment_for_participant", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if(assignmentid != null)
            {
                cmd.Parameters.AddWithValue("@taac_AssignmentID", assignmentid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@taac_AssignmentID", DBNull.Value);
            }
         
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@Participantid", participantid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Participantid", DBNull.Value);
            }


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<proc_ass_get_assignment_comment> LI = new List<proc_ass_get_assignment_comment>();
            foreach (DataRow row in dt.Rows)
            {
                var ss = JsonConvert.DeserializeObject<assignmentparticipant[]>(row["participant"].ToString());
                // List<assignmentparticipant> arr =(List<assignmentparticipant>)(row["participant"].ToString());

                proc_ass_get_assignment_comment cm = new proc_ass_get_assignment_comment();
                cm.assignment = (string)row["assignmentid"].ToString();
                cm.participant = ss;
                LI.Add(cm);
            }



            return LI;
        }

        public List<proc_ass_get_assignment_upload> Get_assignment_Uploads(string assignmentid, string participantid)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_assignment_upload", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taau_AssignmentID", assignmentid);
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@Participantid", participantid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Participantid", DBNull.Value);
            }


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<proc_ass_get_assignment_upload> LI = new List<proc_ass_get_assignment_upload>();
            foreach (DataRow row in dt.Rows)
            {
                var ss = JsonConvert.DeserializeObject<assignmentparticipant[]>(row["participant"].ToString());
                // List<assignmentparticipant> arr =(List<assignmentparticipant>)(row["participant"].ToString());

                proc_ass_get_assignment_upload cm = new proc_ass_get_assignment_upload();
                cm.assignment = (string)row["assignmentid"].ToString();
                cm.status = Convert.ToInt16(row["taau_status"]);
                cm.participant = ss;
                cm.no_of_uploads = ss.Count();
                LI.Add(cm);
            }



            return LI;
        }


        public bool Update_Assignment_Upload_Comment(string uploadid, AssignmentUploadComments a)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_update_assignment_upload_comment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taau_uploadid", uploadid);
            string p1 = JsonConvert.SerializeObject(a);
            cmd.Parameters.AddWithValue("@comment_json", p1);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }


        public bool Insert_Comment(AssignmentComment a)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_insert_assignment_comment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taac_commentid", a.taac_commentid);
            cmd.Parameters.AddWithValue("@taac_AssignmentID", a.taac_AssignmentID);
            cmd.Parameters.AddWithValue("@taac_Participantid", a.taac_Participantid);
            cmd.Parameters.AddWithValue("@taac_commentedby", a.taac_commentedby);
            cmd.Parameters.AddWithValue("@taac_comment", a.taac_comment);
            cmd.Parameters.AddWithValue("@taac_createdon", a.taac_createdon);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        public bool Upload_Document(AssignmentUpload a)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_inupd_assignment_upload", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taau_uploadid", a.taau_uploadid);
            cmd.Parameters.AddWithValue("@taau_uploadpath", a.taau_uploadpath);
            cmd.Parameters.AddWithValue("@taau_AssignmentID", a.taau_AssignmentID);
            cmd.Parameters.AddWithValue("@taau_Participantid", a.taau_Participantid);
            cmd.Parameters.AddWithValue("@taau_uploadedby", a.taau_uploadedby);
            cmd.Parameters.AddWithValue("@taau_title", a.taau_title);
            //cmd.Parameters.AddWithValue("@taau_remark", a.taau_remark);
            cmd.Parameters.AddWithValue("@taau_type", a.taau_type);
            cmd.Parameters.AddWithValue("@taau_createdon", a.taau_createdon);
            string p1 = JsonConvert.SerializeObject(a.taau_comment_json);
            cmd.Parameters.AddWithValue("@comment_json", p1);
            cmd.Parameters.AddWithValue("@taau_status", a.taau_status);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        public List<AssignmentUpload> Get_Participant_Assignment_Uploads(string participantid, string assignmentid = null)
        {

            List<AssignmentUpload> assingdata = new List<AssignmentUpload>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_participant_assignment_uploads", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Participantid", participantid);
            if (assignmentid != null)
            {
                cmd.Parameters.AddWithValue("@taau_AssignmentID", assignmentid);
            }
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                AssignmentUpload ass = new AssignmentUpload();
                ass.taau_uploadid = Convert.ToString(row["taau_uploadid"]);
                ass.taau_uploadpath = Convert.ToString(row["taau_uploadpath"]);
                ass.taau_AssignmentID = Convert.ToString(row["taau_AssignmentID"]);
                ass.taau_AssignmentID = Convert.ToString(row["taau_AssignmentID"]);
                //ass.taau_uploadedby = Convert.ToString(row["taau_uploadedby"]);
                ass.taau_title = Convert.ToString(row["taau_title"]);
                //ass.AssignmentType = Convert.ToString(row["AssignmentType"]);
                //ass.taau_remark = Convert.ToString(row["taau_remark"]);
                ass.taau_type = Convert.ToString(row["taau_type"]);
                ass.taau_status = Convert.ToInt32(row["taau_status"]);

                assingdata.Add(ass);
            }





            return assingdata;
        }

        public Assignment_Question_Valuation Get_assignment_Question_Validation(string assignmentid, string participantid)
        {
            AgencyDB adb = new AgencyDB(_configuration);
            List<Agency> a = new List<Agency>();

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_question_valuation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taaqv_assessmentid", assignmentid);
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@taaqv_participantid", participantid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@taaqv_participantid", DBNull.Value);
            }



            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            Assignment_Question_Valuation av = new Assignment_Question_Valuation();
            List<AssignmentQuestions> LI = new List<AssignmentQuestions>();
            a = adb.Get_Agency(null, null, 1, 10, null, null, null, null,null, "AgencyId,tyaam_status,AgencyName,HAgencyName,ag_email,ag_mobileno,totalrecords");
            foreach (DataRow row in dt.Rows)
            {
                if (row["taaqv_valuation_json"].ToString() != "")
                {
                    av.taaqv_id = Convert.ToString(row["taaqv_id"]);
                    av.taaqv_assessmentid = Convert.ToString(row["taaqv_assessmentid"]);
                    av.taaqv_participantid = Convert.ToString(row["taaqv_participantid"]);
                    av.createdon = Convert.ToString(row["createdon"]);
                    av.createdby = Convert.ToString(row["createdby"]);
                    av.taaqv_status = Convert.ToInt16(row["taaqv_status"]);
                    
                    if (a.Where(o=>o.agencyid== Convert.ToString(row["createdby"])).Count() > 0)
                    {
                        av.createdby_name = a.Where(o => o.agencyid == Convert.ToString(row["createdby"])).FirstOrDefault().agencyname;
                    }


                    LI = JsonConvert.DeserializeObject<List<AssignmentQuestions>>(row["taaqv_valuation_json"].ToString());
                    av.taaqv_valuation_json = LI.ToArray();
                }


            }



            return av;
        }

        public bool Save_Assignmant_Valuation(Assignment_Question_Valuation a)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_ins_upd_question_valuation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taaqv_id", a.taaqv_id);
            cmd.Parameters.AddWithValue("@taaqv_assessmentid", a.taaqv_assessmentid);
            cmd.Parameters.AddWithValue("@taaqv_participantid", a.taaqv_participantid);
            string p1 = JsonConvert.SerializeObject(a.taaqv_valuation_json);
            cmd.Parameters.AddWithValue("@taaqv_valuation_json", p1);
            cmd.Parameters.AddWithValue("@taaqv_status", a.taaqv_status);
            cmd.Parameters.AddWithValue("@createdby", a.createdby);
            cmd.Parameters.AddWithValue("@createdon", a.createdon);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }


        public Assignment_Valuation_Summary Valuation_Summary(string assignmentid)
        {
            Assignment_Valuation_Summary vs = new Assignment_Valuation_Summary();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_get_assignment_summery", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (assignmentid != null)
            {
                cmd.Parameters.AddWithValue("@assignmentid", assignmentid);
            }
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                vs.total_participant = Convert.ToInt32(row["total_participant"]);
                vs.assignment_submitted = Convert.ToInt32(row["assignment_submitted"]);
                vs.valuation_completed = Convert.ToInt32(row["valuation_completed"]);

            }


            return vs;
        }

        public List<Assignment_Question_Valuation> Get_assignment_All_Valuation(string assignmentid)
        {
            AgencyDB adb = new AgencyDB(_configuration);
            List<Agency> a = new List<Agency>();

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_question_valuation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taaqv_assessmentid", assignmentid);
            cmd.Parameters.AddWithValue("@taaqv_participantid", DBNull.Value);



            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            List<Assignment_Question_Valuation> lav = new List<Assignment_Question_Valuation>();


            List<AssignmentQuestions> LI = new List<AssignmentQuestions>();
            a = adb.Get_Agency(null, null, 1, 10, null, null, null, null,null, "AgencyId,tyaam_status,AgencyName,HAgencyName,ag_email,ag_mobileno,totalrecords");
            foreach (DataRow row in dt.Rows)
            {
                if (row["taaqv_valuation_json"].ToString() != "")
                {
                    Assignment_Question_Valuation av = new Assignment_Question_Valuation();
                    av.taaqv_id = Convert.ToString(row["taaqv_id"]);
                    av.taaqv_assessmentid = Convert.ToString(row["taaqv_assessmentid"]);
                    av.taaqv_participantid = Convert.ToString(row["taaqv_participantid"]);
                    av.createdon = Convert.ToString(row["createdon"]);
                    av.createdby = Convert.ToString(row["createdby"]);
                    
                    if (a.Where(o => o.agencyid == Convert.ToString(row["createdby"])).Count() > 0)
                    {
                        av.createdby_name = a.Where(o => o.agencyid == Convert.ToString(row["createdby"])).FirstOrDefault().agencyname;
                    }


                    LI = JsonConvert.DeserializeObject<List<AssignmentQuestions>>(row["taaqv_valuation_json"].ToString());
                    av.taaqv_valuation_json = LI.ToArray();
                    lav.Add(av);
                }


            }



            return lav;
        }

        public assignment_session_mapping_data Get_Assignment_Session_Mapping_Data(string assignmentid)
        {

            assignment_session_mapping_data assignment = new assignment_session_mapping_data();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Assessment.proc_get_assignment_list_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@assignmentid", assignmentid);
          
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
             
                assignment.assignmentid = Convert.ToString(row["AssignmentID"]);
                assignment.sessionid = Convert.ToString(row["ttttt_session_id"]);
                assignment.trainingid = Convert.ToString(row["ttttt_trainingid"]);
             

            }





            return assignment;
        }

        public bool update_Assignment_status(DMS d)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con1 = new SqlConnection(connectionString);
            DMSDB ddb = new DMSDB(_configuration);
            ddb.INS_UPD_DMS(d, con1, null);

            assignment_session_mapping_data T = new assignment_session_mapping_data();
            T = Get_Assignment_Session_Mapping_Data(d.doc_id);


            string sessionstatus = "0";
            if (d.doc_status == 1)
            {
                sessionstatus = "0";
            }
            else if (d.doc_status == -1)
            {
                sessionstatus = "9";
            }
            else
            {
                sessionstatus = d.doc_status.ToString();
            }
            SessionDB sdb = new SessionDB(_configuration);
            bool isupdated = sdb.Update_session_dms_status(T.trainingid, T.sessionid, d, sessionstatus);
            return isupdated;
        }


        public assignment_session_mapping_data Get_Session_Assignment_Details(string sessionid)
        {

            assignment_session_mapping_data assignment = new assignment_session_mapping_data();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("SELECT * FROM Assessment.Schedule WHERE SessionID = @SessionID;", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SessionID", sessionid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {

                assignment.assignmentid = Convert.ToString(row["AssignmentId"]);
                assignment.sessionid = Convert.ToString(row["SessionID"]);
                assignment.trainingid = Convert.ToString(row["trainingid"]);


            }





            return assignment;
        }





    }
}
