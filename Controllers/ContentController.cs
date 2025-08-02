using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public ContentController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/ContentType")]
        public IActionResult GetFunction()
        {
            ContentBL CBL = new ContentBL(_configuration);
            List<contentType> AL = new List<contentType>();
            AL = CBL.Get_Content_Type();
            return Ok(AL);
        }

        [HttpGet]
        [Route("api/Trg_Content")]
        public IActionResult Trg_Content([FromQuery] PaginationParam filter, string trainingid = null, string sessionid = null, string tags = null)
        {
            
            ContentBL CBL = new ContentBL(_configuration);
            PagedResult<Content> AL = new PagedResult<Content>();
            AL = CBL.Get_Trg_Content(trainingid, sessionid,tags,filter);

        

            return Ok(AL);
        }



        [HttpPost]
        [Route("api/Learning_Time")]
        public IActionResult Learning_Time([FromBody]learningtime lt)
        {
            ContentBL CBL = new ContentBL(_configuration);
            bool issaved = CBL.save_participant_learning_time(lt);
            return Ok(issaved);
        }



        [HttpGet]
        [Route("api/GET_CONTENT_DETAILS")]
        public IActionResult GET_CONTENT_DETAILS(string ttsam_id, string participantid)
        {

            string ipaddress = GetClientIp();

            contentDetail cd = new contentDetail();
            ContentDB cdb = new ContentDB(_configuration);
            cd = cdb.Get_ttpai_from_Content(ttsam_id, participantid);
            contentDetail cdn=new contentDetail();
            cdn = cdb.Get_Content_Detail(ttsam_id);
            cd.content_path = cdn.content_path;
            cd.trainingid = cdn.trainingid;
            cd.sessionid = cdn.sessionid;
            //********
            UserDB UBL = new UserDB(_configuration);
            User amob = new User();
            if(cd.mobileno == null)
            {
                throw new Exception("You are not eligible to access this training.");
            }

            amob = UBL.GET_MOBILE_NO_DATA(cd.mobileno, 2);
            if(amob != null)
            {
                cd.userid = amob.userid;
            }
            ContentDB CDB = new ContentDB(_configuration);
            List<Content> AL = new List<Content>();
            PaginationParam param = null;
          
            bool issaved = cdb.INSERT_CONTENT_VISITING(ttsam_id, cd.mobileno, ipaddress);
            // Save Learning Time with 0 entry
            ContentBL CBL = new ContentBL(_configuration);
            learningtime lt=new learningtime {  tplt_Id=Guid.NewGuid().ToString(),
             tplt_learning_time=0,
             tplt_createdon=System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
             tplt_createdby= participantid,
             tplt_ttpai_id=cd.ttpai_id,
             tplt_ttsam_id= ttsam_id

            };


            bool islearningtimesaved = CBL.save_participant_learning_time(lt);

            AL = CDB.Get_Trg_Content(param, cd.trainingid, cd.sessionid);
            AL = AL.Where(o => o.ttsad_ttsam_id.ToString().ToUpper() == ttsam_id.ToString().ToUpper()).ToList();
            //********
            cd.Items = AL.ToArray();




            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_Session_Data_By_Trg(cd.trainingid);
            Session sd = s.Where(o=>o.ttttt_session_id.ToString().ToUpper()==cd.sessionid.ToString().ToUpper()).FirstOrDefault();
            cd.Session = sd;


            //get branchid
            UserBranch ub = new UserBranch();
            AgencyBL abl = new AgencyBL(_configuration);
            ub = abl.Get_User_Branche(cd.userid);
            if (ub.branches.Count() > 0)
            {
                cd.branchid = ub.branches.FirstOrDefault().branchid;
            }

            return Ok(cd);
        }

        //[HttpGet]
        //[Route("api/GlobalContentType")]
        //public IActionResult GlobalContentType()
        //{

        //    ContentBL CBL = new ContentBL(_configuration);
        //    List<contentType> ctype=new List<contentType>();
        //    ctype = CBL.Get_Content_Type();
        //    return Ok(ctype);
        //}
        [HttpGet("GETCLIENTIP")]
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


        [HttpPost]
        [Route("api/Activity_Data")]    
        public IActionResult Activity_Data([FromBody]activity_data a)
        {
            ContentBL CBL = new ContentBL(_configuration);
            bool issaved = CBL.Save_Activity_Data(a);
            return Ok(issaved);
        }


    }
}
