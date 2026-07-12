using LitteraCore.DBContext;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using static Azure.Core.HttpHeader;
using static LitteraCore.Common.CommonEnum;

namespace LitteraCore.Models
{
    public class Training
    {
        public System.Guid TrainingId { get; set; }
        public string TrainingNo { get; set; }
        public string Trainingcode { get; set; }
        public string CourseCode { get; set; }
        public string T_Name { get; set; }
        public string T_Details { get; set; }
        public System.Guid SPONSOR_AG_ID { get; set; }
        public Nullable<decimal> DueFees { get; set; }
        public int ReceivedFees { get; set; }
        public string SponsorName { get; set; }
        public string HSponsorName { get; set; }
        public string ParticipantLevel { get; set; }
        public int LevelId { get; set; }
        public string LevelDescription { get; set; }
        public string HLevelDescription { get; set; }
        public System.Guid CourseDirector { get; set; }
        public string CourseDirectorName { get; set; }
        public string HCourseDirectorName { get; set; }
        public Nullable<System.Guid> AssociateDirector { get; set; }
        public string AssociateDirectorName { get; set; }
        public string HAssociateDirectorName { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public System.DateTime T_StartDate { get; set; }
        public Nullable<System.DateTime> T_EndDate { get; set; }
        public int NoOfParticipants { get; set; }
        public Nullable<System.DateTime> T_ClosingDate { get; set; }
        public int NoOfParticipants_Registered { get; set; }
        public Nullable<System.Guid> TrainingCategoryId { get; set; }
        public string TrainingCategoryName { get; set; }
        public string HTrainingCategoryName { get; set; }
        public string TrainingStatus { get; set; }
        public Nullable<System.DateTime> StatusUpdateDate { get; set; }
        public string StatusReason { get; set; }
        public string HallName { get; set; }
        public string HHallName { get; set; }
        public string financialyear { get; set; }
        public int Training_SponsorType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.Guid CourseId { get; set; }
        public string benefitted { get; set; }
        public string objective { get; set; }
        public string prerequiste { get; set; }
        public string img_path { get; set; }

        public string img_path_absolute { get; set; }
        public Nullable<System.Guid> tttf_id { get; set; }
        public Nullable<byte> trg_type { get; set; }
        public string trg_validity { get; set; }
        public string tttt_name { get; set; }
        public string tttt_hname { get; set; }
        public int exptype { get; set; }
        public byte resident_status { get; set; }
        public string CourseName { get; set; }
        public string HCourseName { get; set; }
        public string DepartmentReferenceNo { get; set; }
        public int participation_type { get; set; }
        public decimal proposed_amt { get; set; }
        public int participant_type { get; set; }
        public Nullable<System.Guid> ChcekListType { get; set; }
        public Nullable<System.Guid> FeedbackType { get; set; }
        public decimal DateDiff { get; set; }

        public int no_of_sessions { get; set; }

        public List<Session> sessions { get; set; }

        public List<Faculty> faculties { get; set; }

        //Additional column added to return participant status in training in case of participant DB
        public string participantstatus { get; set; }

        public int isSelfPaced { get; set; }

        public string Participant_type_name { get; set; }

        public string Participantion_type_name { get; set; }

        public string status_txt { get; set; }
        public string participant_type_txt { get; set; }

        public TRGSPONSORS[] trgsponsors { get; set; }

        public decimal? t_actual_amt { get; set; }
        public decimal? amt_per_participant { get; set; }
        public decimal? total_received_amt { get; set; }

        public DisplayInfo[] displayInfos { get; set; }

        public DisplayInfo[] ActionInfos { get; set; }

        public decimal? trg_completionpercentage { get; set; }
        [JsonInclude]
        public Trg_Setting? trg_Setting { get; set; }

        public bool is_reg_open { get; set; }

        public decimal trg_rating { get; set; } = 0.0M;
        public int no_of_response { get; set; }

        public string trg_setting_search { get; set; }

        public int no_of_assignment { get; set; }

        public string assignment_faculties { get; set; }

        public trg_contact_person[] contact_person { get; set; }

    }

    public class trg_contact_person
    {
        public string person_name { get; set; }
        public string person_email { get; set; }
        public string person_mobile { get; set; }
    }


    public class FB_Share_Data
    {
        public string imagepath { get; set; }
        public string alt_imagepath { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }
    public class Tour_Config_Data
    {
        public int is_overview_required { get; set; }
        public int is_resume_course_required { get; set; }
        public int is_start_course_required { get; set; }
        public int is_instruction_course_required { get; set; }

        public int is_completed_required { get; set; }
    }




    public class Trg_Setting
    {
        public displaycontrols[] displaycontrols { get; set; }
        public session_setting Session { get; set; }

        public certificate_setting certificate_setting { get; set; }


    }
    public class certificate_setting
    {
        public int no_of_signatory_required { get; set; } = 0;
        public certificate_percentage[] certificate_percentage { get; set; }

    }
    public class certificate_percentage
    {
        public decimal from { get; set; }
        public decimal to { get; set; }
        public string grade { get; set; }
    }
    public class displaycontrols
    {
        public int id { get; set; }
        public string name { get; set; }
        public int isdisplay { get; set; }
        public string displaytext { get; set; }

        public string feedback_rating { get; set; }

        public string feedback_responsee { get; set; }
    }
    public class session_setting
    {
        public SessionRestriction SessionRestriction { get; set; }
        public SessionEntry SessionEntry { get; set; }
        public string SessionOrder { get; set; }
        public int Questions_Self_Test { get; set; }
        public int time_per_ques_Self_Test { get; set; }
        public int mark_per_ques_Self_Test { get; set; }

        public int feedback_on_session { get; set; } = 1;

        public int content_feedback_on_session { get; set; } = 1;

        public int? Session_Completion_on_any_one_content { get; set; }
    }
    public class SessionRestriction
    {
        public int isrestricted { get; set; }
        public int restrictionon { get; set; }
    }
    public class TrainingSettings
    {
        public SessionAccessibility SessionAccessibility { get; set; }
        public Session_Display Session_Display { get; set; }
        public Session_CompletionType[] CompletionType { get; set; }
        public SessionEntry Self_Paced_TRG_SessionEntry { get; set; }
        public SessionEntry Other_TRG_SessionEntry { get; set; }

        public string SessionOrder_selfpaced_trg { get; set; }
        public string SessionOrder_other_trg { get; set; }

        public certificate_setting certificate_setting { get; set; }
        public int Session_Completion_on_any_one_content { get; set; } = 0;
    }
    public class SessionAccessibility
    {
        public bool Restricted { get; set; }
        public int RestrictOn { get; set; }
    }
    public class Session_Display
    {
        public bool daywise { get; set; }
        public bool weekwise { get; set; }
    }
    public class Session_CompletionType
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isactive { get; set; }
        public activities[] activities { get; set; }
    }

    public class SessionEntry
    {
        public bool srno { get; set; }
        public bool Day { get; set; }
        public bool Date { get; set; }
        public bool Week { get; set; }
        public bool Module { get; set; }
        public bool Complementory { get; set; }
    }

    public class TRGSPONSORS
    {
        public string sponsorid { get; set; }
        public string sponsorname { get; set; }

        public string proposed_participant { get; set; }
        public string registered_participant { get; set; }
    }
    public class FilterUserTrg
    {
        public string trainingid { get; set; }
    }
    public class UserTrg
    {
        public string traininigid { get; set; }
    }
    public class TrainingAnalytics
    {
        public int trg_in_prog { get; set; }

        public int trg_completed { get; set; }

        public int trg_upcoming { get; set; }

        public string trg_time { get; set; }

        public int upcoming_faculty { get; set; }
    }

    public class UserTypeTrg
    {
    
   
    }
    public class TrainingCategory
    {
        public string TrainingCategoryId { get; set; }
        public string TrainingCategoryName { get; set; }
        public string HTrainingCategoryName { get; set; }
        public string CreatedBy { get; set; }
        public string BranchId { get; set; }
        public int IsJointDeptTraining { get; set; }
        public int Issessiongrouping { get; set; }
        public int usedbit { get; set; }

        public string parentcategoryid { get; set; }

        public string parentcategoryname { get; set; }

        public int categorylevel { get; set; }
    }

    public class TRG_DAY_WEEK
    {
        public int weekcount { get; set; }
        public int week { get; set; }
        public int daycount { get; set; }
        public string datevalue { get; set; }
        public int isenable { get; set; }

    }
    public class trgStatus
    {
        public string id { get; set; }

        public string name { get; set; }

        
    }
    public class CERTIFICATE_SIGNATORY
    {
        public string id { get; set; }

        public string name { get; set; }

        public int signatureorder { get; set; }
        public string signaturepath { get; set; }
        public string designation { get; set; }

    }
    public class TRGMAPPING
    {
        public string trainingid { get; set; }

        public string trainingid_New { get; set; }
        public string sponsorid { get; set; }
        public string sponsorid_new { get; set; }
        public string participantid { get; set; }
        public string branchid { get; set; }

        public string Createdby { get; set; }

        public string userid { get; set; }

        public int status { get; set; }

        public string uploadpath { get; set; }

        public string remark { get; set; }

        public string ttpai_id { get; set; }
    }
    public class Trg_Type
    {
        public string tttt_id { get; set; }
        public string tttt_name { get; set; }
        public string tttt_hname { get; set; }
        public int tttt_active { get; set; }

        // Added 2026-07-11 for the frm_training_type.aspx -> React migration
        // (Training Type master page, "Save" action). Read-only before (used
        // only for the already-migrated Get_Trg_Type() list/dropdown source);
        // CreatedBy is save-only - Get_Trg_Type() is not touched and will not
        // populate this field on read.
        public string CreatedBy { get; set; }
    }

    // Added 2026-07-11 for the frm_Master_Configuration.aspx -> React
    // migration (Fees tab, Sponsor Type dropdown). An earlier pass of
    // FeesTab.jsx hardcoded Sponsor Type as a static 3-option list (Single/
    // Joint/Self Financed), copied from the old page's ASPX markup - but the
    // old JS (TRG_FM_FILL_Sponsor_TYPE, JS L3244) actually loads this
    // dynamically from `trainingplan.proc_tp_get_sponsor_type`, same shape
    // as Trg_Type/Training Type above. Property names mirror the real
    // `ttst_*`-prefixed fields the old JS reads off the JSON response
    // (JsonObj["ttst_id"]/["ttst_name"]/["ttst_hname"], JS L3269-3272).
    public class Trg_Sponsor_Type
    {
        public string ttst_id { get; set; }
        public string ttst_name { get; set; }
        public string ttst_hname { get; set; }
    }
    public class Trg_Title
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string HCourseName { get; set; }
        public string CourseCode { get; set; }

        // Added 2026-07-10 for the frm_Master_Configuration.aspx -> React
        // migration (Training Title tab, "Save" action). This model was
        // read-only before (only 4 fields, matching what the current
        // Get_Trg_Title() query returns - see the KNOWN DATA-SHAPE GAP note
        // in TrainingTitleTab.jsx). These extra properties are for the new
        // Save endpoint's request body only; Get_Trg_Title() has NOT been
        // changed to populate them, so they'll be null/default on any GET
        // response until that separate read-side gap is fixed. Traced from
        // the REAL dm.InsUpdCourseDetails (C:\Projects\TraininingERP_old\
        // API_ERP\API_ERP_TRAINING\Datamanager.vb L2147), which the old
        // RCVP_SAVE_COURSE_MASTER action actually calls - it accepts both
        // of these even though the current read side doesn't expose them.
        public string Coursecategory { get; set; }
        // Kept as a string ("1"/"0"), not bool, to match the old page's own
        // convention and the fact that the underlying SQL parameter's real
        // type was never confirmed against a live database.
        public string Isactive { get; set; }
        public string CreatedBy { get; set; }
        public string BranchId { get; set; }

        // Added 2026-07-10 to close the read-side half of the data-shape gap
        // noted above: the grid was showing "-" in the Course Category column
        // because Get_Trg_Title() never populated Coursecategory/Isactive, even
        // though the same trainingplan.TP_GetCourse call the old grid used
        // already returns them (confirmed via JS_frm_Master_Configuration.js
        // L2531 - the old grid's JsonObj has coursecategory/isactive/
        // trainingcategoryname/htrainingcategoryname/courseduration/
        // durationtype/usedbit alongside the 4 fields already read here).
        // Coursecategory/Isactive above are now also populated by
        // Get_Trg_Title() (read) as well as used by Save_Trg_Title (write).
        // These two are read-only display fields for the category name -
        // there's no separate "set category name" input on this tab, only a
        // category id picker, so only the id (Coursecategory) round-trips on
        // save.
        public string TrainingCategoryName { get; set; }
        public string HTrainingCategoryName { get; set; }
    }


    public class Littera_Events
    {
        public string trainingId { get; set; }
        public string t_Name { get; set; }
        public string t_Details { get; set; }
        public int noOfParticipants_Registered { get; set; }
        public string trainingStatus { get;set; }
        public string img_path { get; set; }
        public string trg_type { get; set; }
        public string redirection_link { get; set; }
        public Trg_Setting? trg_Setting { get; set; }

    }
    public class Certificate_Details
    {
        public string name { get; set; }
        public string enrollmentno { get; set; }
        public string grade { get; set; }
        public string printdate { get; set; }
        public string trainingid { get;set; }
        public string participantid { get; set; }
        public string ttpai_id { get; set; }

        public string mobileno { get; set; }

        public string userid { get; set; }

    }

    public class usertrainings
    {
        public string trainingid { get; set; }
        public string trainingcode { get; set; }
        public string training_title { get; set; }
        public Trg_Setting? trg_Setting { get; set; }
    }

    // Added 2026-07-11 for the frm_Master_Configuration.aspx -> React
    // migration (Fees tab - the last remaining gap on this page). Backs both
    // the read side (Get_Trg_Fees_Master) and the save side
    // (Save_Trg_Fees_Master). Property names mirror the real
    // `tttf_*`-prefixed SQL columns/parameters traced from the old JS and
    // the old stored procedure calls (proc_get_training_fees_data /
    // proc_tp_ins_upd_training_fees) - see TrainingDB.cs for the full trace.
    // Everything is kept as string, same reasoning as Trg_Title's
    // Isactive/Coursecategory: the real SQL parameter/column types were
    // never confirmed against a live database, so string round-trips avoid
    // a wrong numeric/date type guess breaking the whole page.
    public class Trg_Fees_Master
    {
        public string FeesId { get; set; }
        public string TrgType { get; set; }
        public string SponsorType { get; set; }
        public string Duration { get; set; }
        public string DurationType { get; set; }
        // Non-residential training rate (old TERP_TM_TC_txtRate).
        public string NrFees { get; set; }
        // Extra fees per trainee (old TERP_TM_TC_txtExtraCharge).
        public string XnrFees { get; set; }
        // Residential training rate (old TERP_TM_TC_txtResidensialCharge).
        public string RFees { get; set; }
        // Extra lodging/boarding charge per trainee (old TERP_TM_TC_txtLBCharge).
        public string XrFees { get; set; }
        public string MinParticipant { get; set; }
        public string EfDate { get; set; }

        // Save-only fields (not returned by Get_Trg_Fees_Master) - same
        // pattern as Trg_Title's CreatedBy/BranchId.
        public string CreatedBy { get; set; }
        public string BranchId { get; set; }
    }
}
