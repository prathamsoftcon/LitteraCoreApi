using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;
using System.Text.Json;

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
        [SwaggerOperation("To save AI conversation data for chat UI.")]
        public IActionResult Save_AI_Conversation([FromBody] AITool ai)
        {

            bool isSaved = false;
            AIBL cbl = new AIBL(_configuration);
            isSaved = cbl.Save_AI_Conversation(ai);
            return Ok(isSaved);

        }
        [HttpPost]
        [Route("api/Like_Conversation")]
        [SwaggerOperation("To save conversation like data.")]
        public IActionResult Like_Conversation([FromBody] AITool ai)
        {

            bool isSaved = false;
            AIBL cbl = new AIBL(_configuration);
            isSaved = cbl.Update_Conversation_Like(ai);
            return Ok(isSaved);

        }
        [HttpGet]
        [Route("api/GET_AI_CONVERSATION")]
        [SwaggerOperation("To get AI conversation data.")]
        public IActionResult GET_AI_CONVERSATIO(PaginationParam param)
        {
            AIBL cbl = new AIBL(_configuration);
            PagedList <AITool> a = cbl.Get_AI_Tool_Conversations(param);
            return Ok(a);

        }



      

    }
   

}
