using LitteraCore.Common.DMS;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LitteraCore.Controllers
{
   
    public class UserController :  Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("api/CreateParticipantUser")]
        public IActionResult CreateParticipantUser(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
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
                lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();
            }

            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                DMSBL DBL = new DMSBL(_configuration);
                List<ParticipantAdditionlInfo> PAI = PDB.Get_Participant_Additional_info(null, userid);
                var ttpaiIds = string.Join(",", PAI.Select(p => $"'{p.ttpai_id}'"));
                List<DMS> DS = new List<DMS>();
                if (ttpaiIds != "")
                {
                    DS = DBL.GET_DMS_STATUS_DATA_FOR_SELECTED_DOCID(ttpaiIds, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);
                }


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
                item.no_of_sessions = filter.Where(o => o.ttttt_status == "0").Select(x => x.ttttt_session_id).Distinct().Count();

                var filterfaculties = filter.ToList().Where(o => o.ttttt_facultyid != null);
                // item.faculties = filter.ToList().Where(o => o.ttttt_facultyid !=null);




            }

            var pagedList = Paging.GetPagedList(param, lwtc);
            var result = Paging.GetPagedData(param, lwtc);

            return Ok(result);
        }

    }
}
