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

namespace LitteraCore.Controllers
{
   
    public class UserController :  Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("api/CreateUser")]
        [SwaggerOperation("To create new user.")]
        public IActionResult CreateParticipantUser([FromBody] LoginUser user, string APPURL = null)
        {
            UserBL UBL = new UserBL(_configuration);
            if (user.branchid == null)
            {
                user.branchid = CommonEnum.Branchid;
            }
            bool isUserCreationMail = true;
            //variable to check need to send user creation mail or not.
            //*************Code to check already exists user
            //Check mobile
            if (user.mobileno != null)
            {
                Agency amob = new Agency();
                amob = UBL.Check_Mobile(user.mobileno, null, user.agency.AgencyTypeId);
                if (amob.agencyid != null)
                {
                    if (amob.userid.ToString().ToUpper() != user.userid.ToString().ToUpper())
                    {
                        throw new Exception("This mobile no already registerd with another user.");
                    }
                    // if agency already exist then not need to send mail
                    isUserCreationMail = false;

                }
            }





            //Check email
            if (user.emailid != null)
            {
                Agency aemail = new Agency();
                aemail = UBL.Check_Mobile(user.emailid, null, user.agency.AgencyTypeId);

                if (aemail.agencyid != null)
                {
                    if (aemail.userid.ToString().ToUpper() != user.userid.ToString().ToUpper())
                    {
                        throw new Exception("This email id already registerd with another user.");
                    }
                    // if agency already exist then not need to send mail
                    isUserCreationMail = false;
                }
            }


            //***********




            bool issaved = false;
            issaved = UBL.Save_User_Data(user);
            if (issaved == true)
            {
                if (isUserCreationMail == true)
                {
                    if (user.emailid != null)
                    {
                        //Code to send Mail on user creation

                        string mailpassword = "";
                        if (user.password_enc != null)
                        {
                            mailpassword = (YEncryptDecryptData.YEncryptDecryptData.Decrypt(user.password_enc, true));
                        }
                        if (APPURL != null && APPURL != "")
                        {
                            EmailTemplate es = new EmailTemplate();
                            
                            es = SmtpEmailService.Get_EMAIL_TEMPLATE("USERREGISTRATION");

                            string mailsubject = es.subject;
                            string mailtext = es.text;
                            mailsubject = mailsubject.Replace("(#name#)", user.agency.ag_first_name);
                            mailtext = mailtext.Replace("(#name#)", user.agency.ag_first_name);
                            mailtext = mailtext.Replace("(#regname#)", user.agency.ag_first_name);
                            mailtext = mailtext.Replace("(#domain#)", APPURL);
                            mailtext = mailtext.Replace("(#pwd#)", mailpassword);

                         
                            SmtpEmailService s = new SmtpEmailService(_configuration);
                            s.SendEmailAsync(user.emailid, mailsubject, mailtext);


                            ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
                            EMAIL_SEND_BY_APPLICATION ems = new EMAIL_SEND_BY_APPLICATION();
                            DataTable dt = a.Get_Application_Setting("7");
                            ems = JsonConvert.DeserializeObject<EMAIL_SEND_BY_APPLICATION>(dt.Rows[0]["SettingValue"].ToString());
                            if(ems.EMAILSETTING.MAIL_CC_TO != null)
                            {
                                string cctext = "New user" + user.agency.ag_first_name + " has been successfully registered.";
                                s.SendEmailAsync(ems.EMAILSETTING.MAIL_CC_TO, mailsubject, cctext);
                            }

                       
                        }

                    }

                    ////********************
                    ////Code to send SMS
                    //if (user.mobileno != null)
                    //{
                    //    if (APPURL != null && APPURL != "")
                    //    {
                    //        var request = (HttpWebRequest)WebRequest.Create(APPURL + "/TrainingAPI/Get_XML_SMS_TEMPLATE?APIKEY=" + Common.StaticData.APPKEY + "&messageid=11");
                    //        var response = (HttpWebResponse)request.GetResponse();
                    //        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    //        JObject j = JObject.Parse(responseString);
                    //        SMSTemplate st = j.ToObject<SMSTemplate>();

                    //        //  Hashtable td = JsonConvert.SerializeObject(responseString);

                    //        string finalmessage = "";

                    //        finalmessage = st.text.ToString().Replace("{#var#}", APPURL);



                    //        SMSSetting s = new SMSSetting();

                    //        var request2 = (HttpWebRequest)WebRequest.Create(APPURL + "/TrainingAPI/Get_XML_SMS_SETTING?APIKEY=" + Common.StaticData.APPKEY + "");
                    //        var response2 = (HttpWebResponse)request2.GetResponse();
                    //        var responseString2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                    //        // var p = JsonConvert.SerializeObject(responseString);
                    //        JObject j1 = JObject.Parse(responseString2);
                    //        s = j1.ToObject<SMSSetting>();
                    //        CommonDB c = new CommonDB();
                    //        string smsapi = c.Get_SMS_API_URL(0);
                    //        if (smsapi != "")
                    //        {
                    //            c.sendSMS(user.mobileno, finalmessage, st.DLT_CT_ID.ToString(), smsapi);
                    //        }

                    //    }

                    //}
                }


                //*******************

                return Ok(true);
            }
            else
            {
                return BadRequest(true);
            }
            //return Ok(true);



        }

    }
}
