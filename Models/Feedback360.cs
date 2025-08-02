namespace LitteraCore.Models
{
    public class FeedbackReportSummery
    {
        string trainingid { get; set; }
        public string Surveyid { get; set; }
        public string Surveyname { get; set; }
        public string surveystatus { get; set; }

        public int no_of_respondent { get; set; }

        public string Result { get; set; }
    }
    public class FeedbackReport
    {
        public string trainingid { get; set; }
        public string SurveyName { get; set; }
        public string SurveyCategory { get; set; }
        public string sharefeedbackiD { get; set; }
        public string GroupID { get; set; }

        public string Groupname { get; set; }
        public string SurveyID { get; set; }

        public string tssr_id { get; set; }
        public string tssr_sharefeedbackid { get; set; }
        public string tssr_responsdant_name { get; set; }
        public string tssr_responsdant_mobile { get; set; }

        public string tssr_responsdant_email { get; set; }
        public string tssr_responsee_name { get; set; }
        public string tssr_responsee_mobile { get; set; }
        public string tssr_responsee_email { get; set; }

        public string tssr_responsedate { get; set; }
        public string QuestionID { get; set; }
        public string QuestionText { get; set; }

        public string QuestionType { get; set; }
        public string Tags { get; set; }
        public string AnswerID { get; set; }
        public string AnswerText { get; set; }

        public string tssqr_id { get; set; }
        public string tssqr_sharefeedbackid { get; set; }
        public string tssqr_question_id { get; set; }
        public string tssqr_answer_id { get; set; }

        public string tssqr_question_response { get; set; }
        public string tssqr_responsedate { get; set; }
        public string respondenttype { get; set; }
        public string responseetype { get; set; }

    }
    public class SurveyWiseResult
    {
        public string surveyid { get; set; }
        public string surveyname { get; set; }

        public string groupid { get; set; }

        public string sharefeedbackid { get; set; }
        public TSOR_Results[] tsqr_results { get; set; }

        public RatingQuesResult[] ratingResults { get; set; }

        public MCQResult[] mcqResults { get; set; }

        public DescResult[] descResults { get; set; }

    }
   
    public class TSOR_Results
    {
        public string tsqr_id { get; set; }

        public double totalresult { get; set; }
        public RatingQuesResult[] ratingResults { get; set; }

        public MCQResult[] mcqResult { get; set; }
    }

    public class RatingQuesResult
    {
        public string questionid { get; set; }
        public string questiontext { get; set; }

        public RatingResult[] ratingresult { get; set; }
    }
    public class RatingResult
    {
        public string answerid { get; set; }
        public string displaytext { get; set; }
        public double result { get; set; }
    }

    public class MCQResult
    {
        public string questionid { get; set; }
        public string questiontext { get; set; }
        public MCQResultOptions[] mcqresult { get; set; }

    }
    public class MCQResultOptions
    {
        public string answerid { get; set; }
        public string answertext { get; set; }
        public double result { get; set; }

    }
    public class DescResult
    {
        public string questionid { get; set; }
        public string questiontext { get; set; }
        public DescAnsText[] descresult { get; set; }

    }
    public class DescAnsText
    {
        public string answerid { get; set; }
        public string answertext { get; set; }

        public string result { get; set; }
    }
    public class TemplateQuestion
    {
        public string tqid { get; set; }
        public string questionid { get; set; }
        public string groupid { get; set; }
        public int status { get; set; }
        public string branchid { get; set; }

        public string Createdby { get; set; }
        public DateTime Createdon { get; set; }

        public DateTime Modifiedon { get; set; }

        public string QuestionText { get; set; }

        public int QuestionType { get; set; }

        public int QuestionStatus { get; set; }

        public int srno { get; set; }
    }
    public class RatingType
    {
        public string ratingid { get; set; }
        public string ratingtext { get; set; }
        public string ratingvalue { get; set; }
    }
    public class Question_DESC_QUESTIONWISE_DETAIL
    {
        public string surveyid { get; set; }
        public string surveyname { get; set; }
        public string groupid { get; set; }
        public string groupname { get; set; }

        public Question_Responsee_DESC_Results[] Question_DESC_Result_Detail { get; set; }

    }
    public class Question_DESC_Result_Detail
    {
        public string Questionid { get; set; }
        public string Questiontext { get; set; }
        public string responsee_name { get; set; }

        public string responsee_mobileno { get; set; }

        public string responsee_email { get; set; }

        public DescAnsText[] results { get; set; }

    }

    public class FeedbackReportSummery_trainingwise
    {
        public string trainingid { get; set; }
        public string trainingcode { get; set; }
        public decimal training_rating { get; set; }

        public int no_of_respondent { get; set; }

        public string Result { get; set; }
    }
}
