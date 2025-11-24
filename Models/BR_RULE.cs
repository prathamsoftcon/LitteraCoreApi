namespace LitteraCore.Models
{
    public class BR_RULE
    {
        public Generate_Certificate Generate_Certificate { get; set; }

    }
    public class Generate_Certificate { 
         public Eligibility Eligibility { get; set; }
         public signatory signatory { get; set; }

        public string Certificate_text { get; set; }
        public int? Grade_required { get; set; }

    }
    public class Eligibility { 
        public int? Session_Completion_percentage { get;set; }
        public int? min_learning_time_min { get; set; }
        public int? attendance_percentage {  get;set; }


    }
    public class signatory
    {
        public int? min_signatory_required { get; set; }

    }
        
}
