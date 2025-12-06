using Azure.Core;
using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static LitteraCore.Common.CommonEnum;

namespace LitteraCore.Controllers
{
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public FeedbackController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("api/TrainingFeedback")]
        public IActionResult TrainingFeedback(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {
           
            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate);
            TrainingDB usertrg = new TrainingDB(_configuration);

            //***************
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            //**********
            userwise_lwtc = usertrg.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();

            List<TrainingFeedback> TF = new List<TrainingFeedback>();
            TrainingFeedbackBL TBL = new TrainingFeedbackBL(_configuration);
            TF = TBL.Get_Share_Feedback(userid);
            TF = TF.Where(o => o.status == 1 || o.status == 9).ToList();

            List<TrainingFeedback> TFF = new List<TrainingFeedback>();
            foreach (TrainingFeedback F in TF)
            {

                List<Training> ft = new List<Training>();
                ft = lwtc.Where(o => o.TrainingId.ToString().ToUpper() == F.trainingid.ToString().ToUpper()).ToList();
                if (ft.Count > 0)
                {
                    TrainingFeedback TFFI = new TrainingFeedback();
                    TFFI.trainingid = F.trainingid;
                    TFFI.trainingcode = ft.FirstOrDefault().Trainingcode;
                    TFFI.trainingtitle = ft.FirstOrDefault().T_Name;
                    TFFI.sureveyid = F.sureveyid;
                    TFFI.groupid = F.groupid;
                    TFFI.feedbacktitle = F.feedbacktitle;
                    TFFI.sharefeedbackid = F.sharefeedbackid;
                    TFFI.tssr_responsdant_mobile = F.tssr_responsdant_mobile;
                    TFFI.status = F.status;
                    TFFI.Status_txt = F.Status_txt;
                    TFF.Add(TFFI);
                }

            }



            //****************Filter Data because need to show feedback only approve participant
            ParticipantDB PDB = new ParticipantDB(_configuration);
            List<Participant> P = new List<Participant>();
            P = PDB.Get_Participant_training_status(userid);
            P = P.Where(o => o.is_approve != 1).ToList();

            List<TrainingFeedback> feebdacklist = new List<TrainingFeedback>();
            foreach (TrainingFeedback F in TFF)
            {
                List<Participant> FP = new List<Participant>();
                FP = P.Where(o => o.TrainingId.ToString().ToUpper() == F.trainingid.ToString().ToUpper()).ToList();
                if (FP.Count == 0)
                {
                    feebdacklist.Add(F);
                }
            }

            TFF = feebdacklist;







            //if (filters != null && filters.Trim() != "")
            //{
            //    string[] sptfilter = filters.Split(";".ToCharArray());
            //    foreach (string s in sptfilter)
            //    {
            //        if (s != "")
            //        {
            //            string[] sptfield = s.Split(":".ToCharArray());
            //            string nextop = null;
            //            if (sptfield[3] != "")
            //            {
            //                nextop = sptfield[3];
            //            }
            //            Search sd = new Search
            //            {
            //                Column = sptfield[0],
            //                SearchValue = sptfield[1],
            //                SearhOperator = sptfield[2],
            //                NextSearchOperator = nextop
            //            };
            //            searchdata.Add(sd);
            //        }

            //    }

            //}

            //if (searchdata.Count > 0)
            //{
            //    TFF = FilterData.Filter(TFF, searchdata);
            //}


            //********************



            var searchService = new SearchService();
            // Filter items based on the search criteria
            var filteredItems = TFF;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(TFF, searchCriterias.SearchCriteria.ToList());
            }



            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result= Paging.GetPagedData(param, filteredItems);
            return Ok(result);
            //if (pagedList.Count() > 0)
            //{
            //    var metadata = new
            //    {
            //        pagedList.TotalCount,
            //        pagedList.PageSize,
            //        pagedList.CurrentPage,
            //        pagedList.TotalPages,
            //        pagedList.HasNext,
            //        pagedList.HasPrevious,


            //    };
            //    return Ok(new PagedResult<DeptFunction>
            //    {
            //        Items = pagedList,
            //        TotalRecords = pagedList.TotalCount,
            //        PageSize = pagedList.PageSize,
            //        TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
            //        CurrentPage = pagedList.CurrentPage
            //    });

            //    // return Ok(pagedList);
            //}
            //else
            //    return NotFound();

            // if (param != null)
            // {
            //     if (param.PageSize > 0)
            //     {

            //         return Ok(PagedList<TrainingFeedback>.ToPagedList(TFF.ToList(),
            //param.PageNumber,
            //param.PageSize));
            //     }
            //     else
            //     {
            //         return Ok(PagedList<TrainingFeedback>.ToPagedList(TFF.ToList(),
            //        1,
            //        TFF.Count()));
            //     }

            // }
            // else
            // {
            //     return Ok(PagedList<TrainingFeedback>.ToPagedList(TFF.ToList(),
            //        1,
            //        TFF.Count()));
            // }




        }


        [HttpGet]
        [Route("api/Check_Feedback_Status")]
        public IActionResult Check_Feedback_Status(string participantid, string sharefeedbackid)
        {
            TrainingFeedbackBL TFB = new TrainingFeedbackBL(_configuration);
            int feedbackstatus = TFB.Feedback_Participant_Status(participantid, sharefeedbackid);
            return Ok(feedbackstatus);
            




        }



        [HttpPost]
        [Route("api/TESTT")]
        public IActionResult TESTT([FromBody] SearchParam? searchCriterias)
        {

            var searchService = searchCriterias;

            return Ok(true);
        }




        [HttpPost]
        [Route("api/Feedback360_Summery")]
        public IActionResult Feedback360_Summery(string? surveyid = null, [FromQuery] PaginationParam? param=null, [FromBody] SearchParam? searchCriterias=null, string trainingid=null)
        {



            List<FeedbackReportSummery> FRS = new List<FeedbackReportSummery>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            FRS = FBL.Get_Feedback_360_Summery(surveyid,trainingid);
          



            //List<Search> searchdata = new List<Search>();
            //if (filters != null && filters.Trim() != "")
            //{
            //    string[] sptfilter = filters.Split(";".ToCharArray());
            //    foreach (string s in sptfilter)
            //    {
            //        if (s != "")
            //        {
            //            string[] sptfield = s.Split(":".ToCharArray());
            //            string nextop = null;
            //            if (sptfield[3] != "")
            //            {
            //                nextop = sptfield[3];
            //            }
            //            Search sd = new Search
            //            {
            //                Column = sptfield[0],
            //                SearchValue = sptfield[1],
            //                SearhOperator = sptfield[2],
            //                NextSearchOperator = nextop
            //            };
            //            searchdata.Add(sd);
            //        }

            //    }

            //}

            var searchService = new SearchService();
            // Filter items based on the search criteria
            var filteredItems = FRS;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(FRS, searchCriterias.SearchCriteria.ToList());
            }

            //if (searchdata.Count > 0)
            //{
            //    FRS = FilterData.Filter(FRS, searchdata);
            //}


            //IPagedList<FeedbackReportSummery> vwtc = null;
            //if (pageno != 0)
            //{
            //    vwtc = FRS.ToPagedList(pageno, pagesize);
            //}
            //else
            //{
            //    if (FRS.ToList().Count() == 0)
            //    {
            //        vwtc = FRS.ToPagedList(1, 1);
            //    }
            //    else
            //    {
            //        vwtc = FRS.ToPagedList(1, FRS.ToList().Count());
            //    }

            //}





            //Paging<FeedbackReportSummery> pl = new Paging<FeedbackReportSummery>();
            //pl.Count = vwtc.Count;
            //pl.FirstItemOnPage = vwtc.FirstItemOnPage;
            //pl.HasNextPage = vwtc.HasNextPage;
            //pl.HasPreviousPage = vwtc.HasPreviousPage;
            //pl.IsFirstPage = vwtc.IsFirstPage;
            //pl.IsLastPage = vwtc.IsLastPage;
            //pl.LastItemOnPage = vwtc.LastItemOnPage;
            //pl.PageCount = vwtc.PageCount;
            //pl.pagedata = vwtc;
            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result = Paging.GetPagedData(param, filteredItems);

            return Ok(result);


        }

        [HttpPost]
        [Route("api/Feedback360_Survey_Result")]
        public IActionResult Feedback360_Survey_Result(string surveyid, string? responsee_mobileno = null, string? responsee_emailid = null)
        {



            SurveyResponseResult FRS = new SurveyResponseResult();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            FRS = FBL.Get_Feedback_360_Survey_Result_Structured(surveyid, responsee_mobileno, responsee_emailid);



            return Ok(FRS);


        }

        [HttpGet]
        [Route("api/Feedback360_Survey_Result_MCQ_Summery_Questionwise")]
        public IActionResult Feedback360_Survey_Result_MCQ_Summery_Questionwise(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_MCQ_Result ORR = new Question_MCQ_Result();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_MCQ_Result_Summary(surveyid, groupid, sharefeedbackid, responsee_mobileno, responsee_emailid);


            return Ok(ORR);


        }

        [HttpGet]
        [Route("api/Feedback360_Survey_MCQ_Detail_Questionwise")]
        public IActionResult Feedback360_Survey_MCQ_Detail_Questionwise(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_MCQ_QUESTIONWISE_DETAIL ORR = new Question_MCQ_QUESTIONWISE_DETAIL();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_MCQ_DETAIL_QUESTIONWISE(surveyid, groupid, sharefeedbackid, responsee_mobileno, responsee_emailid);



            return Ok(ORR);


        }

        [HttpGet]
        [Route("api/Feedback360_Survey_Result_DESC_Summery_Questionwise")]
        public IActionResult Feedback360_Survey_Result_DESC_Summery_Questionwise(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_MCQ_QUESTIONWISE_DETAIL ORR = new Question_MCQ_QUESTIONWISE_DETAIL();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_MCQ_DETAIL_QUESTIONWISE(surveyid, groupid, sharefeedbackid, responsee_mobileno, responsee_emailid);



            return Ok(ORR);


        }

        [HttpGet]
        [Route("api/Feedback360_Survey_DESC_Detail_Questionwise")]
        public IActionResult Feedback360_Survey_DESC_Detail_Questionwise(string APIKEY, string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_DESC_QUESTIONWISE_DETAIL ORR = new Question_DESC_QUESTIONWISE_DETAIL();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_DESC_DETAIL_QUESTIONWISE(surveyid, groupid, sharefeedbackid, responsee_mobileno, responsee_emailid);



            return Ok(ORR);


        }
        [HttpGet]
        [Route("api/Feedback360_Survey_Result_Rating_Summery_Questionwise")]
        public IActionResult Feedback360_Survey_Result_Rating_Summery_Questionwise(string APIKEY, string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_Rating_Result ORR = new Question_Rating_Result();
            //List<Question_Rating_Result_Summary> FRS = new List<Question_Rating_Result_Summary>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_Rating_Result_Summary(surveyid, groupid, sharefeedbackid, responsee_mobileno, responsee_emailid);



            return Ok(ORR);


        }

        [HttpPost]
        [Route("api/Feedback360_trainingid_Summery")]
        public IActionResult Feedback360_trainingid_Summery(string fromdate, string todate,string? trainigid = null, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias = null)
        {

            List<FeedbackReportSummery_trainingwise> FRS = new List<FeedbackReportSummery_trainingwise>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            FRS = FBL.Get_Feedback_360_Summery_trainingwise(fromdate,todate,trainigid);



            var searchService = new SearchService();
            // Filter items based on the search criteria
            var filteredItems = FRS;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(FRS, searchCriterias.SearchCriteria.ToList());
            }

            
            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result = Paging.GetPagedData(param, filteredItems);

            return Ok(result);


        }



        [HttpPost]
        [Route("api/Training_Wise_Feedback_Summary")]
        public IActionResult Training_Wise_Feedback_Summary(string fromdate, string todate, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias = null, string? trainingid = null)
        {



            List<FeedbackReportSummery_trainingwise> FRS = new List<FeedbackReportSummery_trainingwise>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            FRS = FBL.Get_Feedback_360_Summery_trainingwise(fromdate, todate,trainingid);

            var searchService = new SearchService();
            // Filter items based on the search criteria
            var filteredItems = FRS;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(FRS, searchCriterias.SearchCriteria.ToList());
            }

         

            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result = Paging.GetPagedData(param, filteredItems);

            return Ok(result);


        }


        [HttpPost]
        [Route("api/Training_Questionnaire_wise_Feedback_Summary")]
        public IActionResult Training_Questionnaire_wise_Feedback_Summary(string fromdate, string todate, [FromQuery] PaginationParam? param = null, [FromBody] SearchParam? searchCriterias = null, string? trainingid = null)
        {

            List<FeedbackReportSummery_trainingwise> FRS = new List<FeedbackReportSummery_trainingwise>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            FRS = FBL.Get_Feedback_360_Summery_Groupwise(fromdate, todate,trainingid);

            var searchService = new SearchService();
            // Filter items based on the search criteria
            var filteredItems = FRS;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(FRS, searchCriterias.SearchCriteria.ToList());
            }



            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result = Paging.GetPagedData(param, filteredItems);

            return Ok(result);


        }


        [HttpPost]
        [Route("api/Feedback_Summery_Questionwise")]
        public IActionResult Feedback_Summery_Questionwise(string groupid,string trainingid, string? sharefeedbackid = null, string? responsee_mobileno = null, string? responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_Rating_Result ORR = new Question_Rating_Result();
            //List<Question_Rating_Result_Summary> FRS = new List<Question_Rating_Result_Summary>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_Rating_Result_Summary_New(groupid,trainingid, sharefeedbackid, responsee_mobileno, responsee_emailid);



            return Ok(ORR);


        }



        [HttpPost]
        [Route("api/Feedback_Summery_Questionwise_MCQ")]
        public IActionResult Feedback_Summery_Questionwise_MCQ(string groupid, string trainingid, string? sharefeedbackid = null, string? responsee_mobileno = null, string? responsee_emailid = null)
        {
            //Handle Null
            if (responsee_mobileno == "")
            {
                responsee_mobileno = null;
            }
            if (responsee_emailid == "")
            {
                responsee_emailid = null;
            }

            Question_MCQ_Result ORR = new Question_MCQ_Result();
            //List<Question_Rating_Result_Summary> FRS = new List<Question_Rating_Result_Summary>();
            FeedbackBL FBL = new FeedbackBL(_configuration);
            ORR = FBL.Get_MCQ_Result_Summary_New(groupid, trainingid, sharefeedbackid, responsee_mobileno, responsee_emailid);



            return Ok(ORR);


        }
    }
}
