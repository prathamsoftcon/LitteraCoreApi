using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Xml;
using static LitteraCore.Models.Firebase;

namespace LitteraCore.DBContext
{
    public class AgencyDB
    {
        private readonly IConfiguration _configuration;
        public AgencyDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public PagedList<Agency> Get_Designations(PaginationParam param)
        {

            List<Agency> f = new List<Agency>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("YUser.HR_GetDesignation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
           
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                f.Add(
                    new Agency
                    {
                        agencyid = Convert.ToString(dr["Designationid"]),
                        agencyname= Convert.ToString(dr["DesignationName"]),
                        hagencyname= Convert.ToString(dr["HDesignationName"]),

                    });
            }

            if (param == null)
            {
                return PagedList<Agency>.ToPagedList(f.ToList(),
                   param.PageNumber,
                   f.Count());
            }
            else
            {
                if (param.PageSize > 0)
                {
                    return PagedList<Agency>.ToPagedList(f.ToList(),
                param.PageNumber,
                param.PageSize);
                }
                else
                {
                    return PagedList<Agency>.ToPagedList(f.ToList(),
                param.PageNumber,
               f.Count());
                }
                 
            }
           

        }


        public List<Agency> Get_Agency(string agencytypeid, string agencyid, int pageno, int pagesize, string searchcolumn, string searchvalue, string filtername, string filtervalue, string tat_type_id = null)
        {


            List<Agency> AL = new List<Agency>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //***********Code to get salutaion data for salutation text

            List<SALUTATION> s = new List<SALUTATION>();
            s = Get_SALUTATION();
            //*****************
            List<Agency> organisations = new List<Agency>();
            organisations = Get_ORGANISATION_LIST_DATA();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();


            using (con)
            {
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_agency_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@agencytype", agencytypeid);
                if (agencyid != null)
                {
                    cmd.Parameters.AddWithValue("@agencyid", agencyid);
                }
                if (pageno != 0)
                {
                    cmd.Parameters.AddWithValue("@PageNo", pageno);
                }
                if (pagesize != 0)
                {
                    cmd.Parameters.AddWithValue("@PageSize", pagesize);
                }
                if (searchcolumn != null)
                {
                    cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
                }
                if (searchvalue != null)
                {
                    cmd.Parameters.AddWithValue("@SearchValue", searchvalue);
                }
                if (filtername != null)
                {
                    cmd.Parameters.AddWithValue("@filtername", filtername);
                }
                if (filtervalue != null)
                {
                    cmd.Parameters.AddWithValue("@filtervalue", filtervalue);
                }
                if (tat_type_id != null)
                {
                    cmd.Parameters.AddWithValue("@tat_type_id", tat_type_id);
                }

                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                SqlDataReader row = cmd.ExecuteReader();


                while (row.Read())
                {
                    if (row["tyaam_status"].ToString() != "-1")
                    {
                        Agency vw = new Agency();
                        vw.totalcount = Convert.ToInt32(row["totalrecords"]);
                        vw.agencyid = Convert.ToString(row["AgencyId"]);
                        vw.tyaam_typeid = Convert.ToString(row["tyaam_typeid"]);
                        if (Convert.ToString(row["tyaam_status"]) != "")
                        {
                            vw.tyaam_status = Convert.ToInt32(row["tyaam_status"]);
                        }
                        else
                        {
                            vw.tyaam_status = 0;
                        }
                        if (Convert.ToString(row["tyaam_status"]) != "")
                        {
                            vw.tyaam_status_text = (Enum.GetName(typeof(CommonEnum.Agencystatus), Convert.ToInt32(row["tyaam_status"])));
                        }

                        vw.agencyname = Convert.ToString(row["AgencyName"]);
                        vw.hagencyname = Convert.ToString(row["HAgencyName"]);
                        vw.AgencyTypeId = Convert.ToString(row["tyaam_typeid"]);
                        //vw.Fixed = Convert.ToString(row["Fixed"]);
                        vw.CreatedBy = Convert.ToString(row["CreatedBy"]);
                        vw.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                        vw.ModifiedBy = Convert.ToString(row["ModifiedBy"]);
                        vw.UserCode = Convert.ToString(row["UserCode"]);
                        vw.ParentId = Convert.ToString(row["ParentId"]);
                        vw.Ag_locationtype = Convert.ToString(row["Ag_locationtype"]);
                        vw.Ag_location = Convert.ToString(row["Ag_location"]);
                        vw.Ag_Address = Convert.ToString(row["Ag_Address"]);
                        vw.Ag_Address1 = Convert.ToString(row["Ag_Address1"]);
                        vw.Ag_StateId = Convert.ToString(row["Ag_StateId"]);
                        vw.Ag_DistrictId = Convert.ToString(row["Ag_DistrictId"]);
                        vw.Ag_BlockId = Convert.ToString(row["Ag_BlockId"]);
                        vw.Ag_GramPanchayatId = Convert.ToString(row["Ag_GramPanchayatId"]);
                        vw.ag_divisionid = Convert.ToString(row["ag_divisionid"]);
                        vw.upload_photo_name = Convert.ToString(row["ag_photo_path"]);
                        // vw.userid= Convert.ToString(row["userid"]);

                        //UploadPath UP = new UploadPath();
                        //if (row["uploadpath"].ToString() != "")
                        //{
                        //    vw.uploadpath = UP.Get_Default_Upload_Path() + Convert.ToString(row["uploadpath"]);
                        //}
                        if(row["uploadpath"].ToString() != "")
                        {
                            vw.uploadpath = Convert.ToString(row["uploadpath"]);
                        }
                        if (agencytypeid == "00001" || agencytypeid == "00002" || agencytypeid == "00003" || agencytypeid == "00004" || agencytypeid == "00005")
                        {
                            vw.ag_photo_path = ClientData.Get_Client_Data().CERTIFICATE_LOGO.ToString();
                        }
                        else
                        {
                            if (row["ag_photo_path"].ToString() != "")
                            {


                                vw.ag_photo_path = Convert.ToString(row["ag_photo_path"]);
                            }
                        }

                      
                      
                        vw.ag_first_name = Convert.ToString(row["ag_first_name"]);
                        vw.ag_m_name = Convert.ToString(row["ag_m_name"]);
                        vw.ag_l_name = Convert.ToString(row["ag_l_name"]);
                        vw.ag_hfirst_name = Convert.ToString(row["ag_hfirst_name"]);
                        vw.ag_hm_name = Convert.ToString(row["ag_hm_name"]);
                        vw.ag_hl_name = Convert.ToString(row["ag_hl_name"]);
                        vw.ag_address_city = Convert.ToString(row["ag_address_city"]);
                        vw.ag_address_state = Convert.ToString(row["ag_address_state"]);

                        vw.ag_pincode = Convert.ToString(row["ag_pincode"]);
                        vw.ag_phone = Convert.ToString(row["ag_phone"]);
                        vw.ag_alternative_phone = Convert.ToString(row["ag_alternative_phone"]);
                        vw.ag_mobileno = Convert.ToString(row["ag_mobileno"]);
                        vw.ag_alternative_mobileno = Convert.ToString(row["ag_alternative_mobileno"]);
                        vw.ag_email = Convert.ToString(row["ag_email"]);
                        vw.ag_alternative_email = Convert.ToString(row["ag_alternative_email"]);

                        vw.ag_gender = Convert.ToString(row["ag_gender"]);
                        if (row["ag_age"].ToString() != "")
                        {
                            vw.ag_age = Convert.ToInt32(row["ag_age"]);
                        }

                        if (row["ag_dob"].ToString() != "")
                        {
                            vw.ag_dob = Convert.ToDateTime(row["ag_dob"]).ToString("yyyy/MM/dd");
                        }

                        //vw.latitude = Convert.ToString(row["latitude"]);
                        //vw.longitude = Convert.ToString(row["longitude"]);

                        vw.ag_salutation = Convert.ToString(row["ag_salutation"]);
                        vw.ag_aadhar = Convert.ToString(row["ag_aadhar"]);
                        vw.ag_pan = Convert.ToString(row["ag_pan"]);
                        vw.ag_gstin = Convert.ToString(row["ag_gstin"]);


                        vw.ag_gstin = Convert.ToString(row["ag_gstin"]);
                        vw.ag_sign_path = Convert.ToString(row["ag_sign_path"]);
                        vw.tdds_tat_type_id = Convert.ToString(row["tdds_tat_type_id"]);
                        vw.remark = Convert.ToString(row["tdds_remark"]);

                        if (Convert.ToString(row["ag_salutation"]) != "")
                        {
                            if (s.Where(o => o.ts_id == Convert.ToInt32(row["ag_salutation"])).Count() > 0)
                            {
                                vw.salutation_txt = s.Where(o => o.ts_id == Convert.ToInt32(row["ag_salutation"])).FirstOrDefault().ts_name;
                            }

                        }



                        if (Convert.ToString(row["additional_val"]) != "")
                        {
                            vw.tyaam_val = Convert.ToString(Convert.ToString(row["additional_val"]));

                            try
                            {
                                if (agencytypeid != "00053")
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(Convert.ToString(row["additional_val"]).Replace("&lt;", "<").Replace("&gt;", ">"));
                                    XmlDocument doc1 = new XmlDocument();
                                    doc1.LoadXml(doc.ChildNodes[0].InnerXml);
                                    string JsonText = JsonConvert.SerializeXmlNode(doc1).Replace("\"ADDINFO\":", "");
                                    JsonText = JsonText.Substring(1, JsonText.Length - 2);

                                    vw.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText.Replace("\"DETAILS\":{", "\"DETAILS\":[{").Replace("}}}", "}]}}"));

                                    if (vw.additionalInfo != null)
                                    {
                                        if (vw.additionalInfo.ID_PROOF_TYPE != null)
                                        {
                                            if (vw.additionalInfo.ID_PROOF_TYPE.ToString() != "")
                                            {
                                                vw.additionalInfo.ID_PROOF_TYPE_TXT = (Enum.GetName(typeof(CommonEnum.ID_PROOF_TYPE), Convert.ToInt32(vw.additionalInfo.ID_PROOF_TYPE)));
                                            }
                                        }
                                        if (vw.additionalInfo.CAST != null)
                                        {
                                            if (vw.additionalInfo.CAST.ToString() != "")
                                            {
                                                vw.additionalInfo.CAST_TXT = (Enum.GetName(typeof(CommonEnum.CASTCATEGORY), Convert.ToInt32(vw.additionalInfo.CAST)));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(Convert.ToString(row["additional_val"]).ToString().Replace("&lt;", "<").Replace("&gt;", ">"));
                                    string JsonText1 = JsonConvert.SerializeObject(doc.ChildNodes[0].ChildNodes[0]);
                                    JsonText1 = JsonText1.Replace("{\"ADDINFO\":", "").Replace("}}", "}");
                                    vw.additionalInfo = JsonConvert.DeserializeObject<AgencyAdditionalInfo>(JsonText1);

                                    string JsonText2 = JsonConvert.SerializeObject(doc.ChildNodes[0].ChildNodes[1]);
                                    if (JsonText2 != "null")
                                    {
                                        JsonText2 = JsonText2.Replace("{\"CONTACTPERSON\":", "").Replace("}}", "}");
                                        //JsonText2 = "[{'Designation':'prince','Name':'ds','Phone':'4232','Email':'ddfg@gmail.com'},{'Designation':'Designtion','Name':'Name','Phone':'5574747474','Email':'mail@gmail.com'}]";
                                        JsonText2 = JsonText2.Replace("{\"PERSON\":", "").Replace("]}", "]");
                                        List<CONTACTPERSON> P = JsonConvert.DeserializeObject<List<CONTACTPERSON>>(JsonText2);
                                        vw.additionalInfo.contactPerson = P.ToArray();
                                    }

                                }



                            }
                            catch
                            {
                                vw.additionalInfo = null;
                            }

                        }
                        else
                        {
                            vw.additionalInfo = new AgencyAdditionalInfo();
                        }




                        AL.Add(vw);
                    }
                }
            }

            con.Close();
          
            return AL;
        }


        public List<SALUTATION> Get_SALUTATION()
        {

            List<SALUTATION> AL = new List<SALUTATION>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_tp_get_salutation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                SALUTATION vw = new SALUTATION();
                vw.ts_id = Convert.ToInt16(row["ts_id"]);
                vw.ts_name = Convert.ToString(row["ts_name"]);
                vw.ts_hname = Convert.ToString(row["ts_hname"]);
                AL.Add(vw);
            }
            return AL;
        }

        public List<Agency> Get_ORGANISATION_LIST_DATA()
        {

            List<Agency> AL = new List<Agency>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_agency_vr1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@agencytype", "00053");

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            //***********Code to get salutaion data for salutation text

            List<SALUTATION> s = new List<SALUTATION>();
            s = Get_SALUTATION();





            //******



            dt = ds.Tables[0];
            dt.DefaultView.RowFilter = "tyaam_status<>'-1'";
            dt = dt.DefaultView.ToTable();
            foreach (DataRow row in dt.Rows)
            {
                Agency vw = new Agency();
                vw.agencyid = Convert.ToString(row["AgencyId"]);
                vw.tyaam_typeid = Convert.ToString(row["tyaam_typeid"]);
                if (Convert.ToString(row["tyaam_status"]) != "")
                {
                    vw.tyaam_status = Convert.ToInt32(row["tyaam_status"]);
                }
                else
                {
                    vw.tyaam_status = 0;
                }

                vw.agencyname = Convert.ToString(row["AgencyName"]);
                vw.hagencyname = Convert.ToString(row["HAgencyName"]);
                vw.AgencyTypeId = Convert.ToString(row["tyaam_typeid"]);
                //vw.Fixed = Convert.ToString(row["Fixed"]);
                vw.CreatedBy = Convert.ToString(row["CreatedBy"]);
                vw.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                vw.ModifiedBy = Convert.ToString(row["ModifiedBy"]);
                vw.UserCode = Convert.ToString(row["UserCode"]);
                vw.ParentId = Convert.ToString(row["ParentId"]);
                vw.Ag_locationtype = Convert.ToString(row["Ag_locationtype"]);
                vw.Ag_location = Convert.ToString(row["Ag_location"]);
                vw.Ag_Address = Convert.ToString(row["Ag_Address"]);
                vw.Ag_Address1 = Convert.ToString(row["Ag_Address1"]);
                vw.Ag_StateId = Convert.ToString(row["Ag_StateId"]);
                vw.Ag_DistrictId = Convert.ToString(row["Ag_DistrictId"]);
                vw.Ag_BlockId = Convert.ToString(row["Ag_BlockId"]);
                vw.Ag_GramPanchayatId = Convert.ToString(row["Ag_GramPanchayatId"]);
                vw.ag_divisionid = Convert.ToString(row["ag_divisionid"]);
                vw.upload_photo_name = Convert.ToString(row["ag_photo_path"]);
                // vw.userid= Convert.ToString(row["userid"]);

             
                if (row["uploadpath"].ToString() != "")
                {
                    vw.uploadpath = Convert.ToString(row["uploadpath"]);
                }


                if (row["ag_photo_path"].ToString() != "")
                {


                    vw.ag_photo_path =  Convert.ToString(row["ag_photo_path"]);
                }
               

                vw.ag_first_name = Convert.ToString(row["ag_first_name"]);
                vw.ag_m_name = Convert.ToString(row["ag_m_name"]);
                vw.ag_l_name = Convert.ToString(row["ag_l_name"]);
                vw.ag_hfirst_name = Convert.ToString(row["ag_hfirst_name"]);
                vw.ag_hm_name = Convert.ToString(row["ag_hm_name"]);
                vw.ag_hl_name = Convert.ToString(row["ag_hl_name"]);
                vw.ag_address_city = Convert.ToString(row["ag_address_city"]);
                vw.ag_address_state = Convert.ToString(row["ag_address_state"]);

                vw.ag_pincode = Convert.ToString(row["ag_pincode"]);
                vw.ag_phone = Convert.ToString(row["ag_phone"]);
                vw.ag_alternative_phone = Convert.ToString(row["ag_alternative_phone"]);
                vw.ag_mobileno = Convert.ToString(row["ag_mobileno"]);
                vw.ag_alternative_mobileno = Convert.ToString(row["ag_alternative_mobileno"]);
                vw.ag_email = Convert.ToString(row["ag_email"]);
                vw.ag_alternative_email = Convert.ToString(row["ag_alternative_email"]);

                vw.ag_gender = Convert.ToString(row["ag_gender"]);
                if (row["ag_age"].ToString() != "")
                {
                    vw.ag_age = Convert.ToInt32(row["ag_age"]);
                }

                if (row["ag_dob"].ToString() != "")
                {
                    vw.ag_dob = Convert.ToDateTime(row["ag_dob"]).ToString("yyyy/MM/dd");
                }

                //vw.latitude = Convert.ToString(row["latitude"]);
                //vw.longitude = Convert.ToString(row["longitude"]);

                vw.ag_salutation = Convert.ToString(row["ag_salutation"]);
                vw.ag_aadhar = Convert.ToString(row["ag_aadhar"]);
                vw.ag_pan = Convert.ToString(row["ag_pan"]);
                vw.ag_gstin = Convert.ToString(row["ag_gstin"]);
                //if (Convert.ToString(row["tyaam_is_deleted"]) != "")
                //{
                //    vw.tyaam_is_deleted = Convert.ToInt32(row["tyaam_is_deleted"]);
                //}


                vw.ag_gstin = Convert.ToString(row["ag_gstin"]);
                vw.ag_sign_path = Convert.ToString(row["ag_sign_path"]);
                vw.tdds_tat_type_id = Convert.ToString(row["tdds_tat_type_id"]);
                vw.remark = Convert.ToString(row["tdds_remark"]);

                if (Convert.ToString(row["ag_salutation"]) != "")
                {
                    if (s.Where(o => o.ts_id == Convert.ToInt32(row["ag_salutation"])).Count() > 0)
                    {
                        vw.salutation_txt = s.Where(o => o.ts_id == Convert.ToInt32(row["ag_salutation"])).FirstOrDefault().ts_name;
                    }

                }




                //DataTable dtagencydetails = ds.Tables[1];
                //dtagencydetails.DefaultView.RowFilter = "tyaam_agencyid='" + row["AgencyId"] + "' and tyaam_typeid='" + row["tyaam_typeid"] + "'";
                //dtagencydetails = dtagencydetails.DefaultView.ToTable();



                AL.Add(vw);
            }





            return AL;
        }

        public List<Agency> Get_All_Agency_Name(string agencytypeid = null)
        {


            List<Agency> AL = new List<Agency>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //***********Code to get salutaion data for salutation text

            List<SALUTATION> s = new List<SALUTATION>();
            s = Get_SALUTATION();

            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();


            using (con)
            {
                SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_agency_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (agencytypeid != null)
                {
                    cmd.Parameters.AddWithValue("@agencytype", agencytypeid);
                }


                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                SqlDataReader row = cmd.ExecuteReader();


                while (row.Read())
                {
                    if (row["tyaam_status"].ToString() != "-1")
                    {
                        Agency vw = new Agency();
                        vw.totalcount = Convert.ToInt32(row["totalrecords"]);
                        vw.agencyid = Convert.ToString(row["AgencyId"]);
                        vw.tyaam_typeid = Convert.ToString(row["tyaam_typeid"]);
                        if (Convert.ToString(row["tyaam_status"]) != "")
                        {
                            vw.tyaam_status = Convert.ToInt32(row["tyaam_status"]);
                        }
                        else
                        {
                            vw.tyaam_status = 0;
                        }
                        vw.agencyname = Convert.ToString(row["AgencyName"]);
                        vw.hagencyname = Convert.ToString(row["HAgencyName"]);




                        vw.ag_salutation = Convert.ToString(row["ag_salutation"]);


                        if (Convert.ToString(row["ag_salutation"]) != "")
                        {
                            if (s.Where(o => o.ts_id == Convert.ToInt32(row["ag_salutation"])).Count() > 0)
                            {
                                vw.salutation_txt = s.Where(o => o.ts_id == Convert.ToInt32(row["ag_salutation"])).FirstOrDefault().ts_name;
                            }

                        }

                        vw.additionalInfo = new AgencyAdditionalInfo();
                        AL.Add(vw);
                    }
                }
            }

            con.Close();


            return AL;
        }


        public List<Agency> Get_Agency_by_charge(string chargeid)
        {


            List<Agency> AL = new List<Agency>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //***********Code to get salutaion data for salutation text


            List<Agency> agencynames = new List<Agency>();
            agencynames = Get_All_Agency_Name();


            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();


            using (con)
            {
                SqlCommand cmd = new SqlCommand("DMS.proc_dms_Get_employee_to_forward_doc_vr1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@charge", chargeid);
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                SqlDataReader row = cmd.ExecuteReader();


                while (row.Read())
                {

                    List<Agency> filteragency = agencynames.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["thdd_emp_id"]).ToUpper()).ToList();
                    if (filteragency.Count() > 0)
                    {
                        Agency vw = new Agency();
                        vw.agencyid = filteragency.FirstOrDefault().agencyid;
                        vw.agencyname = filteragency.FirstOrDefault().agencyname;
                        vw.hagencyname = filteragency.FirstOrDefault().hagencyname;
                        AL.Add(vw);
                    }

                }
            }

            con.Close();

            return AL;
        }

        public List<Agency> Get_Agency_Data(string agencytypeid, string agencyid, int pageno, int pagesize, string search, string tat_type_id = null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (search != null)
            {
                if (search != "")
                {
                    string[] sptsearch = search.Split(";".ToCharArray());
                    string[] sptsearchfields = sptsearch[0].Split(":".ToCharArray());
                    searchcolumn = sptsearchfields[0];
                    searchvalue = sptsearchfields[1];
                }
            }


            string filtername = "1";
            string filtervalue = CommonEnum.Agency_Active_Status;

            List<Agency> AL = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            AL = ABD.Get_Agency(agencytypeid, agencyid, pageno, pagesize, searchcolumn, searchvalue, filtername, filtervalue, tat_type_id);
            AL = AL.Where(o => o.agencyid.ToString().ToUpper() != CommonEnum.PortalAdmin_Agencyid.ToString().ToUpper()).ToList();
            AL = AL.Where(o => o.agencyid.ToString().ToUpper() != CommonEnum.SuperAdmin_Agencyid.ToString().ToUpper()).ToList();




            return AL;
        }

        public List<cast_category> Get_Cast_Category()
        {

            List<cast_category> f = new List<cast_category>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("YUser.proc_get_person_cast", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                f.Add(
                    new cast_category
                    {
                        id = Convert.ToString(dr["agencytype"]),
                        name = Convert.ToString(dr["agencyname"]),
                        hname = Convert.ToString(dr["HAgencyName"])
                       

                    });
            }

            return f;
        }


        public List<proc_ass_get_assignment_comment> Get_assignment_Comments(string assignmentid, string participantid)
        {
            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Assessment.proc_ass_get_assignment_comment_for_participant", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@taac_AssignmentID", assignmentid);
            if (participantid != null)
            {
                cmd.Parameters.AddWithValue("@Participantid", participantid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Participantid", DBNull.Value);
            }


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<proc_ass_get_assignment_comment> LI = new List<proc_ass_get_assignment_comment>();
            foreach (DataRow row in dt.Rows)
            {
                var ss = JsonConvert.DeserializeObject<assignmentparticipant[]>(row["participant"].ToString());
                // List<assignmentparticipant> arr =(List<assignmentparticipant>)(row["participant"].ToString());

                proc_ass_get_assignment_comment cm = new proc_ass_get_assignment_comment();
                cm.assignment = (string)row["assignmentid"].ToString();
                cm.participant = ss;
                LI.Add(cm);
            }



            return LI;
        }


        public bool Update_Profile_Data(Update_Profile_Data agency)
        {

            List<cast_category> f = new List<cast_category>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("YUser.proc_yuser_upd_profile_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@AgencyId", agency.AgencyId);
            cmd.Parameters.AddWithValue("@userid", agency.userid);
            cmd.Parameters.AddWithValue("@AgencyName", agency.AgencyName);
            cmd.Parameters.AddWithValue("@HAgencyName", agency.HAgencyName);
            cmd.Parameters.AddWithValue("@CreatedBy", agency.CreatedBy);

            cmd.Parameters.AddWithValue("@photopath", agency.photopath);
            cmd.Parameters.AddWithValue("@branchid", agency.branchid);
            cmd.Parameters.AddWithValue("@Ag_Address", agency.Ag_Address);
            cmd.Parameters.AddWithValue("@Ag_Address1", agency.Ag_Address1);
            cmd.Parameters.AddWithValue("@ag_address_city", agency.ag_address_city);
            cmd.Parameters.AddWithValue("@ag_address_state", agency.ag_address_state);
            cmd.Parameters.AddWithValue("@ag_pincode", agency.ag_pincode);
            cmd.Parameters.AddWithValue("@ag_salutation", agency.ag_salutation);

            cmd.Parameters.AddWithValue("@ag_first_name", agency.ag_first_name);
            cmd.Parameters.AddWithValue("@ag_m_name", agency.ag_m_name);
            cmd.Parameters.AddWithValue("@ag_l_name", agency.ag_l_name);
            cmd.Parameters.AddWithValue("@ag_hfirst_name", agency.ag_hfirst_name);
            cmd.Parameters.AddWithValue("@ag_hm_name", agency.ag_hm_name);
            cmd.Parameters.AddWithValue("@ag_hl_name", agency.ag_hl_name);
            cmd.Parameters.AddWithValue("@ag_gender", agency.ag_gender);
            cmd.Parameters.AddWithValue("@ag_age", agency.ag_age);
            cmd.Parameters.AddWithValue("@ag_dob", agency.ag_dob);
            cmd.Parameters.AddWithValue("@ag_phone", agency.ag_phone);

            cmd.Parameters.AddWithValue("@ag_alternative_phone", agency.ag_alternative_phone);
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", agency.ag_alternative_mobileno);
            cmd.Parameters.AddWithValue("@ag_alternative_email", agency.ag_alternative_email);
            cmd.Parameters.AddWithValue("@ag_aadhar", agency.ag_aadhar);
            cmd.Parameters.AddWithValue("@ag_pan", agency.ag_pan);
            cmd.Parameters.AddWithValue("@ag_gstn", agency.ag_gstn);
            cmd.Parameters.AddWithValue("@ag_sign_path", agency.ag_sign_path);


            cmd.ExecuteNonQuery();
            con.Close();
      
            return true;
        }



        public UserBranch Get_User_Branches(string userid)
        {
            UserBranch userBranch = new UserBranch();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_user_branch_roles", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@UserID", userid);
           
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();




            List<BranchType> bt = new List<BranchType>();
            bt = BranchTypes();
            List<Agency> branches= new List<Agency>();
            branches= Get_All_Agency_Name("00001,00002,00003,00004,00005");




            var distinctValues = dt.AsEnumerable()
                              .Select(row => row.Field<string>("tyubr_branch_type"))
                              .Distinct()
                              .ToList();

            List<BranchType> user_branch_type = new List<BranchType>();
            foreach (var value in distinctValues)
            {
                string branchname = "";
                string hbranchname = "";
                if (bt.Where(o => o.branchtypeid == value.ToString()).Count() > 0)
                {
                    branchname = bt.Where(o => o.branchtypeid == value.ToString()).FirstOrDefault().branchtype_name;
                    hbranchname = bt.Where(o => o.branchtypeid == value.ToString()).FirstOrDefault().branchtype_hname;
                }
                user_branch_type.Add(new BranchType { branchtypeid = value, branchtype_name = branchname, branchtype_hname = hbranchname });
            }




            List<Branches> user_branches = new List<Branches>();
            foreach (DataRow dr in dt.Rows)
            {
                string branchname = "";
                string hbranchname = "";
                if(branches.Where(o=>o.agencyid.ToString().ToUpper()== dr["tyubr_branch_id"].ToString().ToUpper()).Count() > 0)
                {
                    branchname = branches.Where(o => o.agencyid.ToString().ToUpper() == dr["tyubr_branch_id"].ToString().ToUpper()).FirstOrDefault().agencyname;
                    hbranchname = branches.Where(o => o.agencyid.ToString().ToUpper() == dr["tyubr_branch_id"].ToString().ToUpper()).FirstOrDefault().hagencyname;
                }

                Branches ub = new Branches();
                ub.branchtypeId = Convert.ToString(dr["tyubr_branch_type"]);
                ub.branchid = Convert.ToString(dr["tyubr_branch_id"]);
                ub.branch_name = branchname;
                ub.branch_hname = hbranchname;
                if(user_branches.Where(o=>o.branchid.ToString().ToUpper()== Convert.ToString(dr["tyubr_branch_id"]).ToUpper()).Count() <= 0)
                {
                    user_branches.Add(ub);
                }
              
               
            }

            userBranch.branchtype = user_branch_type.ToArray();
            userBranch.branches = user_branches.ToArray();

            return userBranch;
        }


        
        public List<BranchType> BranchTypes()
        {
            List<BranchType> userBranch = new List<BranchType>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.GetAgencyType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
         

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();





            foreach (DataRow dr in dt.Rows)
            {
                BranchType bt=new BranchType();
                bt.branchtypeid = Convert.ToString(dr["AgencyTypeID"]);
                bt.branchtype_name = Convert.ToString(dr["AgencyTypeName"]);
                bt.branchtype_hname = Convert.ToString(dr["HAgencyTypeName"]);
                userBranch.Add(bt);
                
            }

            return userBranch;
        }

        public root Get_Firebase_Token_Details(NotificationUsers users)
        {
            root user;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_firebase_token", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            string p1 = JsonConvert.SerializeObject(users);
            cmd.Parameters.AddWithValue("@agencyid", p1);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            user = JsonConvert.DeserializeObject<root>(dt.Rows[0][0].ToString());

            return user;
        }




    }
}
