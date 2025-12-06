using LitteraCore.Common.EmailService;
using LitteraCore.Models.SmsSettings;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Xml;

namespace LitteraCore.Common.SmsService
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendSmsAsync(string recipientMobile, string message, string templateId)
        {
      
            string url = "";
            string userid = "";
            string password = "";
            string senderid = "";
            string PEID = "";

            // Code to Get SMS Settings
            XmlDocument xmldoc = new XmlDocument();
            //xmldoc.Load(@"StaticFiles/SMSSetting.xml");
            xmldoc.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "SMSSetting.xml"));
            XmlNodeList Nodes = xmldoc.DocumentElement.SelectNodes("/SMSConfiguration");
            foreach (XmlNode Node in Nodes)
            {
                foreach (XmlNode Node2 in Node.ChildNodes)
                {
                    if (Node2.Name == "url")
                    {
                        url = Node2.InnerText;
                    }
                    if (Node2.Name == "USERID")
                    {
                        userid = Node2.InnerText;
                    }
                    if (Node2.Name == "PASSWORD")
                    {
                        password = Node2.InnerText;
                    }
                    if (Node2.Name == "SENDER_ID")
                    {
                        senderid = Node2.InnerText;
                    }
                    if (Node2.Name == "PEID")
                    {
                        PEID = Node2.InnerText;
                    }


                }
            }
            //*******Code to get SMS Template

            string finalurl = "" + url + "?user=" + userid + "&password=" + password + "&senderid=" + senderid + "&channel=Trans&DCS=0&flashsms=0&number=" + recipientMobile.ToString().Replace("-", "") + "&text=" + message + "&DLTTemplateId=" + templateId + "&route=5&PEId=" + PEID + "";
            try
            {
                WebRequest request = WebRequest.Create(finalurl);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                await request.GetResponseAsync();
            }
            catch (Exception ex)
            {

                try
                {
                    SmtpEmailService s = new SmtpEmailService(_configuration);
                    await s.SendEmailAsync("prince@prathamsoft.com", "SMS API ERROR", ex.Message);
                }
                catch(Exception ex1)
                {
                   
                }
            }
          

        }

        public SmsTemplate GetTemplateMsg(int templateType)
        {
            //*******Code to get SMS Template

            XmlDocument xmldocTemp = new XmlDocument();
            //xmldocTemp.Load(@"StaticFiles/SMSTemplate.xml");
            xmldocTemp.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "SMSTemplate.xml"));
            XmlNodeList Nodestemp = xmldocTemp.DocumentElement.SelectNodes("/SMSTemplate/Template");

            List<SmsTemplate> TL = new List<SmsTemplate>();
            foreach (XmlNode Node in Nodestemp)
            {
                SmsTemplate S = new SmsTemplate();
                foreach (XmlNode Node2 in Node.ChildNodes)
                {
                    if (Node2.Name == "ID")
                    {
                        S.ID = Node2.InnerText;
                    }
                    if (Node2.Name == "DLT_CT_ID")
                    {
                        S.TemplateID = Node2.InnerText;
                    }
                    if (Node2.Name == "text")
                    {
                        // S.Message = Node2.InnerText.Replace("(#otp#)", otp);
                        S.Message = Node2.InnerText;
                    }

                }
                TL.Add(S);
            }

            TL = TL.Where(o => o.ID == templateType.ToString()).ToList();
            return TL.FirstOrDefault();
        }
    }
}
