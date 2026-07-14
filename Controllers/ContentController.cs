using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Threading.Tasks;

namespace LitteraCore.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public ContentController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/ContentType")]
        [SwaggerOperation("To get different content types[pdf/video/wysiwyg/document].")]
        public IActionResult GetFunction()
        {
            ContentBL CBL = new ContentBL(_configuration);
            List<contentType> AL = new List<contentType>();
            AL = CBL.Get_Content_Type();
            return Ok(AL);
        }

        [HttpGet]
        [Route("api/Get_GLOBAL_FILE_TYPE")]
        [SwaggerOperation("To get different content types[pdf/video/wysiwyg/document].")]
        public IActionResult Get_Global_File_Type()
        {
            ContentBL CBL = new ContentBL(_configuration);
            List<contentType> AL = new List<contentType>();
            AL = CBL.Get_Global_File_Type();
            return Ok(AL);
        }

        [HttpGet]
        [Route("api/Get_Folder_Data")]
        [SwaggerOperation("To get content folders.")]
        public IActionResult Get_Folder_Data(string? folderid = null)
        {
            ContentBL CBL = new ContentBL(_configuration);
            List<ContentFolder> folders = CBL.Get_Content_Folder_Data(folderid);
            return Ok(folders);
        }

        [HttpGet]
        [Route("api/Trg_Content")]
        [SwaggerOperation("To get particular training/session contents.")]
        public IActionResult Trg_Content([FromQuery] PaginationParam filter, string trainingid = null, string sessionid = null, string tags = null)
        {
            
            ContentBL CBL = new ContentBL(_configuration);
            PagedResult<Content> AL = new PagedResult<Content>();
            AL = CBL.Get_Trg_Content(trainingid, sessionid,tags,filter);

        

            return Ok(AL);
        }



        [HttpPost]
        [Route("api/Learning_Time")]
        [SwaggerOperation("To save participant learning time.")]
        public IActionResult Learning_Time([FromBody]learningtime lt)
        {
            ContentBL CBL = new ContentBL(_configuration);
            bool issaved = CBL.save_participant_learning_time(lt);
            return Ok(issaved);
        }
        [Authorize(Policy = "PublicApiKey")]
        [HttpPost]
        [Route("api/Learning_Time_wk")]
        [SwaggerOperation("To save participant learning time.")]
        public IActionResult Learning_Time_wk([FromBody] learningtime lt)
        {
            ContentBL CBL = new ContentBL(_configuration);
            bool issaved = CBL.save_participant_learning_time(lt);
            return Ok(issaved);
        }



        [HttpGet]
        [Route("api/GET_CONTENT_DETAILS")]
        [SwaggerOperation("To get particular content detail with participant status.")]
        public IActionResult GET_CONTENT_DETAILS(string ttsam_id, string participantid)
        {

            string ipaddress = GetClientIp();

            contentDetail cd = new contentDetail();
            ContentDB cdb = new ContentDB(_configuration);
            cd = cdb.Get_ttpai_from_Content(ttsam_id, participantid);
            contentDetail cdn=new contentDetail();
            cdn = cdb.Get_Content_Detail(ttsam_id);
            cd.content_path = cdn.content_path;
            cd.trainingid = cdn.trainingid;
            cd.sessionid = cdn.sessionid;
            //********
            UserDB UBL = new UserDB(_configuration);
            User amob = new User();
            if(cd.mobileno == null)
            {
                throw new Exception("You are not eligible to access this training.");
            }

            amob = UBL.GET_MOBILE_NO_DATA(cd.mobileno, 2);
            if(amob != null)
            {
                cd.userid = amob.userid;
            }
            ContentDB CDB = new ContentDB(_configuration);
            List<Content> AL = new List<Content>();
            PaginationParam param = null;
          
            bool issaved = cdb.INSERT_CONTENT_VISITING(ttsam_id, cd.mobileno, ipaddress);
            // Save Learning Time with 0 entry
            ContentBL CBL = new ContentBL(_configuration);
            learningtime lt=new learningtime {  tplt_Id=Guid.NewGuid().ToString(),
             tplt_learning_time=0,
             tplt_createdon=System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
             tplt_createdby= participantid,
             tplt_ttpai_id=cd.ttpai_id,
             tplt_ttsam_id= ttsam_id

            };


            bool islearningtimesaved = CBL.save_participant_learning_time(lt);

            AL = CDB.Get_Trg_Content(param, cd.trainingid, cd.sessionid);
            AL = AL.Where(o => o.ttsad_ttsam_id.ToString().ToUpper() == ttsam_id.ToString().ToUpper()).ToList();
            //********
            cd.Items = AL.ToArray();




            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_Session_Data_By_Trg(cd.trainingid);
            Session sd = s.Where(o=>o.ttttt_session_id.ToString().ToUpper()==cd.sessionid.ToString().ToUpper()).FirstOrDefault();
            cd.Session = sd;


            //get branchid
            UserBranch ub = new UserBranch();
            AgencyBL abl = new AgencyBL(_configuration);
            ub = abl.Get_User_Branche(cd.userid);
            if (ub.branches.Count() > 0)
            {
                cd.branchid = ub.branches.FirstOrDefault().branchid;
            }

            return Ok(cd);
        }


        [Authorize(Policy = "PublicApiKey")]
        [HttpGet]
        [Route("api/GET_CONTENT_DETAILS_wk")]
        [SwaggerOperation("To get particular content detail with participant status.")]
        public IActionResult GET_CONTENT_DETAILS_wk(string ttsam_id, string participantid)
        {

            string ipaddress = GetClientIp();

            contentDetail cd = new contentDetail();
            ContentDB cdb = new ContentDB(_configuration);
            cd = cdb.Get_ttpai_from_Content(ttsam_id, participantid);
            contentDetail cdn = new contentDetail();
            cdn = cdb.Get_Content_Detail(ttsam_id);
            cd.content_path = cdn.content_path;
            cd.trainingid = cdn.trainingid;
            cd.sessionid = cdn.sessionid;
            //********
            UserDB UBL = new UserDB(_configuration);
            User amob = new User();
            if (cd.mobileno == null)
            {
                throw new Exception("You are not eligible to access this training.");
            }

            amob = UBL.GET_MOBILE_NO_DATA(cd.mobileno, 2);
            if (amob != null)
            {
                cd.userid = amob.userid;
            }
            ContentDB CDB = new ContentDB(_configuration);
            List<Content> AL = new List<Content>();
            PaginationParam param = null;

            bool issaved = cdb.INSERT_CONTENT_VISITING(ttsam_id, cd.mobileno, ipaddress);
            // Save Learning Time with 0 entry
            ContentBL CBL = new ContentBL(_configuration);
            learningtime lt = new learningtime
            {
                tplt_Id = Guid.NewGuid().ToString(),
                tplt_learning_time = 0,
                tplt_createdon = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                tplt_createdby = participantid,
                tplt_ttpai_id = cd.ttpai_id,
                tplt_ttsam_id = ttsam_id

            };


            bool islearningtimesaved = CBL.save_participant_learning_time(lt);

            AL = CDB.Get_Trg_Content(param, cd.trainingid, cd.sessionid);
            AL = AL.Where(o => o.ttsad_ttsam_id.ToString().ToUpper() == ttsam_id.ToString().ToUpper()).ToList();
            //********
            cd.Items = AL.ToArray();




            SessionBL cbl = new SessionBL(_configuration);
            List<Session> s = new List<Session>();
            s = cbl.Get_Session_Data_By_Trg(cd.trainingid);
            Session sd = s.Where(o => o.ttttt_session_id.ToString().ToUpper() == cd.sessionid.ToString().ToUpper()).FirstOrDefault();
            cd.Session = sd;


            //get branchid
            UserBranch ub = new UserBranch();
            AgencyBL abl = new AgencyBL(_configuration);
            ub = abl.Get_User_Branche(cd.userid);
            if (ub.branches.Count() > 0)
            {
                cd.branchid = ub.branches.FirstOrDefault().branchid;
            }

            return Ok(cd);
        }

        //[HttpGet]
        //[Route("api/GlobalContentType")]
        //public IActionResult GlobalContentType()
        //{

        //    ContentBL CBL = new ContentBL(_configuration);
        //    List<contentType> ctype=new List<contentType>();
        //    ctype = CBL.Get_Content_Type();
        //    return Ok(ctype);
        //}
        [HttpGet("GETCLIENTIP")]
        [SwaggerOperation("To get client ip address.")]
        public string GetClientIp()
        {
            string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // If the application is behind a proxy (like a load balancer), you might need to check the X-Forwarded-For header.
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                clientIp = HttpContext.Request.Headers["X-Forwarded-For"];
            }

            return clientIp;
        }


        [HttpPost]
        [Route("api/Activity_Data")]
        [SwaggerOperation("To save activity tracking data.")]
        public IActionResult Activity_Data([FromBody]activity_data a)
        {
            ContentBL CBL = new ContentBL(_configuration);
            bool issaved = CBL.Save_Activity_Data(a);
            return Ok(issaved);
        }
        [HttpGet("Get_Activity_Data")]
        [SwaggerOperation("To get activity data.")]
        public IActionResult Get_Activity_Data(string agencyid, string activityid = null)
        {
            ContentBL CBL = new ContentBL(_configuration);
            List<activity_data> lCT = new List<activity_data>();
            lCT = CBL.Get_Activity_Data(agencyid, activityid);
            // If the application is behind a proxy (like a load balancer), you might need to check the X-Forwarded-For header.
          
            return Ok(lCT);
        }

        [HttpGet]
        [Route("api/check_content_learning_exist")]
        [SwaggerOperation("To check learning exist on particular content for given participant.")]
        public IActionResult check_content_learning_exist(string ttsam_id, string participantid)
        {
            bool isexist=true;
            ContentBL CBL = new ContentBL(_configuration);
            isexist = CBL.check_content_learning_exist(ttsam_id, participantid);


            return Ok(new {learning_exist= isexist });
        }
        [Authorize(Policy = "PublicApiKey")]
        [HttpGet]
        [Route("api/check_content_learning_exist_wk")]
        [SwaggerOperation("To check learning exist on particular content for given participant.")]
        public IActionResult check_content_learning_exist_wk(string ttsam_id, string participantid)
        {
            bool isexist = true;
            ContentBL CBL = new ContentBL(_configuration);
            isexist = CBL.check_content_learning_exist(ttsam_id, participantid);


            return Ok(new { learning_exist = isexist });
        }


        [HttpGet]
        [Route("api/Get_Session_Avg_Learning_Time")]
        [SwaggerOperation("To Get session wise average learning time.")]
        public IActionResult Get_Session_Avg_Learning_Time(string trainingid)
        {
            List<Avg_Learning_data_Sessionwise> s = new List<Avg_Learning_data_Sessionwise>();
            ContentBL CBL = new ContentBL(_configuration);
            s = CBL.Avg_Learning_data_sessionwise(trainingid);
            return Ok(s);
        }
        [HttpGet]
        [Route("api/Get_content_Avg_Learning_Time")]
        [SwaggerOperation("To Get session wise average learning time.")]
        public IActionResult Get_content_Avg_Learning_Time(string trainingid)
        {
            List<Avg_Learning_data> s = new List<Avg_Learning_data>();
            ContentBL CBL = new ContentBL(_configuration);
            s = CBL.Avg_Learning_data_contentwise(trainingid);
            return Ok(s);
        }

        [HttpGet]
        [Route("api/Get_Content_Permission")]
        [SwaggerOperation("To get view/edit/download/delete permission (per user type - admin/CD/faculty/participant) for a content attachment.")]
        public IActionResult Get_Content_Permission(string attachmentid, string usertype)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                List<contentuserpermission> AL = new List<contentuserpermission>();
                AL = CBL.Get_Content_Permission(attachmentid, usertype);
                return Ok(AL);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // ===================================================================
        // Everything below added 2026-07-12 for the frm_global_content_library.aspx
        // -> React migration. See TrainingType_MIGRATION_NOTES-style doc for
        // this page (GlobalContentLibrary_MIGRATION_NOTES.md) for the full trace.
        // ===================================================================

        // Old page called the generic /TrainingApi/Get_Data dispatcher with
        // ProcedureName=TrainingPlan.proc_TP_Get_tag, @columnname=GlobalContentTag,
        // @tblname=Content.tbl_ContentMaster - always these two literal values on
        // this page, so they are hardcoded server-side rather than accepted from
        // the frontend (see ContentDB.Get_Content_Tags).
        [HttpGet]
        [Route("api/Get_Content_Tags")]
        [SwaggerOperation("To get the distinct list of global content tags used across content records.")]
        public IActionResult Get_Content_Tags()
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                List<string> tags = CBL.Get_Content_Tags();
                return Ok(tags);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // Replacement for the old XML-based /TrainingAPI/GET_GLOBAL_SETTING?Tag=...
        // switch. Currently only "share_content_on_google_drive" is supported (the
        // only Tag this page ever requests) - see ContentBL/ContentDB for the
        // established ApplicationConfigDB.Get_Application_Setting(...) idiom used
        // to back this (SettingID "12" is a newly-invented id, needs a real DB row).
        [HttpGet]
        [Route("api/Get_Global_Setting")]
        [SwaggerOperation("To get a global setting flag by tag. Currently only 'share_content_on_google_drive' is supported (used by frm_global_content_library.aspx).")]
        public IActionResult Get_Global_Setting(string domain = null, string isOnline = null, string tag = "share_content_on_google_drive")
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool settingValue = CBL.Get_Global_Setting(tag);
                return Ok(new { tag = tag, share_content_on_google_drive = settingValue });
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // The core paged/searched/filtered content-grid listing for
        // frm_global_content_library.aspx - almost every other feature on this
        // page depends on this response shape.
        // CORRECTED 2026-07-12: re-derived from the REAL old implementation at
        // C:\Projects\TraininingERP_old\Littera_MVC_API (reached in the old app
        // via https://qa.littera.in/LitteraAPI/api/GlobalContent) after the
        // first draft's fabricated proc name produced an always-empty grid -
        // see ContentDB.Get_Global_Content_List for the full trace. Added
        // "appurl" (matches the old call's "APPURL" param) so file/thumbnail
        // paths can be built the same way the old app did - the frontend
        // passes config.LITTERA_CDN_BASE_URL, since uploaded content is still
        // served from that same legacy static-file location.
        [HttpGet]
        [Route("api/GlobalContent")]
        [SwaggerOperation("To get paged/searched/filtered global content library listing (frm_global_content_library).")]
        public IActionResult GlobalContent(string appurl = null, string folderid = null, string searchcolumn = null, string searchvalue = null, string filtervalue = null, int pageno = 1, int pagesize = 8)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                PagedResult<GlobalContentListItem> AL = CBL.Get_Global_Content_List(appurl, folderid, searchcolumn, searchvalue, filtervalue, pageno, pagesize);
                return Ok(AL);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // "Upload Files" gap for frm_global_content_library.aspx - creates a new
        // global content record after the file itself has already been uploaded
        // via the existing Diet-wide Upload/UploadFile convention (see
        // upload-handling-map.md) - this endpoint only persists metadata, it
        // never receives raw file bytes. Ground truth: old
        // uc_upload_global_content.ascx's LMS_UPLOAD_DATA() ->
        // LitteraAPI/api/Save_Global_Content -> ContentBL.Save_Global_Content ->
        // ContentDB.Save_Global_Content/INS_GLOBAL_CONTENT (content.sp_insert_tbl_ContentMaster_v1)
        // + a DMS row (tat_type_id = Global_Content_Id) - see ContentDB.Save_Global_Content
        // for the full trace and the reused Save_DMS_DATA helper.
        [HttpPost]
        [Route("api/Save_Global_Content")]
        [SwaggerOperation("To create a new global content library item (upload metadata save - the file itself is uploaded separately via the shared Upload/UploadFile endpoint).")]
        public IActionResult Save_Global_Content([FromBody] SaveGlobalContent g)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool issaved = CBL.Save_Global_Content(g);
                return Ok(issaved);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // "Edit Content" save - title/tags/reading-time (and CDN link field for
        // CDN-type content) for a single non-WYSIWYG content item. Ground truth:
        // JS_frm_global_content_library.js L3122-3210 ($scope.LMS_UPDATE_CONTENT_DATA).
        [HttpPost]
        [Route("api/Update_Global_Content")]
        [SwaggerOperation("To update global content library data (title/tag/wysiwyg-or-cdn-link/thumbnail/reading time).")]
        public IActionResult Update_Global_Content([FromBody] UpdateGlobalContent m)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool issaved = CBL.Update_Global_Content(m);
                return Ok(issaved);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // Bulk/checkbox "Delete Content". Old $scope.LMS_DELETE_CONTENT_DATA (JS
        // L2841-2894) posted the WHOLE selected-id list as one comma-separated
        // @ttsam_id value in a single call - preserved as-is here.
        [HttpPost]
        [Route("api/Delete_Content_Data")]
        [SwaggerOperation("To delete one or more content/attachments in bulk via a comma-separated ttsam_id list.")]
        public IActionResult Delete_Content_Data(string ttsam_id)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool isdeleted = CBL.Delete_Content(ttsam_id);
                return Ok(isdeleted);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // Content Approval Workflow - SHARED by the single-item Approve/Reject
        // modal ($scope.UPDATE_STATUS, JS L3519-3610) and the bulk "Approve All"
        // button ($scope.Approve_All_Content, JS L4099-4168). Both old callers hit
        // the identical stored procedure (dms.proc_dms_Ins_upd_doc_status) with the
        // identical 11 params, so this is intentionally one shared endpoint.
        [HttpPost]
        [Route("api/Content_Approve_Reject")]
        [SwaggerOperation("To approve or reject content (single item or bulk comma-separated ids).")]
        public IActionResult Content_Approve_Reject([FromBody] ContentApprovalRequest request)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool isupdated = CBL.Approve_Reject_Content(request);
                return Ok(isupdated);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // Shared by the WYSIWYG-editor handoff, WhatsApp-share, and email-share
        // flows on frm_global_content_library.aspx (all three called the same old
        // VB function). Reuses the existing YEncryptDecryptData helper instead of
        // the legacy SecureData.GetSecureQueryString call. IMPORTANT: whatever
        // decrypts this on the receiving end (frm_global_wysiwyg.aspx's eventual
        // React replacement) must use the matching YEncryptDecryptData.Decrypt
        // call with the same bool flag to successfully decrypt.
        [HttpGet]
        [Route("api/RCVP_TrainingSchedule_Get_Encrypted_QS")]
        [SwaggerOperation("To encrypt a query-string name/value pair for safe URL transport (shared by the WYSIWYG editor handoff, WhatsApp share, and email share flows on frm_global_content_library.aspx).")]
        public IActionResult RCVP_TrainingSchedule_Get_Encrypted_QS(string qsdata, string qsname)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                List<QSResult> result = CBL.Get_Encrypted_QS(qsdata, qsname);
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // Email-share flow for frm_global_content_library.aspx. Reuses the
        // existing SmtpEmailService (no new SMTP code). Does NOT implement the old
        // external short-URL-shortening step (MP_HF_SHORT_URL_API) - out of scope,
        // shared link will be the full un-shortened URL instead.
        [HttpGet]
        [Route("api/TRG_SHARE_CONTENT_MAIL")]
        [SwaggerOperation("To share training content link via email with one or more recipients.")]
        public async Task<IActionResult> TRG_SHARE_CONTENT_MAIL(string Domain, string IsOnline, string TrainingPlanid, string AdditionalID, string linktxt)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                List<ShareContentMailResult> result = await CBL.Share_Content_Mail(Domain, IsOnline, TrainingPlanid, AdditionalID, linktxt);
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        // "Add Content To Session" gaps - checks whether a global content item is
        // already attached to a training/session, and attaches an existing item to
        // a training/session (INSERT gap, distinct from the already-migrated
        // READ-only Get_Trg_Content/proc_tp_get_upload_session_attachement).
        [HttpGet]
        [Route("api/Check_Content_Attached_In_Session")]
        [SwaggerOperation("To check whether a given global content item is already attached to a specific training/session.")]
        public IActionResult Check_Content_Attached_In_Session(string trainingid, string sessionid, string globalcontentid)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool isattached = CBL.Check_Content_Attached_In_Session(trainingid, sessionid, globalcontentid);
                return Ok(new ContentAttachmentStatus { isattached = isattached });
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpPost]
        [Route("api/Save_Session_Content_Attachment")]
        [SwaggerOperation("To attach an existing global content library item to a specific training/session.")]
        public IActionResult Save_Session_Content_Attachment([FromBody] SessionContentAttachment sca)
        {
            try
            {
                ContentBL CBL = new ContentBL(_configuration);
                bool issaved = CBL.Save_Session_Content_Attachment(sca);
                return Ok(new { issaved = issaved });
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }
    }
}
