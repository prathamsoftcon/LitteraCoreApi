using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LitteraCore.BLContext
{
    public class AgencyBL
    {
        private readonly IConfiguration _configuration;
        public AgencyBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public PagedResult<Agency> Get_Agency(string agencytypeid = null, string agencyid = null, string tat_type_id = null, PaginationParam param = null, string search=null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (search != null)
            {
                if (search != "")
                {
                    string[] sptsearch = search.Split(";".ToCharArray());
                    string[] sptsearchfields = sptsearch[0].Split(":".ToCharArray());
                    searchcolumn = sptsearchfields[0];
                    searchvalue = sptsearchfields[1];
                }
            }

            string filtername = "1";
            string filtervalue = CommonEnum.Agency_Active_Status;
            List<Agency> AL = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            AL = ABD.Get_Agency(agencytypeid, agencyid, param.PageNumber, param.PageSize, searchcolumn, searchvalue, filtername, filtervalue, tat_type_id);
            AL = AL.Where(o => o.agencyid.ToString().ToUpper() != CommonEnum.PortalAdmin_Agencyid.ToString().ToUpper()).ToList();
            AL = AL.Where(o => o.agencyid.ToString().ToUpper() != CommonEnum.SuperAdmin_Agencyid.ToString().ToUpper()).ToList();

          
           
        


            var pagedlist = PagedList<Agency>.ToPagedList(AL.ToList(),
          param.PageNumber,
          param.PageSize);


            if (param.PageSize == 0)
            {
                param.PageSize = AL.FirstOrDefault().totalcount;
            }


            if (pagedlist.Count() > 0)
            {
                var totalpages = (int)Math.Ceiling(AL.FirstOrDefault().totalcount / (double)param.PageSize);
               
              

                var metadata = new
                {
                    AL.FirstOrDefault().totalcount,
                    pagedlist.PageSize,
                    pagedlist.CurrentPage,
                    totalpages,
                    pagedlist.HasNext,
                    pagedlist.HasPrevious,


                };
                return (new PagedResult<Agency>
                {
                    Items = pagedlist,
                    TotalRecords = AL.FirstOrDefault().totalcount,
                    PageSize = pagedlist.PageSize,
                    TotalPages = totalpages,
                    CurrentPage = pagedlist.CurrentPage
                });

                // return Ok(pagedList);
            }
            else
                return new PagedResult<Agency> { };

        }

        public List<Agency> Get_Agency_by_charge(string chargeid)
        {
            List<Agency> a = new List<Agency>();

            AgencyDB ABD = new AgencyDB(_configuration);
            a = ABD.Get_Agency_by_charge(chargeid);

            return a;
        }

        public List<Agency> Get_Agency_Data(string agencytypeid, string agencyid, int pageno, int pagesize, string search, string tat_type_id = null)
        {
            string searchcolumn = null; string searchvalue = null;
            if (search != null)
            {
                if (search != "")
                {
                    string[] sptsearch = search.Split(";".ToCharArray());
                    string[] sptsearchfields = sptsearch[0].Split(":".ToCharArray());
                    searchcolumn = sptsearchfields[0];
                    searchvalue = sptsearchfields[1];
                }
            }


            string filtername = "1";
            string filtervalue = CommonEnum.Agency_Active_Status;

            List<Agency> AL = new List<Agency>();
            AgencyDB ABD = new AgencyDB(_configuration);
            AL = ABD.Get_Agency(agencytypeid, agencyid, pageno, pagesize, searchcolumn, searchvalue, filtername, filtervalue, tat_type_id);
            AL = AL.Where(o => o.agencyid.ToString().ToUpper() != CommonEnum.PortalAdmin_Agencyid.ToString().ToUpper()).ToList();
            AL = AL.Where(o => o.agencyid.ToString().ToUpper() != CommonEnum.SuperAdmin_Agencyid.ToString().ToUpper()).ToList();




            return AL;
        }
        public List<cast_category> Get_Cast_Category()
        {

            List<cast_category> f = new List<cast_category>();
            AgencyDB ABD = new AgencyDB(_configuration);
            f = ABD.Get_Cast_Category();
            return f;
        }

        public bool Update_Profile(string agencyid,[FromBody] Update_Profile_Data agency)
        {

            bool issaved = true;
            AgencyDB ABD = new AgencyDB(_configuration);
            issaved = ABD.Update_Profile_Data(agency);

            return issaved;
        }
        public List<SALUTATION> Get_Salutation()
        {
            List<SALUTATION> s = new List<SALUTATION>();
            AgencyDB adb = new AgencyDB(_configuration);
            s = adb.Get_SALUTATION();
            return s;
        }
        public UserBranch Get_User_Branche(string userid)
        {
            UserBranch s = new UserBranch();
            AgencyDB adb = new AgencyDB(_configuration);
            s = adb.Get_User_Branches(userid);
            return s;
        }

    }
}
