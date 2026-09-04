using System.Data;
using System.Text.Json;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;

namespace LitteraCore.DBContext
{
    public class InteractivePlayerDB
    {
        private readonly IConfiguration _configuration;
        private enum ActivityStatus
        {
            Active = 1,
            Inactive = 2,
            Deleted = 9
        }
        private const string ActivitySelectSql = @"
SELECT
    CAST(m.ttiam_activityid AS nvarchar(50)) AS activity_id,
    m.ttiam_sessionid AS session_id,
    m.ttiam_contentid AS content_id,
    UPPER(m.ttiam_activitytype) AS activity_type,
    m.ttiam_title AS title,
    m.ttiam_instruction AS instruction,
    m.ttiam_bodytext AS body_text,
    UPPER(m.ttiam_triggermode) AS trigger_mode,
    CAST(m.ttiam_triggertime AS int) AS trigger_time,
    m.ttiam_pagenumber AS page_number,
    m.ttiam_pauseontrigger AS pause_on_trigger,
    m.ttiam_skippable AS skippable,
    m.ttiam_displayorder AS display_order,
    CAST(m.ttiam_isactive AS int) AS activity_status,
    m.ttiam_createdby AS created_by,
    m.ttiam_updatedby AS updated_by,
    m.ttiam_branchid AS branch_id,
    m.ttiam_createdon AS created_at,
    m.ttiam_updatedon AS updated_at,
    d.ttiad_DetailJson AS detail_json,
    d.ttiad_SchemaVersion AS schema_version
FROM trainingplan.tbl_tp_ip_acvtivity_master m
LEFT JOIN trainingplan.tbl_tp_ip_acvtivity_detail d
    ON d.ttiad_ttiam_ActivityId = m.ttiam_activityid";

        public InteractivePlayerDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<InteractivePlayerActivity> GetActivities(string contentId, string? sessionId = null)
        {
            List<InteractivePlayerActivity> activities = new List<InteractivePlayerActivity>();
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand($@"
{ActivitySelectSql}
WHERE m.ttiam_isactive <> {(int)ActivityStatus.Deleted}
  AND m.ttiam_contentid = @content_id
  AND (@session_id = '' OR m.ttiam_sessionid = @session_id)
ORDER BY
    CASE UPPER(m.ttiam_triggermode)
        WHEN 'START' THEN 0
        WHEN 'CUSTOM' THEN 1
        WHEN 'PAGE' THEN 2
        WHEN 'END' THEN 3
        ELSE 4
    END,
    m.ttiam_triggertime,
    m.ttiam_displayorder,
    m.ttiam_activityid;", con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@content_id", contentId ?? string.Empty);
            cmd.Parameters.AddWithValue("@session_id", sessionId?.Trim() ?? string.Empty);

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlTransaction transaction = con.BeginTransaction();

            try
            {
                long activityId = InsertActivity(con, transaction, request);
                UpsertActivityDetail(con, transaction, activityId, ResolveDetailJson(request.Details, request.DetailJson), request.SchemaVersion);
                InteractivePlayerActivity saved = GetActivityById(con, transaction, activityId)
                    ?? throw new Exception("Interactive player activity could not be loaded after save.");
                transaction.Commit();
                return saved;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public InteractivePlayerActivity? UpdateActivity(string activityId, UpdateInteractivePlayerActivityRequest request)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            if (!TryParseActivityId(activityId, out long parsedActivityId))
            {
                return null;
            }

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlTransaction transaction = con.BeginTransaction();

            try
            {
                bool updated = UpdateActivityRow(con, transaction, parsedActivityId, request);
                if (!updated)
                {
                    transaction.Rollback();
                    return null;
                }

                UpsertActivityDetail(con, transaction, parsedActivityId, ResolveDetailJson(request.Details, request.DetailJson), request.SchemaVersion);
                InteractivePlayerActivity? saved = GetActivityById(con, transaction, parsedActivityId);
                transaction.Commit();
                return saved;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool DeleteActivity(string activityId)
        {
            if (!TryParseActivityId(activityId, out long parsedActivityId))
            {
                return false;
            }

            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand(@"
UPDATE trainingplan.tbl_tp_ip_acvtivity_master
SET
    ttiam_isactive = @deleted_status,
    ttiam_updatedon = SYSUTCDATETIME()
WHERE ttiam_activityid = @activity_id
  AND ttiam_isactive <> @deleted_status;", con);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@activity_id", parsedActivityId);
            cmd.Parameters.AddWithValue("@deleted_status", (int)ActivityStatus.Deleted);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            return cmd.ExecuteNonQuery() > 0;
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

        public InteractivePlayerActivityResponse SaveActivityResponse(CreateInteractivePlayerActivityResponseRequest request)
        {
            if (!TryParseActivityId(request.ActivityId, out long parsedActivityId))
            {
                throw new ArgumentException("activityId must be a valid numeric value.", nameof(request.ActivityId));
            }

            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_ip_ins_activity_response", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@activity_id", parsedActivityId);
            cmd.Parameters.AddWithValue("@session_id", request.SessionId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@content_id", request.ContentId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@activity_type", request.ActivityType?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@user_id", request.UserId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@user_type", request.UserType?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@status", request.Status?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@response_json", DbValue(ToJsonString(request.ResponseJson)));
            cmd.Parameters.AddWithValue("@result_json", DbValue(ToJsonString(request.ResultJson)));
            cmd.Parameters.AddWithValue("@started_at", DbValue(request.StartedAt));
            cmd.Parameters.AddWithValue("@completed_at", DbValue(request.CompletedAt));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapActivityResponse(reader);
            }

            throw new Exception("Interactive player activity response could not be saved.");
        }

        public InteractivePlayerLatestActivityResponse GetLatestActivityResponse(string activityId, string sessionId, string contentId, string userId)
        {
            InteractivePlayerLatestActivityResponse fallbackResponse = new InteractivePlayerLatestActivityResponse
            {
                HasPriorSubmission = false,
                LastAttemptNumber = 0,
                LastSubmittedAt = null,
                LastStatus = string.Empty,
                LatestResponseJson = null,
            };

            if (!TryParseActivityId(activityId, out long parsedActivityId))
            {
                return fallbackResponse;
            }

            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_ip_get_latest_activity_response", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@activity_id", parsedActivityId);
            cmd.Parameters.AddWithValue("@session_id", sessionId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@content_id", contentId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@user_id", userId?.Trim() ?? string.Empty);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return fallbackResponse;
            }

            return MapLatestActivityResponse(reader);
        }

        public InteractivePlayerPollSummary GetPollSummary(string activityId, string sessionId, string contentId)
        {
            InteractivePlayerPollSummary summary = new InteractivePlayerPollSummary
            {
                ActivityId = activityId ?? string.Empty,
                SessionId = sessionId ?? string.Empty,
                ContentId = contentId ?? string.Empty,
                TotalResponses = 0,
                Items = new List<InteractivePlayerPollSummaryItem>(),
            };

            if (!TryParseActivityId(activityId, out long parsedActivityId))
            {
                return summary;
            }

            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand(@"
WITH poll_rows AS (
    SELECT
        ISNULL(NULLIF(JSON_VALUE(r.ttipr_responsejson, '$.selectedOptionId'), ''), '') AS selected_option_id,
        ISNULL(NULLIF(JSON_VALUE(r.ttipr_responsejson, '$.selectedOptionText'), ''), 'Untitled option') AS selected_option_text
    FROM trainingplan.tbl_tp_ip_response r
    WHERE r.ttipr_ttiam_activityid = @activity_id
      AND r.ttipr_sessionid = @session_id
      AND r.ttipr_contentid = @content_id
      AND UPPER(r.ttipr_activitytype) = 'POLL'
      AND UPPER(r.ttipr_status) = 'COMPLETED'
),
poll_counts AS (
    SELECT
        selected_option_id,
        selected_option_text,
        COUNT(1) AS response_count
    FROM poll_rows
    GROUP BY selected_option_id, selected_option_text
)
SELECT
    c.selected_option_id,
    c.selected_option_text,
    c.response_count,
    totals.total_responses
FROM poll_counts c
CROSS JOIN (
    SELECT COUNT(1) AS total_responses
    FROM poll_rows
) totals
ORDER BY c.response_count DESC, c.selected_option_text ASC;", con);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@activity_id", parsedActivityId);
            cmd.Parameters.AddWithValue("@session_id", sessionId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@content_id", contentId?.Trim() ?? string.Empty);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int totalResponses = SafeInt(reader, "total_responses", 0);
                int responseCount = SafeInt(reader, "response_count", 0);

                summary.TotalResponses = totalResponses;
                summary.Items.Add(new InteractivePlayerPollSummaryItem
                {
                    SelectedOptionId = SafeString(reader, "selected_option_id"),
                    SelectedOptionText = SafeString(reader, "selected_option_text"),
                    ResponseCount = responseCount,
                    Percentage = totalResponses > 0
                        ? Math.Round((decimal)responseCount * 100m / totalResponses, 2, MidpointRounding.AwayFromZero)
                        : 0m,
                });
            }

            return summary;
        }

        public InteractivePlayerResumeCheckpoint? GetResumeCheckpoint(
            string trainingId,
            string sessionId,
            string userId,
            string userType,
            string branchId)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand(@"
SELECT TOP (1)
    ttiprc_trainingid AS training_id,
    ttiprc_sessionid AS session_id,
    ttiprc_contentid AS content_id,
    ttiprc_contentkind AS content_kind,
    ttiprc_userid AS user_id,
    ttiprc_usertype AS user_type,
    ttiprc_branchid AS branch_id,
    ttiprc_mediapositionseconds AS media_position_seconds,
    ttiprc_pagenumber AS page_number,
    ttiprc_activeactivityid AS active_activity_id,
    ttiprc_updatedon AS updated_at
FROM trainingplan.tbl_tp_ip_resume_checkpoint
WHERE ttiprc_trainingid = @training_id
  AND ttiprc_sessionid = @session_id
  AND ttiprc_userid = @user_id
  AND ttiprc_usertype = @user_type
  AND ttiprc_branchid = @branch_id;", con);

            AddResumeScopeParameters(cmd, trainingId, sessionId, userId, userType, branchId);
            cmd.CommandTimeout = 5000;
            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            return reader.Read() ? MapResumeCheckpoint(reader) : null;
        }

        public InteractivePlayerResumeCheckpoint SaveResumeCheckpoint(SaveInteractivePlayerResumeRequest request)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LitteraDatabase"));
            using SqlCommand cmd = new SqlCommand(@"
UPDATE trainingplan.tbl_tp_ip_resume_checkpoint
SET
    ttiprc_contentid = @content_id,
    ttiprc_contentkind = @content_kind,
    ttiprc_mediapositionseconds = @media_position_seconds,
    ttiprc_pagenumber = @page_number,
    ttiprc_activeactivityid = @active_activity_id,
    ttiprc_updatedon = SYSUTCDATETIME()
WHERE ttiprc_trainingid = @training_id
  AND ttiprc_sessionid = @session_id
  AND ttiprc_userid = @user_id
  AND ttiprc_usertype = @user_type
  AND ttiprc_branchid = @branch_id;

IF @@ROWCOUNT = 0
BEGIN
    INSERT INTO trainingplan.tbl_tp_ip_resume_checkpoint
    (
        ttiprc_trainingid,
        ttiprc_sessionid,
        ttiprc_contentid,
        ttiprc_contentkind,
        ttiprc_userid,
        ttiprc_usertype,
        ttiprc_branchid,
        ttiprc_mediapositionseconds,
        ttiprc_pagenumber
        ,ttiprc_activeactivityid
    )
    VALUES
    (
        @training_id,
        @session_id,
        @content_id,
        @content_kind,
        @user_id,
        @user_type,
        @branch_id,
        @media_position_seconds,
        @page_number
        ,@active_activity_id
    );
END

SELECT TOP (1)
    ttiprc_trainingid AS training_id,
    ttiprc_sessionid AS session_id,
    ttiprc_contentid AS content_id,
    ttiprc_contentkind AS content_kind,
    ttiprc_userid AS user_id,
    ttiprc_usertype AS user_type,
    ttiprc_branchid AS branch_id,
    ttiprc_mediapositionseconds AS media_position_seconds,
    ttiprc_pagenumber AS page_number,
    ttiprc_activeactivityid AS active_activity_id,
    ttiprc_updatedon AS updated_at
FROM trainingplan.tbl_tp_ip_resume_checkpoint
WHERE ttiprc_trainingid = @training_id
  AND ttiprc_sessionid = @session_id
  AND ttiprc_userid = @user_id
  AND ttiprc_usertype = @user_type
  AND ttiprc_branchid = @branch_id;", con);

            AddResumeScopeParameters(cmd, request.TrainingId, request.SessionId, request.UserId, request.UserType, request.BranchId);
            cmd.Parameters.AddWithValue("@content_id", request.ContentId.Trim());
            cmd.Parameters.AddWithValue("@content_kind", request.ContentKind.Trim().ToLowerInvariant());
            cmd.Parameters.AddWithValue("@media_position_seconds", DbValue(request.MediaPositionSeconds));
            cmd.Parameters.AddWithValue("@page_number", DbValue(request.PageNumber));
            cmd.Parameters.AddWithValue("@active_activity_id", DbValue(request.ActiveActivityId?.Trim()));
            cmd.CommandTimeout = 5000;
            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapResumeCheckpoint(reader);
            }

            throw new Exception("Interactive player resume checkpoint could not be saved.");
        }

        private static InteractivePlayerActivity MapActivity(SqlDataReader reader)
        {
            return new InteractivePlayerActivity
            {
                Id = SafeString(reader, "activity_id"),
                SessionId = SafeString(reader, "session_id"),
                ContentId = SafeString(reader, "content_id"),
                Type = SafeString(reader, "activity_type"),
                Title = SafeNullableString(reader, "title"),
                Instruction = SafeNullableString(reader, "instruction"),
                BodyText = SafeNullableString(reader, "body_text"),
                TriggerTime = SafeNullableInt(reader, "trigger_time"),
                PageNumber = SafeNullableInt(reader, "page_number"),
                TriggerMode = SafeString(reader, "trigger_mode", "START"),
                PauseOnTrigger = SafeBool(reader, "pause_on_trigger"),
                Skippable = SafeBool(reader, "skippable"),
                DisplayOrder = SafeNullableInt(reader, "display_order"),
                IsActive = SafeInt(reader, "activity_status", (int)ActivityStatus.Active),
                CreatedBy = SafeNullableString(reader, "created_by"),
                UpdatedBy = SafeNullableString(reader, "updated_by"),
                BranchId = SafeNullableString(reader, "branch_id"),
                CreatedAt = SafeDateTimeOffset(reader, "created_at"),
                UpdatedAt = SafeNullableDateTimeOffset(reader, "updated_at"),
                DetailJson = SafeNullableString(reader, "detail_json"),
                SchemaVersion = SafeInt(reader, "schema_version", 1)
            };
        }

        private static InteractivePlayerResumeCheckpoint MapResumeCheckpoint(SqlDataReader reader)
        {
            return new InteractivePlayerResumeCheckpoint
            {
                TrainingId = SafeString(reader, "training_id"),
                SessionId = SafeString(reader, "session_id"),
                ContentId = SafeString(reader, "content_id"),
                ContentKind = SafeString(reader, "content_kind"),
                UserId = SafeString(reader, "user_id"),
                UserType = SafeString(reader, "user_type"),
                BranchId = SafeString(reader, "branch_id"),
                MediaPositionSeconds = SafeNullableDecimal(reader, "media_position_seconds"),
                PageNumber = SafeNullableInt(reader, "page_number"),
                ActiveActivityId = SafeNullableString(reader, "active_activity_id"),
                UpdatedAt = SafeDateTimeOffset(reader, "updated_at"),
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

        private static InteractivePlayerActivityResponse MapActivityResponse(SqlDataReader reader)
        {
            return new InteractivePlayerActivityResponse
            {
                ResponseId = SafeString(reader, "response_id"),
                ActivityId = SafeString(reader, "activity_id"),
                SessionId = SafeString(reader, "session_id"),
                ContentId = SafeString(reader, "content_id"),
                ActivityType = SafeString(reader, "activity_type"),
                UserId = SafeString(reader, "user_id"),
                UserType = SafeString(reader, "user_type"),
                AttemptNumber = SafeInt(reader, "attempt_number"),
                IsResubmission = SafeBool(reader, "is_resubmission"),
                Status = SafeString(reader, "status"),
                ResponseJson = ParseJsonElement(SafeNullableString(reader, "response_json")),
                ResultJson = ParseJsonElement(SafeNullableString(reader, "result_json")),
                StartedAt = SafeNullableString(reader, "started_at"),
                CompletedAt = SafeNullableString(reader, "completed_at"),
                CreatedOn = SafeDateTimeOffset(reader, "created_on"),
            };
        }

        private static InteractivePlayerLatestActivityResponse MapLatestActivityResponse(SqlDataReader reader)
        {
            return new InteractivePlayerLatestActivityResponse
            {
                HasPriorSubmission = SafeBool(reader, "has_prior_submission"),
                LastAttemptNumber = SafeInt(reader, "last_attempt_number"),
                LastSubmittedAt = SafeNullableString(reader, "last_submitted_at"),
                LastStatus = SafeString(reader, "last_status"),
                LatestResponseJson = ParseJsonElement(SafeNullableString(reader, "latest_response_json")),
            };
        }

        private static object DbValue(object? value)
        {
            return value ?? DBNull.Value;
        }

        private static void AddResumeScopeParameters(
            SqlCommand cmd,
            string? trainingId,
            string? sessionId,
            string? userId,
            string? userType,
            string? branchId)
        {
            cmd.Parameters.AddWithValue("@training_id", trainingId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@session_id", sessionId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@user_id", userId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@user_type", userType?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@branch_id", branchId?.Trim() ?? string.Empty);
        }

        private static string NormalizeTriggerMode(string? triggerMode)
        {
            string normalized = string.IsNullOrWhiteSpace(triggerMode) ? "START" : triggerMode.Trim().ToUpperInvariant();
            return normalized switch
            {
                "START" => "start",
                "CUSTOM" => "custom",
                "END" => "end",
                "PAGE" => "page",
                _ => "start"
            };
        }

        private static string NormalizeActivityType(string? activityType)
        {
            return string.IsNullOrWhiteSpace(activityType)
                ? "pop"
                : activityType.Trim().ToLowerInvariant();
        }

        private static long InsertActivity(
            SqlConnection con,
            SqlTransaction transaction,
            CreateInteractivePlayerActivityRequest request)
        {
            using SqlCommand cmd = new SqlCommand(@"
INSERT INTO trainingplan.tbl_tp_ip_acvtivity_master
(
    ttiam_sessionid,
    ttiam_contentid,
    ttiam_activitytype,
    ttiam_title,
    ttiam_instruction,
    ttiam_bodytext,
    ttiam_triggermode,
    ttiam_triggertime,
    ttiam_pagenumber,
    ttiam_pauseontrigger,
    ttiam_skippable,
    ttiam_displayorder,
    ttiam_isactive,
    ttiam_createdby,
    ttiam_branchid,
    ttiam_updatedby
)
VALUES
(
    @session_id,
    @content_id,
    @activity_type,
    @title,
    @instruction,
    @body_text,
    @trigger_mode,
    @trigger_time,
    @page_number,
    @pause_on_trigger,
    @skippable,
    @display_order,
    @activity_status,
    @created_by,
    @branch_id,
    @updated_by
);
SELECT CAST(SCOPE_IDENTITY() AS bigint);", con, transaction);

            AddMasterActivityParameters(cmd, request.SessionId, request.ContentId, request.Type, request.Title, request.Instruction, request.BodyText,
                request.TriggerMode, request.TriggerTime, request.PageNumber, request.PauseOnTrigger, request.Skippable,
                request.DisplayOrder, request.CreatedBy, request.UpdatedBy, request.BranchId, request.IsActive ?? (int)ActivityStatus.Active);

            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        private static bool UpdateActivityRow(
            SqlConnection con,
            SqlTransaction transaction,
            long activityId,
            UpdateInteractivePlayerActivityRequest request)
        {
            using SqlCommand cmd = new SqlCommand(@"
UPDATE trainingplan.tbl_tp_ip_acvtivity_master
SET
    ttiam_sessionid = @session_id,
    ttiam_contentid = @content_id,
    ttiam_activitytype = @activity_type,
    ttiam_title = @title,
    ttiam_instruction = @instruction,
    ttiam_bodytext = @body_text,
    ttiam_triggermode = @trigger_mode,
    ttiam_triggertime = @trigger_time,
    ttiam_pagenumber = @page_number,
    ttiam_pauseontrigger = @pause_on_trigger,
    ttiam_skippable = @skippable,
    ttiam_displayorder = @display_order,
    ttiam_isactive = COALESCE(@activity_status, ttiam_isactive),
    ttiam_branchid = @branch_id,
    ttiam_updatedby = @updated_by,
    ttiam_updatedon = SYSUTCDATETIME()
WHERE ttiam_activityid = @activity_id
  AND ttiam_isactive <> @deleted_status;", con, transaction);

            cmd.Parameters.AddWithValue("@activity_id", activityId);
            cmd.Parameters.AddWithValue("@deleted_status", (int)ActivityStatus.Deleted);
            AddMasterActivityParameters(cmd, request.SessionId, request.ContentId, request.Type, request.Title, request.Instruction, request.BodyText,
                request.TriggerMode, request.TriggerTime, request.PageNumber, request.PauseOnTrigger, request.Skippable,
                request.DisplayOrder, request.CreatedBy, request.UpdatedBy, request.BranchId, request.IsActive);

            return cmd.ExecuteNonQuery() > 0;
        }

        private static void UpsertActivityDetail(
            SqlConnection con,
            SqlTransaction transaction,
            long activityId,
            string detailJson,
            int schemaVersion)
        {
            using SqlCommand cmd = new SqlCommand(@"
IF EXISTS (
    SELECT 1
    FROM trainingplan.tbl_tp_ip_acvtivity_detail
    WHERE ttiad_ttiam_ActivityId = @activity_id
)
BEGIN
    UPDATE trainingplan.tbl_tp_ip_acvtivity_detail
    SET
        ttiad_DetailJson = @detail_json,
        ttiad_SchemaVersion = @schema_version,
        ttiad_UpdatedOn = SYSUTCDATETIME()
    WHERE ttiad_ttiam_ActivityId = @activity_id;
END
ELSE
BEGIN
    INSERT INTO trainingplan.tbl_tp_ip_acvtivity_detail
    (
        ttiad_ttiam_ActivityId,
        ttiad_DetailJson,
        ttiad_SchemaVersion
    )
    VALUES
    (
        @activity_id,
        @detail_json,
        @schema_version
    );
END", con, transaction);

            cmd.Parameters.AddWithValue("@activity_id", activityId);
            cmd.Parameters.AddWithValue("@detail_json", detailJson);
            cmd.Parameters.AddWithValue("@schema_version", schemaVersion <= 0 ? 1 : schemaVersion);
            cmd.ExecuteNonQuery();
        }

        private static InteractivePlayerActivity? GetActivityById(
            SqlConnection con,
            SqlTransaction transaction,
            long activityId)
        {
            using SqlCommand cmd = new SqlCommand($@"
{ActivitySelectSql}
WHERE m.ttiam_activityid = @activity_id
  AND m.ttiam_isactive <> @deleted_status;", con, transaction);
            cmd.Parameters.AddWithValue("@activity_id", activityId);
            cmd.Parameters.AddWithValue("@deleted_status", (int)ActivityStatus.Deleted);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapActivity(reader);
            }

            return null;
        }

        private static void AddMasterActivityParameters(
            SqlCommand cmd,
            string sessionId,
            string contentId,
            string? type,
            string? title,
            string? instruction,
            string? bodyText,
            string? triggerMode,
            int? triggerTime,
            int? pageNumber,
            bool pauseOnTrigger,
            bool skippable,
            int? displayOrder,
            string? createdBy,
            string? updatedBy,
            string? branchId,
            int? isActive)
        {
            string normalizedTriggerMode = NormalizeTriggerMode(triggerMode);
            bool isCustom = normalizedTriggerMode == "custom";
            bool isPage = normalizedTriggerMode == "page";
            string resolvedCreatedBy = string.IsNullOrWhiteSpace(createdBy) ? "system" : createdBy.Trim();
            string resolvedUpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? resolvedCreatedBy : updatedBy.Trim();
            int? normalizedStatus = isActive.HasValue ? NormalizeActivityStatus(isActive.Value) : null;

            cmd.Parameters.AddWithValue("@session_id", sessionId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@content_id", contentId?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@activity_type", NormalizeActivityType(type));
            cmd.Parameters.AddWithValue("@title", title?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@instruction", instruction?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@body_text", bodyText?.Trim() ?? string.Empty);
            cmd.Parameters.AddWithValue("@trigger_mode", normalizedTriggerMode);
            cmd.Parameters.AddWithValue("@trigger_time", isCustom ? DbValue(triggerTime) : DBNull.Value);
            cmd.Parameters.AddWithValue("@page_number", isPage ? DbValue(pageNumber) : DBNull.Value);
            cmd.Parameters.AddWithValue("@pause_on_trigger", pauseOnTrigger);
            cmd.Parameters.AddWithValue("@skippable", skippable);
            cmd.Parameters.AddWithValue("@display_order", DbValue(displayOrder));
            cmd.Parameters.AddWithValue("@activity_status", DbValue(normalizedStatus));
            cmd.Parameters.AddWithValue("@created_by", resolvedCreatedBy);
            cmd.Parameters.AddWithValue("@updated_by", resolvedUpdatedBy);
            cmd.Parameters.AddWithValue("@branch_id", branchId?.Trim() ?? string.Empty);
        }

        private static int NormalizeActivityStatus(int value)
        {
            return value switch
            {
                (int)ActivityStatus.Active => (int)ActivityStatus.Active,
                (int)ActivityStatus.Inactive => (int)ActivityStatus.Inactive,
                (int)ActivityStatus.Deleted => (int)ActivityStatus.Deleted,
                _ => (int)ActivityStatus.Active
            };
        }

        private static string ResolveDetailJson(JsonElement? details, string? detailJson)
        {
            if (!string.IsNullOrWhiteSpace(detailJson))
            {
                return detailJson;
            }

            string? detailsJson = ToJsonString(details);
            return string.IsNullOrWhiteSpace(detailsJson) ? "{}" : detailsJson;
        }

        private static bool TryParseActivityId(string? activityId, out long parsedActivityId)
        {
            return long.TryParse(activityId, out parsedActivityId);
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

        private static decimal? SafeNullableDecimal(SqlDataReader reader, string columnName)
        {
            if (!HasColumn(reader, columnName))
            {
                return null;
            }

            object value = reader[columnName];
            return value == DBNull.Value ? null : Convert.ToDecimal(value);
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
