namespace LitteraCore.Models
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }

        public string password_to_mail { get; set;}

        //public string usertype { get; set; }

        public int loginattempt { get; set; }

        public string f_name { get; set; }

        public string m_name { get; set; }
        public string l_name { get; set; }

        public string mobileno { get; set; }
        public string emailid { get; set; }
        public string userid { get; set; }

        public string imagepath { get; set; }

        public string branchid { get; set; }

        public string LoginID { get; set; }
        public UserType[] usertype { get; set; }

        public string roleid { get; set; }

        public UserAgency agency { get; set; }
        public string createdby { get; set; }


        public user_branches branches { get; set; }
    }

    public class UserType
    {
        public string usertype;
        public string usertypename;
    }
    public class UserAgency
    {
        public string AgencyId { get; set; }


        public string AgencyName { get; set; }
        public string HAgencyName { get; set; }
        public string AgencyTypeId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string UserCode { get; set; }


        public string ag_salutation { get; set; }
        public string ag_first_name { get; set; }

        public string ag_m_name { get; set; }
        public string ag_l_name { get; set; }
        public string ag_hfirst_name { get; set; }
        public string ag_hm_name { get; set; }
        public string ag_hl_name { get; set; }


        public string ag_mobileno { get; set; }


        public string ag_email { get; set; }

        public string agencystatus { get; set; }
        public string ag_photo_path { get; set; }

    }
    public class UserPermission
    {
        public string tyfp_formroleid { get; set; }
        public string tyfp_formid { get; set; }
        public string tyfp_permission { get; set; }

        public string tyur_user_type_id { get; set; }

    }
    public class Usertype
    {
        public string id { get; set; }

        public string name { get; set; }
    }

    public class PrintData
    {
        public string printdata { get; set; }
    }
    public class BranchType
    {
        public string branchtypeid { get; set; }

        public string branchtype_name { get; set; }

        public string branchtype_hname { get; set; }
    }
    public class Branches
    {
        public string branchtypeId { get; set; }
        public string branchid { get; set; }

        public string branch_name { get; set; }

        public string branch_hname { get; set; }
    }
    public class UserBranch
    {
        public BranchType[] branchtype { get; set; }

        public Branches[] branches { get; set; }
    }

    public class update_pass
    {
        public string userid { get; set; }

        public string? password { get; set; }
    }
    public class userlist
    {
        public List<update_pass> users { get; set; }
    }

    public class LoginUser
    {
        public string username { get; set; }
        public string password { get; set; }

        public string password_enc { get; set; }
        //public string usertype { get; set; }



        public string f_name { get; set; }

        public string m_name { get; set; }
        public string l_name { get; set; }

        public string mobileno { get; set; }
        public string emailid { get; set; }
        public string userid { get; set; }




        public string branchid { get; set; }

        public string LoginID { get; set; }
        public string usertype { get; set; }

        public string roleid { get; set; }

        public UserAgency agency { get; set; }
        public string createdby { get; set; }
        public user_branches branches { get; set; }
    }
    public class user_branches
    {
        public string branchtype { get; set; }
        public user_branches_detail[] branches { get; set; }

    }
    public class user_branches_detail
    {
        public string branchid { get; set; }
        public string branchname { get; set; }
    }
}
