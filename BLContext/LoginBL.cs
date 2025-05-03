using LitteraCore.DBContext;
using LitteraCore.Models;
using System.Data;

namespace LitteraCore.BLContext
{
    public class LoginBL
    {
        private readonly IConfiguration _configuration;
        public LoginBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UserPermission> Check_Permisiion(string chkpermission, string usertype, string agencyid = null, string formid = null)
        {
            LoginDB UDB = new LoginDB(_configuration);
            List<UserPermission> up = new List<UserPermission>();
            up = UDB.Check_Permisiion(chkpermission, agencyid,usertype, formid);
           
            return up;
        }

        public bool Save_Login_Fail_Entry(string username, string reason) {
            LoginDB UDB = new LoginDB(_configuration);
            bool issaved= UDB.Save_Login_Fail_Entry(username, reason);
            return false; 
        }
    }
}
