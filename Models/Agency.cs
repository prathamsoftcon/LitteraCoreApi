namespace LitteraCore.Models
{
    public class Agency
    {
        //public string agencyid { get; set; }
        //public string agencyname { get; set; }
        //public string hagencyname { get; set; }
        public string agencyid { get; set; }

        public string tyaam_typeid { get; set; }
        public int tyaam_status { get; set; }
        public string agencyname { get; set; }
        public string hagencyname { get; set; }
        public string AgencyTypeId { get; set; }
        public string Fixed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCode { get; set; }

        public string ParentId { get; set; }
        public string Ag_locationtype { get; set; }
        public string Ag_location { get; set; }
        public string Ag_Address { get; set; }
        public string Ag_Address1 { get; set; }
        public string Ag_StateId { get; set; }
        public string Ag_DistrictId { get; set; }

        public string Ag_BlockId { get; set; }
        public string Ag_GramPanchayatId { get; set; }
        public string ag_divisionid { get; set; }
        public string uploadpath { get; set; }
        public string ag_photo_path { get; set; }
        public string ag_first_name { get; set; }

        public string ag_m_name { get; set; }
        public string ag_l_name { get; set; }
        public string ag_hfirst_name { get; set; }
        public string ag_hm_name { get; set; }
        public string ag_hl_name { get; set; }

        public string ag_address_city { get; set; }
        public string ag_address_state { get; set; }
        public string ag_pincode { get; set; }
        public string ag_phone { get; set; }
        public string ag_alternative_phone { get; set; }
        public string ag_mobileno { get; set; }

        public string ag_alternative_mobileno { get; set; }
        public string ag_email { get; set; }
        public string ag_alternative_email { get; set; }
        public string ag_gender { get; set; }
        public int ag_age { get; set; }

        public string ag_dob { get; set; }

        public string ag_salutation { get; set; }

        public string ag_aadhar { get; set; }

        public string ag_pan { get; set; }
        public string ag_gstin { get; set; }
        public int tyaam_is_deleted { get; set; }
        public string tyaam_val { get; set; }
        public string ag_sign_path { get; set; }

        public string branchid { get; set; }

        public string userid { get; set; }

        public AgencyAdditionalInfo additionalInfo { get; set; }


        public string tyaam_status_text { get; set; }

        public string agency_type_txt { get; set; }


        public string agencystatus { get; set; }
        public string latitude { get; set; }

        public string longitude { get; set; }

        public string gender_text { get; set; }

        public string remark { get; set; }

        public string tdds_tat_type_id { get; set; }

        public userDetails[] userdetail { get; set; }

        public string upload_photo_name { get; set; }
        public string salutation_txt { get; set; }

        public int totalcount { get; set; }
    }
    public class AgencyAdditionalInfo
    {
        public string ISHONORARIUMPAYMENT { get; set; }

        public string CURRENTORGANISATION { get; set; }

        public string BANK { get; set; }
        public string BRANCH { get; set; }
        public string IFSC { get; set; }
        public string ACCOUNTNO { get; set; }

        public string BRIEFDESC { get; set; }

        public string ID_PROOF_TYPE { get; set; }

        public string ID_PROOF_TYPE_TXT { get; set; }

        public string ID_PROOF_VALUE { get; set; }

        public string DESIGNATION { get; set; }

        public string DESIGNATIONID { get; set; }

        public string ISBHOPAL { get; set; }

        public string CAST { get; set; }

        public string CAST_TXT { get; set; }

        public string CURRENTPOSTING { get; set; }
        public string TYPE { get; set; }

        public string FATHER_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string DOC_PATH { get; set; }

        public string CV_PATH { get; set; }

        public string CATEGORY { get; set; }

        public string ORGANISATIONGROUP { get; set; }
        public string ORGANISATIONHEAD { get; set; }

        public string ORGANISATION { get; set; }
        public string ORGANISATIONNAME { get; set; }
        public string COURSEDETAILS { get; set; }

        public CONTACTPERSON[] contactPerson { get; set; }


    }
    public class userDetails
    {
        public string usertype { get; set; }
        public string tat_type_id { get; set; }

        public string tyaam_status { get; set; }
    }
    public class CONTACTPERSON
    {
        public string Designation { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class cast_category
    {
        public string id { get; set; }
        public string name { get; set; }
        public string hname { get; set; }
    }

    public class Update_Profile_Data
    {
        public string AgencyId { get; set; }
        public string userid { get; set; }
        public string AgencyName { get; set; }
        public string HAgencyName { get; set; }
        public string CreatedBy { get; set; }
        public string photopath { get; set; }
        public string branchid { get; set; }

        public string Ag_Address { get; set; }
        public string Ag_Address1 { get; set; }
        public string ag_address_city { get; set; }
        public string ag_address_state { get; set; }

        public string ag_pincode { get; set; }
        public string ag_salutation { get; set; }
        public string ag_first_name { get; set; }
        public string ag_l_name { get; set; }
        public string ag_hfirst_name { get; set; }
        public string ag_m_name { get; set; }
        public string ag_hm_name { get; set; }
        public string ag_hl_name { get; set; }
        public string ag_gender { get; set; }
        public string ag_age { get; set; }
        public string ag_dob { get; set; }
        public string ag_phone { get; set; }

        public string ag_alternative_phone { get; set; }
        public string ag_alternative_mobileno { get; set; }
        public string ag_alternative_email { get; set; }
        public string ag_aadhar { get; set; }

        public string ag_pan { get; set; }
        public string ag_gstn { get; set; }
        public string ag_sign_path { get; set; }
    }

    public class Agency_PersonalInfo
    {

        public int? gender { get; set; }
        public string dob { get; set; }

        public string salutation { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public string photo_path { get; set; }
        public Agency_Participant_OtherInfo OtherInfo { get; set; }

    }
    public class Agency_Participant_AddressInfo
    {
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }
        public string alt_mobileno { get; set; }

        public string phone_no { get; set; }
        public string alt_email { get; set; }

    }

    public class Agency_Participant_OtherInfo
    {
        public string fathername { get; set; }
        public string mothername { get; set; }
        public string cast_category { get; set; }
        public string id_type { get; set; }
        public string id_no { get; set; }
        public string class_or_term { get; set; }
        public string school { get; set; }
        public string doc_Path { get; set; }


    }
}
