using LitteraCore.BLContext;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;

namespace LitteraCore.Models
{
    public class TrgDB
    {
        private readonly IConfiguration _configuration;
        public TrgDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Training> Get_Particular_Training(string trainingid)
        {

            List<Training> trgdata = new List<Training>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from  trainingplan.VW_Training_calendar where  TrainingId='" + trainingid + "'", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

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

                vw.T_ClosingDate = Convert.ToDateTime(row["T_ClosingDate"]);

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
                vw.isSelfPaced = Get_Self_Paced_Trg(row["trg_type"].ToString());

                vw.Participant_type_name = ((Common.CommonEnum.ParticipantType)row["participant_type"]).ToString();
                vw.Participantion_type_name = ((Common.CommonEnum.ParticipationType)row["participation_type"]).ToString();
                int registered_participantcount = 0;
                int proposed_participantcount = 0;
                string sponsorname = "";
                List<TRGSPONSORS> sp = new List<TRGSPONSORS>();
                foreach (DataRow row1 in dt.Rows)
                {
                    sp.Add(new TRGSPONSORS { sponsorid = row1["SPONSOR_AG_ID"].ToString(), sponsorname = row1["SponsorName"].ToString(), proposed_participant = row1["NoOfParticipants"].ToString(), registered_participant = row1["NoOfParticipants_Registered"].ToString() });
                    registered_participantcount = registered_participantcount + Convert.ToInt32(row1["NoOfParticipants_Registered"]);
                    proposed_participantcount = proposed_participantcount + Convert.ToInt32(row1["NoOfParticipants"]);
                    sponsorname = sponsorname + Convert.ToString(row1["SponsorName"]) + ",";
                }
                vw.trgsponsors = sp.ToArray();
                vw.NoOfParticipants = proposed_participantcount;
                vw.NoOfParticipants_Registered = registered_participantcount;
                if (sponsorname != "")
                {
                    sponsorname = sponsorname.Substring(0, sponsorname.Length - 1);
                }
                vw.SponsorName = sponsorname;

                Hashtable ht = new Hashtable();
                TrgBL tbl = new TrgBL(_configuration);
                ht = tbl.Calculate_trg_actual_amt(Convert.ToString(row["TrainingId"]), Convert.ToString(row["exptype"]), Convert.ToString(row["Training_SponsorType"]), Convert.ToInt32(row["NoOfParticipants_Registered"]));


                vw.amt_per_participant = Convert.ToDecimal(ht["feesperparticipant"]);
                vw.t_actual_amt = Convert.ToDecimal(ht["actual_fees"]);
                vw.total_received_amt = tbl.Calculate_trg_received_amt(Convert.ToString(row["TrainingId"]));
                trgdata.Add(vw);
            }





            return trgdata;
        }
        public static int Get_Self_Paced_Trg(string trg_type)
        {
            string[] selfarr = { "2" };
            if (selfarr.Contains(trg_type) == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public DataTable Get_Trg_fees_details(string trainingid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_training_fees_details", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@TrainingId", trainingid);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable Get_Trg_received_Amt(string trainingid)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from TrainingPlan.[VW_TotalFees_byReceiptID] where trainingid='" + trainingid + "'", con);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return dt;
        }
    }
    
}
