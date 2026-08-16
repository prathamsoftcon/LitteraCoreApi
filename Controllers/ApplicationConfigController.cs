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
using Microsoft.AspNetCore.Authorization;

namespace LitteraCore.Controllers
{
 
    [ApiController]

    public class ApplicationConfigController : ControllerBase
    {


        private readonly ILogger<ApplicationConfigController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public ApplicationConfigController(
            IConfiguration configuration,
            ILogger<ApplicationConfigController> logger,
            IEmailService emailService)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
        }

       [Authorize(Policy = "PublicApiKey")]
        [HttpGet]
        [Route("api/GetClientData")]
        [SwaggerOperation("To get client specific data to show on page.")]
        public ClientData GetClientData()
        {
            return ClientData.Get_Client_Data();
        }

        [Authorize(Policy = "PublicApiKey")]
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
        [Authorize(Policy = "PublicApiKey")]
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

        [Authorize(Policy = "PublicApiKey")]
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


        [Authorize(Policy = "PublicApiKey")]
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


        [Authorize(Policy = "PublicApiKey")]
        [HttpPost]
        [Route("api/Send_Mail")]
        [SwaggerOperation("To send mail for single user.")]
        public async Task<IActionResult> Send_Mail(maildetails m)
        {
            if (m == null)
            {
                return BadRequest(new { message = "Mail details are required." });
            }

            if (string.IsNullOrWhiteSpace(m.recipientEmail))
            {
                return BadRequest(new { message = "Recipient email is required." });
            }

            if (string.IsNullOrWhiteSpace(m.subject))
            {
                return BadRequest(new { message = "Subject is required." });
            }

            if (string.IsNullOrWhiteSpace(m.message))
            {
                return BadRequest(new { message = "Message is required." });
            }

            try
            {
                await _emailService.SendEmailAsync(
                    m.recipientEmail.Trim(),
                    m.subject.Trim(),
                    m.message,
                    throwOnFailure: true
                );

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send public mail to {RecipientEmail}.", m.recipientEmail);
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    new { message = "Unable to send email. Please try again later." }
                );
            }

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
        [Authorize(Policy = "PublicApiKey")]
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
        [Authorize(Policy = "PublicApiKey")]
        [HttpPost]
        [SwaggerOperation("To sav audit trail")]
        [Route("api/Save_Audit_Trail_wk")]
        public async Task<Boolean> Save_Audit_Trail_wk(Audit_Trail m)
        {
            m.tyat_ip = GetClientIp();
            bool issaved = false;
            ApplicationConfigBL b = new ApplicationConfigBL(_configuration);
            issaved = b.Save_Audit_Trail(m);
            return true;

        }
        [Authorize(Policy = "PublicApiKey")]
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
            var forwardedIp = GetFirstForwardedIp(
                Request.Headers["X-Forwarded-For"].FirstOrDefault(),
                Request.Headers["X-Original-For"].FirstOrDefault(),
                Request.Headers["X-Real-IP"].FirstOrDefault(),
                Request.Headers["CF-Connecting-IP"].FirstOrDefault()
            );

            if (!string.IsNullOrWhiteSpace(forwardedIp))
            {
                return forwardedIp;
            }

            return NormalizeIpAddress(HttpContext.Connection.RemoteIpAddress?.ToString()) ?? string.Empty;
        }

        private static string? GetFirstForwardedIp(params string?[] headerValues)
        {
            foreach (var headerValue in headerValues)
            {
                if (string.IsNullOrWhiteSpace(headerValue))
                {
                    continue;
                }

                var forwardedAddresses = headerValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var address in forwardedAddresses)
                {
                    var normalizedIp = NormalizeIpAddress(address);

                    if (!string.IsNullOrWhiteSpace(normalizedIp))
                    {
                        return normalizedIp;
                    }
                }
            }

            return null;
        }

        private static string? NormalizeIpAddress(string? rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return null;
            }

            var candidate = rawValue.Trim().Trim('"');

            if (candidate.StartsWith("for=", StringComparison.OrdinalIgnoreCase))
            {
                candidate = candidate.Substring(4).Trim().Trim('"');
            }

            if (IPAddress.TryParse(candidate, out var parsedAddress))
            {
                return parsedAddress.IsIPv4MappedToIPv6
                    ? parsedAddress.MapToIPv4().ToString()
                    : parsedAddress.ToString();
            }

            if (Uri.TryCreate($"http://{candidate}", UriKind.Absolute, out var parsedUri)
                && IPAddress.TryParse(parsedUri.Host, out parsedAddress))
            {
                return parsedAddress.IsIPv4MappedToIPv6
                    ? parsedAddress.MapToIPv4().ToString()
                    : parsedAddress.ToString();
            }

            return null;
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
            if (m == null || string.IsNullOrWhiteSpace(m.subject) || string.IsNullOrWhiteSpace(m.message))
            {
                return BadRequest(new { message = "Subject and message are required." });
            }

            try
            {
                string bodytxt = WebUtility.UrlDecode(m.message);
                IEmailService dm = _emailService;
                List<string> recipients;
                if (participantttype == "2")
                {
                    recipients = m.recipientEmail?
                        .Where(email => !string.IsNullOrWhiteSpace(email))
                        .Select(email => email.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList() ?? new List<string>();
                }
                else
                {
                    ParticipantDB tdb = new ParticipantDB(_configuration);
                    recipients = tdb.Get_Trg_Participant_List(trainngid)
                        .Select(participant => participant.email)
                        .Where(email => !string.IsNullOrWhiteSpace(email))
                        .Select(email => email.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                if (recipients.Count == 0)
                {
                    return BadRequest(new { message = "At least one recipient email is required." });
                }

                await Task.WhenAll(recipients.Select(recipient =>
                    dm.SendEmailAsync(recipient, m.subject.Trim(), bodytxt, throwOnFailure: true)));

                return Ok(new { sent = recipients.Count });
            }
            catch (EmailDeliveryException ex)
            {
                _logger.LogError(ex, "Unable to send participant mail.");
                return StatusCode(StatusCodes.Status502BadGateway,
                    new { message = "Unable to send email. Please try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to prepare participant mail.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Unable to prepare email delivery." });
            }
        }





    }
}
