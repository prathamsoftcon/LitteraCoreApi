using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LitteraCore.Controllers
{
    public class DMSController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public DMSController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/GetAllActions")]
        [SwaggerOperation("To get all status of particular document id.")]
        public IActionResult GetAllActions(string docid, int tat_type_id)
        {
            _logger.LogError("test error.");
            DMSBL dbl = new DMSBL(_configuration);
            List<DMS_ACTION_INFO> AL=dbl.Get_Action_Info(docid,tat_type_id);
            return Ok(AL);
           
        }

        [HttpGet]
        [Route("api/DMSData")]
        [SwaggerOperation("To get dms information between dates.")]
        public IActionResult GetDMSData(string fromdate, string todate, string documentno, string applicationtypeid, string employeeid)
        {
            _logger.LogError("test error.");
            DMSBL dbl = new DMSBL(_configuration);
            List<DMS_DASHBOARD> AL = dbl.Get_DMS_Data(fromdate,todate,documentno,applicationtypeid,employeeid);
            return Ok(AL);

        }
        [HttpGet]
        [Route("api/Charges")]
        [SwaggerOperation("To get different agency charges.")]
        public IActionResult Charges()
        {
         
            DMSBL dbl = new DMSBL(_configuration);
            List<Charges> AL = dbl.Get_Charges();
            return Ok(AL);

        }


        [HttpPost]
        [Route("api/DOCUMENT_STATUS")]
        [SwaggerOperation("To update document status.")]
        public IActionResult POST_DOCUMENT_STATUS([FromBody]  DMS D)
        {
            DMSBL DBL = new DMSBL(_configuration);
            bool s = DBL.Update_DMS_DATA(D);
            if (s == true)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
         

        }
    }
}
