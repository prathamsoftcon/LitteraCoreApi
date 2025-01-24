using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace LitteraCore.Models
{
    public class Session
    {
        public string trainingid { get; set; }
        public string? trainingcode { get; set; }

        public string? training_title { get; set; }

        public string ttttt_session_row_no { get; set; }
        public string ttttt_session_id { get; set; }

        public string ttttt_timetableid { get; set; }

        public string ttttt_facultyid { get; set; }

        public string ttttt_content_desc { get; set; }

        public string ttttt_session_dt { get; set; }

        public string ttttt_session_time { get; set; }
        public string ttttt_session_duration { get; set; }
        public int ttttt_session_day { get; set; }
        public string ttttt_is_joint_session { get; set; }
        public string ttttt_session_end_time { get; set; }
        public int? ttttt_session_no { get; set; }

        public string ttttt_status { get; set; }
        public string tttttf_status { get; set; }
        public string ttttt_remark { get; set; }
        public int ttttt_session_week { get; set; }
        public int ttttt_type { get; set; }
        public int ttttt_session_duration_type { get; set; }

        public string Durationtype;
        public string ttttt_session_duration_type_name { get { return this.Durationtype; } set { Durationtype = ((Common.CommonEnum.SessionDurationType)ttttt_session_duration_type).ToString(); } }
        public string ttttt_tag { get; set; }
        public string ttttt_subject { get; set; }

        public string participant_seession_required { get; set; }


        public string facultyname { get; set; }

        public string hfacultyname { get; set; }


        public string facultyimgpath { get; set; }

        public string Attendance { get; set; }

        public string Session_type_name { get; set; }
        public string Session_type_icon { get; set; }

        public Meeting[] meeting { get; set; }

        public int noofcompletion { get; set; } = 0;

        // public int issessioncompleted { get; set; } = 0;

        public decimal completionpercentage { get; set; } = 0;

        public completionDetail[] completiondetail { get; set; }

        public SessionFaculties[] sessionFaculties { get; set; }

        public int ttttt_complimentory { get; set; }

        public string recurringid { get; set; }

        public DateTime sessiondate_time { get; set; }

        public int? module { get; set; }

        public string modulename { get; set; }
        public CompletionType completiontype { get; set; }

        public DisplayInfo[] displayInfos { get; set; }

        public DisplayInfo[] ActionInfos { get; set; }

        public string DisplayOrder { get; set; }

        public string display_txt { get; set; }

        public bool is_Session_Restricted { get; set; }

        public int is_feedback_Required { get; set; } = 1;




        public static string Get_Session_Icon(int sessiontype)
        {
            string icon = "";
            switch (sessiontype)
            {
                case 1:
                    icon = "<i class='fa fa-graduation-cap'></i>";
                    break;
                case 2:
                    icon = "<i class='fa fa-coffee' style='color:orange'></i>";
                    break;
                case 3:
                    icon = "<i class='fa fa-cab' style='color:green'></i>";
                    break;
                case 4:
                    icon = "<i class='fa fa-wechat' style='color:blueviolet'></i>";
                    break;
                case 5:
                    icon = "<i class='fa fa-file-powerpoint-o' style='color:red'></i>";
                    break;
                case 6:
                    icon = "<i class='fa fa-file-text-o'></i>";
                    break;
                case 7:
                    icon = "<i class='glyphicon glyphicon-check' style='color:burlywood'></i>";
                    break;
                case 8:
                    icon = "<i class='fa fa-child'></i>";
                    break;
                case 9:
                    icon = "<i class='fa fa-life-saver' style='color:blue'></i>";
                    break;
                case 10:
                    icon = "<i class='fa fa-flask' style='color:blue'></i>";
                    break;
                case 11:
                    icon = "<i class='fa fa-book' style='color:blue'></i>";
                    break;
            }
            return icon;


        }

        public static string Get_Session_Type_Name(int sessiontype)
        {
            string name = "";
            switch (sessiontype)
            {
                case 1:
                    name = "Academic";
                    break;
                case 2:
                    name = "Tea Break";
                    break;
                case 3:
                    name = "Tour";
                    break;
                case 4:
                    name = "Group Discussion";
                    break;
                case 5:
                    name = "Presentation";
                    break;
                case 6:
                    name = "Assignment";
                    break;
                case 7:
                    name = "Test";
                    break;
                case 8:
                    name = "Physical Training";
                    break;
                case 9:
                    name = "Sports";
                    break;
                case 10:
                    name = "Practical";
                    break;
                case 11:
                    name = "Self Paced";
                    break;
            }
            return name;


        }
    }
    public class completionDetail
    {
        public string agencyid { get; set; }
        public string agencyname { get; set; }

        public string emailid { get; set; }

        public string mobileno { get; set; }

        public string status { get; set; }
    }
    public class SessionFaculties
    {
        public string facultyid { get; set; }
        public string facultyname { get; set; }
        public string hfacultyname { get; set; }
        public string facultuimg { get; set; }
    }
    public class CompletionType
    {
        public int id { get; set; }
        //public string name { get; set; }
        //public bool isactive { get; set; }
        public activities[] activities { get; set; }
    }
    public class DisplayInfo
    {
        public string key { get; set; }
        public string name { get; set; }
        public bool value { get; set; }
    }
    public class activities
    {
        public string type { get; set; }
        public string id { get; set; }
    }


    public class Notes {
        public string ttsn_id { get; set; }
        public string ttsn_training_id { get; set; }
        public string ttsn_session_id { get; set; }
        public notes_detail[] ttsn_notes { get; set; }
        public string ttsn_created_by { get; set; }
        public string ttsn_createdon { get; set; }
    }
    public class notes_detail
    {
        public string notes { get; set; }
        public string createdon { get; set; }
    }

    public class TrgComment
    {
        public string tttcm_commentid { get; set; }
        public string tttcm_trg_id { get; set; }
        public string tttcm_session_id { get; set; }
        public string tttcm_created_by { get; set; }

        public string tttcm_comment { get; set; }
        public int tttcm_is_delete { get; set; }
        public DateTime tttcm_createdon { get; set; }

        public string AgencyName { get; set; }

        public TrgComment_reply[] comments { get; set; }
    }
    public class TrgComment_reply
    {
        public string tttcr_tttcm_commentid { get; set; }
        public string tttcr_replyid { get; set; }
        public string tttcm_commentid { get; set; }

        public string tttcr_replied_by { get; set; }

        public string AgencyName { get; set; }

        public string tttcr_comment { get; set; }

        public int tttcr_is_delete { get; set; }

       
        [Column(TypeName = "datetime")]
        public DateTime tttcr_createdon { get; set; }
    }

    public class SessionFeedback
    {
        //public int srno { get; set; }
        public string? contentid { get; set; }

        //public string content_title { get; set; }

        //public string facultyname { get; set; }
        public string? facultyid { get; set; }
        public SessionFeedbackQuestions[] Questions { get; set; }
    }
    public class SessionFeedbackQuestions
    {
        public string questionid { get; set; }

        //public string questiontext { get; set; }
        public string? questiontype { get; set; }
        public string? answerid { get; set; }
        public string? answerdescription { get; set; }

        public string? rating { get; set; }
        public int? isfacultyQuestion { get; set; }

        //public SessionFeedbackQuestions_Ratings[] rationOptions { get; set; }
    }


    public class SessionContentFacultyFeedback
    {
        [JsonProperty("feedback")]
        public SessionFeedback[] Feedback { get; set; }

       
    }

    public class user_session_status
    {
        public string trainingid { get; set; }
        public string sessionid { get; set; }
        public string userid { get; set; }
        public string status { get; set; }
       
    }

    public class SessionCompletionStatus
    {
        public string tttttm_training_id { get; set; }
        public string ttttt_session_id { get; set; }
        public string agencyid { get; set; }
        public int iscompleted { get; set; }
        public decimal percentcomplete { get; set; }

        public int totalparticipant { get; set; }
    }
    public class Sessiontype
    {
        public string id { get; set; }

        public string name { get; set; }

        public string DisplayClass { get; set; }
    }
    public class CreateSessionDTO
    {
        public string trainingid { get; set; }

        public string time_table_Id { get; set; }
        public string createdby { get; set; }

        public DateTime createdon { get; set; }

        public string branchid { get; set; }

        public string sessionid { get; set; }

        public string[] faculties { get; set; }

        public string subject { get; set; }
        public string description { get; set; }
        public string sessiontype { get; set; }
        public DateTime? date { get; set; }
        public int day { get; set; }
        public int week { get; set; }

        public string duration { get; set; }
        public string duration_type { get; set; }
        public int rowno { get; set; }
        public int isjointSession { get; set; }
        public int sessionno { get; set; }
        public string sessionTime { get; set; }
        public DateTime End_date_time { get; set; }

        public int status { get; set; }
        public string remark { get; set; }
        public string[] tags { get; set; }
        public int iscomplementory { get; set; }

        public string recurringid { get; set; }

        public string module { get; set; }
        public CompletionType completiontype { get; set; }


    }
    public class Trg_session_duration
    {
        public decimal Theory { get; set; }
        public decimal Practical { get; set; }
        public decimal Activity { get; set; }

        public string durationtype { get; set; }
    }
}
