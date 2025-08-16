using LitteraCore.Common.DMS;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using static LitteraCore.Common.CommonEnum;
using System.Text;
using Microsoft.Extensions.Primitives;
using LitteraCore.BLContext;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using LitteraCore.Common.Token;
using System.Collections.Generic;

namespace LitteraCore.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/Dashboard_analytics")]
        public IActionResult Dashboard_analytics(string usertype, string userid, DateTime startdate, DateTime enddate,string? banchid=null)
        {
            DBAnalytics d = new DBAnalytics();
            if (usertype == "5")
            {
                TrainingDB WDB = new TrainingDB(_configuration);

                List<Training> lwtc = new List<Training>();
                List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

                lwtc = WDB.Get_VW_Training_calendar(startdate, enddate);
                //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
                UserTypeTrg usertrg = new UserTypeTrg();

                //***************
                List<FilterUserTrg> FL = new List<FilterUserTrg>();
                FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
                //**********
                userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
                lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();
                TrainingAnalytics t = new TrainingAnalytics();
                t = WDB.Get_StatusWise_Trg_Count(lwtc, startdate, enddate);

                
                d.upcoming_trg = t.trg_upcoming;
                d.trg_in_progress = t.trg_in_prog;
               // d.trg_completed = t.trg_completed;
                d.trg_time = Convert.ToDecimal(t.trg_time);


                SessionDB sdb = new SessionDB(_configuration);
                List<SessionCompletionStatus> cd = new List<SessionCompletionStatus>();
                cd = sdb.Get_Session_Status(null, usertype, userid, startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                d.trg_completed = cd
                                        .GroupBy(s => s.tttttm_training_id)
                                        .Where(g => g.All(s => s.iscompleted == 1))
                                        .Select(g => g.Key)
                                        .ToList().Count;

                //*************Get Enrollment 
                List <Participant> p = new List<Participant>();
                ParticipantDB tdb = new ParticipantDB(_configuration);
                p = tdb.Get_Trg_Participant_List(null, null, banchid, null, null, null, null, null, null, 1, 1);
                if (p.Count() > 0)
                {
                    d.total_enrollments = p.FirstOrDefault().totalrecords;
                }

                List<Participant> p1 = new List<Participant>();
                ParticipantDB tdb1 = new ParticipantDB(_configuration);
                p1 = tdb.Get_Trg_Participant_List(null, null, banchid, null, null, null, null, "Status", "4", 1, 1);
                if (p1.Count() > 0)
                {
                    d.consent_received = p1.FirstOrDefault().totalrecords;
                    d.Pending_for_Approval = p1.FirstOrDefault().totalrecords;
                }

            }
            else
            {
                DashboardDB dbd = new DashboardDB(_configuration);
                DBAnalytics aba = new DBAnalytics();
                aba = dbd.Get_Admin_DB_Analytics(usertype, userid, startdate, enddate, Branchid);

                d.total_participant = aba.total_participant;
                d.active_learners = aba.active_learners;
                d.avg_learning_time_practical = aba.avg_learning_time_practical;
                d.avg_learning_time_therory = aba.avg_learning_time_therory;
            }
           

            //List<Learning_Report_Data> s = new List<Learning_Report_Data>();
            //SupportBL SBL = new SupportBL(_configuration);
            //s = SBL.Learning_Report_Data(null, null, null, banchid,2, null, null, 1, 1, null, null, null, null);
            //if (s.Count() > 0)
            //{
            //    d.Course_started = s.FirstOrDefault().totalrecord;
            //}

            //List<Participant> p2 = new List<Participant>();
            //ParticipantDB tdb2 = new ParticipantDB(_configuration);
            //p2 = tdb.Get_Trg_Participant_List(null, null, banchid, null, null, null, null, "Status", "1", 1, 1);
            //if (p2.Count() > 0)
            //{
            //    d.approved = p2.FirstOrDefault().totalrecords;
              
            //}


            //**********
          
            return Ok(d);
        }
        [HttpPost]
        [Route("api/Dashboard_Data")]
        public IActionResult Dashboard_Data(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param,[FromBody] SearchParam? searchCriterias,string filter_status = null,string filter_cd=null,string filter_acd=null)
        {

            var Authtoken = Request.Cookies["Auth_token"];


            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate, filter_status, filter_cd, filter_acd);


            //***********Code to get Rating data***********
            DashboardBL dbl = new DashboardBL(_configuration);
            List<TRG_FEEDBACK_DATA> rating = new List<TRG_FEEDBACK_DATA>();
            string finyear = DashboardBL.GetFinancialYear(startdate, enddate);

            rating = dbl.Get_Trg_Feedback_Data(finyear);

            //************

            //**********Filter by status
            if (filter_status != null)
            {
                var filter_statuses = filter_status.Split(',').Select(s => s.Trim()).ToList();
                lwtc = lwtc.Where(o => filter_statuses.Contains(o.TrainingStatus.ToString())).ToList();
            }
            else
            {
                lwtc = lwtc.Where(o => o.TrainingStatus != "3").ToList();
            }
            //if (filter_cd != null)
            //{
            //    lwtc = lwtc.Where(o => o.CourseDirector.ToString().ToUpper() == filter_cd.ToString().ToUpper()).ToList();
            //}
            //if (filter_acd != null)
            //{
            //    lwtc = lwtc.Where(o => o.AssociateDirector.ToString().ToUpper() == filter_acd.ToString().ToUpper()).ToList();
            //}

            //**********
           // lwtc = lwtc.Where(o => o.TrainingStatus != "3").ToList();
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
            UserTypeTrg usertrg = new UserTypeTrg();

            //***************
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            //**********
            userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();


            //in case of participant not need to show proposed and cancelled training
            if (usertype == "5")
            {
                lwtc = lwtc.Where(o=>o.TrainingStatus !="2" && o.TrainingStatus != "3").ToList();
            }

            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                DMSBL DBL = new DMSBL(_configuration);
                List<ParticipantAdditionlInfo> PAI = PDB.Get_Participant_Additional_info(null, userid);
                var ttpaiIds = string.Join(",", PAI.Select(p => $"'{p.ttpai_id}'"));
                List<DMS> DS = new List<DMS>();
                if(ttpaiIds != "")
                {
                    DS = DBL.GET_DMS_STATUS_DATA_FOR_SELECTED_DOCID(ttpaiIds, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);
                }
               

                foreach (Training vw in lwtc)
                {
                    string ttpaid = PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).FirstOrDefault().ttpai_id;
                    string status = DS.Where(o => o.doc_id.ToString().ToUpper() == ttpaid.ToString().ToUpper()).FirstOrDefault().doc_status.ToString();
                    vw.participantstatus = status;
                }


            }
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = lwtc;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(lwtc, searchCriterias.SearchCriteria.ToList());
            }

            lwtc = filteredItems;

            //*********

            List<SessionCompletionStatus> trg_session_status = new List<SessionCompletionStatus>();
            SessionDB sdb = new SessionDB(_configuration);
            trg_session_status = sdb.Get_Session_Status(null, usertype, userid, startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
         
            List<Session> sl = sdb.Get_Session_Data(startdate, enddate);
            foreach (Training item in lwtc)
            {
                decimal completion = 0;
                List<SessionCompletionStatus> trg_status = new List<SessionCompletionStatus>();
                trg_status = trg_session_status.Where(o => o.tttttm_training_id.ToString().ToUpper() == item.TrainingId.ToString().ToUpper()).ToList();
                if (trg_status.Count() > 0)
                {
                    completion = Math.Round(trg_status.Sum(o => o.percentcomplete) / trg_status.Count(), 2);
                }


                item.trg_completionpercentage = completion;


                var filter = sl.Where(o => o.trainingid.ToString() == item.TrainingId.ToString());
                item.sessions = filter.ToList();
                item.no_of_sessions = filter.Where(o=>o.ttttt_status == "0").Select(x => x.ttttt_session_id).Distinct().Count();

                var filterfaculties = filter.ToList().Where(o => o.ttttt_facultyid != null);
                // item.faculties = filter.ToList().Where(o => o.ttttt_facultyid !=null);



                //****************Code to calculate Action info
                if(usertype != "5")
                {
                    CommonEnum.DASHBOARD_TRG_ACTIONS[] actinoArray = (CommonEnum.DASHBOARD_TRG_ACTIONS[])Enum.GetValues(typeof(CommonEnum.DASHBOARD_TRG_ACTIONS));
                    int[] intactionArray = Array.ConvertAll(actinoArray, v => (int)v);

                    List<DisplayInfo> trgActiondisplay = new List<DisplayInfo>();
                    foreach (int value in intactionArray)
                    {
                        DisplayInfo DI = new DisplayInfo();
                        DI.key = value.ToString();
                        DI.name = Enum.GetName(typeof(CommonEnum.DASHBOARD_TRG_ACTIONS), value);
                        if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.AddParticipant)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.AssignmentList)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Attendance)
                        {

                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.FACULTY)
                            {
                                DI.value = true;

                            }
                            else
                            {
                                DI.value = false;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.ContentLibrary)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Feedback)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Forum)
                        {
                            DI.value = false;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Litteraroom)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.MeetingList)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.SessionList)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.TestList)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.TrainingCalendar)
                        {
                            DI.value = false;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Trg_Expenses)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.UpdateStatus)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Course_Overview)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = true;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Transaction_Details)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = true;
                            }

                        }
                        trgActiondisplay.Add(DI);
                    }


                    item.ActionInfos = trgActiondisplay.ToArray();

                }


                //*********Get Rating Data
                List<TRG_FEEDBACK_DATA> ratelist = rating
      .Where(o => o.trainingid.ToString().ToUpper() == item.TrainingId.ToString().ToUpper())
      .ToList();
                if (ratelist.Count() > 0)
                {
                    item.trg_rating = Math.Round(ratelist.FirstOrDefault().trg_rating, 2);  
                    item.no_of_response= ratelist.FirstOrDefault().no_of_response;
                }


                //*******************************






            }


        var pagedList = Paging.GetPagedList(param, lwtc);
            var result = Paging.GetPagedData(param, lwtc);

            return Ok(result);
        }

        [HttpPost]
        [Route("api/Dashboard_All_Trg_Data")]
        public IActionResult Dashboard_All_Trg_Data(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias, string filter_status = null, string filter_cd = null, string filter_acd = null)
        {
            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate, filter_status, filter_cd, filter_acd);

            //***********Code to get Rating data***********
            DashboardBL dbl = new DashboardBL(_configuration);
            List<TRG_FEEDBACK_DATA> rating = new List<TRG_FEEDBACK_DATA>();
            string finyear = DashboardBL.GetFinancialYear(startdate, enddate);

            rating = dbl.Get_Trg_Feedback_Data(finyear);

            //************

            //**********Filter by status
            if (filter_status != null)
            {
                var filter_statuses = filter_status.Split(',').Select(s => s.Trim()).ToList();
                lwtc = lwtc.Where(o => filter_statuses.Contains(o.TrainingStatus.ToString())).ToList();
            }
            else
            {
                lwtc = lwtc.Where(o => o.TrainingStatus != "3").ToList();
            }
            //if (filter_cd != null)
            //{
            //    lwtc = lwtc.Where(o => o.CourseDirector.ToString().ToUpper() == filter_cd.ToString().ToUpper()).ToList();
            //}
            //if (filter_acd != null)
            //{
            //    lwtc = lwtc.Where(o => o.AssociateDirector.ToString().ToUpper() == filter_acd.ToString().ToUpper()).ToList();
            //}

            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
            //UserTypeTrg usertrg = new UserTypeTrg();

            ////***************
            //List<FilterUserTrg> FL = new List<FilterUserTrg>();
            //FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            ////**********
            //userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            //lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();

            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                DMSBL DBL = new DMSBL(_configuration);
                List<ParticipantAdditionlInfo> PAI = PDB.Get_Participant_Additional_info(null, userid);
                //List<DMS> DS = DBL.Get_DMS_STATUS(null, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);
                var ttpaiIds = string.Join(",", PAI.Select(p => $"'{p.ttpai_id}'"));
                List<DMS> DS = new List<DMS>();
                if(ttpaiIds != "")
                {
                    DS = DBL.GET_DMS_STATUS_DATA_FOR_SELECTED_DOCID(ttpaiIds, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);
                }
             

                foreach (Training vw in lwtc)
                {
                    if(PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).Count() > 0)
                    {
                        string ttpaid = PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).FirstOrDefault().ttpai_id;
                        string status = DS.Where(o => o.doc_id.ToString().ToUpper() == ttpaid.ToString().ToUpper()).FirstOrDefault().doc_status.ToString();
                        vw.participantstatus = status;
                    }
                    else
                    {
                        vw.participantstatus = null;
                    }
                    
                   
                }


            }
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = lwtc;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(lwtc, searchCriterias.SearchCriteria.ToList());
            }

            lwtc = filteredItems;

            lwtc= lwtc.Where(o=>o.TrainingStatus != "0").ToList();

            //*********

            //in case of participant not need to show proposed and cancelled training
            if (usertype == "5")
            {
                lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();
            }

            //*************

            List<SessionCompletionStatus> trg_session_status = new List<SessionCompletionStatus>();
            SessionDB sdb = new SessionDB(_configuration);
            trg_session_status = sdb.Get_Session_Status(null, usertype, userid, startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));

            List<Session> sl = sdb.Get_Session_Data(startdate, enddate);
            foreach (Training item in lwtc)
            {
                decimal completion = 0;
                List<SessionCompletionStatus> trg_status = new List<SessionCompletionStatus>();
                trg_status = trg_session_status.Where(o => o.tttttm_training_id.ToString().ToUpper() == item.TrainingId.ToString().ToUpper()).ToList();
                if (trg_status.Count() > 0)
                {
                    completion = Math.Round(trg_status.Sum(o => o.percentcomplete) / trg_status.Count(), 2);
                }


                item.trg_completionpercentage = completion;


                var filter = sl.Where(o => o.trainingid.ToString() == item.TrainingId.ToString());
                item.sessions = filter.ToList();
                item.no_of_sessions = filter.Where(o => o.ttttt_status == "0").Select(x => x.ttttt_session_id).Distinct().Count();


                var filterfaculties = filter.ToList().Where(o => o.ttttt_facultyid != null);
                // item.faculties = filter.ToList().Where(o => o.ttttt_facultyid !=null);

                //****************Code to calculate Action info
                if (usertype != "5")
                {
                    CommonEnum.DASHBOARD_TRG_ACTIONS[] actinoArray = (CommonEnum.DASHBOARD_TRG_ACTIONS[])Enum.GetValues(typeof(CommonEnum.DASHBOARD_TRG_ACTIONS));
                    int[] intactionArray = Array.ConvertAll(actinoArray, v => (int)v);

                    List<DisplayInfo> trgActiondisplay = new List<DisplayInfo>();
                    foreach (int value in intactionArray)
                    {
                        DisplayInfo DI = new DisplayInfo();
                        DI.key = value.ToString();
                        DI.name = Enum.GetName(typeof(CommonEnum.DASHBOARD_TRG_ACTIONS), value);
                        if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.AddParticipant)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.AssignmentList)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Attendance)
                        {

                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.FACULTY)
                            {
                                DI.value = true;

                            }
                            else
                            {
                                DI.value = false;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.ContentLibrary)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Feedback)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Forum)
                        {
                            DI.value = false;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Litteraroom)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.MeetingList)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.SessionList)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.TestList)
                        {
                            DI.value = true;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.TrainingCalendar)
                        {
                            DI.value = false;
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Trg_Expenses)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }
                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.UpdateStatus)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = false;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Course_Overview)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = true;
                            }

                        }
                        else if (Convert.ToInt32(value) == (int)CommonEnum.DASHBOARD_TRG_ACTIONS.Transaction_Details)
                        {
                            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.Admin || Convert.ToInt32(usertype) == (int)CommonEnum.usertype.CD)
                            {
                                DI.value = true;
                            }
                            else
                            {
                                DI.value = true;
                            }

                        }
                        trgActiondisplay.Add(DI);
                    }


                    item.ActionInfos = trgActiondisplay.ToArray();

                }

                //*********Get Rating Data
                List<TRG_FEEDBACK_DATA> ratelist = rating
      .Where(o => o.trainingid.ToString().ToUpper() == item.TrainingId.ToString().ToUpper())
      .ToList();
                if (ratelist.Count() > 0)
                {
                    item.trg_rating = Math.Round(ratelist.FirstOrDefault().trg_rating, 2);
                    item.no_of_response = ratelist.FirstOrDefault().no_of_response;
                }


                //*******************************


            }

            var pagedList = Paging.GetPagedList(param, lwtc);
            var result = Paging.GetPagedData(param, lwtc);

            return Ok(result);
        }


        [HttpPost]
        [Route("api/Upcoming_Events")]
        public IActionResult Upcoming_Events(DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {
            param.PageNumber = 1;
            param.PageSize = 100;
            DateTime dtcurrent = DateTime.Now;
            DashboardBL dbl = new DashboardBL(_configuration);
            List<TRG_FEEDBACK_DATA> rating = new List<TRG_FEEDBACK_DATA>();
            string finyear = DashboardBL.GetFinancialYear(startdate, enddate);

            rating = dbl.Get_Trg_Feedback_Data(finyear);

            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();

            List<Training> lwtc_all = new List<Training>();

            List<Training> lwtc_final = new List<Training>();


            var (start, end) = GetCurrentFinancialYearDates();
          

            lwtc = WDB.Get_VW_Training_calendar(start, end);
            lwtc_all = lwtc;

            lwtc = lwtc.Where(o => o.T_EndDate >= dtcurrent).ToList();

            if (lwtc.Where(o=>o.TrainingStatus == "1" || o.TrainingStatus == "5").Count() >= 5)
            {
                lwtc_final = lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").ToList();
            }
            else
            {
                lwtc_final = lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").ToList();


                lwtc_final.AddRange(lwtc_all.Where(o => o.T_EndDate < dtcurrent
                                          && o.TrainingStatus != "2"
                                          && o.TrainingStatus != "3"
                                          && o.TrainingStatus != "0").ToList());
                lwtc_final= lwtc_final.Take(5).ToList();
            }

            //**********Implement Search
           
            var searchService = new SearchService();
            var filteredItems = lwtc_final;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(lwtc_final, searchCriterias.SearchCriteria.ToList());
            }

            lwtc_final = filteredItems;

            //*********

            lwtc_final = lwtc_final.Where(o => o.TrainingStatus != "0").ToList();

          

            //in case of participant not need to show proposed and cancelled training
            //lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();

            //*************

            foreach (Training t in lwtc_final)
            {
                if (t.TrainingStatus == "4")
                {
                    t.is_reg_open = false;
                }
                else
                {
                    if (t.T_EndDate >= System.DateTime.Now)
                    {
                        t.is_reg_open = true;
                    }
                    else
                    {
                        t.is_reg_open = false;
                    }
                }


                List<TRG_FEEDBACK_DATA> ratelist = rating
     .Where(o => o.trainingid.ToString().ToUpper() == t.TrainingId.ToString().ToUpper())
     .ToList();
                if (ratelist.Count() > 0)
                {
                    t.trg_rating = Math.Round(ratelist.FirstOrDefault().trg_rating, 2);
                    t.no_of_response = ratelist.FirstOrDefault().no_of_response;
                }

            }


            var pagedList = Paging.GetPagedList(param, lwtc_final);
            var result = Paging.GetPagedData(param, lwtc_final);

            return Ok(result);
        }

        [HttpPost]
        [Route("api/Get_Tour_Config")]
        public IActionResult Get_Tour_Config(string usertype, string userid, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {
            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();

            lwtc = WDB.Get_VW_Training_calendar(startdate, enddate);
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Get Data Training_calendar" + System.DateTime.Now);
            UserTypeTrg usertrg = new UserTypeTrg();

            //***************
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            FL = lwtc.ConvertAll(x => new FilterUserTrg { trainingid = x.TrainingId.ToString() });
            //**********
            userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, startdate, enddate);
            lwtc = lwtc.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.TrainingId.ToString())).ToList();


            //in case of participant not need to show proposed and cancelled training
            if (usertype == "5")
            {
                lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();
            }

            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                DMSBL DBL = new DMSBL(_configuration);
                List<ParticipantAdditionlInfo> PAI = PDB.Get_Participant_Additional_info(null, userid);
                var ttpaiIds = string.Join(",", PAI.Select(p => $"'{p.ttpai_id}'"));

                List<DMS> DS = DBL.GET_DMS_STATUS_DATA_FOR_SELECTED_DOCID(ttpaiIds, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);


                //List<DMS> DS = DBL.Get_DMS_STATUS(null, (int)CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING);

                foreach (Training vw in lwtc)
                {
                    string ttpaid = PAI.Where(o => o.TrainingId.ToString().ToUpper() == vw.TrainingId.ToString().ToUpper() && o.Participantid.ToString().ToUpper() == userid.ToString().ToUpper()).FirstOrDefault().ttpai_id;
                    string status = DS.Where(o => o.doc_id.ToString().ToUpper() == ttpaid.ToString().ToUpper()).FirstOrDefault().doc_status.ToString();
                    vw.participantstatus = status;
                }


            }
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = lwtc;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(lwtc, searchCriterias.SearchCriteria.ToList());
            }

            lwtc = filteredItems;

            //*********

            List<SessionCompletionStatus> trg_session_status = new List<SessionCompletionStatus>();
            SessionDB sdb = new SessionDB(_configuration);
            trg_session_status = sdb.Get_Session_Status(null, usertype, userid, startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));

            List<Session> sl = sdb.Get_Session_Data(startdate, enddate);
            foreach (Training item in lwtc)
            {
                decimal completion = 0;
                List<SessionCompletionStatus> trg_status = new List<SessionCompletionStatus>();
                trg_status = trg_session_status.Where(o => o.tttttm_training_id.ToString().ToUpper() == item.TrainingId.ToString().ToUpper()).ToList();
                if (trg_status.Count() > 0)
                {
                    completion = Math.Round(trg_status.Sum(o => o.percentcomplete) / trg_status.Count(), 2);
                }


                item.trg_completionpercentage = completion;


                var filter = sl.Where(o => o.trainingid.ToString() == item.TrainingId.ToString());
                item.sessions = filter.ToList();
                item.no_of_sessions = filter.Where(o => o.ttttt_status == "0").Select(x => x.ttttt_session_id).Distinct().Count();

                var filterfaculties = filter.ToList().Where(o => o.ttttt_facultyid != null);
                // item.faculties = filter.ToList().Where(o => o.ttttt_facultyid !=null);




            }

            var pagedList = Paging.GetPagedList(param, lwtc);
            var result = Paging.GetPagedData(param, lwtc);
            Tour_Config_Data td = new Tour_Config_Data();
            foreach(Training t in pagedList)
            {
                if(t.trg_Setting != null)
                {
                    if (t.trg_Setting.displaycontrols.Where(O => O.id == 10).Count() > 0)
                    {
                        if(t.trg_Setting.displaycontrols.Where(O => O.id == 10).FirstOrDefault().isdisplay == 1)
                        {
                            td.is_overview_required = 1;
                        }
                    }
                    if (t.trg_Setting.displaycontrols.Where(O => O.id == 11).Count() > 0)
                    {
                        if (t.trg_Setting.displaycontrols.Where(O => O.id == 11).FirstOrDefault().isdisplay == 1)
                        {
                            td.is_instruction_course_required = 1;
                        }
                    }
                }
                else
                {
                    td.is_overview_required = 1;
                }

                if (t.trg_completionpercentage == 0)
                {
                    td.is_start_course_required = 1;
                }
                if (t.trg_completionpercentage > 0)
                {
                    td.is_resume_course_required = 1;
                }
                if (t.trg_completionpercentage == 100)
                {
                    td.is_completed_required = 1;
                }


            }



            return Ok(td);
        }

        [HttpPost]
        [Route("api/Get_Fb_Data")]
        public IActionResult Get_Fb_Data([FromBody]FB_Share_Data fbdata)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<meta property=\"og:image\" content=\""+fbdata.imagepath+"\"  id=\"metaImage\"  />");
            sb.Append("<meta property=\"og:image:alt\" content=\""+fbdata.alt_imagepath+"\"  />");
            sb.Append("<meta property=\"og:type\" content=\"" + fbdata.type+"\"  />");
            sb.Append("<meta property=\"og:title\"  content=\"" + fbdata.title+"\"  id=\"metaTitle\"  />");
            sb.Append("<meta property=\"og:description\" content=\"" + fbdata.description+"\"  id=\"metaDescription\"  />");
            sb.Append("<meta property=\"og:url\" content=\"" + fbdata.url + "\"   id=\"metaDescription\"  />");
            sb.Append("</head>");

            sb.Append("<body>");
            sb.Append("<script> window.location="+ fbdata .url+ "");

            sb.Append("</script>");

            sb.Append("</body>");

            sb.Append("</html>");


            return Ok(sb.ToString());
        }

        public static (DateTime startDate, DateTime endDate) GetCurrentFinancialYearDates()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Get the current year
            int currentYear = currentDate.Year;

            // Initialize the start and end date for the financial year
            DateTime startDate;
            DateTime endDate;

            // If current month is before April, then financial year starts from previous year
            if (currentDate.Month < 4)
            {
                startDate = new DateTime(currentYear - 1, 4, 1);
                endDate = new DateTime(currentYear, 3, 31);
            }
            else
            {
                startDate = new DateTime(currentYear, 4, 1);
                endDate = new DateTime(currentYear + 1, 3, 31);
            }

            // Return the start and end dates as a tuple
            return (startDate, endDate);
        }



        [HttpGet]
        [Route("api/Get_Training_Tags")]
        public IActionResult Get_Training_Tags(string trainingid)
        {
            List<tags> mcl = new List<tags>();
            EvalBL ebl = new EvalBL(_configuration);
            Mock_test_configuration mtc = new Mock_test_configuration();
            mtc = ebl.GET_MOCK_TEST_CONFIGURATION();
            List<TrainingTags> tt = new List<TrainingTags>();
            tt = mtc.TrainingTags.ToList();
            TrainingTags t=new TrainingTags();
           if(tt.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).Count() > 0)
            {
                t = tt.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).FirstOrDefault();
                mcl = t.tags.ToList();
            }
            else
            {
                mcl.Add(new tags { displayname = "Module I", tag = "asp.net" });
                mcl.Add(new tags { displayname = "Module II", tag = "reactjs" });
                mcl.Add(new tags { displayname = "Module III", tag = "dbms" });
            }
           
            return Ok(mcl);
        }

        [HttpGet]
        [Route("api/Get_Participant_By_MOBILE")]
        public IActionResult Get_Participant_By_MOBILE(string mobileno,string name,string? trainingid=null)
        {
           string participantid=Guid.NewGuid().ToString();
            UserDB UBL = new UserDB(_configuration);
            User amob = new User();
            amob = UBL.GET_MOBILE_NO_DATA(mobileno, 2);
            if(amob.userid != null)
            {
                return Ok(amob.agency.AgencyId);
            }
            else
            {
                string userid= Guid.NewGuid().ToString();
                string agencyid= Guid.NewGuid().ToString();
                List<user_branches_detail> lbd=new    List<user_branches_detail>();
                lbd.Add(new user_branches_detail  { branchid = Common.CommonEnum.Branchid });
                user_branches ubr = new user_branches
                {
                    branchtype = "00001",
                    branches = lbd.ToArray()
                };
                UserAgency ag = new UserAgency
                {
                    AgencyId = agencyid,
                    AgencyName = name,
                    ag_first_name = name,
                    ag_mobileno = mobileno,
                    AgencyTypeId="00051",
                    CreatedBy = userid


                };

                UserBL ub = new UserBL(_configuration);
                LoginUser lu = new LoginUser();
                lu.userid = userid;
                lu.f_name= name;
                lu.username = mobileno;
                lu.usertype = "5";
                lu.branchid = Common.CommonEnum.Branchid;
                lu.createdby = userid;
                lu.agency = ag;
                lu.branches = ubr;




                ub.Save_User_Data(lu);
                return Ok(lu.agency.AgencyId);
            }
            
        }
        [HttpPost]
        [Route("api/MOCK_TEST_EVENTS")]
        public IActionResult MOCK_TEST_EVENTS(DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias)
        {
            param.PageNumber = 1;
            param.PageSize = 100;
            DateTime dtcurrent = DateTime.Now;

            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();

            List<Training> lwtc_all = new List<Training>();

            List<Training> lwtc_final = new List<Training>();


            var (start, end) = GetCurrentFinancialYearDates();


            lwtc = WDB.Get_VW_Training_calendar(start, end);
            lwtc_all = lwtc;

            lwtc = lwtc.Where(o => o.T_EndDate >= dtcurrent).ToList();

            if (lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").Count() >= 5)
            {
                lwtc_final = lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").ToList();
            }
            else
            {
                lwtc_final = lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").ToList();


                lwtc_final.AddRange(lwtc_all.Where(o => o.T_EndDate < dtcurrent
                                          && o.TrainingStatus != "2"
                                          && o.TrainingStatus != "3"
                                          && o.TrainingStatus != "0").ToList());
                lwtc_final = lwtc_final.Take(5).ToList();
            }

            //**********Implement Search
            //var searchService = new SearchService();
            //var filteredItems = lwtc;
            //if (searchCriterias != null)
            //{
            //    filteredItems = searchService.FilterItems(lwtc, searchCriterias.SearchCriteria.ToList());
            //}

            //lwtc = filteredItems;

            //lwtc = lwtc.Where(o => o.TrainingStatus != "0").ToList();

            //*********

            //in case of participant not need to show proposed and cancelled training
            //lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();

            //*************

            foreach (Training t in lwtc_final)
            {
                if (t.TrainingStatus == "4")
                {
                    t.is_reg_open = false;
                }
                else
                {
                    if (t.T_EndDate >= System.DateTime.Now)
                    {
                        t.is_reg_open = true;
                    }
                    else
                    {
                        t.is_reg_open = false;
                    }
                }
            }


            //Filter Trainings
            List<tags> mcl = new List<tags>();
            EvalBL ebl = new EvalBL(_configuration);
            Mock_test_configuration mtc = new Mock_test_configuration();
            mtc = ebl.GET_MOCK_TEST_CONFIGURATION();
            List<TrainingTags> ttg = mtc.TrainingTags.ToList();
            var filteredList = lwtc_final.Where(lwtc => ttg.Any(tt => tt.trainingid.ToString().ToUpper() == lwtc.TrainingId.ToString().ToUpper())).ToList();

            //  var pagedList = Paging.GetPagedList(param, filteredList);
            var result = Paging.GetPagedData(param, filteredList);

            return Ok(result);
        }


        [HttpGet]
        [Route("api/Littera_Events")]
        public IActionResult Littera_Events(string SecretKey, DateTime startdate, DateTime enddate, [FromQuery] PaginationParam param)
        {
            DashboardBL dbl = new DashboardBL(_configuration);
            if(dbl.validate_external_user_key(SecretKey) != true)
            {
                return Unauthorized();
            }


            param.PageNumber = 1;
            param.PageSize = 100;
            DateTime dtcurrent = DateTime.Now;

            TrainingDB WDB = new TrainingDB(_configuration);

            List<Training> lwtc = new List<Training>();

            List<Training> lwtc_all = new List<Training>();

            List<Training> lwtc_final = new List<Training>();


            var (start, end) = GetCurrentFinancialYearDates();


            lwtc = WDB.Get_VW_Training_calendar(start, end);
            lwtc_all = lwtc;

            lwtc = lwtc.Where(o => o.T_EndDate >= dtcurrent).ToList();

            if (lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").Count() >= 5)
            {
                lwtc_final = lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").ToList();
            }
            else
            {
                lwtc_final = lwtc.Where(o => o.TrainingStatus == "1" || o.TrainingStatus == "5").ToList();


                lwtc_final.AddRange(lwtc_all.Where(o => o.T_EndDate < dtcurrent
                                          && o.TrainingStatus != "2"
                                          && o.TrainingStatus != "3"
                                          && o.TrainingStatus != "0").ToList());
                lwtc_final = lwtc_final.Take(5).ToList();
            }

            //**********Implement Search

            var searchService = new SearchService();
            var filteredItems = lwtc_final;
           
            lwtc_final = filteredItems;

            //*********

            lwtc_final = lwtc_final.Where(o => o.TrainingStatus != "0").ToList();



            //in case of participant not need to show proposed and cancelled training
            //lwtc = lwtc.Where(o => o.TrainingStatus != "2" && o.TrainingStatus != "3").ToList();

            //*************

            foreach (Training t in lwtc_final)
            {
                if (t.TrainingStatus == "4")
                {
                    t.is_reg_open = false;
                }
                else
                {
                    if (t.T_EndDate >= System.DateTime.Now)
                    {
                        t.is_reg_open = true;
                    }
                    else
                    {
                        t.is_reg_open = false;
                    }
                }
            }



            List<Littera_Events> flist = new List<Littera_Events>();

            REACT_APP_CONFIGURATION RAC = new REACT_APP_CONFIGURATION();

            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Config.json"));
            RAC = JsonConvert.DeserializeObject<REACT_APP_CONFIGURATION>(jsontxt);

            foreach (Training t in lwtc_final)
            {
                t.trg_Setting.displaycontrols = t.trg_Setting.displaycontrols.Where(o => o.isdisplay == 1).ToArray();
                flist.Add(new Littera_Events
                {
                    t_Name = t.T_Name,
                    t_Details = t.T_Details,
                    noOfParticipants_Registered = t.NoOfParticipants_Registered,
                    trainingId = t.TrainingId.ToString(),
                    trainingStatus = t.TrainingStatus,
                    trg_type = t.trg_type?.ToString(),
                    trg_Setting = t.trg_Setting,
                    img_path = RAC.LITTERA_CDN_BASE_URL + "/Training_Upload/" + t.img_path,
                    redirection_link = RAC.LITTERA_CDN_BASE_URL + "/frm_read_course_details.aspx?q=" + t.TrainingId

                });
            }






          
            var result = Paging.GetPagedData(param, flist);

            return Ok(result);
        }



     



    }
}
