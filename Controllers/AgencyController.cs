using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace LitteraCore.Controllers
{
    public class AgencyController : Controller
    {
        private readonly ILogger<AgencyController> _logger;
       
        private readonly IConfiguration _configuration;
        public AgencyController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/GetDesignations")] 
        public IActionResult GetDesignation([FromQuery] PaginationParam filter)
        {
            _logger.LogError("test error.");
            AgencyDB cdb = new AgencyDB(_configuration);

            var pagedList = cdb.Get_Designations(filter);
            if (pagedList.Count() > 0)
            {
                var metadata = new
                {
                    pagedList.TotalCount,
                    pagedList.PageSize,
                    pagedList.CurrentPage,
                    pagedList.TotalPages,
                    pagedList.HasNext,
                    pagedList.HasPrevious,


                };
                return Ok(new PagedResult<Agency>
                {
                    Items = pagedList,
                    TotalRecords = pagedList.TotalCount,
                    PageSize = pagedList.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                    CurrentPage = pagedList.CurrentPage
                });

                // return Ok(pagedList);
            }
            else
                return NotFound();
        }


        [HttpGet]
        [Route("api/Agency")]
        public IActionResult getAgency(string agencytype = null,string filters = null, string agencyid = null, string tat_type_id = null, [FromQuery] PaginationParam param=null,string filter=null)
        {
            
            AgencyBL ABL = new AgencyBL(_configuration);
            PagedResult<Agency> AL = new PagedResult<Agency>();
            AL = ABL.Get_Agency(agencytype, agencyid, tat_type_id, param, filter);



            return Ok(AL);
        }

        [HttpGet]
        [Route("api/agency_by_charge")]
        public IActionResult agency_by_charge(string chargeid)
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            List<Agency> a = new List<Agency>();
            a = ABL.Get_Agency_by_charge(chargeid);

            return Ok(a);
        }

        [HttpGet]
        [Route("api/Get_cast_category")]
        public IActionResult Get_cast_category()
        {
            List<cast_category> c = new List<cast_category>();
            _logger.LogError("test error.");
            AgencyBL cdb = new AgencyBL(_configuration);
            c = cdb.Get_Cast_Category();
            return Ok(c);


        }

        [HttpPut]
        [Route("api/Update_Profile")]
        public IActionResult Update_Profile(string agencyid,[FromBody] Update_Profile_Data agency)
        {
            bool issaved=false;
            AgencyBL cdb = new AgencyBL(_configuration);
            issaved = cdb.Update_Profile(agencyid,agency);
            return Ok(issaved);


        }

        [HttpGet]
        [Route("api/Salutation")]
        public IActionResult Salutation()
        {
            List<SALUTATION> s = new List<SALUTATION>();
            AgencyBL cdb = new AgencyBL(_configuration);
            s = cdb.Get_Salutation();
            return Ok(s);


        }

        [HttpGet]
        [Route("api/Get_user_branches")]
        public IActionResult Get_user_branches(string userid)
        {
            UserBranch s = new UserBranch();
            AgencyBL cdb = new AgencyBL(_configuration);
            s = cdb.Get_User_Branche(userid);
            return Ok(s);


        }


        [HttpGet]
        [Route("api/Branches")]
        public IActionResult Branches(string agencytypeid, string parentid=null)
        {
            PaginationParam filter=new PaginationParam();
            AgencyBL ABL = new AgencyBL(_configuration);
            PagedResult<Agency> AL = new PagedResult<Agency>();
            List<Agency> al = new List<Agency>();
            AL = ABL.Get_Agency(agencytypeid,null,null, filter,null);
            if(parentid != null)
            {
                foreach (Agency ag in AL.Items)
                {
                    if (parentid != null)
                    {
                        if (parentid.ToString().ToUpper() == ag.ParentId.ToString().ToUpper())
                        {
                            al.Add(ag);
                        }
                    }
                    
                }
            }
            else
            {
                al = AL.Items.ToList();
            }

            PagedResult<Agency> FAL = new PagedResult<Agency>();
            FAL.Items = al;
            return Ok(FAL);
        }


        [HttpPost]
        [Route("api/PARTICIPANT_PERSONAL_INFO")]
        public IActionResult PARTICIPANT_PERSONAL_INFO(string agencytypeid, string agencyid, string branchid, string createdby, [FromBody]Agency_PersonalInfo pi)
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            bool issaved = false;
           
            issaved = ABL.Update_Personal_info(agencyid, agencytypeid, branchid, createdby, pi);
            if (issaved == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

           
        }

        [HttpPost]
        [Route("api/AGENCY_ADDRESS_INFO")]
        public IActionResult AGENCY_ADDRESS_INFO(string agencytypeid, string agencyid, string branchid, string createdby, [FromBody] Agency_Participant_AddressInfo ai)
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            bool issaved = false;

            issaved = ABL.Update_Address_info(agencytypeid, agencyid, branchid, createdby, ai);
            if (issaved == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }


        }

        [HttpPost]
        [Route("api/PARTICIPANT_OTHER_INFO")]
        public IActionResult OTHER_INFO(string agencytypeid, string agencyid, string branchid, string createdby, [FromBody] Agency_Participant_OtherInfo oi)
        {

            AgencyBL ABL = new AgencyBL(_configuration);
            bool issaved = false;
            issaved = ABL.Update_Other_info(agencyid, agencytypeid, branchid, createdby, oi);
            if (issaved == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("api/Update_Email")]
        public IActionResult Update_Email(string agencyid, string emailid)
        {

            AgencyBL ABL = new AgencyBL(_configuration);
            bool issaved = false;
            issaved = ABL.Update_Emailid(agencyid, emailid);
            if (issaved == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("api/Update_Mobileno")]
        public IActionResult Update_Mobileno(string agencyid, string mobileno)
        {

            AgencyBL ABL = new AgencyBL(_configuration);
            bool issaved = false;
            issaved = ABL.Update_Mobile_No(agencyid, mobileno);
            if (issaved == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }


    }
}
