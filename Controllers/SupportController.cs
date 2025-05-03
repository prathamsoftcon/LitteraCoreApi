using LitteraCore.BLContext;
using LitteraCore.Common;
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
        public IActionResult SupportQuery(string? fromdate=null,string? todate=null,string appurl=null)
        {
            List<Support> s = new List<Support>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.GetSupportQuery();
            s = s.Where(o => o.clienturl.ToString().ToUpper().Contains(appurl.ToString().ToUpper())).ToList(); 
            return Ok(s);

        }

        [HttpPost]
        [Route("api/Login_Failed")]
        public IActionResult Login_Failed(string? fromdate = null, string? todate = null, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias=null)
        {
            
            List<Login_Failed_User> s = new List<Login_Failed_User>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.Login_Failed_User(fromdate, todate, param.PageNumber, param.PageSize, searchCriterias);
            param.PageNumber = 1;
            var result = Paging.GetPagedData(param, s);
            if (s.Count > 0)
            {
                result.TotalRecords = s.FirstOrDefault().total;
                result.TotalPages = (int)Math.Ceiling((double)s.FirstOrDefault().total / param.PageSize);
            }
          
            return Ok(result);
        

        }



        [HttpPost]
        [Route("api/User_Analytics_Data")]
        public IActionResult User_Analytics_Data(string? fromdate = null, string? todate = null,int type=1, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias = null)
        {
            //type=1 First Login ,2-Password not updated,3-Password Updated
            List<Support_Analytical_Report> s = new List<Support_Analytical_Report>();
            SupportBL SBL = new SupportBL(_configuration);


            s = SBL.Login_Analytics(fromdate, todate, type, param.PageNumber, param.PageSize, searchCriterias);
            param.PageNumber = 1;
            var result = Paging.GetPagedData(param, s);
            if (s.Count > 0)
            {
                result.TotalPages = (int)Math.Ceiling((double)s.FirstOrDefault().total / param.PageSize);
                result.TotalRecords = s.FirstOrDefault().total;
            }

            return Ok(result);


        }


        [HttpGet]
        [Route("api/Learning_Time")]
        public IActionResult Learning_Time(string? trainingid = null, string? participantid = null, string? ttsam_id = null,int unit=1)
        {
            string unitname = "";
            decimal learning = 0;
            List<Support> s = new List<Support>();
            SupportBL SBL = new SupportBL(_configuration);
            learning = SBL.Learning_Time(trainingid, participantid, ttsam_id, unit);
            if (unit == 1)
            {
                unitname = "Min";
            }
            else if(unit == 2)
            {
                unitname = "Sec";
            }
            else if (unit == 3)
            {
                unitname = "Hr";
            }
            return Ok(new {learning= Math.Round(learning,2) ,unit= unitname });

        }


        [HttpPost]
        [Route("api/Learning_Time_Report")]
        public IActionResult Learning_Time_Report(string? trainingid = null, string? participantid = null, string? ttsam_id = null, int unit = 1, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias = null)
        {
            string unitname = "";
            decimal learning = 0;
            List<Learning_Time> s = new List<Learning_Time>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.Learning_Report(trainingid, participantid, ttsam_id, unit, param.PageNumber, param.PageSize);
           // s = SBL.Login_Analytics(fromdate, todate, type, param.PageNumber, param.PageSize, searchCriterias);
            param.PageNumber = 1;
            var result = Paging.GetPagedData(param, s);
            if (s.Count > 0)
            {
                result.TotalRecords = s.FirstOrDefault().totalrecords;
                result.TotalPages = (int)Math.Ceiling((double)s.FirstOrDefault().totalrecords / param.PageSize);
            }
            return Ok(result);

        }



        [HttpPost]
        [Route("api/SearchParticipant")]
        public IActionResult SearchParticipant(string? trainingid=null,string? searchcolumn=null,string? searchvalue=null,string ? branchid=null)
        {
            ParticipantDB pdb = new ParticipantDB(_configuration);


            List<Participant> s = new List<Participant>();
            s = pdb.Get_Search_Participant(trainingid, null, branchid, searchcolumn, searchvalue);
            SupportBL SBL = new SupportBL(_configuration);
            return Ok(s);


        }


        [HttpGet]
        [Route("api/Learning_Report_Summary")]
        public IActionResult Learning_Report_Summary(string? trainingid = null, string? participantid = null, string? ttsam_id = null, string? branchid = null, int reporttype = 1, string? fromdate = null, string? todate = null, int pageno = 1, int pagesize = -1, string? SearchColumn = null, string? searchvalue = null, string? sortcolumn = null, string? sortdirection = null)
        {
            string unitname = "";
            decimal learning = 0;
            List<Learning_Report_Data> s = new List<Learning_Report_Data>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.Learning_Report_Data(trainingid, participantid, ttsam_id, branchid, reporttype,fromdate,todate,pageno,pagesize, SearchColumn, searchvalue, sortcolumn, sortdirection);
          
            return Ok(s);

        }


     

    }
}
