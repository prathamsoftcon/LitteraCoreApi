using LitteraCore.Common.DMS;
using LitteraCore.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using LitteraCore.Models;
using Microsoft.Extensions.Configuration;
using static Azure.Core.HttpHeader;
using Newtonsoft.Json;
using System.Xml;

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
            if (con.State != ConnectionState.Open) { con.Open(); }
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
            if (con.State != ConnectionState.Open) { con.Open(); }
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
                    if (con.State != ConnectionState.Open) { con.Open(); }
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
            //if (con.State != ConnectionState.Open) { con.Open(); }

        }
        public bool Save_User_Roles(LoginUser user, SqlConnection con, SqlTransaction transaction = null)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
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
            //if (con.State != ConnectionState.Open) { con.Open(); }

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
                    if (con.State != ConnectionState.Open) { con.Open(); }
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
            //if (con.State != ConnectionState.Open) { con.Open(); }

        }

        public bool Save_SignIn_Info(LoginUser user, SqlConnection con, string usercode, SqlTransaction transaction = null)
        {
            try
            {
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
                //if (con.State != ConnectionState.Open) { con.Open(); }
                if (con.State == ConnectionState.Closed)
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
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
                //if (con.State != ConnectionState.Open) { con.Open(); }
                if (con.State == ConnectionState.Closed)
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
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
                    if (con.State != ConnectionState.Open) { con.Open(); }
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
            //if (con.State != ConnectionState.Open) { con.Open(); }

        }


        public Agency Check_Mobile_EMAIL(string mobileno, int type, string APPURL, string agencytypeid)
        {
            DataSet ds = new DataSet();
            Agency a = new Agency();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_check_value_in_agency_master", con);
            cmd.Parameters.AddWithValue("@value", mobileno);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (Convert.ToInt16(ds.Tables[0].Rows[0]["Isavailable"]) != 0)
            {
                DataTable dtfiltereddata = new DataTable();
                ds.Tables[1].DefaultView.RowFilter = "tyaam_typeid='" + agencytypeid + "'";
                dtfiltereddata = ds.Tables[1].DefaultView.ToTable();

                //Calculate user details
                DataTable dtUserDetails = new DataTable();
                dtUserDetails = ds.Tables[1];
                List<userDetails> ud = new List<userDetails>();

                foreach (DataRow dr in dtUserDetails.Rows)
                {
                    userDetails u = new userDetails();
                    u.tat_type_id = Convert.ToString(dr["tyaam_tat_typeid"]);
                    u.usertype = Convert.ToString(dr["tyuam_user_type_id"]);
                    u.tyaam_status = Convert.ToString(dr["tyaam_status"]);
                    ud.Add(u);
                }




                if (dtfiltereddata.Rows.Count > 0)
                {

                    a.upload_photo_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_photo_path"]);
                    a.userdetail = ud.ToArray();
                    a.agencyid = Convert.ToString(dtfiltereddata.Rows[0]["AgencyId"]);
                    a.agencyname = Convert.ToString(dtfiltereddata.Rows[0]["AgencyName"]);
                    a.hagencyname = Convert.ToString(dtfiltereddata.Rows[0]["HAgencyName"]);
                    a.UserCode = Convert.ToString(dtfiltereddata.Rows[0]["UserCode"]);
                    a.Ag_Address = Convert.ToString(dtfiltereddata.Rows[0]["Ag_Address"]);
                    a.Ag_Address1 = Convert.ToString(dtfiltereddata.Rows[0]["Ag_Address1"]);
                    a.uploadpath = Convert.ToString(dtfiltereddata.Rows[0]["uploadpath"]);
                    a.ag_photo_path =  Convert.ToString(dtfiltereddata.Rows[0]["ag_photo_path"]);
                    //if (dtfiltereddata.Rows[0]["ag_photo_path"].ToString() != "")
                    //{
                    //    UploadPath Up = new UploadPath();
                    //    a.ag_photo_path = APPURL + Up.Get_Default_Upload_Path() + Convert.ToString(dtfiltereddata.Rows[0]["ag_photo_path"]);
                    //}

                    a.ag_first_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_first_name"]);
                    a.ag_m_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_m_name"]);
                    a.ag_l_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_l_name"]);
                    a.ag_hfirst_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_hfirst_name"]);
                    a.ag_hm_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_hm_name"]);
                    a.ag_hl_name = Convert.ToString(dtfiltereddata.Rows[0]["ag_hl_name"]);
                    a.ag_address_city = Convert.ToString(dtfiltereddata.Rows[0]["ag_address_city"]);
                    a.ag_address_state = Convert.ToString(dtfiltereddata.Rows[0]["ag_address_state"]);
                    a.ag_pincode = Convert.ToString(dtfiltereddata.Rows[0]["ag_pincode"]);
                    a.ag_phone = Convert.ToString(dtfiltereddata.Rows[0]["ag_phone"]);
                    a.ag_alternative_phone = Convert.ToString(dtfiltereddata.Rows[0]["ag_alternative_phone"]);
                    a.ag_mobileno = Convert.ToString(dtfiltereddata.Rows[0]["ag_mobileno"]);
                    a.ag_alternative_mobileno = Convert.ToString(dtfiltereddata.Rows[0]["ag_alternative_mobileno"]);
                    a.ag_email = Convert.ToString(dtfiltereddata.Rows[0]["ag_email"]);
                    a.ag_alternative_email = Convert.ToString(dtfiltereddata.Rows[0]["ag_alternative_email"]);
                    a.ag_gender = Convert.ToString(dtfiltereddata.Rows[0]["ag_gender"]);
                    if (dtfiltereddata.Rows[0]["ag_age"].ToString() != "")
                    {
                        a.ag_age = Convert.ToInt16(dtfiltereddata.Rows[0]["ag_age"]);
                    }
                    if (dtfiltereddata.Rows[0]["ag_dob"].ToString() != "")
                    {
                        a.ag_dob = Convert.ToString(dtfiltereddata.Rows[0]["ag_dob"]);
                    }
                    else
                    {
                        a.ag_dob = null;
                    }

                    a.ag_salutation = Convert.ToString(dtfiltereddata.Rows[0]["ag_salutation"]);
                    a.ag_aadhar = Convert.ToString(dtfiltereddata.Rows[0]["ag_aadhar"]);
                    a.ag_gstin = Convert.ToString(dtfiltereddata.Rows[0]["ag_gstin"]);
                    a.ag_pan = Convert.ToString(dtfiltereddata.Rows[0]["ag_pan"]);
                    a.tyaam_typeid = Convert.ToString(dtfiltereddata.Rows[0]["tyaam_typeid"]);
                    a.tyaam_val = Convert.ToString(dtfiltereddata.Rows[0]["tyaam_val"]);
                    a.userid = Convert.ToString(dtfiltereddata.Rows[0]["userid"]);


                    if (dtfiltereddata.Rows[0]["tyaam_val"].ToString() != "")
                    {
                        //First Convert XML to Json

                        try
                        {
                            if (agencytypeid != "00053")
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(dtfiltereddata.Rows[0]["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));

                                XmlDocument doc1 = new XmlDocument();

                                doc1.LoadXml(doc.ChildNodes[0].InnerXml);
                                string JsonText = JsonConvert.SerializeXmlNode(doc1).Replace("\"ADDINFO\":", "");
                                JsonText = JsonText.Substring(1, JsonText.Length - 2);
                                a.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}"));

                                a.additionalInfo.DOC_PATH =  a.additionalInfo.DOC_PATH;
                            }
                            else
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(dtfiltereddata.Rows[0]["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));
                                string JsonText1 = JsonConvert.SerializeObject(doc.ChildNodes[0].ChildNodes[0]);
                                JsonText1 = JsonText1.Replace("{\"ADDINFO\":", "").Replace("}}", "}");
                                a.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText1);

                            }







                        }
                        catch
                        {
                            a.additionalInfo = null;
                        }

                    }
                    else
                    {
                        a.additionalInfo = new AgencyAdditionalInfo();
                    }





                }
                else
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        a.upload_photo_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_photo_path"]);
                        a.userdetail = ud.ToArray();
                        a.agencyid = Convert.ToString(ds.Tables[1].Rows[0]["AgencyId"]);
                        a.agencyname = Convert.ToString(ds.Tables[1].Rows[0]["AgencyName"]);
                        a.hagencyname = Convert.ToString(ds.Tables[1].Rows[0]["HAgencyName"]);
                        a.UserCode = Convert.ToString(ds.Tables[1].Rows[0]["UserCode"]);
                        a.Ag_Address = Convert.ToString(ds.Tables[1].Rows[0]["Ag_Address"]);
                        a.Ag_Address1 = Convert.ToString(ds.Tables[1].Rows[0]["Ag_Address1"]);
                        a.uploadpath = Convert.ToString(ds.Tables[1].Rows[0]["uploadpath"]);
                        a.upload_photo_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_photo_path"]);
                        a.ag_photo_path = Convert.ToString(ds.Tables[1].Rows[0]["ag_photo_path"]);
                        //if (ds.Tables[1].Rows[0]["ag_photo_path"].ToString() != "")
                        //{
                        //    UploadPath Up = new UploadPath();
                        //    a.ag_photo_path = APPURL + Up.Get_Default_Upload_Path() + Convert.ToString(ds.Tables[1].Rows[0]["ag_photo_path"]);
                        //}

                        a.ag_first_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_first_name"]);
                        a.ag_m_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_m_name"]);
                        a.ag_l_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_l_name"]);
                        a.ag_hfirst_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_hfirst_name"]);
                        a.ag_hm_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_hm_name"]);
                        a.ag_hl_name = Convert.ToString(ds.Tables[1].Rows[0]["ag_hl_name"]);
                        a.ag_address_city = Convert.ToString(ds.Tables[1].Rows[0]["ag_address_city"]);
                        a.ag_address_state = Convert.ToString(ds.Tables[1].Rows[0]["ag_address_state"]);
                        a.ag_pincode = Convert.ToString(ds.Tables[1].Rows[0]["ag_pincode"]);
                        a.ag_phone = Convert.ToString(ds.Tables[1].Rows[0]["ag_phone"]);
                        a.ag_alternative_phone = Convert.ToString(ds.Tables[1].Rows[0]["ag_alternative_phone"]);
                        a.ag_mobileno = Convert.ToString(ds.Tables[1].Rows[0]["ag_mobileno"]);
                        a.ag_alternative_mobileno = Convert.ToString(ds.Tables[1].Rows[0]["ag_alternative_mobileno"]);
                        a.ag_email = Convert.ToString(ds.Tables[1].Rows[0]["ag_email"]);
                        a.ag_alternative_email = Convert.ToString(ds.Tables[1].Rows[0]["ag_alternative_email"]);
                        a.ag_gender = Convert.ToString(ds.Tables[1].Rows[0]["ag_gender"]);
                        if (ds.Tables[1].Rows[0]["ag_age"].ToString() != "")
                        {
                            a.ag_age = Convert.ToInt16(ds.Tables[1].Rows[0]["ag_age"]);
                        }
                        if (ds.Tables[1].Rows[0]["ag_dob"].ToString() != "")
                        {
                            a.ag_dob = Convert.ToString(ds.Tables[1].Rows[0]["ag_dob"]);
                        }
                        else
                        {
                            a.ag_dob = null;
                        }

                        a.ag_salutation = Convert.ToString(ds.Tables[1].Rows[0]["ag_salutation"]);
                        a.ag_aadhar = Convert.ToString(ds.Tables[1].Rows[0]["ag_aadhar"]);
                        a.ag_gstin = Convert.ToString(ds.Tables[1].Rows[0]["ag_gstin"]);
                        a.ag_pan = Convert.ToString(ds.Tables[1].Rows[0]["ag_pan"]);
                        a.tyaam_typeid = Convert.ToString(ds.Tables[1].Rows[0]["tyaam_typeid"]);
                        a.tyaam_val = Convert.ToString(ds.Tables[1].Rows[0]["tyaam_val"]);
                        a.userid = Convert.ToString(ds.Tables[1].Rows[0]["userid"]);


                        if (ds.Tables[1].Rows[0]["tyaam_val"].ToString() != "")
                        {
                            //First Convert XML to Json

                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(ds.Tables[1].Rows[0]["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));

                                XmlDocument doc1 = new XmlDocument();

                                doc1.LoadXml(doc.ChildNodes[0].InnerXml);



                                //XmlDocument doc2 = new XmlDocument();
                                //doc2.LoadXml(doc.SelectNodes("/DocumentElement/ADDINFO").Item(0).InnerXml);

                                string JsonText = JsonConvert.SerializeXmlNode(doc1).Replace("\"ADDINFO\":", "");
                                JsonText = JsonText.Substring(1, JsonText.Length - 2);



                                a.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}"));

                                a.additionalInfo.DOC_PATH = a.additionalInfo.DOC_PATH;
                            }
                            catch
                            {
                                a.additionalInfo = null;
                            }

                        }
                        else
                        {
                            a.additionalInfo = new AgencyAdditionalInfo();
                        }



                    }
                }



            }


            return a;
        }



        public User GET_MOBILE_NO_DATA(string mobileno, int type)
        {
            DataSet ds = new DataSet();
            Agency a = new Agency();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_check_value_in_agency_master", con);
            cmd.Parameters.AddWithValue("@value", mobileno);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            User u = new User();
            if (Convert.ToInt16(ds.Tables[0].Rows[0]["Isavailable"]) != 0)
            {
                DataTable dtfiltereddata = new DataTable();

                dtfiltereddata = ds.Tables[1];

                //Calculate user details
                DataTable dtUserDetails = new DataTable();
                dtUserDetails = ds.Tables[1];
                List<userDetails> ud = new List<userDetails>();

                foreach (DataRow dr in dtUserDetails.Rows)
                {
                    UserAgency ua = new UserAgency();
                    u.userid = Convert.ToString(dr["UserID"]);
                    ua.AgencyId = Convert.ToString(dr["AgencyId"]);
                    u.agency = ua;
                }




               


            }


            return u;
        }

        public User_Agency_Detail Get_User_Detail_by_userid(string userid)
        {
            DataTable dt = new DataTable();
            Agency a = new Agency();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_user_detail", con);
            cmd.Parameters.AddWithValue("@userid", userid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            User_Agency_Detail uad = new User_Agency_Detail();
            if (dt.Rows.Count > 0)
            {
                uad.userid= Convert.ToString(dt.Rows[0]["tyuam_userid"]);
                uad.agencyid= Convert.ToString(dt.Rows[0]["tyuam_agency_id"]);
                uad.usertype= Convert.ToInt16(dt.Rows[0]["tyuam_user_type_id"]);
            }
          return uad;
        }

        public Trg_User_Details Get_Trg_User_Details(string userocde,string trainingid)
        {
            DataTable dt = new DataTable();
            Agency a = new Agency();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("select userid,vtpu.Agencyid,trainingid from TrainingPlan.Vw_tp_trg_all_user vtpu inner join YUser.AgencyMaster am on vtpu.Agencyid=am.AgencyId where UserCode='" + userocde + "' and trainingid='"+trainingid+"'", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Trg_User_Details uad = new Trg_User_Details();
            if (dt.Rows.Count > 0)
            {
                uad.userid = Convert.ToString(dt.Rows[0]["userid"]);
                uad.agencyid = Convert.ToString(dt.Rows[0]["Agencyid"]);
                uad.trainingid = Convert.ToString(dt.Rows[0]["trainingid"]);
            }
            return uad;
        }


        public string Get_User_agency_by_code(string userocde)
        {
            string agencyid = "";
            DataTable dt = new DataTable();
            Agency a = new Agency();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("SELECT Agencyid FROM YUser.AgencyMaster WHERE UserCode = @UserCode", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserCode", userocde);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            Trg_User_Details uad = new Trg_User_Details();
            if (dt.Rows.Count > 0)
            {
                agencyid = Convert.ToString(dt.Rows[0]["Agencyid"]);
              
            }
            return agencyid;
        }

        public string get_user_id_by_agencyid(string agencyid)
        {
            string userid = "";
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM YUser.tbl_yuser_user_agency_mapping WHERE tyuam_agency_id = @AgencyId", con);
            cmd.Parameters.AddWithValue("@AgencyId", agencyid);

            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                userid = Convert.ToString(dt.Rows[0]["tyuam_userid"]);

            }

            return userid;

        }



    }
}
