using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System.Collections;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Text;
using static LitteraCore.Common.CommonEnum;

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
                string parentid = CL.Where(o => o.TrainingCategoryId == categoryid).FirstOrDefault().parentcategoryid;
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
            loginbranchdetail = abl.Get_Agency(null, branchid,null, param,null).Items.FirstOrDefault();
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

    }
}
