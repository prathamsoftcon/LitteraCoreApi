using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;
using Microsoft.PowerBI.Api.Models;
using System.Data;

namespace LitteraCore.BLContext
{
    public class MentorBL
    {
        private readonly IConfiguration _configuration;
        public MentorBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Save_Mentor_slot(Mentor_slot m)
        {
            bool issaved = false;
            MentorDB MDB = new MentorDB(_configuration);
            issaved= MDB.Save_Mentor_slot(m);
            return issaved;
        }
        public bool Delete_Mentor_slot(string ttsl_id, string createdby)
        {
            bool issaved = false;
            MentorDB MDB = new MentorDB(_configuration);
            issaved = MDB.Delete_Mentor_slot(ttsl_id, createdby);
            return issaved;
        }
        public List<Mentor_slot> Get_Mentor_Slot(string ttsl_training_id, string ttsl_session_id, string ttsl_mentor_id, string slot_date, int status, PaginationParam param)
        {
            List<Mentor_slot> LM=new List<Mentor_slot>();
            MentorDB MDB = new MentorDB(_configuration);
            LM = MDB.Get_Mentor_Slot(ttsl_training_id, ttsl_session_id, ttsl_mentor_id, slot_date, status, param);
            return LM;
        }
        public List<session_slots> Get_Session_Mentor_Slot(string ttsl_session_id)
        {
            List<session_slots> LM = new List<session_slots>();
            MentorDB MDB = new MentorDB(_configuration);
            LM = MDB.Get_Session_Mentor_Slot(ttsl_session_id);
            return LM;
        }
        public bool Save_slot_Participant(slot_participant m)
        {


            bool issaved = false;
            MentorDB MDB = new MentorDB(_configuration);
            issaved = MDB.Save_slot_Participant(m);
            return issaved;
       }
        public bool Delete_slot_Participant(string slotid, string participantlist)
        {   
            bool issaved = false;
            MentorDB MDB = new MentorDB(_configuration);
            issaved = MDB.Delete_slot_Participant(slotid, participantlist);
            return issaved;


        }
        public List<slot_participant> Get_Slot_Participant(string slotid, string participantlist)
        {
            List<slot_participant> LM = new List<slot_participant>();
            MentorDB MDB = new MentorDB(_configuration);
            LM = MDB.Get_Slot_Participant(slotid, participantlist);
            return LM;
        }
    }
}
