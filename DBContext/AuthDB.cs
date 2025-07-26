using LitteraCore.Common;
using LitteraCore.Common.Token;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Web;
namespace LitteraCore.DBContext
{
    public class AuthDB
    {
        private readonly IConfiguration _configuration;
        public AuthDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public UserInfo GetUserInfo(string username, string loginattempt = null)
        {

            UserInfo u = new UserInfo();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("YUser.UserValidate", con);
            cmd.Parameters.AddWithValue("@username", username);
            if (loginattempt != null)
            {
                cmd.Parameters.AddWithValue("@loginattempts", loginattempt);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "active='1'";
            dt = dt.DefaultView.ToTable();

            List<UserInfo_usertype> usertype = new List<UserInfo_usertype>();
            //procedure required to get user typewise role
            List<UserInfo_usertype_roles> usertypewiserole = new List<UserInfo_usertype_roles>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    usertype.Add(new UserInfo_usertype { usertypeid = Convert.ToString(dr["usertype"]), usertypename = Enum.GetName(typeof(CommonEnum.usertype), Convert.ToInt16(dr["usertype"])) });
                    u.userid = Convert.ToString(dr["UserID"]);
                    u.agencyid = Convert.ToString(dr["EMPLOYEEID"]);
                    u.emailid = Convert.ToString(dr["EmailId"]);
                    u.Mobileno = Convert.ToString(dr["MobileNo"]);
                    u.Username = Convert.ToString(dr["f_name"]);
                    u.branchid = "DFF7C661-5B84-4A7E-8250-31C420DD9FCD";
                    if (Convert.ToString(dr["uploadpath"]) != "")
                    {
                        u.photopath = Convert.ToString(dr["uploadpath"]);
                    }

                }
                u.usertype = usertype.ToArray();
                u.userrole = usertypewiserole.ToArray();
            }



            return u;

        }

        public List<User> GET_LOGIN_DETAIL(string username)
        {
            try
            {
                List<User> user = new List<User>();
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
                 if (con.State == ConnectionState.Open) { con.Close();}con.Open();
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public bool Change_Password(string userid, string password)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("YUser.ChangePassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@UserId", userid);
            // cmd.Parameters.AddWithValue("@OldPassword", username);
            cmd.Parameters.AddWithValue("@NewPassword", password);
            cmd.Parameters.AddWithValue("@ty", "1");
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public bool Unblock_Password(string userid)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("YUser.proc_yuser_unblock_user", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@User_Id", userid);
            // cmd.Parameters.AddWithValue("@OldPassword", username);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }


        public bool Make_Login_Entry(string userid, string logoff, string ip)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("yuser.InsUpdUserLog", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@LogOff", logoff);
            cmd.Parameters.AddWithValue("@ip", ip);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }


        public static string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())  // Creates an MD5 instance
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input); // Converts the input string to a byte array
                byte[] hashBytes = md5.ComputeHash(inputBytes); // Computes the MD5 hash

                // Converts the hash byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // "x2" ensures 2 hexadecimal characters for each byte
                }
                return sb.ToString(); // Returns the hexadecimal hash string
            }
        }

        public DataTable Get_User_Agency_Data(userlist ul)
        {
            try
            {
                List<User> user = new List<User>();
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
                 if (con.State == ConnectionState.Open) { con.Close();}con.Open();
                SqlCommand cmd = new SqlCommand("yuser.proc_get_user_agency_details_importdata", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                string p1 = JsonConvert.SerializeObject(ul);
                cmd.Parameters.AddWithValue("@JsonParameter", p1);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool Update_bulk_password(userlist ul)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("YUser.update_bulk_password", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            string p1 = JsonConvert.SerializeObject(ul);
            cmd.Parameters.AddWithValue("@jsonInput", p1);
            // cmd.Parameters.AddWithValue("@OldPassword", username);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public bool is_password_changed(string userid)
        {

            UserInfo u = new UserInfo();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("YUser.proc_yuser_check_password_changed", con);
            cmd.Parameters.AddWithValue("@userid", userid);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            bool ischanged = false;
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["ispasswordchanged"]) == "1")
                {
                    ischanged = true;
                }

            }

            return ischanged;




        }

        public bool Password_Updated(string userid)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("YUser.proc_yuser_update_password_changed", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@userid", userid);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }
    }
}
