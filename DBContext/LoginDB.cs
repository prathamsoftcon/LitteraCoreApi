using LitteraCore.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LitteraCore.DBContext
{
    public class LoginDB
    {
        private readonly IConfiguration _configuration;
        public LoginDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<User> Get_User_Details(string username)
        {



            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_check_and_login_user", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@UserName", username);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                user.Add(
                    new User
                    {
                        password = Convert.ToString(dr["password"]),
                        loginattempt = Convert.ToInt32(dr["loginattempt"])
                    });


            }
            return user;
        }

        public List<Country> Get_Countries()
        {

            List<Country> CL = new List<Country>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.prop_yuser_get_country_code", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow row in dt.Rows)
            {
                Country c = new Country();
                c.Countryid = Convert.ToString(row["Countryid"]);
                c.CountryName = Convert.ToString(row["CountryName"]);
                c.CountryCode = Convert.ToString(row["CountryCode"]);
                c.FlagPath = Convert.ToString(row["CountryFlagPath"]);


                CL.Add(c);
            }



            return CL;
        }

        public List<UserPermission> Check_Permisiion(string chkpermission, string agencyid = null,string userttype=null, string formid = null)
        {

            List<UserPermission> up = new List<UserPermission>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_user_permission", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@chkpermission", chkpermission);
            if (agencyid != null)
            {
                cmd.Parameters.AddWithValue("@agencyid", agencyid);
            }
            if (formid != null)
            {
                cmd.Parameters.AddWithValue("@formid", formid);
            }
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if(userttype != null)
            {
                dt.DefaultView.RowFilter = "tyur_user_type_id='" + userttype + "'";
                dt = dt.DefaultView.ToTable();
            }
         
            foreach (DataRow row in dt.Rows)
            {
                UserPermission ass = new UserPermission();
                ass.tyfp_formroleid = Convert.ToString(row["tyfp_formroleid"]);
                ass.tyfp_formid = Convert.ToString(row["tyfp_formid"]);
                ass.tyfp_permission = Convert.ToString(row["tyfp_permission"]);
                ass.tyur_user_type_id = Convert.ToString(row["tyur_user_type_id"]);
                up.Add(ass);
            }


            return up;
        }

        public bool Insert_Firebase_Token(string agencyid, string token)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_upd_firebase_token", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@agencyid", agencyid);
            cmd.Parameters.AddWithValue("@token", token);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            con.Close();

            User u = new User();

            return true;
        }

        public bool Save_Login_Fail_Entry(string username, string reason)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_ins_failed_login_entry", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tyflu_username", username);
            cmd.Parameters.AddWithValue("@tyflu_reason", reason);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }
    }
}
