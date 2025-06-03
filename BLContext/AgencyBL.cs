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


        public bool Update_Personal_info(string agencyid, string agencytypeid, string branchid, string createdby, Agency_PersonalInfo pi)
        {
            AgencyDB ABD = new AgencyDB(_configuration);
            bool issaved = false;
            string OtherXML = "";
            DataTable dtXML = new DataTable();
            dtXML.Columns.Add("DESIGNATION");
            dtXML.Columns.Add("CURRENTPOSTING");
            dtXML.Columns.Add("ISBHOPAL");
            dtXML.Columns.Add("ID_PROOF_TYPE");
            dtXML.Columns.Add("ID_PROOF_VALUE");
            dtXML.Columns.Add("FATHER_NAME");
            dtXML.Columns.Add("MOTHER_NAME");
            dtXML.Columns.Add("CAST");
            dtXML.Columns.Add("UPLOAD_PATH");

            //Code to get Agency old Data

            AgencyAdditionalInfo AI = new AgencyAdditionalInfo();
            AI = ABD.Get_Agency_Additionl_Info(agencyid, agencytypeid);

            if (pi.OtherInfo.class_or_term != null)
            {
                AI.DESIGNATION = pi.OtherInfo.class_or_term;
            }
            if (pi.OtherInfo.school != null)
            {
                AI.CURRENTPOSTING = pi.OtherInfo.school;
            }
            AI.CAST = pi.OtherInfo.cast_category;
            AI.FATHER_NAME = pi.OtherInfo.fathername;
            AI.MOTHER_NAME = pi.OtherInfo.mothername;
            AI.ID_PROOF_TYPE = pi.OtherInfo.id_type;
            AI.ID_PROOF_VALUE = pi.OtherInfo.id_no;
            
            if (AI != null)
            {
                DataRow dr = dtXML.NewRow();
                if (AI.DESIGNATION != null)
                {
                    dr["DESIGNATION"] = AI.DESIGNATION;
                }
                if (AI.CURRENTPOSTING != null)
                {
                    dr["CURRENTPOSTING"] = AI.CURRENTPOSTING;
                }
                if (AI.ISBHOPAL != null)
                {
                    dr["ISBHOPAL"] = AI.ISBHOPAL;
                }
                if (AI.ID_PROOF_TYPE != null)
                {
                    dr["ID_PROOF_TYPE"] = AI.ID_PROOF_TYPE;
                }
                if (AI.ID_PROOF_VALUE != null)
                {
                    dr["ID_PROOF_VALUE"] = AI.ID_PROOF_VALUE;
                }
                if (AI.FATHER_NAME != null)
                {
                    dr["FATHER_NAME"] = AI.FATHER_NAME;
                }
                if (AI.MOTHER_NAME != null)
                {
                    dr["MOTHER_NAME"] = AI.MOTHER_NAME;
                }
                if (AI.CAST != null)
                {
                    dr["CAST"] = AI.CAST;
                }
                if (AI.DOC_PATH != null)
                {
                    dr["UPLOAD_PATH"] = AI.DOC_PATH;
                }
                dtXML.Rows.Add(dr);
                dtXML.TableName = "ADDINFO";
                StringWriter sr = new StringWriter();
                dtXML.WriteXml(sr);
                OtherXML = sr.ToString();
            }






            issaved = ABD.Update_Agency_Personal_Info(agencyid, agencytypeid, branchid, createdby, pi, OtherXML);
            return issaved;
        }

        public bool Update_Address_info(string agencytypeid, string agencyid, string branchid, string createdby, Agency_Participant_AddressInfo pi)
        {
            AgencyDB ABD = new AgencyDB(_configuration);
            bool issaved = false;

            issaved = ABD.Update_Agency_Address_Info(agencyid, agencytypeid, branchid, createdby, pi);
            return issaved;
        }
        public bool Update_Other_info(string agencyid, string agencytypeid, string branchid, string createdby, Agency_Participant_OtherInfo pi)
        {
            AgencyDB ABD = new AgencyDB(_configuration);
            bool issaved = false;
            string OtherXML = "";
            DataTable dtXML = new DataTable();
            dtXML.Columns.Add("DESIGNATION");
            dtXML.Columns.Add("CURRENTPOSTING");
            dtXML.Columns.Add("ISBHOPAL");
            dtXML.Columns.Add("ID_PROOF_TYPE");
            dtXML.Columns.Add("ID_PROOF_VALUE");
            dtXML.Columns.Add("FATHER_NAME");
            dtXML.Columns.Add("MOTHER_NAME");
            dtXML.Columns.Add("CAST");
            dtXML.Columns.Add("DOC_PATH");

            //Code to get Agency old Data

            AgencyAdditionalInfo AI = new AgencyAdditionalInfo();
            AI = ABD.Get_Agency_Additionl_Info(agencyid, agencytypeid);
            AI.DESIGNATION = pi.class_or_term;
            AI.CURRENTPOSTING = pi.school;
            AI.DOC_PATH = pi.doc_Path;
        
            if (pi.cast_category != null)
            {
                AI.CAST = pi.cast_category;
            }
            if (pi.fathername != null)
            {
                AI.FATHER_NAME = pi.fathername;
            }
            if (pi.mothername != null)
            {
                AI.MOTHER_NAME = pi.mothername;
            }
            if (pi.id_type != null)
            {
                AI.ID_PROOF_TYPE = pi.id_type;
            }
            if (pi.id_no != null)
            {
                AI.ID_PROOF_VALUE = pi.id_no;
            }
            if (pi.id_no != null)
            {
                AI.ID_PROOF_VALUE = pi.id_no;
            }
        
            DataRow dr = dtXML.NewRow();
            dr["DESIGNATION"] = AI.DESIGNATION;
            dr["CURRENTPOSTING"] = AI.CURRENTPOSTING;
            dr["ISBHOPAL"] = AI.ISBHOPAL;
            dr["ID_PROOF_TYPE"] = AI.ID_PROOF_TYPE;
            dr["ID_PROOF_VALUE"] = AI.ID_PROOF_VALUE;
            dr["FATHER_NAME"] = AI.FATHER_NAME;
            dr["MOTHER_NAME"] = AI.MOTHER_NAME;
            dr["CAST"] = AI.CAST;
            dr["DOC_PATH"] = AI.DOC_PATH;
            dtXML.Rows.Add(dr);

            dtXML.TableName = "ADDINFO";
            StringWriter sr = new StringWriter();
            dtXML.WriteXml(sr);
            OtherXML = sr.ToString();
            issaved = ABD.Update_Agency_Other_Info(agencyid, agencytypeid, branchid, createdby, OtherXML);
            return issaved;
        }
    }
}
