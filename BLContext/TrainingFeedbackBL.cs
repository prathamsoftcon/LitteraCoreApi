using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.BLContext
{
    public class TrainingFeedbackBL
    {
        private readonly IConfiguration _configuration;
        public TrainingFeedbackBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<TrainingFeedback> Get_Share_Feedback(string participantid)
        {
            List<TrainingFeedback> TF = new List<TrainingFeedback>();
            FeedbackDB TBD = new FeedbackDB(_configuration);
            TF = TBD.Get_Share_Feedback(participantid);
            return TF;
        }
        public int Feedback_Participant_Status(string participantid, string sharefeedbackid)
        {
            List<TrainingFeedback> LF = new List<TrainingFeedback>();
            FeedbackDB TBD = new FeedbackDB(_configuration);
            LF = TBD.Get_Participant_Feedback_Status(participantid, sharefeedbackid);
            if (LF.Count > 0)
            {
                if (LF.FirstOrDefault().tssr_responsdant_mobile.ToString() != "")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
    }
}
