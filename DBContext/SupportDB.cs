using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography.Xml;
using static LitteraCore.Models.MaskInfo;

namespace LitteraCore.DBContext
{
    public class SupportDB                                                        
    {
        private readonly IConfiguration _configuration;
        public SupportDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Insert_Support(Support S)
        {
            string referenceid = "";
            List<cast_category> f = new List<cast_category>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Masterconfig");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DBO.proc_insert_support_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@id", S.id);
            cmd.Parameters.AddWithValue("@clienturl", S.clienturl);
            cmd.Parameters.AddWithValue("@name", S.name);
            cmd.Parameters.AddWithValue("@mobileno", S.mobileno);
            cmd.Parameters.AddWithValue("@email", S.email);
            cmd.Parameters.AddWithValue("@description", S.description);
            cmd.Parameters.AddWithValue("@createdon", System.DateTime.Now.ToString("yyyy/MM/dd hh:MM:ss"));
            if (S.createdby != null)
            {
                cmd.Parameters.AddWithValue("@createdby", S.createdby);
            }
            else
            {
                cmd.Parameters.AddWithValue("@createdby", DBNull.Value);
            }
            if (S.upload_path != null)
            {
                cmd.Parameters.AddWithValue("@upload_path", S.upload_path);
            }
            else
            {
                cmd.Parameters.AddWithValue("@upload_path", DBNull.Value);
            }




            referenceid = cmd.ExecuteScalar().ToString();
            con.Close();

            return referenceid;
        }

        public List<Support> GetSupportQuery()
        {

            List<Support> L = new List<Support>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Masterconfig");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_get_tbl_client_support_data", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                string? repoliedon = null;
                if (dr["reply_by"].ToString() != "")
                {
                    repoliedon = Convert.ToString(dr["reply_by"]);
                }
                L.Add(
                    new Support
                    {
                        id = Convert.ToString(dr["id"]),
                        referenceno = Convert.ToInt32(dr["referenceno"]),
                        clienturl = Convert.ToString(dr["clienturl"]),
                        name = Convert.ToString(dr["name"]),
                        mobileno = Convert.ToString(dr["mobileno"]),
                        email = Convert.ToString(dr["email"]),
                        description = Convert.ToString(dr["description"]),
                        upload_path = Convert.ToString(dr["upload_path"]),
                        createdon = Convert.ToString(dr["createdon"]),
                        createdby = Convert.ToString(dr["createdon"]),
                        replied = Convert.ToInt32(dr["replied"]),
                        reply_txt = Convert.ToString(dr["reply_txt"]),
                        reply_by = Convert.ToString(dr["reply_by"]),
                        repliedOn = repoliedon
                    }
                    );
            }

            return L;
        }

        public bool Update_Status(Update_Support us)
        {

            List<Support> L = new List<Support>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("Masterconfig");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("dbo.proc_update_reply_status", con);
            cmd.Parameters.AddWithValue("@id", us.id);
            cmd.Parameters.AddWithValue("@status", us.status);
            cmd.Parameters.AddWithValue("@remark", us.remark);
            cmd.Parameters.AddWithValue("@reply_by", us.reply_by);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.ExecuteNonQuery();
            con.Close();
      

            return true;
        }

        public List<Login_Failed_User> Login_Failed_User(string fromdate, string todate, int pageno = 1, int pagesize = 1, SearchParam filter = null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (filter != null)
            {
                searchcolumn = filter.SearchCriteria.FirstOrDefault().Column;
                searchvalue = filter.SearchCriteria.FirstOrDefault().Value;
            }

            List<Login_Failed_User> L = new List<Login_Failed_User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_get_failed_login_users", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            if (pagesize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", pageno);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
            }
           
            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
            cmd.Parameters.AddWithValue("@SearchValue", searchvalue);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
               
                L.Add(
                    new Login_Failed_User
                    {
                        tyflu_id= Convert.ToString(dr["tyflu_id"]),
                        tyflu_username = Convert.ToString(dr["tyflu_username"]),
                        tyflu_createdon = Convert.ToDateTime(dr["tyflu_createdon"]).ToString("yyyy/MM/dd hh:mm:ss"),
                        tyflu_reason = Convert.ToString(dr["tyflu_reason"]),
                        total = Convert.ToInt16(dr["total"])

                    }
                    );
            }

            return L;
        }

        public List<Support_Analytical_Report> First_Login_User_Info(string fromdate, string todate, int pageno = 1, int pagesize = 1, SearchParam filter = null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (filter != null)
            {
                searchcolumn = filter.SearchCriteria.FirstOrDefault().Column;
                searchvalue = filter.SearchCriteria.FirstOrDefault().Value;
            }

            List<Support_Analytical_Report> L = new List<Support_Analytical_Report>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_participant_login_report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            if (pagesize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", pageno);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
            }
            
            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
            cmd.Parameters.AddWithValue("@SearchValue", searchvalue);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            Form f = new Form();
            f = CommonDB.Get_Form_Masking_Info("9654");
           

            foreach (DataRow dr in dt.Rows)
            {

                L.Add(
                    new Support_Analytical_Report
                    {
                       userid= Convert.ToString(dr["tyat_userid"]),
                       username = Convert.ToString(dr["UserName"]),
                       agencyname= Convert.ToString(dr["AgencyName"]),
                       email = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_email"]),f, (int)Common.CommonEnum.MaskingColumn.EMAIL),
                       mobileno = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_mobileno"]), f, (int)Common.CommonEnum.MaskingColumn.MOBILENO),
                       eventdate= Convert.ToString(dr["eventdate"]),
                        total = Convert.ToInt32(dr["total"])
                    }
                    );
            }

            return L;
        }


        public List<Support_Analytical_Report> Password_Not_Updated(string fromdate, string todate, int pageno = 1, int pagesize = 1, SearchParam filter = null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (filter != null)
            {
                searchcolumn = filter.SearchCriteria.FirstOrDefault().Column;
                searchvalue = filter.SearchCriteria.FirstOrDefault().Value;
            }

            List<Support_Analytical_Report> L = new List<Support_Analytical_Report>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_participant_pwd_not_updated", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            if (pagesize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", pageno);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
            }
            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
            cmd.Parameters.AddWithValue("@SearchValue", searchvalue);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();


            Form f = new Form();
            f = CommonDB.Get_Form_Masking_Info("9654");

          
            foreach (DataRow dr in dt.Rows)
            {

                L.Add(
                    new Support_Analytical_Report
                    {
                       // userid = Convert.ToString(dr["tyat_userid"]),
                        username = Convert.ToString(dr["UserName"]),
                        agencyname = Convert.ToString(dr["AgencyName"]),
                        email = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_email"]), f, (int)Common.CommonEnum.MaskingColumn.EMAIL),
                        mobileno = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_mobileno"]), f, (int)Common.CommonEnum.MaskingColumn.MOBILENO),
                        eventdate = Convert.ToString(dr["eventdate"]),
                        total=Convert.ToInt32(dr["total"])
                    }
                    );
            }

            return L;
        }

        public List<Support_Analytical_Report> Password_Updated(string fromdate, string todate, int pageno = 1, int pagesize = 1, SearchParam filter = null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (filter != null)
            {
                searchcolumn = filter.SearchCriteria.FirstOrDefault().Column;
                searchvalue = filter.SearchCriteria.FirstOrDefault().Value;
            }

            List<Support_Analytical_Report> L = new List<Support_Analytical_Report>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_participant_pwd_updated", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            if (pagesize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", pageno);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
            }
            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn);
            cmd.Parameters.AddWithValue("@SearchValue", searchvalue);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            Form f = new Form();
            f = CommonDB.Get_Form_Masking_Info("9654");

         

            foreach (DataRow dr in dt.Rows)
            {

                L.Add(
                    new Support_Analytical_Report
                    {
                        // userid = Convert.ToString(dr["tyat_userid"]),
                        username = Convert.ToString(dr["UserName"]),
                        agencyname = Convert.ToString(dr["AgencyName"]),
                        email = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_email"]), f, (int)Common.CommonEnum.MaskingColumn.EMAIL),
                        mobileno = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_mobileno"]), f, (int)Common.CommonEnum.MaskingColumn.MOBILENO),
                        eventdate = Convert.ToString(dr["eventdate"]),
                        total = Convert.ToInt32(dr["total"])
                    }
                    );
            }

            return L;
        }


        public List<Learning_Time> Learning_Time(string trainingid, string participantid,string ttsam_id,int reporttype,string fromdate=null,string todate=null)
        {
          
            List<Learning_Time> L = new List<Learning_Time>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_participant_learning_report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            cmd.Parameters.AddWithValue("@participantid", participantid);
            cmd.Parameters.AddWithValue("@ttsam_id", ttsam_id);
            cmd.Parameters.AddWithValue("@reporttype", reporttype);
            if(fromdate != null)
            {
                cmd.Parameters.AddWithValue("@Fromdate", fromdate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Fromdate", DBNull.Value);
            }
            if (todate != null)
            {
                cmd.Parameters.AddWithValue("@todate", todate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@todate", DBNull.Value);
            }


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {

                L.Add(
                    new Learning_Time
                    {
                       // tplt_Id = Convert.ToString(dr["tplt_Id"]),
                       // tplt_ttpai_id= Convert.ToString(dr["tplt_ttpai_id"]),
                        tplt_ttsam_id = Convert.ToString(dr["tplt_ttsam_id"]),
                        //tplt_createdby = Convert.ToString(dr["tplt_createdby"]),
                        tplt_learning_time=Convert.ToDecimal(dr["Learningtime"]),
                        GlobalContentTitle = Convert.ToString(dr["GlobalContentTitle"]),
                        GlobalContentyTypeID = Convert.ToString(dr["GlobalContentyTypeID"]),
                        Participantid = Convert.ToString(dr["Participantid"]),
                        AgencyName = Convert.ToString(dr["AgencyName"]),
                        ag_email = Convert.ToString(dr["ag_email"]),
                        ag_mobileno = Convert.ToString(dr["ag_mobileno"]),

                    }
                    );
            }

            return L;
        }

        public List<Learning_Time> Learning_Report(string trainingid, string participantid, string ttsam_id, int pageno = 1, int pagesize = 1)
        {

            List<Learning_Time> L = new List<Learning_Time>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_participant_learning_report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingid", trainingid);
            cmd.Parameters.AddWithValue("@participantid", participantid);
            cmd.Parameters.AddWithValue("@ttsam_id", ttsam_id);
            if (pagesize > 0)
            {
                cmd.Parameters.AddWithValue("@PageNo", pageno);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
            }
         


            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {

                L.Add(
                    new Learning_Time
                    {
                        // tplt_Id = Convert.ToString(dr["tplt_Id"]),
                        // tplt_ttpai_id= Convert.ToString(dr["tplt_ttpai_id"]),
                        tplt_ttsam_id = Convert.ToString(dr["tplt_ttsam_id"]),
                        //tplt_createdby = Convert.ToString(dr["tplt_createdby"]),
                        tplt_learning_time = Convert.ToInt16(dr["Learningtime"]),
                        GlobalContentTitle = Convert.ToString(dr["GlobalContentTitle"]),
                        GlobalContentyTypeID = Convert.ToString(dr["GlobalContentyTypeID"]),
                        Participantid = Convert.ToString(dr["Participantid"]),
                        AgencyName = Convert.ToString(dr["AgencyName"]),
                        ag_email = Convert.ToString(dr["ag_email"]),
                        ag_mobileno = Convert.ToString(dr["ag_mobileno"]),
                        totalrecords=Convert.ToInt16(dr["totalrecords"])

                    }
                    );
            }

            return L;
        }

        public List<Learning_Report_Data> Learning_Report_Data(string trainingid = null, string participantid=null, string ttsam_id=null,string branchid=null,int reporttype=1, int pageno = 1, int pagesize = 1,string SearchColumn=null,string searchvalue=null,string sortcolumn=null,string sortdirection=null,string fromdate=null,string todate=null)
        {
            List<contentType> l = new List<contentType>();

            if (reporttype == 1)
            {
                ContentDB cdb = new ContentDB(_configuration);
                l = cdb.Get_Content_Type_All();
            }
            List<Learning_Report_Data> L = new List<Learning_Report_Data>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("yuser.proc_yuser_participant_learning_report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if(trainingid != null)
            {
                cmd.Parameters.AddWithValue("@trainingid", trainingid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trainingid", DBNull.Value);
            }
            if(participantid != null)
            {
                cmd.Parameters.AddWithValue("@participantid", participantid);
            }
            else
            {
                cmd.Parameters.AddWithValue("@participantid", DBNull.Value);
            }
           if(ttsam_id != null)
            {
                cmd.Parameters.AddWithValue("@ttsam_id", ttsam_id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttsam_id", DBNull.Value);
            }
           
           if(pageno != null)
            {
                cmd.Parameters.AddWithValue("@PageNo", pageno);
            }
            if (pagesize != null)
            {
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
            }

            if (searchvalue != null)
            {
                cmd.Parameters.AddWithValue("@SearchColumn", SearchColumn);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SearchColumn", DBNull.Value);
            }
           if(searchvalue != null)
            {
                cmd.Parameters.AddWithValue("@SearchValue", searchvalue);
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
           if(reporttype != null)
            {
                cmd.Parameters.AddWithValue("@reporttype", reporttype);
            }
            else
            {
                cmd.Parameters.AddWithValue("@reporttype", DBNull.Value);
            }
            if (sortcolumn != null)
            {
                cmd.Parameters.AddWithValue("@SortColumn", sortcolumn);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SortColumn", DBNull.Value);
            }
            if (sortdirection != null)
            {
                cmd.Parameters.AddWithValue("@SortDirection", sortdirection);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SortDirection", DBNull.Value);
            }
            if (fromdate != null)
            {
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@fromdate", DBNull.Value);
            }
           if(todate != null)
            {
                cmd.Parameters.AddWithValue("@todate", todate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@todate", DBNull.Value);
            }
           
         



            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            Form f = new Form();
            f = CommonDB.Get_Form_Masking_Info("9657");




            foreach (DataRow dr in dt.Rows)
            {
                string content_type_name = "";
                if(Convert.ToString(dr["GlobalContentyTypeID"]) != "")
                {
                    if(l.Where(o=>o.GlobalContentTypeID.ToString().ToUpper() == Convert.ToString(dr["GlobalContentyTypeID"]).ToString().ToUpper()).Count()>0)
                    {
                        content_type_name = l.Where(o => o.GlobalContentTypeID.ToString().ToUpper() == Convert.ToString(dr["GlobalContentyTypeID"]).ToString().ToUpper()).FirstOrDefault().GlobalContentDescription;
                    }

                }

                L.Add(
                    new Learning_Report_Data
                    {
                        // tplt_Id = Convert.ToString(dr["tplt_Id"]),
                        // tplt_ttpai_id= Convert.ToString(dr["tplt_ttpai_id"]),
                        AgencyName = Convert.ToString(dr["AgencyName"]),
                        tplt_ttsam_id = Convert.ToString(dr["tplt_ttsam_id"]),
                        GlobalContentTitle = Convert.ToString(dr["GlobalContentTitle"]),
                        GlobalContentyTypeID = Convert.ToString(dr["GlobalContentyTypeID"]),
                        Participantid = Convert.ToString(dr["Participantid"]),
                        ag_email = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_email"]), f, (int)Common.CommonEnum.MaskingColumn.EMAIL),
                        ag_mobileno = CommonDB.Get_MaskData(1, Convert.ToString(dr["ag_mobileno"]), f, (int)Common.CommonEnum.MaskingColumn.MOBILENO),
                        
                        learningtime = Convert.ToDecimal(dr["learningtime"]),
                        totalrecord = Convert.ToInt16(dr["totalrecord"]),
                        trainingid = Convert.ToString(dr["trainingid"]),
                        trainingname = Convert.ToString(dr["trainingname"]),
                        GlobalContentyType_Name= content_type_name

                    }
                    );
            }

            return L;
        }



    }
}
