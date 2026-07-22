using System.Data;
using System.Text.Json;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;

namespace LitteraCore.DBContext
{
    public class InteractivePlayerDB
    {
        private readonly IConfiguration _configuration;
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
WHERE m.ttiam_isactive = 1
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
DELETE FROM trainingplan.tbl_tp_ip_acvtivity_master
WHERE ttiam_activityid = @activity_id;", con);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@activity_id", parsedActivityId);

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
                CreatedBy = SafeNullableString(reader, "created_by"),
                UpdatedBy = SafeNullableString(reader, "updated_by"),
                BranchId = SafeNullableString(reader, "branch_id"),
                CreatedAt = SafeDateTimeOffset(reader, "created_at"),
                UpdatedAt = SafeNullableDateTimeOffset(reader, "updated_at"),
                DetailJson = SafeNullableString(reader, "detail_json"),
                SchemaVersion = SafeInt(reader, "schema_version", 1)
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
    1,
    @created_by,
    @branch_id,
    @updated_by
);
SELECT CAST(SCOPE_IDENTITY() AS bigint);", con, transaction);

            AddMasterActivityParameters(cmd, request.SessionId, request.ContentId, request.Type, request.Title, request.Instruction, request.BodyText,
                request.TriggerMode, request.TriggerTime, request.PageNumber, request.PauseOnTrigger, request.Skippable,
                request.DisplayOrder, request.CreatedBy, request.UpdatedBy, request.BranchId);

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
    ttiam_branchid = @branch_id,
    ttiam_updatedby = @updated_by,
    ttiam_updatedon = SYSUTCDATETIME()
WHERE ttiam_activityid = @activity_id;", con, transaction);

            cmd.Parameters.AddWithValue("@activity_id", activityId);
            AddMasterActivityParameters(cmd, request.SessionId, request.ContentId, request.Type, request.Title, request.Instruction, request.BodyText,
                request.TriggerMode, request.TriggerTime, request.PageNumber, request.PauseOnTrigger, request.Skippable,
                request.DisplayOrder, request.CreatedBy, request.UpdatedBy, request.BranchId);

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
WHERE m.ttiam_activityid = @activity_id;", con, transaction);
            cmd.Parameters.AddWithValue("@activity_id", activityId);

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
            string? branchId)
        {
            string normalizedTriggerMode = NormalizeTriggerMode(triggerMode);
            bool isCustom = normalizedTriggerMode == "custom";
            bool isPage = normalizedTriggerMode == "page";
            string resolvedCreatedBy = string.IsNullOrWhiteSpace(createdBy) ? "system" : createdBy.Trim();
            string resolvedUpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? resolvedCreatedBy : updatedBy.Trim();

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
            cmd.Parameters.AddWithValue("@created_by", resolvedCreatedBy);
            cmd.Parameters.AddWithValue("@updated_by", resolvedUpdatedBy);
            cmd.Parameters.AddWithValue("@branch_id", branchId?.Trim() ?? string.Empty);
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
