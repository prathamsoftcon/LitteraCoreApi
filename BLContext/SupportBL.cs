using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace LitteraCore.BLContext
{
    public class SupportBL
    {
        private readonly IConfiguration _configuration;
        public SupportBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Insert_Support(Support s)
        {

            string issaved = "";
            SupportDB ABD = new SupportDB(_configuration);
            issaved = ABD.Insert_Support(s);

            return issaved;
        }
        public List<Support> GetSupportQuery()
        {
            List <Support> sl =new List<Support>();
        
            SupportDB ABD = new SupportDB(_configuration);
            sl = ABD.GetSupportQuery();

            return sl;
        }
        public bool Update_Status(Update_Support us)
        {
            SupportDB ABD = new SupportDB(_configuration);
            bool issaved = ABD.Update_Status(us);

            return issaved;
        }

        public List<Login_Failed_User> Login_Failed_User(string fromdate, string todate, int pageno = 1, int pagesize = 1, SearchParam filter = null)
        {
            List<Login_Failed_User> sl = new List<Login_Failed_User>();

            SupportDB ABD = new SupportDB(_configuration);
            sl = ABD.Login_Failed_User(fromdate, todate, pageno, pagesize, filter);

            return sl;
        }
        public List<Support_Analytical_Report> Login_Analytics(string fromdate, string todate,int reportype=1, int pageno = 1, int pagesize = 1, SearchParam filter = null)
        {
            List<Support_Analytical_Report> sl = new List<Support_Analytical_Report>();

            SupportDB ABD = new SupportDB(_configuration);
            if (reportype == 1) // For First Login
            {
                sl = ABD.First_Login_User_Info(fromdate, todate, pageno, pagesize, filter);
            }
            else if(reportype == 2) // For password not changed 
            {
                sl = ABD.Password_Not_Updated(fromdate, todate, pageno, pagesize, filter); 
            }
            else if(reportype== 3)
            {
                sl = ABD.Password_Updated(fromdate, todate, pageno, pagesize, filter); 
            }
           

            return sl;
        }

        public Decimal Learning_Time(string trainingid, string participantid, string ttsam_id,int unit)
        {
            decimal learningtime = 0;
            List<Learning_Time> sl = new List<Learning_Time>();

            SupportDB ABD = new SupportDB(_configuration);
            sl = ABD.Learning_Time(trainingid, participantid, ttsam_id);
            if (unit == 1) //min
            {
                learningtime = sl.Sum(o => o.tplt_learning_time) / 60;
            }
            else if (unit == 2) //sec
            {
                learningtime = sl.Sum(o => o.tplt_learning_time);
            }
            else if (unit == 3) //hour
            {
                learningtime = sl.Sum(o => o.tplt_learning_time) / 3600;
            }

            return learningtime;
        }


        public List<Learning_Time> Learning_Report(string trainingid, string participantid, string ttsam_id, int unit,int pageno=1,int pagesize=1)
        {
            decimal learningtime = 0;
            List<Learning_Time> sl = new List<Learning_Time>();
            SupportDB ABD = new SupportDB(_configuration);
            sl = ABD.Learning_Report(trainingid, participantid, ttsam_id, pageno, pagesize);


            return sl;
        }


        public List<Learning_Report_Data> Learning_Report_Data(string? trainingid = null, string? participantid = null, string? ttsam_id = null, string? branchid = null, int reporttype = 1,  string? fromdate = null, string? todate = null, int pageno = 1, int pagesize = -1, string? SearchColumn = null, string? searchvalue = null, string? sortcolumn = null, string? sortdirection = null)
        {
            decimal learningtime = 0;
            List<Learning_Report_Data> sl = new List<Learning_Report_Data>();
            SupportDB ABD = new SupportDB(_configuration);
            sl = ABD.Learning_Report_Data(trainingid, participantid, ttsam_id, branchid,reporttype,pageno,pagesize,SearchColumn,searchvalue,sortcolumn,sortdirection,fromdate,todate);


            return sl;
        }
    }
}
