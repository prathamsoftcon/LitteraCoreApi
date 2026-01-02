using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LitteraCore.Models
{
    public class ApplicationSetting
    {
   
    }
    public class MobileLogin
    {
        public string MOBILE_AS_USER { get; set; }
        public string SMSAPI { get; set; }
        public string EMAIL_MANDATORY { get; set; }

        public string MOBILE_MANDATORY { get; set; }
    }
    public class ParticipantApproval
    {
        public int REG_AUTO_APPROVAL { get; set; }
        public int REG_APPROVAL_STATUS { get; set; }
        public int ENROLL_AUTO_APPROVAL { get; set; }

        public int ENROLL_APPROVAL_STATUS { get; set; }
    }
    public class FEEDBACK_OTP_SETTING
    {
        public int FEEDBACK_OTP_REQUIRED { get; set; }

    }
    public class RegistrationField
    {
        public int PASSWORD_ON_REGISTRATION { get; set; }
        public int OTP_REQUIRED_ON_OUTSIDE_REGISTRATION { get; set; } = 1;
        public int MOBILE_REQUIRED {  get; set; }
        public int EMAIL_REQUIRED { get; set; }
    }
    public class OTP_LOGIN_REQUIRED_SETTING
    {
        public string settingid { get; set; }
        public int OTP_LOGIN_REQUIRED { get; set; }

        public string OTP_ON_MAIL { get; set; }

        public string OTP_ON_SMS { get; set; }
        public string SMSAPI { get; set; }

        public SMSTEMPLATE smstemplate { get; set; }

        public EMAILSETTING EMAILSETTING { get; set; }

    }
    public class SMSTEMPLATE
    {
        public string ID { get; set; }
        public string DLT_CT_ID { get; set; }
        public string text { get; set; }
    }
    public class EMAIL_SEND_BY_APPLICATION
    {
        public string settingid { get; set; }
        public int IS_MAIL_SEND { get; set; }
        public EMAILSETTING EMAILSETTING { get; set; }

    }
    public class SMS_SEND_BY_APPLICATION
    {
        public string settingid { get; set; }
        public int IS_SMS_SEND { get; set; }
        public string SMSAPI { get; set; }


    }
    public class EMAILSETTING
    {
        public string EMAILID { get; set; }
        public string PWD { get; set; }
        public string HOST { get; set; }
        public string PORT { get; set; }
        public string MAIL_CC_TO { get; set; }

    }
    public class PaymentGatewaySetting
    {
        public string img { get; set; }
        public string key { get; set; }
        public string secret { get; set; }

    }

    public class REACT_APP_CONFIGURATION
    {
        public string REACT_APP_API_URL { get; set; }
        public string REACT_APP_API_KEY { get; set; }
        public string REACT_APP_ID { get; set; }
        public string LITTERA_CDN_BASE_URL { get; set; }

        public string LITTERA_SUPPORT_PATH { get {

                return "Training_Upload/Support";
            
            } }
        public string LITTERA_CONTENT_PATH { get; set; }
        public string LITTERA_CONTENT_THUMBNAIL_PATH
        {
            get
            {
                // Return LITTERA_CONTENT_PATH with "/Thumbnails" appended.
                return string.IsNullOrEmpty(LITTERA_CONTENT_PATH) ? string.Empty : LITTERA_CONTENT_PATH.Replace("Content", "") + "/Thumbnails/";
            }
        }
        public string LITTERA_ASSIGNMENT_PATH { get; set; }
        public string LITTERA_CDN_PROFILE_PICK_PATH { get; set; }
        public string LITTERA_CDN_FUNCTION_IMG_PATH { get; set; }

        public string LITTERA_CDN_ASSET_FLAG_PATH { get; set; }
        public string REACT_APP_SURVEY_API_PATH { get; set; }
        public string REACT_APP_SURVEY_API_KEY { get; set; }

        public string REACT_APP_WHITEBOARD_URL { get; set; }
        public string REACT_APP_API_URL_Google { get; set; }
        public string REACT_APP_EVAL_API_PATH { get; set; }
        public string REACT_APP_EVAL_API_KEY { get; set; }

        public string REACT_APP_IDLE_TIMEOUT { get; set; }
        public string REACT_URL_SHORTNER_PATH { get; set; }
        public string REACT_URL_SHORTNER_KEY { get; set; }
        public string REACT_CONFERENCE_API_PATH { get; set; }

        public string REACT_CONFERENCE_API_KEY { get; set; }
        public string GOOGLE_ANALYTICS_MEGER_ID { get; set; }

        public string FEEDBACK_RATING_TYPE { get; set; }

        public string _comment_FEEDBACK_RATING_TYPE { get; set; }

        public string REACT_CDN_PDF_URL { get; set; }
        public string REACT_CDN_PDF_URL_KEY { get; set; }

        public string REACT_APP_LOGOUT_PATH { get; set; }
        public string REACT_PYTHON_API_BASE_PATH { get; set; }

      
    }
    public class maildetails
    {
        public string recipientEmail { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
      
    }

    public class bulk_maildetails
    {
        public string[] recipientEmail { get; set; }
        public string subject { get; set; }
        public string message { get; set; }

    }
    public class Audit_Trail
    {
        public string tyat_userid { get; set; }
        public string tyat_page_name { get; set; }
        public string? tyat_event_name { get; set; }
        public string? tyat_recordid { get; set; }
        public DateTime? tyat_createdon { get; set; }
        public string? tyat_ip { get; set; }

        public client_device_info? device_Info { get; set; }

    }
    public class client_device_info
    {
        public string userAgent { get; set; }
        public string platform { get; set; }
        public string language { get; set; }
        public string device_width { get; set; }
        public string device_height { get; set; }
    }
    public class Error_Log
    {
        public string tyel_userid { get; set; }
        public string tyel_page_name { get; set; }
        public string? tyel_event_name { get; set; }
        public string tyel_error { get; set; }
        public string? tyel_recordid { get; set; }
        public DateTime? tyel_createdon { get; set; }
        public string? tyel_ip { get; set; }

    }
    public class Branch_Configuration
    {
        public int branch_selection_required { get; set; }
        public int max_level_allowed { get; set; }
    }
    public class Masking_Setting
    {
        public int data_masking_required { get; set; }

    }

    public class FIRST_LOGIN_CHANGE_PASSWORD_SETTING
    {
       
        public int PASSWORD_CHANGE_REQUIRED { get; set; }
        public int PASSWORD_CHANGE_MANDATORY { get; set; }

    }
}
