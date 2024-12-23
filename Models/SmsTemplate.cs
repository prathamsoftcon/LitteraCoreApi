namespace LitteraCore.Models.SmsSettings
{
    public class SmsTemplate
    {
        public string ID { get; set; }
        public string TemplateID { get; set; }
        public string Message { get; set; }
    }
    public enum TemplateType
    {
        Otp = 1
    }
}
