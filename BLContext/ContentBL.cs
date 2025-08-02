using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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

    }
}
