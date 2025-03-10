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
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("eval.GetTestListWithUserType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@usertype", usertype);

                cmd.CommandTimeout = 5000;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    TrainingDB tdb = new TrainingDB(_configuration);
                    List<TrainingCategory> categories = tdb.Get_training_Category();

                    while (reader.Read())
                    {
                        Test T = new Test();
                        T.testquestionid = reader["testquestionid"].ToString();
                        T.testid = reader["testid"].ToString();
                        T.testname = reader["testname"].ToString();
                        T.assesmenttime = reader["assesmenttime"].ToString();
                        T.skilltag = reader["skilltag"].ToString();
                        T.isactive = Convert.ToInt32(reader["isactive"]);
                        T.trainingid = reader["trainingid"].ToString();
                        T.trainingcode = reader["trainingcode"].ToString();
                        T.trg_type = reader["trg_type"].ToString();
                        T.createdon = Convert.ToDateTime(reader["createdon"]);
                        T.createdbyagencyid = reader["createdbyagencyid"].ToString();

                        T.training_sponsortype = reader["training_sponsortype"].ToString();
                        T.noofquestion = Convert.ToInt32(reader["noofquestion"]);
                        T.sessionid = reader["Training.sessionid"].ToString();
                        T.ttttt_content_desc = reader["ttttt_content_desc"].ToString();
                        T.ttttt_session_dt = reader["ttttt_session_dt"].ToString();
                        T.ttttt_session_time = reader["ttttt_session_time"].ToString();

                        T.ttttt_session_duration = reader["ttttt_session_duration"].ToString();
                        T.ttpss_participant_id = reader["ttpss_participant_id"].ToString();
                        T.ttpss_session_id = reader["ttpss_session_id"].ToString();
                        T.ttpss_onscreen_time = reader["ttpss_onscreen_time"].ToString();

                        // Safely handle nullable integer columns
                        if (!string.IsNullOrEmpty(reader["tdds_status"].ToString()))
                        {
                            T.tdds_status = Convert.ToInt16(reader["tdds_status"]);
                        }

                        T.ttpss_status = reader["ttpss_status"].ToString();
                        T.type = reader["type"].ToString();
                        T.ttpss_created_on = reader["ttpss_created_on"].ToString();
                        T.participantstatus = reader["participantstatus"].ToString();
                        T.participantenrollstatus = reader["participantenrollstatus"].ToString();

                        if (Common.CommonEnum.Get_Self_Paced_Trg(reader["trg_type"].ToString()) == 1)
                        {
                            T.issessioncompleted = reader["ttpss_status"].ToString() == "1" ? 1 : 0;
                        }
                        else
                        {
                            T.issessioncompleted = DateTime.Now > Convert.ToDateTime(reader["ttttt_session_dt"].ToString()) ? 1 : 0;
                        }

                        T.maxMarks = Convert.ToDecimal(reader["NoOfQuestion"]) * Convert.ToDecimal(reader["mark_per_question"]);
                        T.mark_per_question = Convert.ToDecimal(reader["mark_per_question"]);

                        // Handle potential null category matching
                        var category = categories.FirstOrDefault(o => o.TrainingCategoryId.ToString().ToUpper() == reader["TrainingCategoryID"].ToString().ToUpper());
                        T.Training_category_name = category != null ? category.TrainingCategoryName : string.Empty;

                        T.Test_time = reader["start_time"].ToString();
                        T.ttttt_status = reader["ttttt_status"].ToString();

                        T.TrainingCategoryId = reader["TrainingCategoryId"].ToString();
                        T.QuestionDifficultyID = reader["QuestionDifficultyID"].ToString();

                        assingvaluation.Add(T);
                    }
                }
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
