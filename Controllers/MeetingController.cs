using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace LitteraCore.Controllers
{
    public class MeetingController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public MeetingController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/Meetings")]
        [SwaggerOperation("To get meeting list.")]
        public IActionResult Meetings(string finyear, string branchid, string usertype, string userid, PaginationParam param)
        {

            MeetingBL cbl = new MeetingBL(_configuration);
            List<Meeting> s = new List<Meeting>();
            s = cbl.Get_Meetings(finyear,branchid,usertype,userid,param);
          
            var result = Paging.GetPagedData(param, s);
            //if (s.Count > 0)
            //{
            //    if (param != null)
            //    {
            //        if(param.PageSize > 0)
            //        {
            //            result.TotalPages = (int)Math.Ceiling(s.FirstOrDefault().totalrecord /(double) param.PageSize);
            //        }
            //    }
               
                
            //}
           
            return Ok(result);
        }

        [HttpDelete]
        [Route("api/DeleteMeeting")]
        [SwaggerOperation("To delete existing neeting from software.")]
        public IActionResult DeleteMeeting(string meetingid)
        {
            bool isdeleted = false;
            MeetingBL cbl = new MeetingBL(_configuration);

            isdeleted = cbl.Delete_Meeting(meetingid);

            return Ok(isdeleted);
        }


        [HttpPost]
        [Route("api/Meetings_With_Search")]
        [SwaggerOperation("To delete existing meeting from software.")]
        public IActionResult Meetings_With_Search(string finyear, string branchid, string usertype, string userid, PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {

            MeetingBL cbl = new MeetingBL(_configuration);
            List<Meeting> s = new List<Meeting>();
            s = cbl.Get_Meetings(finyear, branchid, usertype, userid, param);

            var searchService = new SearchService();
            var filteredItems = s;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(s, searchCriterias.SearchCriteria.ToList());
            }

            var result = Paging.GetPagedData(param, filteredItems);
            //if (s.Count > 0)
            //{
            //    if (param != null)
            //    {
            //        if(param.PageSize > 0)
            //        {
            //            result.TotalPages = (int)Math.Ceiling(s.FirstOrDefault().totalrecord /(double) param.PageSize);
            //        }
            //    }


            //}

            return Ok(result);
        }

        // ------------------------------------------------------------------
        // Added 2026-09-25 - frm_session_meeting.aspx -> React migration
        // (Create / Edit / Create-from-session meeting page).
        //
        // The old page had no dedicated action names: it called the generic
        // API_ERP_TRAINING /TrainingAPI/Save_Data and /TrainingAPI/Get_Data
        // dispatchers with ProcedureName=trainingplan.proc_tp_lms_save_meeting,
        // TrainingPlan.proc_tp_lms_get_meeting_data and
        // TrainingPlan.Proc_tp_get_session_detail. So route names follow the
        // established gap-filling convention (RCVP_<domain>_<action>_Data for
        // writes, plain <domain> names for reads - cf. RCVP_Fees_Master_Save_Data
        // / Trg_Fees_Master in TrainingController). Grepped every controller
        // for these three routes first - none existed.
        //
        // The Zoom meeting itself is created by the separate meeting service
        // (reached from the browser via the /conference-api proxy); these
        // endpoints only persist / read the Littera-side record, exactly as
        // the old page did.
        // ------------------------------------------------------------------

        [HttpPost]
        [Route("api/RCVP_Meeting_Save_Data")]
        [SwaggerOperation("To insert/update a meeting record after the meeting has been created/updated on the meeting service.")]
        public IActionResult RCVP_Meeting_Save_Data([FromBody] MeetingSaveRequest meeting)
        {
            if (meeting == null)
            {
                return BadRequest(new { success = false, message = "Meeting details are required." });
            }

            try
            {
                MeetingBL mbl = new MeetingBL(_configuration);
                bool issaved = mbl.Save_Meeting(meeting);
                return Ok(issaved);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpGet]
        [Route("api/Meeting_Data")]
        [SwaggerOperation("To get one meeting's saved details for editing.")]
        public IActionResult Meeting_Data(string meetingid)
        {
            try
            {
                MeetingBL mbl = new MeetingBL(_configuration);
                MeetingEditData? data = mbl.Get_Meeting_Data(meetingid);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpGet]
        [Route("api/Meeting_Session_Detail")]
        [SwaggerOperation("To get session details used to prefill a meeting created from a session.")]
        public IActionResult Meeting_Session_Detail(string sessionid)
        {
            try
            {
                MeetingBL mbl = new MeetingBL(_configuration);
                MeetingSessionDetail? data = mbl.Get_Meeting_Session_Detail(sessionid);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

    }
}
