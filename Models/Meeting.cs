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
}
