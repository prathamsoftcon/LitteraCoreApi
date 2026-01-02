using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerBI.Api.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace LitteraCore.Controllers
{
     [ApiController]
    public class MentorController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public MentorController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/Create_Mentor_Slot")]
        [SwaggerOperation("To create new mentor slot.")]
        public IActionResult Create_Mentor_Slot([FromBody] Mentor_slot m)
        {
            MentorBL UBL = new MentorBL(_configuration);
            bool issaved = false;
            issaved = UBL.Save_Mentor_slot(m);
            return Ok(true);
 
        }
        [HttpPost]
        [Route("api/Remove_Mentor_Slot")]
        [SwaggerOperation("To remove existing mentor slot.")]
        public IActionResult Remove_Mentor_Slot(string ttsl_id, string createdby)
        {
            MentorBL UBL = new MentorBL(_configuration);
            bool issaved = false;
            issaved = UBL.Delete_Mentor_slot(ttsl_id, createdby);
            return Ok(true);

        }
        [HttpPost]
        [Route("api/Get_Mentor_Slots")]
        [SwaggerOperation("To get mentor slots.")]
        public IActionResult Get_Mentor_Slots(string? ttsl_training_id, string? ttsl_session_id, string? ttsl_mentor_id, string? slot_date, int status=1, [FromBody]PaginationParam param=null)
        {
            MentorBL UBL = new MentorBL(_configuration);
            List<Mentor_slot> MS = new List<Mentor_slot>();
            MS = UBL.Get_Mentor_Slot(ttsl_training_id, ttsl_session_id, ttsl_mentor_id, slot_date, status, param);
            return Ok(MS);

        }

        [HttpGet]
        [Route("api/Get_Session_Slot")]
        [SwaggerOperation("To get particular session slot.")]
        public IActionResult Get_Session_Slots(string sessionid,string? participantid=null)
        {
            MentorBL UBL = new MentorBL(_configuration);
            List<session_slots> MS = new List<session_slots>();
            MS = UBL.Get_Session_Mentor_Slot(sessionid);
            if(participantid != null)
            {

            }
            return Ok(MS);

        }

        [HttpPost]
        [Route("api/Get_Slot_Participant")]
        [SwaggerOperation("To get slot participant.")]
        public IActionResult Get_Slot_Participant(string slotid, string? participantlist=null, [FromBody] PaginationParam param = null)
        {
            MentorBL UBL = new MentorBL(_configuration);
            List<slot_participant> MS = new List<slot_participant>();
            MS = UBL.Get_Slot_Participant(slotid, participantlist);
            var pagedList = Paging.GetPagedList(param, MS);
            var result = Paging.GetPagedData(param, MS);
            return Ok(result);

        }
        [HttpPost]
        [Route("api/Add_slot_Participant")]
        [SwaggerOperation("To attache participant in slot.")]
        public IActionResult Add_slot_Participant([FromBody] slot_participant m)
        {
            bool issaved = false;
            MentorBL UBL = new MentorBL(_configuration);
           
            issaved = UBL.Save_slot_Participant(m);
            return Ok();

        }
        [HttpPost]
        [Route("api/Delete_slot_Participant")]
        [SwaggerOperation("To delete participant from slot.")]
        public IActionResult Delete_slot_Participant(string slotid, string participantlist)
        {
            bool issaved = false;
            MentorBL UBL = new MentorBL(_configuration);

            issaved = UBL.Delete_slot_Participant(slotid, participantlist);
            return Ok();

        }


        [HttpGet]
        [Route("api/Get_Mentor_Notification_Option")]
        [SwaggerOperation("To check popup required to send notification or not[Hardcode].")]
        public IActionResult Get_Mentor_Notification_Option()
        {
           
          
            return Ok(new {is_popup_required=1 });

        }


    }
}
