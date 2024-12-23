using LitteraCore.Common.DMS;

namespace LitteraCore.Models
{
    public class Activity
    {
        public string ttca_id { get; set; }
        public string? ttca_code { get;set; }
        public string ttca_name { get;set;}
        public string? ttca_description { get; set;}
        public string ttca_branchid { get; set; }
        public string ttca_createdby { get; set; }
        public DateTime? ttca_createdon { get; set; }
        public DMS DMS { get; set; }
    }
}
