using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using LitteraCore.BLContext;
using Microsoft.Extensions.ObjectPool;
namespace LitteraCore.DBContext
{
    public class CompetencyDB
    {
        private readonly IConfiguration _configuration;
        public CompetencyDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Save_Department_Function(DeptFunction f,string createdby)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlTransaction st = con.BeginTransaction();
            DMSBL dbl = new DMSBL(_configuration);
            String docno = "";
            docno = dbl.Get_dms_doc_no(f.ttcf_id, (int)Common.CommonEnum.DMS_TAT_TYPE_ID.DEPARTMENT_FUNCTIONS, f.DMS.branchid, "FN", "YEAR");
            try
            {
              

                SqlCommand cmd = new SqlCommand("trainingplan.proc_insupd_tbl_tp_competency_functions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = st;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@ttcf_id", f.ttcf_id);
                cmd.Parameters.AddWithValue("@ttcf_name", f.ttcf_name);
                cmd.Parameters.AddWithValue("@ttcf_description", f.ttcf_description);
                if (f.ttcf_hod_designation_id != null)
                {
                    cmd.Parameters.AddWithValue("@ttcf_hod_designation_id", f.ttcf_hod_designation_id);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttcf_hod_designation_id", DBNull.Value);
                }
                if (f.ttcf_objective != null)
                {
                    cmd.Parameters.AddWithValue("@ttcf_objective", f.ttcf_objective);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttcf_objective", DBNull.Value);
                }
                if (f.ttcf_uploads != null)
                {
                    cmd.Parameters.AddWithValue("@ttcf_uploads", JsonConvert.SerializeObject(f.ttcf_uploads));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttcf_uploads", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@ttcf_branchid", f.ttcf_branchid);
                cmd.Parameters.AddWithValue("@ttcf_code", docno);
                cmd.Parameters.AddWithValue("@ttcf_createdby", createdby);
                cmd.ExecuteNonQuery();
                
               

                DMS d = new DMS
                {
                    docno = docno,
                    doc_id =f.DMS.doc_id,
                    createdon =System.DateTime.Now,
                    createdby = f.DMS.createdby,
                    branchid =f.DMS.branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = f.DMS.CreatedBy_empid,
                    fwd_empid = f.DMS.CreatedBy_empid,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.DEPARTMENT_FUNCTIONS),
                    doc_status = f.DMS.doc_status,
                };
                dbl.Save_DMS_DATA(d, con, st);




                st.Commit();

            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return "";
            }
            finally
            {
                con.Close();
            }


           
            return docno;
        }

        public PagedList<DeptFunction> Get_Department_Function(PaginationParam param,string id=null,string searchtext=null,int exactMatch=0)
        {

            List<DeptFunction> f = new List<DeptFunction>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_tbl_tp_competency_functions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if(id != null)
            {
                cmd.Parameters.AddWithValue("@ttcf_id", id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttcf_id", DBNull.Value);
            }
        
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            dt.DefaultView.RowFilter = "tdds_status <> '-1'";
            dt = dt.DefaultView.ToTable();


            if (searchtext != null)
            {
                if (exactMatch == 1)
                {
                    dt.DefaultView.RowFilter = "ttcf_name = '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }
                else
                {
                    dt.DefaultView.RowFilter = "ttcf_name like '%" + searchtext + "%' or ttcf_description like '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }
               
            }  
            foreach (DataRow dr in dt.Rows)
            {
                string[] uploads = null;
                if(Convert.ToString(dr["ttcf_uploads"]) != null)
                {
                    uploads = JsonConvert.DeserializeObject<string[]>(Convert.ToString(dr["ttcf_uploads"]));
                }
                f.Add(
                    new DeptFunction
                    {
                        ttcf_id = Convert.ToString(dr["ttcf_id"]),
                        ttcf_name = Convert.ToString(dr["ttcf_name"]),
                        ttcf_description = Convert.ToString(dr["ttcf_description"]),
                        ttcf_hod_designation_id = Convert.ToString(dr["ttcf_hod_designation_id"]),
                        ttcf_objective= Convert.ToString(dr["ttcf_objective"]),
                        ttcf_uploads= uploads,
                        ttcf_code= Convert.ToString(dr["ttcf_code"]),
                        ttcf_branchid=  Convert.ToString(dr["ttcf_branchid"]),
                    });
            }

            if(param != null)
            {
                if (param.PageSize > 0)
                {
                    return PagedList<DeptFunction>.ToPagedList(f.ToList(),
           param.PageNumber,
           param.PageSize);
                }
                else
                {
                    return PagedList<DeptFunction>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
                }
        
            }
            else
            {
                return PagedList<DeptFunction>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
            }

           

        }

        public bool Delete_Department_Function(string id)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_delete_tbl_tp_competency_functions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttcf_id", id);
          
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public string Save_Job_Position(JobPosition f, string createdby)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlTransaction st = con.BeginTransaction();
            DMSBL dbl = new DMSBL(_configuration);
            String docno = "";
            docno = dbl.Get_dms_doc_no(f.ttcjp_id, (int)Common.CommonEnum.DMS_TAT_TYPE_ID.JOB_POSITION, f.DMS.branchid, "JP", "YEAR");
            try
            {


                SqlCommand cmd = new SqlCommand("trainingplan.proc_insupd_tbl_tp_competency_job_positions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = st;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@ttcjp_id", f.ttcjp_id);
                cmd.Parameters.AddWithValue("@ttcjp_code", docno);
                cmd.Parameters.AddWithValue("@ttcjp_name", f.ttcjp_name);
                if (f.ttcjp_description != null)
                {
                    cmd.Parameters.AddWithValue("@ttcjp_description", f.ttcjp_description);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttcjp_description",DBNull.Value);
                }
                if (f.ttcjp_ttcjpl_id != null)
                {
                    cmd.Parameters.AddWithValue("@ttcjp_ttcjpl_id", f.ttcjp_ttcjpl_id);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttcjp_ttcjpl_id", DBNull.Value);
                }
               
                cmd.Parameters.AddWithValue("@ttcjp_branchid", f.ttcjp_branchid);
                cmd.Parameters.AddWithValue("@ttcjp_createdby", f.ttcjp_createdby);
              
                cmd.ExecuteNonQuery();



                DMS d = new DMS
                {
                    docno = docno,
                    doc_id = f.DMS.doc_id,
                    createdon = System.DateTime.Now,
                    createdby = f.DMS.createdby,
                    branchid = f.DMS.branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = f.DMS.CreatedBy_empid,
                    fwd_empid = f.DMS.CreatedBy_empid,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.JOB_POSITION),
                    doc_status = f.DMS.doc_status,
                };
                dbl.Save_DMS_DATA(d, con, st);




                st.Commit();

            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return "";
            }
            finally
            {
                con.Close();
            }



            return docno;
        }

        public PagedList<JobPosition> Get_JOB_POSITIONS(PaginationParam param, string id = null, string searchtext = null, int exactMatch = 0)
        {

            List<JobPosition> f = new List<JobPosition>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_tbl_tp_competency_job_positions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (id != null)
            {
                cmd.Parameters.AddWithValue("@ttcjp_id", id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttcjp_id", DBNull.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "tdds_status <> '-1'";
            dt = dt.DefaultView.ToTable();
            if (searchtext != null)
            {
                if (exactMatch == 1)
                {
                    dt.DefaultView.RowFilter = "ttcjp_name = '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }
                else
                {
                    dt.DefaultView.RowFilter = "ttcjp_name like '%" + searchtext + "%' or ttcjp_description like '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }

            }
            foreach (DataRow dr in dt.Rows)
            {
                
                f.Add(
                    new JobPosition
                    {
                        ttcjp_id = Convert.ToString(dr["ttcjp_id"]),
                        ttcjp_code = Convert.ToString(dr["ttcjp_code"]),
                        ttcjp_name = Convert.ToString(dr["ttcjp_name"]),
                        ttcjp_description = Convert.ToString(dr["ttcjp_description"]),
                        ttcjp_ttcjpl_id = Convert.ToString(dr["ttcjp_ttcjpl_id"]),
                        ttcjp_branchid = Convert.ToString(dr["ttcjp_branchid"]),
                        ttcjp_createdby = Convert.ToString(dr["ttcjp_createdby"])
                    });
            }

            if (param != null)
            {
                if (param.PageSize > 0)
                {
                    return PagedList<JobPosition>.ToPagedList(f.ToList(),
           param.PageNumber,
           param.PageSize);
                }
                else
                {
                    return PagedList<JobPosition>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
                }

            }
            else
            {
                return PagedList<JobPosition>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
            }



        }

        public bool Delete_Job_Position(string id)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_del_tbl_tp_competency_job_positions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttcjp_id", id);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public string Save_Activity(Activity f, string createdby)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlTransaction st = con.BeginTransaction();
            DMSBL dbl = new DMSBL(_configuration);
            String docno = "";
            docno = dbl.Get_dms_doc_no(f.ttca_id, (int)Common.CommonEnum.DMS_TAT_TYPE_ID.Activity, f.DMS.branchid, "ACT", "YEAR");
            try
            {


                SqlCommand cmd = new SqlCommand("trainingplan.proc_insupd_tbl_tp_competency_activity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = st;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@ttca_id", f.ttca_id);
                cmd.Parameters.AddWithValue("@ttca_code", docno);
                cmd.Parameters.AddWithValue("@ttca_name", f.ttca_name);
                if (f.ttca_description != null)
                {
                    cmd.Parameters.AddWithValue("@ttca_description", f.ttca_description);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttca_description", DBNull.Value);
                }
               

                cmd.Parameters.AddWithValue("@ttca_branchid", f.ttca_branchid);
                cmd.Parameters.AddWithValue("@ttca_createdby", f.ttca_createdby);

                cmd.ExecuteNonQuery();



                DMS d = new DMS
                {
                    docno = docno,
                    doc_id = f.DMS.doc_id,
                    createdon = System.DateTime.Now,
                    createdby = f.DMS.createdby,
                    branchid = f.DMS.branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = f.DMS.CreatedBy_empid,
                    fwd_empid = f.DMS.CreatedBy_empid,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.Activity),
                    doc_status = f.DMS.doc_status,
                };
                dbl.Save_DMS_DATA(d, con, st);




                st.Commit();

            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return "";
            }
            finally
            {
                con.Close();
            }



            return docno;
        }

        public PagedList<Activity> Get_Activity(PaginationParam param, string id = null, string searchtext = null, int exactMatch = 0)
        {

            List<Activity> f = new List<Activity>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_tbl_tp_competency_activity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (id != null)
            {
                cmd.Parameters.AddWithValue("@ttca_id", id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttca_id", DBNull.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "tdds_status <> '-1'";
            dt = dt.DefaultView.ToTable();
            if (searchtext != null)
            {
                if (exactMatch == 1)
                {
                    dt.DefaultView.RowFilter = "ttca_name = '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }
                else
                {
                    dt.DefaultView.RowFilter = "ttca_name like '%" + searchtext + "%' or ttca_description like '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }

            }
            foreach (DataRow dr in dt.Rows)
            {

                f.Add(
                    new Activity
                    {
                        ttca_id = Convert.ToString(dr["ttca_id"]),
                        ttca_code = Convert.ToString(dr["ttca_code"]),
                        ttca_name = Convert.ToString(dr["ttca_name"]),
                        ttca_description = Convert.ToString(dr["ttca_description"]),
                        ttca_branchid = Convert.ToString(dr["ttca_branchid"]),
                        ttca_createdby = Convert.ToString(dr["ttca_createdby"])
                     
                    });
            }

            if (param != null)
            {
                if (param.PageSize > 0)
                {
                    return PagedList<Activity>.ToPagedList(f.ToList(),
           param.PageNumber,
           param.PageSize);
                }
                else
                {
                    return PagedList<Activity>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
                }

            }
            else
            {
                return PagedList<Activity>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
            }



        }

        public bool Delete_Activity(string id)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_delete_tbl_tp_competency_activity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@ttca_id", id);

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public PagedList<position_levels> Get_Position_Lelvels(PaginationParam param)
        {

            List<position_levels> f = new List<position_levels>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_tbl_tp_competency_job_position_level", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
           
            foreach (DataRow dr in dt.Rows)
            {

                f.Add(
                    new position_levels
                    {
                        ttcjpl_id = Convert.ToString(dr["ttcjpl_id"]),
                        ttcjpl_name = Convert.ToString(dr["ttcjpl_name"])
                      
                    });
            }

            if (param != null)
            {
                if (param.PageSize > 0)
                {
                    return PagedList<position_levels>.ToPagedList(f.ToList(),
           param.PageNumber,
           param.PageSize);
                }
                else
                {
                    return PagedList<position_levels>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
                }

            }
            else
            {
                return PagedList<position_levels>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
            }



        }

        public string Save_Job_Role(JobRole f, string createdby)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlTransaction st = con.BeginTransaction();
            DMSBL dbl = new DMSBL(_configuration);
            String docno = "";
            docno = dbl.Get_dms_doc_no(f.ttcjr_id, (int)Common.CommonEnum.DMS_TAT_TYPE_ID.Job_Role, f.DMS.branchid, "JR", "YEAR");
            try
            {


                SqlCommand cmd = new SqlCommand("trainingplan.proc_insupd_tbl_tp_competency_job_role", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = st;
                cmd.Connection = con;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@ttcjr_id", f.ttcjr_id);
                cmd.Parameters.AddWithValue("@ttcjr_code", docno);
                cmd.Parameters.AddWithValue("@ttcjr_name", f.ttcjr_name);
                
                cmd.Parameters.AddWithValue("@ttcjr_branchid", f.ttcjr_branchid);
                cmd.Parameters.AddWithValue("@ttcjr_createdby", f.ttcjr_createdby);
                if (f.ttcjr_description != null)
                {
                    cmd.Parameters.AddWithValue("@ttcjr_description", f.ttcjr_description);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ttcjr_description", DBNull.Value);
                }
               cmd.ExecuteNonQuery();



                DMS d = new DMS
                {
                    docno = docno,
                    doc_id = f.DMS.doc_id,
                    createdon = System.DateTime.Now,
                    createdby = f.DMS.createdby,
                    branchid = f.DMS.branchid,
                    docdate = DateTime.Now,
                    actiondate = DateTime.Now,
                    CreatedBy_empid = f.DMS.CreatedBy_empid,
                    fwd_empid = f.DMS.CreatedBy_empid,
                    tat_type_id = Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.Job_Role),
                    doc_status = f.DMS.doc_status,
                };
                dbl.Save_DMS_DATA(d, con, st);




                st.Commit();

            }
            catch (Exception ex)
            {
                st.Rollback();
                throw new Exception(ex.Message);
                return "";
            }
            finally
            {
                con.Close();
            }



            return docno;
        }


        public PagedList<JobRole> Get_JOB_ROLE(PaginationParam param, string id = null, string searchtext = null, int exactMatch = 0)
        {

            List<JobRole> f = new List<JobRole>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_tbl_tp_competency_job_role", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (id != null)
            {
                cmd.Parameters.AddWithValue("@ttcjr_id", id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ttcjr_id", DBNull.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dt.DefaultView.RowFilter = "tdds_status <> '-1'";
            dt = dt.DefaultView.ToTable();
            if (searchtext != null)
            {
                if (exactMatch == 1)
                {
                    dt.DefaultView.RowFilter = "ttcjr_name = '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }
                else
                {
                    dt.DefaultView.RowFilter = "ttcjr_name like '%" + searchtext + "%' or ttcjr_description like '%" + searchtext + "%'";
                    dt = dt.DefaultView.ToTable();
                }

            }
            foreach (DataRow dr in dt.Rows)
            {

                f.Add(
                    new JobRole
                    {
                        ttcjr_id = Convert.ToString(dr["ttcjr_id"]),
                        ttcjr_code = Convert.ToString(dr["ttcjr_code"]),
                        ttcjr_name = Convert.ToString(dr["ttcjr_name"]),
                        ttcjr_description = Convert.ToString(dr["ttcjr_description"]),
                        ttcjr_branchid = Convert.ToString(dr["ttcjr_branchid"]),
                        ttcjr_createdby = Convert.ToString(dr["ttcjr_createdby"])

                    });
            }

            if (param != null)
            {
                if (param.PageSize > 0)
                {
                    return PagedList<JobRole>.ToPagedList(f.ToList(),
           param.PageNumber,
           param.PageSize);
                }
                else
                {
                    return PagedList<JobRole>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
                }

            }
            else
            {
                return PagedList<JobRole>.ToPagedList(f.ToList(),
                   1,
                   f.Count());
            }



        }


        //public PagedList<DeptFunction> GetPagedList(PaginationParam param,List<DeptFunction> f)
        //{
        //    if (param != null)
        //    {
        //        if (param.PageSize > 0)
        //        {
        //            return PagedList<DeptFunction>.ToPagedList(f.ToList(),
        //   param.PageNumber,
        //   param.PageSize);
        //        }
        //        else
        //        {
        //            return PagedList<DeptFunction>.ToPagedList(f.ToList(),
        //           1,
        //           f.Count());
        //        }

        //    }
        //    else
        //    {
        //        return PagedList<DeptFunction>.ToPagedList(f.ToList(),
        //           1,
        //           f.Count());
        //    }
        //}





        public bool Save_Emp_Details(FracEmp E)
        {

           
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();

            SqlCommand cmd = new SqlCommand("competency.proc_ins_upd_tbl_compt_frack_employee_master", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tcfem_id", E.tcfem_id);
            cmd.Parameters.AddWithValue("@tcfem_f_name", E.tcfem_f_name);
            if(E.tcfem_m_name != null)
            {
                cmd.Parameters.AddWithValue("@tcfem_m_name", E.tcfem_m_name);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tcfem_m_name", DBNull.Value);
            }
            if (E.tcfem_l_name != null)
            {
                cmd.Parameters.AddWithValue("@tcfem_l_name", E.tcfem_l_name);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tcfem_l_name", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@tcfem_gender", E.tcfem_gender);
            if(E.tcfem_cast_category != null){
              cmd.Parameters.AddWithValue("@tcfem_cast_category", E.tcfem_cast_category);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tcfem_cast_category", DBNull.Value);
            }

       
            cmd.Parameters.AddWithValue("@tcfem_mobileno", E.tcfem_mobileno);
            cmd.Parameters.AddWithValue("@tcfem_email", E.tcfem_email);
            cmd.Parameters.AddWithValue("@tcfem_post", E.tcfem_post);
            cmd.Parameters.AddWithValue("@tcfem_branchid", E.tcfem_branchid);
            if (E.tcfem_class != null)
            {
                cmd.Parameters.AddWithValue("@tcfem_class", E.tcfem_class);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tcfem_class", DBNull.Value);
            }
            cmd.ExecuteNonQuery();

            return true;
        }
        public bool Save_Emp_Activities(FracEmpActivity A)
        {


            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();

            SqlCommand cmd = new SqlCommand("competency.proc_ins_upd_tbl_compt_frack_employee_activities", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tcfea_tcfem_id", A.tcfea_tcfem_id);
            string p1 = JsonConvert.SerializeObject(A.activities);
            cmd.Parameters.AddWithValue("@activityjson", p1);
            cmd.Parameters.AddWithValue("@tcfea_branchid", A.tcfea_branchid);
           
            cmd.ExecuteNonQuery();

            return true;
        }

        public bool Save_Emp_Activities_Resources(FracEmpResources R)
        {


            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();

            SqlCommand cmd = new SqlCommand("competency.proc_ins_upd_tbl_compt_frack_employee_knowledge_resources", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            string p1 = JsonConvert.SerializeObject(R.resources);
            cmd.Parameters.AddWithValue("@resourcejson", p1);
         
            cmd.Parameters.AddWithValue("@tcfekr_branchid", R.tcfekr_branchid);
            cmd.ExecuteNonQuery();

            return true;
        }

        public List<Empactivity> Get_Emp_Activities(string employeeid,string branchid)
        {

            List<Empactivity> f = new List<Empactivity>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("competency.proc_get_tbl_compt_frack_employee_activities", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeid", employeeid);
            cmd.Parameters.AddWithValue("@branchid", branchid);
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
           

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
       
         
            foreach (DataRow dr in dt.Rows)
            {

                f.Add(
                    new Empactivity
                    {
                        activityid = (int?)Convert.ToUInt32(dr["tcfea_id"]),
                        activity=Convert.ToString(dr["tcfea_activity_name"])


                    });
            }


            return f;

        }

        public bool check_frack_Emp(string mobileno)
        {

            List<Empactivity> f = new List<Empactivity>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("competency.proc_get_employee_info", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mobileno", mobileno);
          
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


            return false;

        }

        public String[] Get_Post_Data(string postname)
        {

            List<DeptFunction> f = new List<DeptFunction>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("competency.proc_get_frac_post", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            if (postname != null)
            {
                cmd.Parameters.AddWithValue("@postname", postname);
            }
            else
            {
                cmd.Parameters.AddWithValue("@postname", DBNull.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();


           List<string> posts=new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                posts.Add(Convert.ToString(dr["tcfem_post"]));
               
               
            }


            return posts.ToArray();
        }

        public List<EMP_FRACK_REPORT> Get_Fracking_Report()
        {

            List<EMP_FRACK_REPORT> f = new List<EMP_FRACK_REPORT>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("competency.get_fracing_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
           

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();


       

            foreach (DataRow dr in dt.Rows)
            {
                f.Add(new EMP_FRACK_REPORT
                {
                    tcfem_post = Convert.ToString(dr["tcfem_post"]),
                     activity_names= Convert.ToString(dr["activity_names"]),
                    activity_resources = Convert.ToString(dr["activity_resources"]),
                     
                });


            }


            return f;
        }

        public List<Competency> Get_Test_Result_Data(string userid, string usertype)
        {

            List<Competency> AL = new List<Competency>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
             if (con.State == ConnectionState.Open) { con.Close();}con.Open();
            SqlCommand cmd = new SqlCommand("Eval.proc_Get_selfassessmenttest_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);

            cmd.Connection = con;
            cmd.CommandTimeout = 5000;




            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            foreach (DataRow row in dt.Rows)
            {
                Competency vw = new Competency();
                vw.TestID = Convert.ToString(row["TestID"]);
                vw.Totalmarks = Convert.ToDecimal(row["Totalmarks"]);
                vw.TrainingCategoryID = Convert.ToString(row["TrainingCategoryID"]);
                vw.PartcipantID = Convert.ToString(row["PartcipantID"]);
                vw.QuestionID = Convert.ToString(row["QuestionID"]);
                vw.IsCorrect = Convert.ToInt32(row["IsCorrect"]);
                vw.tesQmarksobtained = Convert.ToDecimal(row["tesQmarksobtained"]);
                vw.QuestionMarksPercentage = Convert.ToDecimal(row["QuestionMarksPercentage"]);
                vw.TestDescription = Convert.ToString(row["TestDescription"]);
                vw.TrainingCategoryName = Convert.ToString(row["TrainingCategoryName"]);
                vw.Skilltag = Convert.ToString(row["Skilltag"]);
                vw.CategoryMarksPercentage = Convert.ToDecimal(row["CategoryMarksPercentage"]);
                vw.testMarksPercentage = Convert.ToDecimal(row["testMarksPercentage"]);
                vw.skillMarksPercentage = Convert.ToDecimal(row["skillMarksPercentage"]);

                vw.Youranswer = Convert.ToString(row["Youranswer"]);
                vw.correctAnswerDescription = Convert.ToString(row["correctAnswerDescription"]);

                vw.Question = Convert.ToString(row["Question"]);
                vw.mark_per_question = Convert.ToString(row["mark_per_question"]);

                AL.Add(vw);
            }





            return AL;
        }



    }
}
