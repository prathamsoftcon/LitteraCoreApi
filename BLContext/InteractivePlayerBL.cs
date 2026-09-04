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

        public List<InteractivePlayerActivity> GetActivities(string contentId, string? sessionId = null)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.GetActivities(contentId, sessionId);
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

        public InteractivePlayerActivityResponse SaveActivityResponse(CreateInteractivePlayerActivityResponseRequest request)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.SaveActivityResponse(request);
        }

        public InteractivePlayerLatestActivityResponse GetLatestActivityResponse(string activityId, string sessionId, string contentId, string userId)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.GetLatestActivityResponse(activityId, sessionId, contentId, userId);
        }

        public InteractivePlayerPollSummary GetPollSummary(string activityId, string sessionId, string contentId)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.GetPollSummary(activityId, sessionId, contentId);
        }

        public InteractivePlayerResumeCheckpoint? GetResumeCheckpoint(
            string trainingId,
            string sessionId,
            string userId,
            string userType,
            string branchId)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.GetResumeCheckpoint(trainingId, sessionId, userId, userType, branchId);
        }

        public InteractivePlayerResumeCheckpoint SaveResumeCheckpoint(SaveInteractivePlayerResumeRequest request)
        {
            InteractivePlayerDB db = new InteractivePlayerDB(_configuration);
            return db.SaveResumeCheckpoint(request);
        }
    }
}
