using LitteraCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Diagnostics.Metrics;
using System.Net;
using LitteraCore.DBContext;
using Newtonsoft.Json;
using System.Data;
using LitteraCore.BLContext;
using LitteraCore.Common.EmailService;
using LitteraCore.Common;

namespace LitteraCore.Controllers
{
 
    [ApiController]

    public class ApplicationConfigController : ControllerBase
    {


        private readonly ILogger<ApplicationConfigController> _logger;
        private readonly IConfiguration _configuration;

        public ApplicationConfigController(IConfiguration configuration, ILogger<ApplicationConfigController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

       
        [HttpGet]
        [Route("api/GetClientData")]
        public ClientData GetClientData()
        {
            return ClientData.Get_Client_Data();
        }

        [Route("api/country")]
        [HttpGet]
        public IActionResult Country()
        {
            List<Country> CL = new List<Country>();
            LoginDB CDB = new LoginDB(_configuration);
            CL = CDB.Get_Countries();
            return Ok(CL);
        }
        [Route("api/Finacial_year")]
        [HttpGet]
        public IActionResult Finacial_year()
        {
            List<string> FINYEAR = new List<string>();
            int currentyear = System.DateTime.Now.Year;
            for (var i = (currentyear - 5); i <= (currentyear + 1); i++)
            {
                FINYEAR.Add(i.ToString() + "-" + (i + 1).ToString());
            }
            return Ok(FINYEAR);
        }

        [Route("api/Get_Application_Setting")]
        [HttpGet]
        public IActionResult Get_Application_Setting(int settingtype)
        {
            ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
            HttpResponseMessage response = new HttpResponseMessage();
            if (settingtype == 5 || settingtype == 4)
            {
                FEEDBACK_OTP_SETTING ml = new FEEDBACK_OTP_SETTING();
                ml = JsonConvert.DeserializeObject<FEEDBACK_OTP_SETTING>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
               return Ok(ml);

            }
            else if (settingtype == 3)
            {
                RegistrationField ml = new RegistrationField();
                ml = JsonConvert.DeserializeObject<RegistrationField>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
                return Ok(ml);
            }
            else if (settingtype == 1)
            {
                MobileLogin ml = new MobileLogin();
                ml = JsonConvert.DeserializeObject<MobileLogin>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
                return Ok(ml);
            }
            else if (settingtype == 2)
            {
                ParticipantApproval ml = new ParticipantApproval();
                ml = JsonConvert.DeserializeObject<ParticipantApproval>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
                return Ok(ml);
            }
            else if (settingtype == 6)
            {
                OTP_LOGIN_REQUIRED_SETTING ml = new OTP_LOGIN_REQUIRED_SETTING();
                DataTable dt = a.Get_Application_Setting(settingtype.ToString());
                ml = JsonConvert.DeserializeObject<OTP_LOGIN_REQUIRED_SETTING>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();
                return Ok(ml);
            }
            else if (settingtype == 7)
            {
                EMAIL_SEND_BY_APPLICATION ml = new EMAIL_SEND_BY_APPLICATION();
                DataTable dt = a.Get_Application_Setting(settingtype.ToString());
                ml = JsonConvert.DeserializeObject<EMAIL_SEND_BY_APPLICATION>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();
                return Ok(ml);
            }
            else if (settingtype == 8)
            {
                SMS_SEND_BY_APPLICATION ml = new SMS_SEND_BY_APPLICATION();
                DataTable dt = a.Get_Application_Setting(settingtype.ToString());
                ml = JsonConvert.DeserializeObject<SMS_SEND_BY_APPLICATION>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();
                return Ok(ml);
            }
            else if (settingtype == 9)
            {
                Branch_Configuration ml = new Branch_Configuration();
                DataTable dt = a.Get_Application_Setting(settingtype.ToString());
                ml = JsonConvert.DeserializeObject<Branch_Configuration>(dt.Rows[0]["SettingValue"].ToString());
            
                return Ok(ml);
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("api/Check_Payment_Gateway_Available")]
        public IActionResult Check_Payment_Gateway_Available()
        {
            ApplicationConfigBL a = new ApplicationConfigBL(_configuration);
            PaymentGatewaySetting data = a.Get_Payment_GateWay_Config();
            if (data.key != "")
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
          
        }


        [HttpPost]
        [Route("api/Send_Mail")]
        public async Task<Boolean> Send_Mail(maildetails m)
        {
            SmtpEmailService s = new SmtpEmailService(_configuration);
              await s.SendEmailAsync(m.recipientEmail, m.subject, m.message);
            return true;

        }
        [HttpPost]
        [Route("api/Save_Audit_Trail")]
        public async Task<Boolean> Save_Audit_Trail(Audit_Trail m)
        {
            m.tyat_ip = GetClientIp();
            bool issaved = false;
            ApplicationConfigBL b = new ApplicationConfigBL(_configuration);
            issaved=b.Save_Audit_Trail(m);
            return true;

        }
        [HttpPost]
        [Route("api/Save_Error_Log")]
        public async Task<Boolean> Save_Error_Log(Error_Log m)
        {
            m.tyel_ip = GetClientIp();
            bool issaved = false;
            ApplicationConfigBL b = new ApplicationConfigBL(_configuration);
            issaved = b.Save_Error_Log(m);
            return true;

        }

        [HttpPost]
        [Route("api/Get_Audit_Trail")]
        public IActionResult Get_Audit_Trail(PaginationParam param, string? userid=null, string? fromdate = null, string? todate = null)
        {
            ApplicationConfigBL a = new ApplicationConfigBL(_configuration);
            PagedList<Audit_Trail> pd = a.Get_Audit_Trail(param, userid, fromdate, todate);
            return Ok(pd);

        }
        [HttpPost]
        [Route("api/Get_Error_Log")]
        public IActionResult Get_Error_Log(PaginationParam param, string? userid = null, string? fromdate = null, string? todate = null)
        {
            ApplicationConfigBL a = new ApplicationConfigBL(_configuration);
            PagedList<Error_Log> pd = a.Get_Error_log(param, userid, fromdate, todate);
            return Ok(pd);

        }

        //[HttpGet("client-ip")]
        //public IActionResult GetClientIp()
        //{
        //    string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        //    // If the application is behind a proxy (like a load balancer), you might need to check the X-Forwarded-For header.
        //    if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        //    {
        //        clientIp = HttpContext.Request.Headers["X-Forwarded-For"];
        //    }

        //    return Ok(new { ClientIp = clientIp });
        //}
        [HttpGet("client-ip")]
        public string GetClientIp()
        {
            string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // If the application is behind a proxy (like a load balancer), you might need to check the X-Forwarded-For header.
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                clientIp = HttpContext.Request.Headers["X-Forwarded-For"];
            }

            return clientIp;
        }


        [Route("api/Get_Consent_Config")]
        [HttpGet]
        public IActionResult Get_Consent_Config()
        {
            bool isConsentRequired = false;
            ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
            HttpResponseMessage response = new HttpResponseMessage();

            Branch_Configuration ml = new Branch_Configuration();
            ml = JsonConvert.DeserializeObject<Branch_Configuration>(a.Get_Application_Setting("9").Rows[0]["SettingValue"].ToString());
            if (ml.max_level_allowed > 1)
            {
                isConsentRequired=true; 
            }
            else
            {
                isConsentRequired = false;
            }
            return Ok(isConsentRequired);

      

         
        }


    }
}
