using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LitteraCore.Controllers
{
    public class AIController : Controller
    {   
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public AIController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration; 
            _logger = logger;
        }


        [HttpPost]
        [Route("api/Save_AI_Conversation")]
        public IActionResult Save_AI_Conversation([FromBody] AITool ai)
        {

            bool isSaved = false;
            AIBL cbl = new AIBL(_configuration);
            isSaved = cbl.Save_AI_Conversation(ai);
            return Ok(isSaved);

        }
        [HttpPost]
        [Route("api/Like_Conversation")]
        public IActionResult Like_Conversation([FromBody] AITool ai)
        {

            bool isSaved = false;
            AIBL cbl = new AIBL(_configuration);
            isSaved = cbl.Update_Conversation_Like(ai);
            return Ok(isSaved);

        }
        [HttpGet]
        [Route("api/GET_AI_CONVERSATION")]
        public IActionResult GET_AI_CONVERSATIO(PaginationParam param)
        {
            AIBL cbl = new AIBL(_configuration);
            PagedList <AITool> a = cbl.Get_AI_Tool_Conversations(param);
            return Ok(a);

        }

    }
   

}
