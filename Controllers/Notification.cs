using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using static LitteraCore.Models.Firebase;
using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;



namespace LitteraCore.Controllers
{
    public class Notification : Controller
    {
        private readonly IConfiguration _configuration;
        public Notification(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("send")]
        [SwaggerOperation("Validates user credentials and logs the user in")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationUsers users, string title, string message)
        {
            AgencyDB adb = new AgencyDB(_configuration);
            root ft;
            ft = adb.Get_Firebase_Token_Details(users);

            NotificationRequest request;

            //if (request == null || string.IsNullOrEmpty(request.UserToken) || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Message))
            //{
            //    return BadRequest("Invalid request data.");
            //}

            try
            {
                List<string> sreg = new List<string>();
                foreach (tokens t in ft.tokendetails)
                {
                    Firebase fb = new Firebase(_configuration);
                    // Call Send_Notification method
                    await fb.Send_Notification(t.tyft_token, title, message);
                }


                return Ok("Notification sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
