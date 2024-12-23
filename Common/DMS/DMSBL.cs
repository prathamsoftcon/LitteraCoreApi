using Microsoft.Data.SqlClient;
using LitteraCore.Common.DMS;
using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.Common.DMS
{
    public class DMSBL
    {
        private readonly IConfiguration _configuration;
        public DMSBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Save_DMS_DATA(DMS D, SqlConnection con, SqlTransaction transaction = null)
        {
            DMSDB DDB = new DMSDB(_configuration);
            DDB.INS_UPD_DMS(D, con, transaction);
            return true;
        }
        public string Get_doc_no(string docdate, string branchid, int tat_type_id, string prefix, string repeaton)
        {
            DMSDB DDB = new DMSDB(_configuration);
            string docno = DDB.GENERATE_DOC_NO(docdate, branchid, tat_type_id, prefix, repeaton);
            return docno;
        }
        public bool CHECK_DMS(string docid, int tat_type_id)
        {
            DMSDB DDB = new DMSDB(_configuration);

            return DDB.IS_DMS_EXIST(docid, tat_type_id);
        }
        public int Get_DMS_DOC_STATUS(string tdds_doc_id, int tdds_tat_type_id)
        {
            DMSDB DDB = new DMSDB(_configuration);
            List<DMS> DL = new List<DMS>();
            DL = DDB.GET_DMS_STATUS_DATA(tdds_doc_id, tdds_tat_type_id);
            return DL.FirstOrDefault().doc_status;
        }


        public List<DMS> Get_DMS_STATUS(string tdds_doc_id, int tdds_tat_type_id)
        {
            DMSDB DDB = new DMSDB(_configuration);
            List<DMS> DL = new List<DMS>();
            DL = DDB.GET_DMS_STATUS_DATA(tdds_doc_id, tdds_tat_type_id);
            return DL;
        }

        //public bool Update_DMS_DATA(DMS D)
        //{
        //    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["LitteraAPIstr"].ConnectionString);
        //    DMSDB DDB = new DMSDB(_configuration);
        //    DDB.INS_UPD_DMS(D, con1, null);
        //    return true;
        //}


        public string Get_agency_doc_no(string docdate, string branchid, string prefix, string repeaton)
        {
            DMSDB DDB = new DMSDB(_configuration);
            string docno = DDB.GENERATE_AGENCY_USER_CODE(docdate, branchid, prefix, repeaton);
            return docno;
        }
        public List<DOCTYPE> GET_DMS_DOC_TYPE()
        {
            DMSDB DDB = new DMSDB(_configuration);
            List<DOCTYPE> DL = new List<DOCTYPE>();
            DL = DDB.GET_DMS_DOC_TYPE();
            return DL;
        }
        public List<DOC_REMARK> GET_DMS_DOC_REMARK(string docid, string doctype = null)
        {
            DMSDB DDB = new DMSDB(_configuration);
            List<DOC_REMARK> DL = new List<DOC_REMARK>();
            DL = DDB.GET_DMS_DOC_REMARK(docid, doctype);
            return DL;
        }

        public string Get_dms_doc_no(string docid, int tat_type_id, string branchid, string prefix, string repeaton)
        {
            DMSDB DDB = new DMSDB(_configuration);
            string docno = DDB.GET_DMS_CODE_NO(docid,tat_type_id,branchid,prefix,repeaton);
            return docno;
        }


        public List<DMS_ACTION_INFO> Get_Action_Info(string docid, int tat_type_id)
        {
            DMSDB DDB = new DMSDB(_configuration);
            List<DMS_ACTION_INFO> AI=new List<DMS_ACTION_INFO>();
            AI = DDB.GET_DMS_Doc_Action_DATA(docid, tat_type_id);
            return AI;
        }
        public List<DMS_DASHBOARD> Get_DMS_Data(string fromdate, string todate, string documentno, string applicationtypeid, string employeeid)
        {
            DMSDB DDB = new DMSDB(_configuration);
            List<DMS_DASHBOARD> AI = new List<DMS_DASHBOARD>();
            AI = DDB.GET_DMS_LAST_STATUS_DATA(fromdate, todate, documentno, applicationtypeid, employeeid);
            return AI;
        }
        public List<Charges> Get_Charges()
        {
            List<Charges> c = new List<Charges>();
            DMSDB DDB = new DMSDB(_configuration);
            c = DDB.GET_CHARGES();
            return c;
        }

        public bool Update_DMS_DATA(DMS D)
        {
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con1 = new SqlConnection(connectionString);
            DMSDB DDB = new DMSDB(_configuration);
            DDB.INS_UPD_DMS(D, con1, null);
            return true;
        }

    }
}
