using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.EmailService;
using LitteraCore.Common.OTP;
using LitteraCore.Common.SmsService;
using LitteraCore.Common.Token;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
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
        private readonly IWebHostEnvironment _env;

      
        public SupportController(IConfiguration configuration, OtpManager otpManager, ISmsService smsService, IEmailService emailService, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _otpManager = otpManager;
            _smsService = smsService;
            _mailService = emailService;
            _env = env;
        }

        [HttpPost]
        [Route("api/SupportQuery")]
        [SwaggerOperation("To save support query.")]
        public IActionResult SupportQuery(Support u)
        {
            string refno = "";
            SupportBL SBL = new SupportBL(_configuration);

            refno = SBL.Insert_Support(u);
            return Ok(refno);


        }

        [HttpPost]
        [Route("api/Update_Support_Status")]
        [SwaggerOperation("To update support status.")]
        public IActionResult GetSupportQuery(Update_Support us)
        {
            List<Support> s = new List<Support>();
            SupportBL SBL = new SupportBL(_configuration);
            bool issaved = SBL.Update_Status(us);
            return Ok(issaved);

        }

        [HttpGet]
        [Route("api/SupportQuery")]
        [SwaggerOperation("To get support enquiries.")]
        public IActionResult SupportQuery(string? fromdate=null,string? todate=null,string appurl=null, [FromQuery] PaginationParam? param = null)
        {
            List<Support> s = new List<Support>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.GetSupportQuery();
            s = s.Where(o => o.clienturl.ToString().ToUpper().Contains(appurl.ToString().ToUpper())).ToList();
            var result = Paging.GetPagedData(param, s);
          
            return Ok(result);

        }

        [HttpPost]
        [Route("api/Login_Failed")]
        [SwaggerOperation("To save login failed entry.")]
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
        [SwaggerOperation("To get user's analytics data.")]
        public IActionResult User_Analytics_Data(string? fromdate = null, string? todate = null,int type=1, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias = null)
        {
            //type=1 First Login ,2-Password not updated,3-Password Updated
            param ??= new PaginationParam
            {
                PageNumber = 1,
                PageSize = 10
            };

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
        [SwaggerOperation("To get learning time data.")]
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
        [SwaggerOperation("To get learning time report data.")]
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
        [SwaggerOperation("To search specific participant.")]
        public IActionResult SearchParticipant(string? trainingid=null,string? searchcolumn=null,string? searchvalue=null,string ? branchid=null, [FromQuery] PaginationParam? param = null)
        {
            ParticipantDB pdb = new ParticipantDB(_configuration);


            List<Participant> s = new List<Participant>();
            s = pdb.Get_Search_Participant(trainingid, null, branchid, searchcolumn, searchvalue, "ParticipantId,ParticipantName,HParticipantName,photopath,totalrecords,ttpai_id,is_approve,email,mobileno,usercode");
            SupportBL SBL = new SupportBL(_configuration);

           
            var result = Paging.GetPagedData(param, s);
            if (s.Count > 0)
            {
                result.TotalRecords = s.FirstOrDefault().totalrecords;
                result.TotalPages = (int)Math.Ceiling((double)s.FirstOrDefault().totalrecords / param.PageSize);
            }
            if (s.Count > 0)
            {
                result.TotalRecords = s.Count();
                result.TotalPages = (int)Math.Ceiling((double)s.Count() / param.PageSize);
            }
            return Ok(result);
           


        }


        [HttpGet]
        [Route("api/Learning_Report_Summary")]
        [SwaggerOperation("To get learning report summary.")]
        public IActionResult Learning_Report_Summary(string? trainingid = null, string? participantid = null, string? ttsam_id = null, string? branchid = null, int reporttype = 1, string? fromdate = null, string? todate = null, int pageno = 1, int pagesize = -1, string? SearchColumn = null, string? searchvalue = null, string? sortcolumn = null, string? sortdirection = null)
        {
           if(trainingid != null)
            {
                trainingid = trainingid.Split(",".ToCharArray())[0].ToString();
            }
            string unitname = "";
            decimal learning = 0;
            List<Learning_Report_Data> s = new List<Learning_Report_Data>();
            SupportBL SBL = new SupportBL(_configuration);
            s = SBL.Learning_Report_Data(trainingid, participantid, ttsam_id, branchid, reporttype,fromdate,todate,pageno,pagesize, SearchColumn, searchvalue, sortcolumn, sortdirection);
          
            return Ok(s);

        }
        [HttpGet("api/ErrorFile")]
        [SwaggerOperation("To download error file.")]
        public IActionResult DownloadFile(string APIFolder, string ErrorDate)
        {
            // Full path to the file
            string fileName = "";
            string errorfolder = "";

            if (APIFolder.ToString().ToUpper() == "LITTERACORE")
            {
                fileName = "error" + ErrorDate.Replace("/", "").Replace("-", "") + ".log";
                errorfolder = "logs";
            }
            else if(APIFolder.ToString().ToUpper() == "LITTERAAPI")
            {
                fileName = "Log.txt";
                errorfolder = "Log";
            }
            else if (APIFolder.ToString().ToUpper() == "EVALUATION")
            {
                fileName = "EFLog.txt";
                errorfolder = "Log";
            }
            else if (APIFolder.ToString().ToUpper() == "SUVEY")
            {
                fileName = "WebApi" + ErrorDate.Replace("/", "").Replace("-", "") + ".log";
                errorfolder = "Log";
            }

            string projectRoot = _env.ContentRootPath;
            string parentFolder = Directory.GetParent(projectRoot)?.Parent+"/"+ APIFolder;
           
            var filePath = Path.Combine(parentFolder, errorfolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            //var contentType = GetContentType(filePath);
            //var fileBytes = System.IO.File.ReadAllBytes(filePath);

            //return File(fileBytes, contentType, fileName); // triggers browser download
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var contentType = GetContentType(filePath);

            return File(stream, contentType, fileName);
        }

        [HttpGet("api/GET_API_NAMES")]
        [SwaggerOperation("To get api names to download error file[Hardcode].")]
        public IActionResult GET_API_NAMES()
        {
            // Full path to the file

            List<string> S = new List<string>();
            S.Add("LITTERACORE");
            S.Add("LITTERAAPI");
            S.Add("EVALUATION");
            S.Add("SUVEY");
            S.Add("URLSHORTNER");


            return Ok(S);
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpGet]
        [Route("api/Check_First_Login")]
        [SwaggerOperation("To check participant first login.")]
        public IActionResult Check_First_Login(string participantid)
        {
            string unitname = "";
            decimal learning = 0;
            List<Learning_Report_Data> s = new List<Learning_Report_Data>();
            SupportDB SBL = new SupportDB(_configuration);
            bool ischanged = SBL.Check_First_Login(participantid);

            return Ok(ischanged);

        }
        [Authorize(Policy = "PublicApiKey")]
        [HttpGet]
        [Route("api/Check_First_Login_wk")]
        [SwaggerOperation("To check participant first login.")]
        public IActionResult Check_First_Login_wk(string participantid)
        {
            string unitname = "";
            decimal learning = 0;
            List<Learning_Report_Data> s = new List<Learning_Report_Data>();
            SupportDB SBL = new SupportDB(_configuration);
            bool ischanged = SBL.Check_First_Login(participantid);

            return Ok(ischanged);

        }

        [HttpPost]
        [Route("api/GET_ENROLLMENT_SUMMARY")]
        [SwaggerOperation("To get enrollment summary.")]
        public IActionResult GET_ENROLLMENT_SUMMARY(TrainingList trainings,string branchid, [FromQuery] PaginationParam? param = null)
        {
            List<Enrollment_Summary> li=new List<Enrollment_Summary>();
            List<Participant> p = new List<Participant>();
            ParticipantDB tdb = new ParticipantDB(_configuration);
            foreach (string trg in trainings.id)
            {
                p = tdb.Get_Trg_Participant_List(trg, null, branchid, null, null, null, null, null, null, 1, 1,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve,t_Name,TrainingCode");
                if (p.Count()  > 0)
                {
                    li.Add(new Enrollment_Summary {  trainingid = trg, t_code=p.FirstOrDefault().TrainingCode, total_enrollments=p.FirstOrDefault().totalrecords.ToString(), proposed_participants=null, t_name=p.FirstOrDefault().t_Name, no_of_active_lerners="0" });
                }
            }

         
            var result = Paging.GetPagedData(param, li);
           
            return Ok(result);

        }


    }
}
