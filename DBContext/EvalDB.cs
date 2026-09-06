using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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
        public List<Test> Get_test_List(
            string usertype,
            string userid,
            string testtype = null,
            string trainingid = null)
        {

            List<Test> assingvaluation = new List<Test>();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("eval.GetTestListWithUserType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@usertype", usertype);
                if (!string.IsNullOrWhiteSpace(testtype))
                {
                    cmd.Parameters.AddWithValue("@testType", testtype);
                }

                cmd.CommandTimeout = 5000;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                  

                    TrainingDB tdb = new TrainingDB(_configuration);
                    List<TrainingCategory> categories = tdb.Get_training_Category();

                    while (reader.Read())
                    {
                        if (trainingid != null)
                        {
                            if (reader["trainingid"].ToString().ToUpper() != trainingid.ToString().ToUpper())
                            {
                                continue;
                            }
                        }

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

                        if (Convert.ToString(reader["trg_setting"]) != "")
                        {
                            try
                            {
                                Trg_Setting p = new Trg_Setting();
                                p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(reader["trg_setting"]));
                                T.trg_Setting = p;
                                if (p.displaycontrols != null)
                                {
                                    if (p.displaycontrols.Where(o => o.id == 9).ToList().Count() > 0)
                                    {
                                        if (p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext != "")
                                        {
                                            T.trainingcode = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                           
                                        }
                                    }
                                }



                            }
                            catch
                            {
                                T.trg_Setting = null;

                            }

                        }
                        else
                        {
                            T.trg_Setting = null;
                        }

                        assingvaluation.Add(T);
                    }
                }
            }

            return assingvaluation;

        }

        public List<TEST_RESULT_DATA> GET_TRAINING_TEST_ANALYTIC_DATA(string usertype, string userid, string fromdate, string todate, string trainingid = null, int testtype = 1,string branchid=null)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("eval.proc_ev_get_all_test_result", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@fromdt", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            cmd.Parameters.AddWithValue("@testtype", testtype);

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
                participants = PDB.Get_TRG_PARTICIPANT_Data(trainingid,null, branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            }


            List<TEST_RESULT_DATA> T = new List<TEST_RESULT_DATA>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (Convert.ToInt32(dr["type"]) != testtype)
            //    {
            //        continue;
            //    }
            //    TEST_RESULT_DATA r = new TEST_RESULT_DATA();
            //    r.trainingid = Convert.ToString(dr["trainingid"]);
            //    r.sessionid = Convert.ToString(dr["sessionid"]);
            //    r.testid = Convert.ToString(dr["TestID"]);
            //    r.testname = Convert.ToString(dr["TestName"]);
            //    r.participantid = Convert.ToString(dr["PartcipantID"]);
            //    r.Questionid = Convert.ToString(dr["QuestionID"]);
            //    r.mark_per_question = Convert.ToDecimal(dr["mark_per_question"]);
            //    r.iscorrect = Convert.ToInt32(dr["IsCorrect"]);
            //    //This column is not used because mark obtained calculated on front
            //    //  r.mark_obtained = Convert.ToDecimal(dr["tesQmarksobtained"]);

            //    if (lwtc.Where(o => o.TrainingId.ToString().ToUpper() == dr["trainingid"].ToString().ToUpper()).Count() > 0)
            //    {
            //        r.training_code = lwtc.Where(o => o.TrainingId.ToString().ToUpper() == dr["trainingid"].ToString().ToUpper()).FirstOrDefault().Trainingcode;
            //        r.training_name = lwtc.Where(o => o.TrainingId.ToString().ToUpper() == dr["trainingid"].ToString().ToUpper()).FirstOrDefault().T_Name;
            //    }
            //    if (sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == dr["sessionid"].ToString().ToUpper()).Count() > 0)
            //    {
            //        r.session_desc = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == dr["sessionid"].ToString().ToUpper()).FirstOrDefault().ttttt_subject;
            //        r.session_subject = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == dr["sessionid"].ToString().ToUpper()).FirstOrDefault().ttttt_content_desc;
            //    }
            //    if (participants.Where(o => o.ParticipantId.ToString().ToUpper() == r.participantid.ToString().ToUpper()).Count() > 0)
            //    {
            //        r.participant_name = participants.Where(o => o.ParticipantId.ToString().ToUpper() == r.participantid.ToString().ToUpper()).FirstOrDefault().ParticipantName;
            //    }
            //    r.TestQuestionid = Convert.ToString(dr["TestQuestionID"]);
            //    r.TestParticipantid = Convert.ToString(dr["TestPartcipantID"]);
            //    T.Add(r);
            //}

            var trainingLookup = lwtc.ToDictionary(x => x.TrainingId.ToString().ToUpper(), x => x);
            var sessionLookup = sl.ToDictionary(x => x.ttttt_session_id.ToString().ToUpper(), x => x);
            //var participantLookup = participants.ToDictionary(x => x.ParticipantId.ToString().ToUpper(), x => x);
            var participantLookup = participants
    .GroupBy(x => x.ParticipantId.ToString().ToUpper())
    .ToDictionary(g => g.Key, g => g.First());

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["type"]) != testtype)
                    continue;

                TEST_RESULT_DATA r = new TEST_RESULT_DATA
                {
                    trainingid = Convert.ToString(dr["trainingid"]),
                    sessionid = Convert.ToString(dr["sessionid"]),
                    testid = Convert.ToString(dr["TestID"]),
                    testname = Convert.ToString(dr["TestName"]),
                    participantid = Convert.ToString(dr["PartcipantID"]),
                    Questionid = Convert.ToString(dr["QuestionID"]),
                    mark_per_question = Convert.ToDecimal(dr["mark_per_question"]),
                    iscorrect = Convert.ToInt32(dr["IsCorrect"]),
                    TestQuestionid = Convert.ToString(dr["TestQuestionID"]),
                    TestParticipantid = Convert.ToString(dr["TestPartcipantID"])
                };

                string trainingIdKey = r.trainingid.ToUpper();
                if (trainingLookup.TryGetValue(trainingIdKey, out Training training))
                {
                    r.training_code = training.Trainingcode;
                    r.training_name = training.T_Name;
                }

                string sessionIdKey = r.sessionid.ToUpper();
                if (sessionLookup.TryGetValue(sessionIdKey, out Session session))
                {
                    r.session_desc = session.ttttt_subject;
                    r.session_subject = session.ttttt_content_desc;
                }

                string participantIdKey = r.participantid.ToUpper();
                if (participantLookup.TryGetValue(participantIdKey, out Participant participant))
                {
                    r.participant_name = participant.ParticipantName;
                }

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
            if (con.State != ConnectionState.Open) { con.Open(); }
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
                //testparticipantid = Convert.ToString(dt.Rows[0]["TestPartcipantID"]);
                testparticipantid = Convert.ToString(dt.Rows[0]["TestPartcipantID"]);
            }





            return testparticipantid;
        }

        public List<user_session_test> Get_Test_Detail(string testis)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("select tm.TrainingCategoryId,tq.SkillTag from Eval.TestQuestions tq inner join TrainingPlan.TrainingBasicDetails tm on tm.TrainingId = tq.[Training.TrainingID] where TestID = @TestId", con);
            cmd.Parameters.Add("@TestId", SqlDbType.NVarChar, 100).Value = testis ?? string.Empty;
            cmd.CommandType = CommandType.Text;
        
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data
           

            List<user_session_test> T = new List<user_session_test>();
            foreach (DataRow dr in dt.Rows)
            {
              
                user_session_test r = new user_session_test();
                r.trainingcategoryid = Convert.ToString(dr["trainingcategoryid"]);
                r.skilltags = Convert.ToString(dr["SkillTag"]);
            
                T.Add(r);
            }


            return T;
        }



        public List<participant_test_result> Get_Participant_Test_Result(string testquestionid, string participantid = null, int pageno = 1, int pagesize = 0,string searchcolumn = null, string searchvalue = null)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("eval.GetPrarticipantResultbyTestQuestionID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TestQuestionId", testquestionid);
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@participantID", participantid);
            }
            cmd.Parameters.AddWithValue("@PageNo", pageno);
            cmd.Parameters.AddWithValue("@PageSize", pagesize);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data


            List<participant_test_result> T = new List<participant_test_result>();
            foreach (DataRow dr in dt.Rows)
            {

                participant_test_result r = new participant_test_result();
                r.participantid = Convert.ToString(dr["PartcipantID"]);
                r.testparticipantid = Convert.ToString(dr["TestPartcipantID"]);
                r.AgencyName = Convert.ToString(dr["AgencyName"]);
                r.ag_mobileno = Convert.ToString(dr["ag_mobileno"]);
                r.ag_email = Convert.ToString(dr["ag_email"]);
                if(Convert.ToString(dr["TotalQuestion"]) != ""){
                    r.total_questions = Convert.ToInt16(dr["TotalQuestion"]);
                }
                if (Convert.ToString(dr["correctTotal"]) != "")
                {
                    r.total_correct = Convert.ToInt16(dr["correctTotal"]);
                }
                if (Convert.ToString(dr["IncorrectTotal"]) != "")
                {
                    r.total_incorrect = Convert.ToInt16(dr["IncorrectTotal"]);
                }
                if (Convert.ToString(dr["NoAnsweredTotal"]) != "")
                {
                    r.total_not_answered = Convert.ToInt16(dr["NoAnsweredTotal"]);
                }
                r.totalrecored = Convert.ToInt16(Convert.ToInt16(dr["TotalRow_count"]));

                T.Add(r);
            }


            return T;
        }


        public TEST_SESSION_MAPPING_DATA Get_Test_Session_Mapping_Data(string testid)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("eval.get_test_session_mapping_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@testid", testid);
          

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data


            TEST_SESSION_MAPPING_DATA  T= new TEST_SESSION_MAPPING_DATA();
            foreach (DataRow dr in dt.Rows)
            {

                T.testid = Convert.ToString(dr["testid"]);
                T.TestQuestionID = Convert.ToString(dr["TestQuestionID"]);
                T.trainingid = Convert.ToString(dr["trainingid"]);
                T.sessionid = Convert.ToString(dr["sessionid"]);
              
             
            }


            return T;
        }

        public TEST_SESSION_MAPPING_DATA Get_Test_Session_Mapping_Data_By_Session(string sessionid)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("eval.get_test_session_mapping_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@sessionid", sessionid);


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data


            TEST_SESSION_MAPPING_DATA T = new TEST_SESSION_MAPPING_DATA();
            foreach (DataRow dr in dt.Rows)
            {

                T.testid = Convert.ToString(dr["testid"]);
                T.TestQuestionID = Convert.ToString(dr["TestQuestionID"]);
                T.trainingid = Convert.ToString(dr["trainingid"]);
                T.sessionid = Convert.ToString(dr["sessionid"]);


            }


            return T;
        }

        public bool Check_test_in_use(string testid)
        {

            bool is_used = false;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("eval.proc_eval_get_participant_test_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@testid", testid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data

            if (dt.Rows.Count > 0)
            {
                is_used = true;
            }
           


            return is_used;
        }


        public bool Check_test_participant_status(string testid,string participantid)
        {

            bool is_used = false;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("eval.proc_eval_get_participant_test_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@testid", testid);
            cmd.Parameters.AddWithValue("@participantid", participantid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //************Get Data

            if (dt.Rows.Count > 0)
            {
                is_used = true;
            }



            return is_used;
        }

        public bool update_test_status(DMS d)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con1 = new SqlConnection(connectionString);
            DMSDB ddb = new DMSDB(_configuration);
            ddb.INS_UPD_DMS(d, con1, null);

            TEST_SESSION_MAPPING_DATA T = new TEST_SESSION_MAPPING_DATA();
            T = Get_Test_Session_Mapping_Data(d.doc_id);


            string sessionstatus = "0";
            if (d.doc_status ==1)
            {
                sessionstatus= "0";
            }
            else if(d.doc_status==-1)
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


        public List<Test> Get_trg_test_List_on_session(string userid,string usertype, string trainingid)
        {

            List<Test> assingvaluation = new List<Test>();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("eval.proc_eval_test_list_on_session", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@trainingID", trainingid);
                cmd.Parameters.AddWithValue("@usertype", usertype);

                cmd.CommandTimeout = 5000;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {


                    TrainingDB tdb = new TrainingDB(_configuration);
                    List<TrainingCategory> categories = tdb.Get_training_Category();

                    while (reader.Read())
                    {
                       

                        Test T = new Test();
                       // T.testquestionid = reader["testquestionid"].ToString();
                        T.testid = reader["testid"].ToString();
                       // T.testname = reader["testname"].ToString();
                        //T.assesmenttime = reader["assesmenttime"].ToString();
                        //T.skilltag = reader["skilltag"].ToString();
                        //T.isactive = Convert.ToInt32(reader["isactive"]);
                        T.trainingid = reader["training.trainingid"].ToString();
                        //T.trainingcode = reader["trainingcode"].ToString();
                        //T.trg_type = reader["trg_type"].ToString();
                        //T.createdon = Convert.ToDateTime(reader["createdon"]);
                        //T.createdbyagencyid = reader["createdbyagencyid"].ToString();

                        //T.training_sponsortype = reader["training_sponsortype"].ToString();
                        //T.noofquestion = Convert.ToInt32(reader["noofquestion"]);
                        T.sessionid = reader["Training.sessionid"].ToString();
                        //T.ttttt_content_desc = reader["ttttt_content_desc"].ToString();
                        //T.ttttt_session_dt = reader["ttttt_session_dt"].ToString();
                        //T.ttttt_session_time = reader["ttttt_session_time"].ToString();

                        //T.ttttt_session_duration = reader["ttttt_session_duration"].ToString();
                        //T.ttpss_participant_id = reader["ttpss_participant_id"].ToString();
                        //T.ttpss_session_id = reader["ttpss_session_id"].ToString();
                        //T.ttpss_onscreen_time = reader["ttpss_onscreen_time"].ToString();

                        //// Safely handle nullable integer columns
                        //if (!string.IsNullOrEmpty(reader["tdds_status"].ToString()))
                        //{
                        //    T.tdds_status = Convert.ToInt16(reader["tdds_status"]);
                        //}

                        //T.ttpss_status = reader["ttpss_status"].ToString();
                        //T.type = reader["type"].ToString();
                        //T.ttpss_created_on = reader["ttpss_created_on"].ToString();
                        T.participantstatus = reader["TestPartcipantID"].ToString();
                        //T.participantenrollstatus = reader["participantenrollstatus"].ToString();

                        //if (Common.CommonEnum.Get_Self_Paced_Trg(reader["trg_type"].ToString()) == 1)
                        //{
                        //    T.issessioncompleted = reader["ttpss_status"].ToString() == "1" ? 1 : 0;
                        //}
                        //else
                        //{
                        //    T.issessioncompleted = DateTime.Now > Convert.ToDateTime(reader["ttttt_session_dt"].ToString()) ? 1 : 0;
                        //}

                        //T.maxMarks = Convert.ToDecimal(reader["NoOfQuestion"]) * Convert.ToDecimal(reader["mark_per_question"]);
                        //T.mark_per_question = Convert.ToDecimal(reader["mark_per_question"]);

                        //// Handle potential null category matching
                        //var category = categories.FirstOrDefault(o => o.TrainingCategoryId.ToString().ToUpper() == reader["TrainingCategoryID"].ToString().ToUpper());
                        //T.Training_category_name = category != null ? category.TrainingCategoryName : string.Empty;

                        //T.Test_time = reader["start_time"].ToString();
                        //T.ttttt_status = reader["ttttt_status"].ToString();

                        //T.TrainingCategoryId = reader["TrainingCategoryId"].ToString();
                        //T.QuestionDifficultyID = reader["QuestionDifficultyID"].ToString();

                        //if (Convert.ToString(reader["trg_setting"]) != "")
                        //{
                        //    try
                        //    {
                        //        Trg_Setting p = new Trg_Setting();
                        //        p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(reader["trg_setting"]));
                        //        T.trg_Setting = p;
                        //        if (p.displaycontrols != null)
                        //        {
                        //            if (p.displaycontrols.Where(o => o.id == 9).ToList().Count() > 0)
                        //            {
                        //                if (p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext != "")
                        //                {
                        //                    T.trainingcode = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;

                        //                }
                        //            }
                        //        }



                        //    }
                        //    catch
                        //    {
                        //        T.trg_Setting = null;

                        //    }

                        //}
                        //else
                        //{
                        //    T.trg_Setting = null;
                        //}

                        assingvaluation.Add(T);
                    }
                }
            }

            return assingvaluation;

        }
    }

}
