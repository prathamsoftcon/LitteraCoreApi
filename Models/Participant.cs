namespace LitteraCore.Models
{
    public class Participant
    {
        public string ParticipantId { get; set; }

        public string ParticipantType { get; set; }
        public string ParticipantName { get; set; }
        public string HParticipantName { get; set; }
        public string TrainingId { get; set; }

        public string DesignationName { get; set; }
        public string OfficeAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string OfficePhoneNo { get; set; }


        public string ResidencialPhoneNo { get; set; }
        public decimal? Age { get; set; }
        public string Qualification { get; set; }
        public string DurationInGovtJob { get; set; }
        public string SpecialSkillAreas { get; set; }
        public Decimal? BasicPay { get; set; }

        public int? Is_IAS_IPS_Officer { get; set; }
        public int? Is_With_Spouse { get; set; }

        public string BranchId { get; set; }

        public string SponsorID { get; set; }
        public string t_Name { get; set; }

        public string TrainingCode { get; set; }
        public string Sposnorname { get; set; }
        public string hSposnorname { get; set; }
        public string Training_SponsorType { get; set; }

        public string OfficeAddress1 { get; set; }
        public string OfficeAddress2 { get; set; }
        public string E_Department { get; set; }
        public string H_Department { get; set; }


        public string mobileno { get; set; }
        public string email { get; set; }
        public string gender { get; set; }

        public string gendername { get; set; }

        public string castname { get; set; }


        public string salutation { get; set; }
        public string F_NAME { get; set; }
        public string M_NAME { get; set; }

        public string L_NAME { get; set; }
        public int? WITHCHILD { get; set; }
        public string CURRENTLOCATION { get; set; }
        public string NOOFPERSON { get; set; }
        public int? Is_Bhopal { get; set; }

        public string ts_hname { get; set; }
        public string ts_name { get; set; }
        public string isnodues { get; set; }
        public string feedbackstatus { get; set; }

        public int? is_deleted { get; set; }
        public int? is_approve { get; set; } = 0;
        public string uploadpath { get; set; }
        public string remark { get; set; }

        public DateTime? dob { get; set; }

        public string photopath { get; set; }
        public string photofullpath;

        public string photopath_full { get { return this.photofullpath; } set { photofullpath = get_path(photopath); } }
        public string participat_master_remark { get; set; }
        public string ag_gstin { get; set; }

        public string Ag_Address { get; set; }
        public string ag_address_city { get; set; }
        public string ag_address_state { get; set; }
        public string ag_pincode { get; set; }

        public string ag_alternative_mobileno { get; set; }
        public string usercode { get; set; }
        public string tyaam_val { get; set; }
        public DateTime? ttpai_trg_start_date { get; set; }

        public DateTime? ttpai_trg_end_date { get; set; }
        public string ttpai_is_specific { get; set; }
        public string UserName { get; set; }

        public string status_txt { get; set; }

        public string ttpai_id { get; set; }
        public AgencyAdditionalInfo additionalInfo { get; set; }

        public ParticipantTrainings[] registeredtrg { get; set; }

        public int totalrecords { get; set; }

        public string rcname { get; set; }

        public string scname { get; set; }

        public string ttpai_trg_cert_id { get; set; }

        public Certificate_info ttpai_trg_cert_info { get; set; }

        
        public string get_path(string absouutepath)
        {
            string path = "";
            //if (absouutepath != null && absouutepath.ToString() != "")
            //{
            //    Common.UploadPath up = new Common.UploadPath();
            //    path = up.Get_Agency_Photo_Path() + absouutepath;
            //}
            //else
            //{
            //    Common.UploadPath up = new Common.UploadPath();
            //    path = up.Get_Agency_Default_Photo_Path();
            //}
            return path;
        }
        public class ParticipantTrainings
        {
            public string trainingid { get; set; }
            public string ttpai_id { get; set; }

        }
    }
    public class not_eligible_participant
    {
        public string participantid { get; set; }
        public string name { get; set; }
        public string mobileno { get; set; }
        public string emilid { get; set; }
    }
    public class certificate_obj
    {
        public string participantid { get; set; }
        public string ttpai_id { get; set; }
        public string CertId { get; set; }

        public string CertInfo { get; set; }

        public string participantname { get; set; }
        public string trainingcode { get; set; }
        public string t_name { get; set; }

        public Certificate_info Certificate_Info { get; set;}
    }

    public class Certificate_info 
    {
        public string certificate_id { get; set; }
        public string certificate_dt { get; set; }

        public string created_by { get; set; }
    }
    public class Certificate_info_List
    {
        public certificate_obj[] Certificate_info { get; set; }
    }

    public class ParticipantAdditionlInfo
    {
        public string Participantid { get; set; }

        public string TrainingId { get; set; }
        public string Sponsorid { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string remark { get; set; }
        public string uploadpath { get; set; }

        public string photopath_full { get; set; }

        public string ttpai_trg_start_date { get; set; }

        public string ttpai_trg_end_date { get; set; }

        public string ttpai_is_specific { get; set; }

        public string ttpai_id { get; set; }
        //public string tpai_id { get; set; }

        public string registration_status { get; set; }
    }
     public class learningtime
     {
        public string tplt_Id { get; set; }
      
        public string tplt_ttsam_id { get; set; }
        public string tplt_ttpai_id { get; set; }
        public int tplt_learning_time { get; set; }

        public string tplt_createdon { get; set; }
        public string tplt_createdby { get; set; }

        public string tplt_sessionid { get; set; }
    }


}
