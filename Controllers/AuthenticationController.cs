using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.EmailService;
using LitteraCore.Common.OTP;
using LitteraCore.Common.SmsService;
using LitteraCore.Common.Token;
using LitteraCore.DBContext;
using LitteraCore.Models; 
using LitteraCore.Models.SmsSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using static LitteraCore.Common.CommonEnum;
using static LitteraCore.Models.Firebase;
using static System.Net.WebRequestMethods;

namespace LitteraCore.Controllers
{
   
    [ApiController]

    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        
        private readonly OtpManager _otpManager;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly IEmailService _mailService;
        private readonly AppAuthService _authService;
        private readonly AuthCookieService _authCookieService;

        public AuthenticationController(
            IConfiguration configuration,
            OtpManager otpManager,
            ISmsService smsService,
            IEmailService emailService,
            AppAuthService authService,
            AuthCookieService authCookieService)
        {
            _configuration = configuration;
            _otpManager = otpManager;
            _smsService = smsService;
            _mailService = emailService;
            _authService = authService;
            _authCookieService = authCookieService;
           
        }



        //[HttpGet]
        //[Route("api/Login")]
        //public IActionResult Login()
        //{

        //    List<User> user = new List<User>();
        //    LoginDB lDB = new LoginDB(_configuration);
        //    user = lDB.Get_User_Details("prince@prathamsoft.com");
        //    if (user.Count > 0)
        //    {
        //        foreach (User u in user)
        //        {
        //            //if (VerifyPassword(u.password, logindetails.Salt, logindetails.Password))
        //            //{
        //            //    HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.OK, true);
        //            //    return response1;
        //            //}
        //            //else
        //            //{
        //            //    HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.Unauthorized, false);
        //            //    return response1;
        //            //}
        //        }
        //    }
        //    else
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //        response.Content = new StringContent("false");
        //        return new ContentResult
        //        {
        //            Content = response.Content.ReadAsStringAsync().Result,
        //            StatusCode = (int)response.StatusCode,
        //            ContentType = "application/json" // Assuming response content type is JSON
        //        };
        //    }

        //    HttpResponseMessage response1 = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //    response1.Content = new StringContent("false");
        //    return new ContentResult
        //    {
        //        Content = response1.Content.ReadAsStringAsync().Result,
        //        StatusCode = (int)response1.StatusCode,
        //        ContentType = "application/json" // Assuming response content type is JSON
        //    };

        //}


        [AllowAnonymous]
        [HttpPost]
        [Route("api/GetToken")]
        [SwaggerOperation("To generate token.")]
        public IActionResult GetToken([FromBody]UserLogin u)
        {
            try
            {
               
                if (u.OTP != null)
                {
                    string username = "";

                    if (u.Mobileno != null)
                    {
                        username = u.Mobileno.ToString();

                    }
                    else
                    {
                        username = u.emailid.ToString();

                    }
                    //Code to OTP Login
                    var isValid = _otpManager.VerifyOtpAsync(username, u.OTP.ToString());

                    if (Convert.ToBoolean(isValid.Result))
                    {
                        var auth = _authService;
                        var token = auth.Authenticate(username);
                        _authCookieService.Append(
                            Response,
                            token.Result.AuthToken);
                        AuthDB adb = new AuthDB(_configuration);
                        string ip = GetClientIp();
                        adb.Make_Login_Entry(token.Result.userdetails.userid, "0", ip);
                        return Ok(token);
                    }
                    else
                    {
                        return Unauthorized("Invalid Otp");
                    }

                }
                else
                {
                    var auth = _authService;
                    AuthDB adb = new AuthDB(_configuration);
                    //Code to password Login
                    List<User> lU = new List<User>();

                    string username = "";

                    if (u.Mobileno != null)
                    {
                        username = u.Mobileno.ToString();

                    }
                    else
                    {
                        username = u.emailid.ToString();

                    }
                    lU = adb.GET_LOGIN_DETAIL(username);
                    if (lU.Count > 0)
                    {
                        if (auth.VerifyPassword(lU.FirstOrDefault().password, u.salt, u.Password) == true)
                        {

                            var token = auth.Authenticate(username);
                            _authCookieService.Append(
                                Response,
                                token.Result.AuthToken);
                            
                            string ip = GetClientIp();
                            adb.Make_Login_Entry(token.Result.userdetails.userid, "0", ip);
                            return Ok(token);
                        }
                        else
                        {
                            //Code to update loginAttempt
                            AuthDB ADB = new AuthDB(_configuration);
                            UserInfo U = ADB.GetUserInfo(username, lU.FirstOrDefault().loginattempt.ToString());
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        return NotFound("User not found.");
                    }



                }

                return Unauthorized("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }




        }



        [AllowAnonymous]
        [HttpGet]
        [Route("api/GenerateOTP")]
        [SwaggerOperation("To Generate and send OTP.")]
        public async Task<IActionResult> GenerateMobileOTP(string username,int utilityOTP=0)
        {
            //Check Valid User
            var auth = _authService;
            AuthDB adb = new AuthDB(_configuration);
            //List<User> lU = new List<User>();
            UserInfo lU = adb.GetUserInfo(username);

          


            //Get the OTP from APi and return it back
            if (lU.Mobileno==null && lU.emailid==null)
            {
                return Unauthorized();
            }
            else
            {
                ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
                OTP_LOGIN_REQUIRED_SETTING ml = new OTP_LOGIN_REQUIRED_SETTING();
                DataTable dt = a.Get_Application_Setting("6");
                ml = JsonConvert.DeserializeObject<OTP_LOGIN_REQUIRED_SETTING>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();

                if (utilityOTP == 1)
                {
                    ml.OTP_ON_SMS = "1";
                    ml.OTP_ON_MAIL = "1";
                }

                var otp = await _otpManager.GenerateOtpAsync(username.ToString());
                var otpid = await _otpManager.GenerateOtpID();
                if (otp != null)
                {
                    SmsTemplate template = new SmsTemplate();
                    template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                    string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);
                    if (lU.Mobileno != null)
                    {
                        if (ml.OTP_ON_SMS == "1")
                        {
                            await _smsService.SendSmsAsync(lU.Mobileno.ToString(), msg, template.TemplateID);
                        }
                      
                    }
                    if (lU.emailid != null)
                    {
                        try
                        {
                            if (ml.OTP_ON_MAIL == "1")
                            {
                                SmtpEmailService s = new SmtpEmailService(_configuration);
                                await s.SendEmailAsync(lU.emailid, "OTP Details", msg);
                            }
                           
                        }
                        catch (Exception ex)
                        {

                        }
                      
                    }

                    return Ok(new {otp= otp, userid= lU.userid,agencyid=lU.agencyid});

                }
                else
                {
                    return Unauthorized();
                }
            }
          

            return Unauthorized();
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/GenerateOTP_wk")]
        [SwaggerOperation("To Generate and send OTP.")]
        public async Task<IActionResult> GenerateOTP_wk(string username, int utilityOTP = 0)
        {
            //Check Valid User
            var auth = _authService;
            AuthDB adb = new AuthDB(_configuration);
            //List<User> lU = new List<User>();
            UserInfo lU = adb.GetUserInfo(username);




            //Get the OTP from APi and return it back
            if (lU.Mobileno == null && lU.emailid == null)
            {
                return Unauthorized();
            }
            else
            {
                ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
                OTP_LOGIN_REQUIRED_SETTING ml = new OTP_LOGIN_REQUIRED_SETTING();
                DataTable dt = a.Get_Application_Setting("6");
                ml = JsonConvert.DeserializeObject<OTP_LOGIN_REQUIRED_SETTING>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();

                if (utilityOTP == 1)
                {
                    ml.OTP_ON_SMS = "1";
                    ml.OTP_ON_MAIL = "1";
                }

                var otp = await _otpManager.GenerateOtpAsync(username.ToString());
                var otpid = await _otpManager.GenerateOtpID();
                if (otp != null)
                {
                    SmsTemplate template = new SmsTemplate();
                    template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                    string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);
                    if (lU.Mobileno != null)
                    {
                        if (ml.OTP_ON_SMS == "1")
                        {
                            await _smsService.SendSmsAsync(lU.Mobileno.ToString(), msg, template.TemplateID);
                        }

                    }
                    if (lU.emailid != null)
                    {
                        try
                        {
                            if (ml.OTP_ON_MAIL == "1")
                            {
                                SmtpEmailService s = new SmtpEmailService(_configuration);
                                await s.SendEmailAsync(lU.emailid, "OTP Details", msg);
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    return Ok(new { otp = otp, userid = lU.userid, agencyid = lU.agencyid });

                }
                else
                {
                    return Unauthorized();
                }
            }


            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/VerifyOTP")]
        [SwaggerOperation("To verify OTP .")]
        public async Task<IActionResult> VerifyOTP(string username,string otp)
        {
            //Check Valid User
        

            var isValid = _otpManager.VerifyOtpAsync(username, otp);

            if (Convert.ToBoolean(isValid.Result))
            {
            
                return Ok(true);
            }
            else
            {
                return Unauthorized("Invalid Otp");
            }


            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/VerifyOTP_wk")]
        [SwaggerOperation("To verify OTP .")]
        public async Task<IActionResult> VerifyOTP_wk(string username, string otp)
        {
            //Check Valid User


            var isValid = _otpManager.VerifyOtpAsync(username, otp);

            if (Convert.ToBoolean(isValid.Result))
            {

                return Ok(true);
            }
            else
            {
                return Unauthorized("Invalid Otp");
            }


            return Unauthorized();
        }



        [Route("api/Rights")]
        [SwaggerOperation("To get rights details.")]
        [HttpGet]
        public IActionResult Rights()
        {
            List<Usertype> lu = new List<Usertype>();
            foreach (int i in Enum.GetValues(typeof(Common.CommonEnum.Rights)))
            {
                Usertype u = new Usertype();
                u.id = i.ToString();
                u.name = Enum.GetName(typeof(Common.CommonEnum.Rights), i);
                lu.Add(u);


            }

           
            return Ok(lu);
        }

        //[Route("api/CheckPermission")]
        //[HttpGet]
        //public IActionResult CheckPermission(string usertype, string userid, [FromQuery]  string chkpermission = null, [FromQuery] string formid = null)
        //{
        //    List<UserPermission> lu = new List<UserPermission>();
        //    LoginBL UBL = new LoginBL(_configuration);
        //    lu = UBL.Check_Permisiion(chkpermission, usertype, userid, formid);

        //    return Ok(lu);
        //}

        [Route("api/CheckPermission")]
        [SwaggerOperation("To check user's permission.")]
        [HttpGet]
        public IActionResult CheckPermission(
    [FromQuery] string usertype,
    [FromQuery] string userid,
    [FromQuery] string? chkpermission = null,
    [FromQuery] string? formid = null)
        {
            if (string.IsNullOrEmpty(usertype) || string.IsNullOrEmpty(userid))
            {
                return BadRequest("Usertype and userid are required.");
            }

            try
            {
                List<UserPermission> userPermissions = new List<UserPermission>();
                LoginBL loginBL = new LoginBL(_configuration);
                userPermissions = loginBL.Check_Permisiion(chkpermission, usertype, userid, formid);

                return Ok(userPermissions);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework like Serilog, NLog, etc.)
                // _logger.LogError(ex, "An error occurred while checking permissions");

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }




        [HttpPost]
        [Route("api/UpdatePassword")]
        [SwaggerOperation("To update user's password.")]
        public IActionResult UpdatePassword(Update_Password u)
        {
            //Code to check old password
            var auth = _authService;
            List<User> lU = new List<User>();
            AuthDB adb = new AuthDB(_configuration);
            lU = adb.GET_LOGIN_DETAIL(u.username);
            if (lU.Count > 0)
            {
                string decryptedpass = YEncryptDecryptData.YEncryptDecryptData.Decrypt(lU.FirstOrDefault().password, true);
                if (decryptedpass==u.password)
                {

                   return BadRequest("Old and new password should be different.");
                }
            }


                u.password = YEncryptDecryptData.YEncryptDecryptData.Encrypt(u.password, true);
            adb.Change_Password(u.userid, u.password); 
            return Ok(true);



        }
        [HttpPost]
        [Route("api/UnblockPassword")]
        [SwaggerOperation("To unblock particular user.")]
        public IActionResult UnblockPassword(string userid)
        {
            AuthDB adb = new AuthDB(_configuration);
            adb.Unblock_Password(userid);
            return Ok(true);



        }



        [HttpGet]
        [Route("api/UserInfo")]
        [SwaggerOperation("To get particular user info.")]
        public IActionResult UserInfo(string username)
        {
            try
            {
                if (!IsCurrentIdentity(username))
                {
                    return Forbid();
                }

                var token = _authService.Authenticate(username);
                _authCookieService.Append(
                    Response,
                    token.Result.AuthToken);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }




        }

        [HttpGet]
        [Route("api/UserInfo_wk")]
        [SwaggerOperation("To get particular user info.")]
        public IActionResult UserInfo_wk(string username)
        {
            try
            {
                if (!IsCurrentIdentity(username))
                {
                    return Forbid();
                }

                var token = _authService.Authenticate(username);
                _authCookieService.Append(
                    Response,
                    token.Result.AuthToken);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }




        }




        [HttpGet]
        [Route("api/GenerateOAuthToken")]
        [SwaggerOperation("To generate token .")]
        public async Task<IActionResult> GenerateOAuthToken(string username)
        {
            if (!IsCurrentIdentity(username))
            {
                return Forbid();
            }

            var token = _authService.Authenticate(username).Result.AuthToken;
            var isValid = _otpManager.SetOauthToken(username,token.ToString());


            return Ok(token);
        }

        [HttpGet]
        [Route("api/CheckOAuthToken")]
        [SwaggerOperation("To validate token.")]
        public async Task<IActionResult> CheckOAuthToken(string username)
        {
            //Check Valid User
            var auth = _authService;
            
            var isValid = _otpManager.CheckOauthToken(username);


            return Ok(isValid);
        }



        [HttpPost]
        [Route("api/SET_PRINT_DATA")]
        [SwaggerOperation("To set print data for APP purpose .")]
        public async Task<Boolean> SET_PRINT_DATA(string id, [FromBody] PrintData data)
        {
            //Check Valid User
            var otp = await _otpManager.Set_Print_Data(id, data);


            return true;
        }
        [HttpGet]
        [Route("api/GET_PRINT_DATA")]
        [SwaggerOperation("To get print data for APP purpose .")]
        public async Task<string> GET_PRINT_DATA(string id)
        {
            //Check Valid User
            var otp = await _otpManager.Get_Print_Data(id);


            return otp;
        }

        [HttpPost]
        [Route("api/SAVE_PARTICIPANT_CONTENT_STATUS")]
        [SwaggerOperation("To save participant content status.")]
        public async Task<string> SAVE_PARTICIPANT_CONTENT_STATUS(string userid,string contenid, [FromBody] dynamic jsonContent)
        {
            string jsonString = jsonContent.ToString();


            return userid+"*"+ contenid;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/GET_REACT_APP_CONFIGURATION")]
        [SwaggerOperation("To get react app configuration from config.json.")]
        public IActionResult GET_REACT_APP_CONFIGURATION()
        {
           REACT_APP_CONFIGURATION RAC = new REACT_APP_CONFIGURATION();

            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Config.json"));
            RAC = JsonConvert.DeserializeObject<REACT_APP_CONFIGURATION>(jsontxt);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // This preserves the original property names
            };

            // Return the object using System.Text.Json with custom settings
            return new JsonResult(RAC, options); 
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/GET_REACT_APP_CONFIGURATION_wk")]
        [SwaggerOperation("To get react app configuration from config.json.")]
        public IActionResult GET_REACT_APP_CONFIGURATION_wk()
        {
            REACT_APP_CONFIGURATION RAC = new REACT_APP_CONFIGURATION();

            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Config.json"));
            RAC = JsonConvert.DeserializeObject<REACT_APP_CONFIGURATION>(jsontxt);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // This preserves the original property names
            };

            // Return the object using System.Text.Json with custom settings
            return new JsonResult(RAC, options);
        }

        [HttpPost]
        [Route("api/FirebaseToken")]
        [SwaggerOperation("To get firebase token.")]
        public IActionResult FirebaseToken(string agencyid, string token)
        {
            LoginDB ldb = new LoginDB(_configuration);
            bool isUpdated = ldb.Insert_Firebase_Token(agencyid, token);
            // Return the object using System.Text.Json with custom settings
            return  Ok(isUpdated);
        }

        [HttpPost]
        [Route("api/BulkUpdatePassword")]
        [SwaggerOperation("To update password in bulk.")]
        public IActionResult BulkUpdatePassword(userlist ul)
        {
            AuthDB adb=new AuthDB(_configuration);
            DataTable dtusers = adb.Get_User_Agency_Data(ul);

            foreach (update_pass u in ul.users)
            {
                string userid = u.userid;
                dtusers.DefaultView.RowFilter = "tyuam_userid='"+u.userid+"'";
                DataTable dt = dtusers.DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        if (Convert.ToString(dt.Rows[0]["ag_dob"]) != "")
                        {
                            string password = Convert.ToDateTime(dt.Rows[0]["ag_dob"]).ToString("yyyy/MM/dd").Replace("/", "").Replace("-", "");
                            string hp = AuthDB.GetMD5Hash(password);
                            string finaldata = YEncryptDecryptData.YEncryptDecryptData.Encrypt(hp, true);
                            u.password = finaldata;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                   
                  
                }
               

            }
            AuthDB abd = new AuthDB(_configuration);

            bool issaved = abd.Update_bulk_password(ul);


            return Ok(issaved);
        }

        [HttpGet]
        [Route("api/Is_Password_Changed")]
        [SwaggerOperation("To check is user's password changed.")]
        public IActionResult Is_Password_Changed(string userid)
        {
            try
            {
                var auth = _authService;
                AuthDB ADB = new AuthDB(_configuration);
                bool ischanged = ADB.is_password_changed(userid);
                return Ok(ischanged);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }



        [HttpPost]
        [Route("api/password_updated")]
        [SwaggerOperation("To update password updated status only.")]
        public IActionResult password_updated(string userid)
        {
            AuthDB ADB = new AuthDB(_configuration);
            bool ischanged = ADB.Password_Updated(userid);
            return Ok(ischanged);
        }



        [HttpGet]
        [Route("api/Send_OTP")]
        [SwaggerOperation("To Send OTP.")]
        public async Task<IActionResult> Send_OTP(string username)
        {
            //Check Valid User
            var auth = _authService;
            AuthDB adb = new AuthDB(_configuration);
            //List<User> lU = new List<User>();
            UserInfo lU = adb.GetUserInfo(username);






            //Get the OTP from APi and return it back
            if (lU.Mobileno == null && lU.emailid == null)
            {
                ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
                OTP_LOGIN_REQUIRED_SETTING ml = new OTP_LOGIN_REQUIRED_SETTING();
                DataTable dt = a.Get_Application_Setting("6");
                ml = JsonConvert.DeserializeObject<OTP_LOGIN_REQUIRED_SETTING>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();

                var otp = await _otpManager.GenerateOtpAsync(username.ToString());
                var otpid = await _otpManager.GenerateOtpID();
                SmsTemplate template = new SmsTemplate();
                template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);
                if (username.Contains("@") != false)
                {
                    if (ml.OTP_ON_SMS == "1")
                    {
                        await _smsService.SendSmsAsync(lU.Mobileno.ToString(), msg, template.TemplateID);
                    }

                }
                else
                {
                    try
                    {
                        if (ml.OTP_ON_MAIL == "1")
                        {
                            SmtpEmailService s = new SmtpEmailService(_configuration);
                            await s.SendEmailAsync(lU.emailid, "OTP Details", msg);
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }

                   return Ok(new { otp = otp, userid = "" });


            }
            else
            {
                ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
                OTP_LOGIN_REQUIRED_SETTING ml = new OTP_LOGIN_REQUIRED_SETTING();
                DataTable dt = a.Get_Application_Setting("6");
                ml = JsonConvert.DeserializeObject<OTP_LOGIN_REQUIRED_SETTING>(dt.Rows[0]["SettingValue"].ToString());
                ml.settingid = dt.Rows[0]["SettingID"].ToString();

                var otp = await _otpManager.GenerateOtpAsync(username.ToString());
                var otpid = await _otpManager.GenerateOtpID();
                if (otp != null)
                {
                    SmsTemplate template = new SmsTemplate();
                    template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                    string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);
                    if (lU.Mobileno != null)
                    {
                        if (ml.OTP_ON_SMS == "1")
                        {
                            await _smsService.SendSmsAsync(lU.Mobileno.ToString(), msg, template.TemplateID);
                        }

                    }
                    if (lU.emailid != null)
                    {
                        try
                        {
                            if (ml.OTP_ON_MAIL == "1")
                            {
                                SmtpEmailService s = new SmtpEmailService(_configuration);
                                await s.SendEmailAsync(lU.emailid, "OTP Details", msg);
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    return Ok(new { otp = otp, userid = lU.userid });

                }
                else
                {
                    return Unauthorized();
                }
            }


            return Unauthorized();
        }

        [HttpGet]
        [Route("api/Get_Token_Info")]
        [SwaggerOperation("To Get Token Information.")]
        public async Task<IActionResult> Get_Token_Info(string token)
        {
            // Check Valid User
            var auth = _authService;

            var principal = auth.ValidateJwtToken(token); // Validate token and get claims

            if (principal != null)
            {
                var username = principal.FindFirst(ClaimTypes.Name)?.Value; // Extract username claim
                Console.WriteLine("Authenticated username: " + username);
            }
            var simplifiedClaims = principal.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            }).ToList();

            // Using JsonSerializerOptions to prevent circular reference during serialization
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = true  // Optional: Makes the output more readable
            };

            // Serialize the claims to JSON manually to handle circular references
            var claimsJson = System.Text.Json.JsonSerializer.Serialize(principal.Claims.ToList(), jsonOptions);


            // var userid= simplifiedClaims.Where(o=>o.Type== "userid").FirstOrDefault()?.Value;

            //List<Agency> AL = new List<Agency>();
            //AgencyDB ABD = new AgencyDB(_configuration);
            //AL = ABD.Get_Agency(null, userid, 1, 10, null, null, null, null, null);
            //string user_name = "";
            //if (AL.Count > 0)
            //{
            //    simplifiedClaims.Add(new { Type = "username", Value = AL.FirstOrDefault().agencyname });
            //    simplifiedClaims.Add(new { Type = "agencyid", Value = AL.FirstOrDefault().agencyid });
            //    simplifiedClaims.Add(new { Type = "emailid", Value = AL.FirstOrDefault().ag_email });
            //    simplifiedClaims.Add(new { Type = "mobileno", Value = AL.FirstOrDefault().ag_mobileno });
            //    if (AL.FirstOrDefault().ag_mobileno != "" && AL.FirstOrDefault().ag_mobileno != null)
            //    {
            //        user_name = AL.FirstOrDefault().ag_mobileno;
            //    }
            //    else
            //    {
            //        user_name = AL.FirstOrDefault().ag_email;
            //    }
            //}

            //AuthDB audb = new AuthDB(_configuration);
            //UserInfo a= new UserInfo();
            //a = audb.GetUserInfo(user_name);

            ////var userIdClaim = simplifiedClaims.FirstOrDefault(c => c.Type == "userid");
            ////if (userIdClaim != null)
            ////{
            ////    userIdClaim.Value = a.userid;
            ////}
            //var userIdClaim = simplifiedClaims.FirstOrDefault(c => c.GetType().GetProperty("Type")?.GetValue(c)?.ToString() == "userid");

            //if (userIdClaim != null)
            //{
            //    // Get index
            //    int index = simplifiedClaims.IndexOf(userIdClaim);

            //    // Create new item with updated value
            //    var newClaim = new { Type = "userid", Value = a.userid };

            //    // Replace
            //    simplifiedClaims.RemoveAt(index);
            //    simplifiedClaims.Insert(index, newClaim);
            //}



            // Return the serialized claims
            return Ok(simplifiedClaims);
        }


        [HttpGet]
        [Route("api/Get_API_INFO")]
        [SwaggerOperation("To Get API Information.")]
        public async Task<IActionResult> Get_API_INFO(string userid)
        {
            string username = "";
            //Check Valid User
            var auth = _authService;

            bool isValid = _otpManager.CheckOauthToken(username).Result;
            if (isValid == true)
            {

            }
            else
            {
                return Unauthorized();
            }


            return Ok(isValid);
        }



        [HttpGet]
        [Route("api/GenerateActivityToken")]
        [SwaggerOperation("To Generate activity token.")]
        public async Task<IActionResult> GenerateActivityToken(string ttsm_id,string apipath, string? userid=null, string? ttpai_id=null)
        {
            var currentUserId = User.FindFirst("userid")?.Value;
            if (string.IsNullOrWhiteSpace(currentUserId)
                || (!string.IsNullOrWhiteSpace(userid)
                    && !string.Equals(
                        userid,
                        currentUserId,
                        StringComparison.OrdinalIgnoreCase)))
            {
                return Forbid();
            }

            userid = currentUserId;
            var token = _authService.Activity_Token(
                userid,
                ttpai_id,
                ttsm_id,
                apipath).Result.AuthToken;
            var isValid = _otpManager.SetOauthToken(userid, token.ToString());


            return Ok(token);
        }
        [HttpGet]
        [Route("api/GenerateActivityToken_wk")]
        [SwaggerOperation("To Generate activity token.")]
        public async Task<IActionResult> GenerateActivityToken_wk(string ttsm_id, string apipath, string? userid = null, string? ttpai_id = null)
        {
            var currentUserId = User.FindFirst("userid")?.Value;
            if (string.IsNullOrWhiteSpace(currentUserId)
                || (!string.IsNullOrWhiteSpace(userid)
                    && !string.Equals(
                        userid,
                        currentUserId,
                        StringComparison.OrdinalIgnoreCase)))
            {
                return Forbid();
            }

            userid = currentUserId;
            var token = _authService.Activity_Token(
                userid,
                ttpai_id,
                ttsm_id,
                apipath).Result.AuthToken;
            var isValid = _otpManager.SetOauthToken(userid, token.ToString());


            return Ok(token);
        }
        [AllowAnonymous]
        [HttpGet]
        
        [Route("api/Get_Activity_Token_Info")]
        [SwaggerOperation("To Get Token Information.")]
        public async Task<IActionResult> Get_Activity_Token_Info(string token)
        {
            // Check Valid User
            var principal = _authService.ValidateJwtToken(token);

            if (principal != null)
            {
                var userid = principal.FindFirst(ClaimTypes.Name)?.Value; // Extract username claim
                Console.WriteLine("Authenticated username: " + userid);
            }
            var simplifiedClaims = principal.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            }).ToList();

            // Using JsonSerializerOptions to prevent circular reference during serialization
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = true  // Optional: Makes the output more readable
            };

            // Serialize the claims to JSON manually to handle circular references
            var claimsJson = System.Text.Json.JsonSerializer.Serialize(principal.Claims.ToList(), jsonOptions);

            var user_id = simplifiedClaims.Where(o => o.Type == "userid").FirstOrDefault()?.Value;

            List<Agency> AL = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            if(user_id != "")
            {
                AL = ABD.Get_Agency(null, user_id, 1, 10, null, null, null, null, null, "AgencyId,tyaam_status,AgencyName,HAgencyName,ag_email,ag_mobileno,totalrecords");
            }
          
            string user_name = "";
            if (AL.Count > 0)
            {
                simplifiedClaims.Add(new { Type = "username", Value = AL.FirstOrDefault().agencyname });
                simplifiedClaims.Add(new { Type = "agencyid", Value = AL.FirstOrDefault().agencyid });
                simplifiedClaims.Add(new { Type = "emailid", Value = AL.FirstOrDefault().ag_email });
                simplifiedClaims.Add(new { Type = "mobileno", Value = AL.FirstOrDefault().ag_mobileno });
                if (AL.FirstOrDefault().ag_mobileno != "" && AL.FirstOrDefault().ag_mobileno != null)
                {
                    user_name = AL.FirstOrDefault().ag_mobileno;
                }
                else
                {
                    user_name = AL.FirstOrDefault().ag_email;
                }
            }

            AuthDB audb = new AuthDB(_configuration);
            UserInfo a = new UserInfo();
            if(user_name != "")
            {
                a = audb.GetUserInfo(user_name);
            }
          

            //var userIdClaim = simplifiedClaims.FirstOrDefault(c => c.Type == "userid");
            //if (userIdClaim != null)
            //{
            //    userIdClaim.Value = a.userid;
            //}
            var userIdClaim = simplifiedClaims.FirstOrDefault(c => c.GetType().GetProperty("Type")?.GetValue(c)?.ToString() == "userid");

            if (userIdClaim != null)
            {
                // Get index
                int index = simplifiedClaims.IndexOf(userIdClaim);

                // Create new item with updated value
                var newClaim = new { Type = "userid", Value = a.userid };

                // Replace
                simplifiedClaims.RemoveAt(index);
                simplifiedClaims.Insert(index, newClaim);
            }

            return Ok(simplifiedClaims);
        }

        [HttpGet]
        [Route("api/Send_General_OTP")]
        [SwaggerOperation("To Send General OTP.")]
        public async Task<IActionResult> Send_General_OTP(string username)
        {
            //Check Valid User
            var auth = _authService;
            AuthDB adb = new AuthDB(_configuration);
            //List<User> lU = new List<User>();
            var otp = await _otpManager.GenerateOtpAsync(username.ToString());
            var otpid = await _otpManager.GenerateOtpID();
            if (username.Contains("@") == true)
            {
              
                SmsTemplate template = new SmsTemplate();
                template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);

                SmtpEmailService s = new SmtpEmailService(_configuration);
                await s.SendEmailAsync(username, "OTP Details", msg);



            }
            else
            {
              
                SmsTemplate template = new SmsTemplate();
                template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);
                await _smsService.SendSmsAsync(username.ToString(), msg, template.TemplateID);
            }


           

            return Ok(otp);
        }

        [HttpGet]
        [Route("api/VerifyOTPWithLogin")]
        [SwaggerOperation("To Verify login OTP.")]
        public async Task<IActionResult> VerifyOTPWithLogin(string username, string otp,string user_id)
        {
            //Check Valid User
            bool password_changed=false;
            AuthDB adb=new AuthDB(_configuration); 

            var isValid = _otpManager.VerifyOtpAsync(username, otp);

           
            if (Convert.ToBoolean(isValid.Result))
            {
                bool ischanged = adb.is_password_changed(user_id);
                if (ischanged == false)
                {

                    string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

                    // If the application is behind a proxy (like a load balancer), you might need to check the X-Forwarded-For header.
                    if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        clientIp = HttpContext.Request.Headers["X-Forwarded-For"];
                    }


                    //Login Trail Entry
                    adb.Make_Login_Entry(user_id, "0", clientIp);
                    Audit_Trail at = new Audit_Trail
                    {
                        tyat_ip = clientIp,
                        tyat_userid = user_id,
                        tyat_page_name = "Change Password",
                        tyat_event_name = "Onload Change Password",
                        tyat_recordid = "",
                        tyat_createdon = System.DateTime.Now


                    };
                    //Audit Trail 'On Load Change Password'
                    ApplicationConfigBL abl = new ApplicationConfigBL(_configuration);
                    abl.Save_Audit_Trail(at);

                    //Change Password

                    userlist ul = new userlist();
                    List<update_pass> up = new List<update_pass>();
                    up.Add(new update_pass { userid = user_id });
                    ul.users = up;


                    DataTable dtusers = adb.Get_User_Agency_Data(ul);

                    foreach (update_pass u in ul.users)
                    {
                        string userid = u.userid;
                        dtusers.DefaultView.RowFilter = "tyuam_userid='" + u.userid + "'";
                        DataTable dt = dtusers.DefaultView.ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                if (Convert.ToString(dt.Rows[0]["ag_dob"]) != "")
                                {
                                    string password = Convert.ToDateTime(dt.Rows[0]["ag_dob"]).ToString("yyyy/MM/dd").Replace("/", "").Replace("-", "");
                                    string hp = AuthDB.GetMD5Hash(password);
                                    string finaldata = YEncryptDecryptData.YEncryptDecryptData.Encrypt(hp, true);
                                    u.password = finaldata;
                                }
                            }
                            catch (Exception ex)
                            {

                            }


                        }


                    }
                    AuthDB abd = new AuthDB(_configuration);

                    bool issaved = abd.Update_bulk_password(ul);

                    password_changed = adb.Password_Updated(user_id);

                 
                    //Password Change Entry
                    Audit_Trail atpc = new Audit_Trail
                    {
                        tyat_ip = clientIp,
                        tyat_userid = user_id,
                        tyat_page_name = "Change Password",
                        tyat_event_name = "password_updated",
                        tyat_recordid = "",
                        tyat_createdon = System.DateTime.Now


                    };
                    abl.Save_Audit_Trail(atpc);
                }


               
                //Password Updated entry
                return Ok(new { password_changed= password_changed });
            }
            else
            {
                return Unauthorized("Invalid Otp");
            }


            return Unauthorized();
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Login_Fail_Entry")]
        [SwaggerOperation("To Make Login Fail Entry.")]
        public IActionResult Login_Fail_Entry(string username, string? reason=null)
        {
            //Code to check old password

            LoginBL adb = new LoginBL(_configuration);
            bool  issaved = adb.Save_Login_Fail_Entry(username, reason);

            return Ok(issaved);



        }



        [HttpPost]
        [Route("api/Match_Password")]
        [SwaggerOperation("To check same password before update password.")]
        public IActionResult Match_Password(Update_Password u)
        {
            //Code to check old password
            var auth = _authService;
            List<User> lU = new List<User>();
            AuthDB adb = new AuthDB(_configuration);
            lU = adb.GET_LOGIN_DETAIL(u.username);
            if (lU.Count > 0)
            {
                string decryptedpass = YEncryptDecryptData.YEncryptDecryptData.Decrypt(lU.FirstOrDefault().password, true);
                if (decryptedpass == u.password)
                {
                    return Ok(true);
                  
                }
            }



            return Ok(false);



        }


        [HttpPost]
        [Route("api/SAVE_USER_LOG")]
        [SwaggerOperation("To save user log entry.")]
        public IActionResult SAVE_USER_LOG(string userid)
        {
           
            string ip = GetClientIp();
            AuthDB adb = new AuthDB(_configuration);

            if(adb.Make_Login_Entry(userid, null, ip) == true)
            {
                adb.Password_Updated(userid);
            }
          


            return Ok(true);



        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/SAVE_USER_LOG_wk")]
        [SwaggerOperation("To save user log entry.")]
        public IActionResult SAVE_USER_LOG_wk(string userid)
        {

            string ip = GetClientIp();
            AuthDB adb = new AuthDB(_configuration);

            if (adb.Make_Login_Entry(userid, null, ip) == true)
            {
                adb.Password_Updated(userid);
            }



            return Ok(true);



        }

        [HttpGet("clientip")]
        [SwaggerOperation("To Get Client IP.")]
        public string GetClientIp()
        {
            string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // If the application is behind a proxy (like a load balancer), you might need to check the X-Forwarded-For header.
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                clientIp = HttpContext.Request.Headers["X-Forwarded-For"];
            }

            return clientIp;
        }

        [HttpGet("api/CHECK_VIDEO_LINK_EXPIRY")]
        [SwaggerOperation("To check video expiry link.")]
        public IActionResult CHECK_VIDEO_LINK_EXPIRY(string trainingid)
        {
            ApplicationConfigBL abl = new ApplicationConfigBL(_configuration);
            DateTime content_Expiry = abl.Get_Content_Expiry();
            if (System.DateTime.Now> content_Expiry)
            {
                return Ok(true);
            }

            return Ok(false);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/User_Session_Details")]
        [SwaggerOperation("To get user's session details.")]
        public IActionResult User_Session_Details(string SecretKey, int usertype)
        {
            string username = "";
            DashboardBL dbl = new DashboardBL(_configuration);
            if (dbl.validate_external_user_key(SecretKey) != true)
            {
                return Unauthorized();
            }
            if (usertype == (int)CommonEnum.usertype.Admin)
            {
                username = CommonEnum.default_admin;
            }
            else if (usertype == (int)CommonEnum.usertype.CD)
            {
                username = CommonEnum.default_cd;
            }
            else if (usertype == (int)CommonEnum.usertype.FACULTY)
            {
                username = CommonEnum.default_faulty;
            }
            else if (usertype == (int)CommonEnum.usertype.PARTICIPANT)
            {
                username = CommonEnum.default_participant;
            }
            else if (usertype == (int)CommonEnum.usertype.DEPT)
            {
                username = CommonEnum.default_org;
            }
            var token = _authService.Authenticate(username);
            _authCookieService.Append(
                Response,
                token.Result.AuthToken);
            AuthDB adb = new AuthDB(_configuration);



            return Ok(token);
        }

        [HttpPost]
        [Route("api/Logout")]
        [SwaggerOperation("To end the authenticated browser session.")]
        public IActionResult Logout()
        {
            _authCookieService.Delete(Response);
            return Ok(true);
        }

        private bool IsCurrentIdentity(string? requestedIdentity)
        {
            if (string.IsNullOrWhiteSpace(requestedIdentity))
            {
                return false;
            }

            var identityClaims = new[]
            {
                User.FindFirst("username")?.Value,
                User.FindFirst("userid")?.Value,
                User.FindFirst("mobileno")?.Value,
                User.FindFirst("emailid")?.Value,
                User.Identity?.Name
            };

            return identityClaims.Any(value =>
                !string.IsNullOrWhiteSpace(value)
                && string.Equals(
                    value,
                    requestedIdentity,
                    StringComparison.OrdinalIgnoreCase));
        }



    }
}
