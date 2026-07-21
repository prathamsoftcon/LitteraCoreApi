using System.Text.Json;

namespace LitteraCore.Models
{
    public class CreateInteractivePlayerActivityRequest
    {
        public string ContentId { get; set; } = string.Empty;
        public string Type { get; set; } = "quiz";
        public int TriggerTime { get; set; }
        public string? TriggerMode { get; set; }
        public bool PauseOnTrigger { get; set; } = true;
        public bool Skippable { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? Question { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? AllowVoiceRecord { get; set; }
        public int? VoiceMaxSeconds { get; set; }
        public bool? AllowVideoRecord { get; set; }
        public int? VideoMaxSeconds { get; set; }
        public int? VideoMaxSizeMb { get; set; }
        public bool? AllowFileUpload { get; set; }
        public List<string>? UploadAllowedTypes { get; set; }
        public int? UploadMaxSizeMb { get; set; }
        public string? Prompt { get; set; }
        public bool? CollectStudentName { get; set; }
        public string? Instruction { get; set; }
        public string? QuestionFormat { get; set; }
        public string? QuestionMediaUrl { get; set; }
        public string? QuestionCaption { get; set; }
        public string? FillBlankText { get; set; }
        public JsonElement? Options { get; set; }
        public int? CorrectAnswerIndex { get; set; }
        public string? CorrectOptionId { get; set; }
        public string? DisplayMode { get; set; }
        public List<string>? Images { get; set; }
        public int? ImageDisplaySeconds { get; set; }
        public int? QuestionRepeatCount { get; set; }
        public int? TurnDurationSeconds { get; set; }
        public List<string>? GroupNames { get; set; }
        public string? WinnerRule { get; set; }
        public string? TieBreaker { get; set; }
        public string? Difficulty { get; set; }
    }

    public class UpdateInteractivePlayerActivityRequest
    {
        public int TriggerTime { get; set; }
        public string? TriggerMode { get; set; }
        public bool PauseOnTrigger { get; set; } = true;
        public bool Skippable { get; set; } = true;
        public string? Question { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? AllowVoiceRecord { get; set; }
        public int? VoiceMaxSeconds { get; set; }
        public bool? AllowVideoRecord { get; set; }
        public int? VideoMaxSeconds { get; set; }
        public int? VideoMaxSizeMb { get; set; }
        public bool? AllowFileUpload { get; set; }
        public List<string>? UploadAllowedTypes { get; set; }
        public int? UploadMaxSizeMb { get; set; }
        public string? Prompt { get; set; }
        public bool? CollectStudentName { get; set; }
        public string? Instruction { get; set; }
        public string? QuestionFormat { get; set; }
        public string? QuestionMediaUrl { get; set; }
        public string? QuestionCaption { get; set; }
        public string? FillBlankText { get; set; }
        public JsonElement? Options { get; set; }
        public int? CorrectAnswerIndex { get; set; }
        public string? CorrectOptionId { get; set; }
        public string? DisplayMode { get; set; }
        public List<string>? Images { get; set; }
        public int? ImageDisplaySeconds { get; set; }
        public int? QuestionRepeatCount { get; set; }
        public int? TurnDurationSeconds { get; set; }
        public List<string>? GroupNames { get; set; }
        public string? WinnerRule { get; set; }
        public string? TieBreaker { get; set; }
        public string? Difficulty { get; set; }
    }

    public class InteractivePlayerActivity
    {
        public string Id { get; set; } = string.Empty;
        public string ContentId { get; set; } = string.Empty;
        public string Type { get; set; } = "quiz";
        public int TriggerTime { get; set; }
        public string TriggerMode { get; set; } = "time";
        public bool PauseOnTrigger { get; set; } = true;
        public bool Skippable { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Question { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? AllowVoiceRecord { get; set; }
        public int? VoiceMaxSeconds { get; set; }
        public bool? AllowVideoRecord { get; set; }
        public int? VideoMaxSeconds { get; set; }
        public int? VideoMaxSizeMb { get; set; }
        public bool? AllowFileUpload { get; set; }
        public List<string>? UploadAllowedTypes { get; set; }
        public int? UploadMaxSizeMb { get; set; }
        public string? Prompt { get; set; }
        public bool? CollectStudentName { get; set; }
        public string? Instruction { get; set; }
        public string? QuestionFormat { get; set; }
        public string? QuestionMediaUrl { get; set; }
        public string? QuestionCaption { get; set; }
        public string? FillBlankText { get; set; }
        public JsonElement? Options { get; set; }
        public int? CorrectAnswerIndex { get; set; }
        public string? CorrectOptionId { get; set; }
        public string? DisplayMode { get; set; }
        public List<string>? Images { get; set; }
        public int? ImageDisplaySeconds { get; set; }
        public int? QuestionRepeatCount { get; set; }
        public int? TurnDurationSeconds { get; set; }
        public List<string>? GroupNames { get; set; }
        public string? WinnerRule { get; set; }
        public string? TieBreaker { get; set; }
        public string? Difficulty { get; set; }
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

}
