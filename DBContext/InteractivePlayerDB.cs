using System.Data;
using System.Text.Json;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;

namespace LitteraCore.DBContext
{
    public class InteractivePlayerDB
    {
        private readonly IConfiguration _configuration;

        public InteractivePlayerDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<InteractivePlayerActivity> GetActivities(string contentId)
        {
            List<InteractivePlayerActivity> activities = new List<InteractivePlayerActivity>();
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_interactive_player_get_activities", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@content_id", contentId ?? string.Empty);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                activities.Add(MapActivity(reader));
            }

            return activities;
        }

        public InteractivePlayerActivity SaveActivity(CreateInteractivePlayerActivityRequest request)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_interactive_player_ins_activity", con);
            string activityId = Guid.NewGuid().ToString();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            AddActivityCommandParameters(cmd, request, activityId);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapActivity(reader);
            }

            throw new Exception("Interactive player activity could not be saved.");
        }

        public InteractivePlayerActivity? UpdateActivity(string activityId, UpdateInteractivePlayerActivityRequest request)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_interactive_player_upd_activity", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            AddActivityCommandParameters(cmd, request, activityId);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapActivity(reader);
            }

            return null;
        }

        public bool DeleteActivity(string activityId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_interactive_player_del_activity", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ip_activity_id", activityId ?? string.Empty);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return SafeInt(reader, "deleted_count") > 0;
            }

            return false;
        }

        public InteractivePlayerOutcome SaveOutcome(CreateInteractivePlayerOutcomeRequest request)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_interactive_player_ins_outcome", con);
            string outcomeId = Guid.NewGuid().ToString();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ip_outcome_id", outcomeId);
            cmd.Parameters.AddWithValue("@activity_id", request.ActivityId ?? string.Empty);
            cmd.Parameters.AddWithValue("@content_id", DbValue(request.ContentId));
            cmd.Parameters.AddWithValue("@activity_type", DbValue(request.ActivityType));
            cmd.Parameters.AddWithValue("@user_id", DbValue(request.UserId));
            cmd.Parameters.AddWithValue("@student_name", DbValue(request.StudentName));
            cmd.Parameters.AddWithValue("@role_name", string.IsNullOrWhiteSpace(request.Role) ? "student" : request.Role);
            cmd.Parameters.AddWithValue("@response_json", DbValue(ToJsonString(request.Response)));
            cmd.Parameters.AddWithValue("@result_json", DbValue(ToJsonString(request.Result)));
            cmd.Parameters.AddWithValue("@started_at", DbValue(request.StartedAt));
            cmd.Parameters.AddWithValue("@completed_at", DbValue(request.CompletedAt));
            cmd.Parameters.AddWithValue("@duration_seconds", DbValue(request.DurationSeconds));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapOutcome(reader);
            }

            throw new Exception("Interactive player outcome could not be saved.");
        }

        public InteractivePlayerQuizSubmission SaveQuizSubmission(CreateInteractivePlayerQuizSubmissionRequest request)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_interactive_player_ins_quiz_submission", con);
            string submissionId = Guid.NewGuid().ToString();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ip_quiz_submission_id", submissionId);
            cmd.Parameters.AddWithValue("@question_id", request.QuestionId ?? string.Empty);
            cmd.Parameters.AddWithValue("@content_id", DbValue(request.ContentId));
            cmd.Parameters.AddWithValue("@activity_type", DbValue(request.ActivityType));
            cmd.Parameters.AddWithValue("@user_id", DbValue(request.UserId));
            cmd.Parameters.AddWithValue("@role_name", string.IsNullOrWhiteSpace(request.Role) ? "student" : request.Role);
            cmd.Parameters.AddWithValue("@student_name", DbValue(request.StudentName));
            cmd.Parameters.AddWithValue("@selected_answer_index", request.SelectedAnswerIndex);
            cmd.Parameters.AddWithValue("@selected_option_id", DbValue(request.SelectedOptionId));
            cmd.Parameters.AddWithValue("@text_answer", DbValue(request.TextAnswer));
            cmd.Parameters.AddWithValue("@is_correct", request.IsCorrect);
            cmd.Parameters.AddWithValue("@started_at", DbValue(request.StartedAt));
            cmd.Parameters.AddWithValue("@answered_at", DbValue(request.AnsweredAt));
            cmd.Parameters.AddWithValue("@teacher_id", DbValue(request.TeacherId));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapQuizSubmission(reader);
            }

            throw new Exception("Interactive player quiz submission could not be saved.");
        }

        private static void AddActivityCommandParameters(
            SqlCommand cmd,
            CreateInteractivePlayerActivityRequest request,
            string activityId)
        {
            cmd.Parameters.AddWithValue("@ip_activity_id", activityId);
            cmd.Parameters.AddWithValue("@content_id", request.ContentId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@activity_type", NormalizeActivityType(request.Type));
            cmd.Parameters.AddWithValue("@trigger_time", request.TriggerTime);
            cmd.Parameters.AddWithValue("@trigger_mode", NormalizeTriggerMode(request.TriggerMode));
            cmd.Parameters.AddWithValue("@pause_on_trigger", request.PauseOnTrigger);
            cmd.Parameters.AddWithValue("@skippable", request.Skippable);
            cmd.Parameters.AddWithValue("@created_by", DbValue(request.CreatedBy));
            cmd.Parameters.AddWithValue("@question", DbValue(request.Question));
            cmd.Parameters.AddWithValue("@title", DbValue(request.Title));
            cmd.Parameters.AddWithValue("@description", DbValue(request.Description));
            cmd.Parameters.AddWithValue("@allow_voice_record", DbValue(request.AllowVoiceRecord));
            cmd.Parameters.AddWithValue("@voice_max_seconds", DbValue(request.VoiceMaxSeconds));
            cmd.Parameters.AddWithValue("@allow_video_record", DbValue(request.AllowVideoRecord));
            cmd.Parameters.AddWithValue("@video_max_seconds", DbValue(request.VideoMaxSeconds));
            cmd.Parameters.AddWithValue("@video_max_size_mb", DbValue(request.VideoMaxSizeMb));
            cmd.Parameters.AddWithValue("@allow_file_upload", DbValue(request.AllowFileUpload));
            cmd.Parameters.AddWithValue("@upload_allowed_types", DbValue(ToJsonString(request.UploadAllowedTypes)));
            cmd.Parameters.AddWithValue("@upload_max_size_mb", DbValue(request.UploadMaxSizeMb));
            cmd.Parameters.AddWithValue("@prompt", DbValue(request.Prompt));
            cmd.Parameters.AddWithValue("@collect_student_name", DbValue(request.CollectStudentName));
            cmd.Parameters.AddWithValue("@instruction", DbValue(request.Instruction));
            cmd.Parameters.AddWithValue("@question_format", DbValue(request.QuestionFormat));
            cmd.Parameters.AddWithValue("@question_media_url", DbValue(request.QuestionMediaUrl));
            cmd.Parameters.AddWithValue("@question_caption", DbValue(request.QuestionCaption));
            cmd.Parameters.AddWithValue("@fill_blank_text", DbValue(request.FillBlankText));
            cmd.Parameters.AddWithValue("@options_json", DbValue(ToJsonString(request.Options)));
            cmd.Parameters.AddWithValue("@correct_answer_index", DbValue(request.CorrectAnswerIndex));
            cmd.Parameters.AddWithValue("@correct_option_id", DbValue(request.CorrectOptionId));
            cmd.Parameters.AddWithValue("@display_mode", DbValue(request.DisplayMode));
            cmd.Parameters.AddWithValue("@images_json", DbValue(ToJsonString(request.Images)));
            cmd.Parameters.AddWithValue("@image_display_seconds", DbValue(request.ImageDisplaySeconds));
            cmd.Parameters.AddWithValue("@question_repeat_count", DbValue(request.QuestionRepeatCount));
            cmd.Parameters.AddWithValue("@turn_duration_seconds", DbValue(request.TurnDurationSeconds));
            cmd.Parameters.AddWithValue("@group_names_json", DbValue(ToJsonString(request.GroupNames)));
            cmd.Parameters.AddWithValue("@winner_rule", DbValue(request.WinnerRule));
            cmd.Parameters.AddWithValue("@tie_breaker", DbValue(request.TieBreaker));
            cmd.Parameters.AddWithValue("@difficulty", DbValue(request.Difficulty));
        }

        private static void AddActivityCommandParameters(
            SqlCommand cmd,
            UpdateInteractivePlayerActivityRequest request,
            string activityId)
        {
            cmd.Parameters.AddWithValue("@ip_activity_id", activityId);
            cmd.Parameters.AddWithValue("@trigger_time", request.TriggerTime);
            cmd.Parameters.AddWithValue("@trigger_mode", NormalizeTriggerMode(request.TriggerMode));
            cmd.Parameters.AddWithValue("@pause_on_trigger", request.PauseOnTrigger);
            cmd.Parameters.AddWithValue("@skippable", request.Skippable);
            cmd.Parameters.AddWithValue("@question", DbValue(request.Question));
            cmd.Parameters.AddWithValue("@title", DbValue(request.Title));
            cmd.Parameters.AddWithValue("@description", DbValue(request.Description));
            cmd.Parameters.AddWithValue("@allow_voice_record", DbValue(request.AllowVoiceRecord));
            cmd.Parameters.AddWithValue("@voice_max_seconds", DbValue(request.VoiceMaxSeconds));
            cmd.Parameters.AddWithValue("@allow_video_record", DbValue(request.AllowVideoRecord));
            cmd.Parameters.AddWithValue("@video_max_seconds", DbValue(request.VideoMaxSeconds));
            cmd.Parameters.AddWithValue("@video_max_size_mb", DbValue(request.VideoMaxSizeMb));
            cmd.Parameters.AddWithValue("@allow_file_upload", DbValue(request.AllowFileUpload));
            cmd.Parameters.AddWithValue("@upload_allowed_types", DbValue(ToJsonString(request.UploadAllowedTypes)));
            cmd.Parameters.AddWithValue("@upload_max_size_mb", DbValue(request.UploadMaxSizeMb));
            cmd.Parameters.AddWithValue("@prompt", DbValue(request.Prompt));
            cmd.Parameters.AddWithValue("@collect_student_name", DbValue(request.CollectStudentName));
            cmd.Parameters.AddWithValue("@instruction", DbValue(request.Instruction));
            cmd.Parameters.AddWithValue("@question_format", DbValue(request.QuestionFormat));
            cmd.Parameters.AddWithValue("@question_media_url", DbValue(request.QuestionMediaUrl));
            cmd.Parameters.AddWithValue("@question_caption", DbValue(request.QuestionCaption));
            cmd.Parameters.AddWithValue("@fill_blank_text", DbValue(request.FillBlankText));
            cmd.Parameters.AddWithValue("@options_json", DbValue(ToJsonString(request.Options)));
            cmd.Parameters.AddWithValue("@correct_answer_index", DbValue(request.CorrectAnswerIndex));
            cmd.Parameters.AddWithValue("@correct_option_id", DbValue(request.CorrectOptionId));
            cmd.Parameters.AddWithValue("@display_mode", DbValue(request.DisplayMode));
            cmd.Parameters.AddWithValue("@images_json", DbValue(ToJsonString(request.Images)));
            cmd.Parameters.AddWithValue("@image_display_seconds", DbValue(request.ImageDisplaySeconds));
            cmd.Parameters.AddWithValue("@question_repeat_count", DbValue(request.QuestionRepeatCount));
            cmd.Parameters.AddWithValue("@turn_duration_seconds", DbValue(request.TurnDurationSeconds));
            cmd.Parameters.AddWithValue("@group_names_json", DbValue(ToJsonString(request.GroupNames)));
            cmd.Parameters.AddWithValue("@winner_rule", DbValue(request.WinnerRule));
            cmd.Parameters.AddWithValue("@tie_breaker", DbValue(request.TieBreaker));
            cmd.Parameters.AddWithValue("@difficulty", DbValue(request.Difficulty));
        }

        private static InteractivePlayerActivity MapActivity(SqlDataReader reader)
        {
            return new InteractivePlayerActivity
            {
                Id = SafeString(reader, "ip_activity_id"),
                ContentId = SafeString(reader, "content_id"),
                Type = SafeString(reader, "activity_type"),
                TriggerTime = SafeInt(reader, "trigger_time"),
                TriggerMode = SafeString(reader, "trigger_mode", "time"),
                PauseOnTrigger = SafeBool(reader, "pause_on_trigger"),
                Skippable = SafeBool(reader, "skippable"),
                CreatedBy = SafeNullableString(reader, "created_by"),
                CreatedAt = SafeDateTimeOffset(reader, "created_at"),
                UpdatedAt = SafeNullableDateTimeOffset(reader, "updated_at"),
                Question = SafeNullableString(reader, "question"),
                Title = SafeNullableString(reader, "title"),
                Description = SafeNullableString(reader, "description"),
                AllowVoiceRecord = SafeNullableBool(reader, "allow_voice_record"),
                VoiceMaxSeconds = SafeNullableInt(reader, "voice_max_seconds"),
                AllowVideoRecord = SafeNullableBool(reader, "allow_video_record"),
                VideoMaxSeconds = SafeNullableInt(reader, "video_max_seconds"),
                VideoMaxSizeMb = SafeNullableInt(reader, "video_max_size_mb"),
                AllowFileUpload = SafeNullableBool(reader, "allow_file_upload"),
                UploadAllowedTypes = ParseStringList(SafeNullableString(reader, "upload_allowed_types")),
                UploadMaxSizeMb = SafeNullableInt(reader, "upload_max_size_mb"),
                Prompt = SafeNullableString(reader, "prompt"),
                CollectStudentName = SafeNullableBool(reader, "collect_student_name"),
                Instruction = SafeNullableString(reader, "instruction"),
                QuestionFormat = SafeNullableString(reader, "question_format"),
                QuestionMediaUrl = SafeNullableString(reader, "question_media_url"),
                QuestionCaption = SafeNullableString(reader, "question_caption"),
                FillBlankText = SafeNullableString(reader, "fill_blank_text"),
                Options = ParseJsonElement(SafeNullableString(reader, "options_json")),
                CorrectAnswerIndex = SafeNullableInt(reader, "correct_answer_index"),
                CorrectOptionId = SafeNullableString(reader, "correct_option_id"),
                DisplayMode = SafeNullableString(reader, "display_mode"),
                Images = ParseStringList(SafeNullableString(reader, "images_json")),
                ImageDisplaySeconds = SafeNullableInt(reader, "image_display_seconds"),
                QuestionRepeatCount = SafeNullableInt(reader, "question_repeat_count"),
                TurnDurationSeconds = SafeNullableInt(reader, "turn_duration_seconds"),
                GroupNames = ParseStringList(SafeNullableString(reader, "group_names_json")),
                WinnerRule = SafeNullableString(reader, "winner_rule"),
                TieBreaker = SafeNullableString(reader, "tie_breaker"),
                Difficulty = SafeNullableString(reader, "difficulty")
            };
        }

        private static InteractivePlayerOutcome MapOutcome(SqlDataReader reader)
        {
            return new InteractivePlayerOutcome
            {
                Id = SafeString(reader, "ip_outcome_id"),
                ActivityId = SafeString(reader, "activity_id"),
                ContentId = SafeNullableString(reader, "content_id"),
                ActivityType = SafeNullableString(reader, "activity_type"),
                UserId = SafeNullableString(reader, "user_id"),
                StudentName = SafeNullableString(reader, "student_name"),
                Role = SafeString(reader, "role_name", "student"),
                Response = ParseJsonElement(SafeNullableString(reader, "response_json")),
                Result = ParseJsonElement(SafeNullableString(reader, "result_json")),
                StartedAt = SafeString(reader, "started_at"),
                CompletedAt = SafeString(reader, "completed_at"),
                DurationSeconds = SafeNullableInt(reader, "duration_seconds"),
                CreatedAt = SafeDateTimeOffset(reader, "created_at")
            };
        }

        private static InteractivePlayerQuizSubmission MapQuizSubmission(SqlDataReader reader)
        {
            return new InteractivePlayerQuizSubmission
            {
                Id = SafeString(reader, "ip_quiz_submission_id"),
                QuestionId = SafeString(reader, "question_id"),
                ContentId = SafeNullableString(reader, "content_id"),
                ActivityType = SafeNullableString(reader, "activity_type"),
                UserId = SafeNullableString(reader, "user_id"),
                Role = SafeString(reader, "role_name", "student"),
                StudentName = SafeString(reader, "student_name"),
                SelectedAnswerIndex = SafeInt(reader, "selected_answer_index"),
                SelectedOptionId = SafeNullableString(reader, "selected_option_id"),
                TextAnswer = SafeNullableString(reader, "text_answer"),
                IsCorrect = SafeBool(reader, "is_correct"),
                StartedAt = SafeNullableString(reader, "started_at"),
                AnsweredAt = SafeString(reader, "answered_at"),
                TeacherId = SafeNullableString(reader, "teacher_id"),
                CreatedAt = SafeDateTimeOffset(reader, "created_at")
            };
        }

        private static object DbValue(object? value)
        {
            return value ?? DBNull.Value;
        }

        private static string NormalizeTriggerMode(string? triggerMode)
        {
            return triggerMode switch
            {
                "start" => "start",
                "end" => "end",
                _ => "time"
            };
        }

        private static string NormalizeActivityType(string? activityType)
        {
            return string.IsNullOrWhiteSpace(activityType)
                ? "quiz"
                : activityType.Trim().ToLowerInvariant();
        }

        private static string? ToJsonString<T>(T? value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is JsonElement jsonElement)
            {
                return jsonElement.GetRawText();
            }

            return JsonSerializer.Serialize(value);
        }

        private static JsonElement? ParseJsonElement(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return JsonDocument.Parse(value).RootElement.Clone();
        }

        private static List<string>? ParseStringList(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return JsonSerializer.Deserialize<List<string>>(value);
        }

        private static bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i += 1)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string SafeString(SqlDataReader reader, string columnName, string fallback = "")
        {
            if (!HasColumn(reader, columnName))
            {
                return fallback;
            }

            object value = reader[columnName];
            return value == DBNull.Value ? fallback : Convert.ToString(value) ?? fallback;
        }

        private static string? SafeNullableString(SqlDataReader reader, string columnName)
        {
            if (!HasColumn(reader, columnName))
            {
                return null;
            }

            object value = reader[columnName];
            return value == DBNull.Value ? null : Convert.ToString(value);
        }

        private static int SafeInt(SqlDataReader reader, string columnName, int fallback = 0)
        {
            if (!HasColumn(reader, columnName))
            {
                return fallback;
            }

            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return fallback;
            }

            return Convert.ToInt32(value);
        }

        private static int? SafeNullableInt(SqlDataReader reader, string columnName)
        {
            if (!HasColumn(reader, columnName))
            {
                return null;
            }

            object value = reader[columnName];
            return value == DBNull.Value ? null : Convert.ToInt32(value);
        }

        private static bool SafeBool(SqlDataReader reader, string columnName, bool fallback = false)
        {
            if (!HasColumn(reader, columnName))
            {
                return fallback;
            }

            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return fallback;
            }

            return Convert.ToBoolean(value);
        }

        private static bool? SafeNullableBool(SqlDataReader reader, string columnName)
        {
            if (!HasColumn(reader, columnName))
            {
                return null;
            }

            object value = reader[columnName];
            return value == DBNull.Value ? null : Convert.ToBoolean(value);
        }

        private static DateTimeOffset SafeDateTimeOffset(SqlDataReader reader, string columnName)
        {
            if (!HasColumn(reader, columnName))
            {
                return DateTimeOffset.UtcNow;
            }

            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return DateTimeOffset.UtcNow;
            }

            if (value is DateTimeOffset dto)
            {
                return dto;
            }

            if (value is DateTime dt)
            {
                return new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc));
            }

            return DateTimeOffset.Parse(Convert.ToString(value) ?? DateTimeOffset.UtcNow.ToString("O"));
        }

        private static DateTimeOffset? SafeNullableDateTimeOffset(SqlDataReader reader, string columnName)
        {
            if (!HasColumn(reader, columnName))
            {
                return null;
            }

            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            if (value is DateTimeOffset dto)
            {
                return dto;
            }

            if (value is DateTime dt)
            {
                return new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc));
            }

            return DateTimeOffset.Parse(Convert.ToString(value) ?? DateTimeOffset.UtcNow.ToString("O"));
        }
    }
}
