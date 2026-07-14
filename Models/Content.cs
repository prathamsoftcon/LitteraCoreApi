using LitteraCore.Models;

namespace LitteraCore.Models
{
    public class ContentFolder
    {
        public string? globalcontentfolderid { get; set; }
        public string? globalcontentfoldername { get; set; }
        public string? createdbyagencyid { get; set; }
        public string? createdon { get; set; }
        public string? IsActive { get; set; }
    }

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
        public int is_feedback_required { get; set; } = 0;
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

    // ===================================================================
    // Everything below added 2026-07-12 for the frm_global_content_library.aspx
    // -> React migration.
    // ===================================================================

    // JSON shape stored in ApplicationConfigDB's SettingValue column for the
    // (proposed, not-yet-created) SettingID "12" row.
    public class Share_Content_Google_Drive_Setting
    {
        public string settingid { get; set; }
        public bool share_content_on_google_drive { get; set; }
    }

    // Corrected 2026-07-12: this shape, the underlying stored proc name, and
    // every column name below were re-derived from the REAL old
    // implementation at C:\Projects\TraininingERP_old\Littera_MVC_API
    // (Controllers/ContentController.cs -> Models/Content/ContentBL.cs ->
    // Models/Content/ContentDB.cs -> Models/GlobalContent.cs), reached via
    // https://qa.littera.in/LitteraAPI/api/GlobalContent in the old app. The
    // earlier draft's proc name (Content.proc_get_global_content_list) and
    // several column names (TotalRecords, doc_status) were fabricated
    // placeholders that do not exist in the real database - see
    // ContentDB.Get_Global_Content_List for the full correction.
    public class GlobalContentListItem
    {
        public string GlobalContentID { get; set; }
        public string GlobalContentyTypeID { get; set; }
        // Resolved content-type label (e.g. "PDF", "JPEG", "MP4") - real
        // column "ContentType", used old-side to pick a default thumbnail
        // when GlobalthumbnailPath is empty. Not previously ported.
        public string ContentType { get; set; }
        public string GlobalContentTitle { get; set; }
        public string GlobalContentTag { get; set; }
        public string GlobalContentFolderID { get; set; }
        public string GlobalWysiwagText { get; set; }
        // Real column "GlobalFilePath" (relative path) - file_absolute_path
        // below is APPURL + upload-path-prefix + this value, computed in
        // ContentDB, matching the old app's Common.UploadPath convention.
        public string GlobalFilePath { get; set; }
        public string GlobalthumbnailPath { get; set; }
        public string GlobalFileName { get; set; }
        public string ContentCreatedBy { get; set; }
        public DateTime? ContentCreatedon { get; set; }
        public string ApprovedBy { get; set; }
        public string file_absolute_path { get; set; }
        public string thumbnail_absolute_path { get; set; }
        public int content_link_type { get; set; }
        public int tcm_content_reading_time { get; set; }
        public GlobalContentDmsInfo dmsinfo { get; set; } = new GlobalContentDmsInfo();

        // Internal paging helper only - one value per row returned by the SP.
        // Real column is "TotalRow_count", NOT "TotalRecords" (that name was
        // a fabricated guess in the earlier draft).
        public int TotalRecords { get; set; }
    }

    // Real DMS approval-status fields for a content item, read from the
    // tdds_* columns the old proc returns (see Common/DMS.cs in the old
    // Littera_MVC_API repo). The React page currently only renders
    // doc_status, but the rest are included for parity/future use rather
    // than silently dropped.
    public class GlobalContentDmsInfo
    {
        public string doc_id { get; set; }
        public int tat_type_id { get; set; }
        public string attached_doc_name { get; set; }
        public string CreatedBy_empid { get; set; }
        public string fwd_empid { get; set; }
        // doc_status convention inferred from Fill_Content_Data_on_Page's statustext
        // switch: 0 = Pending, 1 = Approved, -1 = Rejected, 9 = Suspended.
        public int doc_status { get; set; }
        public string docremark { get; set; }
        public string docno { get; set; }
        public int doctype { get; set; }
    }

    public class UpdateGlobalContent
    {
        // Content being edited (@w_GlobalContentID)
        public string GlobalContentID { get; set; }
        // @p_GlobalContentTitle
        public string GlobalContentTitle { get; set; }
        // Comma-joined tag list -> @p_GlobalContentTag
        public string GlobalContentTag { get; set; }
        // Content type id of the record being edited. Used ONLY to decide which
        // meaning @p_GlobalWysiwagText takes on (see ContentBL.Update_Global_Content).
        // Not sent to the SP directly.
        public string GlobalContentTypeID { get; set; }
        // Raw CDN link text as entered by the user in the edit dialog. Only
        // relevant/used when GlobalContentTypeID == 6ECEC2CD-2780-4DB5-B03C-CA37D3CC8B29
        // (the CDN content type). Ignored for all other content types.
        public string CdnLinkText { get; set; }
        // @w_GlobalthumbnailPath
        public string GlobalthumbnailPath { get; set; }
        // @p_tcm_content_reading_time - total seconds (hh/mm/ss already combined
        // client-side, matching getTotalSeconds() in the old JS)
        public int ContentReadingTime { get; set; }
    }

    // Shared by the single-item Approve/Reject modal and the bulk "Approve All"
    // button. DocId is deliberately typed as a single string so it can hold
    // either a lone content id (single-item path) or a comma-separated list of
    // ids (bulk path), letting both frontend flows share this one endpoint.
    public class ContentApprovalRequest
    {
        public string DocId { get; set; }
        public string CreatedBy { get; set; }
        public string BranchId { get; set; }
        public string DocRemark { get; set; }
        public string CreatedByEmpId { get; set; }
        public string FwdEmpId { get; set; }
        public int DocStatus { get; set; }
        public string ActionDate { get; set; }
    }

    // RCVP_TrainingSchedule_Get_Encrypted_QS response shape - single-element
    // array of {QSNAME, QSVALUE}, matching the exact shape the old frontend
    // already consumes.
    public class QSResult
    {
        public string QSNAME { get; set; }
        public string QSVALUE { get; set; }
    }

    // TRG_SHARE_CONTENT_MAIL response shape - single-element array of
    // {status, msg}, matching the old contract.
    public class ShareContentMailResult
    {
        public string status { get; set; }
        public string msg { get; set; }
    }

    public class ContentAttachmentStatus
    {
        public bool isattached { get; set; }
    }

    // "Upload Files" gap for frm_global_content_library.aspx - request shape for
    // the new api/Save_Global_Content (create). Ground truth for field names:
    // C:\Projects\TraininingERP_old\Xgentraining\XgenTrainingErp\UserControls\
    // uc_upload_global_content.ascx (LMS_UPLOAD_DATA(), the "data" object posted
    // to the old LitteraAPI's api/Save_Global_Content). Two deliberate
    // deviations from that old body: (1) no top-level "Createdon" field - the
    // old JS sent the literal string "Createdon":"string" (never a real date),
    // so this is dropped and DateTime.Now is used server-side instead; (2) no
    // nested "dmsinfo" object - the old body's dmsinfo.CreatedBy_empid and
    // dmsinfo.fwd_empid were BOTH always just the acting employee's id, and
    // dmsinfo.doc_status was ALWAYS 0 for a new upload (Pending) - so those
    // collapse to a single CreatedByEmpId field here, and doc_status=0 plus
    // tat_type_id=Global_Content_Id are hardcoded server-side in
    // ContentDB.Save_Global_Content rather than trusted from the client.
    public class SaveGlobalContent
    {
        public string GlobalContentID { get; set; }
        public string GlobalContentyTypeID { get; set; }
        public string GlobalContentTitle { get; set; }
        public string GlobalContentFolderID { get; set; }
        // WYSIWYG body text for WYSIWYG-type content, or the raw CDN link text
        // for CDN-type content (GlobalContentyTypeID == CDN_CONTENT_TYPE_ID) -
        // same dual-purpose field the old app already uses elsewhere (see
        // ContentBL.Update_Global_Content's wysiwygText handling).
        public string GlobalWysiwagText { get; set; }
        public string GlobalContentTag { get; set; }
        // Null for CDN-link content (nothing was actually uploaded); otherwise
        // the unique filename returned by the Upload/UploadFile call.
        public string GlobalFilePath { get; set; }
        public string GlobalFileName { get; set; }
        public string Global_Thumbnail_FilePath { get; set; }
        public string CreatedBy { get; set; }
        public string Branchid { get; set; }
        public int content_link_type { get; set; }
        public int tcm_content_reading_time { get; set; }
        // Collapses the old body's dmsinfo.CreatedBy_empid/fwd_empid (always
        // identical values in the old JS) into one field.
        public string CreatedByEmpId { get; set; }
        public string FwdByEmpId { get; set; }
    }

    public class SessionContentAttachment
    {
        public string trgid { get; set; }
        public string sessionid { get; set; }
        public string title { get; set; }
        public string createdby { get; set; }
        public string ttsad_tag { get; set; }
        public string permission { get; set; }          // "1" or "0" (sponsor on/off), sent as-is to the proc
        public string usertype { get; set; }
        public string globalcontentid { get; set; }
        public string branchid { get; set; }
        public string GlobalContentyTypeID { get; set; }
        public string GlobalContentFolderID { get; set; }
        public string GlobalFilePath { get; set; }
        public string GlobalFileName { get; set; }
        public string CreatedBy_empid { get; set; }
        public string fwd_empid { get; set; }
        public string contentdata { get; set; }          // wysiwyg text, was posted as top-level "contentdata" by the old Save_WYIWAG_Data dispatcher
    }
}
