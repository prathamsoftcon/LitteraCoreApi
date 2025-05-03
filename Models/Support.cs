namespace LitteraCore.Models
{
    public class Support
    {
        public string id { get; set; }

        public int referenceno { get; set; }
        public string clienturl { get; set; }
        public string name { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string description { get; set; }

        public string upload_path { get; set; }
        public string createdon { get; set; }
        public string createdby { get; set; }

        public int replied { get; set; }
        public string reply_txt { get; set; }
        public string reply_by { get; set; }

        public string repliedOn { get; set; }

    }
    public class Update_Support
    {
        public string id { get; set; }

        public int status { get; set; }
        public string remark { get; set; }
        public string reply_by { get; set; }

        
    }
    public class Login_Failed_User
    {
        public string tyflu_id { get; set; }

        public string tyflu_username { get; set; }
        public string tyflu_createdon { get; set; }
        public string tyflu_reason { get; set; }
        public int total { get; set; }


    }

    public class Support_Analytical_Report
    {
        public string userid { get; set; }

        public string agencyname { get; set; }
        public string username { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string eventdate { get; set; }
        public int total { get; set; }

    }
    public class Learning_Time
    {
        public string tplt_Id { get; set; }

        public string tplt_ttsam_id { get; set; }
        public string tplt_ttpai_id { get; set; }
        public decimal tplt_learning_time { get; set; }
        public string tplt_createdon { get; set; }
        public string tplt_createdby { get; set; }

        public string GlobalContentTitle { get; set; }
        public string GlobalContentyTypeID { get; set; }
        public string Participantid { get; set; }
        public string AgencyName { get; set; }
        public string ag_email { get; set; }
        public string ag_mobileno { get; set; }
        public int totalrecords { get; set; }



    }

    public class Learning_Report_Data
    {
        public string tplt_ttsam_id { get; set; }
        public string GlobalContentTitle { get; set; }
        public string GlobalContentyTypeID { get; set; }
        public string GlobalContentyType_Name { get; set; }
        public string Participantid { get; set; }
        public string AgencyName { get; set; }
        public string ag_email { get; set; }
        public string ag_mobileno { get; set; }
        public decimal learningtime { get; set; }
        public int totalrecord { get; set; }
        public string trainingid { get; set; }
        public string trainingname { get; set; }

    }


}
