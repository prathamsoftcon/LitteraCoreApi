namespace LitteraCore.Models
{
    public class Faculty
    {
        public string Trainingid { get; set; }
        public string Sessionid { get; set; }

        public string ttttt_facultyid { get; set; }

        public string tttttf_status { get; set; }

        public string facultyname { get; set; }
        public string firstname { get; set; }

        public string middlename { get; set; }

        public string lastname { get; set; }

        public string hfacultyname { get; set; }
        public string h_firstname { get; set; }

        public string h_middlename { get; set; }

        public string h_lastname { get; set; }


        public string facultyimgpath { get; set; }

        public string mobileno { get; set; }

        public string phoneno { get; set; }

        public string officephone { get; set; }

        public string email { get; set; }

        public string permanent_address { get; set; }
        public string current_address { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string pincode { get; set; }

        public string Designation { get; set; }

        public string state { get; set; }

        public string city { get; set; }

        public string aadhar { get; set; }

        public string currentorganisation { get; set; }

        public string bank { get; set; }

        public string branch { get; set; }

        public string ifsc { get; set; }

        public string accountno { get; set; }

        public int isinhousefaculty { get; set; }

        public int salutation { get; set; }

        public string salutation_name { get; set; }
        public string salutation_name_hindi { get; set; }

        public string ag_photo_path { get; set; }

        public int agencystatus { get; set; }

        public string DOB { get; set; }

        public int Gender { get; set; }

        public string resumepath { get; set; }

        public FacultySpecialisation[] specialisation { get; set; }

    }
    public class FacultySpecialisation
    {
        public string facultyid { get; set; }

        public string courseid { get; set; }

        public string topicid { get; set; }

        public string TotalExperience { get; set; }

        public string SubSpecialisation { get; set; }
    }
}
