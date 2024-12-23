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



    }


}
