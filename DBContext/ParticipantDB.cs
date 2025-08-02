using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using System.Xml;
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
        public List<Participant> Get_TRG_PARTICIPANT_Data(string trainingid = null, string participantid = null, string branchid = null)
        {

            List<Participant> trgdata = new List<Participant>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            //SqlConnection con = new SqlConnection(connectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }


            List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

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
                    cmd.Parameters.AddWithValue("@branchid", branchid);
                    cmd.CommandTimeout = 5000;

                    // Execute reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant vw = new Participant
                            {
                                ParticipantId = Convert.ToString(reader["ParticipantId"]),
                                ParticipantType = Convert.ToString(reader["ParticipantType"]),
                                ParticipantName = Convert.ToString(reader["ParticipantName"]),
                                HParticipantName = Convert.ToString(reader["HParticipantName"]),
                                DesignationName = Convert.ToString(reader["DesignationName"]),
                                OfficeAddress = Convert.ToString(reader["OfficeAddress"]),
                                ResidentialAddress = Convert.ToString(reader["ResidentialAddress"]),
                                OfficePhoneNo = Convert.ToString(reader["OfficePhoneNo"]),
                                ResidencialPhoneNo = Convert.ToString(reader["ResidencialPhoneNo"]),
                                Qualification = Convert.ToString(reader["Qualification"]),
                                DurationInGovtJob = Convert.ToString(reader["DurationInGovtJob"]),
                                SpecialSkillAreas = Convert.ToString(reader["SpecialSkillAreas"]),
                                BranchId = Convert.ToString(reader["BranchId"]),
                                mobileno = Convert.ToString(reader["mobileno"]),
                                email = Convert.ToString(reader["email"]),
                                gender = Convert.ToString(reader["gender"]),
                                salutation = Convert.ToString(reader["salutation"]),
                                F_NAME = Convert.ToString(reader["F_NAME"]),
                                M_NAME = Convert.ToString(reader["M_NAME"]),
                                L_NAME = Convert.ToString(reader["L_NAME"]),
                                CURRENTLOCATION = Convert.ToString(reader["CURRENTLOCATION"]),
                                NOOFPERSON = Convert.ToString(reader["NOOFPERSON"]),
                                isnodues = Convert.ToString(reader["isnodues"]),
                                feedbackstatus = Convert.ToString(reader["feedbackstatus"]),
                                uploadpath = Convert.ToString(reader["uploadpath"]),
                                remark = Convert.ToString(reader["remark"]),
                                photopath = Convert.ToString(reader["photopath"]),
                                participat_master_remark = Convert.ToString(reader["participat_master_remark"]),
                                ag_gstin = Convert.ToString(reader["ag_gstin"]),
                                Ag_Address = Convert.ToString(reader["Ag_Address"]),
                                ag_address_city = Convert.ToString(reader["ag_address_city"]),
                                ag_address_state = Convert.ToString(reader["ag_address_state"]),
                                ag_pincode = Convert.ToString(reader["ag_pincode"]),
                                ag_alternative_mobileno = Convert.ToString(reader["ag_alternative_mobileno"]),
                                usercode = Convert.ToString(reader["usercode"]),
                                tyaam_val = Convert.ToString(reader["tyaam_val"]),
                                ttpai_id = Convert.ToString(reader["ttpai_id"])
                            };

                            // Handle nullable fields with checks
                            if (!string.IsNullOrEmpty(reader["Age"].ToString()))
                            {
                                vw.Age = Convert.ToDecimal(reader["Age"]);
                            }
                            if (!string.IsNullOrEmpty(reader["BasicPay"].ToString()))
                            {
                                vw.BasicPay = Convert.ToDecimal(reader["BasicPay"]);
                            }
                            if (!string.IsNullOrEmpty(reader["Is_IAS_IPS_Officer"].ToString()))
                            {
                                vw.Is_IAS_IPS_Officer = Convert.ToInt32(reader["Is_IAS_IPS_Officer"]);
                            }
                            if (!string.IsNullOrEmpty(reader["Is_With_Spouse"].ToString()))
                            {
                                vw.Is_With_Spouse = Convert.ToInt32(reader["Is_With_Spouse"]);
                            }
                            if (!string.IsNullOrEmpty(reader["WITHCHILD"].ToString()))
                            {
                                vw.WITHCHILD = Convert.ToInt32(reader["WITHCHILD"]);
                            }
                            if (!string.IsNullOrEmpty(reader["Is_Bhopal"].ToString()))
                            {
                                vw.Is_Bhopal = Convert.ToInt32(reader["Is_Bhopal"]);
                            }
                            if (!string.IsNullOrEmpty(reader["is_deleted"].ToString()))
                            {
                                vw.is_deleted = Convert.ToInt32(reader["is_deleted"]);
                            }
                            if (!string.IsNullOrEmpty(reader["is_approve"].ToString()))
                            {
                                vw.is_approve = Convert.ToInt32(reader["is_approve"]);
                            }
                            if (!string.IsNullOrEmpty(reader["dob"].ToString()))
                            {
                                vw.dob = Convert.ToDateTime(reader["dob"]);
                            }
                            if (!string.IsNullOrEmpty(reader["ttpai_trg_start_date"].ToString()))
                            {
                                vw.ttpai_trg_start_date = Convert.ToDateTime(reader["ttpai_trg_start_date"]);
                            }
                            if (!string.IsNullOrEmpty(reader["ttpai_trg_end_date"].ToString()))
                            {
                                vw.ttpai_trg_end_date = Convert.ToDateTime(reader["ttpai_trg_end_date"]);
                            }

                            // Gender name
                            if (!string.IsNullOrEmpty(reader["gender"].ToString()))
                            {
                                vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), (int)reader["gender"]);
                            }

                            // Status text based on condition
                            if (!string.IsNullOrEmpty(reader["is_approve"].ToString()))
                            {
                                if (trainingid != null)
                                {
                                    vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Enroll_Status), Convert.ToInt32(reader["is_approve"]));
                                }
                                else
                                {
                                    vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Status), Convert.ToInt32(reader["is_approve"]));
                                }
                            }

                            // Additional Info parsing from XML
                            if (!string.IsNullOrEmpty(reader["tyaam_val"].ToString()))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(reader["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));

                                XmlDocument doc1 = new XmlDocument();
                                try
                                {
                                    doc1.LoadXml(doc.ChildNodes[0].InnerXml);
                                    string JsonText = JsonConvert.SerializeXmlNode(doc1).Replace("\"ADDINFO\":", "");
                                    JsonText = JsonText.Substring(1, JsonText.Length - 2);

                                    vw.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}"));
                                }
                                catch
                                {
                                    vw.additionalInfo = new AgencyAdditionalInfo();
                                }

                                if (vw.additionalInfo?.CAST != null)
                                {
                                    try
                                    {
                                        vw.castname = Enum.GetName(typeof(Common.CommonEnum.Cast), Convert.ToInt32(vw.additionalInfo.CAST.ToString()));
                                    }
                                    catch
                                    {

                                    }
                                  
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


        public List<Participant> Get_Search_Participant(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn=null, string searchvalue = null)
        {

            List<Participant> trgdata = new List<Participant>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            //SqlConnection con = new SqlConnection(connectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }


            List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

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
                    cmd.Parameters.AddWithValue("@branchid", branchid);
                    cmd.CommandTimeout = 5000;

                    // Execute reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant vw = new Participant
                            {
                                ParticipantId = Convert.ToString(reader["ParticipantId"]),
                                ParticipantType = Convert.ToString(reader["ParticipantType"]),
                                ParticipantName = Convert.ToString(reader["ParticipantName"]),
                                HParticipantName = Convert.ToString(reader["HParticipantName"]),
                                DesignationName = Convert.ToString(reader["DesignationName"]),
                                OfficeAddress = Convert.ToString(reader["OfficeAddress"]),
                                ResidentialAddress = Convert.ToString(reader["ResidentialAddress"]),
                                OfficePhoneNo = Convert.ToString(reader["OfficePhoneNo"]),
                                ResidencialPhoneNo = Convert.ToString(reader["ResidencialPhoneNo"]),
                                Qualification = Convert.ToString(reader["Qualification"]),
                                DurationInGovtJob = Convert.ToString(reader["DurationInGovtJob"]),
                                SpecialSkillAreas = Convert.ToString(reader["SpecialSkillAreas"]),
                                BranchId = Convert.ToString(reader["BranchId"]),
                                mobileno = Convert.ToString(reader["mobileno"]),
                                email = Convert.ToString(reader["email"]),
                                gender = Convert.ToString(reader["gender"]),
                                salutation = Convert.ToString(reader["salutation"]),
                                F_NAME = Convert.ToString(reader["F_NAME"]),
                                M_NAME = Convert.ToString(reader["M_NAME"]),
                                L_NAME = Convert.ToString(reader["L_NAME"]),
                                CURRENTLOCATION = Convert.ToString(reader["CURRENTLOCATION"]),
                                NOOFPERSON = Convert.ToString(reader["NOOFPERSON"]),
                                isnodues = Convert.ToString(reader["isnodues"]),
                                feedbackstatus = Convert.ToString(reader["feedbackstatus"]),
                                uploadpath = Convert.ToString(reader["uploadpath"]),
                                remark = Convert.ToString(reader["remark"]),
                                photopath = Convert.ToString(reader["photopath"]),
                                participat_master_remark = Convert.ToString(reader["participat_master_remark"]),
                                ag_gstin = Convert.ToString(reader["ag_gstin"]),
                                Ag_Address = Convert.ToString(reader["Ag_Address"]),
                                ag_address_city = Convert.ToString(reader["ag_address_city"]),
                                ag_address_state = Convert.ToString(reader["ag_address_state"]),
                                ag_pincode = Convert.ToString(reader["ag_pincode"]),
                                ag_alternative_mobileno = Convert.ToString(reader["ag_alternative_mobileno"]),
                                usercode = Convert.ToString(reader["usercode"]),
                                tyaam_val = Convert.ToString(reader["tyaam_val"]),
                                ttpai_id = Convert.ToString(reader["ttpai_id"])
                            };

                            // Handle nullable fields with checks
                            if (!string.IsNullOrEmpty(reader["Age"].ToString()))
                            {
                                vw.Age = Convert.ToDecimal(reader["Age"]);
                            }
                            if (!string.IsNullOrEmpty(reader["BasicPay"].ToString()))
                            {
                                vw.BasicPay = Convert.ToDecimal(reader["BasicPay"]);
                            }
                            if (!string.IsNullOrEmpty(reader["Is_IAS_IPS_Officer"].ToString()))
                            {
                                vw.Is_IAS_IPS_Officer = Convert.ToInt32(reader["Is_IAS_IPS_Officer"]);
                            }
                            if (!string.IsNullOrEmpty(reader["Is_With_Spouse"].ToString()))
                            {
                                vw.Is_With_Spouse = Convert.ToInt32(reader["Is_With_Spouse"]);
                            }
                            if (!string.IsNullOrEmpty(reader["WITHCHILD"].ToString()))
                            {
                                vw.WITHCHILD = Convert.ToInt32(reader["WITHCHILD"]);
                            }
                            if (!string.IsNullOrEmpty(reader["Is_Bhopal"].ToString()))
                            {
                                vw.Is_Bhopal = Convert.ToInt32(reader["Is_Bhopal"]);
                            }
                            if (!string.IsNullOrEmpty(reader["is_deleted"].ToString()))
                            {
                                vw.is_deleted = Convert.ToInt32(reader["is_deleted"]);
                            }
                            if (!string.IsNullOrEmpty(reader["is_approve"].ToString()))
                            {
                                vw.is_approve = Convert.ToInt32(reader["is_approve"]);
                            }
                            if (!string.IsNullOrEmpty(reader["dob"].ToString()))
                            {
                                vw.dob = Convert.ToDateTime(reader["dob"]);
                            }
                            if (!string.IsNullOrEmpty(reader["ttpai_trg_start_date"].ToString()))
                            {
                                vw.ttpai_trg_start_date = Convert.ToDateTime(reader["ttpai_trg_start_date"]);
                            }
                            if (!string.IsNullOrEmpty(reader["ttpai_trg_end_date"].ToString()))
                            {
                                vw.ttpai_trg_end_date = Convert.ToDateTime(reader["ttpai_trg_end_date"]);
                            }

                            // Gender name
                            if (!string.IsNullOrEmpty(reader["gender"].ToString()))
                            {
                                vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), (int)reader["gender"]);
                            }

                            // Status text based on condition
                            if (!string.IsNullOrEmpty(reader["is_approve"].ToString()))
                            {
                                if (trainingid != null)
                                {
                                    vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Enroll_Status), Convert.ToInt32(reader["is_approve"]));
                                }
                                else
                                {
                                    vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Status), Convert.ToInt32(reader["is_approve"]));
                                }
                            }

                            // Additional Info parsing from XML
                            if (!string.IsNullOrEmpty(reader["tyaam_val"].ToString()))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(reader["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));

                                XmlDocument doc1 = new XmlDocument();
                                try
                                {
                                    doc1.LoadXml(doc.ChildNodes[0].InnerXml);
                                    string JsonText = JsonConvert.SerializeXmlNode(doc1).Replace("\"ADDINFO\":", "");
                                    JsonText = JsonText.Substring(1, JsonText.Length - 2);

                                    vw.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}"));
                                }
                                catch
                                {
                                    vw.additionalInfo = new AgencyAdditionalInfo();
                                }

                                if (vw.additionalInfo?.CAST != null)
                                {
                                    vw.castname = Enum.GetName(typeof(Common.CommonEnum.Cast), Convert.ToInt32(vw.additionalInfo.CAST.ToString()));
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



        public List<Participant> Get_Trg_Participant_List(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn = null, string searchvalue = null,string sortcolumn=null,string sortvalue=null, string filtername = null, string filtervalue = null,int pageno=1,int pagesize=-1)
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


            List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

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
                else
                {
                    cmd.Parameters.AddWithValue("@sortcolumn", DBNull.Value);
                }
                if (sortcolumn != null)
                {
                    cmd.Parameters.AddWithValue("@SortOrder", sortvalue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SortOrder", DBNull.Value);
                }
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
                cmd.Parameters.AddWithValue("@pageno", pageno);
                cmd.Parameters.AddWithValue("@pagesize", pagesize);

                cmd.CommandTimeout = 5000;

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
                            ParticipantId = Convert.ToString(reader["ParticipantId"]),
                            ParticipantType = Convert.ToString(reader["ParticipantType"]),
                            ParticipantName = Convert.ToString(reader["ParticipantName"]),
                            HParticipantName = Convert.ToString(reader["HParticipantName"]),
                            DesignationName = Convert.ToString(reader["DesignationName"]),
                            OfficeAddress = Convert.ToString(reader["OfficeAddress"]),
                            ResidentialAddress = Convert.ToString(reader["ResidentialAddress"]),
                            OfficePhoneNo = Convert.ToString(reader["OfficePhoneNo"]),
                            ResidencialPhoneNo = Convert.ToString(reader["ResidencialPhoneNo"]),
                            Qualification = Convert.ToString(reader["Qualification"]),
                            DurationInGovtJob = Convert.ToString(reader["DurationInGovtJob"]),
                            SpecialSkillAreas = Convert.ToString(reader["SpecialSkillAreas"]),
                            BranchId = Convert.ToString(reader["BranchId"]),
                            email = CommonDB.Get_MaskData(ismaskingrequired, Convert.ToString(reader["email"]), f, (int)Common.CommonEnum.MaskingColumn.EMAIL),
                            mobileno = CommonDB.Get_MaskData(ismaskingrequired, Convert.ToString(reader["mobileno"]), f, (int)Common.CommonEnum.MaskingColumn.MOBILENO),
                            //mobileno = Convert.ToString(reader["mobileno"]),
                            //email = Convert.ToString(reader["email"]),
                            gender = Convert.ToString(reader["gender"]),
                            salutation = Convert.ToString(reader["salutation"]),
                            F_NAME = Convert.ToString(reader["F_NAME"]),
                            M_NAME = Convert.ToString(reader["M_NAME"]),
                            L_NAME = Convert.ToString(reader["L_NAME"]),
                            CURRENTLOCATION = Convert.ToString(reader["CURRENTLOCATION"]),
                            NOOFPERSON = Convert.ToString(reader["NOOFPERSON"]),
                            isnodues = Convert.ToString(reader["isnodues"]),
                            feedbackstatus = Convert.ToString(reader["feedbackstatus"]),
                            uploadpath = Convert.ToString(reader["uploadpath"]),
                            remark = Convert.ToString(reader["remark"]),
                            photopath = Convert.ToString(reader["photopath"]),
                            participat_master_remark = Convert.ToString(reader["participat_master_remark"]),
                            ag_gstin = Convert.ToString(reader["ag_gstin"]),
                            Ag_Address = Convert.ToString(reader["Ag_Address"]),
                            ag_address_city = Convert.ToString(reader["ag_address_city"]),
                            ag_address_state = Convert.ToString(reader["ag_address_state"]),
                            ag_pincode = Convert.ToString(reader["ag_pincode"]),
                            ag_alternative_mobileno = Convert.ToString(reader["ag_alternative_mobileno"]),
                            usercode = Convert.ToString(reader["usercode"]),
                            tyaam_val = Convert.ToString(reader["tyaam_val"]),
                            ttpai_id = Convert.ToString(reader["ttpai_id"]),
                            totalrecords = Convert.ToInt32(reader["totalrecords"]),
                            rcname = Convert.ToString(reader["district_name"]),
                            scname = Convert.ToString(reader["Branchname"]),
                            TrainingCode= Convert.ToString(reader["TrainingCode"]),
                            t_Name = Convert.ToString(reader["t_Name"])


                        };

                        // Handle nullable fields with checks
                        if (!string.IsNullOrEmpty(reader["Age"].ToString()))
                        {
                            vw.Age = Convert.ToDecimal(reader["Age"]);
                        }
                        if (!string.IsNullOrEmpty(reader["BasicPay"].ToString()))
                        {
                            vw.BasicPay = Convert.ToDecimal(reader["BasicPay"]);
                        }
                        if (!string.IsNullOrEmpty(reader["Is_IAS_IPS_Officer"].ToString()))
                        {
                            vw.Is_IAS_IPS_Officer = Convert.ToInt32(reader["Is_IAS_IPS_Officer"]);
                        }
                        if (!string.IsNullOrEmpty(reader["Is_With_Spouse"].ToString()))
                        {
                            vw.Is_With_Spouse = Convert.ToInt32(reader["Is_With_Spouse"]);
                        }
                        if (!string.IsNullOrEmpty(reader["WITHCHILD"].ToString()))
                        {
                            vw.WITHCHILD = Convert.ToInt32(reader["WITHCHILD"]);
                        }
                        if (!string.IsNullOrEmpty(reader["Is_Bhopal"].ToString()))
                        {
                            vw.Is_Bhopal = Convert.ToInt32(reader["Is_Bhopal"]);
                        }
                        if (!string.IsNullOrEmpty(reader["is_deleted"].ToString()))
                        {
                            vw.is_deleted = Convert.ToInt32(reader["is_deleted"]);
                        }
                        if (!string.IsNullOrEmpty(reader["is_approve"].ToString()))
                        {
                            vw.is_approve = Convert.ToInt32(reader["is_approve"]);
                        }
                        if (!string.IsNullOrEmpty(reader["dob"].ToString()))
                        {
                            vw.dob = Convert.ToDateTime(reader["dob"]);
                        }
                        if (!string.IsNullOrEmpty(reader["ttpai_trg_start_date"].ToString()))
                        {
                            vw.ttpai_trg_start_date = Convert.ToDateTime(reader["ttpai_trg_start_date"]);
                        }
                        if (!string.IsNullOrEmpty(reader["ttpai_trg_end_date"].ToString()))
                        {
                            vw.ttpai_trg_end_date = Convert.ToDateTime(reader["ttpai_trg_end_date"]);
                        }

                        // Gender name
                        if (!string.IsNullOrEmpty(reader["gender"].ToString()))
                        {
                            vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), (int)reader["gender"]);
                        }

                        // Status text based on condition
                        if (!string.IsNullOrEmpty(reader["is_approve"].ToString()))
                        {
                            if (trainingid != null)
                            {
                                vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Enroll_Status), Convert.ToInt32(reader["is_approve"]));
                            }
                            else
                            {
                                vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Status), Convert.ToInt32(reader["is_approve"]));
                            }
                        }

                        // Additional Info parsing from XML
                        if (!string.IsNullOrEmpty(reader["tyaam_val"].ToString()))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(reader["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));

                            XmlDocument doc1 = new XmlDocument();
                            try
                            {
                                doc1.LoadXml(doc.ChildNodes[0].InnerXml);
                                string JsonText = JsonConvert.SerializeXmlNode(doc1).Replace("\"ADDINFO\":", "");
                                JsonText = JsonText.Substring(1, JsonText.Length - 2);

                                vw.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}"));
                            }
                            catch
                            {
                                vw.additionalInfo = new AgencyAdditionalInfo();
                            }

                            if (vw.additionalInfo?.CAST != null)
                            {
                                try
                                {
                                    vw.castname = Enum.GetName(typeof(Common.CommonEnum.Cast), Convert.ToInt32(vw.additionalInfo.CAST.ToString()));
                                }
                                catch
                                {

                                }
                             
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


        public bool Validate_User_Training(string Participantid, string trainingid)
        {
            bool isexist = false;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("select * from TrainingPlan.tbl_tp_participant_additional_info where Participantid='"+Participantid+"' and TrainingId='"+trainingid+"'", con);
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
    }
}
