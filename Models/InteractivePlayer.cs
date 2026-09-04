using System.Text.Json;

namespace LitteraCore.Models
{
    public class CreateInteractivePlayerActivityRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string Type { get; set; } = "POP";
        public string? Title { get; set; }
        public string? Instruction { get; set; }
        public string? BodyText { get; set; }
        public string? TriggerMode { get; set; } = "START";
        public int? TriggerTime { get; set; }
        public int? PageNumber { get; set; }
        public bool PauseOnTrigger { get; set; } = true;
        public bool Skippable { get; set; } = false;
        public int? DisplayOrder { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? BranchId { get; set; }
        public int? IsActive { get; set; }
        public JsonElement? Details { get; set; }
        public string? DetailJson { get; set; }
        public int SchemaVersion { get; set; } = 1;
    }

    public class UpdateInteractivePlayerActivityRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string Type { get; set; } = "POP";
        public string? Title { get; set; }
        public string? Instruction { get; set; }
        public string? BodyText { get; set; }
        public string? TriggerMode { get; set; } = "START";
        public int? TriggerTime { get; set; }
        public int? PageNumber { get; set; }
        public bool PauseOnTrigger { get; set; } = true;
        public bool Skippable { get; set; } = false;
        public int? DisplayOrder { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? BranchId { get; set; }
        public int? IsActive { get; set; }
        public JsonElement? Details { get; set; }
        public string? DetailJson { get; set; }
        public int SchemaVersion { get; set; } = 1;
    }

    public class InteractivePlayerActivity
    {
        public string Id { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string Type { get; set; } = "POP";
        public string? Title { get; set; }
        public string? Instruction { get; set; }
        public string? BodyText { get; set; }
        public string TriggerMode { get; set; } = "START";
        public int? TriggerTime { get; set; }
        public int? PageNumber { get; set; }
        public bool PauseOnTrigger { get; set; } = true;
        public bool Skippable { get; set; } = false;
        public int? DisplayOrder { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? BranchId { get; set; }
        public int IsActive { get; set; } = 1;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? DetailJson { get; set; }
        public int SchemaVersion { get; set; } = 1;
    }

    public class CreateInteractivePlayerOutcomeRequest
    {
        public string ActivityId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? StudentName { get; set; }
        public string Role { get; set; } = "student";
        public JsonElement? Response { get; set; }
        public JsonElement? Result { get; set; }
        public string StartedAt { get; set; } = string.Empty;
        public string CompletedAt { get; set; } = string.Empty;
        public int? DurationSeconds { get; set; }
    }

    public class InteractivePlayerOutcome
    {
        public string Id { get; set; } = string.Empty;
        public string ActivityId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? StudentName { get; set; }
        public string Role { get; set; } = "student";
        public JsonElement? Response { get; set; }
        public JsonElement? Result { get; set; }
        public string StartedAt { get; set; } = string.Empty;
        public string CompletedAt { get; set; } = string.Empty;
        public int? DurationSeconds { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateInteractivePlayerQuizSubmissionRequest
    {
        public string QuestionId { get; set; } = string.Empty;
        public string? ContentId { get; set; }
        public string? ActivityType { get; set; }
        public string? UserId { get; set; }
        public string Role { get; set; } = "student";
        public string StudentName { get; set; } = string.Empty;
        public int SelectedAnswerIndex { get; set; }
        public string? SelectedOptionId { get; set; }
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public string? StartedAt { get; set; }
        public string AnsweredAt { get; set; } = string.Empty;
        public string? TeacherId { get; set; }
    }

    public class InteractivePlayerQuizSubmission
    {
        public string Id { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string? ContentId { get; set; }
        public string? ActivityType { get; set; }
        public string? UserId { get; set; }
        public string Role { get; set; } = "student";
        public string StudentName { get; set; } = string.Empty;
        public int SelectedAnswerIndex { get; set; }
        public string? SelectedOptionId { get; set; }
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public string? StartedAt { get; set; }
        public string AnsweredAt { get; set; } = string.Empty;
        public string? TeacherId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateInteractivePlayerActivityResponseRequest
    {
        public string ActivityId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public JsonElement? ResponseJson { get; set; }
        public JsonElement? ResultJson { get; set; }
        public string? StartedAt { get; set; }
        public string? CompletedAt { get; set; }
    }

    public class InteractivePlayerActivityResponse
    {
        public string ResponseId { get; set; } = string.Empty;
        public string ActivityId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
        public bool IsResubmission { get; set; }
        public string Status { get; set; } = string.Empty;
        public JsonElement? ResponseJson { get; set; }
        public JsonElement? ResultJson { get; set; }
        public string? StartedAt { get; set; }
        public string? CompletedAt { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }

    public class InteractivePlayerLatestActivityResponse
    {
        public bool HasPriorSubmission { get; set; }
        public int LastAttemptNumber { get; set; }
        public string? LastSubmittedAt { get; set; }
        public string LastStatus { get; set; } = string.Empty;
        public JsonElement? LatestResponseJson { get; set; }
    }

    public class InteractivePlayerPollSummaryItem
    {
        public string SelectedOptionId { get; set; } = string.Empty;
        public string SelectedOptionText { get; set; } = string.Empty;
        public int ResponseCount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class InteractivePlayerPollSummary
    {
        public string ActivityId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public int TotalResponses { get; set; }
        public List<InteractivePlayerPollSummaryItem> Items { get; set; } = new();
    }

    public class SaveInteractivePlayerResumeRequest
    {
        public string TrainingId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string ContentKind { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public decimal? MediaPositionSeconds { get; set; }
        public int? PageNumber { get; set; }
        public string? ActiveActivityId { get; set; }
    }

    public class InteractivePlayerResumeCheckpoint
    {
        public string TrainingId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string ContentKind { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public decimal? MediaPositionSeconds { get; set; }
        public int? PageNumber { get; set; }
        public string? ActiveActivityId { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

}
