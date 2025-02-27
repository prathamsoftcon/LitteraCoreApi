using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.DBContext
{
    public class EvalDB
    {
        private readonly IConfiguration _configuration;
        public EvalDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Test> Get_test_List(string usertype, string userid)
        {

            List<Test> assingvaluation = new List<Test>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("eval.GetTestListWithUserType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userid", userid);
            cmd.Parameters.AddWithValue("@usertype", usertype);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            TrainingDB tdb = new TrainingDB(_configuration);
            List<TrainingCategory> categories = new List<TrainingCategory>();
            categories = tdb.Get_training_Category();

            foreach (DataRow row in dt.Rows)
            {
                Test T = new Test();
                T.testquestionid = Convert.ToString(row["testquestionid"]);
                T.testid = Convert.ToString(row["testid"]);
                T.testname = Convert.ToString(row["testname"]);
                T.assesmenttime = Convert.ToString(row["assesmenttime"]);
                T.skilltag = Convert.ToString(row["skilltag"]);
                T.isactive = Convert.ToInt32(row["isactive"]);
                T.trainingid = Convert.ToString(row["trainingid"]);
                T.trainingcode = Convert.ToString(row["trainingcode"]);
                T.trg_type = Convert.ToString(row["trg_type"]);
                T.createdon = Convert.ToDateTime(row["createdon"]);
                T.createdbyagencyid = Convert.ToString(row["createdbyagencyid"]);

                T.training_sponsortype = Convert.ToString(row["training_sponsortype"]);
                T.noofquestion = Convert.ToInt32(row["noofquestion"]);
                T.sessionid = Convert.ToString(row["Training.sessionid"]);
                T.ttttt_content_desc = Convert.ToString(row["ttttt_content_desc"]);
                T.ttttt_session_dt = Convert.ToString(row["ttttt_session_dt"]);
                T.ttttt_session_time = Convert.ToString(row["ttttt_session_time"]);

                T.ttttt_session_duration = Convert.ToString(row["ttttt_session_duration"]);
                T.ttpss_participant_id = Convert.ToString(row["ttpss_participant_id"]);
                T.ttpss_session_id = Convert.ToString(row["ttpss_session_id"]);
                T.ttpss_onscreen_time = Convert.ToString(row["ttpss_onscreen_time"]);

                if (row["tdds_status"].ToString() != "")
                {
                    T.tdds_status = Convert.ToInt16(row["tdds_status"]);
                }

                T.ttpss_status = Convert.ToString(row["ttpss_status"]);
                T.type = Convert.ToString(row["type"]);
                T.ttpss_created_on = Convert.ToString(row["ttpss_created_on"]);
                T.participantstatus = Convert.ToString(row["participantstatus"]);
                T.participantenrollstatus = Convert.ToString(row["participantenrollstatus"]);
                if (Common.CommonEnum.Get_Self_Paced_Trg(row["trg_type"].ToString()) == 1)
                {
                    if (Convert.ToString(row["ttpss_status"]) == "1")
                    {
                        T.issessioncompleted = 1;
                    }
                    else
                    {
                        T.issessioncompleted = 0;
                    }
                }
                else
                {
                    if (System.DateTime.Now > Convert.ToDateTime(Convert.ToString(row["ttttt_session_dt"])))
                    {
                        T.issessioncompleted = 1;
                    }
                    else
                    {
                        T.issessioncompleted = 0;
                    }
                }

                T.maxMarks = Convert.ToDecimal(row["NoOfQuestion"]) * Convert.ToDecimal(row["mark_per_question"]);
                T.mark_per_question = Convert.ToDecimal(row["mark_per_question"]);
                T.Training_category_name = categories.Where(o => o.TrainingCategoryId.ToString().ToUpper() == row["TrainingCategoryID"].ToString().ToUpper()).ToList().FirstOrDefault().TrainingCategoryName;
                T.Test_time= Convert.ToString(row["start_time"]);
                T.ttttt_status= Convert.ToString(row["ttttt_status"]);


                T.TrainingCategoryId = Convert.ToString(row["TrainingCategoryId"]);
                T.QuestionDifficultyID = Convert.ToString(row["QuestionDifficultyID"]);
                assingvaluation.Add(T);
            }





            return assingvaluation;
        }

        public List<TEST_RESULT_DATA> GET_TRAINING_TEST_ANALYTIC_DATA(string usertype, string userid, string fromdate, string todate, string trainingid = null, int testtype = 3,string branchid=null)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("eval.proc_ev_get_all_test_result", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@fromdt", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data
            TrainingDB WDB = new TrainingDB(_configuration);
            List<Training> lwtc = new List<Training>();
            lwtc = WDB.Get_VW_Training_calendar(Convert.ToDateTime(fromdate), Convert.ToDateTime(todate));
            List<Session> sl = new List<Session>();
            List<Participant> participants = new List<Participant>();
            if (trainingid != null)
            {
                SessionDB sdb = new SessionDB(_configuration);
                sl = sdb.Get_Session_Data_By_Trg(trainingid);

                ParticipantDB PDB = new ParticipantDB(_configuration);
                participants = PDB.Get_TRG_PARTICIPANT_Data(trainingid,null, branchid);
            }


            List<TEST_RESULT_DATA> T = new List<TEST_RESULT_DATA>();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["type"]) != testtype)
                {
                    continue;
                }
                TEST_RESULT_DATA r = new TEST_RESULT_DATA();
                r.trainingid = Convert.ToString(dr["trainingid"]);
                r.sessionid = Convert.ToString(dr["sessionid"]);
                r.testid = Convert.ToString(dr["TestID"]);
                r.testname = Convert.ToString(dr["TestName"]);
                r.participantid = Convert.ToString(dr["PartcipantID"]);
                r.Questionid = Convert.ToString(dr["QuestionID"]);
                r.mark_per_question = Convert.ToDecimal(dr["mark_per_question"]);
                r.iscorrect = Convert.ToInt32(dr["IsCorrect"]);
                //This column is not used because mark obtained calculated on front
                //  r.mark_obtained = Convert.ToDecimal(dr["tesQmarksobtained"]);

                if (lwtc.Where(o => o.TrainingId.ToString().ToUpper() == dr["trainingid"].ToString().ToUpper()).Count() > 0)
                {
                    r.training_code = lwtc.Where(o => o.TrainingId.ToString().ToUpper() == dr["trainingid"].ToString().ToUpper()).FirstOrDefault().Trainingcode;
                    r.training_name = lwtc.Where(o => o.TrainingId.ToString().ToUpper() == dr["trainingid"].ToString().ToUpper()).FirstOrDefault().T_Name;
                }
                if (sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == dr["sessionid"].ToString().ToUpper()).Count() > 0)
                {
                    r.session_desc = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == dr["sessionid"].ToString().ToUpper()).FirstOrDefault().ttttt_subject;
                    r.session_subject = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == dr["sessionid"].ToString().ToUpper()).FirstOrDefault().ttttt_content_desc;
                }
                if (participants.Where(o => o.ParticipantId.ToString().ToUpper() == r.participantid.ToString().ToUpper()).Count() > 0)
                {
                    r.participant_name = participants.Where(o => o.ParticipantId.ToString().ToUpper() == r.participantid.ToString().ToUpper()).FirstOrDefault().ParticipantName;
                }
                r.TestQuestionid = Convert.ToString(dr["TestQuestionID"]);
                r.TestParticipantid = Convert.ToString(dr["TestPartcipantID"]);
                T.Add(r);
            }


            return T;
        }

        public string Get_test_Participantid(string testquestionid, string userid)
        {

            List<Test> assingvaluation = new List<Test>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("eval.GetTestPrarticipantID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TestQuestionId", testquestionid);
            cmd.Parameters.AddWithValue("@participantID", userid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            string testparticipantid = "";
            if(dt.Rows.Count > 0)
            {
                testparticipantid = Convert.ToString(dt.Rows[0]["TestPartcipantID"]);
            }





            return testparticipantid;
        }

    }

}
