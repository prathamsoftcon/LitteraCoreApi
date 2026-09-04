using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace LitteraCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InteractivePlayerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InteractivePlayerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("activities/{contentId}")]
        [SwaggerOperation("To get interactive player activities for a content item.")]
        public IActionResult GetActivities(string contentId, [FromQuery] string? sessionId = null)
        {
            if (string.IsNullOrWhiteSpace(contentId))
            {
                return BadRequest("contentId is required.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.GetActivities(contentId, sessionId) ?? new List<InteractivePlayerActivity>());
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Unable to load interactive activities."
                });
            }
        }

        [HttpPost("activities")]
        [SwaggerOperation("To create an interactive player activity.")]
        public IActionResult CreateActivity([FromBody] CreateInteractivePlayerActivityRequest request)
        {
            if (request == null)
            {
                return BadRequest("A valid activity payload is required.");
            }
            string? validationMessage = ValidateActivityRequest(
                request.SessionId,
                request.ContentId,
                request.Type,
                request.Title,
                request.Instruction,
                request.BodyText,
                request.BranchId,
                request.IsActive,
                request.DetailJson,
                request.Details);
            if (validationMessage != null)
            {
                return BadRequest(validationMessage);
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                var saved = bl.SaveActivity(request);
                return Ok(saved);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpPut("activities/{activityId}")]
        [SwaggerOperation("To update an interactive player activity.")]
        public IActionResult UpdateActivity(string activityId, [FromBody] UpdateInteractivePlayerActivityRequest request)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return BadRequest("activityId is required.");
            }

            if (!IsValidActivityId(activityId))
            {
                return BadRequest("activityId must be a valid numeric value.");
            }

            if (request == null)
            {
                return BadRequest("A valid update payload is required.");
            }
            string? validationMessage = ValidateActivityRequest(
                request.SessionId,
                request.ContentId,
                request.Type,
                request.Title,
                request.Instruction,
                request.BodyText,
                request.BranchId,
                request.IsActive,
                request.DetailJson,
                request.Details);
            if (validationMessage != null)
            {
                return BadRequest(validationMessage);
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                var updated = bl.UpdateActivity(activityId, request);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpPost("activities/{activityId}/update")]
        [SwaggerOperation("To update an interactive player activity using POST transport.")]
        public IActionResult UpdateActivityPost(string activityId, [FromBody] UpdateInteractivePlayerActivityRequest request)
        {
            return UpdateActivity(activityId, request);
        }

        [HttpDelete("activities/{activityId}")]
        [SwaggerOperation("To delete an interactive player activity.")]
        public IActionResult DeleteActivity(string activityId)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return BadRequest("activityId is required.");
            }

            if (!IsValidActivityId(activityId))
            {
                return BadRequest("activityId must be a valid numeric value.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                var deleted = bl.DeleteActivity(activityId);
                if (!deleted)
                {
                    return NotFound();
                }

                return Ok(true);
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpPost("activities/{activityId}/delete")]
        [SwaggerOperation("To delete an interactive player activity using POST transport.")]
        public IActionResult DeleteActivityPost(string activityId)
        {
            return DeleteActivity(activityId);
        }

        [HttpPost("outcomes")]
        [SwaggerOperation("To save an interactive player outcome.")]
        public IActionResult SaveOutcome([FromBody] CreateInteractivePlayerOutcomeRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ActivityId))
            {
                return BadRequest("A valid outcome payload is required.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.SaveOutcome(request));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpPost("quiz-submissions")]
        [SwaggerOperation("To save a quiz submission for interactive player activities.")]
        public IActionResult SaveQuizSubmission([FromBody] CreateInteractivePlayerQuizSubmissionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.QuestionId))
            {
                return BadRequest("A valid quiz submission payload is required.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.SaveQuizSubmission(request));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpPost("activity-responses")]
        [SwaggerOperation("To save an interactive player activity response.")]
        public IActionResult SaveActivityResponse([FromBody] CreateInteractivePlayerActivityResponseRequest request)
        {
            if (request == null)
            {
                return BadRequest("A valid activity response payload is required.");
            }

            string? validationMessage = ValidateActivityResponseRequest(request);
            if (validationMessage != null)
            {
                return BadRequest(validationMessage);
            }

            if (!IsValidActivityId(request.ActivityId))
            {
                return BadRequest("activityId must be a valid numeric value.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.SaveActivityResponse(request));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpGet("activity-responses/latest")]
        [SwaggerOperation("To get the latest interactive player activity response for a learner.")]
        public IActionResult GetLatestActivityResponse(
            [FromQuery] string activityId,
            [FromQuery] string sessionId,
            [FromQuery] string contentId,
            [FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return BadRequest("activityId is required.");
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return BadRequest("sessionId is required.");
            }

            if (string.IsNullOrWhiteSpace(contentId))
            {
                return BadRequest("contentId is required.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("userId is required.");
            }

            if (!IsValidActivityId(activityId))
            {
                return BadRequest("activityId must be a valid numeric value.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.GetLatestActivityResponse(activityId, sessionId, contentId, userId));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpGet("activity-responses/poll-summary")]
        [SwaggerOperation("To get aggregate poll summary for an interactive player poll activity.")]
        public IActionResult GetPollSummary(
            [FromQuery] string activityId,
            [FromQuery] string sessionId,
            [FromQuery] string contentId)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return BadRequest("activityId is required.");
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return BadRequest("sessionId is required.");
            }

            if (string.IsNullOrWhiteSpace(contentId))
            {
                return BadRequest("contentId is required.");
            }

            if (!IsValidActivityId(activityId))
            {
                return BadRequest("activityId must be a valid numeric value.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.GetPollSummary(activityId, sessionId, contentId));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
        }

        [HttpGet("resume")]
        [SwaggerOperation("To get the authenticated learner's latest session-player resume checkpoint.")]
        public IActionResult GetResumeCheckpoint(
            [FromQuery] string trainingId,
            [FromQuery] string sessionId,
            [FromQuery] string userId,
            [FromQuery] string userType,
            [FromQuery] string branchId)
        {
            string? validationMessage = ValidateResumeScope(trainingId, sessionId, userId, userType, branchId);
            if (validationMessage != null)
            {
                return BadRequest(validationMessage);
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.GetResumeCheckpoint(trainingId, sessionId, userId, userType, branchId));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Unable to load the session-player resume checkpoint."
                });
            }
        }

        [HttpPost("resume")]
        [SwaggerOperation("To save the authenticated learner's latest session-player resume checkpoint.")]
        public IActionResult SaveResumeCheckpoint([FromBody] SaveInteractivePlayerResumeRequest request)
        {
            if (request == null)
            {
                return BadRequest("A valid resume checkpoint payload is required.");
            }

            string? validationMessage = ValidateResumeScope(
                request.TrainingId,
                request.SessionId,
                request.UserId,
                request.UserType,
                request.BranchId);
            if (validationMessage != null)
            {
                return BadRequest(validationMessage);
            }

            if (string.IsNullOrWhiteSpace(request.ContentId))
            {
                return BadRequest("contentId is required.");
            }

            if (string.IsNullOrWhiteSpace(request.ContentKind))
            {
                return BadRequest("contentKind is required.");
            }

            if (request.MediaPositionSeconds.HasValue && request.MediaPositionSeconds < 0)
            {
                return BadRequest("mediaPositionSeconds cannot be negative.");
            }

            if (request.PageNumber.HasValue && request.PageNumber < 1)
            {
                return BadRequest("pageNumber must be at least 1.");
            }

            try
            {
                InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
                return Ok(bl.SaveResumeCheckpoint(request));
            }
            catch (SqlException ex)
            {
                return SqlExceptionResponseHelper.CreateBadRequest(ex);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Unable to save the session-player resume checkpoint."
                });
            }
        }

        private static bool IsValidActivityId(string? activityId)
        {
            return long.TryParse(activityId, out _);
        }

        private static string? ValidateResumeScope(
            string? trainingId,
            string? sessionId,
            string? userId,
            string? userType,
            string? branchId)
        {
            if (string.IsNullOrWhiteSpace(trainingId)) return "trainingId is required.";
            if (string.IsNullOrWhiteSpace(sessionId)) return "sessionId is required.";
            if (string.IsNullOrWhiteSpace(userId)) return "userId is required.";
            if (string.IsNullOrWhiteSpace(userType)) return "userType is required.";
            if (string.IsNullOrWhiteSpace(branchId)) return "branchId is required.";
            return null;
        }

        private static string? ValidateActivityRequest(
            string? sessionId,
            string? contentId,
            string? activityType,
            string? title,
            string? instruction,
            string? bodyText,
            string? branchId,
            int? isActive,
            string? detailJson,
            JsonElement? details)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return "sessionId is required.";
            }

            if (string.IsNullOrWhiteSpace(contentId))
            {
                return "contentId is required.";
            }

            if (string.IsNullOrWhiteSpace(activityType))
            {
                return "type is required.";
            }

            if (string.IsNullOrWhiteSpace(branchId))
            {
                return "branchId is required.";
            }

            if (isActive.HasValue && isActive != 1 && isActive != 2 && isActive != 9)
            {
                return "isActive must be 1 (Active), 2 (Paused), or 9 (Deleted).";
            }

            string resolvedDetailJson = !string.IsNullOrWhiteSpace(detailJson)
                ? detailJson
                : details?.GetRawText() ?? "{}";

            try
            {
                JsonDocument.Parse(resolvedDetailJson);
            }
            catch (JsonException)
            {
                return "detailJson must contain valid JSON.";
            }

            return null;
        }

        private static string? ValidateActivityResponseRequest(CreateInteractivePlayerActivityResponseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ActivityId))
            {
                return "activityId is required.";
            }

            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                return "sessionId is required.";
            }

            if (string.IsNullOrWhiteSpace(request.ContentId))
            {
                return "contentId is required.";
            }

            if (string.IsNullOrWhiteSpace(request.ActivityType))
            {
                return "activityType is required.";
            }

            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return "userId is required.";
            }

            if (string.IsNullOrWhiteSpace(request.UserType))
            {
                return "userType is required.";
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                return "status is required.";
            }

            if (request.ResponseJson == null || request.ResponseJson.Value.ValueKind != JsonValueKind.Object)
            {
                return "responseJson must contain a valid JSON object.";
            }

            if (!string.Equals(request.Status.Trim(), "completed", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            JsonElement responseJson = request.ResponseJson.Value;
            if (TryGetArray(responseJson, "responses", out JsonElement responses))
            {
                return ValidateBundledActivityResponses(responses);
            }

            string responseType = GetJsonString(responseJson, "responseType");
            if (string.IsNullOrWhiteSpace(responseType))
            {
                return "responseJson.responseType is required for completed submissions.";
            }

            string submissionMethod = GetJsonString(responseJson, "submissionMethod");
            if (string.IsNullOrWhiteSpace(submissionMethod))
            {
                return "responseJson.submissionMethod is required for completed submissions.";
            }

            if (string.Equals(submissionMethod, "LINK", StringComparison.OrdinalIgnoreCase))
            {
                string linkUrl = GetJsonString(responseJson, "linkUrl");
                if (string.IsNullOrWhiteSpace(linkUrl))
                {
                    return "responseJson.linkUrl is required when submissionMethod is LINK.";
                }
            }

            if (
                string.Equals(submissionMethod, "UPLOAD", StringComparison.OrdinalIgnoreCase)
                || string.Equals(submissionMethod, "RECORD", StringComparison.OrdinalIgnoreCase)
            )
            {
                string? uploadValidationMessage = ValidateUploadedFile(responseJson, "responseJson.uploadedFile");
                if (uploadValidationMessage != null)
                {
                    return uploadValidationMessage;
                }
            }

            return null;
        }

        private static string? ValidateBundledActivityResponses(JsonElement responses)
        {
            if (responses.ValueKind != JsonValueKind.Array)
            {
                return "responseJson.responses must be an array.";
            }

            int responseIndex = 0;
            bool hasSubmittedResponse = false;

            foreach (JsonElement responseItem in responses.EnumerateArray())
            {
                responseIndex++;

                if (responseItem.ValueKind != JsonValueKind.Object)
                {
                    return $"responseJson.responses[{responseIndex - 1}] must be a JSON object.";
                }

                string responseType = GetJsonString(responseItem, "responseType");
                if (string.IsNullOrWhiteSpace(responseType))
                {
                    return $"responseJson.responses[{responseIndex - 1}].responseType is required.";
                }

                string submissionMethod = GetJsonString(responseItem, "submissionMethod");
                if (string.IsNullOrWhiteSpace(submissionMethod))
                {
                    return $"responseJson.responses[{responseIndex - 1}].submissionMethod is required.";
                }

                bool isEmpty = GetJsonBoolean(responseItem, "isEmpty");
                if (isEmpty)
                {
                    continue;
                }

                hasSubmittedResponse = true;

                if (string.Equals(submissionMethod, "LINK", StringComparison.OrdinalIgnoreCase))
                {
                    string linkUrl = GetJsonString(responseItem, "linkUrl");
                    if (string.IsNullOrWhiteSpace(linkUrl))
                    {
                        return $"responseJson.responses[{responseIndex - 1}].linkUrl is required when submissionMethod is LINK.";
                    }
                }

                if (
                    string.Equals(submissionMethod, "UPLOAD", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(submissionMethod, "RECORD", StringComparison.OrdinalIgnoreCase)
                )
                {
                    string? uploadValidationMessage = ValidateUploadedFile(
                        responseItem,
                        $"responseJson.responses[{responseIndex - 1}].uploadedFile");
                    if (uploadValidationMessage != null)
                    {
                        return uploadValidationMessage;
                    }
                }
            }

            if (responseIndex == 0)
            {
                return "responseJson.responses must contain at least one response item.";
            }

            if (!hasSubmittedResponse)
            {
                return "responseJson.responses must contain at least one completed learner response.";
            }

            return null;
        }

        private static string? ValidateUploadedFile(JsonElement jsonElement, string jsonPath)
        {
            if (!TryGetObject(jsonElement, "uploadedFile", out JsonElement uploadedFile))
            {
                return $"{jsonPath} is required when submissionMethod is UPLOAD or RECORD.";
            }

            string fileUrl = GetJsonString(uploadedFile, "fileUrl");
            string storedPath = GetJsonString(uploadedFile, "storedPath");
            string fileName = GetJsonString(uploadedFile, "fileName");
            if (string.IsNullOrWhiteSpace(fileUrl) && string.IsNullOrWhiteSpace(storedPath) && string.IsNullOrWhiteSpace(fileName))
            {
                return $"{jsonPath} must include file details when submissionMethod is UPLOAD or RECORD.";
            }

            return null;
        }

        private static string GetJsonString(JsonElement jsonElement, string propertyName)
        {
            if (!jsonElement.TryGetProperty(propertyName, out JsonElement value))
            {
                return string.Empty;
            }

            return value.ValueKind switch
            {
                JsonValueKind.String => value.GetString()?.Trim() ?? string.Empty,
                JsonValueKind.Number => value.ToString().Trim(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => string.Empty,
            };
        }

        private static bool GetJsonBoolean(JsonElement jsonElement, string propertyName)
        {
            if (!jsonElement.TryGetProperty(propertyName, out JsonElement value))
            {
                return false;
            }

            return value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.String => bool.TryParse(value.GetString(), out bool parsedValue) && parsedValue,
                JsonValueKind.Number => value.TryGetInt32(out int numericValue) && numericValue != 0,
                _ => false,
            };
        }

        private static bool TryGetObject(JsonElement jsonElement, string propertyName, out JsonElement objectElement)
        {
            if (jsonElement.TryGetProperty(propertyName, out objectElement) && objectElement.ValueKind == JsonValueKind.Object)
            {
                return true;
            }

            objectElement = default;
            return false;
        }

        private static bool TryGetArray(JsonElement jsonElement, string propertyName, out JsonElement arrayElement)
        {
            if (jsonElement.TryGetProperty(propertyName, out arrayElement) && arrayElement.ValueKind == JsonValueKind.Array)
            {
                return true;
            }

            arrayElement = default;
            return false;
        }
    }
}
