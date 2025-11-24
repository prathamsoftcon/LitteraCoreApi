using LitteraCore.BLContext;
using LitteraCore.Common.DMS;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LitteraCore.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ILogger<AgencyController> _logger;

        private readonly IConfiguration _configuration;
        public ParticipantController(IConfiguration configuration, ILogger<AgencyController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/TRG_PARTICIPANT_ACTION")]
        public IActionResult TRG_PARTICIPANT_ACTION(string usertype, string userid, string trainingid, string participantid, string branchid = null)
        {
            Branch_Configuration bc = new Branch_Configuration();
            ApplicationConfigDB adb = new ApplicationConfigDB(_configuration);
            bc = adb.GET_BRANCH_CONFIGURATION();

            string Login_branchtype = "";
            if (branchid != null)
            {
                AgencyBL ABL = new AgencyBL(_configuration);
                List<Agency> AL = new List<Agency>();
                AL = ABL.Get_Agency_Data(null, branchid, 1, 10, null, null);
                AL = AL.Where(o => o.AgencyTypeId == "00001" || o.AgencyTypeId == "00002" || o.AgencyTypeId == "00003" || o.AgencyTypeId == "00004" || o.AgencyTypeId == "00005").ToList();
                if (AL.Count() > 0)
                {
                    Login_branchtype = AL.FirstOrDefault().AgencyTypeId;
                }

            }


            ParticipantDB WDB = new ParticipantDB(_configuration);

            List<Participant> lwtc = new List<Participant>();


            lwtc = WDB.Get_TRG_PARTICIPANT_Data(trainingid, participantid, branchid, "ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve");
            lwtc = lwtc.Where(o => o.ParticipantId.ToString().ToUpper() == participantid.ToString().ToUpper()).ToList();


            TrainingDB WDBT = new TrainingDB(_configuration);
            Training trg = new Training();

            List<Training> TL = new List<Training>();

            trg = WDBT.Get_Particular_Training_Detail(trainingid);


            //Code to get Participant Agency Login Status
            DMSBL DBL = new DMSBL(_configuration);
            int laststatus = 0;
            laststatus = DBL.Get_DMS_DOC_STATUS(participantid, Convert.ToInt32(CommonEnum.DMS_TAT_TYPE_ID.Participant_REGIS));

            List<OptionsDisplay> lu = new List<OptionsDisplay>();
            if (bc.max_level_allowed > 1)
            {
                lu.Add(new OptionsDisplay { id = "12", name = "Change password", dispay = true });
                // lu.Add(new OptionsDisplay { id = "4", name = "Enroll Dates", dispay = false });
                if (usertype == "1" || usertype == "3" || usertype == "4")
                {

                    if (lwtc.FirstOrDefault().is_approve != 1)
                    {
                        if (lwtc.FirstOrDefault().is_approve == (int)Common.CommonEnum.Participant_Enroll_Status.Consent_Received) //Consent Given
                        {
                            if (Login_branchtype == "00001")
                            {
                                if (laststatus == 0 || laststatus == 2)
                                {
                                    lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false });
                                    lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = true });
                                    lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = true });

                                }
                                else
                                {
                                    lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true });
                                    lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false });
                                    lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                                }
                            }
                            else if (Login_branchtype == "00003")
                            {
                                lu.Add(new OptionsDisplay { id = "11", name = "Send for approval", dispay = true });
                                lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false });
                                lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false });
                                lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                            }
                            else
                            {
                                lu.Add(new OptionsDisplay { id = "11", name = "Send for approval", dispay = false });
                                lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false });
                                lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false });
                                lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                            }
                        }
                        else if (lwtc.FirstOrDefault().is_approve == (int)Common.CommonEnum.Participant_Enroll_Status.Sent_For_Approval)
                        {
                            if (Login_branchtype == "00001")
                            {
                                if (laststatus == 0 || laststatus == 2)
                                {
                                    lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false });
                                    lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = true });
                                    lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = true });

                                }
                                else
                                {
                                    lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true });
                                    lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false });
                                    lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                                }
                            }
                        }
                        else
                        {
                            lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false });
                            lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false });
                            lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                            lu.Add(new OptionsDisplay { id = "11", name = "Send for approval", dispay = false });
                        }

                    }
                    else
                    {
                        // in case of already Approve approve not need to show
                        lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false });
                        lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false });
                        lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                        lu.Add(new OptionsDisplay { id = "11", name = "Send for approval", dispay = false });
                    }

                    if (lwtc.FirstOrDefault().is_approve != 9)
                    {
                        lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = true }); //Chage status
                    }
                    else
                    {
                        lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = false }); //Chage status
                    }

                    if (trg.Training_SponsorType == 4)
                    {
                        if (lwtc.FirstOrDefault().is_approve == 0)
                        {
                            lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = true }); //Chage status
                        }
                        else
                        {
                            lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                        }
                        //  lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = false });
                    }
                    else
                    {
                        //  lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = true });
                        lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                    }

                }
                else if (usertype == "2")
                {

                    lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = true }); //Chage status
                                                                                              //  lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                                                                                              // lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = false });
                                                                                              //  lu.Add(new OptionsDisplay { id = "8", name = "Send Mail", dispay = true }); //Add Participant
                                                                                              // lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false }); //Chage status
                                                                                              //lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                    if (lwtc.FirstOrDefault().is_approve == 0 || lwtc.FirstOrDefault().is_approve == 9)
                    {
                        if (laststatus == 0 || laststatus == 2)
                        {
                            lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = true }); //Chage status
                            lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = true });
                        }
                        else
                        {
                            if (Login_branchtype != "")
                            {
                                if (Login_branchtype == "00001")
                                {
                                    if (laststatus == 11)
                                    {
                                        lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status
                                    }
                                }
                                else
                                {
                                    lu.Add(new OptionsDisplay { id = "11", name = "Send for approval", dispay = true }); //Chage status
                                }

                            }
                            else
                            {
                                lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status
                            }
                            //lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status
                        }

                    }

                }
                else
                {
                    //  lu.Add(new OptionsDisplay { id = "1", name = "Remove from training", dispay = false }); //Delete Participant
                    // lu.Add(new OptionsDisplay { id = "2", name = "Move to other training", dispay = false }); //Add Participant
                    lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                                                                                                //lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = false });
                                                                                                //lu.Add(new OptionsDisplay { id = "8", name = "Send Mail", dispay = false }); //Add Participant
                    lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });

                }

            }
            else //Default 
            {
                lu.Add(new OptionsDisplay { id = "4", name = "Enroll Dates", dispay = false });
                lu.Add(new OptionsDisplay { id = "12", name = "Change password", dispay = true });

                if (usertype == "1" || usertype == "3" || usertype == "4")
                {
                    lu.Add(new OptionsDisplay { id = "1", name = "Remove from training", dispay = true }); //Delete Participant
                    lu.Add(new OptionsDisplay { id = "2", name = "Move to other training", dispay = true }); //Add Participant
                    lu.Add(new OptionsDisplay { id = "8", name = "Send Mail", dispay = true }); //Add Participant
                    if (lwtc.FirstOrDefault().is_approve == 0 || lwtc.FirstOrDefault().is_approve == 9)
                    {
                        if (laststatus == 0 || laststatus == 2)
                        {
                            lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = true }); //Chage status
                            lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = true });
                        }
                        else
                        {
                            lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status

                        }

                    }
                    else
                    {
                        lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false }); //Chage status
                        lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false }); //Chage status
                        lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                    }
                    if (lwtc.FirstOrDefault().is_approve != 9)
                    {
                        lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = true }); //Chage status
                    }
                    else
                    {
                        lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = false }); //Chage status
                    }

                    if (trg.Training_SponsorType == 4)
                    {
                        if (lwtc.FirstOrDefault().is_approve == 0)
                        {
                            lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = true }); //Chage status
                        }
                        else
                        {
                            lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                        }
                        lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = false });
                    }
                    else
                    {
                        lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = true });
                        lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                    }

                }
                else if (usertype == "2")
                {
                    lu.Add(new OptionsDisplay { id = "1", name = "Remove from training", dispay = true }); //Delete Participant
                    lu.Add(new OptionsDisplay { id = "2", name = "Move to other training", dispay = false }); //Add Participant
                                                                                                              //lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status
                    lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = true }); //Chage status
                    lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = false });
                    lu.Add(new OptionsDisplay { id = "8", name = "Send Mail", dispay = true }); //Add Participant
                                                                                                // lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false }); //Chage status
                                                                                                //lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });
                    if (lwtc.FirstOrDefault().is_approve == 0 || lwtc.FirstOrDefault().is_approve == 9)
                    {
                        if (laststatus == 0 || laststatus == 2)
                        {
                            lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = true }); //Chage status
                            lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = true });
                        }
                        else
                        {
                            lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status
                                                                                                      //lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = true }); //Chage status
                        }

                    }

                }
                else
                {
                    lu.Add(new OptionsDisplay { id = "1", name = "Remove from training", dispay = false }); //Delete Participant
                    lu.Add(new OptionsDisplay { id = "2", name = "Move to other training", dispay = false }); //Add Participant
                    lu.Add(new OptionsDisplay { id = "3", name = "Approve", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "5", name = "Suspend", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "6", name = "Pay Fees", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "7", name = "Change Sponsor", dispay = false });
                    lu.Add(new OptionsDisplay { id = "8", name = "Send Mail", dispay = false }); //Add Participant
                    lu.Add(new OptionsDisplay { id = "9", name = "Approve with Login", dispay = false }); //Chage status
                    lu.Add(new OptionsDisplay { id = "10", name = "Approve Without Login", dispay = false });

                }
            }


            return Ok(lu);
        }

        [HttpGet]
        [Route("api/Participant_Exist_In_trg")]
        public IActionResult Validate_User_Training(string participantid, string trainingid)
        {

            ParticipantDB WDB = new ParticipantDB(_configuration);
            bool isexist = WDB.Validate_User_Training(participantid, trainingid);
            return Ok(isexist);
        }

        //[HttpPost]
        //[Route("api/Update_Participant_Certificate_Info")]
        //public IActionResult Update_Participant_Certificate_Info([FromBody] Certificate_info_List Certificate_info, string trainingid)
        //{
        //    ParticipantDB tbl = new ParticipantDB(_configuration);

        //    bool issaved = tbl.Update_Participant_certificate_info(Certificate_info, trainingid);

        //    return Ok(issaved);

        //}
    }
}
