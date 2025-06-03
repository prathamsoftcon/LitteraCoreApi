using LitteraCore.Common;
using LitteraCore.Controllers;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static LitteraCore.Models.MaskInfo;

namespace LitteraCore.DBContext
{

    public class CommonDB
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AgencyController> _logger;
        public CommonDB(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public static string CD_CHARGE_ID = "2724EDE6-BE47-4E77-B4F0-B3DF1ED97BF9";


        public static string Get_MaskData(int ismaskingreq, string data, Form form, int columntype)
        {
           
            

            if (ismaskingreq == 1)
            {
                // Check if masking is required for this column
                if (CHECK_MASK_FOR_COLUMN(form, columntype) == true)
                {
                    if (data.Length <= 3)
                        return new string('*', data.Length); // Return stars if data is too short

                    // Mask all but the last 3 digits/characters
                    return new string('*', data.Length - 3) + data.Substring(data.Length - 3);
                }
                else
                {
                    return data; // If no masking is needed, return the original data
                }
            }
            else
            {
                return data; // If masking is not required, return the original data
            }
        }

        public static bool CHECK_MASK_FOR_COLUMN(Form fd, int columntype)
        {
            bool ismasked = false;
            if (columntype == (int)CommonEnum.MaskingColumn.MOBILENO)
            {
                if (fd.Columns.Where(o => o.Contains(Enum.GetName(typeof(CommonEnum.MaskingColumn), CommonEnum.MaskingColumn.MOBILENO))).ToList().Count() > 0)
                {
                    ismasked = true;
                }

            }
            else if (columntype == (int)CommonEnum.MaskingColumn.EMAIL)
            {
                if (fd.Columns.Where(o => o.Contains(Enum.GetName(typeof(CommonEnum.MaskingColumn), CommonEnum.MaskingColumn.EMAIL))).ToList().Count() > 0)
                {
                    ismasked = true;
                }
            }
            else
            {
                ismasked = false;
            }
            return ismasked;
        }

        public static Form Get_Form_Masking_Info(string formid=null)
        {

            MaskDataWrapper maskDataWrapper = new MaskDataWrapper();


            List<Form> TS = new List<Form>();
            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "MaskingInfo.json"));  
            maskDataWrapper = JsonConvert.DeserializeObject<MaskDataWrapper>(jsontxt);
            Form formdetail = new Form();
            if (maskDataWrapper.MaskData.Where(o => o.Formid == formid).Count() > 0)
            {
                formdetail = maskDataWrapper.MaskData.Where(o => o.Formid == formid).FirstOrDefault();
            }

            //List<string> s = new List<string>();
            //s.Add("MOBILENO");
            //s.Add("EMAIL");
            //Form formdetail = new Form { Columns =s , Description="", Formid=""};

            return formdetail;
        }


        public  void LogSqlQuery(SqlCommand cmd, string methodname = null)
        {
            if (_configuration.GetSection("ApiKey").Value == "1")
            {
                // Prepare SQL query with parameters
                string query = cmd.CommandText;

                // Add parameters to the query string (to log them)
                foreach (SqlParameter param in cmd.Parameters)
                {
                    query += $"{param.ParameterName} = {param.Value}, ";
                }

                // Remove the trailing comma and space
                query = query.TrimEnd(',', ' ');

                // Log to a text file
                _logger.LogError("Post error.");
              

            }

        }


        public static Certificate Get_Certificate_Configuration(string trainngid = null)
        {
            Certificate TS = new Certificate();
            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Cetificate.json"));
            TS = JsonConvert.DeserializeObject<Certificate>(jsontxt);

            return TS;
        }
    }
 
}
