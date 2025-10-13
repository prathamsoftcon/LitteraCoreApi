using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using static Azure.Core.HttpHeader;
using static LitteraCore.Common.CommonEnum;

namespace LitteraCore.BLContext
{
    public class SessionBL
    {
        private readonly IConfiguration _configuration;
        public SessionBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Session> Get_Session_Data_By_Trg(string trainingid)
        {

            List<Session> sessiondata = new List<Session>();
            SessionDB sdb=new SessionDB(_configuration);
            sessiondata = sdb.Get_Session_Data_By_Trg(trainingid);
            Training T = new Training();
            TrgBL TBL=new TrgBL(_configuration);
            T = TBL.Get_Particular_Training(trainingid);
            int trainingtype = Convert.ToInt32(T.trg_type);

            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = T;

            foreach (Session session in sessiondata)
            {
                string moduletext = "";
                string srnotext = "";
                string daytext = "";
                string weektext = "";
                if (SESSION_DISPLAY_INFO(trainingtype, session.ttttt_type, Convert.ToInt32(session.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Module, trgdetail.trg_Setting) ==true){
                    moduletext = session.modulename;
                }
                
                if (SESSION_DISPLAY_INFO(trainingtype, session.ttttt_type, Convert.ToInt32(session.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Session_No, trgdetail.trg_Setting) == true)
                {
                    srnotext = "#Sr - " + session.ttttt_session_no.ToString();
                }
                if (SESSION_DISPLAY_INFO(trainingtype, session.ttttt_type, Convert.ToInt32(session.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Day, trgdetail.trg_Setting) == true)
                {
                    daytext = "Unit - " + session.ttttt_session_day.ToString();
                }
                if (SESSION_DISPLAY_INFO(trainingtype, session.ttttt_type, Convert.ToInt32(session.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Week, trgdetail.trg_Setting) == true)
                {
                    weektext = "week - " + session.ttttt_session_week.ToString();
                }


                session.display_txt = moduletext + " " + weektext +" "+ daytext + " " + srnotext + " "+session.ttttt_content_desc+"-"+ session.ttttt_subject;
                session.notes_download_file_name = moduletext + " " + weektext + " " + daytext + " " + srnotext;

            }







          
            return sessiondata;
        }
        public bool Save_Notes(Notes note)
        {
            bool issaved = false;
            SessionDB ABD = new SessionDB(_configuration);
            List<Notes> N = new List<Notes>();
            N = ABD.Get_Session_Notes(note.ttsn_created_by, note.ttsn_training_id, note.ttsn_session_id);
            List<notes_detail> finale_notes = new List<notes_detail>();

            if (N.Count > 0)
            {
                note.ttsn_id = N.FirstOrDefault().ttsn_id;
                finale_notes = N.FirstOrDefault().ttsn_notes.ToList();

            }
            foreach (notes_detail nd in note.ttsn_notes)
            {
                finale_notes.Add(new notes_detail { note_id=Guid.NewGuid().ToString(), createdon = nd.createdon, notes = nd.notes });
            }

            note.ttsn_notes = finale_notes.ToArray();

            issaved = ABD.Save_Session_Notes(note);
            return issaved;
        }

        public List<Notes> Get_Session_Notes(string userid, string trainingid = null, string sessionid = null)
        {
            List<Notes> notes = new List<Notes>();
            SessionDB sdb = new SessionDB(_configuration);
            notes =  sdb.Get_Session_Notes(userid, trainingid, sessionid);   
            notes = notes.OrderByDescending(o => o.ttsn_createdon).ToList();




            Training T = new Training();
            TrgBL TBL = new TrgBL(_configuration);
            T = TBL.Get_Particular_Training(trainingid);
            int trainingtype = Convert.ToInt32(T.trg_type);

            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = T;

            foreach (Notes N in notes)
            {
                string moduletext = "";
                string srnotext = "";
                string daytext = "";
                string weektext = "";
                if (SESSION_DISPLAY_INFO(trainingtype, N.ttttt_type, Convert.ToInt32(N.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Module, trgdetail.trg_Setting) == true)
                {
                    moduletext = SessionDB.Get_Session_Module_Name_by_id(N.ttttt_module_no.ToString());
                }

                if (SESSION_DISPLAY_INFO(trainingtype, N.ttttt_type, Convert.ToInt32(N.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Session_No, trgdetail.trg_Setting) == true)
                {
                    srnotext = "#Sr - " + N.ttttt_session_no.ToString();
                }
                if (SESSION_DISPLAY_INFO(trainingtype, N.ttttt_type, Convert.ToInt32(N.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Day, trgdetail.trg_Setting) == true)
                {
                    daytext = "Unit - " + N.ttttt_session_day.ToString();
                }
                if (SESSION_DISPLAY_INFO(trainingtype, N.ttttt_type, Convert.ToInt32(N.ttttt_status), (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Week, trgdetail.trg_Setting) == true)
                {
                    weektext = "week - " + N.ttttt_session_week.ToString();
                }


                N.display_session_txt = moduletext + " " + weektext + " " + daytext + " " + srnotext + " " + N.ttttt_content_desc;
              

            }
            return notes;

        }

        public List<TrgComment> Get_Trg_Comments(string trainingid, string Sessionid)
        {
            List<TrgComment> notes = new List<TrgComment>();
            SessionDB sdb = new SessionDB(_configuration);
            notes = sdb.Get_Trg_Comments(trainingid, Sessionid);
            return notes;

        }

        public bool Save_Trg_Comment(TrgComment c)
        {
            List<TrgComment> notes = new List<TrgComment>();
            SessionDB sdb = new SessionDB(_configuration);
            bool issaved = sdb.Save_Trg_Comment(c);
            return issaved;

        }
        public bool Save_Trg_Comment_reply(TrgComment_reply r)
        {
            List<TrgComment> notes = new List<TrgComment>();
            SessionDB sdb = new SessionDB(_configuration);
            bool issaved = sdb.Save_Trg_Comment_reply(r);
            return issaved;

        }
        public bool Save_Session_Content_Faculty_Feedback(string trainingid, string sessionid, string loginagencyid, SessionFeedback[] Feedback)
        {
            List<TrgComment> notes = new List<TrgComment>();
            SessionDB sdb = new SessionDB(_configuration);
            bool issaved = sdb.Save_Session_Content_Faculty_Feedback(trainingid,sessionid,loginagencyid,Feedback);
            return issaved;

        }
        public bool Update_Session_Status(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status, Session_Content_Status[] cs=null)
        {
            List<TrgComment> notes = new List<TrgComment>();
            SessionDB sdb = new SessionDB(_configuration);
            bool issaved = sdb.Update_Session_Status(Participantid,trainingid,Sessionid,timeonsession,branchid, status, cs);
            return issaved;

        }


        public bool Check_Content_Feedback_Exists(string userid, string trainingid, string sessionid)
        {
            bool isexist = false;
            SessionDB sdb = new SessionDB(_configuration);
            isexist=sdb.Check_Content_Feedback_Exists(userid,trainingid,sessionid);
            return isexist;
        }

        public List<user_session_status> Get_Participant_session_status(string userid, string trainingid = null, string sessionid = null)
        {

            List<user_session_status> statusdata = new List<user_session_status>();
            SessionDB sdb = new SessionDB(_configuration);
            statusdata = sdb.Get_Participant_session_status(userid, trainingid, sessionid);
            return statusdata;
        }
        public List<Meeting> Get_Session_Meetings(string sessionid)
        {
            List<Meeting> mettings = new List<Meeting>();
            SessionDB sdb = new SessionDB(_configuration);

            mettings=sdb.Get_Session_Meetings(sessionid);
            return mettings;
        }

        public static bool SESSION_DISPLAY_INFO(int trainingtype, int sessiontype, int sessionstatus, int DisplayFor, Trg_Setting TS)
        {



            //TrainingSettings TS = new TrainingSettings();
            //string Foldername = CommonEnum.GET_JSON_FOLDER();
            //string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "TrainingSettings.json"));
            //TS = JsonConvert.DeserializeObject<TrainingSettings>(jsontxt);
            SessionEntry SEI = new SessionEntry();
            SEI=TS.Session.SessionEntry;
            bool isdisplay = false;

            if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Session_No)
            {
                if (SEI.srno == true)
                {
                    isdisplay = true;
                }
                else
                {
                    isdisplay = false;
                }

            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Day)
            {
                if (SEI.Day == true)
                {
                    isdisplay = true;
                }
                else
                {
                    isdisplay = false;
                }


            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Week)
            {
                if (SEI.Week == true)
                {
                    isdisplay = true;
                }
                else
                {
                    isdisplay = false;
                }

            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Date)
            {
                if (SEI.Date == true)
                {
                    isdisplay = true;
                }
                else
                {
                    isdisplay = false;
                }

            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Duration)
            {
                if ((int)trainingtype == (int)CommonEnum.TRAINING_TYPE.SELF_PACED)
                {

                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = false;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = true;
                    }


                }
                else
                {

                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = false;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = true;
                    }

                }
            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Progress)
            {
                if ((int)trainingtype == (int)CommonEnum.TRAINING_TYPE.SELF_PACED)
                {

                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = false;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = true;
                    }

                }
                else
                {
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Study_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Breaks_Group)
                    {
                        isdisplay = false;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Evaluation_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Self_paced)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Sport_Group)
                    {
                        isdisplay = true;
                    }
                    if (SessionDB.Get_Session_Group(sessiontype) == (int)CommonEnum.SessionGroup.Tours_Group)
                    {
                        isdisplay = true;
                    }

                }
            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.SessionIcon)
            {
                isdisplay = true;
            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Time)
            {
                if (SEI.Date == true)
                {
                    isdisplay = true;
                }
                else
                {
                    isdisplay = false;
                }

            }
            else if ((int)DisplayFor == (int)CommonEnum.SESSION_LIST_DISPLAY_OPTIONS.Module)
            {
                if (SEI.Module == true)
                {
                    isdisplay = true;
                }
                else
                {
                    isdisplay = false;
                }

            }


            return isdisplay;
        }

        public List<Session> Get_User_Session(string usertype, string userid, DateTime trg_startdate, DateTime trg_enddate, DateTime? SessionDate = null,string branchid=null)
        {

            List<Session> sessiondata_all = new List<Session>();
            SessionDB sdb = new SessionDB(_configuration);
            sessiondata_all = sdb.Get_Session_Data(trg_startdate, trg_enddate, SessionDate);

            UserTypeTrg usertrg = new UserTypeTrg();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            FL = sessiondata_all.ConvertAll(x => new FilterUserTrg { trainingid = x.trainingid.ToString() });
            TrainingDB WDB = new TrainingDB(_configuration);
            userwise_lwtc = WDB.Get_Users_Trg_Data(FL, usertype, userid, trg_startdate, trg_enddate);
            sessiondata_all = sessiondata_all.Where(x => userwise_lwtc.Any(y => y.trainingid.ToString() == x.trainingid.ToString())).ToList();
            //*******
            sessiondata_all = sessiondata_all.Where(o => o.ttttt_status != ((int)CommonEnum.Session_Status.Delete).ToString()).ToList();

            SessionDB SDB = new SessionDB(_configuration);
            List<SessionCompletionStatus> status = new List<SessionCompletionStatus>();
            status = SDB.Get_Session_Status_vr1(null, usertype, userid, trg_startdate.ToString("yyyy/MM/dd"), trg_enddate.ToString("yyyy/MM/dd"), branchid);
            foreach (Session S in sessiondata_all)
            {

                List<SessionCompletionStatus> sessionstatus = new List<SessionCompletionStatus>();
                sessionstatus = status.Where(o => o.ttttt_session_id.ToString().ToUpper() == S.ttttt_session_id.ToString().ToUpper()).ToList();
                if (sessionstatus.Count > 0)
                {
                    if (Convert.ToString(sessionstatus.FirstOrDefault().percentcomplete) != "")
                    {
                        S.completionpercentage = Convert.ToDecimal(sessionstatus.FirstOrDefault().percentcomplete);
                    }
                    else
                    {
                        S.completionpercentage = 0;
                    }

                }
                else
                {
                    S.completionpercentage = 0;
                }

            }




            return sessiondata_all;
        }

        public List<SessionModule> Get_Session_Module()
        {
            List<SessionModule> modules = new List<SessionModule>();
            SessionDB sdb = new SessionDB(_configuration);

            modules = SessionDB.GET_SESSION_MODULE();
            return modules;
        }

        public bool Save_Session(CreateSessionDTO session)
        {
            //Convert duration in min
            if (Convert.ToInt32(session.duration_type) == (int)Common.CommonEnum.SessionDurationType.Hr)
            {
                session.duration = (Convert.ToInt32(session.duration) * 60).ToString();
            }


            //********Option to Update timetable id,Day,Week,rowno
            TrgBL WDB = new TrgBL(_configuration);
            Training lwtc = new Training();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();
            lwtc = WDB.Get_Particular_Training(session.trainingid);

            string startdate = lwtc.StartDate?.ToString("yyyy/MM/dd");
            string enddate = lwtc.T_EndDate?.ToString("yyyy/MM/dd");




            SessionDB sdb = new SessionDB(_configuration);
            List<Session> sl = sdb.Get_Session_Data_By_Trg(session.trainingid);

            //*******
            sl = sl.Where(o => o.ttttt_status != ((int)CommonEnum.Session_Status.Delete).ToString()).ToList();
            if (sl.Count() > 0)
            {
                session.time_table_Id = sl.FirstOrDefault().ttttt_timetableid;
                session.rowno = (sl.Count() + 1);
            }
            else
            {
                session.time_table_Id = Guid.NewGuid().ToString();
                session.rowno = 1;
            }
            //*****Calculate day week
            //Logic to pass day,week,sessionno,date and time according to display from JSON setting
            if (session.day == null)
            {
                session.day = 0;
            }
            if (session.week == null)
            {
                session.week = 0;
            }
            if (session.sessionno == null)
            {
                session.sessionno = 0;
            }
            if (session.date == null)
            {
                session.date = CommonEnum.DefaultDate;
            }
            if (session.sessionTime == null)
            {
                session.sessionTime = "10:00";
            }


            //***********Calculate session end date
            DateTime startdatetime = Convert.ToDateTime(session.date?.Date.ToString("yyyy/MM/dd") + " " + session.sessionTime);
            DateTime endtime = startdatetime.AddMinutes(Convert.ToInt32(session.duration));
            session.End_date_time = endtime;
            bool issaved = sdb.Save_Session(session);
            return issaved;
        }


        public static SessionEntry GET_SESSION_ENTRY_CONTROLS(int trainingtype)
        {

            TrainingSettings TS = new TrainingSettings();
            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "TrainingSettings.json"));
            TS = JsonConvert.DeserializeObject<TrainingSettings>(jsontxt);
            if (trainingtype == (int)Common.CommonEnum.TRAINING_TYPE.SELF_PACED)
            {
                return TS.Self_Paced_TRG_SessionEntry;
            }
            else
            {
                return TS.Other_TRG_SessionEntry;
            }

        }

        public bool Get_Session_Restriction(string usertype,string sessionid, List<Session> sl,SessionRestriction restrictiondata,string completiontypeid)
        {
            var isrestricted = true;
            if (Convert.ToInt32(usertype) == (int)CommonEnum.usertype.PARTICIPANT)
            {
               
                if (restrictiondata.isrestricted == 1)
                {
                    //Code to get all session completion data

                    Session opensessiondetail = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).FirstOrDefault();
                    if (opensessiondetail == null)
                    {
                        return isrestricted;
                    }

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
                        //if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test || Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Assignment)
                        if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Test)
                        {
                            //if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment && (o.completiontype != null && o.completiontype.id.ToString() != "2")).Count() <= 0)
                            if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1  && (o.completiontype != null && o.completiontype.id.ToString() != "2")).Count() <= 0)
                            {
                                //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                //Extra condition in case of test/Assignment to complete all sessions for the day before complete test/assignment
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && (Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test ) && (o.completiontype != null && o.completiontype.id.ToString() != "2")).Count() <= 0)
                                {
                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                    {
                                        if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day)&& o.noofcompletion != 1 && (o.completiontype != null && o.completiontype.id.ToString() != "2")).Count() <= 0)
                                        {
                                            isrestricted = false;
                                        }

                                        // isrestricted = false;
                                    }
                                }
                            }
                            

                        }
                        else if (Convert.ToInt32(opensessiondetail.ttttt_type) == (int)CommonEnum.SESSION_TYPE.Practical)
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
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Test && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Practical && completiontypeid.ToString() != "2").Count() <= 0)
                                {

                                    if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                    {
                                        isrestricted = false;
                                    }
                                }
                            }
                              
                        }
                        else
                        {
                           if(opensessiondetail.ttttt_complimentory == 1)
                            {
                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_no) < Convert.ToInt32(opensessiondetail.ttttt_session_no) && Convert.ToInt32(o.ttttt_session_day) == Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1).Count() <= 0)
                                {
                                    isrestricted = false;
                                }
                            }
                            else
                            {

                                if (SL.Where(o => Convert.ToInt32(o.ttttt_session_day) < Convert.ToInt32(opensessiondetail.ttttt_session_day) && o.noofcompletion != 1 && Convert.ToInt32(o.ttttt_type) != (int)CommonEnum.SESSION_TYPE.Assignment).Count() <= 0)
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
            return isrestricted;
        }


        public bool Update_Session_Visit_Status(string Participantid, string trainingid, string Sessionid, string timeonsession, string branchid, int status)
        {
            List<TrgComment> notes = new List<TrgComment>();
            SessionDB sdb = new SessionDB(_configuration);
            bool issaved = sdb.Update_Session_Visit_Status(Participantid, trainingid, Sessionid, timeonsession, branchid, status);
            return issaved;

        }



        public bool update_session_notes(Notes note)
        {
            bool issaved = false;
            SessionDB ABD = new SessionDB(_configuration);
            List<Notes> N = new List<Notes>();
            N = ABD.Get_Session_Notes(note.ttsn_created_by, note.ttsn_training_id, note.ttsn_session_id);
            Notes NS = new Notes();
            List<notes_detail> finale_notes = new List<notes_detail>();
            if (N.Count() > 0)
            {
                NS = N.FirstOrDefault();
                finale_notes = NS.ttsn_notes.ToList();
            }
          

            foreach (notes_detail nd in finale_notes)
            {
              if(nd.note_id.ToString().ToUpper() == note.ttsn_notes.FirstOrDefault().note_id.ToString().ToUpper())
                {
                    nd.notes = note.ttsn_notes.FirstOrDefault().notes;
                }
            }

            //if (N.Count > 0)
            //{
            //    note.ttsn_id = N.FirstOrDefault().ttsn_id;
            //    finale_notes = N.FirstOrDefault().ttsn_notes.ToList();

            //}
            //foreach (notes_detail nd in note.ttsn_notes)
            //{
            //    finale_notes.Add(new notes_detail { createdon = nd.createdon, notes = nd.notes });
            //}

            //note.ttsn_notes = finale_notes.ToArray();
            NS.ttsn_notes = finale_notes.ToArray();
            issaved = ABD.Save_Session_Notes(NS);
            return issaved;
        }

        public session_completion_rule session_completion_rule()
        {

            session_completion_rule rule = new session_completion_rule();
            rule.all_content_completion_mandatory = 0;
            return rule;
        }


    }
}
