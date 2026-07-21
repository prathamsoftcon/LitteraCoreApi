using LitteraCore.BLContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        public IActionResult GetActivities(string contentId)
        {
            if (string.IsNullOrWhiteSpace(contentId))
            {
                return BadRequest("contentId is required.");
            }

            InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
            return Ok(bl.GetActivities(contentId));
        }

        [HttpPost("activities")]
        [SwaggerOperation("To create an interactive player activity.")]
        public IActionResult CreateActivity([FromBody] CreateInteractivePlayerActivityRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ContentId))
            {
                return BadRequest("A valid activity payload is required.");
            }

            InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
            var saved = bl.SaveActivity(request);
            return Ok(saved);
        }

        [HttpPut("activities/{activityId}")]
        [SwaggerOperation("To update an interactive player activity.")]
        public IActionResult UpdateActivity(string activityId, [FromBody] UpdateInteractivePlayerActivityRequest request)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return BadRequest("activityId is required.");
            }

            if (request == null)
            {
                return BadRequest("A valid update payload is required.");
            }

            InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
            var updated = bl.UpdateActivity(activityId, request);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
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

            InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
            var deleted = bl.DeleteActivity(activityId);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok(true);
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

            InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
            return Ok(bl.SaveOutcome(request));
        }

        [HttpPost("quiz-submissions")]
        [SwaggerOperation("To save a quiz submission for interactive player activities.")]
        public IActionResult SaveQuizSubmission([FromBody] CreateInteractivePlayerQuizSubmissionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.QuestionId))
            {
                return BadRequest("A valid quiz submission payload is required.");
            }

            InteractivePlayerBL bl = new InteractivePlayerBL(_configuration);
            return Ok(bl.SaveQuizSubmission(request));
        }
    }
}
