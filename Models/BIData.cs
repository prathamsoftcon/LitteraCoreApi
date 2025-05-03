namespace LitteraCore.Models
{
    public class BIData
    {

    }
    public class BIReports
    {
        public string reportid { get; set; }

        public string workspaceid { get; set; }

        public string title { get; set; }
        public string description { get; set; }
    }
    public class PowerBIEmbedDetails
    {
        public string AccessToken { get; set; }
        public string EmbedUrl { get; set; }
        public string ReportId { get; set; }
    }
}
