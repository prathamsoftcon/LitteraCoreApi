using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;

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

        public List<contentType> Get_Content_Type_All()
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

        public List<Content> Get_Trg_Content(PaginationParam param, string trainingid = null, string sessionid = null)
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

                    vw.GlobalFilePath = Convert.ToString(row["GlobalFilePath"]);
                }
                if (Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".PDF") == true || Convert.ToString(row["globalWysiwagText"]).ToUpper().Contains(".PDF") == true)
                {
                    vw.content_icon = "<i class='fa fa-file-pdf-o' style='color:red'></i>";
                }
                else if (Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".MP4") == true || Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".WMV") == true || Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".FLV") == true)
                {
                    vw.content_icon = "<i class='fa fa-video-camera' style='color:#428bca'></i>";
                }
                else if (Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".JPG") == true || Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".JPEG") == true || Convert.ToString(row["GlobalFilePath"]).ToUpper().Contains(".PNG") == true)
                {
                    vw.content_icon = "<i class='fa fa-file-image-o' style='color:red'></i>";
                }
                else
                {
                    vw.content_icon = "<i class='fa fa-file-o'></i>";
                }
                //if (Convert.ToString(row["GlobalthumbnailPath"]) != "")
                //{

                //    vw.GlobalthumbnailPath = Convert.ToString(row["GlobalthumbnailPath"]);
                //}
                //else
                //{
                //    if (Convert.ToString(row["GlobalContentyTypeID"]).ToString().ToUpper() == "6ECEC2CD-2780-4DB5-B03C-CA37D3CC8B29")
                //    {

                //        if (Convert.ToString(row["GlobalWysiwagText"]).ToString().ToUpper().Contains(".PDF"))
                //        {
                //            string url = Convert.ToString(row["GlobalWysiwagText"]).Replace("\\", "/");
                //            string pdfurl = url.Substring(0, url.LastIndexOf("/"));
                //            vw.GlobalthumbnailPath = _configuration["CDN_API_PATH"] + "/" + pdfurl+"/Media/Thumbnail.jpg";
                //        }
                //        else
                //        {
                //            string url = Convert.ToString(row["GlobalWysiwagText"]);
                //            string imgpath = url.Substring(0, url.LastIndexOf("/"))+"/Media/Thumbnail.jpg";
                //            vw.GlobalthumbnailPath = imgpath;
                //        }
                //    }
                //    else
                //    {
                //        vw.GlobalthumbnailPath = "";
                //    }

                //}

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

        public bool save_participant_learning_time(learningtime lt)
        {

            List<Content> AL = new List<Content>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_ins_participant_learning_time", con);
            cmd.Parameters.AddWithValue("@tplt_Id", lt.tplt_Id);
            cmd.Parameters.AddWithValue("@tplt_ttsam_id", lt.tplt_ttsam_id);
            cmd.Parameters.AddWithValue("@tplt_ttpai_id", lt.tplt_ttpai_id);
            cmd.Parameters.AddWithValue("@tplt_learning_time", lt.tplt_learning_time);
            cmd.Parameters.AddWithValue("@tplt_createdon", lt.tplt_createdon);
            cmd.Parameters.AddWithValue("@tplt_createdby", lt.tplt_createdby);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.ExecuteNonQuery();
            con.Close();
           
            return true;

        }

        public contentDetail Get_Content_Detail(string ttsam_id)
        {
            contentDetail cd = new contentDetail();
            DataTable dt = new DataTable();
            string path = "";
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(@"select GlobalWysiwagText,ttsam_trg_id,ttsam_ttttt_session_id from trainingplan.tbl_tp_session_attachment_master tam
inner join Content.tbl_ContentMaster cm on tam.ttsam_globalcontentid = cm.GlobalContentID
where ttsam_id = '"+ ttsam_id + "'", con);
         
            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                cd.content_path = Convert.ToString(dt.Rows[0]["GlobalWysiwagText"]);
                cd.sessionid = Convert.ToString(dt.Rows[0]["ttsam_ttttt_session_id"]);
                cd.trainingid= Convert.ToString(dt.Rows[0]["ttsam_trg_id"]);
            }

            return cd;

        }

        public contentDetail Get_ttpai_from_Content(string contentid,string participantid)
        {
            contentDetail cd = new contentDetail();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(@"select ai.ttpai_id,am.ag_mobileno from TrainingPlan.tbl_tp_participant_additional_info ai 
inner join YUser.AgencyMaster am on ai.Participantid=am.AgencyId where 
TrainingId = (select ttsam_trg_id from TrainingPlan.tbl_tp_session_attachment_master
where ttsam_id = '"+ contentid + "') and Participantid = '"+ participantid + "'", con);


            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                cd.ttpai_id= Convert.ToString(dt.Rows[0]["ttpai_id"]);
                cd.mobileno = Convert.ToString(dt.Rows[0]["ag_mobileno"]);
            }

            return cd;

        }


    }
}
