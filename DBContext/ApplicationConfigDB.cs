using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
            con.Open();
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

            }
            return dt;
        }

    }
}
