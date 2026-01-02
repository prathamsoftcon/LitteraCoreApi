using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
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


    }
}
