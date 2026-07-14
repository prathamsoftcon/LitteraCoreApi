using LitteraCore.Common;
using LitteraCore.Common.EmailService;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace LitteraCore.BLContext
{
    public class ContentBL
    {
        private readonly IConfiguration _configuration;
        public ContentBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<contentType> Get_Content_Type()
        {
            ContentDB CDB = new ContentDB(_configuration);
            List<contentType> lCT = new List<contentType>();
            lCT = CDB.Get_Content_Type();
            return lCT;
        }

        public List<contentType> Get_Global_File_Type()
        {
            ContentDB CDB = new ContentDB(_configuration);
            List<contentType> lCT = new List<contentType>();
            lCT = CDB.Get_Global_File_Type();
            return lCT;
        }

        public List<ContentFolder> Get_Content_Folder_Data(string? folderid = null)
        {
            ContentDB CDB = new ContentDB(_configuration);
            return CDB.Get_Content_Folder_Data(folderid);
        }

        public PagedResult<Content> Get_Trg_Content(string trainingid = null, string sessionid = null,string tags=null, PaginationParam param = null)
        {
            ContentDB CDB = new ContentDB(_configuration);
            List<Content> AL=new List<Content>();
            AL = CDB.Get_Trg_Content(param, trainingid, sessionid);

            if (tags != null)
            {
                AL = AL.Where(o => o.ttsad_tag.ToString().ToUpper().Contains(tags.ToString().ToUpper())).ToList();
            }


            List<contentType> CL = new List<contentType>();
            CL = Get_Content_Type();
            foreach (Content t in AL)
            {
                if (t.GlobalthumbnailPath == null)
                {
                    List<contentType> FCL = CL.Where(o => o.GlobalContentTypeID.ToString().ToUpper() == t.GlobalContentyTypeID.ToString().ToUpper()).ToList();
                    if (FCL.Count > 0)
                    {
                        t.GlobalthumbnailPath = FCL.FirstOrDefault().GlobalContentType;
                    }

                }
            }
            

           var pagedlist= PagedList<Content>.ToPagedList(AL.ToList(),
         param.PageNumber,
         param.PageSize);




            if (pagedlist.Count() > 0)
            {
                var metadata = new
                {
                    pagedlist.TotalCount,
                    pagedlist.PageSize,
                    pagedlist.CurrentPage,
                    pagedlist.TotalPages,
                    pagedlist.HasNext,
                    pagedlist.HasPrevious,


                };
                return (new PagedResult<Content>
                {
                    Items = pagedlist,
                    TotalRecords = pagedlist.TotalCount,
                    PageSize = pagedlist.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagedlist.TotalCount / pagedlist.PageSize),
                    CurrentPage = pagedlist.CurrentPage
                });

                // return Ok(pagedList);
            }
            else
                return new PagedResult<Content> { };

        }

        public bool save_participant_learning_time(learningtime lt)
        {
            ContentDB CDB = new ContentDB(_configuration);


            contentDetail cdn = new contentDetail();
            ContentDB cdb = new ContentDB(_configuration);
            contentDetail cd = new contentDetail();
            cdn = cdb.Get_Content_Detail(lt.tplt_ttsam_id);

            lt.tplt_sessionid = cdn.sessionid;

            bool issaved = CDB.save_participant_learning_time(lt);
            return issaved;
        }
        public bool Save_Activity_Data(activity_data a)
        {
            ContentDB CDB = new ContentDB(_configuration);
            bool issaved = CDB.Save_Activity_Data(a);
            return issaved;

        }
        public List<activity_data> Get_Activity_Data(string agencyid, string activityid = null)
        {
            ContentDB CDB = new ContentDB(_configuration);
            List<activity_data> lCT = new List<activity_data>();
            lCT = CDB.Get_Activity_Data(agencyid, activityid);
            return lCT;
        }
        public bool check_content_learning_exist(string ttsam_id, string participantid)
        {
            ContentDB CDB = new ContentDB(_configuration);
            bool isexist = CDB.check_content_learning_exist(ttsam_id, participantid);
            return isexist;

        }

        public List<Avg_Learning_data_Sessionwise> Avg_Learning_data_sessionwise(string trainingid)
        {
            List<Avg_Learning_data_Sessionwise> s = new List<Avg_Learning_data_Sessionwise>();
            ContentDB CDB = new ContentDB(_configuration);
            List<Avg_Learning_data> lCT = new List<Avg_Learning_data>();
            lCT = CDB.Get_trg_avg_learning_Time(trainingid);
            List<Avg_Learning_data_Sessionwise> sessionWiseList =
       lCT
        .GroupBy(x => new
        {
            x.tplt_trainingid,
            x.tplt_ttsam_id,
            x.tplt_sessionid,
            x.ttttt_subject,
            x.ttttt_content_desc,
            x.content_total_Reading_time

        })
        .Select(g => new Avg_Learning_data_Sessionwise
        {
            tplt_trainingid = g.Key.tplt_trainingid,
            tplt_ttsam_id = g.Key.tplt_ttsam_id,
            tplt_sessionid = g.Key.tplt_sessionid,
            ttttt_subject = g.Key.ttttt_subject,
            ttttt_content_desc = g.Key.ttttt_content_desc,
            session_total_reading_time = g.Sum(x =>  x.content_total_Reading_time),
                           
            avg_learning = g.Sum(x => x.avg_learning * x.content_total_Reading_time)
                           / g.Sum(x => x.content_total_Reading_time)
        })
        .ToList();





            return sessionWiseList;
        }

        public List<contentuserpermission> Get_Content_Permission(string attachmentid, string usertype)
        {
            ContentDB CDB = new ContentDB(_configuration);
            List<contentuserpermission> AL = new List<contentuserpermission>();
            AL = CDB.Get_Content_Permission(attachmentid, usertype);
            return AL;
        }

        // ===================================================================
        // Everything below added 2026-07-12 for the frm_global_content_library.aspx
        // -> React migration.
        // ===================================================================

        public List<string> Get_Content_Tags()
        {
            ContentDB CDB = new ContentDB(_configuration);
            return CDB.Get_Content_Tags();
        }

        // Only "share_content_on_google_drive" is a recognized tag right now (the
        // only Tag value frm_global_content_library.aspx ever sends). Any other
        // tag returns false rather than throwing, since this endpoint may get
        // reused for other tags later without needing a contract change here.
        public bool Get_Global_Setting(string tag)
        {
            ContentDB CDB = new ContentDB(_configuration);
            if (string.Equals(tag, "share_content_on_google_drive", StringComparison.OrdinalIgnoreCase))
            {
                return CDB.Get_Share_Content_On_Google_Drive_Setting();
            }
            return false;
        }

        public PagedResult<GlobalContentListItem> Get_Global_Content_List(string appurl = null, string folderid = null, string searchcolumn = null, string searchvalue = null, string filtervalue = null, int pageno = 1, int pagesize = 8)
        {
            ContentDB CDB = new ContentDB(_configuration);
            List<GlobalContentListItem> AL = new List<GlobalContentListItem>();
            AL = CDB.Get_Global_Content_List(appurl, folderid, searchcolumn, searchvalue, filtervalue, pageno, pagesize);

            int totalRecords = AL.Count > 0 ? AL[0].TotalRecords : 0;
            int totalPages = (pagesize > 0 && totalRecords > 0) ? (int)Math.Ceiling((double)totalRecords / pagesize) : 0;

            return new PagedResult<GlobalContentListItem>
            {
                Items = AL,
                TotalRecords = totalRecords,
                PageSize = pagesize,
                TotalPages = totalPages,
                CurrentPage = pageno
            };
        }

        // "Upload Files" gap - thin passthrough, matching the shape of every
        // other Save_*/Update_* BL method on this class. All the real logic
        // (transaction, DMS insert) lives in ContentDB.Save_Global_Content.
        public bool Save_Global_Content(SaveGlobalContent g)
        {
            ContentDB CDB = new ContentDB(_configuration);
            return CDB.Save_Global_Content(g);
        }

        // Dual-purpose field: for non-CDN content types the SP expects the literal
        // string "NULL"; for CDN-type content (GlobalContentyTypeID ==
        // 6ECEC2CD-2780-4DB5-B03C-CA37D3CC8B29) it instead carries the CDN link text
        // with every backslash doubled. Mirrors $scope.LMS_UPDATE_CONTENT_DATA in
        // JS_frm_global_content_library.js exactly - do NOT collapse these two
        // meanings or let one overwrite the other.
        public bool Update_Global_Content(UpdateGlobalContent m)
        {
            ContentDB CDB = new ContentDB(_configuration);

            string wysiwygText = "NULL";
            if (!string.IsNullOrEmpty(m.GlobalContentTypeID) &&
                m.GlobalContentTypeID.Trim().ToUpper() == "6ECEC2CD-2780-4DB5-B03C-CA37D3CC8B29")
            {
                wysiwygText = (m.CdnLinkText ?? string.Empty).Replace("\\", "\\\\");
            }

            bool issaved = CDB.Update_Global_Content(
                m.GlobalContentTitle,
                m.GlobalContentID,
                m.GlobalContentTag,
                wysiwygText,
                m.GlobalthumbnailPath,
                m.ContentReadingTime);

            return issaved;
        }

        public bool Delete_Content(string ttsam_id)
        {
            ContentDB CDB = new ContentDB(_configuration);
            bool isdeleted = CDB.Delete_Content(ttsam_id);
            return isdeleted;
        }

        public bool Save_Folder_Data(SaveFolder f)
        {
            ContentDB CDB = new ContentDB(_configuration);
            return CDB.Save_Folder_Data(f.GlobalContentFolderID, f.GlobalContentFolderName, f.CreatedByAgencyID);
        }

        // Shared by the single-item Approve/Reject modal and the bulk "Approve All"
        // button - see ContentDB.Approve_Reject_Content for the traced
        // stored-procedure detail (dms.proc_dms_Ins_upd_doc_status).
        public bool Approve_Reject_Content(ContentApprovalRequest request)
        {
            ContentDB CDB = new ContentDB(_configuration);
            return CDB.Approve_Reject_Content(request);
        }

        // Pure encryption helper - reuses the existing YEncryptDecryptData static
        // class instead of the legacy SecureData.GetSecureQueryString call. qsname
        // and qsdata are encrypted SEPARATELY, matching old VB behavior, and
        // returned as a single-element array of {QSNAME, QSVALUE} to match the
        // exact shape already consumed by the old frontend. No DB access required,
        // so the strict Controller->BL->DB chain is intentionally shortened to
        // Controller->BL here (same precedent as AuthenticationController's
        // password encryption calls).
        public List<QSResult> Get_Encrypted_QS(string qsdata, string qsname)
        {
            List<QSResult> result = new List<QSResult>();

            string encryptedName = YEncryptDecryptData.YEncryptDecryptData.Encrypt(qsname, true);
            string encryptedValue = YEncryptDecryptData.YEncryptDecryptData.Encrypt(qsdata, true);

            result.Add(new QSResult
            {
                QSNAME = encryptedName,
                QSVALUE = encryptedValue
            });

            return result;
        }

        // Splits the comma-separated AdditionalID recipient list and sends one
        // email per recipient via the existing SmtpEmailService (no new SMTP
        // code). Domain/IsOnline/TrainingPlanid are accepted for old-call-signature
        // parity but are not used to select a template - no old VB email-template
        // source for this specific mail was found in the traced JS, so a simple new
        // "content shared with you" body is used instead. Returns a single-element
        // list of {status, msg} matching the old contract, where the old frontend
        // does: response.data[0].status.toString().toUpperCase() == "SUCCESS".
        public async Task<List<ShareContentMailResult>> Share_Content_Mail(string domain, string isOnline, string trainingPlanId, string additionalID, string linktxt)
        {
            List<ShareContentMailResult> result = new List<ShareContentMailResult>();

            if (string.IsNullOrWhiteSpace(additionalID))
            {
                result.Add(new ShareContentMailResult
                {
                    status = "FAILED",
                    msg = "No recipient email id provided."
                });
                return result;
            }

            try
            {
                string[] recipients = additionalID.Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                string subject = "Training content shared with you";
                string body = "<p>Content has been shared with you"
                    + (string.IsNullOrWhiteSpace(trainingPlanId) ? "" : " for training " + trainingPlanId)
                    + ".</p><p>Please use the link below to view it:</p>"
                    + "<p><a href=\"" + linktxt + "\">" + linktxt + "</a></p>";

                SmtpEmailService smtp = new SmtpEmailService(_configuration);

                foreach (string recipient in recipients)
                {
                    await smtp.SendEmailAsync(recipient, subject, body);
                }

                result.Add(new ShareContentMailResult
                {
                    status = "SUCCESS",
                    msg = "Mail sent successfully."
                });
            }
            catch (Exception ex)
            {
                result.Add(new ShareContentMailResult
                {
                    status = "FAILED",
                    msg = ex.Message
                });
            }

            return result;
        }

        public bool Check_Content_Attached_In_Session(string trainingid, string sessionid, string globalcontentid)
        {
            ContentDB CDB = new ContentDB(_configuration);
            bool isattached = CDB.Check_Content_Attached_In_Session(trainingid, sessionid, globalcontentid);
            return isattached;
        }

        public bool Save_Session_Content_Attachment(SessionContentAttachment sca)
        {
            ContentDB CDB = new ContentDB(_configuration);

            // Server-generated instead of trusting client-supplied guid/timestamp (old JS did `guid()` + implicit @createdon on the client dispatcher).
            string attachementid = Guid.NewGuid().ToString();
            string createdon = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

            bool issaved = CDB.Save_Session_Content_Attachment(sca, attachementid, createdon);
            return issaved;
        }

        public List<Avg_Learning_data> Avg_Learning_data_contentwise(string trainingid)
        {
            List<Avg_Learning_data_Sessionwise> s = new List<Avg_Learning_data_Sessionwise>();
            ContentDB CDB = new ContentDB(_configuration);
            List<Avg_Learning_data> lCT = new List<Avg_Learning_data>();
            lCT = CDB.Get_trg_avg_learning_Time(trainingid);
           
            return lCT;
        }
    }
}
