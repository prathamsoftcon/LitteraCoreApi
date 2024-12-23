using LitteraCore.Common.DMS;

namespace LitteraCore.Models
{
    public class JobRole
    {
        public string ttcjr_id { get; set; }
        public string? ttcjr_code { get; set; }
        public string? ttcjr_name { get; set; }

        public string? ttcjr_description { get; set; }

        public string ttcjr_branchid { get; set; }
        public string ttcjr_createdby { get; set; }
        public DateTime ttcjr_createdon { get; set; }

        public DMS DMS { get; set; }

    }
}
