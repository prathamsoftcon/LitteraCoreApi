using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.BLContext
{
    public class MeetingBL
    {
        private readonly IConfiguration _configuration;
        public MeetingBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Meeting> Get_Meetings(string finyear, string branchid, string usertype, string userid, PaginationParam param)
        {

            List<Meeting> LI = new List<Meeting>();
            MeetingDB mbl = new MeetingDB(_configuration);
            LI = mbl.Get_Meetings(finyear,branchid,usertype,userid,param);

            return LI;
        }
        public bool Delete_Meeting(string meetingid)
        {
            bool isdeleted=false;
           
            MeetingDB mbl = new MeetingDB(_configuration);
            isdeleted = mbl.Delete_Meeting(meetingid);

            return isdeleted;
        }

        // Added 2026-09-25 - frm_session_meeting.aspx -> React migration.
        public bool Save_Meeting(MeetingSaveRequest meeting)
        {
            // Old page always client-generated the row id (guid() for a new
            // meeting, the ?m= id when editing). Keep that contract, but never
            // let an empty id reach the proc.
            if (string.IsNullOrWhiteSpace(meeting.ttlm_id))
            {
                meeting.ttlm_id = Guid.NewGuid().ToString();
            }

            MeetingDB mdb = new MeetingDB(_configuration);
            return mdb.Save_Meeting(meeting);
        }

        public MeetingEditData? Get_Meeting_Data(string meetingid)
        {
            MeetingDB mdb = new MeetingDB(_configuration);
            return mdb.Get_Meeting_Data(meetingid);
        }

        public MeetingSessionDetail? Get_Meeting_Session_Detail(string sessionid)
        {
            MeetingDB mdb = new MeetingDB(_configuration);
            return mdb.Get_Meeting_Session_Detail(sessionid);
        }



    }


}
