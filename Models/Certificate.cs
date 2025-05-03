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
}
