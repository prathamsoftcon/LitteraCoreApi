using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.EmailService;
using LitteraCore.Common.OTP;
using LitteraCore.Common.SmsService;
using LitteraCore.Common.Token;
using LitteraCore.DBContext;
using LitteraCore.Models; 
using LitteraCore.Models.SmsSettings;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using static LitteraCore.Common.CommonEnum;
using static System.Net.WebRequestMethods;

namespace LitteraCore.Controllers
{
   
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly OtpManager _otpManager;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly IEmailService _mailService;
        public AuthenticationController(IConfiguration configuration, OtpManager otpManager,ISmsService smsService, IEmailService emailService)
        {
            _configuration = configuration;
            _otpManager = otpManager;
            _smsService = smsService;
            _mailService = emailService;
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


        [HttpPost]
        [Route("api/GetToken")]
        public IActionResult GetToken(UserLogin u)
        {
            try {
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
                        AppAuthService auth = new AppAuthService(_configuration);
                        var token = auth.Authenticate(username);
                        Response.Cookies.Append("token", token.ToString());
                        return Ok(token);
                    }
                    else
                    {
                        return Unauthorized("Invalid Otp");
                    }

                }
                else
                {
                    AppAuthService auth = new AppAuthService(_configuration);
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
                            return Ok(token);
                        }
                        else
                        {
                            //Code to update loginAttempt
                            AuthDB ADB = new AuthDB(_configuration);
                            UserInfo U = ADB.GetUserInfo(username,lU.FirstOrDefault().loginattempt.ToString());
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
               
            }    
          



        }


        [HttpGet]
        [Route("api/GenerateOTP")]
        public async Task<IActionResult> GenerateMobileOTP(string username)
        {
            //Check Valid User
            AppAuthService auth = new AppAuthService(_configuration);
            AuthDB adb = new AuthDB(_configuration);
            //List<User> lU = new List<User>();
            UserInfo lU = adb.GetUserInfo(username);

            //Get the OTP from APi and return it back
            if(lU.Mobileno==null && lU.emailid==null)
            {
                return Unauthorized();
            }
            else
            {
                var otp = await _otpManager.GenerateOtpAsync(username.ToString());
                var otpid = await _otpManager.GenerateOtpID();
                if (otp != null)
                {
                    SmsTemplate template = new SmsTemplate();
                    template = _smsService.GetTemplateMsg(Convert.ToInt32(LitteraCore.Models.SmsSettings.TemplateType.Otp));
                    string msg = template.Message.Replace("(#otp#)", otp).Replace("(#otpid#)", otpid);
                    if (lU.Mobileno != null)
                    {
                        await _smsService.SendSmsAsync(lU.Mobileno.ToString(), msg, template.TemplateID);
                    }
                    if (lU.emailid != null)
                    {
                        try
                        {
                            SmtpEmailService s = new SmtpEmailService(_configuration);
                            await s.SendEmailAsync(lU.emailid, "OTP Details", msg);
                        }
                        catch (Exception ex)
                        {

                        }
                      
                    }

                    return Ok(new {otp= otp, userid= lU.userid});

                }
                else
                {
                    return Unauthorized();
                }
            }
          

            return Unauthorized();
        }

        [HttpGet]
        [Route("api/VerifyOTP")]
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



        [Route("api/Rights")]
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
        public IActionResult UpdatePassword(Update_Password u)
        {
            //Code to check old password
            AppAuthService auth = new AppAuthService(_configuration);
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
        public IActionResult UnblockPassword(string userid)
        {
            AuthDB adb = new AuthDB(_configuration);
            adb.Unblock_Password(userid);
            return Ok(true);



        }




        [HttpGet]
        [Route("api/UserInfo")]
        public IActionResult UserInfo(string username)
        {
            try
            {
                AppAuthService auth = new AppAuthService(_configuration);
                AuthDB ADB = new AuthDB(_configuration);
               // UserInfo U = ADB.GetUserInfo(username);
                var token = auth.Authenticate(username);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }




        }




        [HttpGet]
        [Route("api/GenerateOAuthToken")]
        public async Task<IActionResult> GenerateOAuthToken(string username)
        {
            //Check Valid User
            AppAuthService auth = new AppAuthService(_configuration);
            var token = auth.Authenticate(username).Result.AuthToken;
            var isValid = _otpManager.SetOauthToken(username,token.ToString());


            return Ok(token);
        }

        [HttpGet]
        [Route("api/CheckOAuthToken")]
        public async Task<IActionResult> CheckOAuthToken(string username)
        {
            //Check Valid User
            AppAuthService auth = new AppAuthService(_configuration);
            
            var isValid = _otpManager.CheckOauthToken(username);


            return Ok(isValid);
        }



        [HttpPost]
        [Route("api/SET_PRINT_DATA")]
        public async Task<Boolean> SET_PRINT_DATA(string id, [FromBody] PrintData data)
        {
            //Check Valid User
            var otp = await _otpManager.Set_Print_Data(id, data);


            return true;
        }
        [HttpGet]
        [Route("api/GET_PRINT_DATA")]
        public async Task<string> GET_PRINT_DATA(string id)
        {
            //Check Valid User
            var otp = await _otpManager.Get_Print_Data(id);


            return otp;
        }

        [HttpGet]
        [Route("api/SAVE_PARTICIPANT_CONTENT_STATUS")]
        public async Task<string> SAVE_PARTICIPANT_CONTENT_STATUS(string userid,string contenid)
        {
           


            return userid+"*"+ contenid;
        }


        [HttpGet]
        [Route("api/GET_REACT_APP_CONFIGURATION")]
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

        [HttpPost]
        [Route("api/FirebaseToken")]
        public IActionResult FirebaseToken(string agencyid, string token)
        {
            LoginDB ldb = new LoginDB(_configuration);
            bool isUpdated = ldb.Insert_Firebase_Token(agencyid, token);
            // Return the object using System.Text.Json with custom settings
            return  Ok(isUpdated);
        }

        [HttpPost]
        [Route("api/BulkUpdatePassword")]
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
        public IActionResult Is_Password_Changed(string userid)
        {
            try
            {
                AppAuthService auth = new AppAuthService(_configuration);
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
        public IActionResult password_updated(string userid)
        {
            AuthDB ADB = new AuthDB(_configuration);
            bool ischanged = ADB.Password_Updated(userid);
            return Ok(ischanged);
        }


    }
}




