using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static LitteraCore.Common.CommonEnum;

namespace LitteraCore.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public TrainingController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/Categories")]
        public IActionResult GetCategories(string categoryid)
        {
            TrgBL CBL = new TrgBL(_configuration);
            List<TrainingCategory> AL = new List<TrainingCategory>();
            AL = CBL.Get_Training_Category(categoryid);
            return Ok(AL);
        }
        [HttpGet]
        [Route("api/Trainings")]
        public IActionResult GetTrainings(DateTime fromdate, DateTime todate)
        {
            List<Training> T=new List<Training>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_VW_Training_calendar(fromdate, todate);
            return Ok(T);
        }
        [HttpGet]
        [Route("api/Training_Day_Week")]
        public IActionResult Training_Day_Week(string fromdate, string todate)
        {
            List<TRG_DAY_WEEK> T = new List<TRG_DAY_WEEK>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_Day_Week_Count(fromdate, todate);
            return Ok(T);
        }

        [HttpGet]
        [Route("api/Training_Details")]
        public IActionResult Training_Details (string trainingid)
        {
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            return Ok(trgdetail);
        }
        [HttpGet]
        [Route("api/TrainingStatus")]
        public IActionResult TrainingStatus()
        {

            List<trgStatus> lu = new List<trgStatus>();
            foreach (int i in Enum.GetValues(typeof(Common.CommonEnum.TrainingStatus)))
            {
                trgStatus u = new trgStatus();
                u.id = i.ToString();
                u.name = Enum.GetName(typeof(Common.CommonEnum.TrainingStatus), i);
                lu.Add(u);


            }
            return Ok(lu);
        }

        [HttpPost]
        [Route("api/TrainingProgressReport")]
        public IActionResult TrainingProgressReport(string trainingid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias,string sessionid = null, string participantid = null)
        {

            SessionDB sdb = new SessionDB(_configuration);
            List<Session> sl = sdb.Get_Trg_Progress_Data(trainingid, participantid);



            if (sessionid != null)
            {
                sl = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).ToList();
            }
            List<Session> distsession = new List<Session>();
            List<string> sessionids = new List<string>();
            foreach (Session s in sl)
            {
                if (sessionids.Contains(s.ttttt_session_id))
                {
                    continue;
                }
                distsession.Add(s);
                sessionids.Add(s.ttttt_session_id);

            }
            sl = distsession;

            int totalitems = sl.Count();
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = sl;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(sl, searchCriterias.SearchCriteria.ToList());
            }

            sl = filteredItems;

            var pagedList = Paging.GetPagedList(param, sl);
            var result = Paging.GetPagedData(param, sl);
            //*********

            return Ok(result);
        }

        [HttpGet]
        [Route("api/Generate_Certificate")]
        public IActionResult Generate_Certificate(string trainingid, string participantid, string branchid,string APPURL,string Logo_Path)
        {

           TrgBL tbl=new TrgBL(_configuration);
            string certificatedata = tbl.GET_TRG_CERTIFICATE(trainingid, participantid, branchid, APPURL, Logo_Path);
           
            return Ok(certificatedata);
        }
        [HttpGet]
        [Route("api/Check_Signatory_Available")]
        public IActionResult Check_Signatory_Available(string trainingid)
        {

            TrgBL tbl = new TrgBL(_configuration);
            List<CERTIFICATE_SIGNATORY> signatory = tbl.Get_Signatory(trainingid);
            if (signatory.Count > 0)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
          
        }
        [HttpGet]
        [Route("api/Get_Certificate_Signatory")]
        public IActionResult Get_Certificate_Signatory(string trainingid)
        {

            TrgBL tbl = new TrgBL(_configuration);
            List<CERTIFICATE_SIGNATORY> signatory = tbl.Get_Signatory(trainingid);
            return Ok(signatory);

        }
        [HttpPost]
        [Route("api/TRG_PARTICIPANT_MAP")]
        public IActionResult TRG_PARTICIPANT_MAP([FromBody]  TRGMAPPING trgmapping)
        {
            bool issaved = false;
            TrgBL TBD = new TrgBL(_configuration);
            issaved = TBD.Save_Trg_Participant_Mapping(trgmapping);
            return Ok(issaved);
        }

        [HttpGet]
        [Route("api/TRG_SPONSOR")]
        public IActionResult TRG_SPONSOR(string trainingid)
        {
            List<Agency> EH = new List<Agency>();
            TrainingDB TBD = new TrainingDB(_configuration);
            EH = TBD.Get_Trg_Sponsor(trainingid);
            return Ok(EH);
        }

        [HttpGet]
        [Route("api/TRG_PARTICIPANT_DETAILS")]
        public IActionResult TRG_PARTICIPANT_DETAILS(string trainingid,string participantid=null)
        {
           
            List<Participant> PL = new List<Participant>();
            ParticipantDB PDB = new ParticipantDB(_configuration);
            //List of training all participant
            PL = PDB.Get_TRG_PARTICIPANT_Data(trainingid, participantid);
            return Ok(PL);
        }
    }
}
