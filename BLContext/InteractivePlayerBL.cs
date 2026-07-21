using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.BLContext
{
    public class InteractivePlayerBL
    {
        private readonly IConfiguration _configuration;

        public InteractivePlayerBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<InteractivePlayerActivity> GetActivities(string contentId)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.GetActivities(contentId);
        }

        public InteractivePlayerActivity SaveActivity(CreateInteractivePlayerActivityRequest request)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.SaveActivity(request);
        }

        public InteractivePlayerActivity? UpdateActivity(string activityId, UpdateInteractivePlayerActivityRequest request)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.UpdateActivity(activityId, request);
        }

        public bool DeleteActivity(string activityId)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.DeleteActivity(activityId);
        }

        public InteractivePlayerOutcome SaveOutcome(CreateInteractivePlayerOutcomeRequest request)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.SaveOutcome(request);
        }

        public InteractivePlayerQuizSubmission SaveQuizSubmission(CreateInteractivePlayerQuizSubmissionRequest request)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.SaveQuizSubmission(request);
        }
    }
}
