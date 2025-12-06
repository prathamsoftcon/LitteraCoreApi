namespace LitteraCore.Common.EmailService
{
    public class EmailTemplate
    {
        public string ID { get; set; }

        public string subject { get; set; }

        public string text { get; set; }
    }
    public class EmailConfiguration
    {
        public string clienturl { get; set; }
        public string clientname { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string portno { get; set; }
        public string host { get; set; }
        public string header { get; set; }
        public string footer { get; set; }
    }
}
