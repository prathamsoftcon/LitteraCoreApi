using LitteraCore.Common.DMS;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using static LitteraCore.Common.CommonEnum;

namespace LitteraCore.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/Dashboard_analytics")]
        public IActionResult Dashboard_analytics(string usertype, string userid, DateTime startdate, DateTime enddate)
        {
            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate);
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
            UserTypeTrg usertrg = new UserTypeTrg();

            //***************
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            //**********
            userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();
            TrainingAnalytics t = new TrainingAnalytics();
            t = WDB.Get_StatusWise_Trg_Count(lwtc, startdate, enddate);

            DBAnalytics d=new DBAnalytics();
            d.upcoming_trg=t.trg_upcoming;
            d.trg_in_progress=t.trg_in_prog;
            d.trg_completed=t.trg_completed;
            d.trg_time = Convert.ToDecimal(t.trg_time);

            return Ok(d);
        }
        [HttpPost]
        [Route("api/Dashboard_Data")]
        public IActionResult Dashboard_Data(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param,[FromBody] SearchParam? searchCriterias)
        {
            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate);
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
            UserTypeTrg usertrg = new UserTypeTrg();

            //***************
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            //**********
            userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();


            //in case of participant not need to show proposed and cancelled training
            if (usertype == "5")
            {
                lwtc = lwtc.Where(o=>o.TrainingStatus !="2" && o.TrainingStatus != "3").ToList();
            }

            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                DMSBL DBL = new DMSBL(_configuration);
                List<ParticipantAdditionlInfo> PAI = PDB.Get_Participant_Additional_info(null, userid);
                List<DMS> DS = DBL.Get_DMS_STATUS(null, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);

                foreach (Training vw in lwtc)
                {
                    string ttpaid = PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).FirstOrDefault().ttpai_id;
                    string status = DS.Where(o => o.doc_id.ToString().ToUpper() == ttpaid.ToString().ToUpper()).FirstOrDefault().doc_status.ToString();
                    vw.participantstatus = status;
                }


            }
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = lwtc;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(lwtc, searchCriterias.SearchCriteria.ToList());
            }

            lwtc = filteredItems;

            //*********

            List<SessionCompletionStatus> trg_session_status = new List<SessionCompletionStatus>();
            SessionDB sdb = new SessionDB(_configuration);
            trg_session_status = sdb.Get_Session_Status(null, usertype, userid, startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
         
            List<Session> sl = sdb.Get_Session_Data(startdate, enddate);
            foreach (Training item in lwtc)
            {
                decimal completion = 0;
                List<SessionCompletionStatus> trg_status = new List<SessionCompletionStatus>();
                trg_status = trg_session_status.Where(o => o.tttttm_training_id.ToString().ToUpper() == item.TrainingId.ToString().ToUpper()).ToList();
                if (trg_status.Count() > 0)
                {
                    completion = Math.Round(trg_status.Sum(o => o.percentcomplete) / trg_status.Count(), 2);
                }


                item.trg_completionpercentage = completion;


                var filter = sl.Where(o => o.trainingid.ToString() == item.TrainingId.ToString());
                item.sessions = filter.ToList();
                item.no_of_sessions = filter.Select(x => x.ttttt_session_id).Distinct().Count();

                var filterfaculties = filter.ToList().Where(o => o.ttttt_facultyid != null);
                // item.faculties = filter.ToList().Where(o => o.ttttt_facultyid !=null);




            }

            var pagedList = Paging.GetPagedList(param, lwtc);
            var result = Paging.GetPagedData(param, lwtc);

            return Ok(result);
        }

        [HttpPost]
        [Route("api/Dashboard_All_Trg_Data")]
        public IActionResult Dashboard_All_Trg_Data(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {
            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate);
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
            //UserTypeTrg usertrg = new UserTypeTrg();

            ////***************
            //List<FilterUserTrg> FL = new List<FilterUserTrg>();
            //FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            ////**********
            //userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            //lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();

            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                DMSBL DBL = new DMSBL(_configuration);
                List<ParticipantAdditionlInfo> PAI = PDB.Get_Participant_Additional_info(null, userid);
                List<DMS> DS = DBL.Get_DMS_STATUS(null, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);

                foreach (Training vw in lwtc)
                {
                    if(PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).Count() > 0)
                    {
                        string ttpaid = PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).FirstOrDefault().ttpai_id;
                        string status = DS.Where(o => o.doc_id.ToString().ToUpper() == ttpaid.ToString().ToUpper()).FirstOrDefault().doc_status.ToString();
                        vw.participantstatus = status;
                    }
                    else
                    {
                        vw.participantstatus = null;
                    }
                    
                   
                }


            }
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = lwtc;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(lwtc, searchCriterias.SearchCriteria.ToList());
            }

            lwtc = filteredItems;

            lwtc= lwtc.Where(o=>o.TrainingStatus != "0").ToList();

            //*********

            //in case of participant not need to show proposed and cancelled training
            if (usertype == "5")
            {
                lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();
            }

            //*************

            List<SessionCompletionStatus> trg_session_status = new List<SessionCompletionStatus>();
            SessionDB sdb = new SessionDB(_configuration);
            trg_session_status = sdb.Get_Session_Status(null, usertype, userid, startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));

            List<Session> sl = sdb.Get_Session_Data(startdate, enddate);
            foreach (Training item in lwtc)
            {
                decimal completion = 0;
                List<SessionCompletionStatus> trg_status = new List<SessionCompletionStatus>();
                trg_status = trg_session_status.Where(o => o.tttttm_training_id.ToString().ToUpper() == item.TrainingId.ToString().ToUpper()).ToList();
                if (trg_status.Count() > 0)
                {
                    completion = Math.Round(trg_status.Sum(o => o.percentcomplete) / trg_status.Count(), 2);
                }


                item.trg_completionpercentage = completion;


                var filter = sl.Where(o => o.trainingid.ToString() == item.TrainingId.ToString());
                item.sessions = filter.ToList();
                item.no_of_sessions = filter.Select(x => x.ttttt_session_id).Distinct().Count();

                var filterfaculties = filter.ToList().Where(o => o.ttttt_facultyid != null);
                // item.faculties = filter.ToList().Where(o => o.ttttt_facultyid !=null);




            }

            var pagedList = Paging.GetPagedList(param, lwtc);
            var result = Paging.GetPagedData(param, lwtc);

            return Ok(result);
        }
    }
}
