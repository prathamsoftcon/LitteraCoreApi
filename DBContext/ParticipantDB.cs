using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.PowerBI.Api;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using static LitteraCore.Models.MaskInfo;

namespace LitteraCore.DBContext
{
    public class ParticipantDB
    {
        private readonly IConfiguration _configuration;
        public ParticipantDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Participant> Get_TRG_PARTICIPANT_Data(string trainingid = null, string participantid = null, string branchid = null,string columnlist=null)
        {

            List<Participant> trgdata = new List<Participant>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            //SqlConnection con = new SqlConnection(connectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }


            //List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
           // sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

            if (trainingid != null)
            {
                 using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_training_participants_vr2", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command
                    if (trainingid != null)
                    {
                        cmd.Parameters.AddWithValue("@trainingid", trainingid);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
                    }
                    if (participantid != null)
                    {
                        cmd.Parameters.AddWithValue("@ParticipantId", participantid);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value);
                    }
                    if (columnlist != null)
                    {
                        cmd.Parameters.AddWithValue("@ColumnList", columnlist);
                    }
                    cmd.Parameters.AddWithValue("@branchid", branchid);
                    cmd.CommandTimeout = 5000;

                    // Execute reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant vw = new Participant
                            {
                                ParticipantId = reader.GetStringSafe("ParticipantId"),
                                ParticipantType = reader.GetStringSafe("ParticipantType"),
                                ParticipantName = reader.GetStringSafe("ParticipantName"),
                                HParticipantName = reader.GetStringSafe("HParticipantName"),
                                DesignationName = reader.GetStringSafe("DesignationName"),
                                OfficeAddress = reader.GetStringSafe("OfficeAddress"),
                                ResidentialAddress = reader.GetStringSafe("ResidentialAddress"),
                                OfficePhoneNo = reader.GetStringSafe("OfficePhoneNo"),
                                ResidencialPhoneNo = reader.GetStringSafe("ResidencialPhoneNo"),
                                Qualification = reader.GetStringSafe("Qualification"),
                                DurationInGovtJob = reader.GetStringSafe("DurationInGovtJob"),
                                SpecialSkillAreas = reader.GetStringSafe("SpecialSkillAreas"),
                                BranchId = reader.GetStringSafe("BranchId"),
                                mobileno = reader.GetStringSafe("mobileno"),
                                email = reader.GetStringSafe("email"),
                                gender = reader.GetStringSafe("gender"),
                                salutation = reader.GetStringSafe("salutation"),
                                F_NAME = reader.GetStringSafe("F_NAME"),
                                M_NAME = reader.GetStringSafe("M_NAME"),
                                L_NAME = reader.GetStringSafe("L_NAME"),
                                CURRENTLOCATION = reader.GetStringSafe("CURRENTLOCATION"),
                                NOOFPERSON = reader.GetStringSafe("NOOFPERSON"),
                                isnodues = reader.GetStringSafe("isnodues"),
                                feedbackstatus = reader.GetStringSafe("feedbackstatus"),
                                uploadpath = reader.GetStringSafe("uploadpath"),
                                remark = reader.GetStringSafe("remark"),
                                photopath = reader.GetStringSafe("photopath"),
                                participat_master_remark = reader.GetStringSafe("participat_master_remark"),
                                ag_gstin = reader.GetStringSafe("ag_gstin"),
                                Ag_Address = reader.GetStringSafe("Ag_Address"),
                                ag_address_city = reader.GetStringSafe("ag_address_city"),
                                ag_address_state = reader.GetStringSafe("ag_address_state"),
                                ag_pincode = reader.GetStringSafe("ag_pincode"),
                                ag_alternative_mobileno = reader.GetStringSafe("ag_alternative_mobileno"),
                                usercode = reader.GetStringSafe("usercode"),
                                tyaam_val = reader.GetStringSafe("tyaam_val"),
                                ttpai_id = reader.GetStringSafe("ttpai_id"),

                                Age = reader.GetDecimalSafe("Age"),
                                BasicPay = reader.GetDecimalSafe("BasicPay"),
                                Is_IAS_IPS_Officer = reader.GetIntSafe("Is_IAS_IPS_Officer"),
                                Is_With_Spouse = reader.GetIntSafe("Is_With_Spouse"),
                                WITHCHILD = reader.GetIntSafe("WITHCHILD"),
                                Is_Bhopal = reader.GetIntSafe("Is_Bhopal"),
                                is_deleted = reader.GetIntSafe("is_deleted"),
                                is_approve = reader.GetIntSafe("is_approve"),
                                dob = reader.GetDateSafe("dob"),
                                ttpai_trg_start_date = reader.GetDateSafe("ttpai_trg_start_date"),
                                ttpai_trg_end_date = reader.GetDateSafe("ttpai_trg_end_date")
                            };

                            // Gender name
                            if (!string.IsNullOrEmpty(vw.gender))
                                vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), Convert.ToInt32(vw.gender));

                            // Status
                            if (vw.is_approve != 0)
                            {
                                vw.status_txt = Enum.GetName(
                                    trainingid != null
                                        ? typeof(Common.CommonEnum.Participant_Enroll_Status)
                                        : typeof(Common.CommonEnum.Participant_Status),
                                    vw.is_approve);
                            }

                            // Additional Info XML parsing stays same...
                            // (can be optimized too, if needed)

                            trgdata.Add(vw);
                        }
                    }

                }


            }
            else
            {
                AgencyBL abl = new AgencyBL(_configuration);
                List<Agency> al = abl.Get_Agency_Data("00051", null, 0, 0, null);

                foreach (Agency a in al)
                {
                    Participant vw = new Participant
                    {
                        ParticipantId = Convert.ToString(a.agencyid),
                        ParticipantType = Convert.ToString(a.AgencyTypeId),
                        ParticipantName = Convert.ToString(a.agencyname),
                        HParticipantName = Convert.ToString(a.hagencyname),
                        DesignationName = a.additionalInfo != null ? Convert.ToString(a.additionalInfo.DESIGNATION) : null,
                        CURRENTLOCATION = a.additionalInfo != null ? Convert.ToString(a.additionalInfo.CURRENTORGANISATION) : null,
                        OfficeAddress = Convert.ToString(a.Ag_Address),
                        ResidentialAddress = null, // ResidentialAddress is explicitly null
                        OfficePhoneNo = null, // OfficePhoneNo is explicitly null
                        Age = a.ag_age != 0 ? a.ag_age : 0,
                        OfficeAddress1 = Convert.ToString(a.Ag_Address1),
                        OfficeAddress2 = Convert.ToString(a.Ag_Address), // Assuming same as Ag_Address, could be updated as needed
                        mobileno = Convert.ToString(a.ag_mobileno),
                        email = Convert.ToString(a.ag_email),
                        gender = Convert.ToString(a.ag_gender),
                        gendername = Convert.ToString(a.gender_text),
                        salutation = Convert.ToString(a.ag_salutation),
                        F_NAME = Convert.ToString(a.ag_first_name),
                        M_NAME = Convert.ToString(a.ag_m_name),
                        L_NAME = Convert.ToString(a.ag_l_name),
                        is_approve = a.agencystatus != null ? Convert.ToInt32(a.agencystatus) : 0, // Handle null safely for approval status
                        dob = a.ag_dob != null ? Convert.ToDateTime(a.ag_dob) : DateTime.MinValue, // Handle null safely for DOB
                        photopath = Convert.ToString(a.ag_photo_path),
                        Ag_Address = Convert.ToString(a.Ag_Address),
                        ag_address_city = Convert.ToString(a.ag_address_city),
                        ag_address_state = Convert.ToString(a.ag_address_state),
                        ag_pincode = Convert.ToString(a.ag_pincode),
                        ag_alternative_mobileno = Convert.ToString(a.ag_alternative_mobileno),
                        usercode = Convert.ToString(a.UserCode),
                        tyaam_val = Convert.ToString(a.tyaam_val),
                        photopath_full = Convert.ToString(a.ag_photo_path),
                        additionalInfo = a.additionalInfo // Assuming this is an object, no conversion necessary
                    };

                    trgdata.Add(vw);
                }
            }


            return trgdata;
        }

        public List<Participant> Get_Participant_training_status(string Participantid, string trainingid = null)
        {

            List<Participant> trgdata = new List<Participant>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("TrainingPlan.proc_get_participant_training_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@trainingid", trainingid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);

            }
            cmd.Parameters.AddWithValue("@participantid", Participantid);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                Participant vw = new Participant();
                vw.TrainingId = Convert.ToString(row["TrainingId"]);
                vw.is_approve = Convert.ToInt32(row["tdds_status"]);

                trgdata.Add(vw);
            }





            return trgdata;
        }

        public List<ParticipantAdditionlInfo> Get_Participant_Additional_info(string trainingid, string participantid)
        {

            List<ParticipantAdditionlInfo> trgdata = new List<ParticipantAdditionlInfo>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Trainingplan.proc_tp_get_participant_additional_info", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@trainingid", trainingid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
            }
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@participantid", participantid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@participantid", DBNull.Value);
            }

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                ParticipantAdditionlInfo vw = new ParticipantAdditionlInfo();
                vw.Participantid = Convert.ToString(row["Participantid"]);
                vw.TrainingId = Convert.ToString(row["TrainingId"]);
                vw.Sponsorid = Convert.ToString(row["Sponsorid"]);
                vw.CreatedBy = Convert.ToString(row["CreatedBy"]);
                vw.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                vw.remark = Convert.ToString(row["remark"]);

                if (row["ttpai_trg_start_date"] != DBNull.Value)
                {
                    vw.ttpai_trg_start_date = Convert.ToString(Convert.ToDateTime(row["ttpai_trg_start_date"]).ToString("yyyy/MM/dd"));
                }
                if (row["ttpai_trg_end_date"] != DBNull.Value)
                {
                    vw.ttpai_trg_end_date = Convert.ToString(Convert.ToDateTime(row["ttpai_trg_end_date"]).ToString("yyyy/MM/dd"));
                }

                vw.ttpai_is_specific = Convert.ToString(row["ttpai_is_specific"]);
                vw.ttpai_id = Convert.ToString(row["ttpai_id"]);
                //vw.tpai_id = Convert.ToString(row["tpai_id"]);

                vw.uploadpath = Convert.ToString(row["uploadpath"]);
                //Common.UploadPath up = new Common.UploadPath();
                //if (Convert.ToString(row["uploadpath"]) == "")
                //{
                //    vw.photopath_full = up.Get_Agency_Default_Photo_Path();
                //}
                //else
                //{
                //    vw.photopath_full = up.Get_Agency_Photo_Path() + Convert.ToString(row["uploadpath"]);
                //}


                trgdata.Add(vw);
            }





            return trgdata;
        }


        public List<Participant> Get_Search_Participant(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn=null, string searchvalue = null,string columnlist=null)
        {

            List<Participant> trgdata = new List<Participant>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            //SqlConnection con = new SqlConnection(connectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }


            //List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            //sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

            if (trainingid != null)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_training_participants_vr2", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command
                    if (trainingid != null)
                    {
                        cmd.Parameters.AddWithValue("@trainingid", trainingid);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
                    }
                    if (participantid != null)
                    {
                        cmd.Parameters.AddWithValue("@ParticipantId", participantid);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value);
                    }
                    if (searchcolumn != null)
                    {
                        cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
                    }

                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@SearchColumn", DBNull.Value);
                    //}
                    if (searchvalue != null)
                    {
                        cmd.Parameters.AddWithValue("@SearchValue", HttpUtility.UrlDecode(searchvalue));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                    }
                    if (columnlist != null)
                    {
                        cmd.Parameters.AddWithValue("@ColumnList", columnlist);
                    }
                    cmd.Parameters.AddWithValue("@branchid", branchid);
                    cmd.CommandTimeout = 5000;

                    // Execute reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant vw = new Participant
                            {
                                ParticipantId = reader.GetStringSafe("ParticipantId"),
                                ParticipantType = reader.GetStringSafe("ParticipantType"),
                                ParticipantName = reader.GetStringSafe("ParticipantName"),
                                HParticipantName = reader.GetStringSafe("HParticipantName"),
                                DesignationName = reader.GetStringSafe("DesignationName"),
                                OfficeAddress = reader.GetStringSafe("OfficeAddress"),
                                ResidentialAddress = reader.GetStringSafe("ResidentialAddress"),
                                OfficePhoneNo = reader.GetStringSafe("OfficePhoneNo"),
                                ResidencialPhoneNo = reader.GetStringSafe("ResidencialPhoneNo"),
                                Qualification = reader.GetStringSafe("Qualification"),
                                DurationInGovtJob = reader.GetStringSafe("DurationInGovtJob"),
                                SpecialSkillAreas = reader.GetStringSafe("SpecialSkillAreas"),
                                BranchId = reader.GetStringSafe("BranchId"),
                                mobileno = reader.GetStringSafe("mobileno"),
                                email = reader.GetStringSafe("email"),
                                gender = reader.GetStringSafe("gender"),
                                salutation = reader.GetStringSafe("salutation"),
                                F_NAME = reader.GetStringSafe("F_NAME"),
                                M_NAME = reader.GetStringSafe("M_NAME"),
                                L_NAME = reader.GetStringSafe("L_NAME"),
                                CURRENTLOCATION = reader.GetStringSafe("CURRENTLOCATION"),
                                NOOFPERSON = reader.GetStringSafe("NOOFPERSON"),
                                isnodues = reader.GetStringSafe("isnodues"),
                                feedbackstatus = reader.GetStringSafe("feedbackstatus"),
                                uploadpath = reader.GetStringSafe("uploadpath"),
                                remark = reader.GetStringSafe("remark"),
                                photopath = reader.GetStringSafe("photopath"),
                                participat_master_remark = reader.GetStringSafe("participat_master_remark"),
                                ag_gstin = reader.GetStringSafe("ag_gstin"),
                                Ag_Address = reader.GetStringSafe("Ag_Address"),
                                ag_address_city = reader.GetStringSafe("ag_address_city"),
                                ag_address_state = reader.GetStringSafe("ag_address_state"),
                                ag_pincode = reader.GetStringSafe("ag_pincode"),
                                ag_alternative_mobileno = reader.GetStringSafe("ag_alternative_mobileno"),
                                usercode = reader.GetStringSafe("usercode"),
                                tyaam_val = reader.GetStringSafe("tyaam_val"),
                                ttpai_id = reader.GetStringSafe("ttpai_id"),

                                Age = reader.GetDecimalSafe("Age"),
                                BasicPay = reader.GetDecimalSafe("BasicPay"),
                                Is_IAS_IPS_Officer = reader.GetIntSafe("Is_IAS_IPS_Officer"),
                                Is_With_Spouse = reader.GetIntSafe("Is_With_Spouse"),
                                WITHCHILD = reader.GetIntSafe("WITHCHILD"),
                                Is_Bhopal = reader.GetIntSafe("Is_Bhopal"),
                                is_deleted = reader.GetIntSafe("is_deleted"),
                                is_approve = reader.GetIntSafe("is_approve"),
                                dob = reader.GetDateSafe("dob"),
                                ttpai_trg_start_date = reader.GetDateSafe("ttpai_trg_start_date"),
                                ttpai_trg_end_date = reader.GetDateSafe("ttpai_trg_end_date")
                            };

                            // Gender name
                            if (!string.IsNullOrEmpty(vw.gender))
                                vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), Convert.ToInt32(vw.gender));

                            // Status
                            if (vw.is_approve != 0)
                            {
                                vw.status_txt = Enum.GetName(
                                    trainingid != null
                                        ? typeof(Common.CommonEnum.Participant_Enroll_Status)
                                        : typeof(Common.CommonEnum.Participant_Status),
                                    vw.is_approve);
                            }

                            // Additional Info XML parsing stays same...
                            // (can be optimized too, if needed)

                            trgdata.Add(vw);
                        }
                    }

                }


            }
            else
            {
                AgencyBL abl = new AgencyBL(_configuration);
                List<Agency> al = abl.Get_Agency_Data("00051", null, 0, 0, null);

                foreach (Agency a in al)
                {
                    Participant vw = new Participant
                    {
                        ParticipantId = a.agencyid,
                        ParticipantType = a.AgencyTypeId,
                        ParticipantName = a.agencyname,
                        HParticipantName = a.hagencyname,

                        DesignationName = a.additionalInfo?.DESIGNATION,
                        CURRENTLOCATION = a.additionalInfo?.CURRENTORGANISATION,

                        OfficeAddress = a.Ag_Address,
                        ResidentialAddress = null,
                        OfficePhoneNo = null,

                        Age = a.ag_age, // already int, safe

                        OfficeAddress1 = a.Ag_Address1,
                        OfficeAddress2 = a.Ag_Address,

                        mobileno = a.ag_mobileno,
                        email = a.ag_email,

                        gender = a.ag_gender,
                        gendername = a.gender_text,

                        salutation = a.ag_salutation,

                        F_NAME = a.ag_first_name,
                        M_NAME = a.ag_m_name,
                        L_NAME = a.ag_l_name,

                        is_approve = Convert.ToInt16(a.agencystatus),

                        dob = !string.IsNullOrEmpty(a.ag_dob)
                                                ? DateTime.Parse(a.ag_dob)
                                                : DateTime.MinValue,

                        photopath = a.ag_photo_path,

                        Ag_Address = a.Ag_Address,
                        ag_address_city = a.ag_address_city,
                        ag_address_state = a.ag_address_state,
                        ag_pincode = a.ag_pincode,
                        ag_alternative_mobileno = a.ag_alternative_mobileno,

                        usercode = a.UserCode,
                        tyaam_val = a.tyaam_val,

                        photopath_full = a.ag_photo_path,

                        additionalInfo = a.additionalInfo
                    };

                    trgdata.Add(vw);
                }

            }


            return trgdata;
        }



        public List<Participant> Get_Trg_Participant_List(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn = null, string searchvalue = null,string sortcolumn=null,string sortvalue=null, string filtername = null, string filtervalue = null,int pageno=1,int pagesize=-1, int is_certificate_generated=2,string ColumnList=null)
        {
            if (pagesize == -1)
            {
                pagesize = 50000;
            }

            List<Participant> trgdata = new List<Participant>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            //SqlConnection con = new SqlConnection(connectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }


            //List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            //sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_training_participants_vr2", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters to the command
                if (trainingid != null)
                {
                    cmd.Parameters.AddWithValue("@trainingid", trainingid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
                }
                if (participantid != null)
                {
                    cmd.Parameters.AddWithValue("@ParticipantId", participantid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value);
                }
                if (searchcolumn != null)
                {
                    cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SearchColumn", DBNull.Value);
                }
                if (searchvalue != null)
                {
                    cmd.Parameters.AddWithValue("@SearchValue", HttpUtility.UrlDecode(searchvalue));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                }
                if(branchid != null)
                {
                    cmd.Parameters.AddWithValue("@branchid", branchid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@branchid", DBNull.Value);
                }
            

                if (sortcolumn != null)
                {
                    cmd.Parameters.AddWithValue("@sortcolumn", sortcolumn);
                }
                //else
                //{
                //    cmd.Parameters.AddWithValue("@sortcolumn", DBNull.Value);
                //}
                if (sortvalue != null)
                {
                    cmd.Parameters.AddWithValue("@SortOrder", sortvalue);
                }
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SortOrder", DBNull.Value);
                //}
                if (filtername != null)
                {
                    cmd.Parameters.AddWithValue("@filtername", filtername);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@filtername", DBNull.Value);
                }
                if (filtervalue != null)
                {
                    cmd.Parameters.AddWithValue("@filtervalue", filtervalue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@filtervalue", DBNull.Value);
                }
                if (pageno != null & pageno != 0)
                {
                    cmd.Parameters.AddWithValue("@pageno", pageno);
                    cmd.Parameters.AddWithValue("@pagesize", pagesize);
                }
                if (ColumnList != null)
                {
                    cmd.Parameters.AddWithValue("@ColumnList", ColumnList);
                }

                cmd.CommandTimeout = 120;

                // Execute reader
                Form f = new Form();
                f = CommonDB.Get_Form_Masking_Info("9511");

                int ismaskingrequired = 0;
                ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
                Masking_Setting ml = new Masking_Setting();
                ml = JsonConvert.DeserializeObject<Masking_Setting>(a.Get_Application_Setting("10").Rows[0]["SettingValue"].ToString());
                ismaskingrequired = ml.data_masking_required;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Participant vw = new Participant
                        {
                            ParticipantId = reader.GetStringSafe("ParticipantId"),
                            ParticipantType = reader.GetStringSafe("ParticipantType"),
                            ParticipantName = reader.GetStringSafe("ParticipantName"),
                            HParticipantName = reader.GetStringSafe("HParticipantName"),

                            DesignationName = reader.GetStringSafe("DesignationName"),
                            OfficeAddress = reader.GetStringSafe("OfficeAddress"),
                            ResidentialAddress = reader.GetStringSafe("ResidentialAddress"),
                            OfficePhoneNo = reader.GetStringSafe("OfficePhoneNo"),
                            ResidencialPhoneNo = reader.GetStringSafe("ResidencialPhoneNo"),

                            Qualification = reader.GetStringSafe("Qualification"),
                            DurationInGovtJob = reader.GetStringSafe("DurationInGovtJob"),
                            SpecialSkillAreas = reader.GetStringSafe("SpecialSkillAreas"),
                            BranchId = reader.GetStringSafe("BranchId"),

                            email = CommonDB.Get_MaskData(ismaskingrequired, reader.GetStringSafe("email"), f, (int)Common.CommonEnum.MaskingColumn.EMAIL),
                            mobileno = CommonDB.Get_MaskData(ismaskingrequired, reader.GetStringSafe("mobileno"), f, (int)Common.CommonEnum.MaskingColumn.MOBILENO),

                            gender = reader.GetStringSafe("gender"),
                            salutation = reader.GetStringSafe("salutation"),
                            F_NAME = reader.GetStringSafe("F_NAME"),
                            M_NAME = reader.GetStringSafe("M_NAME"),
                            L_NAME = reader.GetStringSafe("L_NAME"),

                            CURRENTLOCATION = reader.GetStringSafe("CURRENTLOCATION"),
                            NOOFPERSON = reader.GetStringSafe("NOOFPERSON"),
                            isnodues = reader.GetStringSafe("isnodues"),
                            feedbackstatus = reader.GetStringSafe("feedbackstatus"),

                            uploadpath = reader.GetStringSafe("uploadpath"),
                            remark = reader.GetStringSafe("remark"),
                            photopath = reader.GetStringSafe("photopath"),

                            participat_master_remark = reader.GetStringSafe("participat_master_remark"),
                            ag_gstin = reader.GetStringSafe("ag_gstin"),
                            Ag_Address = reader.GetStringSafe("Ag_Address"),
                            ag_address_city = reader.GetStringSafe("ag_address_city"),
                            ag_address_state = reader.GetStringSafe("ag_address_state"),
                            ag_pincode = reader.GetStringSafe("ag_pincode"),
                            ag_alternative_mobileno = reader.GetStringSafe("ag_alternative_mobileno"),

                            usercode = reader.GetStringSafe("usercode"),
                            tyaam_val = reader.GetStringSafe("tyaam_val"),
                            ttpai_id = reader.GetStringSafe("ttpai_id"),

                            TrainingCode = reader.GetStringSafe("TrainingCode"),
                            t_Name = reader.GetStringSafe("t_Name"),
                            rcname = reader.GetStringSafe("district_name"),
                            scname = reader.GetStringSafe("Branchname"),

                            totalrecords = reader.GetIntSafe("totalrecords"),

                            ttpai_trg_cert_id = reader.GetStringSafe("ttpai_trg_cert_id"),
                            ttpai_trg_cert_info = ParseCertificateInfo(reader.GetStringSafe("ttpai_trg_cert_info"))
                        };

                        // Numeric & Date fields
                        vw.Age = reader.GetDecimalSafe("Age");
                        vw.BasicPay = reader.GetDecimalSafe("BasicPay");
                        vw.Is_IAS_IPS_Officer = reader.GetIntSafe("Is_IAS_IPS_Officer");
                        vw.Is_With_Spouse = reader.GetIntSafe("Is_With_Spouse");
                        vw.WITHCHILD = reader.GetIntSafe("WITHCHILD");
                        vw.Is_Bhopal = reader.GetIntSafe("Is_Bhopal");
                        vw.is_deleted = reader.GetIntSafe("is_deleted");
                        vw.is_approve = reader.GetIntSafe("is_approve");

                        vw.dob = reader.GetDateSafe("dob");
                        vw.ttpai_trg_start_date = reader.GetDateSafe("ttpai_trg_start_date");
                        vw.ttpai_trg_end_date = reader.GetDateSafe("ttpai_trg_end_date");

                        // Gender enum
                        if (vw.gender != null && vw.gender != "")
                        {
                            var genderInt = reader.GetIntSafe("gender");
                            if (genderInt != null)
                                vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), genderInt);
                        }

                        // Status text 
                        if (vw.is_approve != null)
                        {
                            vw.status_txt = trainingid != null
                                ? Enum.GetName(typeof(Common.CommonEnum.Participant_Enroll_Status), vw.is_approve)
                                : Enum.GetName(typeof(Common.CommonEnum.Participant_Status), vw.is_approve);
                        }

                        // Additional XML Info
                        string xml = vw.tyaam_val;
                        if (!string.IsNullOrEmpty(xml))
                        {
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xml.Replace("&lt;", "<").Replace("&gt;", ">"));

                                XmlDocument doc1 = new XmlDocument();
                                doc1.LoadXml(doc.ChildNodes[0].InnerXml);

                                string json = JsonConvert.SerializeXmlNode(doc1)
                                    .Replace("\"ADDINFO\":", "")
                                    .Trim('{', '}');

                                vw.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(
                                    json.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}")
                                );

                                if (vw.additionalInfo?.CAST != null)
                                {
                                    vw.castname = Enum.GetName(typeof(Common.CommonEnum.Cast),
                                        Convert.ToInt32(vw.additionalInfo.CAST));
                                }
                            }
                            catch
                            {
                                vw.additionalInfo = new AgencyAdditionalInfo();
                            }
                        }
                        else
                        {
                            vw.additionalInfo = new AgencyAdditionalInfo();
                        }

                        trgdata.Add(vw);
                    }
                }

            }


            return trgdata;
        }

        //public List<Participant> Get_Trg_Participant_List_New(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn = null, string searchvalue = null, string sortcolumn = null, string sortvalue = null, string filtername = null, string filtervalue = null, int pageno = 1, int pagesize = -1, int is_certificate_generated = 2)
        //{
        //    if (pagesize == -1)
        //    {
        //        pagesize = 50000;
        //    }

        //    List<Participant> trgdata = new List<Participant>();
        //    DataTable dt = new DataTable();
        //    string connectionString = _configuration.GetConnectionString("LitteraDatabase");
        //    //SqlConnection con = new SqlConnection(connectionString);
        //    //if (con.State != ConnectionState.Open) { con.Open(); }


        //    //List<Agency> sp = new List<Agency>();
        //    AgencyDB ABD = new AgencyDB(_configuration);
        //    //sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        if (con.State != ConnectionState.Open) { con.Open(); }
        //        SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_training_participants_vr2", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        // Adding parameters to the command
        //        if (trainingid != null)
        //        {
        //            cmd.Parameters.AddWithValue("@trainingid", trainingid);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
        //        }
        //        if (participantid != null)
        //        {
        //            cmd.Parameters.AddWithValue("@ParticipantId", participantid);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value);
        //        }
        //        if (searchcolumn != null)
        //        {
        //            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@SearchColumn", DBNull.Value);
        //        }
        //        if (searchvalue != null)
        //        {
        //            cmd.Parameters.AddWithValue("@SearchValue", HttpUtility.UrlDecode(searchvalue));
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
        //        }
        //        if (branchid != null)
        //        {
        //            cmd.Parameters.AddWithValue("@branchid", branchid);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@branchid", DBNull.Value);
        //        }


        //        if (sortcolumn != null)
        //        {
        //            cmd.Parameters.AddWithValue("@sortcolumn", sortcolumn);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@sortcolumn", DBNull.Value);
        //        }
        //        if (sortcolumn != null)
        //        {
        //            cmd.Parameters.AddWithValue("@SortOrder", sortvalue);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@SortOrder", DBNull.Value);
        //        }
        //        if (filtername != null)
        //        {
        //            cmd.Parameters.AddWithValue("@filtername", filtername);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@filtername", DBNull.Value);
        //        }
        //        if (filtervalue != null)
        //        {
        //            cmd.Parameters.AddWithValue("@filtervalue", filtervalue);
        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@filtervalue", DBNull.Value);
        //        }
        //        if (pageno != null & pageno != 0)
        //        {
        //            cmd.Parameters.AddWithValue("@pageno", pageno);
        //            cmd.Parameters.AddWithValue("@pagesize", pagesize);
        //        }


        //        cmd.CommandTimeout = 120;

        //        // Execute reader
        //        Form f = new Form();
        //        f = CommonDB.Get_Form_Masking_Info("9511");

        //        int ismaskingrequired = 0;
        //        ApplicationConfigDB a = new ApplicationConfigDB(_configuration);
        //        Masking_Setting ml = new Masking_Setting();
        //        ml = JsonConvert.DeserializeObject<Masking_Setting>(a.Get_Application_Setting("10").Rows[0]["SettingValue"].ToString());
        //        ismaskingrequired = ml.data_masking_required;
               

        //        using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
        //        {
        //            // Get column ordinals ONCE
        //            int idxParticipantId = reader.GetOrdinal("ParticipantId");
        //            int idxParticipantType = reader.GetOrdinal("ParticipantType");
        //            int idxParticipantName = reader.GetOrdinal("ParticipantName");
        //            int idxHParticipantName = reader.GetOrdinal("HParticipantName");

        //            while (reader.Read())
        //            {
        //                // Read ParticipantId (Guid) and convert to string if not null
        //                string participantId = reader.IsDBNull(idxParticipantId) ? null : reader.GetGuid(idxParticipantId).ToString();

        //                // Read string columns using GetString
        //                string participantType = reader.IsDBNull(idxParticipantType) ? null : reader.GetString(idxParticipantType);
        //                string participantName = reader.IsDBNull(idxParticipantName) ? null : reader.GetString(idxParticipantName);
        //                string hParticipantName = reader.IsDBNull(idxHParticipantName) ? null : reader.GetString(idxHParticipantName);

        //                // Add to list
        //                trgdata.Add(new Participant
        //                {
        //                    ParticipantId = participantId,
        //                    ParticipantType = participantType,
        //                    ParticipantName = participantName,
        //                    HParticipantName = hParticipantName
        //                });
        //            }
        //        }

        //    }


        //    return trgdata;
        //}

        public bool Validate_User_Training(string Participantid, string trainingid)
        {
            bool isexist = false;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("select 1 from TrainingPlan.tbl_tp_participant_additional_info ai inner join DMS.VW_dms_doc_last_status ds on ai.ttpai_id=ds.tdds_doc_id where Participantid='"+ Participantid + "' and TrainingId='"+ trainingid + "' and tdds_status <> -1", con);
            cmd.CommandType = CommandType.Text;
            

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                isexist = true;
            }





            return isexist;
        }

        public bool Update_Participant_certificate_info(certificate_obj[] Certificate_info, string trainingid)
        {
            bool isexist = false;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_tp_update_tp_participant_cert_info", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TrainingId", trainingid);
            string p1 = JsonConvert.SerializeObject(Certificate_info);
            cmd.Parameters.AddWithValue("@JsonData", p1);
            cmd.ExecuteNonQuery();

           



            return true;
        }


        public List<certificate_obj> Get_Certificate_info(string trainingid =null,string certificateid=null)
        {

            List<certificate_obj> trgdata = new List<certificate_obj>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("Trainingplan.proc_tp_get_participant_cert_info", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (trainingid != null)
            {
                cmd.Parameters.AddWithValue("@trainingid", trainingid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
            }
            if (certificateid != null)
            {
                cmd.Parameters.AddWithValue("@ttpai_trg_cert_id", certificateid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttpai_trg_cert_id", DBNull.Value);
            }

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                certificate_obj vw = new certificate_obj();
                vw.ttpai_id = Convert.ToString(row["ttpai_id"]);
                vw.CertId = Convert.ToString(row["ttpai_trg_cert_id"]);
                vw.participantname= Convert.ToString(row["AgencyName"]);
                vw.t_name= Convert.ToString(row["T_Name"]);
                vw.trainingcode= Convert.ToString(row["TrainingNo"]);
                if (Convert.ToString(row["ttpai_trg_cert_info"]) != "")
                {
                    vw.Certificate_Info = ParseCertificateInfo(Convert.ToString(row["ttpai_trg_cert_info"]));
                }
              
               trgdata.Add(vw);
            }





            return trgdata;
        }

        public static Certificate_info ParseCertificateInfo(string certInfo)
        {
            if (string.IsNullOrWhiteSpace(certInfo))
                return null;

            var certificate = new Certificate_info();
            var parts = certInfo.Split(',');

            foreach (var part in parts)
            {
                var kv = part.Split('=');
                if (kv.Length == 2)
                {
                    var key = kv[0].Trim().ToLower();
                    var value = kv[1].Trim();

                    switch (key)
                    {
                        case "id":
                            certificate.certificate_id = value;
                            break;
                        case "date":
                            certificate.certificate_dt = value;
                            break;
                        case "createdby":
                            certificate.created_by = value;
                            break;
                    }
                }
            }

            return certificate;
        }
    }
  
}
