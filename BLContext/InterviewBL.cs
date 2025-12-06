using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.BLContext
{
    public class InterviewBL
    {
        private readonly IConfiguration _configuration;
        public InterviewBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<interviewQuestion> Get_Questions()
        {

            InterviewDB IDB = new InterviewDB(_configuration);
            List<interviewQuestion> IQ = new List<interviewQuestion>();
            IQ = IDB.Get_Interview_Questions();
            return IQ;
        }
        public interviewQuestion GET_AI_QUESTION(string id)
        {

            InterviewDB IDB = new InterviewDB(_configuration);
            interviewQuestion IQ = new interviewQuestion();
            IQ = IDB.Get_AI_Questions(id);
            return IQ;
        }
    }
}
