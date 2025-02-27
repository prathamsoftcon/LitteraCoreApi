using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LitteraCore.BLContext
{
    public class EvalBL
    {
        private readonly IConfiguration _configuration;
        public EvalBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<TEST_RESULT_DATA> GET_TRAINING_TEST_ANALYTIC_DATA(string usertype, string userid, string fromdate, string todate, string trainingid = null, int testtype = 3,string branchid=null)
        {
            EvalDB TBD = new EvalDB(_configuration);
            List<TEST_RESULT_DATA> T = new List<TEST_RESULT_DATA>();
            T = TBD.GET_TRAINING_TEST_ANALYTIC_DATA(usertype, userid, fromdate, todate, trainingid, testtype,branchid);
            T = Get_Test_Marks_Obtained(T);


            return T;
        }

        public List<TEST_RESULT_DATA> Get_Test_Marks_Obtained(List<TEST_RESULT_DATA> Resultdata)
        {
            foreach (TEST_RESULT_DATA d in Resultdata)
            {
                d.mark_obtained = Calculate_Test_Marks_Obtained(d.mark_per_question, d.iscorrect);
                d.mark_percentage = calculate_mark_percentage(d.mark_per_question, d.iscorrect);
            }


            return Resultdata;
        }

        public decimal calculate_mark_percentage(decimal mark_per_question, int iscorrect)
        {
            decimal markpercent = 0;
            decimal markobtained = 0;
            markobtained = Calculate_Test_Marks_Obtained(mark_per_question, iscorrect);
            markpercent = (markobtained / mark_per_question) * 100;
            return markpercent;

        }
        public decimal Calculate_Test_Marks_Obtained(decimal mark_per_question, int iscorrect)
        {
            decimal marksObtained = 0;
            if (iscorrect == (int)Common.CommonEnum.TEST_RESULT_OPTIONS.Correct)
            {
                marksObtained = mark_per_question * 1;
            }
            return marksObtained;
        }

        public  CompetencyConfiguration GET_SELF_TEST_CONFIGURATION(string trainngid =null)
        {
            CompetencyConfiguration TS = new CompetencyConfiguration();
            if (trainngid == null)
            {
              
                string Foldername = CommonEnum.GET_JSON_FOLDER();
                string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Competency_Config.json"));
                TS = JsonConvert.DeserializeObject<CompetencyConfiguration>(jsontxt);
             

            }
            else
            {

                TrainingDB WDB = new TrainingDB(_configuration);
                Training trgdetail = new Training();
                trgdetail = WDB.Get_Particular_Training_Detail(trainngid);
                if (trgdetail.trg_Setting != null)
                {
                    if(trgdetail.trg_Setting.Session.Questions_Self_Test != null)
                    {
                  
                        string Foldername = CommonEnum.GET_JSON_FOLDER();
                        string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Competency_Config.json"));
                        TS = JsonConvert.DeserializeObject<CompetencyConfiguration>(jsontxt);
                        TS.no_of_question = trgdetail.trg_Setting.Session.Questions_Self_Test;
                    }
                    else
                    {
                        string Foldername = CommonEnum.GET_JSON_FOLDER();
                        string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Competency_Config.json"));
                        TS = JsonConvert.DeserializeObject<CompetencyConfiguration>(jsontxt);
                    }
                

                }
                else
                {
               
                    string Foldername = CommonEnum.GET_JSON_FOLDER();
                    string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Competency_Config.json"));
                    TS = JsonConvert.DeserializeObject<CompetencyConfiguration>(jsontxt);
                 
                }

              
            }

            return TS;
        }

    }

}

