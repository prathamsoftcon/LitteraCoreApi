using Azure.Core;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace LitteraCore.BLContext
{
    public class ApplicationConfigBL
    {
        private readonly IConfiguration _configuration;
        public ApplicationConfigBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public PaymentGatewaySetting Get_Payment_GateWay_Config()
        {
            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "GatewayDetail.xml"));

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "GatewayDetail.xml"); // Modify with your actual file path

            // Load the XML file into an XDocument
            XDocument doc = XDocument.Load(filePath);

            // Extract the elements using LINQ to XML
            string imgUrl = doc.Root.Element("img")?.Value;
            string key = doc.Root.Element("key")?.Value;
            string secret = doc.Root.Element("secret")?.Value;

            PaymentGatewaySetting pg = new PaymentGatewaySetting
            {
                img = imgUrl,
                key = key,
                secret = secret

            };

            return pg;
        }
        //public bool Get_Applocation_Setting(int settingtype)
        //{
        //    ApplicationConfigDB a = new ApplicationConfigDB(_configuration);

        //    if (settingtype == 5 || settingtype == 4)
        //    {
        //      //  FEEDBACK_OTP_SETTING ml = new FEEDBACK_OTP_SETTING();
        //        ml = JsonConvert.DeserializeObject<FEEDBACK_OTP_SETTING>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());


        //    }
        //    else if (settingtype == 3)
        //    {
        //        RegistrationField ml = new RegistrationField();
        //        ml = JsonConvert.DeserializeObject<RegistrationField>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
        //        response = Request.CreateResponse(HttpStatusCode.OK, ml);
        //    }
        //    else if (settingtype == 1)
        //    {
        //        MobileLogin ml = new MobileLogin();
        //        ml = JsonConvert.DeserializeObject<MobileLogin>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
        //        response = Request.CreateResponse(HttpStatusCode.OK, ml);
        //    }
        //    else if (settingtype == 2)
        //    {
        //        ParticipantApproval ml = new ParticipantApproval();
        //        ml = JsonConvert.DeserializeObject<ParticipantApproval>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
        //        response = Request.CreateResponse(HttpStatusCode.OK, ml);
        //    }
        //    else if (settingtype == 6)
        //    {
        //        OTP_LOGIN_REQUIRED_SETTING ml = new OTP_LOGIN_REQUIRED_SETTING();
        //        DataTable dt = a.Get_Application_Setting(settingtype.ToString());
        //        ml = JsonConvert.DeserializeObject<OTP_LOGIN_REQUIRED_SETTING>(dt.Rows[0]["SettingValue"].ToString());
        //        ml.settingid = dt.Rows[0]["SettingID"].ToString();
        //        response = Request.CreateResponse(HttpStatusCode.OK, ml);
        //    }
        //    else if (settingtype == 7)
        //    {
        //        EMAIL_SEND_BY_APPLICATION ml = new EMAIL_SEND_BY_APPLICATION();
        //        DataTable dt = a.Get_Application_Setting(settingtype.ToString());
        //        ml = JsonConvert.DeserializeObject<EMAIL_SEND_BY_APPLICATION>(dt.Rows[0]["SettingValue"].ToString());
        //        ml.settingid = dt.Rows[0]["SettingID"].ToString();
        //        response = Request.CreateResponse(HttpStatusCode.OK, ml);
        //    }
        //    else if (settingtype == 8)
        //    {
        //        SMS_SEND_BY_APPLICATION ml = new SMS_SEND_BY_APPLICATION();
        //        DataTable dt = a.Get_Application_Setting(settingtype.ToString());
        //        ml = JsonConvert.DeserializeObject<SMS_SEND_BY_APPLICATION>(dt.Rows[0]["SettingValue"].ToString());
        //        ml.settingid = dt.Rows[0]["SettingID"].ToString();
        //        response = Request.CreateResponse(HttpStatusCode.OK, ml);
        //    }


        //}

    }
}
