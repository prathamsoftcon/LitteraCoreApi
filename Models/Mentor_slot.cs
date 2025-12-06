namespace LitteraCore.Models
{
    public class Mentor_slot
    {
     public string? ttsl_id { get; set; }
     

    public string ttsl_session_id { get; set; }
    public string? slot_title { get; set; }
    public string ttsl_training_id { get; set; }
    public string ttsl_mentor_id { get; set; }
    public string ttsl_slot_date { get; set; }
    public string ttsl_start_time { get; set; }
    public decimal ttsl_duration { get; set; }
    public int ttsl_seats { get; set; }
    public int? ttsl_seats_vacant { get; set; }
    public int ttsl_mode { get; set; }
    public int ttsl_status { get; set; }
    public string? ttsl_link { get;set; }
    public string? ttsl_location { get; set; }
    public DateTime ttsl_createdon { get; set; }
    public string ttsl_created_by { get; set; }
    public string? mentor_name { get; set; }
    public int is_requested { get; set; } = 0;

    }
    public class session_slots
    {
        public string mentorid { get; set; }
        public string mentorname { get;set; }
        public string designation { get; set; }
        public decimal Rating { get; set; }

        public string imagepath { get; set; }

       
        public Mentor_slot[] Mentor_slot {get;set;}


    }

    public class slot_participant
    {
        public string? ttmssp_id { get; set; }
        public string ttmssp_ttmss_id { get; set; }
        public string ttmssp_ttpai_id { get; set; }
        public string ttmssp_participant_id { get; set; }
        public string? participant_name { get; set; }
        public string? participant_mobileno { get; set; }
        public string? participant_email { get; set; }
        public string ttmssp_created_by { get; set; }

     


    }
}
