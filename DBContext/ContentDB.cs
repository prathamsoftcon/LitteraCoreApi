using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.DBContext
{
    public class ContentDB
    {
        private readonly IConfiguration _configuration;
        public ContentDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<contentType> Get_Content_Type()
        {

            List<contentType> AL = new List<contentType>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); con.Open();
            SqlCommand cmd = new SqlCommand("Content.tbl_GlobalContentTypeSelect", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "SessionAttachmentType <> '9'";
            dt = dt.DefaultView.ToTable();
            foreach (DataRow row in dt.Rows)
            {
                contentType vw = new contentType();
                vw.GlobalContentTypeID = Convert.ToString(row["GlobalContentTypeID"]);
                vw.GlobalContentType = Convert.ToString(row["GlobalContentType"]);
                vw.SessionAttachmentType = Convert.ToString(row["SessionAttachmentType"]);
                vw.GlobalContentDescription = Convert.ToString(row["GlobalContentDescription"]);



                AL.Add(vw);
            }





            return AL;
        }

        public List<Content> Get_Trg_Content(PaginationParam param,string trainingid = null, string sessionid = null)
        {

            List<Content> AL = new List<Content>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Trainingplan.proc_tp_get_upload_session_attachement", con);
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@trainingid", trainingid);
            }
            if (sessionid != null)
            {
                cmd.Parameters.AddWithValue("@sessionid", sessionid);
            }
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "ttsad_status <> -1";
            dt = dt.DefaultView.ToTable();
      
            foreach (DataRow row in dt.Rows)
            {
                Content vw = new Content();
                vw.ttsam_id = Convert.ToString(row["ttsam_id"]);
                vw.ttsam_trg_id = Convert.ToString(row["ttsam_trg_id"]);
                vw.ttsam_ttttt_session_id = Convert.ToString(row["ttsam_ttttt_session_id"]);
                vw.ttsam_created_by = Convert.ToString(row["ttsam_created_by"]);

                vw.ttsam_created_on = Convert.ToString(row["ttsam_created_on"]);
                vw.ttsam_globalcontentid = Convert.ToString(row["ttsam_globalcontentid"]);
                vw.ttsad_ttsam_id = Convert.ToString(row["ttsad_ttsam_id"]);
                vw.ttsad_title = Convert.ToString(row["ttsad_title"]);
                vw.ttsad_tag = Convert.ToString(row["ttsad_tag"]);
                vw.ttsad_status = Convert.ToString(row["ttsad_status"]);
                vw.ttttt_session_dt = Convert.ToString(row["ttttt_session_dt"]);
                vw.ttsar_user_type_id = Convert.ToString(row["ttsar_user_type_id"]);
                vw.empname = Convert.ToString(row["empname"]);

                vw.emailid = Convert.ToString(row["emailid"]);
                vw.modifiedempname = Convert.ToString(row["modifiedempname"]);
                vw.modifiedemailid = Convert.ToString(row["modifiedemailid"]);
                vw.GlobalContentyTypeID = Convert.ToString(row["GlobalContentyTypeID"]);
                vw.GlobalContentFolderID = Convert.ToString(row["GlobalContentFolderID"]);

                vw.GlobalWysiwagText = Convert.ToString(row["GlobalWysiwagText"]);
                if (Convert.ToString(row["GlobalFilePath"]) != "")
                {

                    vw.GlobalFilePath =  Convert.ToString(row["GlobalFilePath"]);
                }
                if (Convert.ToString(row["GlobalthumbnailPath"]) != "")
                {

                    vw.GlobalthumbnailPath = Convert.ToString(row["GlobalthumbnailPath"]);
                }
                else
                {
                    vw.GlobalthumbnailPath = "";
                }

                vw.GlobalFileName = Convert.ToString(row["GlobalFileName"]);
                vw.SessionAttachmentType = Convert.ToString(row["SessionAttachmentType"]);
                vw.tdds_status = Convert.ToString(row["tdds_status"]);

                List<contentuserpermission> userpermissions = new List<contentuserpermission>();

                dt.DefaultView.RowFilter = "ttsam_id='" + row["ttsam_id"] + "'";
                DataTable dtpermissions = dt.DefaultView.ToTable();
                for (int i = 0; i <= dtpermissions.Rows.Count - 1; i++)
                {
                    contentuserpermission cup = new contentuserpermission();
                    cup.usertype = dtpermissions.Rows[i]["ttsar_user_type_id"].ToString();

                    contentPermissions p = new contentPermissions();
                    p.ttsar_view = dtpermissions.Rows[i]["ttsar_view"].ToString();
                    p.ttsar_edit = dtpermissions.Rows[i]["ttsar_edit"].ToString();
                    p.ttsar_delete = dtpermissions.Rows[i]["ttsar_delete"].ToString();
                    p.ttsar_download = dtpermissions.Rows[i]["ttsar_download"].ToString();


                    cup.permission = p;
                    userpermissions.Add(cup);

                }








                vw.contentuserpermission = userpermissions.ToArray();



                AL.Add(vw);

            }

            List<Content> FL = new List<Content>();
            foreach (Content c in AL)
            {
                if (FL.Any(a => a.ttsam_id.ToString().ToUpper() == c.ttsam_id.ToString().ToUpper()) == false)
                {
                    FL.Add(c);
                }
            }

            return FL;

        }

       
    }
}
