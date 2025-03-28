using LitteraCore.DBContext;
using LitteraCore.Models;
using Newtonsoft.Json;

namespace LitteraCore.BLContext
{
    public class UserBL
    {
        private readonly IConfiguration _configuration;
        public UserBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Save_User_Data(LoginUser user)
        {
            if (user.roleid == null)
            {
                if (user.usertype == "1")
                {
                    user.roleid = Convert.ToInt32(Common.CommonEnum.UserDefaultRole.ADMIN).ToString();
                }
                else if (user.usertype == "2")
                {
                    user.roleid = Convert.ToInt32(Common.CommonEnum.UserDefaultRole.ORGANISATION).ToString();
                }
                else if (user.usertype == "3")
                {
                    user.roleid = Convert.ToInt32(Common.CommonEnum.UserDefaultRole.CD).ToString();
                }
                else if (user.usertype == "4")
                {
                    user.roleid = Convert.ToInt32(Common.CommonEnum.UserDefaultRole.FACULTY).ToString();
                }
                else if (user.usertype == "5")
                {
                    user.roleid = Convert.ToInt32(Common.CommonEnum.UserDefaultRole.PARTICIPANT).ToString();
                }

            }


            if (user.agency.agencystatus is null)
            {
                ApplicationConfigDB AS = new ApplicationConfigDB(_configuration);
                ParticipantApproval ml = new ParticipantApproval();
                ml = JsonConvert.DeserializeObject<ParticipantApproval>(AS.Get_Application_Setting("2").Rows[0]["SettingValue"].ToString());
                if (ml.REG_AUTO_APPROVAL == 1)
                {
                    user.agency.agencystatus = ml.REG_APPROVAL_STATUS.ToString();
                }
                else
                {
                    user.agency.agencystatus = "0";
                }

            }


            //******* Manage Null Password
            if (user.password == null)
            {
                string pwd = Guid.NewGuid().ToString();
                string encpwd = YEncryptDecryptData.YEncryptDecryptData.Encrypt(pwd, true);
                user.password = encpwd;
            }
            else
            {
                string pwd = user.password;
                string encpwd = YEncryptDecryptData.YEncryptDecryptData.Encrypt(pwd, true);
                user.password = encpwd;
            }






            UserDB udb = new UserDB(_configuration);
            bool issave = udb.Save_User_Data(user);
            return true;
        }

        public Agency Check_Mobile(string mobileno, string APPURL, string agencytype)
        {

            UserDB udb = new UserDB(_configuration);
            Agency a = new Agency();
            a = udb.Check_Mobile_EMAIL(mobileno, 2, APPURL, agencytype);
            return a;
        }
    }
}
