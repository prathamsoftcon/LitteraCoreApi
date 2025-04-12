namespace LitteraCore.Models
{
    public class Dashboard
    {

    }
    public class DBAnalytics
    {
        public int upcoming_trg { get;set; }
        public int trg_in_progress { get;set;}
        public int trg_completed { get; set;}
        public decimal trg_time { get; set; }

        public int no_of_participant_nominated { get; set; }
        public int no_of_participant_registered { get; set; }
        public int no_of_faculties { get; set; }
        public int no_of_certificates { get; set; }
        public int no_of_pending_tests { get; set; }
        public int no_of_pending_assignment { get; set; }

    }
    public class Mock_Test_Tag
    {
        public string displayname { get; set; }
        public string tag { get; set; }
    }
}
