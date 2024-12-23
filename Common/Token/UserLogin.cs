namespace LitteraCore.Common.Token
{
    public class UserLogin
    {
        public string? userid { get; set; }
        public string? Username { get; set; }
        public string? Mobileno { get; set; }
        public string? emailid { get; set; }
        public string? Password { get; set; }

        public string? salt { get; set; }

        public long? OTP { get; set; }
    }
    public class UserInfo
    {
        public string? userid { get; set; }
        public string? agencyid { get; set; }
        public string Username { get; set; }
        public string? Mobileno { get; set; }
        public string emailid { get; set; }
        public string password { get; set; }
        public UserInfo_usertype[] usertype { get; set; }

        public UserInfo_usertype_roles[] userrole { get; set; }

        public string photopath { get; set; } 
        public string branchid { get; set; }
    }

    public class UserInfo_usertype
    {
        public string usertypeid { get; set; }
        public string usertypename { get; set; }
    }
    public class UserInfo_usertype_roles
    {
        public string usertypeid { get; set; }
        public string roleid { get; set; }
    }
    public class Update_Password
    {
        public string userid { get; set; }
        public string password { get; set; }
    }
}
