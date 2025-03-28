using LitteraCore.BLContext;
using LitteraCore.Common.EmailService;
using LitteraCore.Common.OTP;
using LitteraCore.Common.SmsService;
using LitteraCore.Common.Token;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace LitteraCore.Controllers
{
    
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly OtpManager _otpManager;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly IEmailService _mailService;
        public SupportController(IConfiguration configuration, OtpManager otpManager, ISmsService smsService, IEmailService emailService)
        {
            _configuration = configuration;
            _otpManager = otpManager;
            _smsService = smsService;
            _mailService = emailService;
        }

        [HttpPost]
        [Route("api/SupportQuery")]
        public IActionResult SupportQuery(Support u)
        {
            string refno = "";
            SupportBL SBL = new SupportBL(_configuration);

            refno = SBL.Insert_Support(u);
            return Ok(refno);


        }

        [HttpPost]
        [Route("api/Update_Support_Status")]
        public IActionResult GetSupportQuery(Update_Support us)
        {
            List<Support> s = new List<Support>();
            SupportBL SBL = new SupportBL(_configuration);
            bool issaved = SBL.Update_Status(us);
            return Ok(issaved);

        }

        [HttpGet]
        [Route("api/SupportQuery")]
        public IActionResult SupportQuery(string? fromdate=null,string? todate=null)
        {
            List<Support> s = new List<Support>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.GetSupportQuery();
            return Ok(s);

        }
    }
}
