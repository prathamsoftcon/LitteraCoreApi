using LitteraCore.Common.DMS;

namespace LitteraCore.Models
{
    public class JobPosition
    {
        public string ttcjp_id { get; set; }
        public string? ttcjp_code { get; set; }
        public string ttcjp_name { get;set; }
        public string? ttcjp_description { get; set; }
        public string? ttcjp_ttcjpl_id { get; set; }
        public string ttcjp_branchid { get; set; }  
        public string ttcjp_createdby { get; set; }
        public DateTime? ttcjp_createdon { get; set; }
        public DMS DMS { get; set; }
    }
    public class position_levels
    {
        public string ttcjpl_id { get; set; }
        public string ttcjpl_name { get; set; }
    }
}
