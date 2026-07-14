using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
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


        public List<contentuserpermission> Get_Content_Permission(string attachmentid, string usertype)
        {
            List<contentuserpermission> AL = new List<contentuserpermission>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_get_attachement_permission", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trgid", DBNull.Value);
            cmd.Parameters.AddWithValue("@sessionid", DBNull.Value);
            cmd.Parameters.AddWithValue("@attachmentid", attachmentid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@usertype", usertype ?? (object)DBNull.Value);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                contentuserpermission cup = new contentuserpermission();
                cup.usertype = Convert.ToString(row["ttsar_user_type_id"]);

                contentPermissions p = new contentPermissions();
                p.ttsar_view = Convert.ToString(row["ttsar_view"]);
                p.ttsar_edit = Convert.ToString(row["ttsar_edit"]);
                p.ttsar_download = Convert.ToString(row["ttsar_download"]);
                p.ttsar_delete = Convert.ToString(row["ttsar_delete"]);

                cup.permission = p;
                AL.Add(cup);
            }

            return AL;
        }

        // ===================================================================
        // Everything below added 2026-07-12 for the frm_global_content_library.aspx
        // -> React migration.
        // ===================================================================

        // Old page called the generic /TrainingApi/Get_Data dispatcher with
        // ProcedureName=TrainingPlan.proc_TP_Get_tag, @columnname=GlobalContentTag,
        // @tblname=Content.tbl_ContentMaster - both literal values are always the
        // same on this page, so they are hardcoded here rather than parameterized
        // for the caller.
        public List<string> Get_Content_Tags()
        {
            List<string> tags = new List<string>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_TP_Get_tag", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@columnname", "GlobalContentTag");
            cmd.Parameters.AddWithValue("@tblname", "Content.tbl_ContentMaster");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow row in dt.Rows)
            {
                tags.Add(Convert.ToString(row["value"]));
            }
            return tags.Distinct().ToList();
        }

        // Mirrors the established ApplicationConfigDB.Get_Application_Setting(...)
        // JSON-blob-by-SettingID idiom exactly, the same way
        // ParticipantDB.cs/SupportDB.cs read masking setting id "10" and
        // SmtpEmailService.cs reads OTP/email settings ids "6"/"7".
        //
        // OPEN ITEM: SettingID "12" is a NEWLY INVENTED id - no existing
        // SettingID fit share_content_on_google_drive. Unlike other ids, "12"
        // has no fallback default wired into
        // ApplicationConfigDB.Get_Application_Setting, so until a real DB row
        // (SettingID=12, SettingValue='{"share_content_on_google_drive":"0"}')
        // is created, this method safely returns false (Rows.Count <= 0)
        // rather than throwing.
        public bool Get_Share_Content_On_Google_Drive_Setting()
        {
            ApplicationConfigDB ACDB = new ApplicationConfigDB(_configuration);
            DataTable dt = ACDB.Get_Application_Setting("12");
            if (dt.Rows.Count <= 0)
            {
                return false;
            }
            Share_Content_Google_Drive_Setting setting =
                JsonConvert.DeserializeObject<Share_Content_Google_Drive_Setting>(dt.Rows[0]["SettingValue"].ToString());
            return setting != null && setting.share_content_on_google_drive;
        }

        // The core paged/searched/filtered content-grid listing.
        // CORRECTED 2026-07-12: the original draft used a fabricated proc
        // name/column set. Re-derived from the REAL old implementation at
        // C:\Projects\TraininingERP_old\Littera_MVC_API\Models\Content\ContentDB.cs
        // (Get_Global_Content), reached via the old app's
        // https://qa.littera.in/LitteraAPI/api/GlobalContent (ContentController.cs
        // -> ContentBL.Get_Global_Content). Real stored proc:
        // Trainingplan.Proc_TP_Get_Global_Content_withSearch. Real total-count
        // column is "TotalRow_count" (not "TotalRecords"). Real DMS/approval
        // columns are prefixed "tdds_*" (not "doc_status" etc. - see
        // Common/DMS.cs in the old repo). file_absolute_path/
        // thumbnail_absolute_path are NOT database columns - they are computed
        // here exactly like the old Common.UploadPath convention did, from an
        // "appurl" base URL passed in by the caller (old: the ASP.NET app's own
        // origin via HF_APPLICATION_URL; new: the frontend's
        // config.LITTERA_CDN_BASE_URL, since uploaded content is still served
        // from that same legacy static-file location).
        public List<GlobalContentListItem> Get_Global_Content_List(string appurl = null, string folderid = null, string searchcolumn = null, string searchvalue = null, string filtervalue = null, int pageno = 1, int pagesize = 8)
        {

            List<GlobalContentListItem> AL = new List<GlobalContentListItem>();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            using (con)
            {
                SqlCommand cmd = new SqlCommand("Trainingplan.Proc_TP_Get_Global_Content_withSearch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandTimeout = 120;

                cmd.Parameters.AddWithValue("@GlobalContentFolderID", folderid ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SearchValue", searchvalue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterValue", filtervalue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PageNo", pageno);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);

                SqlDataReader row = cmd.ExecuteReader();
                while (row.Read())
                {
                    GlobalContentListItem gc = new GlobalContentListItem();
                    gc.GlobalContentID = Convert.ToString(row["GlobalContentID"]);
                    gc.GlobalContentyTypeID = Convert.ToString(row["GlobalContentyTypeID"]);
                    gc.ContentType = Convert.ToString(row["ContentType"]);
                    gc.GlobalContentTitle = Convert.ToString(row["GlobalContentTitle"]);
                    gc.GlobalContentTag = Convert.ToString(row["GlobalContentTag"]);
                    gc.GlobalContentFolderID = Convert.ToString(row["GlobalContentFolderID"]);
                    gc.GlobalWysiwagText = Convert.ToString(row["GlobalWysiwagText"]);
                    gc.GlobalFilePath = Convert.ToString(row["GlobalFilePath"]);
                    gc.GlobalthumbnailPath = Convert.ToString(row["GlobalthumbnailPath"]);
                    gc.GlobalFileName = Convert.ToString(row["GlobalFileName"]);
                    gc.ContentCreatedBy = Convert.ToString(row["ContentCreatedBy"]);
                    gc.ContentCreatedon = row["ContentCreatedon"] != DBNull.Value ? Convert.ToDateTime(row["ContentCreatedon"]) : (DateTime?)null;
                    gc.ApprovedBy = Convert.ToString(row["ApprovedBy"]);
                    gc.TotalRecords = row["TotalRow_count"] != DBNull.Value ? Convert.ToInt32(row["TotalRow_count"]) : 0;

                    // Absolute file path - old convention: APPURL + "/Training_Upload/Content/" + GlobalFilePath.
                    if (!string.IsNullOrEmpty(gc.GlobalFilePath) && !string.IsNullOrEmpty(appurl))
                    {
                        gc.file_absolute_path = appurl + "/Training_Upload/Content/" + gc.GlobalFilePath;
                    }

                    // Absolute thumbnail path - old convention: if a real thumbnail
                    // was uploaded, APPURL + "/Training_Upload/Thumbnails/" + it;
                    // otherwise fall back to a default image keyed by ContentType.
                    // NOTE: matches old ContentDB.Get_Global_Content (lines ~255-269) exactly -
                    // the fallback branch is NOT guarded by "appurl non-empty". The old app
                    // always sets thumbnail_absolute_path when GlobalthumbnailPath is empty,
                    // even if appurl itself is empty (producing "/DefaultPath..." with no
                    // host prefix in that edge case). Intentionally kept as-is rather than
                    // "improved", to match old-app behavior exactly.
                    if (!string.IsNullOrEmpty(gc.GlobalthumbnailPath) && !string.IsNullOrEmpty(appurl))
                    {
                        gc.thumbnail_absolute_path = appurl + "/Training_Upload/Thumbnails/" + gc.GlobalthumbnailPath;
                    }
                    else if (string.IsNullOrEmpty(gc.GlobalthumbnailPath))
                    {
                        gc.thumbnail_absolute_path = appurl + "/" + Get_Content_Default_Thumbnail(gc.ContentType);
                    }

                    if (row["content_link_type"] != DBNull.Value && int.TryParse(Convert.ToString(row["content_link_type"]), out int linkType))
                    {
                        gc.content_link_type = linkType;
                    }
                    if (row["tcm_content_reading_time"] != DBNull.Value && int.TryParse(Convert.ToString(row["tcm_content_reading_time"]), out int readingTime))
                    {
                        gc.tcm_content_reading_time = readingTime;
                    }

                    gc.dmsinfo = new GlobalContentDmsInfo
                    {
                        doc_id = Convert.ToString(row["tdds_doc_id"]),
                        tat_type_id = row["tdds_tat_type_id"] != DBNull.Value ? Convert.ToInt32(row["tdds_tat_type_id"]) : 0,
                        attached_doc_name = Convert.ToString(row["tdds_uploaded_doc_name"]),
                        CreatedBy_empid = Convert.ToString(row["tdds_sendby_empid"]),
                        fwd_empid = Convert.ToString(row["tdds_fwd_empid"]),
                        doc_status = row["tdds_status"] != DBNull.Value ? Convert.ToInt32(row["tdds_status"]) : 0,
                        docremark = Convert.ToString(row["tdds_remark"]),
                        docno = Convert.ToString(row["tdds_doc_no"]),
                        doctype = row["tdds_doc_type"] != DBNull.Value ? Convert.ToInt32(row["tdds_doc_type"]) : 0,
                    };

                    AL.Add(gc);
                }
            }

            return AL;

        }

        // Ported verbatim from the old app's Common.UploadPath.Get_Content_Default_Images
        // (C:\Projects\TraininingERP_old\Littera_MVC_API\Common\UploadPath.cs) - maps a
        // resolved content type to its default thumbnail image path when no real
        // thumbnail was uploaded. NOTE: the old method's return values include a LEADING
        // slash (e.g. "/Training_Upload/Thumbnails/Thumb_img.PNG"), and the old caller
        // (ContentDB.Get_Global_Content, line ~266) builds the final URL as
        // APPURL + "/" + Get_Content_Default_Images(type) - i.e. it always inserts an
        // extra "/" on top of the leading slash already in the returned path, producing
        // a double slash after the host (APPURL + "//Training_Upload/..."). This mirrors
        // the same double-slash convention already seen elsewhere in this old app's URLs
        // (e.g. ".../LitteraAPI//api/GlobalContent"), so it is kept here exactly rather
        // than "cleaned up", to match old-app behavior byte-for-byte.
        private static string Get_Content_Default_Thumbnail(string type)
        {
            string t = (type ?? string.Empty).ToUpperInvariant();
            switch (t)
            {
                case "JPEG":
                case "JPG":
                case "PNG":
                    return "/Training_Upload/Thumbnails/Thumb_img.PNG";
                case "WMV":
                case "FLV":
                case "WEBM":
                case "AVCHD":
                case "MKV":
                case "MOV":
                case "MP4":
                case "MP3":
                    return "/Training_Upload/Thumbnails/Thumb_video.PNG";
                case "PDF":
                    return "/Training_Upload/Thumbnails/Thumb_pdf.PNG";
                case "PPT":
                case "PPTX":
                    return "/Training_Upload/Thumbnails/Thumb_ppt.PNG";
                case "WYISIWYG":
                    return "/Training_Upload/Thumbnails/Thumb_wsywig.PNG";
                case "DOC":
                case "DOCX":
                    return "/Training_Upload/Thumbnails/Thumb_word.PNG";
                case "HTML":
                    return "/Training_Upload/Thumbnails/Interactive_content.PNG";
                default:
                    return "";
            }
        }

        // "Upload Files" gap. Ground truth: old
        // C:\Projects\TraininingERP_old\Littera_MVC_API\Models\Content\ContentDB.cs
        // Save_Global_Content/INS_GLOBAL_CONTENT (real stored proc
        // content.sp_insert_tbl_ContentMaster_v1, traced param-by-param) plus the
        // DMS row it inserts in the same transaction (tat_type_id = 119 /
        // DMS_TAT_TYPE_ID.Global_Content_Id - matches Content_Approve_Reject's
        // already-hardcoded 119 elsewhere in this file). Uses the SAME
        // DMSBL.Save_DMS_DATA helper already shared by CompetencyDB/TrainingDB/
        // UserDB - not reimplemented here. doc_status is hardcoded to 0
        // (Pending) - a brand new upload is never created pre-approved, so this
        // is not something the client should be able to set.
        public bool Save_Global_Content(SaveGlobalContent g)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlTransaction st = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("content.sp_insert_tbl_ContentMaster_v1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = st;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@p_GlobalContentID", g.GlobalContentID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalContentyTypeID", g.GlobalContentyTypeID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalContentTitle", g.GlobalContentTitle ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalContentTag", g.GlobalContentTag ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalContentFolderID", g.GlobalContentFolderID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalWysiwagText", g.GlobalWysiwagText ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalFilePath", g.GlobalFilePath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_GlobalFileName", g.GlobalFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Global_Thumbnail_FilePath", g.Global_Thumbnail_FilePath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@createdon", DateTime.Now);
                cmd.Parameters.AddWithValue("@createdby", g.CreatedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@branchid", g.Branchid ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy_empid", g.CreatedByEmpId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_content_link_type", g.content_link_type);
                cmd.Parameters.AddWithValue("@p_tcm_content_reading_time", g.tcm_content_reading_time);
                cmd.ExecuteNonQuery();

                DMSBL dbl = new DMSBL(_configuration);
                DMS d = new DMS
                {
                    docno = g.GlobalContentID,
                    doc_id = g.GlobalContentID,
                    createdon = DateTime.Now,
                    createdby = g.CreatedBy,
                    branchid = g.Branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = g.CreatedByEmpId,
                    fwd_empid = g.CreatedByEmpId,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.Global_Content_Id),
                    doc_status = 0,
                };
                dbl.Save_DMS_DATA(d, con, st);

                st.Commit();
                return true;
            }
            catch (Exception ex)
            {
                st.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // "Edit Content" save. Ground truth: JS_frm_global_content_library.js
        // L3122-3210 ($scope.LMS_UPDATE_CONTENT_DATA).
        public bool Update_Global_Content(string globalContentTitle, string globalContentId, string globalContentTag, string globalWysiwagText, string globalThumbnailPath, int readingTime)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("content.sp_update_tbl_ContentMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.Parameters.AddWithValue("@p_GlobalContentTitle", globalContentTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@w_GlobalContentID", globalContentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_GlobalContentTag", globalContentTag ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_GlobalWysiwagText", globalWysiwagText ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@w_GlobalthumbnailPath", globalThumbnailPath ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@p_tcm_content_reading_time", readingTime);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        // Folder create/rename. Ground truth: uc_folder_creation.ascx's
        // LMS_UC_FC_CREATE_FOLDER() -> old FolderController.Insupd_Folder_Data
        // -> Datamanager.Insupd_Folder_Data -> content.tbl_Content_FolderInsert.
        // Same proc handles insert (folderId empty/null -> @GlobalContentFolderID
        // omitted, matching the old "If Not folderid Is Nothing" guard) and
        // update/rename (folderId supplied). Folder DELETE has no equivalent
        // anywhere in either old API project (traced FolderController.vb and
        // Datamanager.vb in full) - not implemented here.
        public bool Save_Folder_Data(string folderId, string folderName, string createdByAgencyId)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("content.tbl_Content_FolderInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            if (!string.IsNullOrEmpty(folderId))
            {
                cmd.Parameters.AddWithValue("@GlobalContentFolderID", folderId);
            }
            cmd.Parameters.AddWithValue("@GlobalContentFolderName", folderName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedbyAgencyID", createdByAgencyId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        // Bulk/checkbox "Delete Content". Ground truth: JS L2841-2894
        // ($scope.LMS_DELETE_CONTENT_DATA) posts a single @ttsam_id param holding
        // the WHOLE selected-id list as a comma-separated string - passed through
        // unsplit here to match that exact old behavior.
        public bool Delete_Content(string ttsam_id)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_delete_upload_session_attachement", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttsam_id", ttsam_id ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        // Content Approval Workflow - shared by single-item and bulk approve/reject.
        // Ground truth: JS L3519-3610 ($scope.UPDATE_STATUS) and L4099-4168
        // ($scope.Approve_All_Content) - both call the identical stored procedure.
        //
        // Fixed 2026-07-17 (reported live: SqlException "Procedure or function
        // 'proc_dms_Ins_upd_doc_status' expects parameter '@createdon', which
        // was not supplied"). The old JS's $http call to the legacy generic
        // dynamic-SQL dispatcher (/TrainingApi/Save_Data) passes a SEPARATE
        // top-level `CreatedOnParameter: "@createdon"` field alongside the
        // explicit `Parameters` list - that old dispatcher uses this field to
        // auto-inject a server-side current-timestamp value for whatever
        // param name it names, entirely outside the JS-visible Parameters
        // array. Because it's not a literal `{'@x':'y'}` entry in that array,
        // the original JS trace (L3519-3610) never surfaced it as one of the
        // proc's explicit parameters - a real proc parameter that the old
        // app's generic plumbing supplies invisibly, not one the JS itself
        // ever sets a value for. Added `@createdon` here, using DateTime.Now
        // to match the same "server supplies current timestamp" semantics as
        // the old dispatcher's CreatedOnParameter mechanism (and consistent
        // with Save_Folder_Data's `@CreatedOn` = DateTime.Now for the same
        // kind of auto-timestamp param elsewhere in this backend).
        //
        // Corrected again 2026-07-17, per explicit user review of the actual
        // exec call this method produced (values captured live, not from the
        // old JS trace): the old JS literally sends `@doc_no`/`@docdate` as
        // the string "NULL" and `@CreatedBy_empid`/`@fwd_empid` as HF_EMPID -
        // but the user, working from the real stored procedure/data rather
        // than that static trace, corrected all four:
        //   - `@doc_no` must mirror `@doc_id` (was DBNull).
        //   - `@CreatedBy_empid`/`@fwd_empid` must mirror `@createdby`, not a
        //     separate employee id (was empty string - `request.CreatedByEmpId`/
        //     `FwdEmpId` were resolving blank in this environment regardless).
        //   - `@docdate` must mirror `@actiondate` (was DBNull).
        // Applied verbatim as instructed rather than re-guessed from the JS.
        public bool Approve_Reject_Content(ContentApprovalRequest request)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("dms.proc_dms_Ins_upd_doc_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@doc_no", request.DocId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tttds_info_desc", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@doc_id", request.DocId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createdby", request.CreatedBy ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@branchid", request.BranchId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@docremark", request.DocRemark ?? string.Empty);
            cmd.Parameters.AddWithValue("@CreatedBy_empid", request.CreatedBy ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fwd_empid", request.CreatedBy ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tat_type_id", 119);
            cmd.Parameters.AddWithValue("@doc_status", request.DocStatus);
            cmd.Parameters.AddWithValue("@actiondate", request.ActionDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@docdate", request.ActionDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createdon", DateTime.Now);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            return true;
        }

        // "Add Content To Session" gaps. GAP 1: no dedicated stored proc found for
        // this check (confirmed gap) - raw inline SQL against the master
        // attachment table. GAP 2: INSERT gap, distinct from the already-migrated
        // READ-only Get_Trg_Content/proc_tp_get_upload_session_attachement.
        public bool Check_Content_Attached_In_Session(string trainingid, string sessionid, string globalcontentid)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }

            SqlCommand cmd = new SqlCommand(@"SELECT CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END AS isattached
FROM TrainingPlan.tbl_tp_session_attachment_master
WHERE ttsam_trg_id = @trainingid
  AND ttsam_ttttt_session_id = @sessionid
  AND ttsam_globalcontentid = @globalcontentid", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@trainingid", trainingid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@sessionid", sessionid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@globalcontentid", globalcontentid ?? (object)DBNull.Value);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            bool isattached = false;
            if (dt.Rows.Count > 0)
            {
                isattached = Convert.ToInt32(dt.Rows[0]["isattached"]) == 1;
            }
            return isattached;
        }

        public bool Save_Session_Content_Attachment(SessionContentAttachment sca, string attachementid, string createdon)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }

            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_upload_session_attachement", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@trgid", sca.trgid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@sessionid", sca.sessionid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@title", sca.title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@attachementid", attachementid);
            cmd.Parameters.AddWithValue("@createdby", sca.createdby ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ttsad_tag", sca.ttsad_tag ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@permission", sca.permission ?? "0");
            cmd.Parameters.AddWithValue("@usertype", sca.usertype ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@globalcontentid", sca.globalcontentid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@branchid", sca.branchid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", 0);
            cmd.Parameters.AddWithValue("@GlobalContentyTypeID", sca.GlobalContentyTypeID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GlobalContentFolderID", sca.GlobalContentFolderID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GlobalFilePath", sca.GlobalFilePath ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GlobalFileName", sca.GlobalFileName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GlobalWysiwagText", string.IsNullOrEmpty(sca.contentdata) ? (object)DBNull.Value : sca.contentdata);
            cmd.Parameters.AddWithValue("@CreatedBy_empid", sca.CreatedBy_empid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fwd_empid", sca.fwd_empid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tat_type_id", 119);
            cmd.Parameters.AddWithValue("@procfor", 0);
            cmd.Parameters.AddWithValue("@createdon", createdon);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            cmd.ExecuteNonQuery();
            con.Close();

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
