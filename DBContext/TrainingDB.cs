using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
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
        public List<Training> Get_VW_Training_calendar(DateTime fromdate, DateTime todate)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Training> trgdata = new List<Training>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from  trainingplan.VW_Training_calendar where  (T_StartDate >= '" + fromdate.ToString("yyyy/MM/dd") + "' or T_ClosingDate>='" + fromdate.ToString("yyyy/MM/dd") + "') and T_StartDate <='" + todate.ToString("yyyy/MM/dd") + "' order by T_StartDate desc", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
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

            con.Open();
            SqlCommand cmd = new SqlCommand("select * from [TrainingPlan].[Ft_tp_get_trgid_for_usertype]('" + usertype + "','" + userid + "','" + fromdate.ToString("yyyy/MM/dd") + "','" + todate.ToString("yyyy/MM/dd") + "')", con);
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
            TrainingDB cdb = new TrainingDB(_configuration);
            List<UserTrg> usertrg = cdb.Get_Users_Training(usertype, userid, fromdate, todate);
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
            con.Open();
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

        public List<TRG_DAY_WEEK> Get_Day_Week_Count_Id(string fromdate, string todate)
        {
            List<TRG_DAY_WEEK> dayweek = new List<TRG_DAY_WEEK>();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select  *  from [trainingplan].[ft_tp_get_week_day_count]('" + fromdate + "','" + todate + "')", con);
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
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from trainingplan.VW_Training_calendar where TrainingId = @TrainingId", con);
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
            con.Open();
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
            con.Open();
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
            con.Open();
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
    }
}
