using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using static LitteraCore.Common.CommonEnum;
using static System.Net.Mime.MediaTypeNames;

namespace LitteraCore.Controllers
{
    public class SessionController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public SessionController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/UserSessions")]
        public IActionResult UserSessions(string usertype, string userid, DateTime trg_startdate, DateTime trg_enddate, [FromQuery] PaginationParam param, DateTime? SessionDate = null)
        {

            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_User_Session(usertype, userid, trg_startdate, trg_enddate, SessionDate);
            var result = Paging.GetPagedData(param, s);
            return Ok(result);
        }


        [HttpGet]
        [Route("api/TrgSessions")]
        public IActionResult TrgSessions(string trainingid,int pagetype=0,string usertype=null,string userid=null)
        {
          
            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s=new List<Session>();
            s=cbl.Get_Session_Data_By_Trg(trainingid);
            //*********Get Training Setting Detail
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            string startdate = trgdetail.StartDate?.ToString("yyyy/MM/dd");
            string enddate = trgdetail.T_EndDate?.ToString("yyyy/MM/dd");



            MeetingDB mbl = new MeetingDB(_configuration);
            List<Meeting> m = new List<Meeting>();
            m = mbl.Get_Trg_Meetings(trainingid);

            foreach (Session sl in s)
            {
                List<Meeting> lm = new List<Meeting>();
                lm = m.Where(o => o.ttlm_ttttt_session_id.ToString().ToUpper() == sl.ttttt_session_id.ToString().ToUpper()).ToList();
                sl.meeting = lm.ToArray();
            }

            CommonEnum.SESSION_LIST_ACTIONS[] enumActionArray = (CommonEnum.SESSION_LIST_ACTIONS[])Enum.GetValues(typeof(CommonEnum.SESSION_LIST_ACTIONS));
            int[] intActionArray = Array.ConvertAll(enumActionArray, v => (int)v);
            //**************
            if (trgdetail.trg_Setting != null)
            {
                if(trgdetail.trg_Setting.Session != null)
                {
                    if(trgdetail.trg_Setting.Session.SessionEntry != null)
                    {
                        List<DisplayInfo> di = new List<DisplayInfo>();
                        if (trgdetail.trg_Setting.Session.SessionEntry.Module == true)
                        {
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                        }
                        else{
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Week == true)
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Date == true)
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Day == true)
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.srno == true)
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                        }

                        foreach (Session sess in s)
                        {
                            sess.displayInfos= di.ToArray();
                            sess.DisplayOrder = trgdetail.trg_Setting.Session.SessionOrder;
                        }

                    }
                }
            }
            else
            {
                if (trgdetail.isSelfPaced == 1)
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                    foreach (Session sess in s)
                    {
                        sess.displayInfos = di.ToArray();
                        sess.DisplayOrder = "1,4,2,5,3";
                    }
                }
                else
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                    foreach (Session sess in s)
                    {
                        sess.displayInfos = di.ToArray();
                        sess.DisplayOrder = "1,2,4,3,5";
                    }
                }
            }

            //**************Attach Action Info
            int iscdLogin = 0;
            int participantstatus = 0;
            string testparticipantid = "";
            int ismeetingavailable = 0;

            if (usertype != null)
            {
                List<Test> TESTS = new List<Test>();
                if (Convert.ToInt16(usertype)  == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    ParticipantDB PDB = new ParticipantDB(_configuration);
                    List<Participant> pl = new List<Participant>();
                    pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid);
                    pl = pl.Where(o => o.ParticipantId.ToUpper() == userid.ToString().ToUpper()).ToList();
                    if (pl.Count() > 0)
                    {
                        participantstatus = pl.FirstOrDefault().is_approve;
                    }
                    else
                    {
                        participantstatus = 0;
                    }
                  
                    EvalDB tbl = new EvalDB(_configuration);
                    TESTS = tbl.Get_test_List(usertype, userid);

                   

                }

                if (trgdetail.CourseDirector.ToString().ToUpper() == userid.ToString().ToUpper() || trgdetail.AssociateDirector.ToString().ToUpper() == userid.ToString().ToUpper())
                {
                    iscdLogin = 1;
                }


                SessionDB SDB = new SessionDB(_configuration);
                List<SessionCompletionStatus> status = new List<SessionCompletionStatus>();
                if (userid.ToString() != "")
                {
                    status = SDB.Get_Session_Status(trainingid, usertype, userid, startdate, enddate);
                }





                foreach (Session sess in s)
                {
                    List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
                    sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).ToList();
                    if (sessionstatus.Count > 0)
                    {
                        if (Convert.ToString(sessionstatus.FirstOrDefault().percentcomplete) != "")
                        {
                            sess.completionpercentage = Convert.ToDecimal(sessionstatus.FirstOrDefault().percentcomplete);
                        }
                        else
                        {
                            sess.completionpercentage = 0;
                        }

                    }
                    else
                    {
                        sess.completionpercentage = 0;
                    }

                    if (m.Where(o => o.ttlm_ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).Count()>0)
                    {
                        ismeetingavailable = 1;
                    }
                    else
                    {
                        ismeetingavailable = 0;
                    }


                    if (TESTS.Where(o => o.sessionid.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                    {
                        testparticipantid = TESTS.Where(o => o.sessionid.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).FirstOrDefault().participantstatus;
                    }
                    else
                    {
                        testparticipantid = "";
                    }
                    List<DisplayInfo> sessionActiondisplay = new List<DisplayInfo>();
                    foreach (int value in intActionArray)
                    {
                        DisplayInfo DI = new DisplayInfo();
                        DI.key = value.ToString();
                        DI.name = Enum.GetName(typeof(CommonEnum.SESSION_LIST_ACTIONS), value);
                        DI.value = SessionDB.SESSION_DISPLAY_ACTION(usertype, Convert.ToInt32(trgdetail.trg_type), Convert.ToInt32(sess.ttttt_type), Convert.ToInt32(sess.ttttt_status), value, Convert.ToInt32(sess.ttttt_complimentory), ismeetingavailable,iscdLogin,participantstatus,testparticipantid, sess.completiontype?.id.ToString(), sess.completionpercentage);
                        sessionActiondisplay.Add(DI);
                    }
                    sess.ActionInfos = sessionActiondisplay.ToArray();
                  



                }

            }
    
          
            //*************

            if (pagetype != 0)
            {
                string[] strarr = CommonEnum.Page_Allowed_Session_Type(pagetype).Split(",".ToCharArray());
                int[] arr = Array.ConvertAll(strarr, int.Parse);
                s = s.Where(o => arr.Contains(o.ttttt_type)).ToList();

            }

            //***********Get Trg Setting
            Trg_Setting TS=new Trg_Setting();
            session_setting sessionSetting = new session_setting();
            TS.Session = sessionSetting;
                 if (trgdetail.trg_Setting != null)
            {
                if(trgdetail.trg_Setting.Session != null)
                {
                    if(trgdetail.trg_Setting.Session.SessionEntry != null)
                    {
                        List<DisplayInfo> di = new List<DisplayInfo>();
                        if (trgdetail.trg_Setting.Session.SessionEntry.Module == true)
                        {
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                        }
                        else{
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Week == true)
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Date == true)
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Day == true)
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.srno == true)
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                        }
                        TS.Session.SessionOrder = trgdetail.trg_Setting.Session.SessionOrder;
                        

                    }
                }
            }
            else
            {
                if (trgdetail.isSelfPaced == 1)
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                    TS.Session.SessionOrder = "1,4,2,5,3";
                }
                else
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                    TS.Session.SessionOrder ="1,2,4,3,5";
                 
                }
            }

            s = s.Where(o => o.ttttt_status != "9").ToList();
            s = CommonEnum.OrderSessionData(TS.Session.SessionOrder,s);



            //****************Get Session Restriction data
            SessionDB sdb=new SessionDB(_configuration);
            List<Session> slp = sdb.Get_Trg_Progress_Data(trainingid, userid);
            SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
            SessionBL sbl=new SessionBL(_configuration);
            foreach (Session sessn in s) {
                sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata);
            }
            //**********


            return Ok(s);   
        }


        [HttpPost]
        [Route("api/SessionNotes")]
        public IActionResult SaveSessionNotes([FromBody] Notes notes)
        {
            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.Save_Notes(notes);

            return Ok();
        }

        [HttpGet]
        [Route("api/SessionNotes")]
        public IActionResult GetSessionNotes(string userid, string trainingid = null, string sessionid = null)
        {
            List<Notes> N = new List<Notes>();
            SessionBL SDB = new SessionBL(_configuration);
            N = SDB.Get_Session_Notes(userid, trainingid, sessionid);

            return Ok(N);
        }

        [HttpGet]
        [Route("api/Comment")]
        public IActionResult Comment(string userid, string trainingid = null, string sessionid = null)
        {
            List<TrgComment> c = new List<TrgComment>();
            SessionBL sdb = new SessionBL(_configuration);
            c = sdb.Get_Trg_Comments(trainingid, sessionid);
            return Ok(c);
        }
        [HttpPost]
        [Route("api/Comment")]
        public IActionResult SaveComment([FromBody]TrgComment C)
        {
            List<TrgComment> c = new List<TrgComment>();
            SessionBL sdb = new SessionBL(_configuration);
            var issaved= sdb.Save_Trg_Comment(C);
            return Ok(issaved);
        }
        [HttpPost]
        [Route("api/Comment_Reply")]
        public IActionResult SaveComment_reply([FromBody] TrgComment_reply r)
        {
            List<TrgComment> c = new List<TrgComment>();
            SessionBL sdb = new SessionBL(_configuration);
            var issaved = sdb.Save_Trg_Comment_reply(r);
            return Ok(issaved);
        }


       


        [HttpPost]
        [Route("api/SaveContentFeedback")]
        public IActionResult SaveContentFeedback_new(string trainingid, string sessionid, string loginagencyid, [FromBody] SessionContentFacultyFeedback Feedback)
        {
            if (Feedback == null)
            {
                return BadRequest("Feedback cannot be null");
            }

            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.Save_Session_Content_Faculty_Feedback(trainingid, sessionid, loginagencyid, Feedback.Feedback.ToArray());

            return Ok(issaved);
        }

        [HttpPost]
        [Route("api/Update_Session_Status")]
        public IActionResult Update_Session_Status(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status)
        {
            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.Update_Session_Status(Participantid,trainingid,Sessionid,timeonsession,branchid,status);

            return Ok(issaved);
        }

        [HttpGet]
        [Route("api/CHECK_SESSION_FEEBDACK")]
        public IActionResult CHECK_SESSION_FEEBDACK(string userid, string trainingid, string sessionid)
        {
            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.Check_Content_Feedback_Exists(userid,trainingid,sessionid);

            return Ok(issaved);
        }

        [HttpGet]
        [Route("api/CHECK_SESSION_COMPLETION_STATUS")]
        public IActionResult CHECK_SESSION_COMPLETION_STATUS(string userid, string trainingid, string sessionid)
        {
            List<user_session_status> statusdata = new List<user_session_status>();
            SessionBL SDB = new SessionBL(_configuration);
            statusdata = SDB.Get_Participant_session_status(userid, trainingid, sessionid);
            return Ok(statusdata);
        }
        [HttpGet]
        [Route("api/SESSION_MEETINGS")]
        public IActionResult SESSION_MEETINGS(string sessionid)
        {
            List<Meeting> meetings = new List<Meeting>();
            SessionBL SDB = new SessionBL(_configuration);
            meetings = SDB.Get_Session_Meetings(sessionid);
            return Ok(meetings);
        }

        [HttpGet]
        [Route("api/CHECK_SESSION_RESTRICTION")]
        public IActionResult CHECK_SESSION_RESTRICTION(string usertype, string userid, string sessionid, string trainingid)
        {
            var isrestricted = true;
            SessionDB sdb=new SessionDB(_configuration);
            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
                SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
                if (restrictiondata.isrestricted == 1)
                {
                    //Code to get all session completion data

                   // SessionDB sdb = new SessionDB(_configuration);
                    List<Session> sl = sdb.Get_Trg_Progress_Data(trainingid, userid);

                    Session opensessiondetail = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).FirstOrDefault();
                    List<Session> SL = sl;
                    SL = SL.Where(o => o.ttttt_type != (int)Common.CommonEnum.SESSION_TYPE.Breaks).ToList();
                    // SL = SL.Where(o => o.ttttt_session_no != 0).ToList();
                    //****************Condition to relax check on complementory session
                    if (opensessiondetail.ttttt_complimentory == 1)
                    {
                        isrestricted = false;
                    }
                    else
                    {
                        if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.SrNo)
                        {
                            if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && o.noofcompletion != 1).Count() <= 0)
                            {
                                isrestricted = false;
                            }
                        }
                        else if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.Date)
                        {
                            if (SL.Where(o => Convert.ToDateTime(o.ttttt_session_dt) < Convert.ToDateTime(opensessiondetail.ttttt_session_dt) && o.noofcompletion != 1).Count() <= 0)
                            {
                                isrestricted = false;
                            }
                        }
                        else if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.Day)
                        {
                            if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test || Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Assignment)
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                                {
                                    //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment)).Count() <= 0)
                                    {
                                        isrestricted = false;
                                    }
                                }

                            }
                            else
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }

                        }
                        else if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.Week)
                        {
                            if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test || Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Assignment)
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_week) < Convert.ToInt32(opensessiondetail.ttttt_session_week) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                                {
                                    //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment)).Count() <= 0)
                                    {
                                        isrestricted = false;
                                    }
                                }

                            }
                            else
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_week) < Convert.ToInt32(opensessiondetail.ttttt_session_week) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }
                        }
                        else if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.Module)
                        {
                            if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test || Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Assignment)
                            {
                                if (SL.Where(o => Convert.ToInt32(o.module) < Convert.ToInt32(opensessiondetail.module) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                                {
                                    //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment)).Count() <= 0)
                                    {
                                        isrestricted = false;
                                    }
                                }

                            }
                            else
                            {
                                if (SL.Where(o => Convert.ToInt32(o.module) < Convert.ToInt32(opensessiondetail.module) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }
                        }
                    }



                }
                else
                {
                    isrestricted = false;
                }
            }
            else
            {
                isrestricted = false;
            }
            return Ok(isrestricted);
        }


        [HttpGet]
        [Route("api/SessionType")]
        public IActionResult SessionType()
        {

            List<Sessiontype> lu = new List<Sessiontype>();
            foreach (int i in Enum.GetValues(typeof(Common.CommonEnum.SessionType)))
            {
                Sessiontype u = new Sessiontype();
                u.id = i.ToString();
                u.name = Enum.GetName(typeof(Common.CommonEnum.SessionType), i);
                u.DisplayClass = CommonEnum.Get_Session_type_Display_Icon(i);
                lu.Add(u);


            }

            return Ok(lu);
        }

        [HttpGet]
        [Route("api/SessionModules")]
        public IActionResult SessionModules()
        {
            List<SessionModule> modules = new List<SessionModule>();
            SessionBL SDB = new SessionBL(_configuration);
            modules = SDB.Get_Session_Module();
            return Ok(modules);
        }

        [HttpPost]
        [Route("api/Session")]
        public IActionResult SaveSession([FromBody] CreateSessionDTO session)
        {
            
            SessionBL sdb = new SessionBL(_configuration);
            var issaved = sdb.Save_Session(session);
            return Ok(issaved);
        }
        [HttpGet]
        [Route("api/GET_SESSION_ENTRY_CONTROLS")]
        public IActionResult GET_SESSION_ENTRY_CONTROLS(int trainingtype)
        {
            SessionEntry E = new SessionEntry();
            E = SessionBL.GET_SESSION_ENTRY_CONTROLS(trainingtype);
            
            return Ok(E);
        }


        [HttpGet]
        [Route("api/TRG_SESSIONS_DURATION")]
        public IActionResult TRG_SESSIONS_DURATION(string trainingid, string durationtype = null)
        {

            SessionDB sdb = new SessionDB(_configuration);
            List<Session> sl = sdb.Get_Session_Data_By_Trg(trainingid);
            //*******

            sl = sl.Where(o => o.ttttt_status != ((int)CommonEnum.Session_Status.Delete).ToString()).ToList();

            Trg_session_duration tsd = new Trg_session_duration();
            decimal total_therory_min = 0;
            decimal total_practical_min = 0;
            decimal total_activity_min = 0;

            List<Session> dissession = new List<Session>();

            foreach (Session s in sl)
            {
                if (dissession.Where(o => o.ttttt_session_id.ToString().ToUpper() == s.ttttt_session_id.ToString().ToUpper()).Count() == 0)
                {
                    dissession.Add(s);
                }



            }






            total_therory_min = dissession.Where(o => o.ttttt_type == (int)CommonEnum.SessionType.Academic || o.ttttt_type == (int)CommonEnum.SessionType.SelfPaced).Sum(o => Convert.ToDecimal(o.ttttt_session_duration));
            total_practical_min = dissession.Where(o => o.ttttt_type == (int)CommonEnum.SessionType.Practical).Sum(o => Convert.ToDecimal(o.ttttt_session_duration));
            total_activity_min = dissession.Where(o => o.ttttt_type != (int)CommonEnum.SessionType.Academic && o.ttttt_type != (int)CommonEnum.SessionType.Practical && o.ttttt_type != (int)CommonEnum.SessionType.SelfPaced && o.ttttt_type != (int)CommonEnum.SessionType.Test).Sum(o => Convert.ToDecimal(o.ttttt_session_duration));

            if (durationtype == "2") // For Hour
            {
                tsd.Theory = decimal.Round((total_therory_min / 60), 2);
                tsd.Practical = decimal.Round((total_practical_min / 60), 2);
                tsd.Activity = decimal.Round((total_activity_min / 60), 2);
                tsd.durationtype = "Hr";
            }
            else if (durationtype == "3") // For days
            {
                tsd.Theory = decimal.Round((total_therory_min / 1440), 2);
                tsd.Practical = decimal.Round((total_practical_min / 1440), 2);
                tsd.Activity = decimal.Round((total_activity_min / 1440), 2);
                tsd.durationtype = "Days";
            }
            else  // min
            {
                tsd.Theory = total_therory_min;
                tsd.Practical = total_practical_min;
                tsd.Activity = total_activity_min;
                tsd.durationtype = "Min";
            }

            return Ok(tsd);
            
        }

        [HttpGet]
        [Route("api/GET_PARTICIPANT_NEXT_SESSION")]
        public IActionResult GET_PARTICIPANT_NEXT_SESSION(string trainingid,string participantid)
        {
            string userid = participantid;
            int pagetype = 0;
            string usertype = "5";
            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_Session_Data_By_Trg(trainingid);
            //*********Get Training Setting Detail
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            string startdate = trgdetail.StartDate?.ToString("yyyy/MM/dd");
            string enddate = trgdetail.T_EndDate?.ToString("yyyy/MM/dd");



            MeetingDB mbl = new MeetingDB(_configuration);
            List<Meeting> m = new List<Meeting>();
            m = mbl.Get_Trg_Meetings(trainingid);

            foreach (Session sl in s)
            {
                List<Meeting> lm = new List<Meeting>();
                lm = m.Where(o => o.ttlm_ttttt_session_id.ToString().ToUpper() == sl.ttttt_session_id.ToString().ToUpper()).ToList();
                sl.meeting = lm.ToArray();
            }

            CommonEnum.SESSION_LIST_ACTIONS[] enumActionArray = (CommonEnum.SESSION_LIST_ACTIONS[])Enum.GetValues(typeof(CommonEnum.SESSION_LIST_ACTIONS));
            int[] intActionArray = Array.ConvertAll(enumActionArray, v => (int)v);
            //**************
            if (trgdetail.trg_Setting != null)
            {
                if (trgdetail.trg_Setting.Session != null)
                {
                    if (trgdetail.trg_Setting.Session.SessionEntry != null)
                    {
                        List<DisplayInfo> di = new List<DisplayInfo>();
                        if (trgdetail.trg_Setting.Session.SessionEntry.Module == true)
                        {
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Week == true)
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Date == true)
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Day == true)
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.srno == true)
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                        }

                        foreach (Session sess in s)
                        {
                            sess.displayInfos = di.ToArray();
                            sess.DisplayOrder = trgdetail.trg_Setting.Session.SessionOrder;
                        }

                    }
                }
            }
            else
            {
                if (trgdetail.isSelfPaced == 1)
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                    foreach (Session sess in s)
                    {
                        sess.displayInfos = di.ToArray();
                        sess.DisplayOrder = "1,4,2,5,3";
                    }
                }
                else
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                    foreach (Session sess in s)
                    {
                        sess.displayInfos = di.ToArray();
                        sess.DisplayOrder = "1,2,4,3,5";
                    }
                }
            }

            //**************Attach Action Info
            int iscdLogin = 0;
            int participantstatus = 0;
            string testparticipantid = "";
            int ismeetingavailable = 0;

            if (usertype != null)
            {
                List<Test> TESTS = new List<Test>();
                if (Convert.ToInt16(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    ParticipantDB PDB = new ParticipantDB(_configuration);
                    List<Participant> pl = new List<Participant>();
                    pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid);
                    pl = pl.Where(o => o.ParticipantId.ToUpper() == userid.ToString().ToUpper()).ToList();
                    if (pl.Count() > 0)
                    {
                        participantstatus = pl.FirstOrDefault().is_approve;
                    }
                    else
                    {
                        participantstatus = 0;
                    }

                    EvalDB tbl = new EvalDB(_configuration);
                    TESTS = tbl.Get_test_List(usertype, userid);



                }

                if (trgdetail.CourseDirector.ToString().ToUpper() == userid.ToString().ToUpper() || trgdetail.AssociateDirector.ToString().ToUpper() == userid.ToString().ToUpper())
                {
                    iscdLogin = 1;
                }


                SessionDB SDB = new SessionDB(_configuration);
                List<SessionCompletionStatus> status = new List<SessionCompletionStatus>();
                if (userid.ToString() != "")
                {
                    status = SDB.Get_Session_Status(trainingid, usertype, userid, startdate, enddate);
                }





                foreach (Session sess in s)
                {
                    List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
                    sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).ToList();
                    if (sessionstatus.Count > 0)
                    {
                        if (Convert.ToString(sessionstatus.FirstOrDefault().percentcomplete) != "")
                        {
                            sess.completionpercentage = Convert.ToDecimal(sessionstatus.FirstOrDefault().percentcomplete);
                        }
                        else
                        {
                            sess.completionpercentage = 0;
                        }

                    }
                    else
                    {
                        sess.completionpercentage = 0;
                    }

                    if (m.Where(o => o.ttlm_ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                    {
                        ismeetingavailable = 1;
                    }
                    else
                    {
                        ismeetingavailable = 0;
                    }


                    if (TESTS.Where(o => o.sessionid.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                    {
                        testparticipantid = TESTS.Where(o => o.sessionid.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).FirstOrDefault().participantstatus;
                    }
                    else
                    {
                        testparticipantid = "";
                    }
                    List<DisplayInfo> sessionActiondisplay = new List<DisplayInfo>();
                    foreach (int value in intActionArray)
                    {
                        DisplayInfo DI = new DisplayInfo();
                        DI.key = value.ToString();
                        DI.name = Enum.GetName(typeof(CommonEnum.SESSION_LIST_ACTIONS), value);
                        DI.value = SessionDB.SESSION_DISPLAY_ACTION(usertype, Convert.ToInt32(trgdetail.trg_type), Convert.ToInt32(sess.ttttt_type), Convert.ToInt32(sess.ttttt_status), value, Convert.ToInt32(sess.ttttt_complimentory), ismeetingavailable, iscdLogin, participantstatus, testparticipantid, sess.completiontype?.id.ToString(), sess.completionpercentage);
                        sessionActiondisplay.Add(DI);
                    }
                    sess.ActionInfos = sessionActiondisplay.ToArray();




                }

            }


            //*************

            if (pagetype != 0)
            {
                string[] strarr = CommonEnum.Page_Allowed_Session_Type(pagetype).Split(",".ToCharArray());
                int[] arr = Array.ConvertAll(strarr, int.Parse);
                s = s.Where(o => arr.Contains(o.ttttt_type)).ToList();

            }

            //***********Get Trg Setting
            Trg_Setting TS = new Trg_Setting();
            session_setting sessionSetting = new session_setting();
            TS.Session = sessionSetting;
            if (trgdetail.trg_Setting != null)
            {
                if (trgdetail.trg_Setting.Session != null)
                {
                    if (trgdetail.trg_Setting.Session.SessionEntry != null)
                    {
                        List<DisplayInfo> di = new List<DisplayInfo>();
                        if (trgdetail.trg_Setting.Session.SessionEntry.Module == true)
                        {
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "1", name = "Module", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Week == true)
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "2", name = "Week", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Date == true)
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.Day == true)
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "4", name = "Day", value = false });
                        }
                        if (trgdetail.trg_Setting.Session.SessionEntry.srno == true)
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                        }
                        else
                        {
                            di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                        }
                        TS.Session.SessionOrder = trgdetail.trg_Setting.Session.SessionOrder;


                    }
                }
            }
            else
            {
                if (trgdetail.isSelfPaced == 1)
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
                    TS.Session.SessionOrder = "1,4,2,5,3";
                }
                else
                {
                    List<DisplayInfo> di = new List<DisplayInfo>();
                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
                    di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
                    di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
                    TS.Session.SessionOrder = "1,2,4,3,5";

                }
            }

            s = s.Where(o => o.ttttt_status != "9").ToList();
            s = CommonEnum.OrderSessionData(TS.Session.SessionOrder, s);



            //****************Get Session Restriction data
            SessionDB sdb = new SessionDB(_configuration);
            List<Session> slp = sdb.Get_Trg_Progress_Data(trainingid, userid);
            SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
            SessionBL sbl = new SessionBL(_configuration);
            foreach (Session sessn in s)
            {
                sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata);
            }


            //**************Now logic to get participant next session

            Session activeSession=null;
            foreach (Session sessn in s)
            {
                if (sessn.is_Session_Restricted == false)
                {
                    if(sessn.completionpercentage == 0)
                    {
                        if (activeSession == null)
                        {
                            activeSession = sessn;
                        }
                    }
                 
                }
             
            }



            //***************

            return Ok(activeSession);
        }
    }
}
