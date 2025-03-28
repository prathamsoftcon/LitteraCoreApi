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
    }
}
