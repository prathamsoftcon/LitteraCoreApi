using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Security.Certificates;
using System.Data;

namespace LitteraCore.DBContext
{

    public class TrainingDB
    {
        private readonly IConfiguration _configuration;
        public TrainingDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Training> Get_VW_Training_calendar(DateTime fromdate, DateTime todate, string status = null, string couse_director = null, string associated_course_director = null)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Training> trgdata = new List<Training>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            if (status != null)
            {
                 cmd = new SqlCommand("select TrainingId,TrainingNo,Trainingcode,CourseCode,T_Name,T_Details,SPONSOR_AG_ID,DueFees,ReceivedFees,SponsorName,HSponsorName,ParticipantLevel,LevelId,LevelDescription,HLevelDescription,CourseDirector,CourseDirectorName,HCourseDirectorName,AssociateDirector,AssociateDirectorName,HAssociateDirectorName,Duration,DurationType,T_StartDate,T_EndDate,NoOfParticipants,T_ClosingDate,NoOfParticipants_Registered,TrainingCategoryId,TrainingCategoryName,HTrainingCategoryName,TrainingStatus,StatusUpdateDate,StatusReason,HallName,HHallName,financialyear,Training_SponsorType,StartDate,CourseId,benefitted,objective,prerequiste,img_path,trg_setting,img_path,tttf_id,trg_type,trg_validity,tttt_name,tttt_hname,exptype,resident_status,CourseName,HCourseName,DepartmentReferenceNo,participation_type,proposed_amt,participant_type,ChcekListType,FeedbackType,trg_type,participant_type,participation_type,participant_type,TrainingStatus from  trainingplan.VW_Training_calendar where  (T_StartDate >= @FromDate or T_ClosingDate >= @FromDate) and T_StartDate <= @ToDate and TrainingStatus in (@TrainingStatus) order by T_StartDate desc", con);
                 cmd.Parameters.Add("@TrainingStatus", SqlDbType.NVarChar, 100).Value = status;
            }
            else
            {
                 cmd = new SqlCommand("select TrainingId,TrainingNo,Trainingcode,CourseCode,T_Name,T_Details,SPONSOR_AG_ID,DueFees,ReceivedFees,SponsorName,HSponsorName,ParticipantLevel,LevelId,LevelDescription,HLevelDescription,CourseDirector,CourseDirectorName,HCourseDirectorName,AssociateDirector,AssociateDirectorName,HAssociateDirectorName,Duration,DurationType,T_StartDate,T_EndDate,NoOfParticipants,T_ClosingDate,NoOfParticipants_Registered,TrainingCategoryId,TrainingCategoryName,HTrainingCategoryName,TrainingStatus,StatusUpdateDate,StatusReason,HallName,HHallName,financialyear,Training_SponsorType,StartDate,CourseId,benefitted,objective,prerequiste,img_path,trg_setting,img_path,tttf_id,trg_type,trg_validity,tttt_name,tttt_hname,exptype,resident_status,CourseName,HCourseName,DepartmentReferenceNo,participation_type,proposed_amt,participant_type,ChcekListType,FeedbackType,trg_type,participant_type,participation_type,participant_type,TrainingStatus from  trainingplan.VW_Training_calendar where  (T_StartDate >= @FromDate or T_ClosingDate >= @FromDate) and T_StartDate <= @ToDate order by T_StartDate desc", con);
            }
            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime2).Value = fromdate.Date;
            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime2).Value = todate.Date;
          
           
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            //if (status != null)
            //{
            //    dt.DefaultView.RowFilter = "TrainingStatus in ('" + status + "')";
            //    dt = dt.DefaultView.ToTable();
            //}
            if(couse_director != null)
            {
                List<string> course_director_strings = couse_director.Split(",".ToCharArray()).ToList();
                // Safely parse to Guid list
                List<Guid> directorGuids = course_director_strings
                    .Where(g => Guid.TryParse(g, out _))
                    .Select(Guid.Parse)
                    .ToList();

                // Filter the DataTable using LINQ
                var matchingRows = dt.AsEnumerable()
                    .Where(row => directorGuids.Contains(row.Field<Guid>("CourseDirector")));

                // Create filtered DataTable (handles empty result safely)
                DataTable filteredTable = matchingRows.Any()
                    ? matchingRows.CopyToDataTable()
                    : dt.Clone(); // return empty table with same schema if no matches

                dt = filteredTable;
            }
            if(associated_course_director != null)
            {
                List<string> associated_course_director_strings = associated_course_director.Split(",".ToCharArray()).ToList();
                // Safely parse to Guid list
                List<Guid> acddirectorGuids = associated_course_director_strings
                    .Where(g => Guid.TryParse(g, out _))
                    .Select(Guid.Parse)
                    .ToList();

                // Filter the DataTable using LINQ
                var matchingRows = dt.AsEnumerable()
                    .Where(row => acddirectorGuids.Contains(row.Field<Guid>("CourseDirector")));

                // Create filtered DataTable (handles empty result safely)
                DataTable filteredTable = matchingRows.Any()
                    ? matchingRows.CopyToDataTable()
                    : dt.Clone(); // return empty table with same schema if no matches

                dt = filteredTable;
            }

            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Get Data" + System.DateTime.Now);
            foreach (DataRow row in dt.Rows)
            {
                Training vw = new Training();
                vw.TrainingId = (System.Guid)(row["TrainingId"]);
                vw.TrainingNo = Convert.ToString(row["TrainingNo"]);
                vw.Trainingcode = Convert.ToString(row["Trainingcode"]);
                vw.CourseCode = Convert.ToString(row["CourseCode"]);
                vw.T_Name = Convert.ToString(row["T_Name"]);
                vw.T_Details = Convert.ToString(row["T_Details"]);
                vw.SPONSOR_AG_ID = (System.Guid)(row["SPONSOR_AG_ID"]);
                vw.DueFees = Convert.ToDecimal(row["DueFees"]);
                vw.ReceivedFees = Convert.ToInt32(row["ReceivedFees"]);
                vw.SponsorName = Convert.ToString(row["SponsorName"]);
                vw.HSponsorName = Convert.ToString(row["HSponsorName"]);
                vw.ParticipantLevel = Convert.ToString(row["ParticipantLevel"]);
                vw.LevelId = Convert.ToInt32(row["LevelId"]);
                vw.LevelDescription = Convert.ToString(row["LevelDescription"]);
                vw.HLevelDescription = Convert.ToString(row["HLevelDescription"]);
                vw.CourseDirector = (System.Guid)(row["CourseDirector"]);
                vw.CourseDirectorName = Convert.ToString(row["CourseDirectorName"]);
                vw.HCourseDirectorName = Convert.ToString(row["HCourseDirectorName"]);
                vw.AssociateDirector = (System.Guid)(row["AssociateDirector"]);
                vw.AssociateDirectorName = Convert.ToString(row["AssociateDirectorName"]);
                vw.HAssociateDirectorName = Convert.ToString(row["HAssociateDirectorName"]);
                vw.Duration = Convert.ToInt32(row["Duration"]);
                vw.DurationType = Convert.ToString(row["DurationType"]);
                vw.T_StartDate = Convert.ToDateTime(row["T_StartDate"]);
                vw.T_EndDate = Convert.ToDateTime(row["T_EndDate"]);
                vw.NoOfParticipants = Convert.ToInt32(row["NoOfParticipants"]);
                vw.T_ClosingDate = Convert.ToDateTime(row["T_ClosingDate"]);
                vw.NoOfParticipants_Registered = Convert.ToInt32(row["NoOfParticipants_Registered"]);
                vw.TrainingCategoryId = (System.Guid)(row["TrainingCategoryId"]);
                vw.TrainingCategoryName = Convert.ToString(row["TrainingCategoryName"]);
                vw.HTrainingCategoryName = Convert.ToString(row["HTrainingCategoryName"]);
                vw.TrainingStatus = Convert.ToString(row["TrainingStatus"]);
                vw.StatusUpdateDate = Convert.ToDateTime(row["StatusUpdateDate"]);
                vw.StatusReason = Convert.ToString(row["StatusReason"]);
                vw.HallName = Convert.ToString(row["HallName"]);
                vw.HHallName = Convert.ToString(row["HHallName"]);
                vw.financialyear = Convert.ToString(row["financialyear"]);
                vw.Training_SponsorType = Convert.ToInt32(row["Training_SponsorType"]);
                vw.StartDate = Convert.ToDateTime(row["StartDate"]);
                vw.CourseId = (System.Guid)(row["CourseId"]);
                vw.benefitted = Convert.ToString(row["benefitted"]);
                vw.objective = Convert.ToString(row["objective"]);
                vw.prerequiste = Convert.ToString(row["prerequiste"]);
                vw.img_path = Convert.ToString(row["img_path"]);

                vw.trg_setting_search = Convert.ToString(row["trg_setting"]);

                if (vw.img_path != "")
                {

                    vw.img_path_absolute = Convert.ToString(row["img_path"]);
                }
                else
                {
                    vw.img_path_absolute = null;
                }


                if (row["tttf_id"] != DBNull.Value)
                {
                    vw.tttf_id = (System.Guid)(row["tttf_id"]);
                }

                vw.trg_type = Convert.ToByte(row["trg_type"]);
                vw.trg_validity = Convert.ToString(row["trg_validity"]);
                vw.tttt_name = Convert.ToString(row["tttt_name"]);
                vw.tttt_hname = Convert.ToString(row["tttt_hname"]);
                vw.exptype = Convert.ToInt32(row["exptype"]);
                vw.resident_status = Convert.ToByte(row["resident_status"]);
                vw.CourseName = Convert.ToString(row["CourseName"]);
                vw.HCourseName = Convert.ToString(row["HCourseName"]);
                vw.DepartmentReferenceNo = Convert.ToString(row["DepartmentReferenceNo"]);
                vw.participation_type = Convert.ToInt32(row["participation_type"]);
                vw.proposed_amt = Convert.ToDecimal(row["proposed_amt"]);
                vw.participant_type = Convert.ToInt32(row["participant_type"]);
                if (row["ChcekListType"] != DBNull.Value)
                {
                    vw.ChcekListType = (System.Guid)(row["ChcekListType"]);

                }
                if (row["FeedbackType"] != DBNull.Value)
                {
                    vw.FeedbackType = (System.Guid)(row["FeedbackType"]);
                }
                //Calculate is self paced bit
                vw.isSelfPaced = Common.CommonEnum.Get_Self_Paced_Trg(row["trg_type"].ToString());

                vw.Participant_type_name = ((Common.CommonEnum.ParticipantType)row["participant_type"]).ToString();
                vw.Participantion_type_name = ((Common.CommonEnum.ParticipationType)row["participation_type"]).ToString();

                vw.participant_type_txt = Enum.GetName(typeof(Common.CommonEnum.ParticipantType), Convert.ToInt32(row["participant_type"]));
                vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.TrainingStatus), Convert.ToInt32(row["TrainingStatus"]));

                if (Convert.ToString(row["trg_setting"]) != "")
                {
                    try
                    {
                        Trg_Setting p = new Trg_Setting();
                        p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(row["trg_setting"]));
                        vw.trg_Setting = p;
                        if (p.displaycontrols != null)
                        {
                            if (p.displaycontrols.Where(o => o.id == 9).ToList().Count() > 0)
                            {
                                if(p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext != "")
                                {
                                    vw.Trainingcode = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                    vw.TrainingNo = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                }
                            }
                            if (p.displaycontrols.Where(o => o.id == 13).ToList().Count() > 0)
                            {
                                p.displaycontrols.Where(o => o.id == 13).FirstOrDefault().displaytext = vw.NoOfParticipants_Registered.ToString();
                            }
                        }
                      


                    }
                    catch
                    {
                        vw.trg_Setting = null;

                    }

                }
                else
                {
                    vw.trg_Setting = null;
                }

                trgdata.Add(vw);
            }



            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }
        public List<UserTrg> Get_Users_Training(string usertype, string userid, DateTime fromdate, DateTime todate)
        {

            List<UserTrg> usertrg = new List<UserTrg>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);

            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("select * from [TrainingPlan].[Ft_tp_get_trgid_for_usertype](@UserType, @UserId, @FromDate, @ToDate)", con);
            cmd.Parameters.Add("@UserType", SqlDbType.NVarChar, 50).Value = usertype ?? string.Empty;
            cmd.Parameters.Add("@UserId", SqlDbType.NVarChar, 100).Value = userid ?? string.Empty;
            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime2).Value = fromdate.Date;
            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime2).Value = todate.Date;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow row in dt.Rows)
            {
                UserTrg ut = new UserTrg();
                ut.traininigid = Convert.ToString(row["trainingid"]);
                usertrg.Add(ut);
            }



            return usertrg;
        }
        public List<FilterUserTrg> Get_Users_Trg_Data(List<FilterUserTrg> trg, string usertype, string userid, DateTime fromdate, DateTime todate)
        {
            List<FilterUserTrg> LWTC = new List<FilterUserTrg>();
          
            List<UserTrg> usertrg = Get_Users_Training(usertype, userid, fromdate, todate);
            // trg.FindAll(m => m.TrainingId = userid.);
            List<FilterUserTrg> filter = (List<FilterUserTrg>)(trg.Where(x => usertrg.Any(y => y.traininigid == x.trainingid))).ToList();
            // LWTC = (List<VW_Training_calendar>)filtered;
            return filter;
        }

        public TrainingAnalytics Get_StatusWise_Trg_Count(List<Training> trg, DateTime trg_startdate, DateTime trg_enddate)
        {
            //Code to calculate distict
            //  var filter = trg.Select(o=> new {o.TrainingId, o.TrainingStatus, o.T_StartDate, o.T_EndDate, o.NoOfParticipants_Registered }).Distinct().ToList();

            //List<VW_Training_calendar> LiUniq= trg.Count()
            // trg= trg.ForEach(x => x.DateDiff = Convert.ToDecimal(1) * Convert.ToDecimal(1))
            TrainingAnalytics ts = new TrainingAnalytics();
            ts.trg_completed = trg.Where(m => Convert.ToInt32(m.TrainingStatus) == 4).Count();
            ts.trg_in_prog = trg.Where(m => Convert.ToInt32(m.TrainingStatus) == 1 || Convert.ToInt32(m.TrainingStatus) == 5).Count();
            List<Training> trgfortime = (List<Training>)trg.Where(m => Convert.ToInt32(m.TrainingStatus) == 0 || Convert.ToInt32(m.TrainingStatus) == 1 || Convert.ToInt32(m.TrainingStatus) == 4 || Convert.ToInt32(m.TrainingStatus) == 5).ToList();

            ts.trg_time = Total_Trg_Time(trgfortime).ToString();

            List<Training> upcoming_trg = new List<Training>();
            upcoming_trg = trg.Where(m => Convert.ToInt32(m.TrainingStatus) == 0).ToList();
            ts.trg_upcoming = upcoming_trg.Count();



            //*************Code to get upcoming faculties

            int facultycount = 0;
            SessionDB sdb = new SessionDB(_configuration);
            List<Session> sl = sdb.Get_Session_Data(trg_startdate, trg_enddate);

            foreach (Training item in upcoming_trg)
            {
                var filter = sl.Where(o => o.trainingid.ToString() == item.TrainingId.ToString());
                item.sessions = filter.ToList();
                item.no_of_sessions = filter.ToList().Count();
                List<Faculty> fl = new List<Faculty>();
                foreach (Session s in item.sessions)
                {

                    Faculty fac = new Faculty();
                    fac.Sessionid = s.ttttt_session_id;
                    fac.ttttt_facultyid = s.ttttt_facultyid;
                    fac.facultyname = s.facultyname;
                    fac.hfacultyname = s.hfacultyname;

                    //*****
                    bool isexists = fl.Any(x => x.ttttt_facultyid == fac.ttttt_facultyid);
                    if (isexists == false && s.ttttt_facultyid != null)
                    {
                        fl.Add(fac);
                        facultycount = facultycount + 1;
                    }

                }

            }

            ts.upcoming_faculty = facultycount;

            return ts;
        }
        private decimal Total_Trg_Time(List<Training> trg)
        {
            decimal training_time = 0;

            foreach (Training n in trg)
            {
                var noofparticipant_reg = n.NoOfParticipants_Registered;
                TimeSpan dateduration = Convert.ToDateTime(n.T_EndDate).Subtract(Convert.ToDateTime(n.T_StartDate));
                var trainingtime = dateduration.Days * noofparticipant_reg;
                training_time = training_time + trainingtime;
            }


            return training_time;
        }

        public List<TrainingCategory> Get_training_Category(string categiryid = null)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Trainingplan.TP_GetTrainingCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (categiryid != null)
            {
                cmd.Parameters.AddWithValue("@TrainingCategoryId", categiryid);
            }




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<TrainingCategory> LI = new List<TrainingCategory>();
            foreach (DataRow row in dt.Rows)
            {
                TrainingCategory category = new TrainingCategory();
                category.TrainingCategoryId = Convert.ToString(row["TrainingCategoryId"]);
                category.TrainingCategoryName = Convert.ToString(row["TrainingCategoryName"]);
                category.HTrainingCategoryName = Convert.ToString(row["HTrainingCategoryName"]);
                category.CreatedBy = Convert.ToString(row["CreatedBy"]);
                category.BranchId = Convert.ToString(row["BranchId"]);
                category.IsJointDeptTraining = Convert.ToInt32(row["IsJointDeptTraining"]);
                category.Issessiongrouping = Convert.ToInt32(row["Issessiongrouping"]);
                category.usedbit = Convert.ToInt32(row["usedbit"]);
                if (Convert.ToString(row["parentcategoryid"]) != "")
                {
                    category.parentcategoryid = Convert.ToString(row["parentcategoryid"]);
                    dt.DefaultView.RowFilter = "TrainingCategoryId='" + Convert.ToString(row["parentcategoryid"]) + "'";
                    DataTable dtfiltereddata = dt.DefaultView.ToTable();
                    if (dtfiltereddata.Rows.Count > 0)
                    {
                        category.parentcategoryname = dtfiltereddata.Rows[0]["TrainingCategoryName"].ToString();
                    }


                }

                LI.Add(category);

            }

            LI = LI.OrderBy(o => o.parentcategoryid).ThenBy(o => o.parentcategoryid).ToList();

            return LI;
        }

        // Added 2026-07-09 for the frm_Master_Configuration.aspx -> React migration
        // (Training Category tab, "Save" action). Old page called this exact
        // procedure via the generic /TrainingApi/RCVP_Training_Type_Save_Data
        // dispatcher (ProcedureName=[TrainingPlan].proc_tp_ins_upd_training_category_new),
        // confirmed by tracing JS_frm_Master_Configuration.js ->
        // TRAININGAPIController.vb -> dm.Save_Common_Data(Domain, IsOnline,
        // ProcedureName, ...). Params below match exactly what the old JS sent.
        public bool Save_Training_Category(TrainingCategory category)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("[TrainingPlan].proc_tp_ins_upd_training_category_new", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@TrainingCategoryId", category.TrainingCategoryId);
            cmd.Parameters.AddWithValue("@TrainingCategoryName", category.TrainingCategoryName);
            cmd.Parameters.AddWithValue("@HTrainingCategoryName", category.HTrainingCategoryName);
            cmd.Parameters.AddWithValue("@CreatedBy", category.CreatedBy);
            cmd.Parameters.AddWithValue("@BranchId", category.BranchId);
            cmd.Parameters.AddWithValue("@Issessiongrouping", category.Issessiongrouping);
            // Old page only sent @parentcategoryid when a parent was actually
            // selected (ddlparentcategory.selectedIndex > 0) - mirror that here
            // rather than always sending it, since the procedure signature was
            // never confirmed to accept a NULL/empty value for this param.
            if (!string.IsNullOrEmpty(category.parentcategoryid))
            {
                cmd.Parameters.AddWithValue("@parentcategoryid", category.parentcategoryid);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        // Added 2026-07-09 for the frm_Master_Configuration.aspx -> React migration
        // (Training Category tab, "Delete" action). Old page called this exact
        // procedure via the generic /TrainingAPI/RCVP_Training_Type_Delete_Data
        // dispatcher (ProcedureName=TrainingPlan.TP_DeleteTRainingCategory,
        // Parameters=[{'@TrainingCategoryId':'<id>'}] - confirmed by tracing
        // JS_frm_Master_Configuration.js line 1941). Confirmed gap - this
        // procedure is not called anywhere else in this file or any other
        // DBContext file as of 2026-07-09 (checked all 20).
        public bool Delete_Training_Category(string trainingCategoryId)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.TP_DeleteTRainingCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@TrainingCategoryId", trainingCategoryId);

            SqlDataAdapter da2 = new SqlDataAdapter(cmd);
            da2.Fill(dt);
            con.Close();

            return true;
        }

        // Added 2026-07-09 for the frm_Master_Configuration.aspx -> React migration
        // (Training Category tab, "in use?" check shown before edit/delete). Old
        // page's dm.CHK_CATEGORY_IN_USE was found in the REAL old
        // C:\Projects\TraininingERP_old\API_ERP\API_ERP_TRAINING\Datamanager.vb
        // (L2979) - NOT the reference-only LitteraCoreReactAPI\DBContext\old\
        // copy, which is off-limits per the user. That method calls this exact
        // SQL scalar function via ExecuteScalar, not a stored procedure.
        // Parameterized here (unlike the old VB, which concatenated the id
        // directly into inline SQL text) to avoid replicating that
        // SQL-injection-shaped pattern. Confirmed gap - not called anywhere in
        // any DBContext file as of 2026-07-09 (checked all 20).
        //
        // Semantics note (preserved as-is, not reinterpreted): in the old JS
        // (TRG_TC_CHK_CATEGORY_DETAIL_IN_USE_OnLoadDelete,
        // JS_frm_Master_Configuration.js L2036), the response's "isexist" being
        // the string "False" is what BLOCKS deletion - i.e. despite the old
        // method/function names ("CHK_CATEGORY_IN_USE",
        // "F_CheckTrainingCategoryRate"), this reads as "does the rate/detail
        // exist" rather than a literal "is-in-use" flag. The value is passed
        // straight through here exactly as the old code did.
        public bool Chk_Category_In_Use(string categoryDetailId)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Select TrainingPlan.[F_CheckTrainingCategoryRate] (@CategoryDetailID)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@CategoryDetailID", categoryDetailId);
            bool isExist = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return isExist;
        }

        public List<TRG_DAY_WEEK> Get_Day_Week_Count_Id(string fromdate, string todate)
        {
            List<TRG_DAY_WEEK> dayweek = new List<TRG_DAY_WEEK>();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select * from [trainingplan].[ft_tp_get_week_day_count](@FromDate, @ToDate)", con);
            cmd.Parameters.Add("@FromDate", SqlDbType.NVarChar, 50).Value = fromdate ?? string.Empty;
            cmd.Parameters.Add("@ToDate", SqlDbType.NVarChar, 50).Value = todate ?? string.Empty;
            cmd.CommandType = CommandType.Text;


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {

                TRG_DAY_WEEK w = new TRG_DAY_WEEK
                {
                    datevalue = Convert.ToDateTime(row["datevalue"]).ToString("yyyy/MM/dd"),
                    daycount = Convert.ToInt32(row["daycount"]),
                    week = Convert.ToInt32(row["week"]),
                    weekcount = Convert.ToInt32(row["weekcount"]),
                    isenable = Convert.ToInt32(row["inenable"])
                };
                dayweek.Add(w);
            }

            return dayweek;
        }


        public Training Get_Particular_Training_Detail(string trainingid)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Training> trgdata = new List<Training>();
            Training trainingDetail = new Training();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("select TrainingId,TrainingNo,Trainingcode,CourseCode,T_Name,T_Details,SPONSOR_AG_ID,DueFees,ReceivedFees,SponsorName,HSponsorName,ParticipantLevel,LevelId,LevelDescription,HLevelDescription,CourseDirector,CourseDirectorName,HCourseDirectorName,AssociateDirector,AssociateDirectorName,HAssociateDirectorName,Duration,DurationType,T_StartDate,T_EndDate,NoOfParticipants,T_ClosingDate,NoOfParticipants_Registered,TrainingCategoryId,TrainingCategoryName,HTrainingCategoryName,TrainingStatus,StatusUpdateDate,StatusReason,HallName,HHallName,financialyear,Training_SponsorType,StartDate,CourseId,benefitted,objective,prerequiste,img_path,tttf_id,trg_type,trg_validity,tttt_name,tttt_hname,exptype,resident_status,CourseName,HCourseName,DepartmentReferenceNo,participation_type,proposed_amt,participant_type,ChcekListType,FeedbackType,trg_type,participant_type,participation_type,participant_type,TrainingStatus,trg_setting from trainingplan.VW_Training_calendar where TrainingId = @TrainingId", con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 5000;

                // Add the parameter to avoid SQL injection
                cmd.Parameters.AddWithValue("@TrainingId", trainingid);



                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Training vw = new Training
                        {
                            TrainingId = (Guid)(reader["TrainingId"]),
                            TrainingNo = Convert.ToString(reader["TrainingNo"]),
                            Trainingcode = Convert.ToString(reader["Trainingcode"]),
                            CourseCode = Convert.ToString(reader["CourseCode"]),
                            T_Name = Convert.ToString(reader["T_Name"]),
                            T_Details = Convert.ToString(reader["T_Details"]),
                            SPONSOR_AG_ID = (Guid)(reader["SPONSOR_AG_ID"]),
                            DueFees = Convert.ToDecimal(reader["DueFees"]),
                            ReceivedFees = Convert.ToInt32(reader["ReceivedFees"]),
                            SponsorName = Convert.ToString(reader["SponsorName"]),
                            HSponsorName = Convert.ToString(reader["HSponsorName"]),
                            ParticipantLevel = Convert.ToString(reader["ParticipantLevel"]),
                            LevelId = Convert.ToInt32(reader["LevelId"]),
                            LevelDescription = Convert.ToString(reader["LevelDescription"]),
                            HLevelDescription = Convert.ToString(reader["HLevelDescription"]),
                            CourseDirector = (Guid)(reader["CourseDirector"]),
                            CourseDirectorName = Convert.ToString(reader["CourseDirectorName"]),
                            HCourseDirectorName = Convert.ToString(reader["HCourseDirectorName"]),
                            AssociateDirector = (Guid)(reader["AssociateDirector"]),
                            AssociateDirectorName = Convert.ToString(reader["AssociateDirectorName"]),
                            HAssociateDirectorName = Convert.ToString(reader["HAssociateDirectorName"]),
                            Duration = Convert.ToInt32(reader["Duration"]),
                            DurationType = Convert.ToString(reader["DurationType"]),
                            T_StartDate = Convert.ToDateTime(reader["T_StartDate"]),
                            T_EndDate = Convert.ToDateTime(reader["T_EndDate"]),
                            NoOfParticipants = Convert.ToInt32(reader["NoOfParticipants"]),
                            T_ClosingDate = Convert.ToDateTime(reader["T_ClosingDate"]),
                            NoOfParticipants_Registered = Convert.ToInt32(reader["NoOfParticipants_Registered"]),
                            TrainingCategoryId = (Guid)(reader["TrainingCategoryId"]),
                            TrainingCategoryName = Convert.ToString(reader["TrainingCategoryName"]),
                            HTrainingCategoryName = Convert.ToString(reader["HTrainingCategoryName"]),
                            TrainingStatus = Convert.ToString(reader["TrainingStatus"]),
                            StatusUpdateDate = Convert.ToDateTime(reader["StatusUpdateDate"]),
                            StatusReason = Convert.ToString(reader["StatusReason"]),
                            HallName = Convert.ToString(reader["HallName"]),
                            HHallName = Convert.ToString(reader["HHallName"]),
                            financialyear = Convert.ToString(reader["financialyear"]),
                            Training_SponsorType = Convert.ToInt32(reader["Training_SponsorType"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            CourseId = (Guid)(reader["CourseId"]),
                            benefitted = Convert.ToString(reader["benefitted"]),
                            objective = Convert.ToString(reader["objective"]),
                            prerequiste = Convert.ToString(reader["prerequiste"]),
                            img_path = Convert.ToString(reader["img_path"]),
                            img_path_absolute = string.IsNullOrEmpty(Convert.ToString(reader["img_path"])) ? null : Convert.ToString(reader["img_path"])
                        };

                        // Handle the optional fields
                        if (reader["tttf_id"] != DBNull.Value)
                        {
                            vw.tttf_id = (Guid)(reader["tttf_id"]);
                        }

                        vw.trg_type = Convert.ToByte(reader["trg_type"]);
                        vw.trg_validity = Convert.ToString(reader["trg_validity"]);
                        vw.tttt_name = Convert.ToString(reader["tttt_name"]);
                        vw.tttt_hname = Convert.ToString(reader["tttt_hname"]);
                        vw.exptype = Convert.ToInt32(reader["exptype"]);
                        vw.resident_status = Convert.ToByte(reader["resident_status"]);
                        vw.CourseName = Convert.ToString(reader["CourseName"]);
                        vw.HCourseName = Convert.ToString(reader["HCourseName"]);
                        vw.DepartmentReferenceNo = Convert.ToString(reader["DepartmentReferenceNo"]);
                        vw.participation_type = Convert.ToInt32(reader["participation_type"]);
                        vw.proposed_amt = Convert.ToDecimal(reader["proposed_amt"]);
                        vw.participant_type = Convert.ToInt32(reader["participant_type"]);

                        if (reader["ChcekListType"] != DBNull.Value)
                        {
                            vw.ChcekListType = (Guid)(reader["ChcekListType"]);
                        }

                        if (reader["FeedbackType"] != DBNull.Value)
                        {
                            vw.FeedbackType = (Guid)(reader["FeedbackType"]);
                        }

                        // Calculate is self-paced bit
                        vw.isSelfPaced = Common.CommonEnum.Get_Self_Paced_Trg(Convert.ToString(reader["trg_type"]));

                        vw.Participant_type_name = Enum.GetName(typeof(Common.CommonEnum.ParticipantType), Convert.ToInt32(reader["participant_type"]));
                        vw.Participantion_type_name = Enum.GetName(typeof(Common.CommonEnum.ParticipationType), Convert.ToInt32(reader["participation_type"]));

                        vw.participant_type_txt = Enum.GetName(typeof(Common.CommonEnum.ParticipantType), Convert.ToInt32(reader["participant_type"]));
                        vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.TrainingStatus), Convert.ToInt32(reader["TrainingStatus"]));

                        // Deserialize trg_setting if not empty
                        if (reader["trg_setting"] != DBNull.Value)
                        {
                            try
                            {
                                Trg_Setting p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(reader["trg_setting"]));
                                vw.trg_Setting = p;
                                //vw.trg_Setting.certificate_setting = new certificate_setting();
                                if (p.displaycontrols != null)
                                {
                                    if (p.displaycontrols.Where(o => o.id == 9).ToList().Count() > 0)
                                    {
                                        if (p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext != "")
                                        {
                                            vw.Trainingcode = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                            vw.TrainingNo = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                        }
                                    }
                                }



                            }
                            catch
                            {
                                vw.trg_Setting = null;
                            }
                        }
                        else
                        {
                            vw.trg_Setting = null;
                        }

                        trgdata.Add(vw);
                    }
                }


           
                trainingDetail = trgdata.FirstOrDefault();

                PaginationParam param = new PaginationParam { PageNumber = 1, PageSize = 10 };
                AgencyBL abl = new AgencyBL(_configuration);
                Agency CDdetails = new Agency();
                CDdetails=abl.Get_Agency(null, trainingDetail.CourseDirector.ToString(), null, param, null).Items.FirstOrDefault();
                Agency ACDdetails = new Agency();
                ACDdetails = abl.Get_Agency(null, trainingDetail.AssociateDirector.ToString(), null, param, null).Items.FirstOrDefault();
                List<trg_contact_person> cp=new List<trg_contact_person>();

                cp.Add(new trg_contact_person { person_name=CDdetails.agencyname, person_email= CDdetails.ag_email, person_mobile= CDdetails.ag_mobileno });
                cp.Add(new trg_contact_person { person_name = ACDdetails.agencyname, person_email = ACDdetails.ag_email, person_mobile = ACDdetails.ag_mobileno });
                trainingDetail.contact_person = cp.ToArray();

            }

            return trainingDetail;

        }
        public Training Get_Particular_Training_Detail_By_Code(string trainingcode)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Training> trgdata = new List<Training>();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("select TrainingId,TrainingNo,Trainingcode,CourseCode,T_Name,T_Details,SPONSOR_AG_ID,DueFees,ReceivedFees,SponsorName,HSponsorName,ParticipantLevel,LevelId,LevelDescription,HLevelDescription,CourseDirector,CourseDirectorName,HCourseDirectorName,AssociateDirector,AssociateDirectorName,HAssociateDirectorName,Duration,DurationType,T_StartDate,T_EndDate,NoOfParticipants,T_ClosingDate,NoOfParticipants_Registered,TrainingCategoryId,TrainingCategoryName,HTrainingCategoryName,TrainingStatus,StatusUpdateDate,StatusReason,HallName,HHallName,financialyear,Training_SponsorType,StartDate,CourseId,benefitted,objective,prerequiste,img_path,tttf_id,trg_type,trg_validity,tttt_name,tttt_hname,exptype,resident_status,CourseName,HCourseName,DepartmentReferenceNo,participation_type,proposed_amt,participant_type,ChcekListType,FeedbackType,trg_type,participant_type,participation_type,participant_type,TrainingStatus,trg_setting from trainingplan.VW_Training_calendar where trainingno = @TrainingCode", con);
                cmd.Parameters.Add("@TrainingCode", SqlDbType.NVarChar, 100).Value = trainingcode ?? string.Empty;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 5000;

                // Add the parameter to avoid SQL injection
               // cmd.Parameters.AddWithValue("@TrainingId", trainingid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Training vw = new Training
                        {
                            TrainingId = (Guid)(reader["TrainingId"]),
                            TrainingNo = Convert.ToString(reader["TrainingNo"]),
                            Trainingcode = Convert.ToString(reader["Trainingcode"]),
                            CourseCode = Convert.ToString(reader["CourseCode"]),
                            T_Name = Convert.ToString(reader["T_Name"]),
                            T_Details = Convert.ToString(reader["T_Details"]),
                            SPONSOR_AG_ID = (Guid)(reader["SPONSOR_AG_ID"]),
                            DueFees = Convert.ToDecimal(reader["DueFees"]),
                            ReceivedFees = Convert.ToInt32(reader["ReceivedFees"]),
                            SponsorName = Convert.ToString(reader["SponsorName"]),
                            HSponsorName = Convert.ToString(reader["HSponsorName"]),
                            ParticipantLevel = Convert.ToString(reader["ParticipantLevel"]),
                            LevelId = Convert.ToInt32(reader["LevelId"]),
                            LevelDescription = Convert.ToString(reader["LevelDescription"]),
                            HLevelDescription = Convert.ToString(reader["HLevelDescription"]),
                            CourseDirector = (Guid)(reader["CourseDirector"]),
                            CourseDirectorName = Convert.ToString(reader["CourseDirectorName"]),
                            HCourseDirectorName = Convert.ToString(reader["HCourseDirectorName"]),
                            AssociateDirector = (Guid)(reader["AssociateDirector"]),
                            AssociateDirectorName = Convert.ToString(reader["AssociateDirectorName"]),
                            HAssociateDirectorName = Convert.ToString(reader["HAssociateDirectorName"]),
                            Duration = Convert.ToInt32(reader["Duration"]),
                            DurationType = Convert.ToString(reader["DurationType"]),
                            T_StartDate = Convert.ToDateTime(reader["T_StartDate"]),
                            T_EndDate = Convert.ToDateTime(reader["T_EndDate"]),
                            NoOfParticipants = Convert.ToInt32(reader["NoOfParticipants"]),
                            T_ClosingDate = Convert.ToDateTime(reader["T_ClosingDate"]),
                            NoOfParticipants_Registered = Convert.ToInt32(reader["NoOfParticipants_Registered"]),
                            TrainingCategoryId = (Guid)(reader["TrainingCategoryId"]),
                            TrainingCategoryName = Convert.ToString(reader["TrainingCategoryName"]),
                            HTrainingCategoryName = Convert.ToString(reader["HTrainingCategoryName"]),
                            TrainingStatus = Convert.ToString(reader["TrainingStatus"]),
                            StatusUpdateDate = Convert.ToDateTime(reader["StatusUpdateDate"]),
                            StatusReason = Convert.ToString(reader["StatusReason"]),
                            HallName = Convert.ToString(reader["HallName"]),
                            HHallName = Convert.ToString(reader["HHallName"]),
                            financialyear = Convert.ToString(reader["financialyear"]),
                            Training_SponsorType = Convert.ToInt32(reader["Training_SponsorType"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            CourseId = (Guid)(reader["CourseId"]),
                            benefitted = Convert.ToString(reader["benefitted"]),
                            objective = Convert.ToString(reader["objective"]),
                            prerequiste = Convert.ToString(reader["prerequiste"]),
                            img_path = Convert.ToString(reader["img_path"]),
                            img_path_absolute = string.IsNullOrEmpty(Convert.ToString(reader["img_path"])) ? null : Convert.ToString(reader["img_path"])
                        };

                        // Handle the optional fields
                        if (reader["tttf_id"] != DBNull.Value)
                        {
                            vw.tttf_id = (Guid)(reader["tttf_id"]);
                        }

                        vw.trg_type = Convert.ToByte(reader["trg_type"]);
                        vw.trg_validity = Convert.ToString(reader["trg_validity"]);
                        vw.tttt_name = Convert.ToString(reader["tttt_name"]);
                        vw.tttt_hname = Convert.ToString(reader["tttt_hname"]);
                        vw.exptype = Convert.ToInt32(reader["exptype"]);
                        vw.resident_status = Convert.ToByte(reader["resident_status"]);
                        vw.CourseName = Convert.ToString(reader["CourseName"]);
                        vw.HCourseName = Convert.ToString(reader["HCourseName"]);
                        vw.DepartmentReferenceNo = Convert.ToString(reader["DepartmentReferenceNo"]);
                        vw.participation_type = Convert.ToInt32(reader["participation_type"]);
                        vw.proposed_amt = Convert.ToDecimal(reader["proposed_amt"]);
                        vw.participant_type = Convert.ToInt32(reader["participant_type"]);

                        if (reader["ChcekListType"] != DBNull.Value)
                        {
                            vw.ChcekListType = (Guid)(reader["ChcekListType"]);
                        }

                        if (reader["FeedbackType"] != DBNull.Value)
                        {
                            vw.FeedbackType = (Guid)(reader["FeedbackType"]);
                        }

                        // Calculate is self-paced bit
                        vw.isSelfPaced = Common.CommonEnum.Get_Self_Paced_Trg(Convert.ToString(reader["trg_type"]));

                        vw.Participant_type_name = Enum.GetName(typeof(Common.CommonEnum.ParticipantType), Convert.ToInt32(reader["participant_type"]));
                        vw.Participantion_type_name = Enum.GetName(typeof(Common.CommonEnum.ParticipationType), Convert.ToInt32(reader["participation_type"]));

                        vw.participant_type_txt = Enum.GetName(typeof(Common.CommonEnum.ParticipantType), Convert.ToInt32(reader["participant_type"]));
                        vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.TrainingStatus), Convert.ToInt32(reader["TrainingStatus"]));

                        // Deserialize trg_setting if not empty
                        if (reader["trg_setting"] != DBNull.Value)
                        {
                            try
                            {
                                Trg_Setting p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(reader["trg_setting"]));
                                vw.trg_Setting = p;
                                if (p.displaycontrols != null)
                                {
                                    if (p.displaycontrols.Where(o => o.id == 9).ToList().Count() > 0)
                                    {
                                        if (p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext != "")
                                        {
                                            vw.Trainingcode = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                            vw.TrainingNo = p.displaycontrols.Where(o => o.id == 9).ToList().FirstOrDefault().displaytext;
                                        }
                                    }
                                }



                            }
                            catch
                            {
                                vw.trg_Setting = null;
                            }
                        }
                        else
                        {
                            vw.trg_Setting = null;
                        }

                        trgdata.Add(vw);
                    }
                }
            }

            return trgdata.FirstOrDefault();

        }

        public List<CERTIFICATE_SIGNATORY> Get_Certificate_signatory(string trainingid)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<CERTIFICATE_SIGNATORY> signatory = new List<CERTIFICATE_SIGNATORY>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_get_certificate_signatory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttcs_trainingid", trainingid);



            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Get Data" + System.DateTime.Now);
            foreach (DataRow row in dt.Rows)
            {
                CERTIFICATE_SIGNATORY vw = new CERTIFICATE_SIGNATORY();
                vw.id = Convert.ToString(row["ttcs_agenyid"]);
                vw.name = Convert.ToString(row["AgencyName"]);
                vw.signatureorder = Convert.ToInt16(row["ttcs_order"]);
                vw.signaturepath = Convert.ToString(row["ag_sign_path"]);
                vw.designation = Convert.ToString(row["designation"]);
                signatory.Add(vw);
            }



            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return signatory;
        }


        public bool Save_Trg_Participant_Mapping(TRGMAPPING trgmapping)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlTransaction st = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_ins_upd_participant_trg_map_vr1", con);
                cmd.Transaction = st;
                cmd.Parameters.AddWithValue("@Trainingid", trgmapping.trainingid);
                cmd.Parameters.AddWithValue("@Createdby", trgmapping.Createdby);
                cmd.Parameters.AddWithValue("@CreatedOn", System.DateTime.Now.ToString("yyyy/MM/dd hh:MM:ss"));
                cmd.Parameters.AddWithValue("@Branchid", trgmapping.branchid);
                cmd.Parameters.AddWithValue("@Sponsorid", trgmapping.sponsorid);
                cmd.Parameters.AddWithValue("@Participantid", trgmapping.participantid);
                //cmd.Parameters.AddWithValue("@Newtrainingid", trgmapping.trainingid_New);
                //cmd.Parameters.AddWithValue("@Newsponsorid", trgmapping.sponsorid_new);
                //cmd.Parameters.AddWithValue("@uploadpath", trgmapping.uploadpath);
                //cmd.Parameters.AddWithValue("@remark", trgmapping.remark);

                cmd.Parameters.Add("@ttpai_id", SqlDbType.VarChar, 50);
                cmd.Parameters["@ttpai_id"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@newttpai_id", SqlDbType.VarChar, 50);
                //cmd.Parameters.Add("@UserCode", SqlDbType.VarChar, 50);
                //cmd.Parameters.Add("@newUserCode", SqlDbType.VarChar, 50);

                //cmd.Parameters["@newttpai_id"].Direction = ParameterDirection.Output;
                //cmd.Parameters["@UserCode"].Direction = ParameterDirection.Output;
                //cmd.Parameters["@newUserCode"].Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = st;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                cmd.ExecuteNonQuery();
                string ttpaiid = Convert.ToString(cmd.Parameters["@ttpai_id"].Value);
                //string newttpai_id = Convert.ToString(cmd.Parameters["@newttpai_id"].Value);
                //string UserCode = Convert.ToString(cmd.Parameters["@UserCode"].Value);
                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "mapped-"+ ttpaiid);
                DMSBL dbl = new DMSBL(_configuration);
                string UserCode = dbl.Get_doc_no(System.DateTime.Now.ToString("yyyy/MM/dd"), trgmapping.branchid, Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING), "$$", "YEAR");
                //string newUserCode = Convert.ToString(cmd.Parameters["@newUserCode"].Value);

                //***********

                // DMSBL dbl = new DMSBL();
                DMS d = new DMS
                {
                    docno = UserCode,
                    doc_id = ttpaiid,
                    createdon = DateTime.Now,
                    createdby = trgmapping.Createdby,
                    branchid = trgmapping.branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = trgmapping.userid,
                    fwd_empid = trgmapping.userid,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.Participant_MAPPING),
                    doc_status = Convert.ToInt32(trgmapping.status),
                };
                dbl.Save_DMS_DATA(d, con, st);
                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "DMS SAVED");
                //****************Code to update finance amount
                SqlCommand cmd1 = new SqlCommand("trainingplan.proc_tp_ins_upd_trg_fees_amount", con);
                cmd1.Transaction = st;
                cmd1.Parameters.AddWithValue("@trainingId", trgmapping.trainingid);
                cmd1.Parameters.AddWithValue("@Sponsorid", trgmapping.sponsorid);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();
                st.Commit();
                con.Close();
                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "Fees Updated");
                return true;
            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
            return false;

        }

        public List<Agency> Get_Trg_Sponsor(string trainingid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.TP_GetTRainingSponsors", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@TrainingId", trainingid);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Agency> LI = new List<Agency>();
            foreach (DataRow row in dt.Rows)
            {
                Agency exp = new Agency();
                exp.agencyid = Convert.ToString(row["Departmentid"]);
                exp.agencyname = Convert.ToString(row["AgencyName"]);
                exp.hagencyname = Convert.ToString(row["HAgencyName"]);

                LI.Add(exp);

            }



            return LI;
        }

        public List<Trg_Type> Get_Trg_Type()
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Trg_Type> trgdata = new List<Trg_Type>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_training_type", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Get Data" + System.DateTime.Now);
            foreach (DataRow row in dt.Rows)
            {
                Trg_Type vw = new Trg_Type();
                vw.tttt_id = Convert.ToString(row["tttt_id"]);
                vw.tttt_name = Convert.ToString(row["tttt_name"]);
                vw.tttt_hname = Convert.ToString(row["tttt_hname"]);
                vw.tttt_active = Convert.ToInt32(row["tttt_active"]);
               
                trgdata.Add(vw);
            }



            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        // Added 2026-07-11 for the frm_Master_Configuration.aspx -> React
        // migration (Fees tab, Sponsor Type dropdown - confirmed genuine
        // gap). Old TRG_FM_FILL_Sponsor_TYPE() (JS L3244) calls
        // /TrainingApi/Get_Data with
        // ProcedureName=trainingplan.proc_tp_get_sponsor_type. Not called
        // anywhere in any LitteraCoreReactAPI\DBContext\*.cs file (checked
        // all 20) - only present in the old JS and the real old
        // Datamanager.vb's Get_Training_Payment_Type() (which calls this
        // same procedure with CommandType.StoredProcedure, no params).
        // Mirrors Get_Trg_Type() immediately above (Training Type's already-
        // migrated sibling endpoint) - same shape, same no-params call.
        public List<Trg_Sponsor_Type> Get_Trg_Sponsor_Type()
        {
            List<Trg_Sponsor_Type> stdata = new List<Trg_Sponsor_Type>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_get_sponsor_type", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                Trg_Sponsor_Type st = new Trg_Sponsor_Type();
                st.ttst_id = Convert.ToString(row["ttst_id"]);
                st.ttst_name = Convert.ToString(row["ttst_name"]);
                st.ttst_hname = Convert.ToString(row["ttst_hname"]);

                stdata.Add(st);
            }

            return stdata;
        }

        public List<Trg_Title> Get_Trg_Title()
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Trg_Title> trgdata = new List<Trg_Title>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.TP_GetCourse", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Get Data" + System.DateTime.Now);
            foreach (DataRow row in dt.Rows)
            {
                Trg_Title vw = new Trg_Title();
                vw.CourseId = Convert.ToString(row["CourseId"]);
                vw.CourseName = Convert.ToString(row["CourseName"]);
                vw.HCourseName = Convert.ToString(row["HCourseName"]);
                vw.CourseCode = Convert.ToString(row["CourseCode"]);

                // Added 2026-07-10 to close the read-side data-shape gap: the
                // React grid's Course Category column was always showing "-"
                // because these columns were never read here, even though
                // trainingplan.TP_GetCourse already returns them (confirmed via
                // the old grid's JsonObj["coursecategory"] /
                // JsonObj["trainingcategoryname"] / JsonObj["htrainingcategoryname"] /
                // JsonObj["isactive"] in JS_frm_Master_Configuration.js L2531).
                // Column-name casing (CourseCategory / TrainingCategoryName /
                // HTrainingCategoryName / IsActive) is inferred from the same
                // PascalCase pattern the 4 columns above already use
                // successfully (CourseId/CourseName/HCourseName/CourseCode) -
                // not independently verified against a live database. Guarded
                // with Columns.Contains so a wrong guess degrades to blank
                // values instead of throwing and breaking the whole list.
                if (dt.Columns.Contains("CourseCategory"))
                {
                    vw.Coursecategory = Convert.ToString(row["CourseCategory"]);
                }
                if (dt.Columns.Contains("IsActive"))
                {
                    vw.Isactive = Convert.ToString(row["IsActive"]);
                }
                if (dt.Columns.Contains("TrainingCategoryName"))
                {
                    vw.TrainingCategoryName = Convert.ToString(row["TrainingCategoryName"]);
                }
                if (dt.Columns.Contains("HTrainingCategoryName"))
                {
                    vw.HTrainingCategoryName = Convert.ToString(row["HTrainingCategoryName"]);
                }

                trgdata.Add(vw);
            }



            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        // Added 2026-07-10 for the frm_Master_Configuration.aspx -> React
        // migration (Training Title tab, "Save" action). Old
        // Beta_Pages_TrainingMaster_SaveData() (JS_frm_Master_Configuration.js
        // L2699) calls /TrainingAPI/RCVP_SAVE_COURSE_MASTER, which
        // (TRAININGAPIController.vb L16975, in the real
        // C:\Projects\TraininingERP_old\API_ERP\API_ERP_TRAINING project) calls
        // dm.InsUpdCourseDetails(...) - found in that project's Datamanager.vb
        // L2147, NOT the differently-shaped same-named method in
        // TrainingClass.vb (9 params, no isactive/coursecategory). The REAL
        // one actually used here has 13 params including @isactive and
        // @coursecategory, both of which this stored procedure genuinely
        // accepts even though the current read side (Get_Trg_Title above)
        // doesn't return them - that's a separate, still-open data-shape gap.
        // Confirmed gap: TP_InsUpdCourse is not called anywhere in any
        // LitteraCoreReactAPI\DBContext\*.cs file (checked all 20).
        //
        // @CourseDuration / @DurationType / @CourseDetails are intentionally
        // NOT sent as real values here: this tab's UI collects no duration
        // data, and even the old page's actual JS always sends the literal
        // string 'NULL' for these regardless of any stale duration markup
        // elsewhere on the page - the real dm.InsUpdCourseDetails treats a
        // null CourseDuration/DurationType by omitting those params entirely,
        // which is mirrored here rather than guessing at values nothing
        // collects.
        public bool Save_Trg_Title(Trg_Title title)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("[TrainingPlan].[TP_InsUpdCourse]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@CourseId", title.CourseId);
            cmd.Parameters.AddWithValue("@CourseName", title.CourseName);
            cmd.Parameters.AddWithValue("@HCourseName", title.HCourseName);
            cmd.Parameters.AddWithValue("@CourseCode", string.IsNullOrEmpty(title.CourseCode) ? (object)DBNull.Value : title.CourseCode);
            cmd.Parameters.AddWithValue("@CreatedBy", title.CreatedBy);
            cmd.Parameters.AddWithValue("@BranchId", title.BranchId);
            cmd.Parameters.AddWithValue("@CourseDetails", DBNull.Value);
            cmd.Parameters.AddWithValue("@isactive", title.Isactive);
            cmd.Parameters.AddWithValue("@coursecategory", title.Coursecategory);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        // Added 2026-07-10 for the frm_Master_Configuration.aspx -> React
        // migration (Training Title tab, "Delete" action). Old
        // Beta_Pages_TrainingMaster_DeleteData() (JS L2769) calls
        // /TrainingAPI/RCVP_Course_Master_Delete_Data with
        // ProcedureName=[TrainingPlan].[TP_DeleteCourse], @CourseId. Confirmed
        // gap - not called anywhere in any DBContext file (checked all 20).
        public bool Delete_Trg_Title(string courseId)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("[TrainingPlan].[TP_DeleteCourse]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@CourseId", courseId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        // Added 2026-07-11 for the frm_Master_Configuration.aspx -> React
        // migration (Fees tab, "Load" action - the last remaining gap on this
        // page). Old Fill_Fees_Details() (JS_frm_Master_Configuration.js
        // L3101) calls /TrainingApi/Get_Data with
        // ProcedureName=TrainingPlan.proc_get_training_fees_data and
        // @tttf_id='NULL' (unfiltered - list everything), same "send NULL to
        // list all" idiom as Category's Get_training_Category. Confirmed
        // gap: proc_get_training_fees_data is not called anywhere in any
        // LitteraCoreReactAPI\DBContext\*.cs file (checked all 20).
        //
        // Column names (tttf_id/tttf_trg_type/tttf_sponsor_type/
        // tttf_duration/tttf_durationtype/tttf_nr_fees/tttf_xnr_fees/
        // tttf_r_fees/tttf_xr_fees/tttf_min_participant/tttf_ef_date) are
        // read verbatim off the DataTable rather than guessed at in
        // PascalCase, because the old JS itself reads these exact
        // lowercase/underscore names straight off the JSON response
        // (JsonObj["tttf_id"] etc., JS L3117) - the old API's serializer
        // preserves raw SQL column names as-is, so these are very likely the
        // real column names, not just a display convention.
        public List<Trg_Fees_Master> Get_Trg_Fees_Master(string feesid = null)
        {
            List<Trg_Fees_Master> feesdata = new List<Trg_Fees_Master>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_get_training_fees_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (feesid != null)
            {
                cmd.Parameters.AddWithValue("@tttf_id", feesid);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                Trg_Fees_Master f = new Trg_Fees_Master();
                if (dt.Columns.Contains("tttf_id")) { f.FeesId = Convert.ToString(row["tttf_id"]); }
                if (dt.Columns.Contains("tttf_trg_type")) { f.TrgType = Convert.ToString(row["tttf_trg_type"]); }
                if (dt.Columns.Contains("tttf_sponsor_type")) { f.SponsorType = Convert.ToString(row["tttf_sponsor_type"]); }
                if (dt.Columns.Contains("tttf_duration")) { f.Duration = Convert.ToString(row["tttf_duration"]); }
                if (dt.Columns.Contains("tttf_durationtype")) { f.DurationType = Convert.ToString(row["tttf_durationtype"]); }
                if (dt.Columns.Contains("tttf_nr_fees")) { f.NrFees = Convert.ToString(row["tttf_nr_fees"]); }
                if (dt.Columns.Contains("tttf_xnr_fees")) { f.XnrFees = Convert.ToString(row["tttf_xnr_fees"]); }
                if (dt.Columns.Contains("tttf_r_fees")) { f.RFees = Convert.ToString(row["tttf_r_fees"]); }
                if (dt.Columns.Contains("tttf_xr_fees")) { f.XrFees = Convert.ToString(row["tttf_xr_fees"]); }
                if (dt.Columns.Contains("tttf_min_participant")) { f.MinParticipant = Convert.ToString(row["tttf_min_participant"]); }
                if (dt.Columns.Contains("tttf_ef_date")) { f.EfDate = Convert.ToString(row["tttf_ef_date"]); }

                feesdata.Add(f);
            }

            return feesdata;
        }

        // Added 2026-07-11 (Fees tab, "Save" action). Old
        // TrainingErp_Save_Fees_Master() (JS L2866) calls
        // /TrainingApi/Save_Data (the GENERIC ProcedureName/Parameters
        // dispatcher, not a dedicated action like RCVP_SAVE_COURSE_MASTER)
        // with ProcedureName=[TrainingPlan].proc_tp_ins_upd_training_fees
        // and params @tttf_id/@tttf_trg_type/@tttf_sponsor_type/
        // @tttf_CreatedBy/@tttf_BranchId/@tttf_duration/@tttf_durationtype/
        // @tttf_nr_fees/@tttf_xnr_fees/@tttf_r_fees/@tttf_xr_fees/
        // @tttf_min_participant/@tttf_ef_date (JS L3020-3033). No `Domain`/
        // `IsOnline` param exists on the actual procedure - those two are
        // only part of the old generic Save_Data envelope, not real
        // stored-procedure parameters, which is why the new frontend payload
        // (see FeesTab.jsx) doesn't send a `domain` field either. Confirmed
        // gap: proc_tp_ins_upd_training_fees is not called anywhere in any
        // LitteraCoreReactAPI\DBContext\*.cs file (checked all 20) - do not
        // confuse with the similarly-named
        // trainingplan.proc_tp_ins_upd_trg_fees_amount, a different
        // procedure entirely (see the "Similarly-named != same procedure"
        // lesson in backend-api-notes.md).
        public bool Save_Trg_Fees_Master(Trg_Fees_Master fees)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("[TrainingPlan].proc_tp_ins_upd_training_fees", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tttf_id", fees.FeesId);
            cmd.Parameters.AddWithValue("@tttf_trg_type", fees.TrgType);
            cmd.Parameters.AddWithValue("@tttf_sponsor_type", fees.SponsorType);
            cmd.Parameters.AddWithValue("@tttf_CreatedBy", fees.CreatedBy);
            cmd.Parameters.AddWithValue("@tttf_BranchId", fees.BranchId);
            cmd.Parameters.AddWithValue("@tttf_duration", fees.Duration);
            cmd.Parameters.AddWithValue("@tttf_durationtype", fees.DurationType);
            cmd.Parameters.AddWithValue("@tttf_nr_fees", string.IsNullOrWhiteSpace(fees.NrFees) ? "0" : fees.NrFees);
            cmd.Parameters.AddWithValue("@tttf_xnr_fees", string.IsNullOrWhiteSpace(fees.XnrFees) ? "0" : fees.XnrFees);
            cmd.Parameters.AddWithValue("@tttf_r_fees", string.IsNullOrWhiteSpace(fees.RFees) ? "0" : fees.RFees);
            cmd.Parameters.AddWithValue("@tttf_xr_fees", string.IsNullOrWhiteSpace(fees.XrFees) ? "0" : fees.XrFees);
            cmd.Parameters.AddWithValue("@tttf_min_participant", string.IsNullOrWhiteSpace(fees.MinParticipant) ? "0" : fees.MinParticipant);
            cmd.Parameters.AddWithValue("@tttf_ef_date", fees.EfDate);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        // Added 2026-07-11 (Fees tab, "Delete" action). Old
        // TRG_FM_Delete_Fees() (JS L3472) also goes through the generic
        // Save_Data dispatcher with
        // ProcedureName=TrainingPlan.proc_delete_training_fees_data and a
        // single @tttf_id param. Confirmed gap - not called anywhere in any
        // DBContext file (checked all 20).
        public bool Delete_Trg_Fees_Master(string feesId)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_delete_training_fees_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tttf_id", feesId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        // Added 2026-07-11 (Fees tab, "in use?" check). Unlike Category's
        // check, this one has a REAL dedicated old action name -
        // /TrainingApi/CHECK_FEES_IN_Used (TRAININGAPIController.vb L75678,
        // real API_ERP_TRAINING project) - which calls
        // dm.TRG_CHECK_FEES_STATUS(Domain, isOnline, feesid) ->
        // Select [TRAININGPLAN].[F_proc_tp_check_used_fees] ('<feesid>')
        // (Datamanager.vb L6963, a SQL scalar function, string-concatenated
        // in the old code rather than parameterized - parameterized here
        // instead, same as Category's check). IMPORTANT - opposite polarity
        // from Category's check: here the old JS
        // (TRG_FM_CHECK_FEE_STATUS, JS L3424) treats `isused == "1"` as WHAT
        // BLOCKS the action (edit or delete) - "in use" means exactly what
        // it says here, unlike Category's confusingly-named check. Preserve
        // this polarity as-is; don't normalize it to match Category's.
        public bool Chk_Fees_In_Use(string feesId)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Select [TRAININGPLAN].[F_proc_tp_check_used_fees] (@FeesId)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@FeesId", feesId);
            object result = cmd.ExecuteScalar();
            con.Close();

            string isused = Convert.ToString(result);
            return isused == "1" || string.Equals(isused, "true", StringComparison.OrdinalIgnoreCase);
        }

        public bool Update_Training_Status(string trainingid, int trainingstatus, string reason, string createdby, string branchid)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.TP_UpdTrainingStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@TrainingId", trainingid);
            cmd.Parameters.AddWithValue("@TrainingStatus", trainingstatus);
            cmd.Parameters.AddWithValue("@StatusUpdateDate", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            cmd.Parameters.AddWithValue("@CreatedBy", createdby);
            if (reason != null)
            {
                cmd.Parameters.AddWithValue("@StatusReason", reason);
            }
            else
            {
                cmd.Parameters.AddWithValue("@StatusReason", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@BranchId", branchid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }



        public bool Update_Bulk_Trg_Participant_Status(string participantid, string trainingid, string branchid, string currentstatus, string updatedstatus, string createdbyempid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_training_update_participant_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@ParticipantId", participantid);
            }
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@TrainingId", trainingid);
            }
            if (branchid != null)
            {
                cmd.Parameters.AddWithValue("@branchid", branchid);
            }
            cmd.Parameters.AddWithValue("@currentstatus", currentstatus);
            cmd.Parameters.AddWithValue("@updatedstatus", updatedstatus);
            cmd.Parameters.AddWithValue("@createdbyempid", createdbyempid);


            cmd.ExecuteNonQuery();

            con.Close();

            return true;
        }


        public Certificate_Details Get_Certificate_details(string ttpai_id)
        {
            Certificate_Details c = new Certificate_Details();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("trainingplan.proc_tp_get_certificate_details", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ttpai_id", ttpai_id);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {

                c = new Certificate_Details
                {
                    name =  Convert.ToString(row["name"]),
                    enrollmentno = Convert.ToString(row["enrollment_no"]),
                    grade = Convert.ToString(row["grade"]),
                    printdate = System.DateTime.Now.ToString("dd-MM-yyyy"),
                    trainingid= Convert.ToString(row["trainingid"]),
                    participantid= Convert.ToString(row["participantid"]),
                    ttpai_id= Convert.ToString(row["ttpai_id"]),
                    mobileno= Convert.ToString(row["ag_mobileno"]).Replace("91-","") 
                    
                };
                
            }
            UserDB udb = new UserDB(_configuration);
            c.userid = udb.get_user_id_by_agencyid(c.participantid);
            return c;
        }
        public bool Update_Training_Rating_Data()
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("[trainingplan].[proc_tp_insert_avg_rating_per_training]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }


        public bool Update_Certificate_Signatory(string trainingid, string signatoryid,string loginuserid)
        {
            bool isexist = false;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_ins_upd_certificate_signatory", con);
            cmd.Parameters.AddWithValue("@ttcs_trainingid", trainingid);
            cmd.Parameters.AddWithValue("@ttcs_agenyid", signatoryid);
            cmd.Parameters.AddWithValue("@ttcs_created_by", loginuserid);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();

          
            return true;
        }
        public bool Update_Certificate_status_Data(string trainingid,string Loginuserid,cert_status_list cert_status)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("[trainingplan].[proc_tp_insupd_certificate_status]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ttcgs_trainingid", trainingid);
            string p1 = JsonConvert.SerializeObject(cert_status.certificate_Statuses);
            cmd.Parameters.AddWithValue("@jsondata", p1);
            cmd.Parameters.AddWithValue("@ttcgs_createdon", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            cmd.Parameters.AddWithValue("@createdby", Loginuserid);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }


        public List<certificate_status> Get_certificate_status(string trainingid)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<certificate_status> trgdata = new List<certificate_status>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_certificate_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Get Data" + System.DateTime.Now);
            foreach (DataRow row in dt.Rows)
            {
                certificate_status vw = new certificate_status();
                if(Convert.ToString(row["ttcgs_status"]) != "")
                {
                    vw.ttcgs_status = Convert.ToInt32(row["ttcgs_status"]);
                }
         
                vw.ttcgs_agenyid = Convert.ToString(row["ttcgs_agenyid"]);
               

                trgdata.Add(vw);
            }



            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        public int Get_Participant_Certificates(string agencyid)
        {
            int total_Certificates = 0;
            List<certificate_status> trgdata = new List<certificate_status>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            if (!Guid.TryParse(agencyid, out Guid agencyGuid))
            {
                throw new ArgumentException("Invalid agency id.", nameof(agencyid));
            }
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS certificates FROM trainingplan.tbl_tp_certificate_generate_status WHERE ttcgs_agenyid = @AgencyId", con);
            cmd.Parameters.Add("@AgencyId", SqlDbType.UniqueIdentifier).Value = agencyGuid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                total_Certificates = Convert.ToInt16(dt.Rows[0]["certificates"]);
            }
           

            return total_Certificates;
        }



        public List<usertrainings> Get_participants_Training(string participantid)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<usertrainings> trgdata = new List<usertrainings>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_participant_trainings", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@participantid", participantid);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow row in dt.Rows)
            {
                usertrainings vw = new usertrainings();
                vw.trainingid = Convert.ToString(row["trainingid"]);
                vw.trainingcode = Convert.ToString(row["TrainingNo"]);
                vw.training_title = Convert.ToString(row["t_name"]);


                if (Convert.ToString(row["trg_setting"]) != "")
                {
                    try
                    {
                        Trg_Setting p = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(row["trg_setting"]));
                        vw.trg_Setting = p;
                      

                    }
                    catch
                    {
                        vw.trg_Setting = null;
                    }
                }
                else
                {
                    vw.trg_Setting = null;
                }

                trgdata.Add(vw);
            }



            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        public List<Session> Get_Trg_Progress_Data(string trainingid,string sessionid, string login_user_id,string login_user_type,int status, string branchid = null, int pageno = 0, int pagesize = 0, int ismaskingrequired = 0, string formid = null, string searchcolumn = null, string searchvalue = null)
        {
        
            List<Session> sessiondata = new List<Session>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State == ConnectionState.Open) { con.Close(); }
            con.Open();
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("trainingplan.proc_tp_get_session_completion_status_participantwise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            if(login_user_id != null)
            {
                cmd.Parameters.AddWithValue("@loginuserid", login_user_id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@loginuserid", DBNull.Value);
            }
           if(login_user_type != null)
            {
                cmd.Parameters.AddWithValue("@loginusertype", login_user_type);
            }
            else
            {
                cmd.Parameters.AddWithValue("@loginusertype", DBNull.Value);
            }
           
            cmd.Parameters.AddWithValue("@sessionid", sessionid);
          
            if (branchid != null)
            {
                cmd.Parameters.AddWithValue("@branchid", branchid);
            }
            


            cmd.Connection = con;
            cmd.CommandTimeout = 120;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
          

            ParticipantDB WDB = new ParticipantDB(_configuration);
            List<Participant> trgparticipants = new List<Participant>();
            if (status == 2)
            {
                trgparticipants = WDB.Get_Trg_Participant_List(trainingid, null, branchid, searchcolumn, searchvalue, null, null, null, null, pageno, pagesize,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            }
            else
            {
                trgparticipants = WDB.Get_Trg_Participant_List(trainingid, null, branchid, searchcolumn, searchvalue, null, null, null, null,0,0,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            }
           

            List<Session> LS = new List<Session>();
            Session vw = new Session();
            List<completionDetail> cp = new List<completionDetail>();
            if (dt.Rows.Count > 0)
            {
                vw.trainingid = Convert.ToString(dt.Rows[0]["ttam_trainingid"]);
                vw.ttttt_session_id = Convert.ToString(dt.Rows[0]["ttam_session_id"]);
                vw.noofcompletion = Convert.ToInt32(dt.Rows[0]["iscompleted"]);
            }

            //foreach (Participant p in trgparticipants)
            //{
            //    dt.DefaultView.RowFilter = "ttam_session_id='" + Convert.ToString(sessionid) + "' and tta_ttpai_id='" + p.ttpai_id + "'";
            //    DataTable dtfilterdata1 = dt.DefaultView.ToTable();

            //    if (dtfilterdata1.Rows.Count > 0)
            //    {
            //        cp.Add(new completionDetail { agencyid = p.ParticipantId, agencyname = p.ParticipantName, status = "Completed", emailid = p.email, mobileno = p.mobileno });
            //    }
            //    else
            //    {
            //        cp.Add(new completionDetail { agencyid = p.ParticipantId, agencyname = p.ParticipantName, status = "Pending", emailid = p.email, mobileno = p.mobileno });
            //    }


            //    //}
            //    //var filteredDt = dt.AsEnumerable()
            //    //       .Where(r => r.Field<string>("ttam_session_id") == Convert.ToString(sessionid))
            //    //       .ToList();

            //    //        var filteredDt = dt.AsEnumerable()
            //    //.Where(r => r.Field<Guid>("ttam_session_id").ToString() == sessionid.ToString())
            //    //.ToList();

            //    //        var result1 = from p in trgparticipants
            //    //                     join r in filteredDt
            //    //                    on p.ttpai_id.ToString()
            //    //   equals r.Field<Guid>("tta_ttpai_id").ToString()  into pr
            //    //                     from r in pr.DefaultIfEmpty()
            //    //                     select new completionDetail
            //    //                     {
            //    //                         agencyid = p.ParticipantId,
            //    //                         agencyname = p.ParticipantName,
            //    //                         status = r != null ? "Completed" : "Pending",
            //    //                         emailid = p.email,
            //    //                         mobileno = p.mobileno
            //    //                     };



            //}

            var completedSet = new HashSet<string>(
    dt.AsEnumerable()
      .Select(r => $"{r["ttam_session_id"]}|{r["tta_ttpai_id"]}")
);

            foreach (Participant p in trgparticipants)
            {
                string key = $"{sessionid}|{p.ttpai_id}";
                bool exists = completedSet.Contains(key);

                cp.Add(new completionDetail
                {
                    agencyid = p.ParticipantId,
                    agencyname = p.ParticipantName,
                    status = exists ? "Completed" : "Pending",
                    emailid = p.email,
                    mobileno = p.mobileno
                });
            }

            if (status == 0)
            {
                cp = cp.Where(o => o.status.ToString().ToUpper() == "PENDING").ToList();
            }
            else if (status == 1)
            {
                cp = cp.Where(o => o.status.ToString().ToUpper() == "COMPLETED").ToList();
            }

            int totalrecord= trgparticipants.FirstOrDefault().totalrecords;
            int final_page_no = 0;
            int final_total_count = 0;
            if (status == 2)
            {
                final_page_no = 1;
                final_total_count = totalrecord;
            }
            else
            {
                final_page_no = pageno;
                final_total_count = cp.Count();
            }
            PaginationParam param = new PaginationParam
            {
                PageNumber = 1,
                PageSize = pagesize
            };
            
            var result = Paging.GetPagedData(param, cp);
            if (trgparticipants.Count() > 0)
            {
                result.TotalRecords = final_total_count;
                result.TotalPages = (int)Math.Ceiling(final_total_count / (double)param.PageSize);
                result.CurrentPage = pageno;

            }


            vw.completiondetail = result;

            sessiondata.Add(vw);

          
          

            // sessiondata = sessiondata.Where(o => o.ttttt_timetableid != null).ToList();

            return sessiondata;
        }
    }
}
