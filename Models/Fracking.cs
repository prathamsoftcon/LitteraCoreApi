namespace LitteraCore.Models
{
    public class Fracking
    {

    }
    public class FracEmp
    {
        public string tcfem_id { get; set; }
        public string tcfem_f_name { get; set; }
        public string? tcfem_m_name { get; set; }
        public string? tcfem_l_name { get; set; }
        public int? tcfem_gender { get; set; }
        public int? tcfem_cast_category { get; set; }
        public int? tcfem_class { get; set; }
        public string tcfem_mobileno { get; set; }
        public string tcfem_email { get; set; }
        public string tcfem_post { get; set; }
        public string tcfem_branchid { get; set; }
        public DateTime tcfem_createdon { get; set; }
    }

    public class FracEmpActivity
    {
        public int? tcfea_id { get; set; }
        public string tcfea_tcfem_id { get; set; }
        public Empactivity[] activities { get; set; }
        public string? tcfea_branchid { get; set; }
        public DateTime tcfea_createdon { get; set; }
       
    }

    public class FracEmpResources
    {
        public EmpActivityResource[] resources { get; set; }
        public string tcfekr_branchid { get; set; }
        public DateTime tcfekr_createdon { get; set; }
    }
    public class EmpActivityResource
    {
        public string resourceid {  get; set; }
        public int activityid { get; set; }
        public string resources { get; set; }

    }
    public class Empactivity
    {
        public int? activityid { get; set; }
        public string activity { get; set; }
    }
    public class EMP_FRACK_REPORT
    {
        public string tcfem_post { get; set; }
        public string activity_names { get; set; }
        public string activity_resources { get; set; }
    }
}
