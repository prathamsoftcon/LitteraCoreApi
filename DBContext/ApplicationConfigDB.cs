using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Nodes;

namespace LitteraCore.DBContext
{
    public class ApplicationConfigDB
    {
        private readonly IConfiguration _configuration;
        public ApplicationConfigDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

     
        public DataTable Get_Application_Setting(string settinguniqueid)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.sp_get_PortalSetting", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@SettinguniqueID", settinguniqueid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count <= 0)
            {
                if (settinguniqueid == "5")
                {
                    DataRow dr = dt.NewRow();
                    dr["SettingValue"] = "{'FEEDBACK_OTP_REQUIRED':'0'}";
                    dt.Rows.Add(dr);
                }
                if (settinguniqueid == "6")
                {
                    DataRow dr = dt.NewRow();
                    dr["SettingValue"] = "{'OTP_LOGIN_REQUIRED':'0','SMSAPI':'',SMSTEMPLATE:{'ID':'','DLT_CT_ID':'','text':''},EMAILSETTING:{'EMAILID':'','PWD':'','HOST':'','PORT':''}}";
                    dt.Rows.Add(dr);
                }
                if (settinguniqueid == "7")
                {
                    DataRow dr = dt.NewRow();
                    dr["SettingValue"] = "{'IS_MAIL_SEND':'0',EMAILSETTING:{'EMAILID':'','PWD':'','HOST':'','PORT':''}}";
                    dt.Rows.Add(dr);
                }
                if (settinguniqueid == "8")
                {
                    DataRow dr = dt.NewRow();
                    dr["SettingValue"] = "{'IS_SMS_SEND':'0','SMSAPI':''}";
                    dt.Rows.Add(dr);
                }
                if (settinguniqueid == "10")
                {
                    DataRow dr = dt.NewRow();
                    dr["SettingValue"] = "{'data_masking_required':'0'}";
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }


        public bool Save_Trial_Log(Audit_Trail at)
        {
          
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_ins_tbl_yuser_audit_trail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tyat_userid", at.tyat_userid);
            cmd.Parameters.AddWithValue("@tyat_page_name", at.tyat_page_name);
            cmd.Parameters.AddWithValue("@tyat_event_name", at.tyat_event_name);
            if (at.tyat_recordid.Trim() == "")
            {
                cmd.Parameters.AddWithValue("@tyat_recordid",DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tyat_recordid", at.tyat_recordid);
            }
     
            cmd.Parameters.AddWithValue("tyat_ip", at.tyat_ip);
            string jsonString = JsonConvert.SerializeObject(at.device_Info);
            // cmd.Parameters.AddWithValue("deciceinfo", at.tyat_ip);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            con.Close();
     
            return true;
        }
        

        public bool Save_Error_Log(Error_Log at)
        {

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_ins_tbl_yuser_error_log", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tyel_userid", at.tyel_userid);
            cmd.Parameters.AddWithValue("@tyel_page_name", at.tyel_page_name);
            cmd.Parameters.AddWithValue("@tyel_event_name", at.tyel_event_name);
            cmd.Parameters.AddWithValue("@tyel_error", at.tyel_error);
            cmd.Parameters.AddWithValue("@tyel_recordid", at.tyel_recordid);
            cmd.Parameters.AddWithValue("tyel_ip", at.tyel_ip);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public PagedList<Audit_Trail> Get_Audit_Trail(PaginationParam param,string userid=null,string fromdate=null,string todate=null)
        {

            List<Audit_Trail> f = new List<Audit_Trail>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_get_tbl_yuser_audit_trail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (userid != null)
            {
                cmd.Parameters.AddWithValue("@tyat_userid", userid);
            }
           if(fromdate != null)
            {
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
            }
           if (todate != null)
            {
                cmd.Parameters.AddWithValue("@todate", todate);
            }
          
           
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                f.Add(
                    new Audit_Trail
                    {
                         tyat_userid= Convert.ToString(dr["tyat_userid"]),
                         tyat_page_name= Convert.ToString(dr["tyat_page_name"]),
                         tyat_event_name= Convert.ToString(dr["tyat_event_name"]),
                         tyat_recordid= Convert.ToString(dr["tyat_recordid"]),
                         tyat_createdon = Convert.ToDateTime(dr["tyat_createdon"]),
                         tyat_ip= Convert.ToString(dr["tyat_ip"])

                    });
            }

            if (param == null)
            {
                return PagedList<Audit_Trail>.ToPagedList(f.ToList(),
                   param.PageNumber,
                   f.Count());
            }
            else
            {
                if (param.PageSize > 0)
                {
                    return PagedList<Audit_Trail>.ToPagedList(f.ToList(),
                param.PageNumber,
                param.PageSize);
                }
                else
                {
                    return PagedList<Audit_Trail>.ToPagedList(f.ToList(),
                param.PageNumber,
               f.Count());
                }

            }


        }

        public PagedList<Error_Log> Get_Error_Log(PaginationParam param, string userid=null, string fromdate=null, string todate=null)
        {

            List<Error_Log> f = new List<Error_Log>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("yuser.proc_get_tbl_yuser_error_log", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (userid != null)
            {
                cmd.Parameters.AddWithValue("@tyel_userid", userid);
            }
            if (fromdate != null)
            {
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
            }
            if (todate != null)
            {
                cmd.Parameters.AddWithValue("@todate", todate);
            }


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                f.Add(
                    new Error_Log
                    { 
                        tyel_userid = Convert.ToString(dr["tyel_userid"]),
                        tyel_page_name = Convert.ToString(dr["tyel_page_name"]),
                        tyel_event_name = Convert.ToString(dr["tyel_event_name"]),
                        tyel_recordid = Convert.ToString(dr["tyel_recordid"]),
                        tyel_createdon = Convert.ToDateTime(dr["tyel_createdon"]),
                        tyel_error= Convert.ToString(dr["tyel_error"]),
                         tyel_ip = Convert.ToString(dr["tyel_ip"])

                    });
            }

            if (param == null)
            {
                return PagedList<Error_Log>.ToPagedList(f.ToList(),
                   param.PageNumber,
                   f.Count());
            }
            else
            {
                if (param.PageSize > 0)
                {
                    return PagedList<Error_Log>.ToPagedList(f.ToList(),
                param.PageNumber,
                param.PageSize);
                }
                else
                {
                    return PagedList<Error_Log>.ToPagedList(f.ToList(),
                param.PageNumber,
               f.Count());
                }

            }


        }

        public Branch_Configuration GET_BRANCH_CONFIGURATION()
        {

            //string Foldername = CommonDB.GET_JSON__FOLDER();
            //string jsontxt = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/TrainingSettings.json"));
            ApplicationSetting a = new ApplicationSetting();
            HttpResponseMessage response = new HttpResponseMessage();
            Branch_Configuration ml = new Branch_Configuration();
            ml = JsonConvert.DeserializeObject<Branch_Configuration>(Get_Application_Setting("9").Rows[0]["SettingValue"].ToString());
            return ml;
        }


        public  Branch_Configuration GET_BRANCH_CONFIGURATION()
        {

            //string Foldername = CommonDB.GET_JSON__FOLDER();
            //string jsontxt = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/TrainingSettings.json"));
            ApplicationSetting a = new ApplicationSetting();
            HttpResponseMessage response = new HttpResponseMessage();
            Branch_Configuration ml = new Branch_Configuration();
            ml = JsonConvert.DeserializeObject<Branch_Configuration>(Get_Application_Setting("9").Rows[0]["SettingValue"].ToString());
            return ml;
        }

    }
}
