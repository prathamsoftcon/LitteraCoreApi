using Azure;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace LitteraCore.DBContext
{
    public class FeedbackDB
    {
        private readonly IConfiguration _configuration;
        public FeedbackDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<TrainingFeedback> Get_Share_Feedback(string participantid)
        {

            List<FeedbackQuestion> AL = new List<FeedbackQuestion>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("select * from Survey360.Vw_su_particiapnt_training where participantid='" + participantid + "'", con);
            cmd.CommandType = CommandType.Text;


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            dt = ds.Tables[0];
            List<TrainingFeedback> LFD = new List<TrainingFeedback>();
            foreach (DataRow dr in dt.Rows)
            {
                TrainingFeedback FD = new TrainingFeedback();
                FD.trainingid = Convert.ToString(dr["trainingid"]);
                FD.sharefeedbackid = Convert.ToString(dr["sharefeedbackiD"]);
                FD.sureveyid = Convert.ToString(dr["SurveyID"]);
                FD.feedbacktitle = Convert.ToString(dr["SurveyName"]);
                FD.groupid = Convert.ToString(dr["GroupID"]);
                FD.SurveyDescription = Convert.ToString(dr["SurveyDescription"]);
                FD.tssr_responsdant_mobile = Convert.ToString(dr["tssr_responsdant_mobile"]);
                FD.status = Convert.ToInt32(dr["status"]);
                LFD.Add(FD);
            }

            return LFD;
        }

        public List<TrainingFeedback> Get_Participant_Feedback_Status(string participantid, string sharefeedbackid)
        {

            List<FeedbackQuestion> AL = new List<FeedbackQuestion>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("select * from Survey360.Vw_su_particiapnt_training where participantid='" + participantid + "' and sharefeedbackiD='" + sharefeedbackid + "'", con);
            cmd.CommandType = CommandType.Text;


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            dt = ds.Tables[0];
            List<TrainingFeedback> LFD = new List<TrainingFeedback>();
            foreach (DataRow dr in dt.Rows)
            {
                TrainingFeedback FD = new TrainingFeedback();
                FD.trainingid = Convert.ToString(dr["trainingid"]);
                FD.sharefeedbackid = Convert.ToString(dr["sharefeedbackiD"]);
                FD.sureveyid = Convert.ToString(dr["SurveyID"]);
                FD.feedbacktitle = Convert.ToString(dr["SurveyName"]);
                FD.groupid = Convert.ToString(dr["GroupID"]);
                FD.SurveyDescription = Convert.ToString(dr["SurveyDescription"]);
                FD.tssr_responsdant_mobile = Convert.ToString(dr["tssr_responsdant_mobile"]);
                LFD.Add(FD);
            }

            return LFD;
        }

        public List<FeedbackReport> Get_Feedback_Report_Data(string surveyid)
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("survey360.proc_su_survey360_report_data", con);
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@SurveyID", surveyid);


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            List<FeedbackReport> PL = new List<FeedbackReport>();
            foreach (DataRow dr in dt.Rows)
            {
                FeedbackReport P = new FeedbackReport();
                P.SurveyName = Convert.ToString(dr["SurveyName"]);
                P.SurveyCategory = Convert.ToString(dr["SurveyCategory"]);
                P.sharefeedbackiD = Convert.ToString(dr["sharefeedbackiD"]);
                P.GroupID = Convert.ToString(dr["GroupID"]);
                P.Groupname = Convert.ToString(dr["GroupName"]);
                P.SurveyID = Convert.ToString(dr["SurveyID"]);
                P.tssr_id = Convert.ToString(dr["tssr_id"]);

                P.tssr_sharefeedbackid = Convert.ToString(dr["tssr_sharefeedbackid"]);
                P.tssr_responsdant_name = Convert.ToString(dr["tssr_responsdant_name"]);
                P.tssr_responsdant_mobile = Convert.ToString(dr["tssr_responsdant_mobile"]);
                P.tssr_responsdant_email = Convert.ToString(dr["tssr_responsdant_email"]);
                P.tssr_responsee_name = Convert.ToString(dr["tssr_responsdant_name"]);

                P.tssr_responsee_mobile = Convert.ToString(dr["tssr_responsdant_mobile"]);
                P.tssr_responsee_email = Convert.ToString(dr["tssr_responsdant_email"]);
                if (Convert.ToString(dr["tssr_responsedate"]).ToString() != "")
                {
                    P.tssr_responsedate = Convert.ToDateTime(dr["tssr_responsedate"]).ToString("yyyy/MM/dd");
                }
                P.QuestionID = Convert.ToString(dr["tssqr_question_id"]);
                //P.QuestionText = Convert.ToString(dr["QuestionText"]);
                P.QuestionType = Convert.ToString(dr["QuestionType"]);
                //P.Tags = Convert.ToString(dr["Tags"]);

                //P.AnswerID = Convert.ToString(dr["AnswerID"]);
                //P.AnswerText = Convert.ToString(dr["AnswerText"]);
                P.tssqr_id = Convert.ToString(dr["tssqr_id"]);

                P.tssqr_sharefeedbackid = Convert.ToString(dr["tssqr_sharefeedbackid"]);
                P.tssqr_question_id = Convert.ToString(dr["tssqr_question_id"]);
                P.tssqr_answer_id = Convert.ToString(dr["tssqr_answer_id"]);
                P.tssqr_question_response = Convert.ToString(dr["tssqr_question_response"]);
                if (Convert.ToString(dr["tssqr_responsedate"]) != "")
                {
                    P.tssqr_responsedate = Convert.ToDateTime(dr["tssqr_responsedate"]).ToString("yyyy/MM/dd");
                }

                P.respondenttype = Convert.ToString(dr["respondenttype"]);
                P.responseetype = Convert.ToString(dr["responseetype"]);
                P.trainingid = Convert.ToString(dr["trainingid"]);

                PL.Add(P);
            }


            if (surveyid != null)
            {
                PL = PL.Where(o => o.SurveyID.ToString().ToUpper() == surveyid.ToString().ToUpper()).ToList();
            }



            return PL;
        }

        public List<TemplateQuestion> Get_All_Template_Question_Data()
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("survey360.proc_su_survey360_template_question_data", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            List<TemplateQuestion> PL = new List<TemplateQuestion>();
            foreach (DataRow dr in dt.Rows)
            {
                TemplateQuestion P = new TemplateQuestion();
                P.questionid = Convert.ToString(dr["QuestionID"]);
                P.groupid = Convert.ToString(dr["GroupID"]);
                P.QuestionText = Convert.ToString(dr["QuestionText"]);
                P.QuestionType = Convert.ToInt32(dr["QuestionType"]);

                PL.Add(P);
            }






            return PL;
        }

        public List<RatingType> Get_Rating_Type()
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("survey360.proc_su_survey360_rating_data", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            List<RatingType> PL = new List<RatingType>();
            foreach (DataRow dr in dt.Rows)
            {
                RatingType P = new RatingType();
                P.ratingid = Convert.ToString(dr["RatingID"]);
                P.ratingtext = Convert.ToString(dr["RatingText"]);
                P.ratingvalue = Convert.ToString(dr["Ratingvalue"]);

                PL.Add(P);
            }






            return PL;
        }
        public List<singleChoiceAnswers> Get_MCQ_Answer()
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("survey360.proc_su_survey360_template_mcq_answer", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            List<singleChoiceAnswers> PL = new List<singleChoiceAnswers>();
            foreach (DataRow dr in dt.Rows)
            {
                singleChoiceAnswers P = new singleChoiceAnswers();
                P.QuestionID = Convert.ToString(dr["QuestionID"]);
                P.AnswerID = Convert.ToString(dr["AnswerID"]);
                P.AnswerText = Convert.ToString(dr["AnswerText"]);
                P.QuestionGroupid = Convert.ToString(dr["GroupID"]);

                PL.Add(P);
            }






            return PL;
        }


        public List<descriptiveAnswers> Get_DESC_Answer()
        {


            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("survey360.proc_su_survey360_template_desc_answer", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            List<descriptiveAnswers> PL = new List<descriptiveAnswers>();
            foreach (DataRow dr in dt.Rows)
            {
                descriptiveAnswers P = new descriptiveAnswers();
                P.QuestionID = Convert.ToString(dr["QuestionID"]);
                P.AnswerID = Convert.ToString(dr["AnswerID"]);
                P.AnswerText = Convert.ToString(dr["AnswerText"]);
                P.QuestionGroupid = Convert.ToString(dr["GroupID"]);

                PL.Add(P);
            }






            return PL;
        }

    
    }
}
