using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Xml;

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
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();


            List<Agency> sp = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            sp = ABD.Get_Agency("00053,00068", null, 0, 0, null, null, "1", CommonEnum.Agency_Active_Status);

            if (trainingid != null)
            {
                SqlCommand cmd = new SqlCommand("TrainingPlan.proc_tp_training_participants_vr2", con);
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
                    cmd.Parameters.AddWithValue("@ParticipantId", participantid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@branchid", branchid);
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;




                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                foreach (DataRow row in dt.Rows)
                {

                    Participant vw = new Participant();
                    vw.ParticipantId = Convert.ToString(row["ParticipantId"]);
                    vw.ParticipantType = Convert.ToString(row["ParticipantType"]);
                    vw.ParticipantName = Convert.ToString(row["ParticipantName"]);
                    vw.HParticipantName = Convert.ToString(row["HParticipantName"]);
                    if (row.Table.Columns.Contains("TrainingId"))
                    {
                        vw.TrainingId = Convert.ToString(row["TrainingId"]);
                    }

                    vw.DesignationName = Convert.ToString(row["DesignationName"]);
                    vw.OfficeAddress = Convert.ToString(row["OfficeAddress"]);
                    vw.ResidentialAddress = Convert.ToString(row["ResidentialAddress"]);
                    vw.OfficePhoneNo = Convert.ToString(row["OfficePhoneNo"]);
                    vw.ResidencialPhoneNo = Convert.ToString(row["ResidencialPhoneNo"]);
                    if (row["Age"].ToString() != "")
                    {
                        vw.Age = Convert.ToDecimal(row["Age"]);
                    }

                    vw.Qualification = Convert.ToString(row["Qualification"]);
                    vw.DurationInGovtJob = Convert.ToString(row["DurationInGovtJob"]);
                    vw.SpecialSkillAreas = Convert.ToString(row["SpecialSkillAreas"]);
                    if (row["BasicPay"].ToString() != "")
                    {
                        vw.BasicPay = Convert.ToDecimal(row["BasicPay"]);

                    }

                    if (row["Is_IAS_IPS_Officer"].ToString() != "")
                    {
                        vw.Is_IAS_IPS_Officer = Convert.ToInt32(row["Is_IAS_IPS_Officer"]);
                    }
                    if (row["Is_With_Spouse"].ToString() != "")
                    {
                        vw.Is_With_Spouse = Convert.ToInt32(row["Is_With_Spouse"]);
                    }

                    vw.BranchId = Convert.ToString(row["BranchId"]);
                    if (row.Table.Columns.Contains("SponsorID"))
                    {
                        vw.SponsorID = Convert.ToString(row["SponsorID"]);
                        try
                        {
                            vw.Sposnorname = sp.Where(o => o.agencyid.ToString().ToUpper() == row["SponsorID"].ToString().ToUpper()).FirstOrDefault().agencyname;
                        }
                        catch (Exception e)
                        {
                            vw.Sposnorname = null;
                        }

                    }

                    if (row.Table.Columns.Contains("t_Name"))
                    {
                        vw.t_Name = Convert.ToString(row["t_Name"]);
                    }
                    if (row.Table.Columns.Contains("t_Name"))
                    {
                        vw.TrainingCode = Convert.ToString(row["TrainingCode"]);
                    }
                    if (row.Table.Columns.Contains("Sposnorname"))
                    {
                        vw.Sposnorname = Convert.ToString(row["Sposnorname"]);
                    }
                    if (row.Table.Columns.Contains("hSposnorname"))
                    {
                        vw.hSposnorname = Convert.ToString(row["hSposnorname"]);
                    }
                    if (row.Table.Columns.Contains("Training_SponsorType"))
                    {
                        vw.Training_SponsorType = Convert.ToString(row["Training_SponsorType"]);
                    }


                    vw.OfficeAddress1 = Convert.ToString(row["OfficeAddress1"]);
                    vw.OfficeAddress2 = Convert.ToString(row["OfficeAddress2"]);
                    if (row.Table.Columns.Contains("E_Department"))
                    {
                        vw.E_Department = Convert.ToString(row["E_Department"]);
                    }

                    vw.mobileno = Convert.ToString(row["mobileno"]);
                    vw.email = Convert.ToString(row["email"]);
                    vw.gender = Convert.ToString(row["gender"]);
                    if (row["gender"].ToString() != "")
                    {
                        vw.gendername = Enum.GetName(typeof(Common.CommonEnum.Gender), (int)row["gender"]);
                    }

                    vw.salutation = Convert.ToString(row["salutation"]);
                    vw.F_NAME = Convert.ToString(row["F_NAME"]);
                    vw.M_NAME = Convert.ToString(row["M_NAME"]);
                    vw.L_NAME = Convert.ToString(row["L_NAME"]);
                    if (row["WITHCHILD"].ToString() != "")
                    {
                        vw.WITHCHILD = Convert.ToInt32(row["WITHCHILD"]);
                    }

                    vw.CURRENTLOCATION = Convert.ToString(row["CURRENTLOCATION"]);
                    vw.NOOFPERSON = Convert.ToString(row["NOOFPERSON"]);
                    if (row["Is_Bhopal"].ToString() != "")
                    {
                        vw.Is_Bhopal = Convert.ToInt32(row["Is_Bhopal"]);
                    }

                    //vw.ts_hname = Convert.ToString(row["ts_hname"]);
                    //vw.ts_name = Convert.ToString(row["ts_name"]);
                    vw.isnodues = Convert.ToString(row["isnodues"]);
                    vw.feedbackstatus = Convert.ToString(row["feedbackstatus"]);
                    if (row["is_deleted"].ToString() != "")
                    {
                        vw.is_deleted = Convert.ToInt32(row["is_deleted"]);
                    }
                    if (row["is_approve"].ToString() != "")
                    {
                        vw.is_approve = Convert.ToInt32(row["is_approve"]);
                    }


                    vw.uploadpath = Convert.ToString(row["uploadpath"]);
                    vw.remark = Convert.ToString(row["remark"]);
                    if (row["dob"].ToString() != "")
                    {
                        vw.dob = Convert.ToDateTime(row["dob"]);
                    }

                    vw.photopath = Convert.ToString(row["photopath"]);
                    vw.participat_master_remark = Convert.ToString(row["participat_master_remark"]);
                    vw.ag_gstin = Convert.ToString(row["ag_gstin"]);
                    vw.Ag_Address = Convert.ToString(row["Ag_Address"]);
                    vw.ag_address_city = Convert.ToString(row["ag_address_city"]);
                    vw.ag_address_state = Convert.ToString(row["ag_address_state"]);
                    vw.ag_pincode = Convert.ToString(row["ag_pincode"]);
                    vw.ag_alternative_mobileno = Convert.ToString(row["ag_alternative_mobileno"]);
                    vw.usercode = Convert.ToString(row["usercode"]);
                    vw.tyaam_val = Convert.ToString(row["tyaam_val"]);
                    vw.ttpai_id = Convert.ToString(row["ttpai_id"]);
                    if (Convert.ToString(row["is_approve"]) != "")
                    {
                        if (trainingid != null)
                        {
                            vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Enroll_Status), Convert.ToInt32(row["is_approve"]));
                        }
                        else
                        {
                            vw.status_txt = Enum.GetName(typeof(Common.CommonEnum.Participant_Status), Convert.ToInt32(row["is_approve"]));
                        }

                    }




                    if (row["ttpai_trg_start_date"].ToString() != "")
                    {
                        vw.ttpai_trg_start_date = Convert.ToDateTime(row["ttpai_trg_start_date"]);
                    }
                    if (row["ttpai_trg_end_date"].ToString() != "")
                    {
                        vw.ttpai_trg_end_date = Convert.ToDateTime(row["ttpai_trg_end_date"]);
                    }

                    vw.ttpai_is_specific = Convert.ToString(row["ttpai_is_specific"]);
                    // vw.UserName = Convert.ToString(row["UserName"]);

                    if (Convert.ToString(row["photopath"]) == "")
                    {
                        vw.photopath_full = Convert.ToString(row["photopath"]);
                    }


                    if (row["tyaam_val"].ToString() != "")
                    {
                        //First Convert XML to Json
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(row["tyaam_val"].ToString().Replace("&lt;", "<").Replace("&gt;", ">"));

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
                            AgencyAdditionalInfo ai = new AgencyAdditionalInfo();
                            vw.additionalInfo = ai;
                        }

                        if (vw.additionalInfo != null)
                        {
                            if (vw.additionalInfo.CAST != null)
                            {
                                vw.castname = Enum.GetName(typeof(Common.CommonEnum.Cast), Convert.ToInt32(vw.additionalInfo.CAST.ToString()));
                            }
                        }



                    }
                    else
                    {
                        AgencyAdditionalInfo ai = new AgencyAdditionalInfo();
                        vw.additionalInfo = ai;
                    }


                    trgdata.Add(vw);
                }

            }
            else
            {
                AgencyBL abl = new AgencyBL(_configuration);
                List<Agency> al = abl.Get_Agency_Data("00051", null, 0, 0, null);
                foreach (Agency a in al)
                {
                    Participant vw = new Participant();
                    vw.ParticipantId = a.agencyid;
                    vw.ParticipantType = a.AgencyTypeId;
                    vw.ParticipantName = a.agencyname;
                    vw.HParticipantName = a.hagencyname;
                    if (a.additionalInfo != null)
                    {
                        vw.DesignationName = a.additionalInfo.DESIGNATION;
                        vw.CURRENTLOCATION = a.additionalInfo.CURRENTORGANISATION;
                    }

                    vw.OfficeAddress = a.Ag_Address;
                    vw.ResidentialAddress = null;
                    vw.OfficePhoneNo = null;
                    vw.Age = a.ag_age;

                    vw.OfficeAddress1 = a.Ag_Address1;
                    vw.OfficeAddress2 = a.Ag_Address;


                    vw.mobileno = a.ag_mobileno;
                    vw.email = a.ag_email;
                    vw.gender = a.ag_gender;
                    vw.gendername = a.gender_text;

                    vw.salutation = a.ag_salutation;
                    vw.F_NAME = a.ag_first_name;
                    vw.M_NAME = a.ag_m_name;
                    vw.L_NAME = a.ag_l_name;

                    if (a.agencystatus != null)
                    {
                        vw.is_approve = Convert.ToInt32(a.agencystatus);
                    }
                    if (a.ag_dob != null)
                    {
                        vw.dob = Convert.ToDateTime(a.ag_dob);
                    }


                    vw.photopath = a.ag_photo_path;

                    vw.Ag_Address = a.Ag_Address;
                    vw.ag_address_city = a.ag_address_city;
                    vw.ag_address_state = a.ag_address_state;
                    vw.ag_pincode = a.ag_pincode;
                    vw.ag_alternative_mobileno = a.ag_alternative_mobileno;
                    vw.usercode = a.UserCode;
                    vw.tyaam_val = a.tyaam_val;


                    vw.photopath_full = a.ag_photo_path;
                    vw.additionalInfo = a.additionalInfo;

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
            con.Open();
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
            con.Open();
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

    }
}
