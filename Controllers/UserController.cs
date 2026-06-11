using LitteraCore.Common.DMS;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LitteraCore.BLContext;
using LitteraCore.Models.SmsSettings;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json.Linq;
using System.Net;
using LitteraCore.Common.EmailService;
using System.Runtime.ConstrainedExecution;
using LitteraCore.Common.SmsService;
using Newtonsoft.Json;
using System.Data;
using static QRCoder.PayloadGenerator;
using Swashbuckle.AspNetCore.Annotations;
using LitteraCore.Common.OTP;
using LitteraCore.Common.Token;
using Microsoft.AspNetCore.Authorization;

namespace LitteraCore.Controllers
{
   
    public class UserController :  Controller
    {
        private readonly ILogger<AgencyController> _logger;
        private readonly IConfiguration _configuration;
        private readonly OtpManager _otpManager;
        private readonly AppAuthService _authService;
        private readonly AuthCookieService _authCookieService;
        private readonly UserRegistrationService _userRegistrationService;

        public UserController(
            IConfiguration configuration,
            ILogger<AgencyController> logger,
            OtpManager otpManager,
            AppAuthService authService,
            AuthCookieService authCookieService,
            UserRegistrationService userRegistrationService)
        {
            _configuration = configuration;
            _logger = logger;
            _otpManager = otpManager;
            _authService = authService;
            _authCookieService = authCookieService;
            _userRegistrationService = userRegistrationService;
        }

        [Authorize(Policy = "PublicApiKey")]
        [HttpPost]
        [Route("api/RegisterWithOtp")]
        [SwaggerOperation(
            "Verifies OTP, creates a new user, and returns the normal authentication response.")]
        public async Task<IActionResult> RegisterWithOtp(
            [FromBody] RegisterWithOtpRequest request)
        {
            if (request == null
                || string.IsNullOrWhiteSpace(request.verifiedIdentifier)
                || string.IsNullOrWhiteSpace(request.otp))
            {
                return BadRequest(
                    "Verified identifier and OTP are required.");
            }

            var identifier = request.verifiedIdentifier.Trim();
            var authDb = new AuthDB(_configuration);
            if (!string.IsNullOrWhiteSpace(
                    authDb.GetUserInfo(identifier).userid))
            {
                return Conflict("User already exists. Use GetToken with OTP.");
            }

            if (request.user?.agency == null)
            {
                return BadRequest("User details are required for a new account.");
            }

            if (!IdentifierMatchesUser(identifier, request.user))
            {
                return BadRequest(
                    "Verified identifier must match the user's email or mobile number.");
            }

            if (!await _otpManager.VerifyOtpAsync(identifier, request.otp))
            {
                return Unauthorized("Invalid or expired OTP.");
            }

            try
            {
                var registration = _userRegistrationService.Create(request.user);
                if (!registration.Created)
                {
                    return BadRequest("User could not be created.");
                }

                var token = await _authService.Authenticate(identifier);
                if (token?.userdetails?.userid == null)
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        "User was created, but authentication could not be completed. Request a new OTP and sign in.");
                }

                _authCookieService.Append(Response, token.AuthToken);
                authDb.Make_Login_Entry(
                    token.userdetails.userid,
                    "0",
                    GetClientIp());

                return Ok(token);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegisterWithOtp failed.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Registration failed after OTP verification. Request a new OTP before retrying.");
            }
        }

        [HttpPost]
        [Route("api/CreateUser")]
        [SwaggerOperation("To create new user.")]
        public IActionResult CreateParticipantUser([FromBody] LoginUser user, string APPURL = null)
        {
            try
            {
                var registration = _userRegistrationService.Create(user);
                bool isUserCreationMail = registration.ShouldSendCreationEmail;
                bool issaved = registration.Created;
                if (issaved == true)
                {
                    if (isUserCreationMail == true)
                    {
                        if (user.emailid != null)
                        {
                            try
                            {
                                // User creation is already committed. Notification failure
                                // must not report the registration itself as failed.
                                SendUserCreationEmail(user, APPURL);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "User {UserId} was created, but the creation email failed.",
                                    user.userid);
                            }
                        }
                    }

                    return Ok(true);
                }

                return BadRequest("User could not be created.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUser failed for {UserId}.", user?.userid);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "User could not be created.");
            }
        }

        [HttpGet]
        [Route("api/ParticipantLookup")]
        [SwaggerOperation(
            "Looks up participant registration details without changing the current session.")]
        public IActionResult ParticipantLookup(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Mobile number or email is required.");
            }

            AuthDB authDb = new AuthDB(_configuration);
            UserInfo userDetails = authDb.GetUserInfo(username.Trim());
            if (string.IsNullOrWhiteSpace(userDetails.userid)
                || string.IsNullOrWhiteSpace(userDetails.agencyid))
            {
                return NotFound("User not found.");
            }

            AgencyDB agencyDb = new AgencyDB(_configuration);
            Agency agencyDetails = agencyDb
                .Get_Agency_Data_For_Login(
                    null,
                    userDetails.agencyid,
                    1,
                    1,
                    null)
                .FirstOrDefault();

            return Ok(new
            {
                result = new
                {
                    userdetails = userDetails,
                    agencydetail = agencyDetails
                }
            });
        }

        private void SendUserCreationEmail(LoginUser user, string APPURL)
        {
            if (string.IsNullOrWhiteSpace(APPURL))
            {
                return;
            }

            string mailpassword = "";
            if (user.password_enc != null)
            {
                mailpassword = YEncryptDecryptData.YEncryptDecryptData.Decrypt(
                    user.password_enc,
                    true);
            }

            EmailTemplate template =
                SmtpEmailService.Get_EMAIL_TEMPLATE("USERREGISTRATION");
            string mailsubject = template.subject
                .Replace("(#name#)", user.agency.ag_first_name);
            string mailtext = template.text
                .Replace("(#name#)", user.agency.ag_first_name)
                .Replace("(#regname#)", user.agency.ag_first_name)
                .Replace("(#domain#)", APPURL)
                .Replace("(#pwd#)", mailpassword);

            SmtpEmailService smtp = new SmtpEmailService(_configuration);
            _ = smtp.SendEmailAsync(user.emailid, mailsubject, mailtext);

            ApplicationConfigDB configDb =
                new ApplicationConfigDB(_configuration);
            DataTable settings = configDb.Get_Application_Setting("7");
            if (settings.Rows.Count == 0)
            {
                return;
            }

            EMAIL_SEND_BY_APPLICATION emailSettings =
                JsonConvert.DeserializeObject<EMAIL_SEND_BY_APPLICATION>(
                    settings.Rows[0]["SettingValue"].ToString());
            string ccAddress = emailSettings?.EMAILSETTING?.MAIL_CC_TO;
            if (!string.IsNullOrWhiteSpace(ccAddress))
            {
                string ccText =
                    "New user " + user.agency.ag_first_name
                    + " has been successfully registered.";
                _ = smtp.SendEmailAsync(ccAddress, mailsubject, ccText);
            }
        }

        private static bool IdentifierMatchesUser(
            string identifier,
            LoginUser user)
        {
            var isEmail = identifier.Contains('@');
            var candidates = isEmail
                ? new[] { user.emailid, user.agency.ag_email }
                : new[] { user.mobileno, user.agency.ag_mobileno };

            return candidates.Any(candidate =>
                !string.IsNullOrWhiteSpace(candidate)
                && string.Equals(
                    candidate.Trim(),
                    identifier,
                    isEmail
                        ? StringComparison.OrdinalIgnoreCase
                        : StringComparison.Ordinal));
        }

        private string GetClientIp()
        {
            var clientIp =
                HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? string.Empty;

            if (HttpContext.Request.Headers.TryGetValue(
                    "X-Forwarded-For",
                    out var forwardedFor))
            {
                clientIp = forwardedFor.FirstOrDefault() ?? clientIp;
            }

            return clientIp;
        }

    }
}
