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
}
