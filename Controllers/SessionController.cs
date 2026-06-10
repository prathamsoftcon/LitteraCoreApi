using Azure.Core.Pipeline;
using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        [SwaggerOperation("To get user related sessions.")]
        public IActionResult UserSessions(string usertype, string userid, DateTime trg_startdate, DateTime trg_enddate, [FromQuery] PaginationParam param, DateTime? SessionDate = null)
        {

            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_User_Session(usertype, userid, trg_startdate, trg_enddate, SessionDate);
            var result = Paging.GetPagedData(param, s);
            return Ok(result);
        }


        [HttpGet]
        [Authorize(Policy = "PublicApiKey")]
        [Route("api/TrgSessions")]
        [SwaggerOperation("To get training sessions.")]
        public IActionResult TrgSessions(string trainingid,int pagetype=0,string usertype=null,string userid=null,string branchid=null)
        {
           
            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s=new List<Session>();
            //********Filter faculty data only
            if (usertype != null)
            {
                if (usertype == "4")
                {
                    s = cbl.Get_Session_Data_By_Trg(trainingid,userid);
                }
                else
                {
                    s = cbl.Get_Session_Data_By_Trg(trainingid);
                    if (usertype == "6")  // This is an extra usertype which is get from front end to differentiate to get all session for faculty
                    {
                        usertype = "4";
                    }
                }
            }
            else
            {
                s = cbl.Get_Session_Data_By_Trg(trainingid);
            }

           


         
         

            //*****
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
            int? participantstatus = 0;
            string testparticipantid = "";
            int ismeetingavailable = 0;

            if (usertype != null)
            {
                List<Test> TESTS = new List<Test>();
                if (Convert.ToInt16(usertype)  == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    ParticipantDB PDB = new ParticipantDB(_configuration);
                    List<Participant> pl = new List<Participant>();
                    pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid,userid, branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
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
                    TESTS = tbl.Get_trg_test_List_on_session(userid, usertype, trainingid);

                   

                }
                if(userid != null)
                {
                    if (trgdetail.CourseDirector.ToString().ToUpper() == userid.ToString().ToUpper() || trgdetail.AssociateDirector.ToString().ToUpper() == userid.ToString().ToUpper())
                    {
                        iscdLogin = 1;
                    }
                }
                else
                {
                    iscdLogin = 0;
                }
              


                SessionDB SDB = new SessionDB(_configuration);
                List<SessionCompletionStatus> status = new List<SessionCompletionStatus>();
                if(userid != null)
                {
                    if (userid.ToString() != "")
                    {
                        status = SDB.Get_Session_Status_vr1(trainingid, usertype, userid, startdate, enddate, branchid);
                    }
                }



                //***********Extra condition in case of faculty to get mentors slot count
              
                List<Mentor_slot> mss = new List<Mentor_slot>();
                if (Convert.ToInt16(usertype) == (int)CommonEnum.usertype.FACULTY)
                {
                    MentorDB mdb = new MentorDB(_configuration);
                    mss = mdb.Get_Mentor_Session_Slots(trainingid, null, userid, 1);
                }

                //***********


                foreach (Session sess in s)
                {
                    List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
                    sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper() && o.iscompleted == 1).ToList();
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


                    int mentors_session_slots = 0;
                    mentors_session_slots = mss.Where(o => o.ttsl_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).ToList().Count;

                    List<DisplayInfo> sessionActiondisplay = new List<DisplayInfo>();
                    foreach (int value in intActionArray)
                    {
                        DisplayInfo DI = new DisplayInfo();
                        DI.key = value.ToString();
                        DI.name = Enum.GetName(typeof(CommonEnum.SESSION_LIST_ACTIONS), value);
                        DI.value = SessionDB.SESSION_DISPLAY_ACTION(usertype, Convert.ToInt32(trgdetail.trg_type), Convert.ToInt32(sess.ttttt_type), Convert.ToInt32(sess.ttttt_status), value, Convert.ToInt32(sess.ttttt_complimentory), ismeetingavailable,iscdLogin,participantstatus,testparticipantid, sess.completiontype?.id.ToString(), sess.completionpercentage, mentors_session_slots);
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

            List<Session> slp = new List<Session>();

            if (usertype !=null)
            {
                slp = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid, branchid);
            }
         


            SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
            SessionBL sbl=new SessionBL(_configuration);


            //***Code to update competiontype 
            foreach(Session ss in slp)
            {
                if (s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                {
                    ss.completiontype = s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToUpper()).FirstOrDefault().completiontype;
                }
            
             }
           


            //**********
        

            foreach (Session sessn in s) {
             
                string completion_typeid = "1";
                if(sessn.completiontype != null)
                {
                    if(sessn.completiontype.id != null)
                    {
                        completion_typeid = sessn.completiontype.id.ToString();
                    }
                }
                if(usertype != null)
                {
                    sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata, completion_typeid);
                    if (sessn.ActionInfos.Where(o => o.key == "7").FirstOrDefault().value == true)
                    {
                        sessn.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value = false;
                    }
                    //Extra condition in case of bhoj to handle feedback not required for session =1
                    //if (sessn.ttttt_session_no == 1 || sessn.ttttt_type == 10)
                    //{
                    //    sessn.is_feedback_Required = 0;
                    //}
                    //Comment above part because as discussion this is not a right way to stop feedback
                  
                }
                else
                {
                    sessn.is_Session_Restricted = false;
                }
              

                //Extra condition in case of bhoj to hide littera room from self test session
              



            }
            //**********




            return Ok(s);   
        }


        [HttpPost]
        [Route("api/SessionNotes")]
        [SwaggerOperation("To save session notes.")]
        public IActionResult SaveSessionNotes([FromBody] Notes notes)
        {
            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.Save_Notes(notes);

            return Ok();
        }
        [HttpPost]
        [Route("api/UpdateNotes")]
        [SwaggerOperation("To update session notes.")]
        public IActionResult UpdateNotes([FromBody] Notes notes)
        {
            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.update_session_notes(notes);

            return Ok();
        }


        [HttpGet]
        [Route("api/SessionNotes")]
        [SwaggerOperation("To get session notes.")]
        public IActionResult GetSessionNotes(string userid, string trainingid = null, string sessionid = null)
        {
            List<Notes> N = new List<Notes>();
            SessionBL SDB = new SessionBL(_configuration);
            N = SDB.Get_Session_Notes(userid, trainingid, sessionid);

            return Ok(N);
        }

        [HttpGet]
        [Route("api/Comment")]
        [SwaggerOperation("To get comments.")]
        public IActionResult Comment(string trainingid = null, string sessionid = null, string userid=null)
        {
            List<TrgComment> c = new List<TrgComment>();
            SessionBL sdb = new SessionBL(_configuration);
            c = sdb.Get_Trg_Comments(trainingid, sessionid);
            if (userid != null)
            {
                c = c.Where(comment =>
      comment.tttcm_created_by.ToString().ToUpper() == userid.ToString().ToUpper() ||
      (comment.comments != null && comment.comments.Any(reply => reply.tttcr_replied_by.ToString().ToUpper() == userid.ToString().ToUpper()))
  ).ToList();
            }
           
            return Ok(c);
        }
        [HttpPost]
        [Route("api/Comment")]
        [SwaggerOperation("To save comments.")]
        public IActionResult SaveComment([FromBody]TrgComment C)
        {
            List<TrgComment> c = new List<TrgComment>();
            SessionBL sdb = new SessionBL(_configuration);
            var issaved= sdb.Save_Trg_Comment(C);
            return Ok(issaved);
        }
        [HttpPost]
        [Route("api/Comment_Reply")]
        [SwaggerOperation("To save comment reply.")]
        public IActionResult SaveComment_reply([FromBody] TrgComment_reply r)
        {
            List<TrgComment> c = new List<TrgComment>();
            SessionBL sdb = new SessionBL(_configuration);
            var issaved = sdb.Save_Trg_Comment_reply(r);
            return Ok(issaved);
        }


       


        [HttpPost]
        [Route("api/SaveContentFeedback")]
        [SwaggerOperation("To save content feedback.")]
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

        //[HttpPost]
        //[Route("api/Update_Session_Status_old")]
        //public IActionResult Update_Session_Status_old(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status,[FromBody] contents_status_list cl = null)
        //{
        //    Session_Content_Status[] cs = null;
        //    if (cl != null)
        //    {
        //        cs = cl.Session_Content_Status;
        //    }
           
        //    SessionBL SDB = new SessionBL(_configuration);
        //    session_completion_rule r = new session_completion_rule();
        //    r = SDB.session_completion_rule();
        //    if (status == 1)
        //    {
        //        if (r.all_content_completion_mandatory == 1)
        //        {
        //            if (cs != null)
        //            {
        //                if (cs.Where(o => o.is_completed == 0).Count() > 0)
        //                {
        //                    throw new Exception("Please read all content first");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if(cs != null)
        //            {
        //                foreach (Session_Content_Status c in cs)
        //                {
        //                    c.is_completed = 1;
        //                }
        //            }
                   
        //        }
        //    }

        //    if (status == 1)
        //    {
        //        List<user_session_status> completiondata = SDB.Get_Participant_session_status(Participantid, trainingid, Sessionid);
        //        if (completiondata.Count > 0)
        //        {
        //            cs = completiondata.FirstOrDefault().contentstatus;
        //        }
               
        //    }

        //    //extra condition in case of status=0 means that content completion then update/insert content entry this happened from single share
        //    if (status == 0)
        //    {
        //        List<user_session_status> completiondata = SDB.Get_Participant_session_status(Participantid, trainingid, Sessionid);
        //        List<Session_Content_Status> scs = completiondata.FirstOrDefault().contentstatus.ToList();
        //        if (cs.Count() > 0)
        //        {
        //            if(scs.Where(o => o.ttsam_id.ToString().ToUpper() == cs.FirstOrDefault().ttsam_id.ToString().ToUpper()).Count() > 0)
        //            {
        //                var itemToUpdate = scs.FirstOrDefault(o => o.ttsam_id.ToString().ToUpper() == cs.FirstOrDefault().ttsam_id.ToString().ToUpper());
        //                if (itemToUpdate != null)
        //                {
        //                    itemToUpdate.is_completed = cs.FirstOrDefault().is_completed;
                            
        //                }
        //            }
        //            else
        //            {
        //                scs.Add(new Session_Content_Status { sessionid = cs.FirstOrDefault().sessionid, ttsam_id = cs.FirstOrDefault().ttsam_id, is_completed = cs.FirstOrDefault().is_completed });
        //            }
        //        }
        //        cs = scs.ToArray();

        //        ContentBL CBL = new ContentBL(_configuration);
        //        PagedResult<Content> AL = new PagedResult<Content>();
        //        PaginationParam p = new PaginationParam();
        //        AL = CBL.Get_Trg_Content(trainingid, Sessionid, null, p);
        //        int is_All_completed = 1;
        //        if( AL != null)
        //        {
        //          foreach(Content c in AL.Items)
        //            {
        //                if (cs.Where(o => o.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()).Count() > 0)
        //                {
        //                    if(cs.Where(o => o.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()).FirstOrDefault().is_completed != 1)
        //                    {
        //                        is_All_completed = 0;
        //                    }
        //                }
        //            }
        //        }
        //        if (is_All_completed == 1)
        //        {
        //            status = 1;
        //        }
        //    }
        //    bool issaved = SDB.Update_Session_Status(Participantid,trainingid,Sessionid,timeonsession,branchid,status, cs);

        //    return Ok(issaved);
        //}


        [HttpPost]
        [Route("api/Update_Session_Status")]
        [SwaggerOperation("To update participat session status.")]
        public IActionResult Update_Session_Status(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status, [FromBody] contents_status_list cl = null)
        {
            //*****************************************
            //Get configuration for session completion
           



            //if completion required on any one content
            //Check in given content status if any one is completed
            // set session status=1
            //Get all session content and update status 1 and save.


            //if completion required on all content
            //Get session content status (if content status not found then this will return all content with completion =0
            //update given content status in above 
            //check if all session is completed 
            //set session status =1
            //Update session status


            //Exception Condition
            // if session status=1 and cl is null as per discussion no any case there raise error by backend
            //**************************

            //**************Code started


            //Get configuration for session completion
            SessionBL sbl = new SessionBL(_configuration);
            int? session_completion_on_any_one_content = sbl.session_completion_on_content(trainingid).Session_Completion_on_any_one_content;



            //Get session content status (if content status not found then this will return all content with completion =0
          
            List<user_session_status> sessioncontent = new List<user_session_status>();
            sessioncontent = sbl.Get_Participant_session_status(Participantid, trainingid, Sessionid);
            Session_Content_Status[] sessioncontent_status = sessioncontent.FirstOrDefault().contentstatus;
            //update given content status in above 
            foreach (Session_Content_Status c in sessioncontent_status)
            {
                if (cl?.Session_Content_Status.Where(o => o.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()).Count() > 0)
                {
                    c.is_completed = cl.Session_Content_Status.Where(o => o.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()).FirstOrDefault().is_completed;
                }
            }
            //if completion required on any one content
            if (session_completion_on_any_one_content == 1)
            {
                //Check in given content status if any one is completed
                if (sessioncontent_status.Where(o => o.is_completed == 1).Count() > 0)
                {
                    // set session status=1
                    status = 1;
                    // Also set here all content status=1
                    Array.ForEach(sessioncontent_status, x => x.is_completed = 1);

                }
            }
            else if(session_completion_on_any_one_content == 0)
            {
                //check if all session is completed (no anyone incomplete)
                if (sessioncontent_status.Where(o => o.is_completed == 0).Count() <= 0)
                {
                    //set session status =1
                    status = 1;
                }
            }
            //Update session status
            bool issaved = sbl.Update_Session_Status(Participantid, trainingid, Sessionid, timeonsession, branchid, status, sessioncontent_status);

            return Ok(issaved);



            //Exception Condition
            // if session status=1 and cl is null as per discussion no any case there raise error by backend

            //**********************

            
        }
        [Authorize(Policy = "PublicApiKey")]
        [HttpPost]
        [Route("api/Update_Session_Status_wk")]
        [SwaggerOperation("To update participat session status.")]
        public IActionResult Update_Session_Status_wk(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status, [FromBody] contents_status_list cl = null)
        {
            //*****************************************
            //Get configuration for session completion




            //if completion required on any one content
            //Check in given content status if any one is completed
            // set session status=1
            //Get all session content and update status 1 and save.


            //if completion required on all content
            //Get session content status (if content status not found then this will return all content with completion =0
            //update given content status in above 
            //check if all session is completed 
            //set session status =1
            //Update session status


            //Exception Condition
            // if session status=1 and cl is null as per discussion no any case there raise error by backend
            //**************************

            //**************Code started


            //Get configuration for session completion
            SessionBL sbl = new SessionBL(_configuration);
            int? session_completion_on_any_one_content = sbl.session_completion_on_content(trainingid).Session_Completion_on_any_one_content;



            //Get session content status (if content status not found then this will return all content with completion =0

            List<user_session_status> sessioncontent = new List<user_session_status>();
            sessioncontent = sbl.Get_Participant_session_status(Participantid, trainingid, Sessionid);
            Session_Content_Status[] sessioncontent_status = sessioncontent.FirstOrDefault().contentstatus;
            //update given content status in above 
            foreach (Session_Content_Status c in sessioncontent_status)
            {
                if (cl?.Session_Content_Status.Where(o => o.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()).Count() > 0)
                {
                    c.is_completed = cl.Session_Content_Status.Where(o => o.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()).FirstOrDefault().is_completed;
                }
            }
            //if completion required on any one content
            if (session_completion_on_any_one_content == 1)
            {
                //Check in given content status if any one is completed
                if (sessioncontent_status.Where(o => o.is_completed == 1).Count() > 0)
                {
                    // set session status=1
                    status = 1;
                    // Also set here all content status=1
                    Array.ForEach(sessioncontent_status, x => x.is_completed = 1);

                }
            }
            else if (session_completion_on_any_one_content == 0)
            {
                //check if all session is completed (no anyone incomplete)
                if (sessioncontent_status.Where(o => o.is_completed == 0).Count() <= 0)
                {
                    //set session status =1
                    status = 1;
                }
            }
            //Update session status
            bool issaved = sbl.Update_Session_Status(Participantid, trainingid, Sessionid, timeonsession, branchid, status, sessioncontent_status);

            return Ok(issaved);



            //Exception Condition
            // if session status=1 and cl is null as per discussion no any case there raise error by backend

            //**********************


        }

        [HttpGet]
        [Route("api/CHECK_SESSION_FEEBDACK")]
        [SwaggerOperation("To check user feedback exist or not on session.")]
        public IActionResult CHECK_SESSION_FEEBDACK(string userid, string trainingid, string sessionid,string branchid=null)
        {
            bool isFeedbackExist = false;
            SessionBL SDB = new SessionBL(_configuration);
            SessionDB db=new SessionDB(_configuration);
            List<Session> completiondata = db.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid, branchid);
            completiondata = completiondata.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).ToList();
            if (completiondata.Count > 0)
            {
               
                if (completiondata.FirstOrDefault().noofcompletion == 1)
                {
                    isFeedbackExist = true;
                }
                else
                {
                    isFeedbackExist = SDB.Check_Content_Feedback_Exists(userid, trainingid, sessionid);
                }
            }
            else
            {
                 isFeedbackExist = SDB.Check_Content_Feedback_Exists(userid, trainingid, sessionid);
            }

           

            return Ok(isFeedbackExist);
        }

        [HttpGet]
        [Route("api/CHECK_SESSION_COMPLETION_STATUS")]
        [SwaggerOperation("To check session completion status.")]
        public IActionResult CHECK_SESSION_COMPLETION_STATUS(string userid, string trainingid, string sessionid)
        {
            List<user_session_status> statusdata = new List<user_session_status>();
            SessionBL SDB = new SessionBL(_configuration);
            statusdata = SDB.Get_Participant_session_status(userid, trainingid, sessionid);
            return Ok(statusdata);
        }
        [HttpGet]
        [Route("api/SESSION_MEETINGS")]
        [SwaggerOperation("To get session meeting.")]
        public IActionResult SESSION_MEETINGS(string sessionid)
        {
            List<Meeting> meetings = new List<Meeting>();
            SessionBL SDB = new SessionBL(_configuration);
            meetings = SDB.Get_Session_Meetings(sessionid);
            return Ok(meetings);
        }

        [HttpGet]
        [Route("api/CHECK_SESSION_RESTRICTION")]
        [SwaggerOperation("To check session restriction.")]
        public IActionResult CHECK_SESSION_RESTRICTION(string usertype, string userid, string sessionid, string trainingid,string branchid=null)
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
                    List<Session> sl = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid,branchid);

                    List<Session> allsession = sdb.Get_Session_Data_By_Trg(trainingid);
                    //***Code to update competiontype 
                    foreach (Session ss in sl)
                    {
                        if (allsession.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                        {
                            ss.completiontype = allsession.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToUpper()).FirstOrDefault().completiontype;
                        }

                    }

                  //**********





                    Session opensessiondetail = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).FirstOrDefault();
                    List<Session> SL = sl;
                    SL = SL.Where(o => o.ttttt_type != (int)Common.CommonEnum.SESSION_TYPE.Breaks).ToList();
                    // SL = SL.Where(o => o.ttttt_session_no != 0).ToList();
                    //****************Condition to relax check on complementory session
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
                        if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test)
                        {
                            if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (o.completiontype != null && o.completiontype.id.ToString() != "2")).Count() <= 0)
                            {
                                //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test ) && (o.completiontype != null && o.completiontype.id.ToString() != "2")).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }

                        }
                        else if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Practical)
                        {
                            string completiontypeid = "1";
                            if(opensessiondetail.completiontype != null)
                            {
                                completiontypeid = opensessiondetail.completiontype.id.ToString();
                            }
                            if (opensessiondetail.ttttt_complimentory == 1)
                            {

                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }
                            else
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1  && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Practical && completiontypeid.ToString() != "2").Count() <= 0)
                                {

                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                    {
                                        isrestricted = false;
                                    }
                                }
                               
                            }
                            //    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Practical && o.completiontype.id.ToString() != "2").Count() <= 0)
                            //{
                            //    //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                            //    //if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment)).Count() <= 0)
                            //    //{
                            //    //    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
                            //    //    {
                            //    //        if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && o.noofcompletion != 1).Count() <= 0)
                            //    //        {
                            //    //            isrestricted = false;
                            //    //        }

                            //    //        // isrestricted = false;
                            //    //    }
                            //    //}
                            //    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                            //    {
                            //        isrestricted = false;
                            //    }
                            //}
                        }
                        else
                        {
                            if (opensessiondetail.ttttt_complimentory == 1)
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }
                            else
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 ).Count() <= 0)
                                {
                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                    {
                                        isrestricted = false;
                                    }
                                }
                            }

                        }

                    }
                    else if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.Week)
                    {
                        if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test)
                        {
                            if (SL.Where(o => Convert.ToInt32(o.ttttt_session_week) < Convert.ToInt32(opensessiondetail.ttttt_session_week) && o.noofcompletion != 1 ).Count() <= 0)
                            {
                                //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test )).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }

                        }
                        else
                        {
                            if (SL.Where(o => Convert.ToInt32(o.ttttt_session_week) < Convert.ToInt32(opensessiondetail.ttttt_session_week) && o.noofcompletion != 1 ).Count() <= 0)
                            {
                                isrestricted = false;
                            }
                        }
                    }
                    else if (restrictiondata.restrictionon == (int)CommonEnum.SessionEntryControl.Module)
                    {
                        if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test )
                        {
                            if (SL.Where(o => Convert.ToInt32(o.module) < Convert.ToInt32(opensessiondetail.module) && o.noofcompletion != 1 ).Count() <= 0)
                            {
                                //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test )).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }

                        }
                        else
                        {
                            if (SL.Where(o => Convert.ToInt32(o.module) < Convert.ToInt32(opensessiondetail.module) && o.noofcompletion != 1).Count() <= 0)
                            {
                                isrestricted = false;
                            }
                        }
                    }
                    //if (opensessiondetail.ttttt_complimentory == 1)
                    //{
                    //    isrestricted = false;
                    //}
                    //else
                    //{

                    //}



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
        [SwaggerOperation("To get different session types.")]
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
        [SwaggerOperation("To get session modules.")]
        public IActionResult SessionModules()
        {
            List<SessionModule> modules = new List<SessionModule>();
            SessionBL SDB = new SessionBL(_configuration);
            modules = SDB.Get_Session_Module();
            return Ok(modules);
        }

        [HttpPost]
        [Route("api/Session")]
        [SwaggerOperation("To save session.")]
        public IActionResult SaveSession([FromBody] CreateSessionDTO session)
        {
            
            SessionBL sdb = new SessionBL(_configuration);
            var issaved = sdb.Save_Session(session);
            return Ok(issaved);
        }
        [HttpGet]
        [Route("api/GET_SESSION_ENTRY_CONTROLS")]
        [SwaggerOperation("To get session entry controls on basis of training type.")]
        public IActionResult GET_SESSION_ENTRY_CONTROLS(int trainingtype)
        {
            SessionEntry E = new SessionEntry();
            E = SessionBL.GET_SESSION_ENTRY_CONTROLS(trainingtype);
            
            return Ok(E);
        }


        [HttpGet]
        [Route("api/TRG_SESSIONS_DURATION")]
        [SwaggerOperation("To get session summary of particular training.")]
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
        [SwaggerOperation("To get participant next session.")]
        public IActionResult GET_PARTICIPANT_NEXT_SESSION(string trainingid,string participantid,string branchid=null)
        {
            string userid = participantid;
            int pagetype = 0;
            string usertype = "5";
            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_Session_Data_By_Trg(trainingid);
            if (s.Count <= 0)
            {
                return NotFound(new { message = "No any session available in this training,Please contact to admin" });
             
            }


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
            int? participantstatus = 0;
            string testparticipantid = "";
            int ismeetingavailable = 0;

            if (usertype != null)
            {
                List<Test> TESTS = new List<Test>();
                if (Convert.ToInt16(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    ParticipantDB PDB = new ParticipantDB(_configuration);
                    List<Participant> pl = new List<Participant>();
                    pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid,userid,branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
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
                    status = SDB.Get_Session_Status_vr1(trainingid, usertype, userid, startdate, enddate, branchid);
                }





                foreach (Session sess in s)
                {
                    sess.participant_trg_status = participantstatus;
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
            List<Session> slp = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid, branchid);
            SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
            SessionBL sbl = new SessionBL(_configuration);


            //***Code to update competiontype 
            foreach (Session ss in slp)
            {
                if (s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                {
                    ss.completiontype = s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToUpper()).FirstOrDefault().completiontype;
                }

            }

            //**********



            foreach (Session sessn in s)
            {
                string completion_typeid = "1";
                if (sessn.completiontype != null)
                {
                    if (sessn.completiontype.id != null)
                    {
                        completion_typeid = sessn.completiontype.id.ToString();
                    }
                }
                sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata, completion_typeid);

                ////Extra condition in case of bhoj to handle feedback not required for session =1
                //if (sessn.ttttt_session_no == 1 || sessn.ttttt_type == 10 || completion_typeid == "2")
                //{
                //    sessn.is_feedback_Required = 0;
                //}
            }


            //**************Now logic to get participant next session

            Session activeSession=null;
            int is_session_not_restricted = 0;
            if (trgdetail.trg_Setting.Session.SessionRestriction != null)
            {
                if (trgdetail.trg_Setting.Session.SessionRestriction.isrestricted == 0)
                {
                    is_session_not_restricted = 1;
                }
               
            }

            foreach (Session sessn in s)
            {
                if (sessn.is_Session_Restricted == false)
                {
                    if(sessn.completionpercentage == 0)
                    {
                        //if(sessn.ttttt_type==6 || sessn.ttttt_type == 7)
                        if (sessn.ttttt_type == 7)
                        {
                                                     
                            if(participantstatus == 1)
                            {
                                // Check extra condition for Attempted
                                EvalDB edb = new EvalDB(_configuration);
                                string testid = edb.Get_Test_Session_Mapping_Data_By_Session(sessn.ttttt_session_id).testid;
                                //Check status
                                bool is_test_attempted = edb.Check_test_participant_status(testid,participantid);
                                //If entry not found then run else not
                                if (is_test_attempted == false)
                                {
                                    activeSession = sessn;
                                    if (is_session_not_restricted == 1)
                                    {
                                        break;
                                    }
                                }
                          
                       


                              
                            }
                            
                        }
                        else if(sessn.ttttt_type == 6) //Assignment
                        {
                            if (activeSession == null)
                            {
                                if (participantstatus == 1 || sessn.ttttt_complimentory==1)
                                {
                                    activeSession = sessn;
                                    if (is_session_not_restricted == 1)
                                    {
                                        break;
                                    }
                                }
                               
                            }
                        }
                        else
                        {
                            if (sessn.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value == true)
                            {
                                if (activeSession == null)
                                {
                                    activeSession = sessn;
                                    if (is_session_not_restricted == 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                     
                       
                    }
                 
                }

            }

            //string completion_typeid = "1";
            //if (activeSession.completiontype != null)
            //{
            //    if (activeSession.completiontype.id != null)
            //    {
            //        completion_typeid = activeSession.completiontype.id.ToString();
            //    }
            //}
            if (activeSession != null)
            {
                if (activeSession.ActionInfos.Where(o => o.key == "7").FirstOrDefault().value == true)
                {
                    activeSession.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value = false;
                }
            }
           

            //***************

            return Ok(activeSession);
        }


        //[HttpGet]
        //[Route("api/GET_PARTICIPANT_NEXT_SESSION_V3")]
        //public IActionResult GET_PARTICIPANT_NEXT_SESSION_V3(string trainingid, string participantid, string branchid = null)
        //{
        //    string userid = participantid;
        //    int pagetype = 0;
        //    string usertype = "5";
        //    SessionBL cbl = new SessionBL(_configuration);
        //    List<Session> s = new List<Session>();
        //    s = cbl.Get_Session_Data_By_Trg(trainingid);
        //    //*********Get Training Setting Detail
        //    TrainingDB WDB = new TrainingDB(_configuration);
        //    Training trgdetail = new Training();
        //    trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
        //    string startdate = trgdetail.StartDate?.ToString("yyyy/MM/dd");
        //    string enddate = trgdetail.T_EndDate?.ToString("yyyy/MM/dd");



        //    MeetingDB mbl = new MeetingDB(_configuration);
        //    List<Meeting> m = new List<Meeting>();
        //    m = mbl.Get_Trg_Meetings(trainingid);

        //    foreach (Session sl in s)
        //    {
        //        List<Meeting> lm = new List<Meeting>();
        //        lm = m.Where(o => o.ttlm_ttttt_session_id.ToString().ToUpper() == sl.ttttt_session_id.ToString().ToUpper()).ToList();
        //        sl.meeting = lm.ToArray();
        //    }

        //    CommonEnum.SESSION_LIST_ACTIONS[] enumActionArray = (CommonEnum.SESSION_LIST_ACTIONS[])Enum.GetValues(typeof(CommonEnum.SESSION_LIST_ACTIONS));
        //    int[] intActionArray = Array.ConvertAll(enumActionArray, v => (int)v);
        //    //**************
        //    if (trgdetail.trg_Setting != null)
        //    {
        //        if (trgdetail.trg_Setting.Session != null)
        //        {
        //            if (trgdetail.trg_Setting.Session.SessionEntry != null)
        //            {
        //                List<DisplayInfo> di = new List<DisplayInfo>();
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Module == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "1", name = "Module", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Week == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "2", name = "Week", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Date == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Day == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "4", name = "Day", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.srno == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
        //                }

        //                foreach (Session sess in s)
        //                {
        //                    sess.displayInfos = di.ToArray();
        //                    sess.DisplayOrder = trgdetail.trg_Setting.Session.SessionOrder;
        //                }

        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (trgdetail.isSelfPaced == 1)
        //        {
        //            List<DisplayInfo> di = new List<DisplayInfo>();
        //            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
        //            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
        //            di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
        //            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
        //            di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
        //            foreach (Session sess in s)
        //            {
        //                sess.displayInfos = di.ToArray();
        //                sess.DisplayOrder = "1,4,2,5,3";
        //            }
        //        }
        //        else
        //        {
        //            List<DisplayInfo> di = new List<DisplayInfo>();
        //            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
        //            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
        //            di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
        //            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
        //            di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
        //            foreach (Session sess in s)
        //            {
        //                sess.displayInfos = di.ToArray();
        //                sess.DisplayOrder = "1,2,4,3,5";
        //            }
        //        }
        //    }

        //    //**************Attach Action Info
        //    int iscdLogin = 0;
        //    int participantstatus = 0;
        //    string testparticipantid = "";
        //    int ismeetingavailable = 0;

        //    if (usertype != null)
        //    {
        //        List<Test> TESTS = new List<Test>();
        //        if (Convert.ToInt16(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
        //        {
        //            ParticipantDB PDB = new ParticipantDB(_configuration);
        //            List<Participant> pl = new List<Participant>();
        //            pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid, userid, branchid);
        //            pl = pl.Where(o => o.ParticipantId.ToUpper() == userid.ToString().ToUpper()).ToList();
        //            if (pl.Count() > 0)
        //            {
        //                participantstatus = pl.FirstOrDefault().is_approve;
        //            }
        //            else
        //            {
        //                participantstatus = 0;
        //            }

        //            EvalDB tbl = new EvalDB(_configuration);
        //            TESTS = tbl.Get_test_List(usertype, userid);



        //        }

        //        if (trgdetail.CourseDirector.ToString().ToUpper() == userid.ToString().ToUpper() || trgdetail.AssociateDirector.ToString().ToUpper() == userid.ToString().ToUpper())
        //        {
        //            iscdLogin = 1;
        //        }


        //        SessionDB SDB = new SessionDB(_configuration);
        //        List<SessionCompletionStatus> status = new List<SessionCompletionStatus>();
        //        if (userid.ToString() != "")
        //        {
        //            status = SDB.Get_Session_Status_vr1(trainingid, usertype, userid, startdate, enddate, branchid);
        //        }





        //        foreach (Session sess in s)
        //        {
        //            List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
        //            sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).ToList();
        //            if (sessionstatus.Count > 0)
        //            {
        //                if (Convert.ToString(sessionstatus.FirstOrDefault().percentcomplete) != "")
        //                {
        //                    sess.completionpercentage = Convert.ToDecimal(sessionstatus.FirstOrDefault().percentcomplete);
        //                }
        //                else
        //                {
        //                    sess.completionpercentage = 0;
        //                }

        //            }
        //            else
        //            {
        //                sess.completionpercentage = 0;
        //            }

        //            if (m.Where(o => o.ttlm_ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).Count() > 0)
        //            {
        //                ismeetingavailable = 1;
        //            }
        //            else
        //            {
        //                ismeetingavailable = 0;
        //            }


        //            if (TESTS.Where(o => o.sessionid.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).Count() > 0)
        //            {
        //                testparticipantid = TESTS.Where(o => o.sessionid.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper()).FirstOrDefault().participantstatus;
        //            }
        //            else
        //            {
        //                testparticipantid = "";
        //            }
        //            List<DisplayInfo> sessionActiondisplay = new List<DisplayInfo>();
        //            foreach (int value in intActionArray)
        //            {
        //                DisplayInfo DI = new DisplayInfo();
        //                DI.key = value.ToString();
        //                DI.name = Enum.GetName(typeof(CommonEnum.SESSION_LIST_ACTIONS), value);
        //                DI.value = SessionDB.SESSION_DISPLAY_ACTION_V3(usertype, Convert.ToInt32(trgdetail.trg_type), Convert.ToInt32(sess.ttttt_type), Convert.ToInt32(sess.ttttt_status), value, Convert.ToInt32(sess.ttttt_complimentory), ismeetingavailable, iscdLogin, participantstatus, testparticipantid, sess.completiontype?.id.ToString(), sess.completionpercentage);
        //                sessionActiondisplay.Add(DI);
        //            }
        //            sess.ActionInfos = sessionActiondisplay.ToArray();




        //        }

        //    }


        //    //*************

        //    if (pagetype != 0)
        //    {
        //        string[] strarr = CommonEnum.Page_Allowed_Session_Type(pagetype).Split(",".ToCharArray());
        //        int[] arr = Array.ConvertAll(strarr, int.Parse);
        //        s = s.Where(o => arr.Contains(o.ttttt_type)).ToList();

        //    }

        //    //***********Get Trg Setting
        //    Trg_Setting TS = new Trg_Setting();
        //    session_setting sessionSetting = new session_setting();
        //    TS.Session = sessionSetting;
        //    if (trgdetail.trg_Setting != null)
        //    {
        //        if (trgdetail.trg_Setting.Session != null)
        //        {
        //            if (trgdetail.trg_Setting.Session.SessionEntry != null)
        //            {
        //                List<DisplayInfo> di = new List<DisplayInfo>();
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Module == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "1", name = "Module", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Week == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "2", name = "Week", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Date == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.Day == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "4", name = "Day", value = false });
        //                }
        //                if (trgdetail.trg_Setting.Session.SessionEntry.srno == true)
        //                {
        //                    di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
        //                }
        //                else
        //                {
        //                    di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
        //                }
        //                TS.Session.SessionOrder = trgdetail.trg_Setting.Session.SessionOrder;


        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (trgdetail.isSelfPaced == 1)
        //        {
        //            List<DisplayInfo> di = new List<DisplayInfo>();
        //            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
        //            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
        //            di.Add(new DisplayInfo { key = "3", name = "Date", value = false });
        //            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
        //            di.Add(new DisplayInfo { key = "5", name = "srno", value = true });
        //            TS.Session.SessionOrder = "1,4,2,5,3";
        //        }
        //        else
        //        {
        //            List<DisplayInfo> di = new List<DisplayInfo>();
        //            di.Add(new DisplayInfo { key = "1", name = "Module", value = true });
        //            di.Add(new DisplayInfo { key = "2", name = "Week", value = true });
        //            di.Add(new DisplayInfo { key = "3", name = "Date", value = true });
        //            di.Add(new DisplayInfo { key = "4", name = "Day", value = true });
        //            di.Add(new DisplayInfo { key = "5", name = "srno", value = false });
        //            TS.Session.SessionOrder = "1,2,4,3,5";

        //        }
        //    }

        //    s = s.Where(o => o.ttttt_status != "9").ToList();
        //    s = CommonEnum.OrderSessionData(TS.Session.SessionOrder, s);



        //    //****************Get Session Restriction data
        //    SessionDB sdb = new SessionDB(_configuration);
        //    List<Session> slp = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid, branchid);
        //    SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
        //    SessionBL sbl = new SessionBL(_configuration);


        //    //***Code to update competiontype 
        //    foreach (Session ss in slp)
        //    {
        //        if (s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToString().ToUpper()).Count() > 0)
        //        {
        //            ss.completiontype = s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToUpper()).FirstOrDefault().completiontype;
        //        }

        //    }

        //    //**********



        //    foreach (Session sessn in s)
        //    {
        //        string completion_typeid = "1";
        //        if (sessn.completiontype != null)
        //        {
        //            if (sessn.completiontype.id != null)
        //            {
        //                completion_typeid = sessn.completiontype.id.ToString();
        //            }
        //        }
        //        sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata, completion_typeid);

        //        ////Extra condition in case of bhoj to handle feedback not required for session =1
        //        //if (sessn.ttttt_session_no == 1 || sessn.ttttt_type == 10 || completion_typeid == "2")
        //        //{
        //        //    sessn.is_feedback_Required = 0;
        //        //}
        //    }


        //    //**************Now logic to get participant next session

        //    Session activeSession = null;
        //    int is_session_not_restricted = 0;
        //    if (trgdetail.trg_Setting.Session.SessionRestriction != null)
        //    {
        //        if (trgdetail.trg_Setting.Session.SessionRestriction.isrestricted == 0)
        //        {
        //            is_session_not_restricted = 1;
        //        }

        //    }

        //    foreach (Session sessn in s)
        //    {
        //        if (sessn.is_Session_Restricted == false)
        //        {
        //            if (sessn.completionpercentage == 0)
        //            {
        //                //if(sessn.ttttt_type==6 || sessn.ttttt_type == 7)
        //                if (sessn.ttttt_type == 7)
        //                {
        //                    if (participantstatus == 1)
        //                    {
        //                        activeSession = sessn;
        //                        if (is_session_not_restricted == 1)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                }
        //                else if (sessn.ttttt_type == 6) //Assignment
        //                {
        //                    if (activeSession == null)
        //                    {
        //                        activeSession = sessn;
        //                        if (is_session_not_restricted == 1)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (sessn.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value == true)
        //                    {
        //                        if (activeSession == null)
        //                        {
        //                            activeSession = sessn;
        //                            if (is_session_not_restricted == 1)
        //                            {
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }


        //            }

        //        }

        //    }

        //    //string completion_typeid = "1";
        //    //if (activeSession.completiontype != null)
        //    //{
        //    //    if (activeSession.completiontype.id != null)
        //    //    {
        //    //        completion_typeid = activeSession.completiontype.id.ToString();
        //    //    }
        //    //}
        //    //if (activeSession != null)
        //    //{
        //    //    if (activeSession.ActionInfos.Where(o => o.key == "7").FirstOrDefault().value == true)
        //    //    {
        //    //        activeSession.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value = false;
        //    //    }
        //    //}


        //    //***************

        //    return Ok(activeSession);
        //}



        [HttpGet]
        [Route("api/CHECK_PREVIOUS_SESSION_FOR_COMPLETION")]
        [SwaggerOperation("To check session for completion.")]
        public IActionResult CHECK_PREVIOUS_SESSION_FOR_COMPLETION(string trainingid,string sessionid,int pagetype = 0, string usertype = null, string userid = null,string branchid=null)
        {

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
            int? participantstatus = 0;
            string testparticipantid = "";
            int ismeetingavailable = 0;

            if (usertype != null)
            {
                List<Test> TESTS = new List<Test>();
                if (Convert.ToInt16(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    ParticipantDB PDB = new ParticipantDB(_configuration);
                    List<Participant> pl = new List<Participant>();
                    pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid,userid, branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
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
                    status = SDB.Get_Session_Status_vr1(trainingid, usertype, userid, startdate, enddate, branchid);
                }





                foreach (Session sess in s)
                {
                    List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
                    sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper() && o.iscompleted==1).ToList();
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
            List<Session> slp = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid, branchid);
            SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
            SessionBL sbl = new SessionBL(_configuration);
            foreach (Session sessn in s)
            {
                string completion_typeid = "1";
                if (sessn.completiontype != null)
                {
                    if (sessn.completiontype.id != null)
                    {
                        completion_typeid = sessn.completiontype.id.ToString();
                    }
                }
                sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata, completion_typeid);


                //Extra condition in case of bhoj to hide littera room from self test session
                if (sessn.ActionInfos.Where(o => o.key == "7").FirstOrDefault().value == true)
                {
                    sessn.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value = false;
                }
                ////Extra condition in case of bhoj to handle feedback not required for session =1
                //if (sessn.ttttt_session_no == 1 || sessn.ttttt_type == 10 || completion_typeid=="2")
                //{
                //    sessn.is_feedback_Required = 0;
                //}
            }
            //**********


            //*********Logic for previous session check
            string previousSessionId = "";
            int currentIndex = s.FindIndex(s => s.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper());
            if (currentIndex > 0)
            {
                // Get the previous session's ID (index - 1)
                previousSessionId = s[currentIndex - 1].ttttt_session_id;
                
            }
            int isFeedbackOpen = 0;
            Session PreviousSession = null;
            if (previousSessionId != "")
            {

                PreviousSession = s.Where(o => o.ttttt_session_id.ToString().ToUpper() == previousSessionId.ToString().ToUpper()).FirstOrDefault();
                if (PreviousSession.is_Session_Restricted == false)
                {
                    if (PreviousSession.completionpercentage < 100)
                    {
                        //check is visited=true
                        List<user_session_status> statusdata = new List<user_session_status>();
                        SessionBL SDB = new SessionBL(_configuration);
                        statusdata = SDB.Get_Participant_session_status(userid, trainingid, previousSessionId);


                        if (statusdata.Where(o=>o.status=="2").Count()>0)
                        {
                            isFeedbackOpen = 1;
                        }
                    }
                }
            }
           
           



            return Ok(new { isFeedbackOpen = isFeedbackOpen, PreviousSession= PreviousSession });
        }



        [HttpPost]
        [Route("api/Update_Session_visit_Status")]
        [SwaggerOperation("To update session visit status of particular user .")]
        public IActionResult Update_Session_visit_Status(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status)
        {
            SessionBL SDB = new SessionBL(_configuration);
            bool issaved = SDB.Update_Session_Visit_Status(Participantid, trainingid, Sessionid, timeonsession, branchid, status);

            return Ok(issaved);
        }
        [HttpGet]
        [Route("api/Check_Exception")]
        [SwaggerOperation("To check custom exception[Hardcode].")]
        public IActionResult Check_Exception()
        {
            throw new Exception("This is a custom exception message.");
            return Ok(true);
        }

        [Route("api/Get_Consent_msg")]
        [HttpGet]
        [SwaggerOperation("To get conset message on basis of participant status.")]
        public IActionResult Get_Consent_msg(string trainingid,string participantid,string branchid)
        {
            string msg = "";

            SessionDB sdb = new SessionDB(_configuration);
            List<Session> slp = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, participantid, branchid);
            int completed = slp.Where(o => o.noofcompletion == 1).Count();
            int totalsession = slp.Count();
            decimal percentages = (Convert.ToDecimal(completed) / Convert.ToDecimal(totalsession)) * 100;
            if (percentages > 0)
            {
                msg = "Congratulations ! You have successfully completed "+ completed + " session of this course.Would you like to unlock all sessions? <br/>(बधाई हो! आपने इस कोर्स के "+completed +" सत्र को सफलतापूर्वक पूरा किया है। क्या आप सभी सत्रों को अनलॉक करना चाहेंगे?)";
            }
            else
            {
                msg = "Welcome to the course! You currently have limited access to sessions. Would you like to unlock all sessions? <br/>(कोर्स में आपका स्वागत है! आप सीमित सत्रों का ही अध्ययन कर सकते हैं। क्या आप सभी सत्रों को अनलॉक करना चाहेंगे?)";
            }
           

            return Ok(msg);




        }




        [HttpGet]
        [Route("api/TrgSessions_with_content")]
        [SwaggerOperation("To get training all sessions and content.")]
        public IActionResult TrgSessions_with_content(string trainingid, int pagetype = 0, string usertype = null, string userid = null, string branchid = null)
        {

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
            int? participantstatus = 0;
            string testparticipantid = "";
            int ismeetingavailable = 0;

            if (usertype != null)
            {
                List<Test> TESTS = new List<Test>();
                if (Convert.ToInt16(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
                {
                    ParticipantDB PDB = new ParticipantDB(_configuration);
                    List<Participant> pl = new List<Participant>();
                    pl = PDB.Get_TRG_PARTICIPANT_Data(trainingid, userid, branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
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
                    TESTS = tbl.Get_trg_test_List_on_session(userid, usertype, trainingid);



                }
                if (userid != null)
                {
                    if (trgdetail.CourseDirector.ToString().ToUpper() == userid.ToString().ToUpper() || trgdetail.AssociateDirector.ToString().ToUpper() == userid.ToString().ToUpper())
                    {
                        iscdLogin = 1;
                    }
                }
                else
                {
                    iscdLogin = 0;
                }



                SessionDB SDB = new SessionDB(_configuration);
                List<SessionCompletionStatus> status = new List<SessionCompletionStatus>();
                if (userid != null)
                {
                    if (userid.ToString() != "")
                    {
                        status = SDB.Get_Session_Status_vr1(trainingid, usertype, userid, startdate, enddate,branchid);
                    }
                }






                foreach (Session sess in s)
                {
                    List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
                    sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == sess.ttttt_session_id.ToString().ToUpper() && o.iscompleted == 1).ToList();
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
                    //List<DisplayInfo> sessionActiondisplay = new List<DisplayInfo>();
                    //foreach (int value in intActionArray)
                    //{
                    //    DisplayInfo DI = new DisplayInfo();
                    //    DI.key = value.ToString();
                    //    DI.name = Enum.GetName(typeof(CommonEnum.SESSION_LIST_ACTIONS), value);
                    //    DI.value = true;
                    //    //if (SessionDB.Get_Session_Group(sess.ttttt_type) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    //    //{
                    //    //    DI.value = true;
                    //    //}
                    //    //else
                    //    //{
                    //    //    DI.value = SessionDB.SESSION_DISPLAY_ACTION(usertype, Convert.ToInt32(trgdetail.trg_type), Convert.ToInt32(sess.ttttt_type), Convert.ToInt32(sess.ttttt_status), value, Convert.ToInt32(sess.ttttt_complimentory), ismeetingavailable, iscdLogin, participantstatus, testparticipantid, sess.completiontype?.id.ToString(), sess.completionpercentage);
                    //    //}

                    //    sessionActiondisplay.Add(DI);
                    //}
                    //sess.ActionInfos = sessionActiondisplay.ToArray();


                    List<DisplayInfo> sessionActiondisplay = new List<DisplayInfo>();
                    foreach (int value in intActionArray)
                    {
                        DisplayInfo DI = new DisplayInfo();
                        DI.key = value.ToString();
                        DI.name = Enum.GetName(typeof(CommonEnum.SESSION_LIST_ACTIONS), value);
                        DI.value = SessionDB.SESSION_DISPLAY_ACTION_V3(usertype, Convert.ToInt32(trgdetail.trg_type), Convert.ToInt32(sess.ttttt_type), Convert.ToInt32(sess.ttttt_status), value, Convert.ToInt32(sess.ttttt_complimentory), ismeetingavailable, iscdLogin, participantstatus, testparticipantid, sess.completiontype?.id.ToString(), sess.completionpercentage);
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
            int content_feedback_on_session = 1;

            Trg_Setting TS = new Trg_Setting();
            session_setting sessionSetting = new session_setting();
            TS.Session = sessionSetting;
            if (trgdetail.trg_Setting != null)
            {
                if (trgdetail.trg_Setting.Session != null)
                {
                    if(trgdetail.trg_Setting.Session.content_feedback_on_session != null)
                    {
                        content_feedback_on_session = trgdetail.trg_Setting.Session.content_feedback_on_session;
                    }
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

            List<Session> slp = new List<Session>();

            if (usertype != null)
            {
                slp = sdb.Get_Trg_Progress_Data_For_Next_Session(trainingid, userid, branchid);
            }



            SessionRestriction restrictiondata = sdb.GET_SESSION_RESTRICTION_INFO(trainingid);
            SessionBL sbl = new SessionBL(_configuration);


            //***Code to update competiontype 
            foreach (Session ss in slp)
            {
                if (s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToString().ToUpper()).Count() > 0)
                {
                    ss.completiontype = s.Where(o => o.ttttt_session_id.ToString().ToUpper() == ss.ttttt_session_id.ToUpper()).FirstOrDefault().completiontype;
                }

            }



            //**********


            foreach (Session sessn in s)
            {

                string completion_typeid = "1";
                if (sessn.completiontype != null)
                {
                    if (sessn.completiontype.id != null)
                    {
                        completion_typeid = sessn.completiontype.id.ToString();
                    }
                }
                if (usertype != null)
                {
                    sessn.is_Session_Restricted = sbl.Get_Session_Restriction(usertype, sessn.ttttt_session_id, slp, restrictiondata, completion_typeid);
                    if (sessn.ActionInfos.Where(o => o.key == "7").FirstOrDefault().value == true)
                    {
                       // sessn.ActionInfos.Where(o => o.key == "5").FirstOrDefault().value = false;
                    }
                   

                }
                else
                {
                    sessn.is_Session_Restricted = false;
                }

            }
            //**********

            //******************Add session contents
            ContentDB cdb = new ContentDB(_configuration);
            List<Content> cl = new List<Content>();
            PaginationParam param = new PaginationParam();
            cl = cdb.Get_Trg_Content(param, trainingid);
            if (content_feedback_on_session == 1)
            {
                cl.ForEach(c => c.is_feedback_required = 0);
            }
            else
            {
                cl.ForEach(c => c.is_feedback_required = 1);
            }

            cl.ForEach(c => c.is_feedback_required = content_feedback_on_session);

            foreach (Session cs in s)
            {
                Item items = new Item();
                items.Items = cl.Where(o => o.ttsam_ttttt_session_id.ToString().ToUpper() == cs.ttttt_session_id.ToString().ToUpper()).ToArray();
                items.totalRecords = items.Items.Count();
                cs.sessioncontent= items;
                
            }



            return Ok(s);
        }

        [HttpGet]
        [Route("api/CHECK_SESSION_STATUS")]
        [SwaggerOperation("To check session status.")]
        public IActionResult CHECK_SESSION_STATUS(string Participantid, string trainingid = null, string sessionid = null)
        {
            bool isFeedbackExist = false;
            SessionBL SDB = new SessionBL(_configuration);

            List<user_session_status> completiondata = SDB.Get_Participant_session_status(Participantid, trainingid, sessionid);
         


            return Ok(completiondata);
        }


        [HttpGet]
        [Route("api/Get_Test_Session_Mapping_Data")]
        [SwaggerOperation("To get test detail on particular session id in case of test session.")]
        public IActionResult Get_Test_Session_Mapping_Data(string sessionid)
        {

            EvalDB edb = new EvalDB(_configuration);
            TEST_SESSION_MAPPING_DATA s = new TEST_SESSION_MAPPING_DATA();
            s = edb.Get_Test_Session_Mapping_Data_By_Session(sessionid);
            return Ok(s);
        }


        [HttpPost]
        [Route("api/Complete_Activity")]
        [SwaggerOperation("To update complete status of user's activity.")]
        public IActionResult Complete_Activity(string Participantid, string timeonsession, string ttsam_id, int status, [FromBody] contents_status_list cl = null)
        {
            string userid = "";
            string branchid = "";
            user_agency_mapping uam = new user_agency_mapping();
            AgencyDB adb = new AgencyDB(_configuration);
            uam = adb.Get_User_Agency_Mapping_Data(Participantid);
            userid = uam.userid;
            UserBranch b = new UserBranch();
            b = adb.Get_User_Branches(userid);
            if (b.branches.Length > 0)
            {
                branchid = b.branches.FirstOrDefault().branchid;
            }

            contentDetail cd = new contentDetail();
            ContentDB cdb = new ContentDB(_configuration);
            contentDetail cdn = new contentDetail();
            cdn = cdb.Get_Content_Detail(ttsam_id);

            


            string trainingid = cdn.trainingid;
            string Sessionid = cdn.sessionid;
            Update_Session_Status(Participantid, trainingid, Sessionid, timeonsession, branchid,status, cl);
          
            return Ok(true);

        }


        [HttpGet]
        [Route("api/Session_Test_Detail")]
        [SwaggerOperation("To get test detail of particular session.")]
        public IActionResult Session_Test_Detail(string trainingid,string sessionid)
        {
            session_test_details sd=new session_test_details();
            List<Session> sessiondata = new List<Session>();
            Session s = new Session();
            SessionDB sdb = new SessionDB(_configuration);
            sessiondata = sdb.Get_Session_Data_By_Trg(trainingid);
            s = sessiondata.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).FirstOrDefault();
            sd.trainingCategory = s.trainingcategoryid;
            sd.questionDifficutyID = null;
            sd.skillSet = s.ttttt_tag;


            CompetencyConfiguration c = new CompetencyConfiguration();
            EvalBL ebl = new EvalBL(_configuration);
            c = ebl.GET_SELF_TEST_CONFIGURATION(trainingid);
            sd.questionCount = c.no_of_question;
            sd.time_per_question = c.time_per_question * c.no_of_question;
            //Here we multiply temp
            sd.mark_per_question = c.mark_per_question;
            sd.application_type_id = c.application_type_id;
            return Ok(sd);
        }

    }
}
