using LitteraCore.BLContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerBI.Api.Models;
using System.Data;
using System.Data.Common;
using System.Net.NetworkInformation;
using static LitteraCore.Common.CommonEnum;
using static System.Reflection.Metadata.BlobBuilder;

namespace LitteraCore.DBContext
{
    public class MentorDB
    {
        private readonly IConfiguration _configuration;
        public MentorDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Save_Mentor_slot(Mentor_slot m)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_save_trg_session_slot", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if(m.ttsl_id != null)
            {
                cmd.Parameters.AddWithValue("@ttmss_id", m.ttsl_id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttmss_id", DBNull.Value);
            }
           
            cmd.Parameters.AddWithValue("@ttmss_training_id", m.ttsl_training_id);
            cmd.Parameters.AddWithValue("@ttmss_session_id", m.ttsl_session_id);
            cmd.Parameters.AddWithValue("@ttmss_mentor_id", m.ttsl_mentor_id);
            cmd.Parameters.AddWithValue("@ttmss_slot_date", m.ttsl_slot_date);
            cmd.Parameters.AddWithValue("@ttmss_start_time", m.ttsl_start_time);
            cmd.Parameters.AddWithValue("@ttmss_duration", m.ttsl_duration);
            cmd.Parameters.AddWithValue("@ttmss_seats", m.ttsl_seats);
            cmd.Parameters.AddWithValue("@ttmss_mode", m.ttsl_mode);

            cmd.Parameters.AddWithValue("@ttmss_status", m.ttsl_status);
            cmd.Parameters.AddWithValue("@ttmss_link", m.ttsl_link);
            cmd.Parameters.AddWithValue("@ttmss_location", m.ttsl_location);
            cmd.Parameters.AddWithValue("@ttmss_createdon", m.ttsl_createdon);
            cmd.Parameters.AddWithValue("@ttmss_created_by", m.ttsl_created_by);

            cmd.ExecuteNonQuery();


            return true;
        }
        public bool Delete_Mentor_slot(string ttsl_id,string createdby)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_delete_trg_session_slot", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttmss_id", ttsl_id);
            cmd.Parameters.AddWithValue("@deleted_by", createdby);

            cmd.ExecuteNonQuery();


            return true;
        }

        public List<Mentor_slot> Get_Mentor_Slot(string ttsl_training_id, string ttsl_session_id, string ttsl_mentor_id, string slot_date,int status, PaginationParam param)
        {

            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_get_trg_session_slots", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttmss_training_id", ttsl_training_id);
            cmd.Parameters.AddWithValue("@ttmss_session_id", ttsl_session_id);
            cmd.Parameters.AddWithValue("@ttmss_mentor_id", ttsl_mentor_id);
            if(slot_date == null)
            {
                cmd.Parameters.AddWithValue("@slot_date", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@slot_date", slot_date);
            }
          
            cmd.Parameters.AddWithValue("@status", status);
           
           
            if (param.PageSize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", param.PageNumber);
                cmd.Parameters.AddWithValue("@PageSize", param.PageSize);
            }


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Mentor_slot> LI = new List<Mentor_slot>();
            foreach (DataRow row in dt.Rows)
            {
                Mentor_slot exp = new Mentor_slot();
                exp.ttsl_id = Convert.ToString(row["SlotID"]);
                exp.ttsl_session_id = Convert.ToString(row["SessionID"]);
                exp.ttsl_training_id = Convert.ToString(row["TrainingID"]);
                exp.ttsl_mentor_id = Convert.ToString(row["MentorID"]);
                exp.ttsl_slot_date = Convert.ToString(row["SlotDate"]);
                exp.ttsl_start_time = Convert.ToString(row["StartTime"]);
                exp.ttsl_duration = Convert.ToDecimal(row["Duration"]);
                exp.ttsl_seats = Convert.ToInt16(row["no_of_seats"]);
                exp.ttsl_mode = Convert.ToInt16(row["SlotMode"]);
                exp.ttsl_status = Convert.ToInt16(row["Status"]);

                exp.ttsl_link = Convert.ToString(row["MeetingLink"]);
                exp.ttsl_location = Convert.ToString(row["meetingLocation"]);


                exp.ttsl_createdon = Convert.ToDateTime(row["CreatedOn"]);
                exp.mentor_name = Convert.ToString(row["mentorname"]);
            

                LI.Add(exp);

            }



            return LI;
        }


        public List<Mentor_slot> Get_Mentor_Session_Slots(string ttsl_training_id, string ttsl_session_id, string ttsl_mentor_id, int status)
        {
            SessionDB sdb = new SessionDB(_configuration);
            Session s = new Session();
            if(ttsl_session_id != null)
            {
                s=sdb.Get_Session_Details(ttsl_session_id);
            }
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_get_trg_session_slots", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttmss_training_id", ttsl_training_id);
            if(ttsl_session_id == null)
            {
                cmd.Parameters.AddWithValue("@ttmss_session_id", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttmss_session_id", ttsl_session_id);
            }
            
            cmd.Parameters.AddWithValue("@ttmss_mentor_id", ttsl_mentor_id);
            cmd.Parameters.AddWithValue("@status", status);




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Mentor_slot> LI = new List<Mentor_slot>();
            foreach (DataRow row in dt.Rows)
            {
                Mentor_slot exp = new Mentor_slot();
                exp.ttsl_id = Convert.ToString(row["SlotID"]);
                exp.ttsl_session_id = Convert.ToString(row["SessionID"]);
                exp.ttsl_training_id = Convert.ToString(row["TrainingID"]);
                exp.ttsl_mentor_id = Convert.ToString(row["MentorID"]);
                exp.ttsl_slot_date = Convert.ToString(row["SlotDate"]);
                exp.ttsl_start_time = Convert.ToString(row["StartTime"]);
                exp.ttsl_duration = Convert.ToDecimal(row["Duration"]);
                exp.ttsl_seats = Convert.ToInt16(row["no_of_seats"]);
                exp.ttsl_mode = Convert.ToInt16(row["SlotMode"]);
                exp.ttsl_status = Convert.ToInt16(row["Status"]);

                exp.ttsl_link = Convert.ToString(row["MeetingLink"]);
                exp.ttsl_location = Convert.ToString(row["meetingLocation"]);


                exp.ttsl_createdon = Convert.ToDateTime(row["CreatedOn"]);
                exp.mentor_name = Convert.ToString(row["mentorname"]);
                exp.slot_title= s.ttttt_content_desc;

                LI.Add(exp);

            }



            return LI;
        }



        public List<session_slots> Get_Session_Mentor_Slot(string ttsl_session_id)
        {
            List<session_slots> ss = new List<session_slots>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_get_trg_session_slots", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
      
            cmd.Parameters.AddWithValue("@ttmss_session_id", ttsl_session_id);
           
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Mentor_slot> LI = new List<Mentor_slot>();
            foreach (DataRow row in dt.Rows)
            {
                Mentor_slot exp = new Mentor_slot();
                exp.ttsl_id = Convert.ToString(row["SlotID"]);
                exp.ttsl_session_id = Convert.ToString(row["SessionID"]);
                exp.ttsl_training_id = Convert.ToString(row["TrainingID"]);
                exp.ttsl_mentor_id = Convert.ToString(row["MentorID"]);
                exp.ttsl_slot_date = Convert.ToString(row["SlotDate"]);
                exp.ttsl_start_time = Convert.ToString(row["StartTime"]);
                exp.ttsl_duration = Convert.ToDecimal(row["Duration"]);
                exp.ttsl_seats = Convert.ToInt16(row["no_of_seats"]);
                exp.ttsl_mode = Convert.ToInt16(row["SlotMode"]);
                exp.ttsl_status = Convert.ToInt16(row["Status"]);

                exp.ttsl_link = Convert.ToString(row["MeetingLink"]);
                exp.ttsl_location = Convert.ToString(row["meetingLocation"]);


                exp.ttsl_createdon = Convert.ToDateTime(row["CreatedOn"]);
                exp.mentor_name = Convert.ToString(row["mentorname"]);


                LI.Add(exp);

            }
            LI = LI.Where(o => o.ttsl_status == 1).ToList();
            List<Agency> mentor = new List<Agency>();
            AgencyDB adb=new AgencyDB(_configuration);
            mentor = adb.Get_Agency("00054", null, 0, 0, null, null, null, null, null);

            var distinctMentorIds = LI
     .Select(x => x.ttsl_mentor_id)
     .Distinct()
     .ToList();
           foreach(string s in distinctMentorIds)
            {
                session_slots sslot=new session_slots();
                sslot.mentorid = s;
                if(mentor.Where(o => o.agencyid.ToString().ToUpper() == s.ToString().ToUpper()).Count()>0)
                {
                    sslot.mentorname = mentor.Where(o => o.agencyid.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault().agencyname;
                    if(mentor.Where(o => o.agencyid.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault().additionalInfo != null)
                    {
                       if(mentor.Where(o => o.agencyid.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault().additionalInfo.DESIGNATION != null)
                        {
                            sslot.designation = mentor.Where(o => o.agencyid.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault().additionalInfo.DESIGNATION;
                        }
                    }
                  
                    sslot.Rating = 4.5M;
                    sslot.imagepath= mentor.Where(o => o.agencyid.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault().ag_photo_path;

                }    
               
                sslot.Mentor_slot = LI.Where(o => o.ttsl_mentor_id.ToString().ToUpper() == s.ToString().ToUpper()).ToArray();
                ss.Add(sslot);

            }

            return ss;
        }

        public bool Save_slot_Participant(slot_participant m)
        {

          
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_slot_participant_save", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
           

            cmd.Parameters.AddWithValue("@ttmssp_id", m.ttmssp_id);
            cmd.Parameters.AddWithValue("@ttmssp_ttmss_id", m.ttmssp_ttmss_id);
            cmd.Parameters.AddWithValue("@ttmssp_ttpai_id", m.ttmssp_ttpai_id);
            cmd.Parameters.AddWithValue("@ttmssp_participant_id", m.ttmssp_participant_id);
            cmd.Parameters.AddWithValue("@ttmssp_created_by", m.ttmssp_created_by);
          

            cmd.ExecuteNonQuery();


            return true;
        }

        public bool Delete_slot_Participant(string slotid, string participantlist)
        {


            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_mss_participant_delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            cmd.Parameters.AddWithValue("@ttmssp_ttmss_id", slotid);
            cmd.Parameters.AddWithValue("@ParticipantList", participantlist);
          

            cmd.ExecuteNonQuery();


            return true;
        }


        public List<slot_participant> Get_Slot_Participant(string slotid,string participantid=null)
        {
            List<session_slots> ss = new List<session_slots>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_mentor_mss_participant_get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.Parameters.AddWithValue("@ttmss_id", slotid);
            cmd.Parameters.AddWithValue("@ttmssp_ttpai_id", participantid);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<Agency> AL = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            AL = ABD.Get_Agency("00051",null, 0, 0, null, null, null, null, null);

            List<slot_participant> LI = new List<slot_participant>();
            foreach (DataRow row in dt.Rows)
            {
                slot_participant exp = new slot_participant();
                exp.ttmssp_id = Convert.ToString(row["ttmssp_id"]);
                exp.ttmssp_ttmss_id = Convert.ToString(row["ttmssp_ttmss_id"]);
                exp.ttmssp_ttpai_id = Convert.ToString(row["ttmssp_ttpai_id"]);
                exp.ttmssp_participant_id = Convert.ToString(row["ttmssp_participant_id"]);
                Agency a= new Agency();
                if(AL.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["ttmssp_participant_id"]).ToString().ToUpper()).ToList().Count() > 0)
                {
                    a = AL.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["ttmssp_participant_id"]).ToString().ToUpper()).ToList().FirstOrDefault();
                    exp.participant_name = a.agencyname;
                    exp.participant_email =a.ag_email;
                    exp.participant_mobileno = a.ag_mobileno;
                }


                exp.ttmssp_created_by = Convert.ToString(row["ttmssp_created_by"]);


                LI.Add(exp);

            }
         
            return LI;
        }



    }


}
