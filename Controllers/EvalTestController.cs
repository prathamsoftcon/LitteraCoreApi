using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using LitteraCore.Models;
using LitteraCore.Common.DMS;
using Swashbuckle.AspNetCore.Annotations;

namespace LitteraCore.Controllers
{
    public class EvalTestController : Controller
    {
        private readonly ILogger<ApplicationConfigController> _logger;
        private readonly IConfiguration _configuration;

        public EvalTestController(IConfiguration configuration, ILogger<ApplicationConfigController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [Route("api/Get_Self_Test_Configuration")]
        [HttpGet]
        [SwaggerOperation("To get self test configuration from Mock_Test_Configuration.")]
        public IActionResult Get_Self_Test_Configuration(string trainingid=null)
        {
            //At present this data is hardcode in modal need to change by config file
            CompetencyConfiguration c = new CompetencyConfiguration();
            EvalBL ebl=new EvalBL(_configuration);
            c = ebl.GET_SELF_TEST_CONFIGURATION(trainingid);
            return Ok(c);
        }

        [HttpGet]
        [Route("api/Check_Test_Eligibility")]
        [SwaggerOperation("To check participant test eligibility.")]
        public IActionResult Check_Test_Eligibility(string userid, string trainingid, string testid = null)
        {
            int isTestAllowed = 0;
            ParticipantDB WDB = new ParticipantDB(_configuration);
            List<Participant> lwtc = new List<Participant>();
            lwtc = WDB.Get_TRG_PARTICIPANT_Data(trainingid, userid, null, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            lwtc = lwtc.Where(o => o.ParticipantId.ToString().ToUpper() == userid.ToString().ToUpper()).ToList();
            if (lwtc.Count > 0)
            {
                if (lwtc.FirstOrDefault().is_approve == 1)
                {
                    isTestAllowed = 1;
                }
                else
                {
                    isTestAllowed = 0;
                }

            }
            else
            {
                isTestAllowed = 0;
            }


            return Ok(isTestAllowed);
        }



        [HttpGet]
        [Route("api/Selt_Test_Analytics")]
        [SwaggerOperation("To get self test analytics.")]
        public IActionResult Selt_Test_Analytics(string usertype, string userid, int GroupBy, string categoryid = null, string testid = null)
        {
            CompetencyBL BL = new CompetencyBL(_configuration);
            List<CompentencyLevel> T = new List<CompentencyLevel>();
            T = BL.Get_Test_Result_Data(userid, usertype, GroupBy, categoryid, testid);


            return Ok(T);
        }

        [HttpGet]
        [Route("api/Self_test_Report_Data")]
        [SwaggerOperation("To get self test report data.")]
        public IActionResult Self_test_Report_Data(string usertype, string userid)
        {
            CompetencyBL BL = new CompetencyBL(_configuration);
            List<Competency> T = new List<Competency>();
            T = BL.Get_Test_Report_Data(userid, usertype);

            return Ok(T);
        }
        [HttpGet]
        [Route("api/TRAINING_TEST_ANALYTIC_DATA")]
        [SwaggerOperation("To get training test alanytic data.[groupOn=1 for training, groupOn=2 for session,groupOn=3 for Participant,groupOn=4 for test ]")]
        public IActionResult TRAINING_TEST_ANALYTIC_DATA(string usertype, string userid, string fromdate, string todate, string trainingid = null, string sessionid = null, string participantid = null, int groupOn = 1, int testtype = 1, string testid = null,string branchid=null)
        {
            //groupOn=1 for training, groupOn=2 for session,groupOn=3 for Participant,groupOn=4 for test 
            EvalBL BL = new EvalBL(_configuration);
            List<TEST_RESULT_DATA> T = new List<TEST_RESULT_DATA>();
            T = BL.GET_TRAINING_TEST_ANALYTIC_DATA(usertype, userid, fromdate, todate, trainingid, testtype, branchid);
            if (trainingid != null)
            {
                T = T.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();
            }
            if (sessionid != null)
            {
                T = T.Where(o => o.sessionid.ToString().ToUpper() == sessionid.ToString().ToUpper()).ToList();
            }
            if (participantid != null)
            {
                T = T.Where(o => o.participantid.ToString().ToUpper() == participantid.ToString().ToUpper()).ToList();
            }
            if (testid != null)
            {
                T = T.Where(o => o.testid.ToString().ToUpper() == testid.ToString().ToUpper()).ToList();
            }
            List<ResultPercentage> result = new List<ResultPercentage>();
            if (groupOn == 1)
            {
                result = GroupAndCalculateAverage(T, "trainingid", "mark_percentage");
                //Code to get Name
                foreach (ResultPercentage R in result)
                {
                    R.name = T.Where(o => o.trainingid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().training_name?.ToString() + " - " + T.Where(o => o.trainingid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().training_code?.ToString();
                    R.GroupOn = 1;

                }

            }
            else if (groupOn == 2)
            {
                result = GroupAndCalculateAverage(T, "sessionid", "mark_percentage");
                foreach (ResultPercentage R in result)
                {
                    R.name = T.Where(o => o.sessionid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().session_subject;
                    R.GroupOn = 2;
                }
            }
            else if (groupOn == 3)
            {
                result = GroupAndCalculateAverage(T, "participantid", "mark_percentage");
                foreach (ResultPercentage R in result)
                {
                    R.name = T.Where(o => o.participantid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().participant_name;
                    R.GroupOn = 3;
                }
            }
            else if (groupOn == 4)
            {
                result = GroupAndCalculateAverage(T, "testid", "mark_percentage");
                foreach (ResultPercentage R in result)
                {
                    R.name = T.Where(o => o.testid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().testname;
                    R.GroupOn = 4;
                    R.testQuestionid = T.Where(o => o.testid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().TestQuestionid;
                    R.testparticipantid = T.Where(o => o.testid.ToString().ToUpper() == R.key.ToString().ToUpper()).FirstOrDefault().TestParticipantid;
                }
            }


            return Ok(result);
        }

        public static List<ResultPercentage> GroupAndCalculateAverage<T>(List<T> items, string groupColumn, string averageColumn)
        {
            // Group the items by the groupColumn
            var groupedItems = items.GroupBy(
                item => typeof(T).GetProperty(groupColumn).GetValue(item).ToString(),
                (key, group) => new
                {
                    GroupKey = key,
                    AverageValue = group.Average(item => Convert.ToDouble(typeof(T).GetProperty(averageColumn).GetValue(item))),
                    //AdditionalColumnValues = group.Select(item => Convert.ToString(typeof(T).GetProperty(columnname).GetValue(item))).Where(o=>o.,
                });

            // Create a dictionary to store the group key and its average value
            var result = new Dictionary<string, double>();
            List<ResultPercentage> RL = new List<ResultPercentage>();
            // Populate the dictionary
            foreach (var item in groupedItems)
            {
                ResultPercentage R = new ResultPercentage();
                R.key = item.GroupKey;
                // R.name = item.AdditionalColumnValues;
                R.percentage = item.AverageValue;
                result[item.GroupKey] = item.AverageValue;
                RL.Add(R);
            }

            return RL;
        }


        [HttpPost]
        [Route("api/get_user_tests")]
        [SwaggerOperation("To get user's test.")]
        public IActionResult get_user_tests(string usertype, string userid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias, string testtype = "1", string trainingid = null)
        {
           List<Test> TESTS = new List<Test>();
           EvalDB tbl = new EvalDB(_configuration);
           TESTS = tbl.Get_test_List(usertype, userid, testtype);
            if (trainingid != null)
            {
                TESTS = TESTS.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();
            }
            var orderedTests = TESTS
     .OrderBy(t => t.type == "1" ? 0 : t.type == "2" ? 1 : 2) // custom type order
     .ThenByDescending(t => t.createdon)                       // order within type by createdon desc
     .ToList();

            var searchService = new SearchService();
            var filteredItems = orderedTests;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(orderedTests, searchCriterias.SearchCriteria.ToList());
            }
            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result = Paging.GetPagedData(param, filteredItems);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/get_user_test_by_testid")]
        [SwaggerOperation("To get a user's test by its mapped test ID.")]
        public IActionResult get_user_test_by_testid(string testid, string usertype, string userid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {
            if (!Guid.TryParse(testid, out _) || !Guid.TryParse(userid, out _))
            {
                return BadRequest("testid and userid must be valid GUIDs.");
            }

            EvalDB tbl = new EvalDB(_configuration);
            List<Test> tests = tbl.Get_test_List_by_testid(testid, usertype, userid);
            var orderedTests = tests
                .OrderBy(t => t.type == "1" ? 0 : t.type == "2" ? 1 : 2)
                .ThenByDescending(t => t.createdon)
                .ToList();

            var filteredItems = orderedTests;
            if (searchCriterias?.SearchCriteria != null)
            {
                var searchService = new SearchService();
                filteredItems = searchService.FilterItems(orderedTests, searchCriterias.SearchCriteria.ToList());
            }

            return Ok(Paging.GetPagedData(param, filteredItems));
        }

        [Route("api/get_test_participant_id")]
        [HttpGet]
        [SwaggerOperation("To get particular participant testparticipant id .")]
        public IActionResult get_test_participant_id(string testquestionid, string userid)
        {
            //At present this data is hardcode in modal need to change by config file
            CompetencyConfiguration c = new CompetencyConfiguration();
            EvalDB ebl = new EvalDB(_configuration);
            string id = ebl.Get_test_Participantid(testquestionid, userid);
            return Ok(id);
        }


        [Route("api/get_user_session_test_details")]
        [HttpGet]
        [SwaggerOperation("To get session detail from test.")]
        public IActionResult get_user_session_test_details(string userid, string testid)
        {

            UserDB udb = new UserDB(_configuration);
            User_Agency_Detail uad =new User_Agency_Detail();
            uad = udb.Get_User_Detail_by_userid(userid);

            AgencyBL abl=new AgencyBL(_configuration);
            UserBranch ub =new UserBranch();
            ub = abl.Get_User_Branche(userid);

            EvalDB tbd = new EvalDB(_configuration);
            List<user_session_test> ss = tbd.Get_Test_Detail(testid);


            ParticipantDB PDB = new ParticipantDB(_configuration);
            //List of training all participant
            List<Participant> PL = new List<Participant>();
            // PL = PDB.Get_TRG_PARTICIPANT_Data(s.trainingid, uad.agencyid,null);




            List<Test> TESTS = new List<Test>();
            EvalDB tbl = new EvalDB(_configuration);
            TESTS = tbl.Get_test_List("1", userid);

            user_session_test ust = new user_session_test
            {
                agencyid = uad.agencyid,
                branchid = ub.branches.FirstOrDefault().branchid,
               trainingcategoryid = ss.FirstOrDefault().trainingcategoryid,
               skilltags = ss.FirstOrDefault().skilltags,
                usertype = uad.usertype,
                test= TESTS.Where(o=>o.testid.ToString().ToUpper() == testid.ToString().ToUpper()).FirstOrDefault()
                //ttpai_id= PL.FirstOrDefault().ttpai_id,
                // trainingid= s.trainingid
            };


            //At present this data is hardcode in modal need to change by config file
            //CompetencyConfiguration c = new CompetencyConfiguration();
            //EvalDB ebl = new EvalDB(_configuration);
            //string id = ebl.Get_test_Participantid(testquestionid, userid);
            return Ok(ust);
        }


        [Route("api/Get_Participant_test_Result")]
        [HttpGet]
        [SwaggerOperation("To get particular test participant result.")]
        public IActionResult Get_Participant_test_Result(string testquestionid, string participantid = null, int pageno = 1, int pagesize = 0, string searchcolumn = null, string searchvalue = null)
        {
            //At present this data is hardcode in modal need to change by config file
            EvalBL ebl=new EvalBL(_configuration);
            List<participant_test_result> ptr = new List<participant_test_result>();

            ptr = ebl.Get_Participant_Test_Result(testquestionid,participantid,pageno,pagesize,searchcolumn,searchvalue);

            var searchService = new SearchService();
            if (searchvalue != null)
            {
                SearchParam searchparam = new SearchParam();
                List<SearchCriteria> searchcriteria = new List<SearchCriteria>();
                searchcriteria.Add(new SearchCriteria { Column = searchcolumn, Value = searchvalue, Condition = "Like", NextOperator = "OR" });
                searchparam.SearchCriteria = searchcriteria.ToArray();

                ptr = searchService.FilterItems(ptr, searchparam.SearchCriteria.ToList());
            }



            PaginationParam param = new PaginationParam { PageNumber = 1, PageSize = pagesize };
            var result = Paging.GetPagedData(param, ptr);
            if (ptr.Count > 0)
            {
                result.TotalRecords = ptr.FirstOrDefault().totalrecored;
                result.TotalPages = (int)Math.Ceiling((double)ptr.FirstOrDefault().totalrecored / param.PageSize);
            }
            return Ok(result);

        }

        [Route("api/check_test_in_use")]
        [HttpGet]
        [SwaggerOperation("To check particular test is in use before any change in test details.")]
        public IActionResult check_test_in_use(string testid)
        {

            bool is_used = false;
            EvalBL ebl = new EvalBL(_configuration);
            is_used = ebl.check_test_in_use(testid);



            return Ok(new { is_used = is_used });
        }

        [Route("api/Update_Test_Status")]
        [HttpPost]
        [SwaggerOperation("To update particular test status.")]
        public IActionResult Update_Test_Status([FromBody] DMS d)
        {

            bool is_used = false;
            EvalBL ebl = new EvalBL(_configuration);
            is_used = ebl.update_test_status(d);



            return Ok(is_used);
        }


       
    }

}

