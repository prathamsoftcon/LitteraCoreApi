using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.PowerBI.Api.Models;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using static System.Net.WebRequestMethods;

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
            SqlConnection con = new SqlConnection(connectionString); if (con.State != ConnectionState.Open) { con.Open(); }
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

        public List<ContentFolder> Get_Content_Folder_Data(string? folderid = null)
        {
            List<ContentFolder> folders = new List<ContentFolder>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("[Content].[tbl_Content_FolderSelect]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            if (!string.IsNullOrWhiteSpace(folderid))
            {
                cmd.Parameters.AddWithValue("@GlobalContentFolderID", folderid);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            //rowno GlobalContentFolderID   GlobalContentFolderName IsActive    CreatedByAgencyID CreatedOn

            foreach (DataRow row in dt.Rows)
            {
                folders.Add(new ContentFolder
                {
                    globalcontentfolderid = Convert.ToString(row["GlobalContentFolderID"]),
                    globalcontentfoldername = Convert.ToString(row["GlobalContentFolderName"]),
                    createdbyagencyid = Convert.ToString(row["CreatedbyAgencyID"]),
                    createdon = Convert.ToString(row["CreatedOn"]),
                    IsActive = Convert.ToString(row["IsActive"])
                });
            }

            return folders;
        }

        public List<contentType> Get_Global_File_Type()
        {
            return Get_Content_Type_All();
        }

        public List<contentType> Get_Content_Type_All()
        {

            List<contentType> AL = new List<contentType>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); if (con.State != ConnectionState.Open) { con.Open(); }
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
            if (con.State != ConnectionState.Open) { con.Open(); }
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
                if(row["tcm_content_reading_time"] != null)
                {
                    if(Convert.ToString(row["tcm_content_reading_time"]) != "")
                    {
                        vw.minreadingtime = Convert.ToInt32(row["tcm_content_reading_time"]);
                    }
                  
                }


                //vw.GlobalWysiwagText = Convert.ToString(row["GlobalWysiwagText"]);
                vw.GlobalWysiwagText = Convert.ToString(row["GlobalWysiwagText"]).Trim().Replace("\u200B", "");
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
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_ins_participant_learning_time", con);
            cmd.Parameters.AddWithValue("@tplt_Id", lt.tplt_Id);
            cmd.Parameters.AddWithValue("@tplt_ttsam_id", lt.tplt_ttsam_id);
            cmd.Parameters.AddWithValue("@tplt_ttpai_id", lt.tplt_ttpai_id);
            cmd.Parameters.AddWithValue("@tplt_learning_time", lt.tplt_learning_time);
            cmd.Parameters.AddWithValue("@tplt_createdon", lt.tplt_createdon);
            cmd.Parameters.AddWithValue("@tplt_createdby", lt.tplt_createdby);
            cmd.Parameters.AddWithValue("@tplt_sessionid", lt.tplt_sessionid);
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
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand(@"select GlobalWysiwagText,ttsam_trg_id,ttsam_ttttt_session_id from trainingplan.tbl_tp_session_attachment_master tam
inner join Content.tbl_ContentMaster cm on tam.ttsam_globalcontentid = cm.GlobalContentID
where ttsam_id = @ContentId", con);
            cmd.Parameters.Add("@ContentId", SqlDbType.NVarChar, 100).Value = ttsam_id ?? string.Empty;
         
            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                cd.content_path = Convert.ToString(dt.Rows[0]["GlobalWysiwagText"]).Trim().Replace("\u200B", "");
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
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand(@"select ai.ttpai_id,am.ag_mobileno,am.AgencyId,amp.tyuam_userid from TrainingPlan.tbl_tp_participant_additional_info ai 
inner join YUser.AgencyMaster am on ai.Participantid=am.AgencyId 
inner join YUser.tbl_yuser_user_agency_mapping amp on amp.tyuam_agency_id=am.AgencyId
where TrainingId = (select ttsam_trg_id from TrainingPlan.tbl_tp_session_attachment_master
where ttsam_id = @ContentId) and Participantid = @ParticipantId", con);
            cmd.Parameters.Add("@ContentId", SqlDbType.NVarChar, 100).Value = contentid ?? string.Empty;
            cmd.Parameters.Add("@ParticipantId", SqlDbType.NVarChar, 100).Value = participantid ?? string.Empty;


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
                cd.userid = Convert.ToString(dt.Rows[0]["tyuam_userid"]);
            }

            return cd;

        }

        public bool INSERT_CONTENT_VISITING(string contentid, string mobileno, string ip)
        {

            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_insert_track_content", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ContentID", contentid);
            cmd.Parameters.AddWithValue("@MobileNo", mobileno);
            cmd.Parameters.AddWithValue("@IPAddress", ip);
            cmd.ExecuteNonQuery();
            con.Close();
            return true;

        }

        public bool Save_Activity_Data(activity_data a)
        {

            List<Content> AL = new List<Content>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_insert_activity_data", con);
            cmd.Parameters.AddWithValue("@tpad_id",a.tpad_id);
            cmd.Parameters.AddWithValue("@tpad_activity_id",a.tpad_activity_id);
            cmd.Parameters.AddWithValue("@tpad_ttpai_id", a.tpad_ttpai_id);
            cmd.Parameters.AddWithValue("@tpad_ttsam_id",a.tpad_ttsam_id);
            cmd.Parameters.AddWithValue("@tpad_activity_data",a.tpad_activity_data);
            cmd.Parameters.AddWithValue("@tpad_createdon",a.tpad_createdon);
           
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.ExecuteNonQuery();
            con.Close();

            return true;

        }

        public List<activity_data> Get_Activity_Data(string agencyid, string activityid = null)
        {

            List<activity_data> AL = new List<activity_data>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }

            SqlCommand cmd = new SqlCommand();
            if (activityid != null)
            {
                 cmd = new SqlCommand("select * from TrainingPlan.tbl_tp_activity_data act inner join TrainingPlan.tbl_tp_participant_additional_info ai on ai.ttpai_id = act.tpad_ttpai_id where ai.Participantid = @AgencyId and tpad_activity_id = @ActivityId", con);
                 cmd.Parameters.Add("@ActivityId", SqlDbType.NVarChar, 100).Value = activityid;
            }
            else
            {
                 cmd = new SqlCommand("select * from TrainingPlan.tbl_tp_activity_data act inner join TrainingPlan.tbl_tp_participant_additional_info ai on ai.ttpai_id = act.tpad_ttpai_id where ai.Participantid = @AgencyId", con);
            }
            cmd.Parameters.Add("@AgencyId", SqlDbType.NVarChar, 100).Value = agencyid ?? string.Empty;
          
            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
         

            foreach (DataRow row in dt.Rows)
            {
                activity_data vw = new activity_data();
                vw.tpad_id = Convert.ToString(row["tpad_id"]);
                vw.tpad_activity_id = Convert.ToString(row["tpad_activity_id"]);
                vw.tpad_ttpai_id = Convert.ToString(row["tpad_ttpai_id"]);
                vw.tpad_ttsam_id    = Convert.ToString(row["tpad_ttsam_id"]);

                vw.tpad_activity_data = Convert.ToString(row["tpad_activity_data"]);
                vw.tpad_createdon = Convert.ToDateTime(row["tpad_createdon"]);
                //vw.tpad_upload = Convert.ToString(row["tpad_upload"]);
              
                AL.Add(vw);

            }

          
    

            return AL;

        }



        public List<activity_data> Get_Activity_Data_by_id(string tpad_id)
        {

            List<activity_data> AL = new List<activity_data>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }

            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select * from TrainingPlan.tbl_tp_activity_data where tpad_id = @ActivityDataId", con);
            cmd.Parameters.Add("@ActivityDataId", SqlDbType.NVarChar, 100).Value = tpad_id ?? string.Empty;

            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();


            foreach (DataRow row in dt.Rows)
            {
                activity_data vw = new activity_data();
                vw.tpad_id = Convert.ToString(row["tpad_id"]);
                vw.tpad_activity_id = Convert.ToString(row["tpad_activity_id"]);
                vw.tpad_ttpai_id = Convert.ToString(row["tpad_ttpai_id"]);
                vw.tpad_ttsam_id = Convert.ToString(row["tpad_ttsam_id"]);

                vw.tpad_activity_data = Convert.ToString(row["tpad_activity_data"]);
                vw.tpad_createdon = Convert.ToDateTime(row["tpad_createdon"]);
                //vw.tpad_upload = Convert.ToString(row["tpad_upload"]);

                AL.Add(vw);

            }




            return AL;

        }

        public bool check_content_learning_exist(string ttsam_id,string participantid)
        {

            List<contentType> AL = new List<contentType>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("SELECT * FROM TrainingPlan.tbl_participant_learning_time WHERE tplt_ttsam_id = @tplt_ttsam_id AND tplt_participantid = @tplt_participantid;", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@tplt_ttsam_id", ttsam_id);
            cmd.Parameters.AddWithValue("@tplt_participantid", participantid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0) { 
                return true;
            }
            else
            {
                return false;
            }

            





            return true;
        }


        public List<Avg_Learning_data> Get_trg_avg_learning_Time(string trainingid)
        {
            SessionBL cbl = new SessionBL(_configuration);
            List<Session> ls=new List<Session>();
            ls = cbl.Get_Session_Data_By_Trg(trainingid);

            ContentBL CBL = new ContentBL(_configuration);
            PagedResult<Content> cl = new PagedResult<Content>();
            PaginationParam p = new PaginationParam();
            cl = CBL.Get_Trg_Content(trainingid, null, null,p);
           

            List<Avg_Learning_data> AL = new List<Avg_Learning_data>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_get_learning_time_bi_data", con);
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
           
            foreach (DataRow row in dt.Rows)
            {
                Avg_Learning_data vw = new Avg_Learning_data();
                vw.tplt_trainingid = Convert.ToString(row["ttbfld_trainingid"]);
                vw.tplt_sessionid = Convert.ToString(row["ttbfld_sessionid"]);
                vw.tplt_ttsam_id = Convert.ToString(row["ttbfld_ttsam_id"]);
                vw.content_total_Reading_time = Convert.ToDecimal(row["ttbfld_total_learningtime"]);
                vw.avg_learning = Convert.ToDecimal(row["ttbfld_avg_learningtime"]);
                if(ls.Where(o=>o.ttttt_session_id.ToString().ToUpper()== Convert.ToString(row["ttbfld_sessionid"]).ToString().ToUpper()).Count()>0){
                    vw.ttttt_content_desc = ls.Where(o => o.ttttt_session_id.ToString().ToUpper() == Convert.ToString(row["ttbfld_sessionid"]).ToString().ToUpper()).FirstOrDefault().ttttt_content_desc;
                    vw.ttttt_subject = ls.Where(o => o.ttttt_session_id.ToString().ToUpper() == Convert.ToString(row["ttbfld_sessionid"]).ToString().ToUpper()).FirstOrDefault().ttttt_subject;
                }
               if(cl.Items.Where(o=>o.ttsam_id== Convert.ToString(row["ttbfld_ttsam_id"])).Count() > 0)
                {
                    vw.content_title = cl.Items.Where(o => o.ttsam_id == Convert.ToString(row["ttbfld_ttsam_id"])).FirstOrDefault().ttsad_title;
                }
               
             
                AL.Add(vw);
            }





            return AL;
        }


    }
}
