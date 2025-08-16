using LitteraCore.BLContext;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using static LitteraCore.Common.CommonEnum;

namespace LitteraCore.Common.DMS
{
    public class DMSDB
    {
        private readonly IConfiguration _configuration;
        public DMSDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool INS_UPD_DMS(DMS D, SqlConnection con, SqlTransaction transaction = null)
        {
            if (D.docno == null)
            {
                D.docno = GET_DMS_CODE_NO(D.doc_id, D.tat_type_id, D.branchid, "", "YEAR");
            }
           
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
            //if (con.State != ConnectionState.Open) { con.Open(); }
            if (con.State == ConnectionState.Closed)
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
            }
            SqlCommand cmd = new SqlCommand("DMS.proc_dms_Ins_upd_doc_status", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            cmd.CommandTimeout = 5000;
            if (D.docno != null)
            {
                cmd.Parameters.AddWithValue("@doc_no", D.docno);
            }
            else
            {
                cmd.Parameters.AddWithValue("@doc_no", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@tttds_info_desc", D.tttds_info_desc);
            cmd.Parameters.AddWithValue("@doc_id", D.doc_id);
            cmd.Parameters.AddWithValue("@createdon", D.createdon.ToString("yyyy/MM/dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@createdby", D.createdby);
            cmd.Parameters.AddWithValue("@branchid", D.branchid);
            cmd.Parameters.AddWithValue("@docremark", D.docremark);
            cmd.Parameters.AddWithValue("@docdate", D.docdate.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@actiondate", D.actiondate.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@CreatedBy_empid", D.CreatedBy_empid);
            if (D.fwd_empid != null)
            {
                cmd.Parameters.AddWithValue("@fwd_empid", D.fwd_empid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@fwd_empid", D.CreatedBy_empid);
            }

            cmd.Parameters.AddWithValue("@tat_type_id", D.tat_type_id);
            cmd.Parameters.AddWithValue("@doc_status", D.doc_status);
            cmd.Parameters.AddWithValue("@attached_doc", D.attached_doc);
            cmd.Parameters.AddWithValue("@attached_doc_name", D.attached_doc_name);
            cmd.Parameters.AddWithValue("@doctype", D.doctype);
            cmd.Parameters.AddWithValue("@draftletter", D.draftletter);
            cmd.Parameters.AddWithValue("@tttds_is_final", D.tttds_is_final);
            cmd.Parameters.AddWithValue("@tttds_letter_type", D.tttds_letter_type);
            cmd.Parameters.AddWithValue("@docremarkenc", D.docremarkenc);
            cmd.Parameters.AddWithValue("@draftletterenc", D.draftletterenc);

            cmd.ExecuteNonQuery();
            //con.Close();

            return true;
        }

        public string GENERATE_DOC_NO(string docdate, string branchid, int tat_type_id, string prefix, string repeaton)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
          
            if (con.State != ConnectionState.Open) { con.Open(); }
            string docno = "";
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select [DMS].[f_dms_doc_ref_no]('" + docdate + "','" + branchid + "','" + tat_type_id + "','" + prefix + "','" + repeaton + "') ", con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            docno = (string)cmd.ExecuteScalar();
            return docno;
        }


        public bool IS_DMS_EXIST(string docid, int tat_type_id)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("DMS.proc_dms_get_doc_ref_no", con);
            cmd.Parameters.AddWithValue("@tat_type_id", tat_type_id);
            cmd.Parameters.AddWithValue("@docid", docid);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public string GET_DMS_CODE_NO(string docid, int tat_type_id,string  branchid,string prefix,string repeaton)
        {
            string codeno = "";
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("DMS.proc_dms_get_doc_ref_no", con);
            cmd.Parameters.AddWithValue("@tat_type_id", tat_type_id);
            cmd.Parameters.AddWithValue("@docid", docid);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                codeno = Convert.ToString(dt.Rows[0]["tdds_doc_no"]);
            }
            else
            {
                codeno = GENERATE_DOC_NO(System.DateTime.Now.ToString("yyyy/MM/dd"),branchid, tat_type_id, prefix, repeaton);
            }
            return codeno;
        }


        public List<DMS> GET_DMS_STATUS_DATA(string tdds_doc_id, int tdds_tat_type_id)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (tdds_doc_id != null)
            {
                cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_no,tdds_doc_id,tdds_doc_id,tdds_status from DMS.VW_dms_doc_last_status where tdds_doc_id='" + tdds_doc_id + "' and  tdds_tat_type_id='" + tdds_tat_type_id.ToString() + "'", con);
            }
            else
            {
                cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_no,tdds_doc_id,tdds_doc_id,tdds_status from DMS.VW_dms_doc_last_status where  tdds_tat_type_id='" + tdds_tat_type_id.ToString() + "'", con);
            }


            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<DMS> DL = new List<DMS>();
            foreach (DataRow row in dt.Rows)
            {
                DMS D = new DMS();
                D.tat_type_id = Convert.ToInt32(row["tdds_tat_type_id"]);
                D.docno = Convert.ToString(row["tdds_doc_no"]);
                D.doc_id = Convert.ToString(row["tdds_doc_id"]);
                D.doc_id = Convert.ToString(row["tdds_doc_id"]);
                D.doc_status = Convert.ToInt32(row["tdds_status"]);
                DL.Add(D);
            }

            return DL;


        }



        public DataTable GET_DOC_DEFAULT_EMP(string tat_type_id)
        {
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            string docno = "";
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("dms.proc_get_tbl_dms_default_doc_user", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tat_type_id", tat_type_id);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }



        public string GENERATE_AGENCY_USER_CODE(string docdate, string branchid, string prefix, string repeaton)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            string docno = "";
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select [DMS].[f_dms_doc_ref_no_for_agency]('" + docdate + "','" + branchid + "','" + prefix + "','" + repeaton + "') ", con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            docno = (string)cmd.ExecuteScalar();
            return docno;
        }



        public DataTable CHECK_DMS_CODE(string agencyid)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("YUser.proc_yuser_get_agency_vr1", con);
            cmd.Parameters.AddWithValue("@agencyid", agencyid);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;


        }


        public List<DOCTYPE> GET_DMS_DOC_TYPE()
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("dms.proc_dms_get_sam_formfees_vr1", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dt.DefaultView.Sort = "ApplicationName asc";
            dt = dt.DefaultView.ToTable();
            List<DOCTYPE> DL = new List<DOCTYPE>();
            foreach (DataRow row in dt.Rows)
            {
                DOCTYPE D = new DOCTYPE();
                D.tat_type_id = Convert.ToString(row["tat_type_id"]);
                D.name = Convert.ToString(row["ApplicationName"]);
                D.hname = Convert.ToString(row["HApplicatioName"]);

                DL.Add(D);
            }

            return DL;


        }


        public List<DOC_REMARK> GET_DMS_DOC_REMARK(string docid, string doctype = null)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("TrainingPlan.proc_tp_get_doc_remark_vr_1", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@docid", docid);
            cmd.Parameters.AddWithValue("@date", System.DateTime.UtcNow.ToString("yyyy/MM/dd"));
            if (doctype != null)
            {
                cmd.Parameters.AddWithValue("@tttds_doc_type", doctype);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dt.DefaultView.Sort = "tttds_created_on asc";
            dt = dt.DefaultView.ToTable();
            List<DOC_REMARK> DL = new List<DOC_REMARK>();
            foreach (DataRow row in dt.Rows)
            {
                DOC_REMARK D = new DOC_REMARK();
                D.tttds_doc_no = Convert.ToString(row["tttds_doc_no"]);
                D.tttds_fwd_empid = Convert.ToString(row["tttds_fwd_empid"]);
                D.tttds_sendby_empid = Convert.ToString(row["tttds_sendby_empid"]);
                if (Convert.ToString(row["tttds_created_on"]) != "")
                {
                    D.tttds_created_on = Convert.ToDateTime(row["tttds_created_on"]);
                }

                D.tttds_remark = Convert.ToString(row["tttds_remark"]);
                //D.receiver = Convert.ToString(row["receiver"]);
                // D.sendder = Convert.ToString(row["sendder"]);
                D.tttds_doc_type = Convert.ToString(row["tttds_doc_type"]);

                D.tttds_doc_id = Convert.ToString(row["tttds_doc_id"]);
                D.tttds_process_id = Convert.ToString(row["tttds_process_id"]);
                D.tttds_uploaded_doc = Convert.ToString(row["tttds_uploaded_doc"]);
                D.tttds_uploaded_doc_name = Convert.ToString(row["tttds_uploaded_doc_name"]);
                D.reciverdesignation = Convert.ToString(row["reciverdesignation"]);

                // D.hreciverdesignation = Convert.ToString(row["hreciverdesignation"]);
                // D.hreceiver = Convert.ToString(row["hreceiver"]);
                // D.hsendder = Convert.ToString(row["hsendder"]);
                // D.senderdesignation = Convert.ToString(row["senderdesignation"]);


                // D.hsenderdesignation = Convert.ToString(row["hsenderdesignation"]);
                D.tttds_draft_letter = Convert.ToString(row["tttds_draft_letter"]);
                D.tttds_is_final = Convert.ToString(row["tttds_is_final"]);
                D.isenabled = Convert.ToString(row["isenabled"]);
                D.doc_letter_name = Convert.ToString(row["doc_letter_name"]);
                D.tttds_remark_enc = Convert.ToString(row["tttds_remark_enc"]);

                DL.Add(D);
            }

            return DL;


        }

        public List<DMS_DASHBOARD> GET_DMS_DASHBOARD_DATA(string employeeid, string branchid, string fromdate, string todate, string applicationtypeid, string status, string searchempid)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("TrainingPlan.proc_tp_get_dash_board_details", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@employeeid", employeeid);
            cmd.Parameters.AddWithValue("@Branchid", branchid);
            cmd.Parameters.AddWithValue("@from_date", fromdate);
            cmd.Parameters.AddWithValue("@To_date", todate);
            if (applicationtypeid != null)
            {
                cmd.Parameters.AddWithValue("@App_type_id", applicationtypeid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@App_type_id", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@procfor", 1);
            if (status != null)
            {
                cmd.Parameters.AddWithValue("@status", status);
            }
            else
            {
                cmd.Parameters.AddWithValue("@status", "00000");
            }

            if (searchempid != null)
            {
                cmd.Parameters.AddWithValue("@searchemployee", searchempid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@searchemployee", DBNull.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            List<DMS_DASHBOARD> DL = new List<DMS_DASHBOARD>();
            foreach (DataRow row in dt.Rows)
            {
                DMS_DASHBOARD D = new DMS_DASHBOARD();
                D.Applicationid = Convert.ToString(row["Applicationid"]);
                D.Ref_No = Convert.ToString(row["Ref_No"]);
                D.Applicationno = Convert.ToString(row["Applicationno"]);
                D.Applicationdate = Convert.ToString(row["Applicationdate"]);
                D.aplicationtypeid = Convert.ToString(row["aplicationtypeid"]);
                D.Applicationtype = Convert.ToString(row["Applicationtype"]);
                D.HApplicationtype = Convert.ToString(row["HApplicationtype"]);
                D.AppCreationEmpid = Convert.ToString(row["AppCreationEmpid"]);
                D.AppCreationEmpName = Convert.ToString(row["AppCreationEmpName"]);

                D.Receiveddate = Convert.ToDateTime(row["Receiveddate"]);
                // D.tttds_uploaded_doc = Convert.ToString(row["tttds_uploaded_doc"]);
                D.ReceivedEmpId = Convert.ToString(row["ReceivedEmpId"]);
                D.ReceivedEmpname = Convert.ToString(row["ReceivedEmpname"]);

                D.ReceivedFromEmpId = Convert.ToString(row["ReceivedFromEmpId"]);
                D.ReceivedFromEmp = Convert.ToString(row["ReceivedFromEmp"]);
                D.AppCurrentstatusBit = Convert.ToInt32(row["AppCurrentstatusBit"]);
                D.AppcurrentStatus = Convert.ToString(row["AppcurrentStatus"]);
                D.AppcurrentHStatus = Convert.ToString(row["AppcurrentHStatus"]);
                D.PreviousRemark = Convert.ToString(row["PreviousRemark"]);
                D.thada_handle_status = Convert.ToInt32(row["thada_handle_status"]);
                //D.isactioable = Convert.ToInt32(row["isactioable"]);
                D.ReceivedEmDesg = Convert.ToString(row["ReceivedEmDesg"]);
                D.ReceivedhEmDesg = Convert.ToString(row["ReceivedhEmDesg"]);
                D.ReceivedfromEmDesg = Convert.ToString(row["ReceivedfromEmDesg"]);
                D.ReceivedfromhEmDesg = Convert.ToString(row["ReceivedfromhEmDesg"]);
                D.ReceivedhEmpname = Convert.ToString(row["ReceivedhEmpname"]);
                D.ReceivedFromhEmp = Convert.ToString(row["ReceivedFromhEmp"]);
                DL.Add(D);
            }

            return DL;


        }


        //public List<DMS_DASHBOARD> GET_DMS_DATA_WITH_DATE(string fromdate, string todate, string employeeid, string documentno, string applicationtypeid, string searchempid)
        //{
        //    string connectionString = _configuration.GetConnectionString("LitteraDatabase");
        //    SqlConnection con = new SqlConnection(connectionString);
        //    if (con.State != ConnectionState.Open) { con.Open(); }
        //    DataTable dt = new DataTable();
        //    List<Agency> a = new List<Agency>();
        //    AgencyBL abl = new AgencyBL();
        //    a = abl.Get_Agency_Data("00008", null, 0, 0, null);


        //    SqlCommand cmd = new SqlCommand();
        //    if (documentno != null)
        //    {
        //        cmd = new SqlCommand("select * from DMS.VW_dms_doc_last_status where tdds_doc_no='" + documentno + "'", con);
        //    }
        //    else
        //    {
        //        if (searchempid != null)
        //        {
        //            cmd = new SqlCommand("select * from DMS.VW_dms_doc_last_status where tdds_created_on >= '" + fromdate + "' and  tdds_created_on <=dateadd( day,1,'" + todate + "') and tdds_fwd_empid='" + searchempid + "'", con);
        //        }
        //        else
        //        {
        //            cmd = new SqlCommand("select * from DMS.VW_dms_doc_last_status where tdds_created_on >= '" + fromdate + "' and  tdds_created_on <=dateadd( day,1,'" + todate + "')", con);
        //        }
        //    }



        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandTimeout = 5000;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    if (applicationtypeid != null)
        //    {
        //        dt.DefaultView.RowFilter = "tdds_tat_type_id='" + applicationtypeid + "'";
        //        dt = dt.DefaultView.ToTable();
        //    }

        //    List<DMS_DASHBOARD> DL = new List<DMS_DASHBOARD>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        DMS_DASHBOARD D = new DMS_DASHBOARD();
        //        D.Applicationid = Convert.ToString(row["tdds_doc_id"]);

        //        D.Applicationno = Convert.ToString(row["tdds_doc_no"]);
        //        if (Convert.ToString(row["tdds_doc_date"]) != "")
        //        {
        //            D.Applicationdate = Convert.ToDateTime(row["tdds_doc_date"]).ToString("dd/MM/yyyy");
        //        }

        //        D.aplicationtypeid = Convert.ToString(row["tdds_tat_type_id"]);
        //        D.Applicationtype = Convert.ToString(row["tddi_description"]);
        //        D.HApplicationtype = Convert.ToString(row["tddi_hdescription"]);
        //        D.AppCreationEmpid = Convert.ToString(row["tdds_sendby_empid"]);
        //        if (a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().Count > 0)
        //        {
        //            D.AppCreationEmpName = a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().FirstOrDefault().AgencyName;
        //        }

        //        D.Receiveddate = Convert.ToDateTime(row["tdds_created_on"]);

        //        D.ReceivedEmpId = Convert.ToString(row["tdds_fwd_empid"]);
        //        //D.ReceivedEmpname = Convert.ToString(row["ReceivedEmpname"]);
        //        if (a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().Count > 0)
        //        {
        //            D.ReceivedEmpname = a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().FirstOrDefault().AgencyName;
        //        }

        //        D.ReceivedFromEmpId = Convert.ToString(row["tdds_sendby_empid"]);

        //        if (a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().Count > 0)
        //        {
        //            D.ReceivedFromEmp = a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().FirstOrDefault().AgencyName;
        //        }
        //        D.AppCurrentstatusBit = Convert.ToInt32(row["tdds_status"]);
        //        //D.AppcurrentStatus = Convert.ToString(row["AppcurrentStatus"]);
        //        //D.AppcurrentHStatus = Convert.ToString(row["AppcurrentHStatus"]);
        //        D.PreviousRemark = Convert.ToString(row["tdds_remark"]);
        //        // D.thada_handle_status = Convert.ToInt32(row["thada_handle_status"]);

        //        //D.isactioable = Convert.ToInt32(row["isactioable"]);
        //        if (a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().Count > 0)
        //        {
        //            D.ReceivedhEmpname = a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().FirstOrDefault().AgencyName;
        //            D.ReceivedFromhEmp = a.Where(o => o.AgencyId.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().FirstOrDefault().HAgencyName;
        //        }


        //        DL.Add(D);
        //    }

        //    return DL;


        //}



        //public List<Charges> GET_CHARGES()
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
        //    if (con.State != ConnectionState.Open) { con.Open(); }
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("yuser.proc_hr_tbl_charge_master", con);

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandTimeout = 5000;


        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    List<Charges> DL = new List<Charges>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        Charges D = new Charges();
        //        D.name = Convert.ToString(row["thcm_name"]);
        //        D.hname = Convert.ToString(row["thcm_hname"]);
        //        D.id = Convert.ToString(row["thcm_id"]);
        //        D.isactive = Convert.ToInt32(row["thcm_active"]);

        //        DL.Add(D);
        //    }

        //    return DL;


        //}


        //public List<Charges> GET_EMP_CHARGES(string employeeid)
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
        //    if (con.State != ConnectionState.Open) { con.Open(); }
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("yuser.proc_yuser_get_hr_delegated_department", con);
        //    cmd.Parameters.AddWithValue("@procedurefor", "1");
        //    cmd.Parameters.AddWithValue("@thdd_emp_id", employeeid);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandTimeout = 5000;


        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    List<Charges> DL = new List<Charges>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        Charges D = new Charges();
        //        D.name = Convert.ToString(row["thcm_name"]);
        //        D.hname = Convert.ToString(row["thcm_hname"]);
        //        D.id = Convert.ToString(row["thdd_charge_id"]);
        //        D.isactive = 1;

        //        DL.Add(D);
        //    }

        //    return DL;


        //}


        public List<DMS_ACTION_INFO> GET_DMS_Doc_Action_DATA(string tdds_doc_id, int tdds_tat_type_id)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_no,tdds_doc_id,tdds_created_on,tdds_status,tdds_sendby_empid,tdds_fwd_empid,tdds_remark,tdds_status from dms.VW_dms_doc_all_status where tdds_doc_id='" + tdds_doc_id + "' and tdds_tat_type_id='"+ tdds_tat_type_id + "' order by tdds_process_id asc", con);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            AgencyDB ADB = new AgencyDB(_configuration);
            List<Agency> agency = ADB.Get_Agency(CommonEnum.Agencytype_Staff, null, 1, 0,null,null,null,null,null);

            List<DMS_ACTION_INFO> DL = new List<DMS_ACTION_INFO>();
            foreach (DataRow row in dt.Rows)
            {
                DMS_ACTION_INFO D = new DMS_ACTION_INFO();
                D.tat_type_id = Convert.ToInt32(row["tdds_tat_type_id"]);
                D.docno = Convert.ToString(row["tdds_doc_no"]);
                D.doc_id = Convert.ToString(row["tdds_doc_id"]);
                D.createdon = Convert.ToDateTime(row["tdds_created_on"]);
                D.doc_status = Convert.ToInt32(row["tdds_status"]);

                D.senderid = Convert.ToString(row["tdds_sendby_empid"]);
                D.receiverid = Convert.ToString(row["tdds_fwd_empid"]);
                D.docremark = Convert.ToString(row["tdds_remark"]);
                D.doc_status = Convert.ToInt16(row["tdds_status"]);

                //*******Get empname and sender and receivername
                if(agency.Where(o => o.agencyid.ToString().ToUpper() == D.senderid.ToString().ToUpper()).Count() > 0)
                {
                    D.sendername = agency.Where(o => o.agencyid.ToString().ToUpper() == D.senderid.ToString().ToUpper()).FirstOrDefault().agencyname;
                }
                if (agency.Where(o => o.agencyid.ToString().ToUpper() == D.receiverid.ToString().ToUpper()).Count() > 0)
                {
                    D.receiverid = agency.Where(o => o.agencyid.ToString().ToUpper() == D.receiverid.ToString().ToUpper()).FirstOrDefault().agencyname;
                }
                if(D.doc_status != null)
                {
                    D.status_txt = Enum.GetName(typeof(CommonEnum.FunctionStatus), D.doc_status);
                }
                DL.Add(D);
            }

            return DL;


        }

        public List<DMS_DASHBOARD> GET_DMS_LAST_STATUS_DATA(string fromdate, string todate, string documentno, string applicationtypeid, string employeeid)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString); if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            List<Agency> a = new List<Agency>();
            AgencyDB ADB = new AgencyDB(_configuration);
            List<Agency> agency = ADB.Get_Agency(CommonEnum.Agencytype_Staff, null, 1, 0, null, null, null, null, null);
            a = agency;

            SqlCommand cmd = new SqlCommand();
            if (documentno != null)
            {
                cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_id,tdds_doc_no,tdds_doc_date,tdds_tat_type_id,tddi_description,tddi_hdescription,tdds_sendby_empid,tdds_created_on,tdds_fwd_empid,tdds_status,tdds_remark from DMS.VW_dms_doc_last_status where tdds_doc_no='" + documentno + "'", con);
            }
            else
            {
                if (employeeid != null)
                {
                    cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_id,tdds_doc_no,tdds_doc_date,tdds_tat_type_id,tddi_description,tddi_hdescription,tdds_sendby_empid,tdds_created_on,tdds_fwd_empid,tdds_status,tdds_remark from DMS.VW_dms_doc_last_status where tdds_created_on >= '" + fromdate + "' and  tdds_created_on <=dateadd( day,1,'" + todate + "') and tdds_fwd_empid='" + employeeid + "'", con);
                }
                else
                {
                    cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_id,tdds_doc_no,tdds_doc_date,tdds_tat_type_id,tddi_description,tddi_hdescription,tdds_sendby_empid,tdds_created_on,tdds_fwd_empid,tdds_status,tdds_remark from DMS.VW_dms_doc_last_status where tdds_created_on >= '" + fromdate + "' and  tdds_created_on <=dateadd( day,1,'" + todate + "')", con);
                }
            }



            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (applicationtypeid != null)
            {
                dt.DefaultView.RowFilter = "tdds_tat_type_id='" + applicationtypeid + "'";
                dt = dt.DefaultView.ToTable();
            }

            List<DMS_DASHBOARD> DL = new List<DMS_DASHBOARD>();
            foreach (DataRow row in dt.Rows)
            {
                DMS_DASHBOARD D = new DMS_DASHBOARD();
                D.Applicationid = Convert.ToString(row["tdds_doc_id"]);

                D.Applicationno = Convert.ToString(row["tdds_doc_no"]);
                if (Convert.ToString(row["tdds_doc_date"]) != "")
                {
                    D.Applicationdate = Convert.ToDateTime(row["tdds_doc_date"]).ToString("dd/MM/yyyy");
                }

                D.aplicationtypeid = Convert.ToString(row["tdds_tat_type_id"]);
                D.Applicationtype = Convert.ToString(row["tddi_description"]);
                D.HApplicationtype = Convert.ToString(row["tddi_hdescription"]);
                D.AppCreationEmpid = Convert.ToString(row["tdds_sendby_empid"]);
                if (a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().Count > 0)
                {
                    D.AppCreationEmpName = a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().FirstOrDefault().agencyname;
                }

                D.Receiveddate = Convert.ToDateTime(row["tdds_created_on"]);

                D.ReceivedEmpId = Convert.ToString(row["tdds_fwd_empid"]);
                //D.ReceivedEmpname = Convert.ToString(row["ReceivedEmpname"]);
                if (a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().Count > 0)
                {
                    D.ReceivedEmpname = a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().FirstOrDefault().agencyname;
                }

                D.ReceivedFromEmpId = Convert.ToString(row["tdds_sendby_empid"]);

                if (a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().Count > 0)
                {
                    D.ReceivedFromEmp = a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_sendby_empid"]).ToUpper()).ToList().FirstOrDefault().agencyname;
                }
                D.AppCurrentstatusBit = Convert.ToInt32(row["tdds_status"]);
                D.PreviousRemark = Convert.ToString(row["tdds_remark"]);
               
                if (a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().Count > 0)
                {
                    D.ReceivedhEmpname = a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().FirstOrDefault().agencyname;
                    D.ReceivedFromhEmp = a.Where(o => o.agencyid.ToString().ToUpper() == Convert.ToString(row["tdds_fwd_empid"]).ToUpper()).ToList().FirstOrDefault().agencyname;
                }

                if (D.AppCurrentstatusBit != null)
                {
                    D.status_txt = Enum.GetName(typeof(CommonEnum.FunctionStatus), D.AppCurrentstatusBit);
                }
                DL.Add(D);
            }

            return DL;


        }

        public List<Charges> GET_CHARGES()
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("yuser.proc_hr_tbl_charge_master", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            List<Charges> DL = new List<Charges>();
            foreach (DataRow row in dt.Rows)
            {
                Charges D = new Charges();
                D.name = Convert.ToString(row["thcm_name"]);
                D.hname = Convert.ToString(row["thcm_hname"]);
                D.id = Convert.ToString(row["thcm_id"]);
                D.isactive = Convert.ToInt32(row["thcm_active"]);

                DL.Add(D);
            }

            return DL;


        }


        public List<DMS> GET_DMS_STATUS_DATA_FOR_SELECTED_DOCID(string tdds_doc_id, int tdds_tat_type_id)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            if (tdds_doc_id != null)
            {
                cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_no,tdds_doc_id,tdds_doc_id,tdds_status from DMS.VW_dms_doc_last_status where tdds_doc_id in (" + tdds_doc_id + ") and  tdds_tat_type_id='" + tdds_tat_type_id.ToString() + "'", con);
            }
            else
            {
                cmd = new SqlCommand("select tdds_tat_type_id,tdds_doc_no,tdds_doc_id,tdds_doc_id,tdds_status from DMS.VW_dms_doc_last_status where  tdds_tat_type_id in (" + tdds_tat_type_id.ToString() + ")", con);
            }


            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<DMS> DL = new List<DMS>();
            foreach (DataRow row in dt.Rows)
            {
                DMS D = new DMS();
                D.tat_type_id = Convert.ToInt32(row["tdds_tat_type_id"]);
                D.docno = Convert.ToString(row["tdds_doc_no"]);
                D.doc_id = Convert.ToString(row["tdds_doc_id"]);
                D.doc_id = Convert.ToString(row["tdds_doc_id"]);
                D.doc_status = Convert.ToInt32(row["tdds_status"]);
                DL.Add(D);
            }

            return DL;


        }


    }
}
