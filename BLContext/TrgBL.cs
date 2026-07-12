using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.Controllers;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.PowerBI.Api;
using Newtonsoft.Json;
using Serilog;
using System.Collections;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using static LitteraCore.Common.CommonEnum;
using static QRCoder.PayloadGenerator;

namespace LitteraCore.BLContext
{
    public class TrgBL
    {
        private readonly IConfiguration _configuration;
       
        public TrgBL(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        public Hashtable Calculate_trg_actual_amt(string trainingid, string expensetype, string trg_sponsortype, int noofparticipant_reg)
        {
            Hashtable ht = new Hashtable();
            Decimal sponsoredamt = 0;
            Decimal actual_fees = 0;
            Decimal feesperparticipant = 0;
            Decimal discountpercent = 0;

            DataTable dtexpense = new DataTable();
            TrgDB tdb = new TrgDB(_configuration);
            dtexpense = tdb.Get_Trg_fees_details(trainingid);
            foreach (DataRow dr in dtexpense.Rows)
            {
                decimal qty = 0;
                decimal rate = 0;
                decimal amt = 0;
                if (Convert.ToString(dr["t_actual_qty"]) != "")
                {
                    qty = Convert.ToDecimal(dr["t_actual_qty"]);
                }
                if (Convert.ToString(dr["T_Rate"]) != "")
                {
                    rate = Convert.ToDecimal(dr["T_Rate"]);
                }
                amt = qty * rate;
                sponsoredamt = sponsoredamt + amt;

                //Calculate Fees Per Participant
                if (dr["T_ExpHeadId"].ToString().ToUpper() != CommonEnum.Discount_Ledgerid)
                {
                    feesperparticipant = feesperparticipant + rate;
                }
                else
                {
                    if (Convert.ToString(dr["T_Rate"]) != "")
                    {
                        discountpercent = discountpercent + Convert.ToDecimal(dr["T_Rate"]);
                    }
                }

            }
            feesperparticipant = feesperparticipant - ((feesperparticipant * discountpercent)) / 100;

            if (expensetype != "0") //Variable Fees Case
            {
                //actual_fees = sponsoredamt * noofparticipant_reg;
                actual_fees = sponsoredamt;
            }
            else
            {
                actual_fees = sponsoredamt;
            }
            ht.Add("actual_fees", actual_fees);
            ht.Add("feesperparticipant", feesperparticipant);
            return ht;
        }
        public Decimal Calculate_trg_received_amt(string trainingid)
        {
            decimal totalreceivedamt = 0;
            TrgDB tbd = new TrgDB(_configuration);
            DataTable dtreceived = new DataTable();
            dtreceived = tbd.Get_Trg_received_Amt(trainingid);
            foreach (DataRow dr in dtreceived.Rows)
            {
                if (Convert.ToString(dr["TotalAmount"]) != "")
                {
                    totalreceivedamt = totalreceivedamt + Convert.ToDecimal(dr["TotalAmount"]);
                }
            }

            return totalreceivedamt;
        }

        public Training Get_Particular_Training(string trainingid)
        {
            Training T = new Training();
            TrgDB tdb=new TrgDB(_configuration);
            T = tdb.Get_Particular_Training(trainingid).FirstOrDefault();

            return T;
        }


        public List<TrainingCategory> Get_Training_Category(string catrgoryid)
        {

            List<TrainingCategory> TC = new List<TrainingCategory>();
            TrainingDB TDB = new TrainingDB(_configuration);
            TC = TDB.Get_training_Category(catrgoryid);


            List<TrainingCategory> TCG = new List<TrainingCategory>();
            //Logic to group all data on basis of parentcategoryid

            foreach (TrainingCategory c in TC)
            {
                if (TCG.Where(o => o.TrainingCategoryId.ToString().ToUpper() == c.TrainingCategoryId.ToString().ToUpper()).Count() <= 0)
                {
                    TCG.Add(c);
                }

                List<TrainingCategory> FilteredCat = new List<TrainingCategory>();
                FilteredCat = Get_Sub_Categories(TC, c.TrainingCategoryId);
                foreach (TrainingCategory ftc in FilteredCat)
                {
                    if (TCG.Where(o => o.TrainingCategoryId.ToString().ToUpper() == ftc.TrainingCategoryId.ToString().ToUpper()).Count() <= 0)
                    {
                        TCG.Add(ftc);
                    }

                }

            }




            //

            foreach (TrainingCategory c in TCG)
            {
                c.categorylevel = Get_Categories_Level(TC, c.TrainingCategoryId);
            }


            return TCG;
        }

        // Added 2026-07-09 for the frm_Master_Configuration.aspx -> React migration
        // (Training Category tab, "Save" action). See TrainingDB.Save_Training_Category
        // for the traced stored-procedure detail.
        public bool Save_Training_Category(TrainingCategory category)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Save_Training_Category(category);
        }

        // Added 2026-07-09 for the frm_Master_Configuration.aspx -> React migration
        // (Training Category tab, "Delete" action). See
        // TrainingDB.Delete_Training_Category for the traced stored-procedure detail.
        public bool Delete_Training_Category(string trainingCategoryId)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Delete_Training_Category(trainingCategoryId);
        }

        // Added 2026-07-09 for the frm_Master_Configuration.aspx -> React migration
        // (Training Category tab, "in use?" check). See
        // TrainingDB.Chk_Category_In_Use for the traced SQL-function detail,
        // including the real old-source path this was found through.
        public bool Chk_Category_In_Use(string categoryDetailId)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Chk_Category_In_Use(categoryDetailId);
        }

        public List<TrainingCategory> Get_Sub_Categories(List<TrainingCategory> CL, string categoryid)
        {
            List<TrainingCategory> subcategories = new List<TrainingCategory>();
            List<TrainingCategory> F = CL.Where(o => o.parentcategoryid?.ToString().ToUpper() == categoryid.ToString().ToUpper()).ToList();
            foreach (TrainingCategory c in F)
            {
                subcategories.Add(c);


            }


            foreach (TrainingCategory TC in F)
            {
                List<TrainingCategory> GF = CL.Where(o => o.parentcategoryid?.ToString().ToUpper() == TC.TrainingCategoryId.ToString().ToUpper()).ToList();
                foreach (TrainingCategory c in GF)
                {
                    subcategories.Add(c);

                }

                foreach (TrainingCategory T in GF)
                {
                    List<TrainingCategory> GGF = CL.Where(o => o.parentcategoryid?.ToString().ToUpper() == T.TrainingCategoryId.ToString().ToUpper()).ToList();
                    foreach (TrainingCategory c in GGF)
                    {
                        subcategories.Add(c);

                    }
                }

            }

           
            return subcategories;
        }
        public int Get_Categories_Level(List<TrainingCategory> CL, string categoryid)
        {
            int level = 0;
            bool isParentnull = false;
            while (isParentnull != true)
            {
                string parentid = null;
                if(CL.Where(o => o.TrainingCategoryId.ToString().ToUpper() == categoryid.ToString().ToUpper()).Count() > 0)
                {
                    parentid = CL.Where(o => o.TrainingCategoryId.ToString().ToUpper() == categoryid.ToString().ToUpper()).FirstOrDefault().parentcategoryid;
                }
             
                if (parentid == null)
                {
                    isParentnull = true;

                }
                else
                {
                    level = level + 1;
                    categoryid = parentid;
                }


            }
            return level;
        }

        public List<Training> Get_VW_Training_calendar(DateTime fromdate, DateTime todate)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Training> trgdata = new List<Training>();
            TrainingDB tdb=new TrainingDB(_configuration);
            trgdata = tdb.Get_VW_Training_calendar(fromdate, todate);


            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }
        public List<TRG_DAY_WEEK> Get_Day_Week_Count(string fromdate, string todate)
        {
            List<TRG_DAY_WEEK> dayweek = new List<TRG_DAY_WEEK>();
            TrainingDB tdb = new TrainingDB(_configuration);
            dayweek = tdb.Get_Day_Week_Count_Id(fromdate, todate);
            return dayweek;
        }

        public string GET_TRG_CERTIFICATE(string trainingid, string participantid,string branchid,string APPURL,string Logo_PATH)
        {
            TrainingDB tbl=new TrainingDB(_configuration);
            string participantname = "";
            string trainingtype = "";
            string trainingname = "";
            string organisationname = "";
            string startdate = "";
            string enddate = "";

            string f_signatoryname = "";
            string f_singatorysign = "";
            string s_signatoryname = "";
            string s_singatorysign = "";
            string logo = "";

            Training Trg = new Training();
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            PaginationParam param=new PaginationParam {  PageNumber=1,PageSize=10 };

            trainingname = Trg.T_Name;
            trainingtype = Convert.ToString(Trg.trg_type);
            enddate = Convert.ToDateTime(Trg.T_EndDate).ToString("yyyy/MM/dd");
            startdate = Convert.ToDateTime(Trg.T_StartDate).ToString("yyyy/MM/dd");
            
            Agency loginbranchdetail= new Agency();
            AgencyBL abl = new AgencyBL(_configuration);
            loginbranchdetail = abl.Get_Agency("00001", branchid,null, param,null).Items.FirstOrDefault();
            organisationname = loginbranchdetail.agencyname;
            logo = APPURL +"/"+loginbranchdetail.ag_photo_path;

            Agency ParticpantDetail= new Agency();
            ParticpantDetail = abl.Get_Agency(null, participantid, null, param, null).Items.FirstOrDefault();

            participantname = ParticpantDetail.salutation_txt + " " + ParticpantDetail.agencyname;

            StringBuilder sb = new StringBuilder();

            string participantname_padding = (44 - (0.6 * participantname.Length)).ToString();

            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);

         
            string certtext = "";
            ClientData lo = ClientData.Get_Client_Data();
          
            certtext = lo.CERTIFICATE_TXT;
            certtext = certtext.Replace("(#trgname#)", trainingname);
            certtext = certtext.Replace("(#startdate#)", Convert.ToDateTime(startdate).ToString("dd-MMMM-yyyy"));
            certtext = certtext.Replace("(#enddate#)", Convert.ToDateTime(enddate).ToString("dd-MMMM-yyyy"));
            certtext = certtext.Replace("(#clientname#)", organisationname);

            sb.Append("<div style='background-image:url("+ APPURL + "/theme/images/certificate_bg.png);background-repeat:no-repeat;object-fit:cover;background-size: 100% 100%;;width:calc(100%-100px);height:100%;-webkit-print-color-adjust: exact;position:relative;min-height:869px'>");
            sb.Append("<div style='text-align:center;padding-top: 7%;padding-left:10px;padding-right:10px'>");
            sb.Append("<label style='color: #04047d; font-size: 57px; font-weight: 100; letter-spacing: 7px;'>" + organisationname + "</label>");
            sb.Append("</div>");
            sb.Append("<div style='text-align:center;padding-top: 2%;'>");
            sb.Append("<label style='color: cornflowerblue; font-size: 15px; margin-right: 20px;'><img src='" + logo + "' style='width: 185px; margin-right: 0px; margin-left: -24px;' /></label>");
            sb.Append("</div>");
            sb.Append("<div style='text-align:center;padding-top: 2%;'>");
            sb.Append("<label style='color: #04047d; font-size: 57px; font-weight: 100; letter-spacing: 7px;'>CERTIFICATE</label>");
            sb.Append("</div>");
            sb.Append("<div style='text-align:center;padding-top: 0%;'>");
            sb.Append("<label style='font-size: 29px; padding-left: 0%;font-family: Brush Script MT;color:#428bca;margin-left:25px'>This Certificate is presented to</label>");
            sb.Append("</div>");
            sb.Append("<div style='padding-left: 35%;padding-top: 0%;'>");
            sb.Append("<div style='width:135px;display:inline-block;margin-left:20px'> <hr/></div><div style='width:50x;display:inline-block;margin-left:5px; margin-right:4px'><img src='"+ APPURL + "/theme/images/cert_icon.png'></div> <div style='width:135px;display:inline-block'> <hr/></div>");
            sb.Append("</div>");
            sb.Append("<div style='text-align:center;padding-top: 2%;'>");
            sb.Append("<label style=' font-size: 45px; font-family: cursive; font-style: oblique; font-weight:500;text-transform:uppercase'>" + participantname + "</label>");
            sb.Append("</div>");
            sb.Append("<div style='padding-left: 10%;padding-top: 2%;padding-right: 10%'>");
            sb.Append("<label style='font-size: 22px;color: #04047c;font-stretch:'> " + certtext + "</label>");
            sb.Append("</div>");
            sb.Append("<div style='padding-left: 10%;padding-top: 9%;top:64%;position:absolute;width:100%'>");
            sb.Append("<div style='width:13%;display:inline-block;margin-right:45px'>");
            sb.Append("<label style='color: gray; font-size: 15px; margin-left: 15px;margin-right: -9px; '>Date: </label> <label style='color: black; font-size: 15px; margin-left: 15px;margin-right: -9px; '>" + Convert.ToDateTime(enddate).ToString("dd/MM/yyyy") + "</label>");
            sb.Append("</div>");

            string leftpadding = "0";
            if (dtsignatory.Count == 3)
            {
                leftpadding = "5%";
            }
            else if (dtsignatory.Count == 2)
            {
                leftpadding = "5%";
            }
            else if (dtsignatory.Count == 1)
            {
                leftpadding = "40%";
            }
            else
            {
                leftpadding = "0";
            }

            foreach(CERTIFICATE_SIGNATORY sign in dtsignatory)
            {
                string displayimg = "";
                if (string.IsNullOrEmpty(sign.signaturepath))
                {
                    displayimg = "hidden";
                }

                sb.Append("<div style='width:15%;display:inline-block;text-transform:uppercase;padding-left:" + leftpadding + "'>");
                sb.Append("<img src='"+ APPURL + "/"+Logo_PATH+"/" + sign.signaturepath + "' style='width: 130px;height: 50px;visibility:" + displayimg + "' />");
                sb.Append("<label style='color: black; font-size: 15px; margin-left: 6px;display:block'>" + sign.name + " </label>");
                sb.Append("<label style='color: black; font-size: 15px; margin-left: 6px;display:block'>(" + sign.designation + ") </label>");
                sb.Append("</div>");
            }


            sb.Append("</div></div>");
            return sb.ToString();
        }

        public List<CERTIFICATE_SIGNATORY> Get_Signatory(string trainingid)
        {
            List<CERTIFICATE_SIGNATORY> signatory = new List<CERTIFICATE_SIGNATORY>();
            TrainingDB tdb = new TrainingDB(_configuration);
            signatory = tdb.Get_Certificate_signatory(trainingid);
            return signatory;
        }

        //public List<CERTIFICATE_SIGNATORY> Check_Certificate_signatory(string trainingid)
        //{


        //    List<CERTIFICATE_SIGNATORY> signatory = new List<CERTIFICATE_SIGNATORY>();
        //    TrainingDB tdb = new TrainingDB(_configuration);
        //    signatory = tdb.Get_Certificate_signatory(trainingid);
        //    return signatory;
        //}

        public bool Save_Trg_Participant_Mapping(TRGMAPPING trgmapping)
        {
            //*********Code to get training details
            TrainingDB WDB = new TrainingDB(_configuration);

            Training lwtc = new Training();
            List<FilterUserTrg> userwise_lwtc = new List<FilterUserTrg>();
            lwtc = WDB.Get_Particular_Training_Detail(trgmapping.trainingid);


            //

            //************Rule to send status
            if (lwtc.Training_SponsorType == 4) // in case of paid training
            {
                trgmapping.status = 0;
                //Send Mail to cd for register but not paid

            }
            else
            {
                ApplicationSetting AS = new ApplicationSetting();
                ParticipantApproval ml = new ParticipantApproval();
                ApplicationConfigDB ACD = new ApplicationConfigDB(_configuration);
                ml = JsonConvert.DeserializeObject<ParticipantApproval>(ACD.Get_Application_Setting("2").Rows[0]["SettingValue"].ToString());
                if (ml.ENROLL_AUTO_APPROVAL == 1)
                {
                    // user.agency.agencystatus = ml.REG_APPROVAL_STATUS.ToString();
                    //Check Participant Reg status
                    trgmapping.status = 1;
                    DMSBL dbl = new DMSBL(_configuration);
                    int participantstatus = 0;
                    List<DMS> DL = new List<DMS>();
                    DL = dbl.Get_DMS_STATUS(trgmapping.participantid, Convert.ToInt32(Common.CommonEnum.DMS_TAT_TYPE_ID.Participant_REGIS));
                    participantstatus = DL.FirstOrDefault().doc_status;
                    if (participantstatus == 0)
                    {

                        DMS d = new DMS
                        {
                            docno = DL.FirstOrDefault().docno,
                            doc_id = DL.FirstOrDefault().doc_id,
                            createdon = DateTime.Now,
                            createdby = trgmapping.Createdby,
                            branchid = trgmapping.branchid,
                            docdate = DateTime.Now,
                            actiondate = DateTime.Now,
                            CreatedBy_empid = trgmapping.Createdby,
                            fwd_empid = trgmapping.Createdby,
                            tat_type_id = DL.FirstOrDefault().tat_type_id,
                            doc_status = 1,

                        };

                        DMSBL DBL = new DMSBL(_configuration);
                        bool s = DBL.Update_DMS_DATA(d);
                    }




                }
                else
                {
                    trgmapping.status = 0;
                    //Send mail to participant
                    //Send mail to cd with approval link
                }
            }
            //*******


            TrainingDB TDB = new TrainingDB(_configuration);
            bool issaved = TDB.Save_Trg_Participant_Mapping(trgmapping);
            return issaved;
        }

        public List<Trg_Type> Get_Trg_Type()
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Trg_Type> trgdata = new List<Trg_Type>();
            TrainingDB tdb = new TrainingDB(_configuration);
            trgdata = tdb.Get_Trg_Type();


            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        // Added 2026-07-11 for the frm_training_type.aspx -> React migration.
        // Thin wrappers - see TrainingDB.Save_Trg_Type /
        // TrainingDB.Update_Trg_Type_Status for the traced stored-procedure
        // detail.
        public bool Save_Trg_Type(Trg_Type type)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Save_Trg_Type(type);
        }

        public bool Update_Trg_Type_Status(string tttt_id, string tttt_active, string createdBy, string remark)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Update_Trg_Type_Status(tttt_id, tttt_active, createdBy, remark);
        }

        // Added 2026-07-11 for the frm_Master_Configuration.aspx -> React
        // migration (Fees tab, Sponsor Type dropdown). See
        // TrainingDB.Get_Trg_Sponsor_Type for the traced stored-procedure
        // detail - mirrors Get_Trg_Type immediately below.
        public List<Trg_Sponsor_Type> Get_Trg_Sponsor_Type()
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Get_Trg_Sponsor_Type();
        }

        public List<Trg_Title> Get_Trg_Title()
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Trg_Title> trgdata = new List<Trg_Title>();
            TrainingDB tdb = new TrainingDB(_configuration);
            trgdata = tdb.Get_Trg_Title();


            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        // Added 2026-07-10 for the frm_Master_Configuration.aspx -> React
        // migration (Training Title tab, "Save"/"Delete" actions). See
        // TrainingDB.Save_Trg_Title / TrainingDB.Delete_Trg_Title for the
        // traced stored-procedure detail, including the real old-source
        // path this was found through.
        public bool Save_Trg_Title(Trg_Title title)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Save_Trg_Title(title);
        }

        public bool Delete_Trg_Title(string courseId)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Delete_Trg_Title(courseId);
        }

        // Added 2026-07-11 for the frm_Master_Configuration.aspx -> React
        // migration (Fees tab - Load/Save/Delete/"in use?" check). See
        // TrainingDB's Get_Trg_Fees_Master / Save_Trg_Fees_Master /
        // Delete_Trg_Fees_Master / Chk_Fees_In_Use for the traced
        // stored-procedure/scalar-function detail.
        public List<Trg_Fees_Master> Get_Trg_Fees_Master(string feesid = null)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Get_Trg_Fees_Master(feesid);
        }

        public bool Save_Trg_Fees_Master(Trg_Fees_Master fees)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Save_Trg_Fees_Master(fees);
        }

        public bool Delete_Trg_Fees_Master(string feesId)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Delete_Trg_Fees_Master(feesId);
        }

        public bool Chk_Fees_In_Use(string feesId)
        {
            TrainingDB TDB = new TrainingDB(_configuration);
            return TDB.Chk_Fees_In_Use(feesId);
        }

        public string Geenerate_certificate_text(Training Trg, List<CERTIFICATE_SIGNATORY> dtsignatory, string participantid,string APPURL,string Logo_Path)
        {
           
            string cert = "";
            
            Certificate ct = new Certificate();
            ct=CommonDB.Get_Certificate_Configuration();
            string certificateHtml = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Certificate</title>
    <link rel=""stylesheet"" type=""text/css"" href=""style.css"" />
</head>
<body>
    <div class="""">
        <div class=""certificate-container"" id=""certificate"">
            <img class=""certificate-img"" src=""certificate.png""/>
            <div class=""certificate"">
                <div class=""certificate-text"">
                    <p>##certtext##</p>
                    <p></p>
                </div>
                <div class=""certificate-footer"">
                    <div class=""date"">
                        <p>Date: ""##PrintDate##""</p>
                    </div>
                 ##Sinatory##
                </div>
            </div>
        </div>
    </div>
</body>
</html>";


            Agency loginbranchdetail = new Agency();
            AgencyBL abl = new AgencyBL(_configuration);
            Agency ParticpantDetail = new Agency();
            PaginationParam param = new PaginationParam { PageNumber = 1, PageSize = 10 };
            ParticpantDetail = abl.Get_Agency(null, participantid, null, param, null).Items.FirstOrDefault();


          
            TrainingDB tbl = new TrainingDB(_configuration);
     

            List<variables> lv = new List<variables>();
            lv = ct.variables.ToList();
            string f_cert_text = "";
            f_cert_text = ct.certificate_text;
            foreach (variables v in lv)
            {
                if (ParticpantDetail.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                {
                    var propInfo = ParticpantDetail.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var value = propInfo.GetValue(ParticpantDetail, null)?.ToString() ?? "";
                    f_cert_text = f_cert_text.Replace(v.name, value);
                }
                else if (ParticpantDetail.additionalInfo.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                {
                    var propInfo = ParticpantDetail.additionalInfo.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var value = propInfo.GetValue(ParticpantDetail.additionalInfo, null)?.ToString() ?? "";
                    f_cert_text = f_cert_text.Replace(v.name, value);
                }
                else if (Trg.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                {
                    var propInfo = Trg.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var value = propInfo.GetValue(Trg, null)?.ToString() ?? "";
                    f_cert_text = f_cert_text.Replace(v.name, value);
                }

            }



            string signatorytext = "";
           
            int signatoryindex = 0;
            foreach (CERTIFICATE_SIGNATORY sign in dtsignatory)
            {
                signatoryindex = signatoryindex + 1;
                signatorytext = signatorytext + @" <div class=""signature signature-@"+ signatoryindex + @""">
                <img src='="+ APPURL + @"/""@""@"+ Logo_Path + @"""/""@" + sign.signaturepath +@"' style='width: 130px;height: 50px;visibility:"" + displayimg + ""' />
                        <p>" + sign.name + @"</p>
                        <p>" + sign.designation + @"</p>
                    </div>";
            }

            certificateHtml = certificateHtml.Replace("certificate.png", (APPURL + "/" + ct.certificate_bg_path).Replace("//","/"));
            certificateHtml = certificateHtml.Replace("style.css", APPURL + "/css/certificate_style.css");
            certificateHtml = certificateHtml.Replace("##PrintDate##", System.DateTime.Now.ToString("dd-MM-yyyy"));
            certificateHtml = certificateHtml.Replace("##certtext##", f_cert_text);
            certificateHtml = certificateHtml.Replace("##Sinatory##", signatorytext);



            return certificateHtml;
        }

        public string Geenerate_certificate_text_with_QR(Training Trg, List<CERTIFICATE_SIGNATORY> dtsignatory, string participantid, string APPURL, string Logo_Path,string? certificateid=null)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "Log.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.AppendAllText(path, "Step 1 completed");

            certificate_obj certificatedata =new certificate_obj();
            ParticipantDB pdb = new ParticipantDB(_configuration);
            certificatedata = pdb.Get_Certificate_info(null, certificateid).FirstOrDefault();



            File.AppendAllText(path, "Step 2 completed");

            REACT_APP_CONFIGURATION RAC = new REACT_APP_CONFIGURATION();

            string Foldername = CommonEnum.GET_JSON_FOLDER();
            string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "Config.json"));
            RAC = JsonConvert.DeserializeObject<REACT_APP_CONFIGURATION>(jsontxt);

            File.AppendAllText(path, "Step 3 completed");
            string cert = "";
            string QRstr = CommonEnum.generate_qr_code(RAC.REACT_APP_LOGOUT_PATH +"/verifycertificate/"+ certificateid);

            Certificate ct = new Certificate();
            ct = CommonDB.Get_Certificate_Configuration();

            File.AppendAllText(path, "Step 4 completed");

            string certificateHtml = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Certificate</title>
    <link rel=""stylesheet"" type=""text/css"" href=""style.css"" />
</head>
<body>
    <div class="""">
        <div class=""certificate-container"" id=""certificate"">
            <img class=""certificate-img"" src=""certificate.png""/>
            <div class=""certificate"">
                <div class=""certificate-text"">
                    <p>##certtext##</p>
                    <p></p>
                </div>
                <div class=""certificate-footer"">
                    <div class=""date"">
                        <p>Date: ""##PrintDate##""</p>
                       QRstr
                    </div>
                 ##Sinatory##
                </div>
            </div>
        </div>
    </div>
</body>
</html>";

            File.AppendAllText(path, "Step 5 completed");
            Agency loginbranchdetail = new Agency();
            AgencyBL abl = new AgencyBL(_configuration);
            Agency ParticpantDetail = new Agency();
            PaginationParam param = new PaginationParam { PageNumber = 1, PageSize = 10 };
            ParticpantDetail = abl.Get_Agency(null, participantid, null, param, null).Items.FirstOrDefault();



            TrainingDB tbl = new TrainingDB(_configuration);

            File.AppendAllText(path, "Step 6 completed");

            List<variables> lv = new List<variables>();
            lv = ct.variables.ToList();
            string f_cert_text = "";
            f_cert_text = ct.certificate_text;

            File.AppendAllText(path, "Step 7 completed");
           // File.AppendAllText(path, JsonConvert.SerializeObject(Trg));
            foreach (variables v in lv)
            {
                if (ParticpantDetail.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                {
                    File.AppendAllText(path, "Step inside 1 completed");
                    var propInfo = ParticpantDetail.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var value = propInfo.GetValue(ParticpantDetail, null)?.ToString() ?? "";
                    f_cert_text = f_cert_text.Replace(v.name, value);
                }
                else if (ParticpantDetail.additionalInfo !=null)
                {
                    if(ParticpantDetail?.additionalInfo.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                    {
                        var propInfo = ParticpantDetail.additionalInfo.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        var value = propInfo.GetValue(ParticpantDetail.additionalInfo, null)?.ToString() ?? "";
                        f_cert_text = f_cert_text.Replace(v.name, value);
                    }
                    else if (Trg.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                    {
                        File.AppendAllText(path, "Step inside 3 completed");
                        var propInfo = Trg.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        File.AppendAllText(path, "Step inside 2 completed");
                        var value = propInfo.GetValue(Trg, null)?.ToString() ?? "";
                        File.AppendAllText(path, "Step inside 3 completed");
                        f_cert_text = f_cert_text.Replace(v.name, value);
                    }

                }
                else if (Trg.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null)
                {
                    File.AppendAllText(path, "Step inside 3 completed");
                    var propInfo = Trg.GetType().GetProperty(v.replacecolumnvalue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    File.AppendAllText(path, "Step inside 2 completed");
                    var value = propInfo.GetValue(Trg, null)?.ToString() ?? "";
                    File.AppendAllText(path, "Step inside 3 completed");
                    f_cert_text = f_cert_text.Replace(v.name, value);
                }

            }

            File.AppendAllText(path, "Step 8 completed");

            string signatorytext = "";

            int signatoryindex = 0;
            if(Trg.trg_Setting != null)
            {
                if(Trg.trg_Setting.certificate_setting != null)
                {
                    if(Trg.trg_Setting.certificate_setting.no_of_signatory_required > 0)
                    {
                        foreach (CERTIFICATE_SIGNATORY sign in dtsignatory)
                        {
                            signatoryindex = signatoryindex + 1;
                            string f_path = "";
                            if (sign.signaturepath != null)
                            {
                                if (sign.signaturepath != "")
                                {
                                    f_path = APPURL + "/" + Logo_Path + "/" + sign.signaturepath;
                                    f_path = Regex.Replace(f_path, @"(?<!https:)(?<!http:)/{2,}", "/");


                                }
                            }
                            if (f_path != "")
                            {
                                signatorytext += @"<div class=""signature signature-" + signatoryindex + @""">
    <img src='" + APPURL + "/" + Logo_Path + "/" + sign.signaturepath +
                   @"' style='width: 130px; height: 50px;' />
    <p>" + sign.name + @"</p>
    <p>" + sign.designation + @"</p>
</div>";

                            }
                            else
                            {
                                signatorytext = signatorytext + @" <div class=""signature signature-@" + signatoryindex + @""">
                        <p>" + sign.name + @"</p>
                        <p>" + sign.designation + @"</p>
                    </div>";
                            }

                        }
                    }
                }
            }

          

            certificateHtml = certificateHtml.Replace("certificate.png", APPURL + "/" + ct.certificate_bg_path);
            certificateHtml = certificateHtml.Replace("style.css", APPURL + "/css/certificate_style.css");
            if(certificatedata != null)
            {
                certificateHtml = certificateHtml.Replace("##PrintDate##",Convert.ToDateTime(certificatedata.Certificate_Info.certificate_dt).ToString("dd-MM-yyyy"));
            }
            else
            {
                certificateHtml = certificateHtml.Replace("##PrintDate##", "");
            }
            File.AppendAllText(path, "Step 9 completed");

            certificateHtml = certificateHtml.Replace("##certtext##", f_cert_text);
            certificateHtml = certificateHtml.Replace("##Sinatory##", signatorytext);
            certificateHtml = certificateHtml.Replace("QRstr", QRstr);


            return certificateHtml;
        }

        public bool IS_Certificate_Grade_Required()
        {

            Certificate ct = new Certificate();
            ct = CommonDB.Get_Certificate_Configuration();
            if(ct.certificate_text.Contains("#grade#"))
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        public List<Participant> Get_Trg_Participant_List(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn = null, string searchvalue = null, string sortcolumn = null, string sortvalue = null, string filtername = null, string filtervalue = null,int pageno = 1, int pagesize = -1, int is_certificate_generated=2)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            List<Participant> trgdata = new List<Participant>();
            ParticipantDB tdb = new ParticipantDB(_configuration);
            trgdata = tdb.Get_Trg_Participant_List(trainingid,participantid,branchid,searchcolumn,searchvalue,sortcolumn,sortvalue,filtername,filtervalue, pageno,pagesize,is_certificate_generated);


            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return trgdata;
        }

        public Boolean Update_Training_Status(string trainingid, int trainingstatus, string reason, string createdby, string branchid)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
         
            TrainingDB tdb = new TrainingDB(_configuration);
            bool issaved = tdb.Update_Training_Status(trainingid,trainingstatus,reason,createdby,branchid);
  

            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return issaved;
        }

        public Boolean Update_Bulk_Participant_Status(string participantid, string trainingid, string branchid, string currentstatus, string updatedstatus, string createdbyempid)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);

            TrainingDB tdb = new TrainingDB(_configuration);
            bool issaved = tdb.Update_Bulk_Trg_Participant_Status(participantid,trainingid,branchid,currentstatus,updatedstatus,createdbyempid);


            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return issaved;
        }

        public Certificate_Details Get_Certificate_Details(string ttpai_id)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar" + System.DateTime.Now);
            Certificate_Details c = new Certificate_Details();
            TrainingDB tdb = new TrainingDB(_configuration);
            c = tdb.Get_Certificate_details(ttpai_id);


            //File.AppendAllText(HostingEnvironment.MapPath("~/Log/Log.txt"), "Within Get_VW_Training_calendar-Return Data" + System.DateTime.Now);

            return c;
        }

        //public string Calculate_Certificate_grade_old(string trainingid,string participantid)
        //{
        //    //TrainingDB WDB = new TrainingDB(_configuration);
        //    //Training trgdetail = new Training();
        //    //trgdetail = WDB.Get_Particular_Training_Detail(trainingid);


        //    string Grade = "D";
        //    List<Learning_Report_Data> ld = new List<Learning_Report_Data>();
        //    SupportBL SBL = new SupportBL(_configuration);
        //    ld = SBL.Learning_Report_Data(trainingid, participantid, null, null, 2);
        //    if (ld.Count > 0)
        //    {
        //        if (ld.FirstOrDefault().learningtime != null)
        //        {

        //            if (ld.FirstOrDefault().learningtime > 0)
        //            {
        //                decimal totalmin = ld.FirstOrDefault().learningtime / 60;
        //                decimal totalhours = ld.FirstOrDefault().learningtime / 3600;
        //                if (totalhours >= 20)
        //                {
        //                    Grade = "A";

        //                }
        //                else if (totalhours >= 10 && totalhours < 20)
        //                {
        //                    Grade = "B";

        //                }
        //                else if (totalhours >= 2 && totalhours < 10)
        //                {
        //                    Grade = "C";
        //                }
        //                else if (totalmin > 59 && totalhours < 2)
        //                {
        //                    Grade = "D";
        //                }
        //                else if(totalmin <= 59)
        //                {
        //                    Grade = "";
        //                }


        //            }
        //            else
        //            {
        //                Grade = "";
        //            }
        //        }
        //        else
        //        {
        //            Grade = "";
        //        }
        //    }
        //    else
        //    {
        //        Grade = "";
        //    }
        //    return Grade;
        //}


        public string Calculate_Certificate_grade_old(string trainingid, string participantid)
        {
            string Grade = "";
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            SessionDB sdb = new SessionDB(_configuration);
            List<SessionCompletionStatus> scs = new List<SessionCompletionStatus>();
            List<certificate_percentage> cpl = new List<certificate_percentage>();
            if(trgdetail.trg_Setting !=null)
            {
                if(trgdetail.trg_Setting.certificate_setting != null)
                {
                    if(trgdetail.trg_Setting.certificate_setting.certificate_percentage != null)
                    {
                        cpl = trgdetail.trg_Setting.certificate_setting.certificate_percentage.ToList();
                    }
                }
            }
            if(cpl.Count <=0)
            {
                TrainingSettings TS = new TrainingSettings();
                string Foldername = CommonEnum.GET_JSON_FOLDER();
                string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "TrainingSettings.json"));
                TS = JsonConvert.DeserializeObject<TrainingSettings>(jsontxt);
                cpl = TS.certificate_setting.certificate_percentage.ToList();

            }
            scs = sdb.Get_Session_Status_vr1(trainingid, null, participantid, null, null, null);

            Decimal totalcompletionPercentage = Math.Round((scs.Sum(o => o.percentcomplete) / scs.Count()),2);
            if(cpl.Where(o => totalcompletionPercentage >= o.from && totalcompletionPercentage <= o.to).ToList().Count > 0)
            {
                Grade = cpl.Where(o => totalcompletionPercentage >= o.from && totalcompletionPercentage <= o.to).ToList().FirstOrDefault().grade;
            }
           
            return Grade;
        }

        public string Calculate_Certificate_grade(string trainingid, string participantid)
        {
            string Grade = "";
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            //trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            SessionDB sdb = new SessionDB(_configuration);
         
            List<certificate_percentage> cpl = new List<certificate_percentage>();
           // if (trgdetail.trg_Setting != null)
           // {
           //     if (trgdetail.trg_Setting.certificate_setting != null)
           //     {
           //         if (trgdetail.trg_Setting.certificate_setting.certificate_percentage != null)
           //         {
           //             cpl = trgdetail.trg_Setting.certificate_setting.certificate_percentage.ToList();
           //         }
           //     }
           // }

           //if (cpl.Count <= 0)
            {
                TrainingSettings TS = new TrainingSettings();
                string Foldername = CommonEnum.GET_JSON_FOLDER();
                string jsontxt = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "TrainingSettings.json"));
                TS = JsonConvert.DeserializeObject<TrainingSettings>(jsontxt);
                cpl = TS.certificate_setting.certificate_percentage.ToList();

            }



        
            List<Learning_Report_Data> ld = new List<Learning_Report_Data>();
            SupportBL SBL = new SupportBL(_configuration);
            ld = SBL.Learning_Report_Data(trainingid, participantid, null, null, 2);
            decimal trg_session_total_learning_time = 0;
            decimal participant_total_learning_time = 0;
            //Calculate total session duration
           
            List<Session> sl = sdb.Get_Session_Data_By_Trg(trainingid);
            sl = sl.Where(o => o.ttttt_status != ((int)CommonEnum.Session_Status.Delete).ToString()).ToList();
            List<Session> dissession = new List<Session>();
            //Remove Duplicate
            foreach (Session s in sl)
            {
                if (dissession.Where(o => o.ttttt_session_id.ToString().ToUpper() == s.ttttt_session_id.ToString().ToUpper()).Count() == 0)
                {
                    dissession.Add(s);
                }
            }

            trg_session_total_learning_time= dissession.Where(o => o.ttttt_type != (int)CommonEnum.SessionType.Break && o.ttttt_type != (int)CommonEnum.SessionType.Test && o.ttttt_type != (int)CommonEnum.SessionType.Assignment).Sum(o => Convert.ToDecimal(o.ttttt_session_duration))*60;



            if (ld.Count > 0)
            {
                if (ld.FirstOrDefault().learningtime != null)
                {
                    participant_total_learning_time = ld.FirstOrDefault().learningtime;
                }
                
            }
        

            Decimal totalcompletionPercentage = Math.Round(participant_total_learning_time/ trg_session_total_learning_time, 2) *100;
            if (cpl.Where(o => totalcompletionPercentage >= o.from && totalcompletionPercentage <= o.to).ToList().Count > 0)
            {
                Grade = cpl.Where(o => totalcompletionPercentage >= o.from && totalcompletionPercentage <= o.to).ToList().FirstOrDefault().grade;
            }

            return Grade;
        }


        public Boolean Update_Trg_rating_data()
        {
           
            bool issaved =false;
            TrainingDB tdb = new TrainingDB(_configuration);
            issaved = tdb.Update_Training_Rating_Data();
            return issaved;
        }

        public bool Update_Certificate_Signatory(string trainingid, string signatoryid, string loginuserid)
        {
            bool issaved = false;
            TrainingDB tdb = new TrainingDB(_configuration);
            issaved = tdb.Update_Certificate_Signatory(trainingid, signatoryid, loginuserid);
            return issaved;
        }

        public bool is_participant_eligible_for_certificate(string ttpai_id, string trainingid)
        {

            return true;
        }
        public Boolean Update_certificate_status(string trainingid, string Loginuserid, cert_status_list cert_status)
        {

            bool issaved = false;
            TrainingDB tdb = new TrainingDB(_configuration);
            issaved = tdb.Update_Certificate_status_Data(trainingid, Loginuserid,cert_status);
            return issaved;
        }

        public List<certificate_status> Get_certificate_status(string trainingid)
        {
            List<certificate_status> cs=new List<certificate_status>();
            TrainingDB tdb = new TrainingDB(_configuration);
            cs = tdb.Get_certificate_status(trainingid);
            return cs;
        }
        public int Get_Participant_Certificates(string agencyid)
        {
            int certificates = 0;
            TrainingDB tdb = new TrainingDB(_configuration);
            certificates = tdb.Get_Participant_Certificates(agencyid);
            return certificates;
        }
        public List<usertrainings> Get_participant_Trainings(string participantid)
        {
            List<usertrainings> cs = new List<usertrainings>();
            TrainingDB tdb = new TrainingDB(_configuration);
            cs = tdb.Get_participants_Training(participantid);
            return cs;
        }

        public List<Session> Get_Trg_Progress_Data(string trainingid,string sessionid, string loginuserid,string loginusertype, int status, string branchid = null, int pageno = 0, int pagesize = 0, string searchcolumn = null, string searchvalue = null)
        {
            List<Session> cs = new List<Session>();
            TrainingDB tdb = new TrainingDB(_configuration);
            cs = tdb.Get_Trg_Progress_Data(trainingid,sessionid, loginuserid, loginusertype, status,branchid, pageno, pagesize,0,null,searchcolumn,searchvalue);
            return cs;
        }
    }
}
