using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
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
        [SwaggerOperation("To get Distinct designations in system")]
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
        [Authorize(Policy = "PublicApiKey")]
        [Route("api/Agency")]
        [SwaggerOperation("To get Agencies on basis of agency type")]
        public IActionResult getAgency(string agencytype = null,string filters = null, string agencyid = null, string tat_type_id = null, [FromQuery] PaginationParam param=null,string filter=null)
        {
            
            AgencyBL ABL = new AgencyBL(_configuration);
            PagedResult<Agency> AL = new PagedResult<Agency>();
            AL = ABL.Get_Agency(agencytype, agencyid, tat_type_id, param, filter);



            return Ok(AL);
        }


        [HttpGet]
        [Route("api/courseDirector")]
        [SwaggerOperation("To get Course Directors")]
        public IActionResult courseDirector([FromQuery] PaginationParam param)
        {

            AgencyBL ABL = new AgencyBL(_configuration);
            List<Agency> AL = new List<Agency>();
            AL = ABL.Get_CD_Data();
            var pagedList = Paging.GetPagedList(param, AL);
            var result = Paging.GetPagedData(param, AL);
            return Ok(result);
        }

        // Added 2026-08-13 for the frm_training_creation.aspx -> React
        // migration (Training Master wizard, Step 2 Sponsor / Paid By
        // dropdown). Reuses the same AgencyBL.Get_Agency(agencytype=00053)
        // call as api/Agency above - same underlying data
        // (yuser.proc_yuser_get_agency_vr1) - but WITHOUT that route's
        // [Authorize(Policy = "PublicApiKey")] attribute. Confirmed live
        // this blocked the wizard ("Unable to load: Sponsor / Paid By" -
        // api/Agency was the one call in that pass gated behind
        // PublicApiKey while every sibling dropdown, including the
        // brand-new api/Trg_Payment_Type added alongside it, loaded fine).
        // The PublicApiKey policy is meant for genuinely public/pre-login
        // use (api/Training_Details and friends per
        // session-create-edit-delete-functional-analysis.md's own
        // authentication note) - that same doc records an earlier, similar
        // mistake (reusing a PublicApiKey-gated endpoint from an
        // authenticated flow) being reverted, so the fix here is a new,
        // narrowly-scoped, unauthenticated wrapper rather than removing the
        // attribute from the shared api/Agency route (which likely still
        // needs it for whatever public-facing caller it was added for).
        // Mirrors courseDirector immediately above: a thin, unauthenticated
        // GET scoped to one agency type.
        [HttpGet]
        [Route("api/Trg_Sponsor_Agency")]
        [SwaggerOperation("To get Sponsor / Paid By agencies (agency type 00053) for the Training Master Payment step.")]
        public IActionResult Trg_Sponsor_Agency([FromQuery] PaginationParam param)
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            PagedResult<Agency> AL = ABL.Get_Agency("00053", null, null, param, null);
            return Ok(AL);
        }

        // Added 2026-09-14 for the session-level Content Library page's Faculty panel
        // (frm_content_manager.aspx -> React migration). Old page's real flow:
        // Show_Faculty_Details(facultyid) -> Angular scope's
        // YF_GET_Particular_Faculty_Detail -> GET TrainingAPI/Get_Instructor_Data
        // ?instructorid=X, which calls dm.GET_FACULTY_DATA_NEW(...) ->
        // [yuser].[proc_yuser_get_agency] with @agencytype='00054' (Guest Faculty -
        // a different, narrower agency type than the '00053' Faculty type used
        // elsewhere for session-assignment dropdowns) and @AgencyId=instructorid, then
        // enriches the base row from an XML blob column (name/designation/org/bank/
        // IFSC/account/honorarium + a per-course specialization sub-table).
        // Confirmed this app's own Get_Agency (called below) already runs the modern
        // equivalent proc (yuser.proc_yuser_get_agency_vr1) and already does that same
        // XML-blob enrichment into AgencyAdditionalInfo for every agencytype other than
        // '00053' - so this endpoint is a thin, narrowly-scoped wrapper (matching
        // Trg_Sponsor_Agency's own pattern immediately above) rather than a new
        // hand-rolled query. The one real gap found and closed alongside this endpoint:
        // AgencyAdditionalInfo had no field for the nested <DETAILS> specialization
        // table, so it was being silently dropped for every agency type - see the new
        // `DETAILS`/`FacultyCourseDetail` addition in Models/Agency.cs.
        [HttpGet]
        [Route("api/Get_Instructor_Data")]
        [SwaggerOperation("To get a Guest Faculty's full profile detail (contact/bank/specialization) for the Faculty Details popup.")]
        public IActionResult Get_Instructor_Data(string instructorid)
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            PagedResult<Agency> AL = ABL.Get_Agency("00054", instructorid, null, new PaginationParam(), null);
            Agency agency = AL.Items?.FirstOrDefault();
            if (agency == null)
            {
                return NotFound();
            }
            return Ok(agency);
        }

        [HttpGet]
        [Route("api/Search_Agency")]
        [SwaggerOperation("To search particular agency data ")]
        public IActionResult Search_Agency(string searchtext,string agencytype = null)
        {

            AgencyBL ABL = new AgencyBL(_configuration);
            List<Agency> AL = new List<Agency>();
            AL = ABL.Search_Agency(searchtext);



            return Ok(AL);
        }

        [HttpGet]
        [Route("api/agency_by_charge")]
        [SwaggerOperation("To get all agencies on basis of given charge.")]
        public IActionResult agency_by_charge(string chargeid)
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            List<Agency> a = new List<Agency>();
            a = ABL.Get_Agency_by_charge(chargeid);

            return Ok(a);
        }

        [HttpGet]
        [Route("api/Get_cast_category")]
        [SwaggerOperation("To get cast category.")]
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
        [SwaggerOperation("To update agency profile.")]
        public IActionResult Update_Profile(string agencyid,[FromBody] Update_Profile_Data agency)
        {
            bool issaved=false;
            AgencyBL cdb = new AgencyBL(_configuration);
            issaved = cdb.Update_Profile(agencyid,agency);
            return Ok(issaved);


        }

        [HttpGet]
        [Authorize(Policy = "PublicApiKey")]
        [Route("api/Salutation")]
        [SwaggerOperation("To get agency salutation data.")]
        public IActionResult Salutation()
        {
            List<SALUTATION> s = new List<SALUTATION>();
            AgencyBL cdb = new AgencyBL(_configuration);
            s = cdb.Get_Salutation();
            return Ok(s);


        }

        [HttpGet]
        [Route("api/Get_user_branches")]
        [SwaggerOperation("To get user's branches.")]
        public IActionResult Get_user_branches(string userid)
        {
            UserBranch s = new UserBranch();
            AgencyBL cdb = new AgencyBL(_configuration);
            s = cdb.Get_User_Branche(userid);
            return Ok(s);


        }


        [HttpGet]
        [Authorize(Policy = "PublicApiKey")]
        [Route("api/Branches")]
        [SwaggerOperation("To get branches.")]
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

        // Added for the Administrator/Staff (Employee) migration
        // (/administrator-list, /employee-list). Old app's AdminListDetail /
        // StaffListDetail actions both proxied to the shared api/Agency with a
        // hardcoded &tdds_tat_type_id (125 for Admin, 106 for Staff) appended
        // server-side - but api/Agency here is PublicApiKey-gated, which an
        // authenticated page's own axios calls (Bearer token, no API key) can't
        // satisfy. Mirrors the Trg_Sponsor_Agency / Get_Instructor_Data pattern
        // immediately above in this file: a thin, non-PublicApiKey wrapper
        // scoped to one agencytype, reusing the exact same AgencyBL.Get_Agency
        // call the gated endpoint already makes. Serves both the paginated
        // list grid (tat_type_id required, agencyid omitted) and single-record
        // edit-prefill (agencyid supplied) - same dual role Get_Instructor_Data
        // plays for its own feature.
        [HttpGet]
        [Route("api/Employee_Agency_List")]
        [SwaggerOperation("To get Administrator/Staff (Employee, agencytype 00008) accounts scoped by tat_type_id (125=Administrator, 106=Staff), for the Administrator/Employee list grid and edit-prefill.")]
        public IActionResult Employee_Agency_List([FromQuery] PaginationParam param, string tat_type_id, string agencyid = null, string search = null)
        {
            if (string.IsNullOrWhiteSpace(tat_type_id))
            {
                return BadRequest("tat_type_id is required.");
            }

            try
            {
                AgencyBL ABL = new AgencyBL(_configuration);
                PagedResult<Agency> AL = ABL.Get_Agency(CommonEnum.Agencytype_Staff, agencyid, tat_type_id, param, search);
                return Ok(AL);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // Added for the same migration - the Add New/Edit form's optional
        // Salutation dropdown. api/Salutation above already returns exactly
        // this data but is PublicApiKey-gated (same problem as api/Agency);
        // this is a thin non-gated wrapper over the same AgencyBL.Get_Salutation,
        // matching the api/Employee_Agency_List precedent just above.
        [HttpGet]
        [Route("api/Agency_Salutation")]
        [SwaggerOperation("To get agency salutation data (authenticated, non-PublicApiKey).")]
        public IActionResult Agency_Salutation()
        {
            AgencyBL cdb = new AgencyBL(_configuration);
            List<SALUTATION> s = cdb.Get_Salutation();
            return Ok(s);
        }

        // Added for the same migration - the Add New/Edit form's branch-scope
        // picker (old app: State/RC/SC/Grampanchayat/Village radio group + a
        // matching lookup select). api/Get_Branch_Types lists the available
        // scope types (AgencyDB.BranchTypes(), already used internally by
        // Get_user_branches above but never exposed on its own); api/Get_Branches
        // is a non-PublicApiKey twin of api/Branches above, for the same reason
        // as api/Employee_Agency_List/api/Agency_Salutation.
        [HttpGet]
        [Route("api/Get_Branch_Types")]
        [SwaggerOperation("To get the available branch-scope types (State/RC/SC/Grampanchayat/Village).")]
        public IActionResult Get_Branch_Types()
        {
            AgencyBL ABL = new AgencyBL(_configuration);
            return Ok(ABL.Get_Branch_Types());
        }

        [HttpGet]
        [Route("api/Get_Branches")]
        [SwaggerOperation("To get branches for a branch-scope type (authenticated, non-PublicApiKey twin of api/Branches).")]
        public IActionResult Get_Branches(string agencytypeid, string parentid = null)
        {
            if (string.IsNullOrWhiteSpace(agencytypeid))
            {
                return BadRequest("agencytypeid is required.");
            }

            PaginationParam filter = new PaginationParam();
            AgencyBL ABL = new AgencyBL(_configuration);
            PagedResult<Agency> AL = ABL.Get_Agency(agencytypeid, null, null, filter, null);
            List<Agency> al;
            if (parentid != null)
            {
                al = AL.Items.Where(ag => parentid.ToUpper() == (ag.ParentId ?? "").ToUpper()).ToList();
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
        [SwaggerOperation("To get participant personal information.")]
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
        [SwaggerOperation("To get agency address information.")]
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
        [SwaggerOperation("To get participant's other info.")]
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
        [Authorize(Policy = "PublicApiKey")]
        [HttpGet("api/trainingplan")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult trainingplan()
        {
            CommonDB cdb = new CommonDB(_configuration, null);

            string name = cdb.Get_TP_Data("name");
            var code = cdb.Get_TP_Data("code");
            return Ok(new { c = name, k = code });


        }
        [HttpPost]
        [Route("api/Update_Email")]
        [SwaggerOperation("To update agency email id .")]
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
        [SwaggerOperation("To update agency mobile no.")]
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
