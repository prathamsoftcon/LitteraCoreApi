using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
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


            if (trgdetail.TrainingStatus == "4")
            {
                trgdetail.is_reg_open = false;
            }
            else
            {
                if (trgdetail.T_EndDate >= System.DateTime.Now)
                {
                    trgdetail.is_reg_open = true;
                }
                else
                {
                    trgdetail.is_reg_open = false;
                }
            }

            return Ok(trgdetail);
        }
        [HttpGet]
        [Route("api/Training_Details_by_Code")]
        public IActionResult Training_Details_by_Code(string trainingcode)
        {
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail_By_Code(trainingcode);


            if (trgdetail.TrainingStatus == "4")
            {
                trgdetail.is_reg_open = false;
            }
            else
            {
                if (trgdetail.T_EndDate >= System.DateTime.Now)
                {
                    trgdetail.is_reg_open = true;
                }
                else
                {
                    trgdetail.is_reg_open = false;
                }
            }

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
        public IActionResult TrainingProgressReport(string trainingid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias,string sessionid = null, string participantid = null,string branchid=null)
        {

            SessionDB sdb = new SessionDB(_configuration);
            List<Session> sl = sdb.Get_Trg_Progress_Data(trainingid, participantid, branchid);



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

        //[HttpGet]
        //[Route("api/Generate_Certificate")]
        //public IActionResult Generate_Certificate(string trainingid, string participantid, string branchid,string APPURL,string Logo_Path)
        //{

        //   TrgBL tbl=new TrgBL(_configuration);
        //    string certificatedata = tbl.GET_TRG_CERTIFICATE(trainingid, participantid, branchid, APPURL, Logo_Path);
           
        //    return Ok(certificatedata);
        //}
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
        public IActionResult TRG_PARTICIPANT_DETAILS(string trainingid,string participantid=null,string branchid=null)
        {
           
            List<Participant> PL = new List<Participant>();
            ParticipantDB PDB = new ParticipantDB(_configuration);
            //List of training all participant
            PL = PDB.Get_TRG_PARTICIPANT_Data(trainingid, participantid, branchid);
            return Ok(PL);
        }

        [HttpGet]
        [Route("api/Trg_Type")]
        public IActionResult Trg_Type()
        {
            List<Trg_Type> T = new List<Trg_Type>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_Trg_Type();
            return Ok(T);
        }
        [HttpGet]
        [Route("api/Trg_Title")]
        public IActionResult Trg_Title()
        {
            List<Trg_Title> T = new List<Trg_Title>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_Trg_Title();
            return Ok(T);
        }


        [HttpGet]
        [Route("api/Generate_Certificate")]
        public IActionResult Generate_Certificate(string trainingid, string participantid, string branchid, string APPURL, string Logo_Path)
        {

            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            string certificatedata = tb.Geenerate_certificate_text(Trg, dtsignatory, participantid, APPURL, Logo_Path);

            return Ok(certificatedata);
        }




        [HttpGet]
        [Route("api/Generate_Certificate_All")]
        public IActionResult Generate_Certificate(string trainingid, string branchid, string APPURL, string Logo_Path)
        {

            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            List<Participant> p = new List<Participant>();
            ParticipantDB pdb = new ParticipantDB(_configuration);
            p = pdb.Get_TRG_PARTICIPANT_Data(trainingid);
            string certificatedata = "";
            foreach (Participant pr in p)
            {
                certificatedata= certificatedata+tb.Geenerate_certificate_text(Trg, dtsignatory, pr.ParticipantId, APPURL, Logo_Path);
            }

          
            return Ok(certificatedata);
        }


        [HttpPost]
        [Route("api/Get_Trg_Participant_List")]
        public IActionResult Get_Trg_Participant_List(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn = null, string searchvalue = null, string sortcolumn = null, string sortvalue = null,string filtername=null,string filtervalue=null, int pageno = 1, int pagesize = -1)
        {
           TrgBL tbl=new TrgBL(_configuration);
            List<Participant> p = new List<Participant>();
            p = tbl.Get_Trg_Participant_List(trainingid, participantid, branchid, searchcolumn, searchvalue, sortcolumn, sortvalue,filtername,filtervalue, pageno, pagesize);
            PaginationParam param= new PaginationParam{ PageNumber = 1, PageSize = pagesize };
            var result = Paging.GetPagedData(param, p);
            if (p.Count > 0)
            {
                result.TotalRecords = p.FirstOrDefault().totalrecords;
                result.TotalPages = (int)Math.Ceiling((double)p.FirstOrDefault().totalrecords / param.PageSize);
            }
            return Ok(result);

        }


        [HttpPost]
        [Route("api/Update_Training_Status")]
        public IActionResult Update_Training_Status(string trainingid, int trainingstatus, string reason, string createdby, string branchid)
        {
            TrgBL tbl = new TrgBL(_configuration);
           
            bool issaved = tbl.Update_Training_Status(trainingid,trainingstatus,reason,createdby,branchid);
         
            return Ok(issaved);

        }

        [HttpPost]
        [Route("api/Update_Bulk_Participant_Status")]
        public IActionResult Update_Bulk_Participant_Status(string trainingid, string branchid, string currentstatus, string updatedstatus, string createdbyempid,string? participantid= null)
        {
            TrgBL tbl = new TrgBL(_configuration);

            bool issaved = tbl.Update_Bulk_Participant_Status(participantid,trainingid,branchid,currentstatus,updatedstatus,createdbyempid);

            return Ok(issaved);

        }

        [HttpPost]
        [Route("api/Update_Multiple_Participant_Status")]
        public IActionResult Update_Multiple_Participant_Status(string trainingid, string branchid, string currentstatus, string updatedstatus, string createdbyempid, string? participantid = null)
        {
            TrgBL tbl = new TrgBL(_configuration);
            if (participantid != null)
            {
                string[] participant=participantid.Split(',');
                foreach(string part in participant)
                {
                    bool issaved = tbl.Update_Bulk_Participant_Status(part, trainingid, branchid, currentstatus, updatedstatus, createdbyempid);
                }
            }
          

            return Ok(true);

        }


        [HttpGet]
        [Route("api/Certificate_Details")]
        public IActionResult Certificate_Details(string ttpai_id)
        {
            TrgBL tbl=new TrgBL(_configuration);
            Certificate_Details c = new Certificate_Details();
            c=tbl.Get_Certificate_Details(ttpai_id);
            string grade= tbl.Calculate_Certificate_grade(c.trainingid, c.participantid);
            if(grade != "")
            {
                c.grade = grade;
            }
            else
            {
                return NotFound("आपकी अध्ययन अवधि सर्टिफिकेट प्राप्त करने के लिए अभी पर्याप्त नहीं है। कृपया कोर्स कंटेंट का अध्ययन करें और कोर्स में दी सभी प्रेक्टिकल गतिविधियों को करें। जब निर्धारित अध्ययन अवधि पूर्ण हो जाएगी, तब आप सर्टिफिकेट जनरेट कर सकेंगे और अपना ग्रेड देख सकेंगे।\r\nकोर्स कंटेंट Link - https://learningplatform.mpbou.in/view_more_content1.html");
            }
            
            
            return Ok(c);
        }


        [HttpGet]
        [Route("api/Verify_Certificate")]
        public IActionResult Verify_Certificate(string usercode,string trainingid)
        {
            string grade = "";
            TrgBL tbl = new TrgBL(_configuration);
            UserDB udb = new UserDB(_configuration);
            Trg_User_Details tud=new Trg_User_Details();
            tud=udb.Get_Trg_User_Details(usercode, trainingid);
           
            if(tud.agencyid != null)
            {
               grade = tbl.Calculate_Certificate_grade(tud.trainingid, tud.agencyid);
            }
            else
            {
                grade = "";
            }
            

            return Ok(grade);
        }


    }
}
