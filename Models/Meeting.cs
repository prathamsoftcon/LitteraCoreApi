namespace LitteraCore.Models
{
    public class Meeting
    {
        public string ttlm_id { get; set; }
        
        public string ttlm_host_id { get; set; }
        public string ttlm_ttttt_session_id { get; set; }
        public string ttlm_title { get; set; }
        public string ttlm_agenda { get; set; }

        public DateTime ttlm_date { get; set; }
        public string ttlm_time { get; set; }
        public int ttlm_duration { get; set; }
        public string ttlm_zid { get; set; }

        public string ttlm_zencpwd { get; set; }
        public string ttlm_zpwd { get; set; }
        public string ttlm_z_join_link { get; set; }
        public string ttlm_z_start_link { get; set; }

        public string ttlm_z_hostVideo { get; set; }
        public string ttlm_z_participantVideo { get; set; }
        public string ttlm_z_mute_upon_entry { get; set; }

        public string ttlm_z_watermark { get; set; }
        public string ttlm_z_approval_type { get; set; }
        public string ttlm_z_registration_type { get; set; }
        public string ttlm_z_audio { get; set; }

        public string ttlm_z_recording { get; set; }
        public string ttlm_z_joinBeforeHost { get; set; }
        public string ttlm_z_jointimeBeforeHost { get; set; }

        public string ttlm_z_hostemail { get; set; }
        public string ttlm_z_alt_hostemail { get; set; }
        public string ttlm_z_registration_url { get; set; }

        public DateTime ttlm_created_on { get; set; }
        public string ttlm_z_response_data { get; set; }
        public int tttlm_isenable { get; set; }
        public int tttlm_type { get; set; }

        public string tttlm_ttlms_LitteraMeetingID { get; set; }
        public string ToolParticipantCapacity { get; set; }

        public string? trainingid { get; set; }
        public string? training_code { get; set; }
        public string? training_title { get; set; }
        public string? course_director_id { get; set; }
        public string? associate_course_director_id { get; set; }

        public int totalrecord { get; set; }
    }

    // Added 2026-09-25 for the frm_session_meeting.aspx -> React migration
    // (Create / Edit / Create-from-session meeting). Request body for
    // api/RCVP_Meeting_Save_Data. Every property maps 1:1 to a parameter the
    // old page sent to trainingplan.proc_tp_lms_save_meeting through the
    // generic /TrainingAPI/Save_Data dispatcher
    // (JS_frm_session_meeting.js LMS_SAVE_MEETING_DETAILS, L515-676). Property
    // names are kept identical to the proc parameter names (minus '@') so
    // the mapping stays auditable. All strings: the old dispatcher sent every
    // value as a string and SKIPPED any empty one (Datamanager.vb
    // Save_Common_Data), which MeetingDB.Save_Meeting reproduces.
    public class MeetingSaveRequest
    {
        public string? ttlm_id { get; set; }
        public string? ttlm_host_id { get; set; }
        public string? ttlm_ttttt_session_id { get; set; }
        public string? ttlm_title { get; set; }
        public string? ttlm_agenda { get; set; }
        public string? ttlm_date { get; set; }
        public string? ttlm_time { get; set; }
        public string? ttlm_duration { get; set; }
        public string? ttlm_zid { get; set; }
        public string? ttlm_zencpwd { get; set; }
        public string? ttlm_zpwd { get; set; }
        public string? ttlm_z_join_link { get; set; }
        public string? ttlm_z_start_link { get; set; }
        public string? ttlm_z_hostVideo { get; set; }
        public string? ttlm_z_participantVideo { get; set; }
        public string? ttlm_z_mute_upon_entry { get; set; }
        public string? ttlm_z_watermark { get; set; }
        public string? ttlm_z_approval_type { get; set; }
        public string? ttlm_z_registration_type { get; set; }
        public string? ttlm_z_audio { get; set; }
        public string? ttlm_z_recording { get; set; }
        public string? ttlm_z_joinBeforeHost { get; set; }
        public string? ttlm_z_hostemail { get; set; }
        public string? ttlm_z_alt_hostemail { get; set; }
        public string? ttlm_z_response_data { get; set; }
        public string? tttlm_type { get; set; }
        public string? tttlm_isenable { get; set; }
        public string? tttlm_ttlms_LitteraMeetingID { get; set; }
    }

    // Edit-mode prefill (api/Meeting_Data). Mirrors the columns the old page
    // read from TrainingPlan.proc_tp_lms_get_meeting_data
    // (LMS_GET_PARTICULAR_MEETING_EDIT, JS L1244-1352). Property names are
    // all-lowercase on purpose - the old generic Get_Data dispatcher
    // lowercased every column name, and all-lowercase names also come
    // through Program.cs's camelCase JSON policy unchanged.
    public class MeetingEditData
    {
        public string? ttlm_id { get; set; }
        public string? ttlm_zid { get; set; }
        public string? ttlm_host_id { get; set; }
        public string? ttlm_ttttt_session_id { get; set; }
        public string? ttlm_title { get; set; }
        public string? ttlm_agenda { get; set; }
        public string? ttlm_date { get; set; }
        public string? ttlm_time { get; set; }
        public string? ttlm_duration { get; set; }
        public string? ttlm_zpwd { get; set; }
        public string? ttlm_z_hostvideo { get; set; }
        public string? ttlm_z_participantvideo { get; set; }
        public string? ttlm_z_mute_upon_entry { get; set; }
        public string? ttlm_z_jointimebeforehost { get; set; }
        public string? ttlm_z_recording { get; set; }
        public string? ttlm_z_hostemail { get; set; }
        public string? ttlm_z_alt_hostemail { get; set; }
        public string? tttlm_ttlms_litterameetingid { get; set; }
    }

    // Create-from-session prefill (api/Meeting_Session_Detail). Mirrors the
    // columns the old page read from TrainingPlan.Proc_tp_get_session_detail
    // (LMS_GET_SESSION_DETAILS, JS L1144-1210).
    public class MeetingSessionDetail
    {
        public string? session_id { get; set; }
        public string? t_name { get; set; }
        public string? t_code { get; set; }
        public string? subject { get; set; }
        public string? session_dt { get; set; }
        public string? starttime { get; set; }
        public string? duration { get; set; }
        public string? cd_mailid { get; set; }
        public string? acd_mailid { get; set; }
        public string? faculty_mailid { get; set; }
    }
}
