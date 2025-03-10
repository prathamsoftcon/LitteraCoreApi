using LitteraCore.Common.DMS;
using LitteraCore.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using LitteraCore.Models;
using Microsoft.Extensions.Configuration;
using static Azure.Core.HttpHeader;

namespace LitteraCore.DBContext
{
    public class UserDB
    {
         private readonly IConfiguration _configuration;
        public UserDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Save_User_Data(LoginUser user)
        {
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlTransaction st = con.BeginTransaction();
            try
            {
                DataTable dtuserdetail = new DataTable();
                dtuserdetail = Get_User_Details(user.userid);

                if (dtuserdetail.Rows[0]["isexist"].ToString() == "0")
                {
                    if (Save_User(user, con, st) == false)
                    {
                        throw new Exception("Error on save user");
                    }
                }

                if (user.agency.AgencyTypeId != "00053")
                {
                    if (Save_User_Roles(user, con, st) == false)
                    {
                        throw new Exception("Error on save role");
                    }
                }
                if (user.branches != null)
                {
                    if (Save_User_Branches(user.branches, con, user, st) == false)
                    {
                        throw new Exception("Error on save branches");
                    }
                }





                DMSBL dbl = new DMSBL(_configuration);
                string usercode = "";
                DMSDB dmsdb = new DMSDB(_configuration);
                List<DMS> dls = new List<DMS>();
                DataTable dt = new DataTable();

                if (dt.Rows.Count > 0)
                {
                    usercode = dt.Rows[0]["UserCode"].ToString();
                }
                else
                {
                    usercode = dbl.Get_agency_doc_no(System.DateTime.Now.ToString("yyyy/MM/dd"), user.branchid, "$$", "YEAR");
                }

                if (Save_SignIn_Info(user, con, usercode, st) == false)
                {
                    throw new Exception("Error on save Sign In Info");
                }

             
                if (Save_Agency_Mapping_Data(user, con, st) == false)
                {
                    throw new Exception("Error on save Mapping data");
                }


                //Code to save DMS DATA

                DMS d = new DMS
                {
                    docno = usercode,
                    doc_id = user.agency.AgencyId,
                    createdon = DateTime.Now,
                    createdby = user.createdby,
                    branchid = user.branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = user.createdby,
                    fwd_empid = user.createdby,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.Get_Default_USER_TAT_TYPE(Convert.ToInt32(user.usertype), user.agency.AgencyTypeId)),
                    doc_status = Convert.ToInt32(user.agency.agencystatus),
                };
                dbl.Save_DMS_DATA(d, con, st);


                //Code to save Delegate department entry in case of Staff
                if (Convert.ToInt32(user.usertype) == Convert.ToInt32(CommonEnum.UserType.CD))
                {
                    if (Save_Delegate_Department(user, con, st) == false)
                    {
                        throw new Exception("Error on save role");
                    }
                }




                st.Commit();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return false;
            }
            finally
            {
                con.Close();

            }

            return false;
        }
        public DataTable Get_User_Details(string userid)
        {
            DataTable dt = new DataTable();
            Agency a = new Agency();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_chk_user_exists_vr1", con);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public bool Save_User(LoginUser user, SqlConnection con, SqlTransaction transaction = null)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_upd_user_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@UserID", user.userid);
                cmd.Parameters.AddWithValue("@UserName", user.username);
                cmd.Parameters.AddWithValue("@Password", user.password);
              
                cmd.Parameters.AddWithValue("@createdby", user.createdby);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;

                cmd.ExecuteNonQuery();

                cmd.ExecuteNonQuery();
                //con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            //con.Open();

        }
        public bool Save_User_Roles(LoginUser user, SqlConnection con, SqlTransaction transaction = null)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_upd_user_roles_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@UserID", user.userid);
                cmd.Parameters.AddWithValue("@createdby", user.createdby);
                cmd.Parameters.AddWithValue("@tyur_form_role_id", user.roleid);
                cmd.Parameters.AddWithValue("@tyur_user_type_id", user.usertype);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;

                cmd.ExecuteNonQuery();

                cmd.ExecuteNonQuery();
                //con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            //con.Open();

        }
        public bool Save_User_Branches(user_branches branches, SqlConnection con, LoginUser user, SqlTransaction transaction)
        {
            string branchstring = "";
            foreach (user_branches_detail s in branches.branches)
            {
                branchstring = branchstring + s.branchid.ToString() + ",";
            }

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_upd_user_branch_roles_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@UserID", user.userid);
                cmd.Parameters.AddWithValue("@createdby", user.createdby);
                cmd.Parameters.AddWithValue("@tyubr_branch_id", branchstring);
                cmd.Parameters.AddWithValue("@tyubr_branch_type", branches.branchtype);
                cmd.Parameters.AddWithValue("@tyubr_user_type_id", user.usertype);
                cmd.Parameters.AddWithValue("@tyubr_agency_id", user.agency.AgencyId);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;

                cmd.ExecuteNonQuery();


                //con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            //con.Open();

        }

        public bool Save_SignIn_Info(LoginUser user, SqlConnection con, string usercode, SqlTransaction transaction = null)
        {
            try
            {
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
                //con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_upd_agency_signup_info_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@AgencyId", user.agency.AgencyId);
                cmd.Parameters.AddWithValue("@AgencyName", user.agency.AgencyName);
                cmd.Parameters.AddWithValue("@HAgencyName", user.agency.AgencyName);
                cmd.Parameters.AddWithValue("@ag_salutation", user.agency.ag_salutation);
                cmd.Parameters.AddWithValue("@ag_first_name", user.agency.ag_first_name);
                cmd.Parameters.AddWithValue("@ag_m_name", user.agency.ag_m_name);
                cmd.Parameters.AddWithValue("@ag_l_name", user.agency.ag_l_name);
                cmd.Parameters.AddWithValue("@ag_hfirst_name", user.agency.ag_first_name);
                cmd.Parameters.AddWithValue("@ag_hm_name", user.agency.ag_m_name);
                cmd.Parameters.AddWithValue("@ag_hl_name", user.agency.ag_l_name);
                cmd.Parameters.AddWithValue("@AgencyTypeID", user.agency.AgencyTypeId);
                cmd.Parameters.AddWithValue("@CreatedBy", user.createdby);

                cmd.Parameters.AddWithValue("@UserCode", usercode);
                cmd.Parameters.AddWithValue("@ag_email", user.agency.ag_email);
                cmd.Parameters.AddWithValue("@ag_mobileno", user.agency.ag_mobileno);
                cmd.Parameters.AddWithValue("@agencystatus", user.agency.agencystatus);
                cmd.Parameters.AddWithValue("@ag_photo_path", user.agency.ag_photo_path);
                cmd.Parameters.AddWithValue("@tyaam_tat_typeid", Convert.ToInt32(Common.CommonEnum.Get_Default_USER_TAT_TYPE(Convert.ToInt32(user.usertype), user.agency.AgencyTypeId)));

                cmd.Parameters.AddWithValue("@branchid", user.branchid);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;

                cmd.ExecuteNonQuery();


                //con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }

        public bool Save_Agency_Mapping_Data(LoginUser user, SqlConnection con, SqlTransaction transaction = null)
        {
            try
            {
                DMSBL dbl = new DMSBL(_configuration);
                //string usercode = dbl.Get_doc_no(System.DateTime.Now.ToString("yyyy/MM/dd"), user.branchid, Convert.ToInt16(Common.CommonEnum.Get_Default_USER_TAT_TYPE(Convert.ToInt32(user.usertype), user.agency.AgencyTypeId)), "$$", "YEAR");
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
                //con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_user_agency_mapping_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@UserID", user.userid);
                cmd.Parameters.AddWithValue("@usertype", user.usertype);
                cmd.Parameters.AddWithValue("@agencyid", user.agency.AgencyId);
                cmd.Parameters.AddWithValue("@tyuam_tat_typeid", Convert.ToInt16(Common.CommonEnum.Get_Default_USER_TAT_TYPE(Convert.ToInt32(user.usertype), user.agency.AgencyTypeId)));
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;



                cmd.ExecuteNonQuery();
                //con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool Save_Delegate_Department(LoginUser user, SqlConnection con, SqlTransaction transaction = null)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_ins_hr_delegated_department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@thdd_emp_id", user.agency.AgencyId);
                cmd.Parameters.AddWithValue("@thdd_link_emp_id", DBNull.Value);
                cmd.Parameters.AddWithValue("@thdd_created_by", user.createdby);
                cmd.Parameters.AddWithValue("@thdd_effective_dt", "2021/01/01");
                cmd.Parameters.AddWithValue("@thdd_created_on", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm"));
                cmd.Parameters.AddWithValue("@chargexml", "<DocumentElement><CHARGE><THDD_CHARGE_ID>" + CommonDB.CD_CHARGE_ID + "</THDD_CHARGE_ID></CHARGE></DocumentElement>");


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;

                cmd.ExecuteNonQuery();


                //con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            //con.Open();

        }




    }
}
