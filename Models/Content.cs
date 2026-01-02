namespace LitteraCore.Models
{
    public class Content
    {
        public string ttsam_id { get; set; }
        public string ttsam_trg_id { get; set; }
        public string ttsam_ttttt_session_id { get; set; }
        public string ttsam_created_by { get; set; }
        public string ttsam_created_on { get; set; }
        public string ttsam_globalcontentid { get; set; }
        public string ttsad_ttsam_id { get; set; }
        public string ttsad_title { get; set; }
        public string ttsad_tag { get; set; }
        public string ttsad_status { get; set; }

        public string ttttt_session_dt { get; set; }


        public string ttsar_user_type_id { get; set; }

        public string empname { get; set; }
        public string emailid { get; set; }
        public string modifiedempname { get; set; }
        public string modifiedemailid { get; set; }
        public string GlobalContentyTypeID { get; set; }
        public string GlobalContentFolderID { get; set; }
        public string GlobalWysiwagText { get; set; }

        public string GlobalFilePath { get; set; }
        public string GlobalthumbnailPath { get; set; }
        public string GlobalFileName { get; set; }

        public string SessionAttachmentType { get; set; }
        public string tdds_status { get; set; }


        public int minreadingtime { get; set; }

        public contentuserpermission[] contentuserpermission { get; set; }

        public string content_icon { get; set; }

        public string content_language { get; set; } = "HI";

        public int is_feedback_required { get; set; }=0;
    }
    public class contentType
    {
        public string GlobalContentTypeID { get; set; }
        public string GlobalContentType { get; set; }
        public string GlobalContentDescription { get; set; }
        public string SessionAttachmentType { get; set; }
    }
    public class contentuserpermission
    {
        public string usertype { get; set; }
        public contentPermissions permission { get; set; }
    }
    public class contentPermissions
    {
        public string ttsar_view { get; set; }

        public string ttsar_edit { get; set; }

        public string ttsar_download { get; set; }
        public string ttsar_delete { get; set; }
    }

    public class contentDetail
    {
        public string content_path { get; set; }
        public string ttpai_id { get; set; }
        public string mobileno { get; set; }
        public string userid { get; set; }
        public string trainingid { get; set; }
        public string sessionid { get; set; }
        public bool feedbacksubmitted { get; set; } = false;

        public string branchid { get; set; }
        public Content[] Items { get; set; }

        public Session Session { get; set; }
    }


    public class activity_data
    {
        public string tpad_id { get; set; }
        public string tpad_activity_id { get; set; }
        public string tpad_ttpai_id { get; set; }
        public string tpad_ttsam_id { get; set; }
        public string tpad_activity_data { get; set; }
        public DateTime tpad_createdon { get; set; }
        public string? tpad_upload { get; set; }

    }
    public class Avg_Learning
    {
        public Avg_Learning_data Avg_Learning_data { get; set; }
      
    }

    public class Avg_Learning_data
    {
        public string tplt_trainingid { get; set; }
        public string tplt_ttsam_id { get; set; }
        public decimal content_total_Reading_time { get; set; }
        public string content_title { get; set; }
        public string tplt_sessionid { get; set; }
        public decimal avg_learning { get; set; }
        public string ttttt_subject { get; set; }
        public string ttttt_content_desc { get; set; }
       
     
    }


    public class Avg_Learning_data_Sessionwise
    {
        public string tplt_trainingid { get; set; }
        public string tplt_ttsam_id { get; set; }
        public string tplt_sessionid { get; set; }
        public decimal session_total_reading_time { get; set; }
        public decimal avg_learning { get; set; }
        public string ttttt_subject { get; set; }
        public string ttttt_content_desc { get; set; }


    }


}
