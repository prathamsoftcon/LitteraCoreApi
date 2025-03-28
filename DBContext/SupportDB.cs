using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography.Xml;

namespace LitteraCore.DBContext
{
    public class SupportDB                                                        
    {
        private readonly IConfiguration _configuration;
        public SupportDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Insert_Support(Support S)
        {
            string referenceid = "";
            List<cast_category> f = new List<cast_category>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Masterconfig");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DBO.proc_insert_support_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@id", S.id);
            cmd.Parameters.AddWithValue("@clienturl", S.clienturl);
            cmd.Parameters.AddWithValue("@name", S.name);
            cmd.Parameters.AddWithValue("@mobileno", S.mobileno);
            cmd.Parameters.AddWithValue("@email", S.email);
            cmd.Parameters.AddWithValue("@description", S.description);
            cmd.Parameters.AddWithValue("@createdon", System.DateTime.Now.ToString("yyyy/MM/dd hh:MM:ss"));
            if (S.createdby != null)
            {
                cmd.Parameters.AddWithValue("@createdby", S.createdby);
            }
            else
            {
                cmd.Parameters.AddWithValue("@createdby", DBNull.Value);
            }
            if (S.upload_path != null)
            {
                cmd.Parameters.AddWithValue("@upload_path", S.upload_path);
            }
            else
            {
                cmd.Parameters.AddWithValue("@upload_path", DBNull.Value);
            }




            referenceid = cmd.ExecuteScalar().ToString();
            con.Close();

            return referenceid;
        }

        public List<Support> GetSupportQuery()
        {

            List<Support> L = new List<Support>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Masterconfig");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_get_tbl_client_support_data", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                string? repoliedon = null;
                if (dr["reply_by"].ToString() != "")
                {
                    repoliedon = Convert.ToString(dr["reply_by"]);
                }
                L.Add(
                    new Support
                    {
                        id = Convert.ToString(dr["id"]),
                        referenceno = Convert.ToInt32(dr["referenceno"]),
                        clienturl = Convert.ToString(dr["clienturl"]),
                        name = Convert.ToString(dr["name"]),
                        mobileno = Convert.ToString(dr["mobileno"]),
                        email = Convert.ToString(dr["email"]),
                        description = Convert.ToString(dr["description"]),
                        upload_path = Convert.ToString(dr["upload_path"]),
                        createdon = Convert.ToString(dr["createdon"]),
                        createdby = Convert.ToString(dr["createdon"]),
                        replied = Convert.ToInt32(dr["replied"]),
                        reply_txt = Convert.ToString(dr["reply_txt"]),
                        reply_by = Convert.ToString(dr["reply_by"]),
                        repliedOn = repoliedon
                    }
                    );
            }

            return L;
        }

        public bool Update_Status(Update_Support us)
        {

            List<Support> L = new List<Support>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Masterconfig");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_update_reply_status", con);
            cmd.Parameters.AddWithValue("@id", us.id);
            cmd.Parameters.AddWithValue("@status", us.status);
            cmd.Parameters.AddWithValue("@remark", us.remark);
            cmd.Parameters.AddWithValue("@reply_by", us.reply_by);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.ExecuteNonQuery();
            con.Close();
      

            return true;
        }
    }
}
