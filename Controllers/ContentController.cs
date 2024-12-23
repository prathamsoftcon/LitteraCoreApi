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

        //[HttpGet]
        //[Route("api/GlobalContentType")]
        //public IActionResult GlobalContentType()
        //{

        //    ContentBL CBL = new ContentBL(_configuration);
        //    List<contentType> ctype=new List<contentType>();
        //    ctype = CBL.Get_Content_Type();
        //    return Ok(ctype);
        //}


    }
}
