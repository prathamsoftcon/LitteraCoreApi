namespace LitteraCore.Models
{
    public class Certificate
    {
        public string certificate_bg_path { get; set; }

        public string certificate_text { get; set; }

        public variables[] variables { get; set; }
    }
    public class variables
    {
        public string name { get; set; }
        public string replacecolumnvalue { get; set; }
    }
    public class certificate_status
    {
        public string ttcgs_agenyid { get; set; }
        public int ttcgs_status { get; set; }
    }
    public class cert_status_list
    {
        public certificate_status[] certificate_Statuses { get; set; }

    }
}
