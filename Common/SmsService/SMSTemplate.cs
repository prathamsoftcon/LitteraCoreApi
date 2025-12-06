namespace LitteraCore.Common.SmsService
{
    public class SMSTemplate
    {
        public string ID { get; set; }

        public string DLT_CT_ID { get; set; }

        public string text { get; set; }
    }
    public class SMSSetting
    {
        public string url { get; set; }

        public string key { get; set; }

        public string userid { get; set; }

        public string password { get; set; }

        public string routeid { get; set; }
        public string SENDER_ID { get; set; }
        public string PEID { get; set; }
    }
}
