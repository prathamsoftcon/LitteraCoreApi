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
            con.Open();
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
            con.Open();
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
    }
}
