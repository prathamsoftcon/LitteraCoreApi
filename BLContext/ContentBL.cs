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
