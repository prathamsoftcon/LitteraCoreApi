namespace LitteraCore.Common.DMS
{
    public class DMS
    {
        public string? docno { get; set; }

        public string? tttds_info_desc { get; set; }

        public string doc_id { get; set; }

        public DateTime createdon { get; set; }

        public string createdby { get; set; }

        public string branchid { get; set; }

        public string? docremark { get; set; }

        public DateTime docdate { get; set; }

        public DateTime actiondate { get; set; }

        public string? CreatedBy_empid { get; set; }

        public string? fwd_empid { get; set; }

        public int tat_type_id { get; set; }

        public int doc_status { get; set; }

        public string? attached_doc { get; set; }
        public string? attached_doc_name { get; set; }

        public int doctype { get; set; }

        public string? draftletter { get; set; }

        public int tttds_is_final { get; set; }

        public int tttds_letter_type { get; set; }

        public string? docremarkenc { get; set; }

        public string? draftletterenc { get; set; }
    }
    public class DMSINFO
    {
        public string CreatedBy_empid { get; set; }

        public string fwd_empid { get; set; }
        public int tat_type_id { get; set; }
        public int doc_status { get; set; }
    }

    public class DOCTYPE
    {
        public string tat_type_id { get; set; }

        public string name { get; set; }
        public string hname { get; set; }

    }



    public class DOC_REMARK
    {
        public string tttds_doc_no { get; set; }

        public string tttds_fwd_empid { get; set; }
        public string tttds_sendby_empid { get; set; }

        public DateTime tttds_created_on { get; set; }

        public string tttds_remark { get; set; }

        public string receiver { get; set; }
        public string sendder { get; set; }
        public string tttds_doc_type { get; set; }
        public string tttds_doc_id { get; set; }
        public string tttds_process_id { get; set; }
        public string tttds_uploaded_doc { get; set; }
        public string tttds_uploaded_doc_name { get; set; }

        public string reciverdesignation { get; set; }
        public string hreciverdesignation { get; set; }
        public string hreceiver { get; set; }
        public string hsendder { get; set; }

        public string senderdesignation { get; set; }
        public string hsenderdesignation { get; set; }
        public string tttds_draft_letter { get; set; }
        public string tttds_tddlt_id { get; set; }

        public string tttds_is_final { get; set; }
        public string isenabled { get; set; }
        public string doc_letter_name { get; set; }

        public string tttds_remark_enc { get; set; }

    }


    public class DMS_DASHBOARD
    {
        public string Applicationid { get; set; }

        public string Ref_No { get; set; }
        public string Applicationno { get; set; }

        public string Applicationdate { get; set; }

        public string aplicationtypeid { get; set; }

        public string Applicationtype { get; set; }
        public string HApplicationtype { get; set; }
        public string AppCreationEmpid { get; set; }
        public string AppCreationEmpName { get; set; }
        public DateTime Receiveddate { get; set; }
        //public string tttds_uploaded_doc { get; set; }
        public string ReceivedEmpId { get; set; }

        public string ReceivedEmpname { get; set; }
        public string ReceivedFromEmpId { get; set; }
        public string ReceivedFromEmp { get; set; }
        public int AppCurrentstatusBit { get; set; }

        public string AppcurrentStatus { get; set; }
        public string AppcurrentHStatus { get; set; }
        public string PreviousRemark { get; set; }
        public int thada_handle_status { get; set; }

        public int isactionable { get; set; }
        public string ReceivedEmDesg { get; set; }
        public string ReceivedhEmDesg { get; set; }

        public string ReceivedfromEmDesg { get; set; }

        public string ReceivedfromhEmDesg { get; set; }

        public string ReceivedhEmpname { get; set; }

        public string ReceivedFromhEmp { get; set; }

        public string status_txt { get; set; }

    }


    public class DMS_ACTION_INFO
    {
        public string? docno { get; set; }

     
        public string doc_id { get; set; }

        public DateTime createdon { get; set; }

        public string senderid { get; set; }
        public string sendername { get; set; }

        public string receiverid { get; set; }
        public string receivername { get; set; }
        public string? docremark { get; set; }

      
        public int tat_type_id { get; set; }

        public int doc_status { get; set; }
        public string status_txt { get; set; }

       
    }
    public class Charges
    {
        public string id { get; set; }
        public string name { get; set; }
        public string hname { get; set; }
        public int isactive { get; set; }
    }
}
