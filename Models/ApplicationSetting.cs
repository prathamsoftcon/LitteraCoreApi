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
    }
    public class OTP_LOGIN_REQUIRED_SETTING
    {
        public string settingid { get; set; }
        public int OTP_LOGIN_REQUIRED { get; set; }
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

    }
    public class PaymentGatewaySetting
    {
        public string img { get; set; }
        public string key { get; set; }
        public string secret { get; set; }

    }
}
