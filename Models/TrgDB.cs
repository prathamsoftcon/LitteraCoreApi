using LitteraCore.BLContext;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
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
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("select * from trainingplan.VW_Training_calendar where TrainingId=@TrainingId", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TrainingId", trainingid);
                    cmd.CommandTimeout = 5000;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Training vw = new Training
                            {
                                TrainingId = Guid.Parse(Convert.ToString(reader["Trainingid"])),
                                TrainingNo = Convert.ToString(reader["TrainingNo"]),
                                Trainingcode = Convert.ToString(reader["Trainingcode"]),
                                CourseCode = Convert.ToString(reader["Trainingcode"]),  // Changed here
                                T_Name = Convert.ToString(reader["T_Name"]),
                                T_Details = Convert.ToString(reader["T_Details"]),
                                SPONSOR_AG_ID = Guid.Parse(Convert.ToString(reader["SPONSOR_AG_ID"])),
                                DueFees = Convert.ToDecimal(reader["DueFees"]),
                                ReceivedFees = Convert.ToInt32(reader["ReceivedFees"]),
                                HSponsorName = Convert.ToString(reader["HSponsorName"]),
                                ParticipantLevel = Convert.ToString(reader["ParticipantLevel"]),
                                LevelId = Convert.ToInt32(reader["LevelId"]),
                                LevelDescription = Convert.ToString(reader["LevelDescription"]),
                                HLevelDescription = Convert.ToString(reader["HLevelDescription"]),
                                CourseDirector = Guid.Parse(Convert.ToString(reader["CourseDirector"])),
                                CourseDirectorName = Convert.ToString(reader["CourseDirectorName"]),
                                HCourseDirectorName = Convert.ToString(reader["HCourseDirectorName"]),
                                AssociateDirector = Guid.Parse(Convert.ToString(reader["AssociateDirector"])),
                                AssociateDirectorName = Convert.ToString(reader["AssociateDirectorName"]),
                                HAssociateDirectorName = Convert.ToString(reader["HAssociateDirectorName"]),
                                Duration = Convert.ToInt32(reader["Duration"]),
                                DurationType = Convert.ToString(reader["DurationType"]),
                                T_StartDate = Convert.ToDateTime(reader["T_StartDate"]),
                                T_EndDate = Convert.ToDateTime(reader["T_EndDate"]),
                                T_ClosingDate = Convert.ToDateTime(reader["T_ClosingDate"]),
                                TrainingCategoryId = Guid.Parse(Convert.ToString(reader["TrainingCategoryId"])),
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
                                CourseId = Guid.Parse(Convert.ToString(reader["CourseId"])),
                                benefitted = Convert.ToString(reader["benefitted"]),
                                objective = Convert.ToString(reader["objective"]),
                                prerequiste = Convert.ToString(reader["prerequiste"]),
                                img_path = Convert.ToString(reader["img_path"]),
                                trg_type = Convert.ToByte(reader["trg_type"]),
                                trg_validity = Convert.ToString(reader["trg_validity"]),
                                tttt_name = Convert.ToString(reader["tttt_name"]),
                                tttt_hname = Convert.ToString(reader["tttt_hname"]),
                                exptype = Convert.ToInt32(reader["exptype"]),
                                resident_status = Convert.ToByte(reader["resident_status"]),
                                CourseName = Convert.ToString(reader["CourseName"]),
                                HCourseName = Convert.ToString(reader["HCourseName"]),
                                DepartmentReferenceNo = Convert.ToString(reader["DepartmentReferenceNo"]),
                                participation_type = Convert.ToInt32(reader["participation_type"]),
                                proposed_amt = Convert.ToDecimal(reader["proposed_amt"]),
                                participant_type = Convert.ToInt32(reader["participant_type"])
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("tttf_id")))
                            {
                                vw.tttf_id = Guid.Parse(Convert.ToString(reader["tttf_id"]));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("ChcekListType")))
                            {
                                vw.ChcekListType = Guid.Parse(Convert.ToString(reader["ChcekListType"]));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("FeedbackType")))
                            {
                                vw.FeedbackType = Guid.Parse(Convert.ToString(reader["FeedbackType"]));
                            }

                            vw.isSelfPaced = Get_Self_Paced_Trg(Convert.ToString(reader["trg_type"]));

                            // Deserialize trg_setting if not empty
                            if (!reader.IsDBNull(reader.GetOrdinal("trg_setting")))
                            {
                                try
                                {
                                    vw.trg_Setting = JsonConvert.DeserializeObject<Trg_Setting>(Convert.ToString(reader["trg_setting"]));
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

                            // Enum conversion
                            vw.Participant_type_name = ((Common.CommonEnum.ParticipantType)Convert.ToInt32(reader["participant_type"])).ToString();
                            vw.Participantion_type_name = ((Common.CommonEnum.ParticipationType)Convert.ToInt32(reader["participation_type"])).ToString();

                            // Aggregate sponsor data
                            int registered_participantcount = 0;
                            int proposed_participantcount = 0;
                            string sponsorname = "";
                            List<TRGSPONSORS> sp = new List<TRGSPONSORS>();

                            // Note: Instead of iterating over `dt.Rows` again, you'll need to query this sponsor information in another query or structure.
                            // For now, this logic should be refactored accordingly.

                            vw.trgsponsors = sp.ToArray();
                            vw.NoOfParticipants = proposed_participantcount;
                            vw.NoOfParticipants_Registered = registered_participantcount;
                            if (!string.IsNullOrEmpty(sponsorname))
                            {
                                sponsorname = sponsorname.TrimEnd(',');
                            }
                            vw.SponsorName = sponsorname;

                            // Calculate amounts
                            Hashtable ht = new Hashtable();
                            TrgBL tbl = new TrgBL(_configuration);
                            ht = tbl.Calculate_trg_actual_amt(vw.TrainingId.ToString(), vw.exptype.ToString(), vw.Training_SponsorType.ToString(), vw.NoOfParticipants_Registered);

                            vw.amt_per_participant = Convert.ToDecimal(ht["feesperparticipant"]);
                            vw.t_actual_amt = Convert.ToDecimal(ht["actual_fees"]);
                            vw.total_received_amt = tbl.Calculate_trg_received_amt(vw.TrainingId.ToString());

                            trgdata.Add(vw);
                        }
                    }

                }
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
