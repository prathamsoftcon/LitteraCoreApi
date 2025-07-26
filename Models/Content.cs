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


        public int minreadingtime { get { return 2; } set { minreadingtime = 2; } }

        public contentuserpermission[] contentuserpermission { get; set; }

        public string content_icon { get; set; }
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
        public string tpad_createdon { get; set; }
      
    }


}
