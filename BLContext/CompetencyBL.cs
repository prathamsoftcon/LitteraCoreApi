using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using LitteraCore.Common;
using Newtonsoft.Json;

namespace LitteraCore.BLContext
{
    
    public class CompetencyBL
    {
        private readonly IConfiguration _configuration;
        public CompetencyBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Save_Department_Function(DeptFunction f,string createdby)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            string code = cdb.Save_Department_Function(f, createdby);
            return code;
        }
        public bool Delete_Dept_Function(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            bool isSaved = cdb.Delete_Department_Function(id);
            return isSaved;
        }

        public string Save_Job_Position(JobPosition f, string createdby)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            string code = cdb.Save_Job_Position(f, createdby);
            return code;
        }
        public bool Delete_Job_Position(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            bool isSaved = cdb.Delete_Job_Position(id);
            return isSaved;
        }


        public string Save_Activity(Activity f, string createdby)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            string code = cdb.Save_Activity(f, createdby);
            return code;
        }
        public bool Delete_Activity(string id)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            bool isSaved = cdb.Delete_Activity(id);
            return isSaved;
        }

        public PagedList<position_levels> Get_Position_Lelvels(PaginationParam param)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            PagedList<position_levels> leveldata = cdb.Get_Position_Lelvels(param);
            return leveldata;
        }


        public string Save_Job_Role(JobRole f, string createdby)
        {
            CompetencyDB cdb = new CompetencyDB(_configuration);
            string code = cdb.Save_Job_Role(f, createdby);
            return code;
        }
        public bool Save_Emp_Details(FracEmp E)
        {
            bool issaved = false;
            CompetencyDB cdb = new CompetencyDB(_configuration);
            issaved=cdb.Save_Emp_Details(E);
            return issaved;
        }
        public bool Save_Emp_Activities(FracEmpActivity Act)
        {
            bool issaved = false;
            CompetencyDB cdb = new CompetencyDB(_configuration);
            issaved = cdb.Save_Emp_Activities(Act);
            return issaved;
        }
        public bool Save_Emp_Activities_Resources(FracEmpResources resource)
        {
            bool issaved = false;
            CompetencyDB cdb = new CompetencyDB(_configuration);
            issaved = cdb.Save_Emp_Activities_Resources(resource);
            return issaved;
        }
        public List<Empactivity> Get_Emp_activity(string employeeid, string branchid)
        {
            List<Empactivity> act = new List<Empactivity>();
            CompetencyDB cdb = new CompetencyDB(_configuration);
            act = cdb.Get_Emp_Activities(employeeid, branchid);
            return act;
        }
        public bool Check_Frack_Emp_Exist(string mobileno)
        {
            bool isexist = false;
            CompetencyDB cdb = new CompetencyDB(_configuration);
            isexist = cdb.check_frack_Emp(mobileno);
            return isexist;
        }

        public List<CompentencyLevel> Get_Test_Result_Data(string userid, string usertype, int Groupby, string categoryid, string testid)
        {
            List<Competency> C = new List<Competency>();
            CompetencyDB CBL = new CompetencyDB(_configuration);
            C = CBL.Get_Test_Result_Data(userid, usertype);
            if (categoryid != null)
            {
                C = C.Where(o => o.TrainingCategoryID.ToString().ToUpper() == categoryid.ToString().ToUpper()).ToList();
            }
            if (testid != null)
            {
                C = C.Where(o => o.TestID.ToString().ToUpper() == testid.ToString().ToUpper()).ToList();
            }


            List<CompentencyLevel> cl = new List<CompentencyLevel>();
            if (Groupby == 1)  //Categorywise
            {
                //Code to get distinct category

                List<string> DistCat = C.Select(o => o.TrainingCategoryID).Distinct().ToList();
                foreach (string s in DistCat)
                {
                    List<Competency> FC = new List<Competency>();
                    FC = C.Where(o => o.TrainingCategoryID.ToString().ToUpper() == s.ToUpper()).ToList();
                    // var percentage = FC.Sum(o => o.CategoryMarksPercentage);
                    cl.Add(new CompentencyLevel { categoryid = FC.FirstOrDefault().TrainingCategoryID, name = FC.FirstOrDefault().TrainingCategoryName, percentage = FC.FirstOrDefault().CategoryMarksPercentage, testid = FC.FirstOrDefault().TestID });

                }


            }
            else if (Groupby == 2) //Test wise
            {
                List<string> DistCat = C.Select(o => o.TestID).Distinct().ToList();
                foreach (string s in DistCat)
                {
                    List<Competency> FC = new List<Competency>();
                    FC = C.Where(o => o.TestID.ToString().ToUpper() == s.ToUpper()).ToList();
                    //var percentage = FC.Sum(o => o.QuestionMarksPercentage);
                    cl.Add(new CompentencyLevel { categoryid = FC.FirstOrDefault().TrainingCategoryID, name = FC.FirstOrDefault().TestDescription, percentage = FC.FirstOrDefault().testMarksPercentage, testid = FC.FirstOrDefault().TestID });

                }
            }
            else    // Tag wise
            {
                List<string> DistCat = C.Select(o => o.Skilltag.ToString().ToUpper()).Distinct().ToList();

                List<Competency> DistCatdata = C.Select(o => new Competency { Skilltag = o.Skilltag, skillMarksPercentage = o.skillMarksPercentage }).Distinct().ToList();

                foreach (string s in DistCat)
                {
                    List<Competency> FC = new List<Competency>();
                    FC = DistCatdata.Where(o => o.Skilltag.ToString().ToUpper() == s.ToUpper()).ToList();
                    var percentage = Decimal.Round((Convert.ToDecimal(FC.Sum(o => o.skillMarksPercentage)) / FC.Count), 2);
                    cl.Add(new CompentencyLevel { categoryid = FC.FirstOrDefault().TrainingCategoryID, name = FC.FirstOrDefault().Skilltag, percentage = percentage, testid = FC.FirstOrDefault().TestID });

                }

            }


            return cl;
        }

        public List<Competency> Get_Test_Report_Data(string userid, string usertype)
        {
            List<Competency> C = new List<Competency>();
            CompetencyDB CBL = new CompetencyDB(_configuration);
            C = CBL.Get_Test_Result_Data(userid, usertype);

            return C;
        }

   


    }
}
