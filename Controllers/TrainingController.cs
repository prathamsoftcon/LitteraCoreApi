using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.EmailService;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text;
using static LitteraCore.Common.CommonEnum;
using Google.Apis.Logging;

namespace LitteraCore.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public TrainingController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/Categories")]
        public IActionResult GetCategories(string categoryid)
        {
            TrgBL CBL = new TrgBL(_configuration);
            List<TrainingCategory> AL = new List<TrainingCategory>();
            AL = CBL.Get_Training_Category(categoryid);
            return Ok(AL);
        }
        [HttpGet]
        [Route("api/Trainings")]
        public IActionResult GetTrainings(DateTime fromdate, DateTime todate)
        {
            List<Training> T=new List<Training>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_VW_Training_calendar(fromdate, todate);
            return Ok(T);
        }
        [HttpGet]
        [Route("api/Training_Day_Week")]
        public IActionResult Training_Day_Week(string fromdate, string todate)
        {
            List<TRG_DAY_WEEK> T = new List<TRG_DAY_WEEK>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_Day_Week_Count(fromdate, todate);
            return Ok(T);
        }

        [HttpGet]
        [Route("api/Training_Details")]
        public IActionResult Training_Details (string trainingid,string? usertype=null,string? loginuserid=null,string? branchid=null)
        {
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);


            if (trgdetail.TrainingStatus == "4")
            {
                trgdetail.is_reg_open = false;
            }
            else
            {
                if (trgdetail.T_EndDate >= System.DateTime.Now)
                {
                    trgdetail.is_reg_open = true;
                }
                else
                {
                    trgdetail.is_reg_open = false;
                }
            }



            //************Get Completion Percentage
            if(branchid != null)
            {
                List<SessionCompletionStatus> trg_session_status = new List<SessionCompletionStatus>();
                SessionDB sdb = new SessionDB(_configuration);
                trg_session_status = sdb.Get_Session_Status_vr1(trainingid, usertype, loginuserid, trgdetail.StartDate?.ToString("yyyy-MM-dd"), trgdetail.T_EndDate?.ToString("yyyy-MM-dd"), branchid);
                decimal completion = 0;
                List<SessionCompletionStatus> trg_status = new List<SessionCompletionStatus>();
                trg_status = trg_session_status.Where(o => o.tttttm_training_id.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();
                if (trg_status.Count() > 0)
                {
                    completion = Math.Round(trg_status.Sum(o => o.percentcomplete) / trg_status.Count(), 2);
                }
                trgdetail.trg_completionpercentage = completion;
            }
         


          

            return Ok(trgdetail);
        }
        [HttpGet]
        [Route("api/Training_Details_by_Code")]
        public IActionResult Training_Details_by_Code(string trainingcode)
        {
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail_By_Code(trainingcode);


            if (trgdetail.TrainingStatus == "4")
            {
                trgdetail.is_reg_open = false;
            }
            else
            {
                if (trgdetail.T_EndDate >= System.DateTime.Now)
                {
                    trgdetail.is_reg_open = true;
                }
                else
                {
                    trgdetail.is_reg_open = false;
                }
            }

            return Ok(trgdetail);
        }
        [HttpGet]
        [Route("api/TrainingStatus")]
        public IActionResult TrainingStatus()
        {

            List<trgStatus> lu = new List<trgStatus>();
            foreach (int i in Enum.GetValues(typeof(Common.CommonEnum.TrainingStatus)))
            {
                trgStatus u = new trgStatus();
                u.id = i.ToString();
                u.name = Enum.GetName(typeof(Common.CommonEnum.TrainingStatus), i);
                lu.Add(u);


            }
            return Ok(lu);
        }

        [HttpPost]
        [Route("api/TrainingProgressReport")]
        public IActionResult TrainingProgressReport(string trainingid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias,string sessionid = null, string participantid = null,string branchid=null)
        {

            SessionDB sdb = new SessionDB(_configuration);
            List<Session> sl = sdb.Get_Trg_Progress_Data(trainingid, participantid, branchid);

            int is_all_completed = 0;
            if (sl.Where(o => o.noofcompletion == 0).Count() <= 0)
            {
                is_all_completed = 1;
            }
            else
            {
                is_all_completed = 0;
            }

            sl = sl.Where(o => o.ttttt_type != 6 && o.ttttt_type != 7).ToList();


            if (sessionid != null)
            {
                sl = sl.Where(o => o.ttttt_session_id.ToString().ToUpper() == sessionid.ToString().ToUpper()).ToList();
            }
            List<Session> distsession = new List<Session>();
            List<string> sessionids = new List<string>();
            foreach (Session s in sl)
            {
                if (sessionids.Contains(s.ttttt_session_id))
                {
                    continue;
                }
                distsession.Add(s);
                sessionids.Add(s.ttttt_session_id);

            }
            sl = distsession;

            int totalitems = sl.Count();
            //**********Implement Search
            var searchService = new SearchService();
            var filteredItems = sl;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(sl, searchCriterias.SearchCriteria.ToList());
            }

            sl = filteredItems;



            //******************Order 
            TrainingDB WDB = new TrainingDB(_configuration);
            Training trgdetail = new Training();
            trgdetail = WDB.Get_Particular_Training_Detail(trainingid);
            if (trgdetail.trg_Setting != null)
            {
                if (trgdetail.trg_Setting.Session != null)
                {
                    sl = CommonEnum.OrderSessionData(trgdetail.trg_Setting.Session.SessionOrder, sl);
                }
            }
          
                var pagedList = Paging.GetPagedList(param, sl);
            var result = Paging.GetPagedData(param, sl);
            //*********

            return Ok(new {result= result ,is_all_completed= is_all_completed });
        }

        //[HttpGet]
        //[Route("api/Generate_Certificate")]
        //public IActionResult Generate_Certificate(string trainingid, string participantid, string branchid,string APPURL,string Logo_Path)
        //{

        //   TrgBL tbl=new TrgBL(_configuration);
        //    string certificatedata = tbl.GET_TRG_CERTIFICATE(trainingid, participantid, branchid, APPURL, Logo_Path);
           
        //    return Ok(certificatedata);
        //}
        [HttpGet]
        [Route("api/Check_Signatory_Available")]
        public IActionResult Check_Signatory_Available(string trainingid)
        {
            bool is_signatory_exist = false;
            TrgBL tbl = new TrgBL(_configuration);
            TrainingDB tb = new TrainingDB(_configuration);
            Training training = new Training();
            training = tb.Get_Particular_Training_Detail(trainingid);

            if(training.trg_Setting != null)
            {
                if(training.trg_Setting.certificate_setting != null)
                {
                    if (training.trg_Setting.certificate_setting.no_of_signatory_required > 0)
                    {
                        List<CERTIFICATE_SIGNATORY> signatory = tbl.Get_Signatory(trainingid);
                        if (signatory.Count > 0)
                        {
                            is_signatory_exist = true;
                        }
                        else
                        {
                            is_signatory_exist = false;
                        }

                    }
                    else
                    {
                        is_signatory_exist = true;
                    }
                }
                else
                {
                    List<CERTIFICATE_SIGNATORY> signatory = tbl.Get_Signatory(trainingid);
                    if (signatory.Count > 0)
                    {
                        is_signatory_exist = true;
                    }
                    else
                    {
                        is_signatory_exist = false;
                    }

                }
            }
            else
            {
                List<CERTIFICATE_SIGNATORY> signatory = tbl.Get_Signatory(trainingid);
                if (signatory.Count > 0)
                {
                    is_signatory_exist = true;
                }
                else
                {
                    is_signatory_exist = false;
                }

            }



            return Ok(is_signatory_exist);
        }
        [HttpGet]
        [Route("api/Get_Certificate_Signatory")]
        public IActionResult Get_Certificate_Signatory(string trainingid)
        {

            TrgBL tbl = new TrgBL(_configuration);
            List<CERTIFICATE_SIGNATORY> signatory = tbl.Get_Signatory(trainingid);
            return Ok(signatory);

        }
        [HttpPost]
        [Route("api/TRG_PARTICIPANT_MAP")]
        public IActionResult TRG_PARTICIPANT_MAP([FromBody]  TRGMAPPING trgmapping)
        {
            bool issaved = false;
            TrgBL TBD = new TrgBL(_configuration);
            issaved = TBD.Save_Trg_Participant_Mapping(trgmapping);
            return Ok(issaved);
        }

        [HttpGet]
        [Route("api/TRG_SPONSOR")]
        public IActionResult TRG_SPONSOR(string trainingid)
        {
            List<Agency> EH = new List<Agency>();
            TrainingDB TBD = new TrainingDB(_configuration);
            EH = TBD.Get_Trg_Sponsor(trainingid);
            return Ok(EH);
        }

        [HttpGet]
        [Route("api/TRG_PARTICIPANT_DETAILS")]
        public IActionResult TRG_PARTICIPANT_DETAILS(string trainingid,string participantid=null,string branchid=null)
        {
           
            List<Participant> PL = new List<Participant>();
            ParticipantDB PDB = new ParticipantDB(_configuration);
            //List of training all participant
            PL = PDB.Get_TRG_PARTICIPANT_Data(trainingid, participantid, branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            return Ok(PL);
        }

        [HttpGet]
        [Route("api/Trg_Type")]
        public IActionResult Trg_Type()
        {
            List<Trg_Type> T = new List<Trg_Type>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_Trg_Type();
            return Ok(T);
        }
        [HttpGet]
        [Route("api/Trg_Title")]
        public IActionResult Trg_Title()
        {
            List<Trg_Title> T = new List<Trg_Title>();
            TrgBL CBL = new TrgBL(_configuration);
            T = CBL.Get_Trg_Title();
            return Ok(T);
        }


        [HttpGet]
        [Route("api/Generate_Certificate")]
        public IActionResult Generate_Certificate(string trainingid, string participantid, string branchid, string APPURL, string Logo_Path,string? loginuserid=null)
        {

            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            ParticipantDB pdb=new ParticipantDB(_configuration);
            List<Participant> p = new List<Participant>();
            string ttpai_id = "";
            p = pdb.Get_Trg_Participant_List(trainingid, participantid,null,null,null,null,null,null,null,0,0,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            if (p.Count > 0)
            {
                ttpai_id = p.FirstOrDefault().ttpai_id;
            }

            string certificateid = Guid.NewGuid().ToString();

            certificate_obj c = new certificate_obj
            {
                CertId = certificateid,
                ttpai_id= ttpai_id,
                CertInfo= "id="+ certificateid + ",date="+System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+",createdby="+ loginuserid + ""
            };

            string certificatedata = "";
            List <certificate_obj> lc=new List<certificate_obj>();
            lc.Add(c);
            if (pdb.Update_Participant_certificate_info(lc.ToArray(), trainingid) == true)
            {
                 certificatedata = tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, participantid, APPURL, Logo_Path, certificateid);
            }



            return Ok(certificatedata);
        }

        [HttpGet]
        [Route("api/Generate_Certificate_New")]
        public IActionResult Generate_Certificate_New(string trainingid, string participantid, string branchid, string APPURL, string Logo_Path, string? loginuserid = null)
        {

            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            ParticipantDB pdb = new ParticipantDB(_configuration);
            List<Participant> p = new List<Participant>();
            string ttpai_id = "";
            p = pdb.Get_Trg_Participant_List(trainingid, participantid,null,null,null,null,null,null,null,0,0,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            if (p.Count > 0)
            {
                ttpai_id = p.FirstOrDefault().ttpai_id;
            }

            string certificateid = Guid.NewGuid().ToString();

            certificate_obj c = new certificate_obj();

            if (tb.IS_Certificate_Grade_Required() == true)
            {

             
                Certificate_Details cd = new Certificate_Details();
                cd = tb.Get_Certificate_Details(ttpai_id);
                string grade = tb.Calculate_Certificate_grade(trainingid, participantid);
                if (grade == "" || grade == null)
                {
                    return NotFound("आपकी अध्ययन अवधि सर्टिफिकेट प्राप्त करने के लिए अभी पर्याप्त नहीं है। कृपया कोर्स कंटेंट का अध्ययन करें और कोर्स में दी सभी प्रेक्टिकल गतिविधियों को करें। जब निर्धारित अध्ययन अवधि पूर्ण हो जाएगी, तब आप सर्टिफिकेट जनरेट कर सकेंगे और अपना ग्रेड देख सकेंगे।\r\nकोर्स कंटेंट Link - https://learningplatform.mpbou.in/view_more_content1.html");
                }

                    c = new certificate_obj
                {
                    CertId = certificateid,
                    ttpai_id = ttpai_id,
                    CertInfo = "id=" + certificateid + ",date=" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",createdby=" + loginuserid + ",grade="+ grade + ""
                    };
            }
            else
            {
                c = new certificate_obj
                {
                    CertId = certificateid,
                    ttpai_id = ttpai_id,
                    CertInfo = "id=" + certificateid + ",date=" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",createdby=" + loginuserid + ""
                };
            }


           
            List<certificate_obj> lc = new List<certificate_obj>();
            lc.Add(c);

            string certificatedata = "";
          
            if (pdb.Update_Participant_certificate_info(lc.ToArray(), trainingid) == true)
            {
                certificatedata = tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, participantid, APPURL, Logo_Path, certificateid);
            }



            return Ok(certificatedata);
        }



        [HttpGet]
        [Route("api/Generate_ALL_Certificate")]
        public IActionResult Generate_ALL_Certificate(string trainingid, string branchid, string APPURL, string Logo_Path, string? loginuserid = null)
        {
            Task.Run(async () =>
            {
                bool isgenerated = false;
                TrgBL tb = new TrgBL(_configuration);
                TrainingDB tbl = new TrainingDB(_configuration);
                Training Trg = tbl.Get_Particular_Training_Detail(trainingid);
                List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);

                ParticipantDB pdb = new ParticipantDB(_configuration);
                List<Participant> p = pdb.Get_Trg_Participant_List(trainingid, null, branchid,null,null,null,null,null,null,0,0,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
              

                List<not_eligible_participant> not_eligible = new List<not_eligible_participant>();
                List<certificate_obj> lc = new List<certificate_obj>();

                foreach (Participant part in p)
                {
                    string certificateid = Guid.NewGuid().ToString();
                    bool iseligible = tb.is_participant_eligible_for_certificate(part.ttpai_id, trainingid);
                    if (iseligible)
                    {
                        lc.Add(new certificate_obj
                        {
                            CertId = certificateid,
                            ttpai_id = part.ttpai_id,
                            CertInfo = "id=" + certificateid + ",date=" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",createdby=" + loginuserid
                        });
                    }
                    else
                    {
                        not_eligible.Add(new not_eligible_participant
                        {
                             participantid=part.ParticipantId,
                             name=part.ParticipantName,
                             emilid=part.email,
                             mobileno=part.mobileno
                               
                        });
                    }
                }

                if (lc.Count > 0)
                {
                    if (pdb.Update_Participant_certificate_info(lc.ToArray(), trainingid))
                    {
                        isgenerated = true;
                    }
                }

                // Send Email Notification
                AgencyDB adb = new AgencyDB(_configuration);
                List<Agency> a = new List<Agency>();
                a = adb.Get_Agency(null, loginuserid,1,1, null, null, null, null, null, "AgencyId,tyaam_status,AgencyName,HAgencyName,ag_email,ag_mobileno,totalrecords");
                string mailid = a.FirstOrDefault().ag_email;
                if (a.Count > 0) {
                    if (not_eligible.Count > 0)
                    {
                        //string notEligibleJson = JsonConvert.SerializeObject(not_eligible);
                        SmtpEmailService s = new SmtpEmailService(_configuration);
                        //await s.SendEmailAsync(mailid, "Certificate Generated","Your process to generate certificate is completed successfully. These participants are not eligible:\n" + notEligibleJson);

                        string json = JsonConvert.SerializeObject(not_eligible, Formatting.Indented);
                        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                        var jsonStream = new MemoryStream(jsonBytes);

                        var attachments = new List<(string FileName, Stream Content)>
                            {
                            ("NotEligibleParticipants.json", jsonStream)
                            };

                        await s.SendEmailAsync_with_attachment(mailid, "Certificate Generated",
                            "Certificate generation completed. Please find the not-eligible participant list attached.",
                            attachments);
                    }
                    else
                    {
                        SmtpEmailService s = new SmtpEmailService(_configuration);
                        await s.SendEmailAsync(mailid, "Certificate Generated", "Your Process to generate certificate is completed successfully.");
                    }
                   
                }
              
            

                // Optionally log or store results (isgenerated, not_eligible) to DB or logs

            });

            return Ok(new {status=true, message = "Certificate generation started in background.We will inform you with your mail id on process completion." });
        }




        [HttpGet]
        [Route("api/Generate_And_Download_ALL_Certificate")]
        public IActionResult Generate_And_Download_ALL_Certificate(string trainingid, string branchid, string APPURL, string Logo_Path, string? loginuserid = null)
        {
            Task.Run(async () =>
            {
                bool isgenerated = false;
                TrgBL tb = new TrgBL(_configuration);
                TrainingDB tbl = new TrainingDB(_configuration);
                Training Trg = tbl.Get_Particular_Training_Detail(trainingid);
                List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);

                ParticipantDB pdb = new ParticipantDB(_configuration);
                List<Participant> p = pdb.Get_Trg_Participant_List(trainingid, null, branchid,null,null,null,null,null,null,0,0,2, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
             

                List<not_eligible_participant> not_eligible = new List<not_eligible_participant>();
                List<certificate_obj> lc = new List<certificate_obj>();

                foreach (Participant part in p)
                {
                    string certificateid = Guid.NewGuid().ToString();
                    bool iseligible = tb.is_participant_eligible_for_certificate(part.ttpai_id, trainingid);
                    if (iseligible)
                    {
                        lc.Add(new certificate_obj
                        {
                            participantid=part.ttpai_id,
                            CertId = certificateid,
                            ttpai_id = part.ttpai_id,
                            CertInfo = "id=" + certificateid + ",date=" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",createdby=" + loginuserid
                        });
                    }
                    else
                    {
                        not_eligible.Add(new not_eligible_participant
                        {
                            participantid = part.ParticipantId,
                            name = part.ParticipantName,
                            emilid = part.email,
                            mobileno = part.mobileno

                        });
                    }
                }
                string certificatedata = "";
                if (lc.Count > 0)
                {
                    if (pdb.Update_Participant_certificate_info(lc.ToArray(), trainingid))
                    {
                        foreach(certificate_obj co in lc)
                        {
                            certificatedata = certificatedata + tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, co.participantid, APPURL, Logo_Path, co.CertId);
                        }
                        //certificatedata = certificatedata + tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, participantid, APPURL, Logo_Path, certificateid);
                    }
                }

                // Send Email Notification
                AgencyDB adb = new AgencyDB(_configuration);
                List<Agency> a = new List<Agency>();
                a = adb.Get_Agency(null, loginuserid, 1, 1, null, null, null, null, null, "AgencyId,tyaam_status,AgencyName,HAgencyName,ag_email,ag_mobileno,totalrecords");
                string mailid = a.FirstOrDefault().ag_email;
                if (a.Count > 0)
                {
                    if (not_eligible.Count > 0)
                    {
                        string notEligibleJson = JsonConvert.SerializeObject(not_eligible);
                        SmtpEmailService s = new SmtpEmailService(_configuration);
                        //await s.SendEmailAsync(mailid, "Certificate Generated","Your process to generate certificate is completed successfully. These participants are not eligible:\n" + notEligibleJson);

                        byte[] jsonBytes = Encoding.UTF8.GetBytes(notEligibleJson);
                        var jsonStream = new MemoryStream(jsonBytes);

                        // Attach certificate HTML
                        byte[] certBytes = Encoding.UTF8.GetBytes(certificatedata);
                        var certStream = new MemoryStream(certBytes);

                        var attachments = new List<(string FileName, Stream Content)>
                            {
                                ("NotEligibleParticipants.json", jsonStream),
                                ("GeneratedCertificates.html", certStream)
                            };
                        await s.SendEmailAsync_with_attachment(mailid, "Certificate Generated",
                            "Certificate generation completed. Please find the not-eligible participant list attached.",
                            attachments);
                    }
                    else
                    {
                        SmtpEmailService s = new SmtpEmailService(_configuration);
                     
                        // Attach certificate HTML
                        byte[] certBytes = Encoding.UTF8.GetBytes(certificatedata);
                        var certStream = new MemoryStream(certBytes);

                        var attachments = new List<(string FileName, Stream Content)>
                        {
                         
                            ("GeneratedCertificates.html", certStream)
                        };
                        await s.SendEmailAsync(mailid, "Certificate Generated", "Your Process to generate certificate is completed successfully.");
                    }

                }



                // Optionally log or store results (isgenerated, not_eligible) to DB or logs

            });

            return Ok(new { message = "Certificate generation started in background." });
        }



        [HttpGet]
        [Route("api/Download_All_Certificate")]
        public IActionResult Download_All_Certificate(string trainingid, string branchid, string APPURL, string Logo_Path, string? loginuserid = null)
        {
            Task.Run(async () =>
            {
                bool isgenerated = false;
                TrgBL tb = new TrgBL(_configuration);
                TrainingDB tbl = new TrainingDB(_configuration);
                Training Trg = tbl.Get_Particular_Training_Detail(trainingid);
                List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);

                ParticipantDB pdb = new ParticipantDB(_configuration);
                List<Participant> p = pdb.Get_Trg_Participant_List(trainingid, null, branchid);


                List<not_eligible_participant> not_eligible = new List<not_eligible_participant>();
                List<certificate_obj> lc = new List<certificate_obj>();
                string certificatedata = "";
                foreach (Participant part in p)
                {
                    string certificateid = Guid.NewGuid().ToString();
                    if(part.ttpai_trg_cert_id != null)
                    {
                        if(Convert.ToString(part.ttpai_trg_cert_id) != "")
                        {
                            certificatedata = certificatedata + tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, part.ParticipantId, APPURL, Logo_Path, part.ttpai_trg_cert_id);
                        }
                       
                    }
                   
                }
             

                // Send Email Notification
                AgencyDB adb = new AgencyDB(_configuration);
                List<Agency> a = new List<Agency>();
                a = adb.Get_Agency(null, loginuserid, 1, 1, null, null, null, null, null, "AgencyId,tyaam_status,AgencyName,HAgencyName,ag_email,ag_mobileno,totalrecords");
                string mailid = a.FirstOrDefault().ag_email;
                if (a.Count > 0)
                {
                    SmtpEmailService s = new SmtpEmailService(_configuration);

                    // Attach certificate HTML
                    byte[] certBytes = Encoding.UTF8.GetBytes(certificatedata);
                    var certStream = new MemoryStream(certBytes);

                    var attachments = new List<(string FileName, Stream Content)>
                        {

                            ("GeneratedCertificates.html", certStream)
                        };
                    await s.SendEmailAsync(mailid, "Certificate Generated", "Your Process to generate certificate is completed successfully.");
                }



                // Optionally log or store results (isgenerated, not_eligible) to DB or logs

            });

            return Ok(new { status = true, message = "Certificate generation started in background.We will inform you with your mail id on process completion." });
        }

        //public async Task<IActionResult> Generate_ALL_Certificate(string trainingid, string branchid, string APPURL, string Logo_Path, string? loginuserid = null)
        //{
        //    bool isgenerated = false;
        //    TrgBL tb = new TrgBL(_configuration);
        //    TrainingDB tbl = new TrainingDB(_configuration);

        //    Training Trg = new Training();
        //    List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
        //    Trg = tbl.Get_Particular_Training_Detail(trainingid);

        //    ParticipantDB pdb = new ParticipantDB(_configuration);
        //    List<Participant> p = new List<Participant>();

        //    p = pdb.Get_Trg_Participant_List(trainingid,null,branchid);


        //    string certificateid = Guid.NewGuid().ToString();

        //    List<Participant> not_eligible = new List<Participant>();

        //    string certificatedata = "";
        //    List<certificate_obj> lc = new List<certificate_obj>();
        //    foreach(Participant part in p)
        //    {
        //        //Check Eligibility
        //        bool iseligible = tb.is_participant_eligible_for_certificate(part.ttpai_id, trainingid);
        //        if (iseligible == true)
        //        {
        //            certificate_obj c = new certificate_obj
        //            {
        //                CertId = certificateid,
        //                ttpai_id = part.ttpai_id,
        //                CertInfo = "id=" + certificateid + ",date=" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",createdby=" + loginuserid + ""
        //            };
        //            lc.Add(c);

        //        }
        //        else
        //        {
        //            not_eligible.Add(part);
        //        }

        //    }

        //    if (lc.Count > 0)
        //    {
        //        if (pdb.Update_Participant_certificate_info(lc.ToArray(), trainingid) == true)
        //        {
        //            isgenerated = true;
        //        }
        //    }


        //    //Send mail to login user
        //    SmtpEmailService s = new SmtpEmailService(_configuration);
        //    await s.SendEmailAsync("mailid@gmail.com","Subject","Message");


        //    return Ok(new { isgenerated= isgenerated, not_eligible=not_eligible });
        //}



        [HttpGet]
        [Route("api/Reprint_Certificate")]
        public IActionResult Reprint_Certificate(string trainingid, string participantid, string branchid, string APPURL, string Logo_Path,string certificateid)
        {

            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);
            string certificatedata = tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, participantid, APPURL, Logo_Path, certificateid);

            return Ok(certificatedata);
        }


        [HttpGet]
        [Route("api/QR_Certificate_verification")]
        public IActionResult QR_Certificate_verification(string certificateid)
        {

            certificate_obj certificatedata = new certificate_obj();
            ParticipantDB pdb = new ParticipantDB(_configuration);
            List<certificate_obj> L = pdb.Get_Certificate_info(null, certificateid);
            if (L.Count > 0)
            {
                return Ok(new  { verified=true,certificate= L.FirstOrDefault() });
            }
            else
            {
                return Ok(new { verified = false});
            }

      
        }

        [HttpGet]
        [Route("api/Check_Certificate_Eligibility")]
        public IActionResult Check_Certificate_Eligibility(string ttpai_id,string trainingid)
        {
            TrgBL tbl=new TrgBL(_configuration);
            bool iseligible = tbl.is_participant_eligible_for_certificate(ttpai_id,trainingid);
            return Ok(new { eligible = true });
        }



        [HttpGet]
        [Route("api/Generate_Certificate_All")]
        public IActionResult Generate_Certificate(string trainingid, string branchid, string APPURL, string Logo_Path)
        {

            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            List<Participant> p = new List<Participant>();
            ParticipantDB pdb = new ParticipantDB(_configuration);
            p = pdb.Get_TRG_PARTICIPANT_Data(trainingid,null,null,"ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            string certificatedata = "";
            foreach (Participant pr in p)
            {
                certificatedata= certificatedata+tb.Geenerate_certificate_text(Trg, dtsignatory, pr.ParticipantId, APPURL, Logo_Path);
            }

          
            return Ok(certificatedata);
        }


        [HttpPost]
        [Route("api/Get_Trg_Participant_List")]
        public IActionResult Get_Trg_Participant_List(string trainingid = null, string participantid = null, string branchid = null, string searchcolumn = null, string searchvalue = null, string sortcolumn = null, string sortvalue = null,string filtername=null,string filtervalue=null, int pageno = 1, int pagesize = -1,int is_certificate_generated=2)
        {
           TrgBL tbl=new TrgBL(_configuration);
            List<Participant> p = new List<Participant>();
            p = tbl.Get_Trg_Participant_List(trainingid, participantid, branchid, searchcolumn, searchvalue, sortcolumn, sortvalue,filtername,filtervalue, pageno, pagesize, is_certificate_generated);
            PaginationParam param= new PaginationParam{ PageNumber = 1, PageSize = pagesize };
            var result = Paging.GetPagedData(param, p);
            if (p.Count > 0)
            {
                result.TotalRecords = p.FirstOrDefault().totalrecords;
                result.TotalPages = (int)Math.Ceiling((double)p.FirstOrDefault().totalrecords / param.PageSize);
            }
            return Ok(result);

        }


        [HttpPost]
        [Route("api/Update_Training_Status")]
        public IActionResult Update_Training_Status(string trainingid, int trainingstatus, string reason, string createdby, string branchid)
        {
            TrgBL tbl = new TrgBL(_configuration);
           
            bool issaved = tbl.Update_Training_Status(trainingid,trainingstatus,reason,createdby,branchid);
         
            return Ok(issaved);

        }

        [HttpPost]
        [Route("api/Update_Bulk_Participant_Status")]
        public IActionResult Update_Bulk_Participant_Status(string trainingid, string branchid, string currentstatus, string updatedstatus, string createdbyempid,string? participantid= null)
        {
            TrgBL tbl = new TrgBL(_configuration);

            bool issaved = tbl.Update_Bulk_Participant_Status(participantid,trainingid,branchid,currentstatus,updatedstatus,createdbyempid);

            return Ok(issaved);

        }

        [HttpPost]
        [Route("api/Update_Multiple_Participant_Status")]
        public IActionResult Update_Multiple_Participant_Status(string trainingid, string branchid, string currentstatus, string updatedstatus, string createdbyempid, string? participantid = null)
        {
            TrgBL tbl = new TrgBL(_configuration);
            if (participantid != null)
            {
                string[] participant=participantid.Split(',');
                foreach(string part in participant)
                {
                    bool issaved = tbl.Update_Bulk_Participant_Status(part, trainingid, branchid, currentstatus, updatedstatus, createdbyempid);
                }
            }
          

            return Ok(true);

        }


        [HttpGet]
        [Route("api/Certificate_Details")]
        public IActionResult Certificate_Details(string ttpai_id)
        {
            TrgBL tbl=new TrgBL(_configuration);
            Certificate_Details c = new Certificate_Details();
            c=tbl.Get_Certificate_Details(ttpai_id);
            string grade= tbl.Calculate_Certificate_grade(c.trainingid, c.participantid);
            if(grade != "" || grade==null)
            {
                c.grade = grade;
            }
            else
            {
                return NotFound("आपकी अध्ययन अवधि सर्टिफिकेट प्राप्त करने के लिए अभी पर्याप्त नहीं है। कृपया कोर्स कंटेंट का अध्ययन करें और कोर्स में दी सभी प्रेक्टिकल गतिविधियों को करें। जब निर्धारित अध्ययन अवधि पूर्ण हो जाएगी, तब आप सर्टिफिकेट जनरेट कर सकेंगे और अपना ग्रेड देख सकेंगे।\r\nकोर्स कंटेंट Link - https://learningplatform.mpbou.in/view_more_content1.html");
            }
            
            
            return Ok(c);
        }


        [HttpGet]
        [Route("api/Verify_Certificate")]
        public IActionResult Verify_Certificate(string usercode,string trainingid)
        {
            string grade = "";
            TrgBL tbl = new TrgBL(_configuration);
            UserDB udb = new UserDB(_configuration);
            Trg_User_Details tud=new Trg_User_Details();
            tud=udb.Get_Trg_User_Details(usercode, trainingid);
           
            if(tud.agencyid != null)
            {
               grade = tbl.Calculate_Certificate_grade(tud.trainingid, tud.agencyid);
            }
            else
            {
                grade = "";
            }
            

            return Ok(grade);
        }

        [HttpGet]
        [Route("api/Get_User_Agency")]
        public IActionResult Get_User_Agency(string usercode)
        {
            string grade = "";
            string agencyid = "";
            TrgBL tbl = new TrgBL(_configuration);
            UserDB udb = new UserDB(_configuration);
            Trg_User_Details tud = new Trg_User_Details();
            agencyid = udb.Get_User_agency_by_code(usercode);

            return Ok(agencyid);
        }

        [HttpPost]
        [Route("api/update_trg_rating_data")]
        public IActionResult update_trg_rating_data()
        {
            bool isupdated = false;
            TrgBL tbl = new TrgBL(_configuration);
            UserDB udb = new UserDB(_configuration);
            isupdated = tbl.Update_Trg_rating_data();
            return Ok(isupdated);
        }


        [HttpPost]
        [Route("api/update_certificate_signatory")]
        public IActionResult update_certificate_signatory(string trainingid, string signatoryid, string loginuserid)
        {
            bool isupdated = false;
            TrgBL tbl = new TrgBL(_configuration);
            UserDB udb = new UserDB(_configuration);
            isupdated = tbl.Update_Certificate_Signatory(trainingid, signatoryid,loginuserid);
            return Ok(isupdated);
        }


        [HttpPost]
        [Route("api/update_certificate_status")]
        public IActionResult update_certificate_status(string trainingid, string Loginuserid, [FromBody]cert_status_list cert_status)
        {
            bool isupdated = false;
            TrgBL tbl = new TrgBL(_configuration);
            
            isupdated = tbl.Update_certificate_status(trainingid, Loginuserid, cert_status);
            return Ok(isupdated);
        }

        [HttpGet]
        [Route("api/Participants_training")]
        public IActionResult Participants_training(string participantid)
        {
            List<usertrainings> ut = new List<usertrainings>();
            TrgBL tbl = new TrgBL(_configuration);
          
            ut = tbl.Get_participant_Trainings(participantid);

            return Ok(ut);
        }

        [HttpGet]
        [Route("api/Training_Progress_Report_Participantwise")]
        public IActionResult Training_Progress_Report_Participantwise(string trainingid, string loginuserid, string loginusertype,string sessionid=null,int status=2, string branchid = null, int pageno = 0, int pagesize = 0, string searchcolumn = null, string searchvalue=null)
        {
            List<Session> ut = new List<Session>();
            TrgBL tbl = new TrgBL(_configuration);
            if(loginusertype != "5")
            {
               // loginusertype = null;
                loginuserid=null;
            }
            ut = tbl.Get_Trg_Progress_Data(trainingid, sessionid, loginuserid,loginusertype, status, branchid, pageno, pagesize, searchcolumn, searchvalue);

         

            return Ok(ut);
        }


        [HttpGet]
        [Route("api/Generate_Certificate_BR")]
        public IActionResult Generate_Certificate_BR(string trainingid, string participantid, string branchid, string APPURL, string Logo_Path, string? loginuserid = null)
        {
            CommonEnum ce=new CommonEnum();
            string jsostr=ce.GET_BR_CONFIGURATION("Generate_Certificate");
            Generate_Certificate gc=new Generate_Certificate();
            if (jsostr != null) {
                gc = JsonConvert.DeserializeObject<Generate_Certificate>(jsostr);
            }
            
            TrgBL tb = new TrgBL(_configuration);
            TrainingDB tbl = new TrainingDB(_configuration);

            Training Trg = new Training();
            List<CERTIFICATE_SIGNATORY> dtsignatory = tbl.Get_Certificate_signatory(trainingid);
            Trg = tbl.Get_Particular_Training_Detail(trainingid);

            ParticipantDB pdb = new ParticipantDB(_configuration);
            List<Participant> p = new List<Participant>();
            string ttpai_id = "";
            p = pdb.Get_Trg_Participant_List(trainingid, participantid,null,null,null,null,null,null,null,0,0,2, "\"ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve\"");
            if (p.Count > 0)
            {
                ttpai_id = p.FirstOrDefault().ttpai_id;
            }

            string certificateid = Guid.NewGuid().ToString();

            certificate_obj c = new certificate_obj
            {
                CertId = certificateid,
                ttpai_id = ttpai_id,
                CertInfo = "id=" + certificateid + ",date=" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",createdby=" + loginuserid + ""
            };

            string certificatedata = "";
            List<certificate_obj> lc = new List<certificate_obj>();
            lc.Add(c);
            if (pdb.Update_Participant_certificate_info(lc.ToArray(), trainingid) == true)
            {
                certificatedata = tb.Geenerate_certificate_text_with_QR(Trg, dtsignatory, participantid, APPURL, Logo_Path, certificateid);
            }



            return Ok(certificatedata);
        }

    }
}
