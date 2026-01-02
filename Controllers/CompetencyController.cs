using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text;

namespace LitteraCore.Controllers
{
    [ApiController]
    public class CompetencyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AgencyController> _logger;
        public CompetencyController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/SaveFunction")]
        [SwaggerOperation("To save competency function data.")]
        public IActionResult SaveFunction(DeptFunction f,string createdby)
        {
            _logger.LogError("Post error.");
            string code = "";
            CompetencyBL cbl = new CompetencyBL(_configuration);
            code = cbl.Save_Department_Function(f, createdby);
            return Ok(code);

        }

        [HttpGet]
        [Route("api/GetFunction")]
        [SwaggerOperation("To get competency function data.")]
        public IActionResult GetFunction([FromQuery] PaginationParam filter,int? exactMatch=0,string? searchtext=null)
        {
            CompetencyDB cdb=new CompetencyDB(_configuration);

            var pagedList = cdb.Get_Department_Function(filter, null,searchtext,exactMatch ?? 0);
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
                return Ok(new PagedResult<DeptFunction>
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

        [HttpDelete]
        [Route("api/DeleteFunction")]
        [SwaggerOperation("To delete competency function data.")]
        public IActionResult DeleteFunction(string id)
        {
         
            bool isSaved = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            isSaved = cbl.Delete_Dept_Function(id);
            return Ok(isSaved);

        }

        [HttpGet]
        [Route("api/GetFunctionById")]
        [SwaggerOperation("To get particular function detail.")]
        public IActionResult GetFunctionById(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_Department_Function(null,id);
            return Ok(new PagedResult<DeptFunction>
            {
                Items = pagedList,
                TotalRecords = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                CurrentPage = pagedList.CurrentPage
            });
        }


        [HttpPost]
        [Route("api/SaveJobPosition")]
        [SwaggerOperation("To save competency job position data.")]
        public IActionResult SaveJobPosition(JobPosition f, string createdby)
        {
            _logger.LogError("Post error.");
            string code = "";
            CompetencyBL cbl = new CompetencyBL(_configuration);
            code = cbl.Save_Job_Position(f, createdby);
            return Ok(code);

        }
        [HttpDelete]
        [Route("api/DeleteJobPosition")]
        [SwaggerOperation("To delete competency function data.")]
        public IActionResult DeleteJobPosition(string id)
        {

            bool isSaved = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            isSaved = cbl.Delete_Job_Position(id);
            return Ok(isSaved);

        }

        [HttpGet]
        [Route("api/GetJobPosition")]
        [SwaggerOperation("To get job position data.")]
        public IActionResult GetJobPosition([FromQuery] PaginationParam filter, int? exactMatch = 0, string? searchtext = null)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_JOB_POSITIONS(filter, null, searchtext, exactMatch ?? 0);
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
                return Ok(new PagedResult<JobPosition>
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
        [Route("api/GetJobPositionById")]
        [SwaggerOperation("To get particulatr competency job position.")]
        public IActionResult GetJobPositionById(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_JOB_POSITIONS(null, id);
            return Ok(new PagedResult<JobPosition>
            {
                Items = pagedList,
                TotalRecords = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                CurrentPage = pagedList.CurrentPage
            });
        }

        [HttpPost]
        [Route("api/SaveActivity")]
        [SwaggerOperation("To save competency activity data.")]
        public IActionResult SaveActivity(Activity f, string createdby)
        {
            _logger.LogError("Post error.");
            string code = "";
            CompetencyBL cbl = new CompetencyBL(_configuration);
            code = cbl.Save_Activity(f, createdby);
            return Ok(code);

        }

        [HttpDelete]
        [Route("api/DeleteActivity")]
        [SwaggerOperation("To delete competency activity data.")]
        public IActionResult DeleteActivity(string id)
        {

            bool isSaved = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            isSaved = cbl.Delete_Activity(id);
            return Ok(isSaved);

        }

        [HttpGet]
        [Route("api/GetActivity")]
        [SwaggerOperation("To get competency activity data.")]
        public IActionResult GetActivity([FromQuery] PaginationParam filter, int? exactMatch = 0, string? searchtext = null)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_Activity(filter, null, searchtext, exactMatch ?? 0);
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
                return Ok(new PagedResult<Activity>
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
        [Route("api/GetActivityById")]
        [SwaggerOperation("To get particular competency activity data.")]
        public IActionResult GetActivityById(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_Activity(null, id);
            return Ok(new PagedResult<Activity>
            {
                Items = pagedList,
                TotalRecords = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                CurrentPage = pagedList.CurrentPage
            });
        }

        [HttpGet]
        [Route("api/GetPositionLevels")]
        [SwaggerOperation("To get positions level master data.")]
        public IActionResult GetPositionLevels([FromQuery] PaginationParam filter)
        {
            CompetencyBL cdb = new CompetencyBL(_configuration);

            var pagedList = cdb.Get_Position_Lelvels(filter);
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
                return Ok(new PagedResult<position_levels>
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


        [HttpPost]
        [Route("api/SaveJobRole")]
        [SwaggerOperation("To save competency job role data.")]
        public IActionResult SaveJobRole(JobRole R, string createdby)
        {
            _logger.LogError("Post error.");
            string code = "";
            CompetencyBL cbl = new CompetencyBL(_configuration);
            code = cbl.Save_Job_Role(R, createdby);
            return Ok(code);

        }


        [HttpGet]
        [Route("api/GetJobRole")]
        [SwaggerOperation("To get competency job role data.")]
        public IActionResult GetJobRole([FromQuery] PaginationParam filter, int? exactMatch = 0, string? searchtext = null)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_JOB_ROLE(filter, null, searchtext, exactMatch ?? 0);
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
                return Ok(new PagedResult<JobRole>
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
        [Route("api/GetJobRoleById")]
        [SwaggerOperation("To get competency job role by id.")]
        public IActionResult GetJobRoleById(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);

            var pagedList = cdb.Get_JOB_ROLE(null, id);
            return Ok(new PagedResult<JobRole>
            {
                Items = pagedList,
                TotalRecords = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                CurrentPage = pagedList.CurrentPage
            });
        }

        [HttpPost]
        [Route("api/Save_Fracking_EMP")]
        [SwaggerOperation("To save fracking employee data.")]
        public IActionResult Save_Fracking_EMP(FracEmp E)
        {
            _logger.LogError("Post error.");
            bool issaved = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            issaved = cbl.Save_Emp_Details(E);
            return Ok(issaved);

        }
        [HttpPost]
        [Route("api/Save_Frack_Emp_Activity")]
        [SwaggerOperation("To save fracking employee data.")]
        public IActionResult Save_Frack_Emp_Activity(FracEmpActivity A)
        {
            _logger.LogError("Post error.");
            bool issaved = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            issaved = cbl.Save_Emp_Activities(A);
            return Ok(issaved);

        }
        [HttpPost]
        [Route("api/Save_Frack_Emp_Activity_Resources")]
        [SwaggerOperation("To save frack employee activity resource.")]
        public IActionResult Save_Frack_Emp_Activity_Resources(FracEmpResources R)
        {
            _logger.LogError("Post error.");
            bool issaved = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            issaved = cbl.Save_Emp_Activities_Resources(R);
            return Ok(issaved);

        }

        [HttpGet]
        [Route("api/Get_Frac_Emp_Activity")]
        [SwaggerOperation("To get fracking employee activity data.")]
        public IActionResult Get_Frac_Emp_Activity(string employeeid, string branchid)
        {
            _logger.LogError("Post error.");
            List<Empactivity> act = new List<Empactivity>();
            CompetencyBL cbl = new CompetencyBL(_configuration);
            act = cbl.Get_Emp_activity(employeeid, branchid);
            return Ok(act);

        }
        [HttpGet]
        [Route("api/Check_Frack_Emp_Exist")]
        [SwaggerOperation("To check eisting employee.")]
        public IActionResult Check_Frack_Emp_Exist(string mobileno, string branchid)
        {
            bool isexist = false;
            CompetencyBL cbl = new CompetencyBL(_configuration);
            isexist = cbl.Check_Frack_Emp_Exist(mobileno);
            return Ok(isexist);

        }



        [HttpGet]
        [Route("api/GetPosts")]
        [SwaggerOperation("To get posts master data.")]
        public IActionResult GetPosts(string? name)
        {
            string[] posts;
            CompetencyDB cdb = new CompetencyDB(_configuration);
            posts=cdb.Get_Post_Data(name); 
            return Ok(posts);

        }

        [HttpGet]
        [Route("api/Get_Fracing_Report")]
        [SwaggerOperation("To get fracing data.")]
        public IActionResult Get_Fracing_Report()
        {
            List<EMP_FRACK_REPORT> r = new List<EMP_FRACK_REPORT>();
            CompetencyDB cdb = new CompetencyDB(_configuration);
            r = cdb.Get_Fracking_Report();
            //StringBuilder sb = new StringBuilder();
            //sb.Append("<table>");
            //sb.Append("<tr>");
            //sb.Append("<td>");
            //sb.Append("Post");
            //sb.Append("</td>");
            //sb.Append("<td>");
            //sb.Append("Activity");
            //sb.Append("</td>");
            //sb.Append("<td>");
            //sb.Append("Resources");
            //sb.Append("</td>");
            //sb.Append("<td>");
            //sb.Append("Roles");
            //sb.Append("</td>");
            //sb.Append("<td>");
            //sb.Append("Competency");
            //sb.Append("</td>");
            //sb.Append("</tr>");
            //sb.Append("</table>");

            //Response.Clear();
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spredsheetml.sheet";
            ////Response.ContentType = "application/ms-excel";
            //Response.AddHeader("content-disposition", "attachment;filename=CompetencyData.xls");
            //Response.Write(sb.ToString());
            //Response.End();
            return Ok(r);

        }

    }

}
