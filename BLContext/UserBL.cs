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

        // Added: the email counterpart to Check_Mobile above. Both are thin
        // wrappers over the same Check_Mobile_EMAIL(value, type, APPURL,
        // agencytype) - type 2 = mobile column, type 1 = email column (mirrors
        // the old Littera_MVC_API's UserBL.Check_Mobile/Check_EMAIL split).
        // This method did not exist before; its absence is why
        // UserRegistrationService.EnsureIdentifierIsAvailable was calling
        // Check_Mobile for the email identifier too (searching the mobile
        // column for an email value, so the check could never match) - see
        // UserRegistrationService.cs for the fix that now calls this instead.
        public Agency Check_EMAIL(string emailid, string APPURL, string agencytype)
        {

            UserDB udb = new UserDB(_configuration);
            Agency a = new Agency();
            a = udb.Check_Mobile_EMAIL(emailid, 1, APPURL, agencytype);
            return a;
        }

        public AgencyExistenceLookup Check_Agency_Exists(string userMobileMail)
        {
            UserDB udb = new UserDB(_configuration);
            return udb.Check_Agency_Exists(userMobileMail);
        }

        // Ported from Littera_MVC_API/Models/UserBL.cs (old app) - form-role
        // lookups for the Administrator/Staff "Form Role" multi-select and its
        // edit-mode display. No equivalent existed anywhere in this API before.
        public List<Usertype> Get_Form_Role(string createdby)
        {
            UserDB udb = new UserDB(_configuration);
            return udb.Get_Form_Role(createdby);
        }

        public List<Usertype> Get_User_Form_Rights(string userid)
        {
            UserDB udb = new UserDB(_configuration);
            return udb.Get_User_Form_Rights(userid);
        }

        // See UserDB.Save_User_Roles_Standalone for why this is a narrow,
        // standalone entry point rather than routing through Save_User_Data.
        public bool Update_User_Roles(string userid, string roleid, string usertype, string createdby)
        {
            UserDB udb = new UserDB(_configuration);
            return udb.Save_User_Roles_Standalone(userid, roleid, usertype, createdby);
        }

    }
}
