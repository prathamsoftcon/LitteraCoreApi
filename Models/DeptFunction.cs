using LitteraCore.Common.DMS;

namespace LitteraCore.Models
{
    public class DeptFunction
    {
        public string ttcf_id { get; set; }
        public string ttcf_name { get; set; }
        public string ttcf_description { get; set; }
        public string? ttcf_hod_designation_id { get; set; }

        public string? ttcf_objective { get; set; } 
        public string[] ttcf_uploads { get; set; }

        public string ttcf_branchid { get; set; }

        public string? ttcf_code { get; set; }
        public DMS DMS { get; set; }


    }
}
