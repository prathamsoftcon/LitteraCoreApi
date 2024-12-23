using Azure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LitteraCore.Models
{
    public class Feedback
    {
    }
    public class TrainingFeedback
    {
        public string trainingid { get; set; }
        public string trainingcode { get; set; }

        public string trainingtitle { get; set; }
        public string sureveyid { get; set; }

        public string feedbacktitle { get; set; }
        public string groupid { get; set; }

        public string sharefeedbackid { get; set; }

        public string tssr_responsdant_mobile { get; set; }

        public string SurveyDescription { get; set; }

        public int status { get; set; }
        public string status_text = "";
        public string Status_txt { get { return Convert.ToString((Common.CommonEnum.Feedbackstatus)status); } set { status_text = ""; } }
    }
    public class FeedbackDesign
    {
        public string sharefeedbackiD { get; set; }
        public string SurveyID { get; set; }

        public string Surveyname { get; set; }

        public string SurveyDescription { get; set; }

        public string GroupID { get; set; }

        public string Templatename { get; set; }

        public string TemplateDescription { get; set; }



        public int Status { get; set; }

        public string status_text = "";
        public string Status_txt { get { return Convert.ToString((Common.CommonEnum.Feedbackstatus)Status); } set { status_text = "test"; } }
        public Respondent Respondent { get; set; }

        public Responsee Responsee { get; set; }

        public string createdby { get; set; }

        public string branchid { get; set; }

        public string Feedbacktype { get; set; }
        public string Feedbackdate { get; set; }
        public string Feedbackday { get; set; }

        public string trainingid { get; set; }

        public string traininigcode { get; set; }

        public string additionaltemplateid { get; set; }
        public string faculty_feedback_from { get; set; }

        public string faculty_feedback_to { get; set; }

        public int? faculty_feedback_from_day { get; set; }

        public int? faculty_feedback_to_day { get; set; }

        public string shareurl { get; set; }

        public string feedbackurl { get; set; }

        public int? participantcount { get; set; }
        public int? noofresponse { get; set; }
    }
    public class Respondent
    {
        public string type { get; set; }

        public string participanttype { get; set; }

        public FEEDBACKDetails[] details { get; set; }
    }

    public class Responsee
    {
        public string type { get; set; }

        public string participanttype { get; set; }

        public FEEDBACKDetails[] details { get; set; }
    }
    public class FEEDBACKDetails
    {
        public string rowid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }

    }
    public class FeedbackQuestion
    {
        [Key]
        [Column("QuestionID")]
        public Guid QuestionID { get; set; }
        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int? FeedbackType { get; set; }
        public int Status { get; set; }
        [Column("GroupID")]
        public Guid? GroupId { get; set; }

        public string Tags { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }


        public string ResponseTypeTxt { get { return ((Common.CommonEnum.QuestionResponseType)this.QuestionType).ToString(); } }

        public descriptiveAnswers[] descriptiveAnswers { get; set; }

        public singleChoiceAnswers[] singleChoiceAnswers { get; set; }

        public ratingAnswers[] ratingAnswers { get; set; }

        public int max { get; set; }

        public int srno { get; set; }

        public int mintextlength { get; set; }
        public int maxtextlength { get; set; }

    }
    public class descriptiveAnswers
    {
        public string AnswerID { get; set; }

        public string AnswerText { get; set; }

        public string QuestionID { get; set; }

        public string Status { get; set; }

        public string QuestionGroupid { get; set; }
        public int mintextlength { get; set; }
        public int maxtextlength { get; set; }
    }

    public class singleChoiceAnswers
    {
        public string AnswerID { get; set; }

        public string AnswerText { get; set; }

        public string QuestionID { get; set; }

        public string QuestionGroupid { get; set; }

        public string Status { get; set; }
    }
    public class ratingAnswers
    {
        public string AnswerID { get; set; }

        public string Rating { get; set; }

        public string QuestionID { get; set; }

        public string Status { get; set; }
    }

}
