using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.BLContext
{
    public class AIBL
    {
        private readonly IConfiguration _configuration;
        public AIBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Save_AI_Conversation(AITool ai)
        {
            AIDB cdb = new AIDB(_configuration);
            bool isSaved = cdb.Save_AI_RESPONSE(ai);
            return isSaved;
        }
        public bool Update_Conversation_Like(AITool ai)
        {
            AIDB cdb = new AIDB(_configuration);
            bool isSaved = cdb.Update_Like_Dislike(ai);
            return isSaved;
        }

        public PagedList<AITool> Get_AI_Tool_Conversations(PaginationParam param)
        {
            AIDB cdb = new AIDB(_configuration);
            PagedList<AITool> leveldata = cdb.Get_AI_Conversation(param);
            return leveldata;
        }
    }
}
