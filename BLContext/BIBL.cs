using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace LitteraCore.BLContext
{
    public class BIBL
    {
        private readonly IConfiguration _configuration;
        public BIBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PagedResult<BIReports> Get_BI_Report_List(PaginationParam param = null, string search = null)
        {
          

            string filtername = "1";
            string filtervalue = CommonEnum.Agency_Active_Status;
            List<BIReports> AL = new List<BIReports>();
            AL.Add(new BIReports { reportid = "d3142412-fb11-4814-904f-2aa3c134e5fe", workspaceid= "d7d617be-5737-4f79-8092-d1bba8d4ee7a", title = "Comments", description = "Comments" });
            AL.Add(new BIReports { reportid = "060073ee-2087-4453-9f79-05cef93d3e6c", workspaceid = "d7d617be-5737-4f79-8092-d1bba8d4ee7a", title = "Feedback Analytics", description = "Feedback Analytics" });
            AL.Add(new BIReports { reportid = "a1584f1e-866f-416c-8fd0-57eeeca4375c", workspaceid = "d7d617be-5737-4f79-8092-d1bba8d4ee7a", title = "GetAgency", description = "GetAgency" });
            AL.Add(new BIReports { reportid = "aab865c1-a180-4ecc-acf3-372c6646f905", workspaceid = "d7d617be-5737-4f79-8092-d1bba8d4ee7a", title = "Test Analytics", description = "Test Analytics" });
            AL.Add(new BIReports { reportid = "8b6b6195-3c75-4b6d-b094-7cc51d4db787", workspaceid = "d7d617be-5737-4f79-8092-d1bba8d4ee7a", title = "Trg Dashboard", description = "Trg Dashboard" });


            var pagedlist = PagedList<BIReports>.ToPagedList(AL.ToList(),
          param.PageNumber,
          param.PageSize);





            if (pagedlist.Count() > 0)
            {

                var metadata = new
                {
                    AL.Count,
                    pagedlist.PageSize,
                    pagedlist.CurrentPage,
                    pagedlist.HasNext,
                    pagedlist.HasPrevious,


                };
                return (new PagedResult<BIReports>
                {
                    Items = pagedlist,
                    TotalRecords = AL.Count,
                    PageSize = pagedlist.PageSize,
                    TotalPages = 1,
                    CurrentPage = pagedlist.CurrentPage
                });

            }
            else {
                return new PagedResult<BIReports> { };
            }
            

        }
    }
}
