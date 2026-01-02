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
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;
using LitteraCore.Common.OTP;
using LitteraCore.Common.Token;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using LitteraCore.Common.SmsService;
using LitteraCore.Models.SmsSettings;

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
        [SwaggerOperation("To get client specific data to show on page.")]
        public ClientData GetClientData()
        {
            return ClientData.Get_Client_Data();
        }

        [Route("api/country")]
         [SwaggerOperation("To get country code.")]
        [HttpGet]
        public IActionResult Country()
        {
            List<Country> CL = new List<Country>();
            LoginDB CDB = new LoginDB(_configuration);
            CL = CDB.Get_Countries();
            return Ok(CL);
        }
        [Route("api/Finacial_year")]
        [SwaggerOperation("To get financial years.")]
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
        [SwaggerOperation("To get application setting data setting type=1 for mobile login data.2 for participant approval data.3 for registration fields setting,4/5 for feedback required setting,6 for OTP login required setting,7 for email send by application,8 sms send by application,9 for branch configuration,10 for masking required setting,11 for first login change password setting ")]
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
            else if (settingtype ==10)
            {
                Masking_Setting ml = new Masking_Setting();
                ml = JsonConvert.DeserializeObject<Masking_Setting>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
                return Ok(ml);

            }
            else if (settingtype == 11)
            {
                FIRST_LOGIN_CHANGE_PASSWORD_SETTING ml = new FIRST_LOGIN_CHANGE_PASSWORD_SETTING();
                ml = JsonConvert.DeserializeObject<FIRST_LOGIN_CHANGE_PASSWORD_SETTING>(a.Get_Application_Setting(settingtype.ToString()).Rows[0]["SettingValue"].ToString());
                return Ok(ml);

            }

            return Ok(response);
        }


        [HttpGet]
        [Route("api/Check_Payment_Gateway_Available")]
        [SwaggerOperation("To get payment gateway available or not if gateway available then this will return true.")]
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
        [SwaggerOperation("To send mail for single user.")]
        public async Task<Boolean> Send_Mail(maildetails m)
        {
            SmtpEmailService s = new SmtpEmailService(_configuration);
              await s.SendEmailAsync(m.recipientEmail, m.subject, m.message);
            return true;

        }


        [HttpGet]
        [Route("api/SEND_SMS")]
        [SwaggerOperation("To Send SMS ")]
        public async Task<IActionResult> SEND_SMS(string mobileno,int templatetype)
        {
            //Check Valid User
            SmsService _smsService = new SmsService(_configuration);
            SmsTemplate template = new SmsTemplate();
            template = _smsService.GetTemplateMsg(templatetype);
            string msg = template.Message;
            await _smsService.SendSmsAsync(mobileno, msg, template.TemplateID);



            return Unauthorized();
        }



        [HttpPost]
        [SwaggerOperation("To sav audit trail")]
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
        [SwaggerOperation("To save error log.")]
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
        [SwaggerOperation("To get Audit trail detail.")]
        public IActionResult Get_Audit_Trail(PaginationParam param, string? userid=null, string? fromdate = null, string? todate = null)
        {
            ApplicationConfigBL a = new ApplicationConfigBL(_configuration);
            PagedList<Audit_Trail> pd = a.Get_Audit_Trail(param, userid, fromdate, todate);
            return Ok(pd);

        }
        [HttpPost]
        [Route("api/Get_Error_Log")]
        [SwaggerOperation("To get error log.")]
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
        [SwaggerOperation("To get client -IP.")]
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
        [SwaggerOperation("To get consent configuration on basis of branch configuration .")]
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


        [HttpPost]
        [Route("TRG_SEND_PARTICIPANT_MAIL")]
        [SwaggerOperation("To send participant mail if participanttype=2 then this will send mail to send email id's else send to all participant's in given training.")]
        public async Task<IActionResult> TRG_SEND_PARTICIPANT_MAIL(
     bulk_maildetails m,
     string? participantttype,
     string? trainngid = null)  
        {
         
            var rows = new List<Dictionary<string, object>>();

            try
            {
              
                string bodytxt = WebUtility.UrlDecode(m.message);

              
                // ===== Optional Fields =====
                //if (s.ContainsKey("attachment"))
                //    attachment = s["attachment"];

                //if (s.ContainsKey("ccto"))
                //    ccto = s["ccto"];

                //if (s.ContainsKey("bccto"))
                //    bccto = s["bccto"];

                SmtpEmailService dm=new SmtpEmailService(_configuration);
                EmailConfiguration emailSetting = dm.Get_Mail_Setting();

                //if (!string.IsNullOrEmpty(attachment))
                //    attachment = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.TrimStart('/'));

                // ===== Send Mail =====
                if (participantttype == "2")
                {
                    var emails = m.recipientEmail;

                    foreach (var mail in emails)
                    {
                        _ = Task.Run(() =>
                            dm.SendEmailAsync(
                                mail.Trim(),
                                m.subject,
                                bodytxt

                            ));
                    }
                }
                else
                {
                    List<Participant> trgparticipant = new List<Participant>();
                    ParticipantDB tdb = new ParticipantDB(_configuration);
                    trgparticipant = tdb.Get_Trg_Participant_List(trainngid);



                    foreach (Participant row in trgparticipant)
                    {
                        string email = row.email;
                        if (!string.IsNullOrEmpty(email))
                        {
                            _ = Task.Run(() =>
                                dm.SendEmailAsync(
                                    email,
                                    m.subject,
                                    bodytxt
                                   
                                ));
                        }
                    }
                }

                return Ok(true);
            }
            catch (SqlException ex)
            {
                return NotFound();
       
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }





    }
}
