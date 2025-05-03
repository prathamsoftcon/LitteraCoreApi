using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.Common.DMS;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LitteraCore.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ILogger<AssignmentController> _logger;

        private readonly IConfiguration _configuration;
        public AssignmentController(IConfiguration configuration, ILogger<AssignmentController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("api/Assignment")]
        public IActionResult Assignment(string usertype, string userid, DateTime Startdate, DateTime Enddate, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias, string trainingid = null)
        {

            AssignmentBL ABL = new AssignmentBL(_configuration);
            List<Assignment> assignments = new List<Assignment>();
            assignments = ABL.Get_Assignments(trainingid, usertype, userid, Startdate, Enddate);

            //Get Assignment only for approve participants
            if (usertype == "5")
            {
                ParticipantDB PDB = new ParticipantDB(_configuration);
                List<Participant> P = new List<Participant>();
                P = PDB.Get_Participant_training_status(userid);
                P = P.Where(o => o.is_approve != 1).ToList();
                List<Assignment> assignments_new = new List<Assignment>();
                foreach (Assignment a in assignments)
                {
                    List<Participant> FP = new List<Participant>();
                    FP = P.Where(o => o.TrainingId.ToString().ToUpper() == a.Trainingid.ToString().ToUpper()).ToList();
                    if (FP.Count == 0)
                    {
                        assignments_new.Add(a);
                    }
                }
                //Comment below condition to show assignment on bhoj
               // assignments = assignments_new;

                //************To get assignment upload status
                List<AssignmentUpload> uploads=new List<AssignmentUpload>();
                uploads = ABL.Get_Participant_Uploaded_Assignments(userid);

                foreach(Assignment a in assignments)
                {
                    if(uploads.Where(o => o.taau_AssignmentID.ToString().ToUpper() == a.AssignmentID.ToString().ToUpper()).Count() > 0)
                    {

                        a.taau_status = uploads.Where(o => o.taau_AssignmentID.ToString().ToUpper() == a.AssignmentID.ToString().ToUpper()).FirstOrDefault().taau_status.ToString();
                        a.taau_uploadid= uploads.Where(o => o.taau_AssignmentID.ToString().ToUpper() == a.AssignmentID.ToString().ToUpper()).FirstOrDefault().taau_uploadid.ToString();
                    }
                    else
                    {
                        a.taau_status = "";
                        a.taau_uploadid = "";
                    }


                }


            }

            assignments = assignments.OrderBy(o => o.TrainingCode).ToList();
            assignments= assignments.OrderByDescending(o=>o.ttttt_session_dt).ToList();
            var searchService = new SearchService();
            // Filter items based on the search criteria
            var filteredItems = assignments;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(assignments, searchCriterias.SearchCriteria.ToList());
            }

            foreach (Assignment a in filteredItems)
            {
                if (a.status != -1)
                {
                    if (a.status != 4)
                    {
                        if (Convert.ToDateTime(a.ttttt_session_end_time) < System.DateTime.Now)
                        {
                            a.status = 3;
                        }
                    }
                }
            }

            //if (status != 99)
            //{
            //    p.pagedata = p.pagedata.Where(o => o.status == status).ToList();
            //}
            

            var pagedList = Paging.GetPagedList(param, filteredItems);
            var result = Paging.GetPagedData(param, filteredItems);


       
            return Ok(result);
        }

        [HttpGet]
        [Route("api/AssignmentDetail")]
        public IActionResult AssignmentDetail(string assignmentid = null,string userid=null)
        {
            AssignmentBL ABL = new AssignmentBL(_configuration);
            List<Assignment> assignments = new List<Assignment>();
            assignments = ABL.Get_Assignment_Data(assignmentid);

            foreach (Assignment a in assignments)
            {
                if (a.status != -1)
                {
                    if (a.status != 4)
                    {
                        if (Convert.ToDateTime(a.ttttt_session_end_time) < System.DateTime.Now)
                        {
                            a.status = 3;
                        }
                    }
                }
            }

            if(userid != null)
            {
                List<AssignmentUpload> uploads = new List<AssignmentUpload>();
                uploads = ABL.Get_Participant_Uploaded_Assignments(userid);

                foreach (Assignment a in assignments)
                {
                    if (uploads.Where(o => o.taau_AssignmentID.ToString().ToUpper() == a.AssignmentID.ToString().ToUpper()).Count() > 0)
                    {

                        a.taau_status = uploads.Where(o => o.taau_AssignmentID.ToString().ToUpper() == a.AssignmentID.ToString().ToUpper()).FirstOrDefault().taau_status.ToString();
                        a.taau_uploadid = uploads.Where(o => o.taau_AssignmentID.ToString().ToUpper() == a.AssignmentID.ToString().ToUpper()).FirstOrDefault().taau_uploadid.ToString();
                    }
                    else
                    {
                        a.taau_status = "";
                        a.taau_uploadid = "";
                    }


                }
            }
          
            return Ok(assignments);
        }
        [HttpGet]
        [Route("api/Get_Assignment_Valuation")]
        public IActionResult Get_Assignment_Valuation(string assignmentid, string participantid = null)
        {
            AssignmentBL ABL = new AssignmentBL(_configuration);
            List<AssignmentValuation> assignments = new List<AssignmentValuation>();
            assignments = ABL.Get_Valuation(assignmentid);

            if (participantid != null)
            {
                string taav_id = assignments.FirstOrDefault().taav_id;

                decimal participantmarks = 0;
                if (taav_id != null)
                {
                    DMSBL dbl = new DMSBL(_configuration);
                    int ValuationStatus = dbl.Get_DMS_DOC_STATUS(taav_id, 124);

                    List<valuation_json> valuations = assignments.FirstOrDefault().taav_valuation_json;
                   
                    valuations = valuations.Where(o => o.participantid.ToString().ToUpper() == participantid.ToString().ToUpper()).ToList();
                    return Ok(new { status = ValuationStatus, marks = valuations });
                }
                else
                {
                    //Assignment Not Prepared

                    return Ok(new { status = 0, marks = assignments.FirstOrDefault().taav_valuation_json });
                }


            }
            else
            {
                string T_taav_id = assignments.FirstOrDefault().taav_id;
                DMSBL dbl = new DMSBL(_configuration);
                int laststatus = dbl.Get_DMS_DOC_STATUS(T_taav_id, 124);
                return Ok(new { status = laststatus, marks = assignments.FirstOrDefault().taav_valuation_json });
            }
         

            return Ok(assignments);
        }


        [HttpPost]
        [Route("api/Get_Comments")]
        public IActionResult Get_Comments (string assignmentid,string trainingid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias, string participantid = null,string branchid=null)
        {


            List<proc_ass_get_assignment_comment> final_assignments = new List<proc_ass_get_assignment_comment>();
            List<Participant> PL = new List<Participant>();
            ParticipantDB PDB = new ParticipantDB(_configuration);
            //List of training all participant
            PL = PDB.Get_TRG_PARTICIPANT_Data(trainingid, participantid, branchid);

            AssignmentBL PBL = new AssignmentBL(_configuration);
            List<proc_ass_get_assignment_comment> assignments = new List<proc_ass_get_assignment_comment>();
            //List of all assignmen comment
            assignments = PBL.Get_Comment_Data(assignmentid, participantid);

            List<assignmentparticipant> Allcommented = new List<assignmentparticipant>();
            if (assignments.Count > 0)
            {
                Allcommented = assignments.FirstOrDefault().participant.ToList();
            }


            //*************Code to Create new final assignment list
            proc_ass_get_assignment_comment AM = new proc_ass_get_assignment_comment();
            AM.assignment = assignmentid;

            List<assignmentparticipant> Part = new List<assignmentparticipant>();
            foreach (Participant P in PL)
            {

                var filtercomment = Allcommented.Where(x => x.participantid.ToString().ToUpper() == P.ParticipantId.ToString().ToUpper());

                List<paticipant_assignment_comments> cmt = new List<paticipant_assignment_comments>();
                if (filtercomment.Count() > 0)
                {
                    cmt = filtercomment.FirstOrDefault().comment.ToList();
                }


                Part.Add(new assignmentparticipant
                {
                    participantid = P.ParticipantId,
                    participantname = P.ParticipantName,
                    participantphoto = P.photopath,
                    //  photofullpath = P.photopath,
                    comment = cmt.ToArray()

                }); ;
                //AM.participant = new assignmentparticipant { participantid = P.ParticipantId, }
            }


            //Filter data before
            var searchService = new SearchService();
            var filteredItems = Part;
            if (searchCriterias != null)
            {
                filteredItems = searchService.FilterItems(Part, searchCriterias.SearchCriteria.ToList());
            }

            Part = filteredItems;
            AM.participant = Part.ToArray();

            final_assignments.Add(AM);


            var pagedList = Paging.GetPagedList(param, final_assignments);
            var result = Paging.GetPagedData(param, final_assignments);



            return Ok(result);
        }

        [HttpPost]
        [Route("api/Get_Uploads")]
        public IActionResult Get_Assignment_Uploads(string assignmentid, [FromQuery] PaginationParam param, [FromBody] SearchParam? searchCriterias, string participantid = null,string searchname = null, string doc_type = null,int isdraft=1)
        {
            AssignmentBL PBL = new AssignmentBL(_configuration);
            List<proc_ass_get_assignment_upload> assignments = new List<proc_ass_get_assignment_upload>();
            //List of all assignmen Upload
            assignments = PBL.Get_Upload_Data(assignmentid, participantid);
            if (isdraft == 0)
            {
                assignments = assignments.Where(o => o.status == 1).ToList();
            }

            List<Assignment_Question_Valuation> lav = new List<Assignment_Question_Valuation>();
            lav = PBL.Get_assignment_All_Valuation(assignmentid);
            foreach (proc_ass_get_assignment_upload u in assignments)
            {
                foreach(assignmentparticipant p in u.participant)
                {
                    if (lav.Where(o => o.taaqv_participantid.ToString().ToUpper() == p.participantid.ToString().ToUpper()).Count()>0)
                    {
                        p.valuation_status = lav.FirstOrDefault().taaqv_status;
                    }
                }
            }

            //*******Filter Code
            //if(searchname != null)
            //{
            //    List<proc_ass_get_assignment_upload> filterassignments = new List<proc_ass_get_assignment_upload>();
            //    var ttt = assignments.SelectMany(x => x.participant).Where(y => y.participantname.ToUpper().Contains(searchname.ToUpper())).ToList();
            //    filterassignments.Add(new proc_ass_get_assignment_upload { assignment = assignments.FirstOrDefault().assignment, participant = ttt.ToArray() });
            //    assignments = filterassignments;
            //}

            //List<proc_ass_get_assignment_upload> filterassignments = new List<proc_ass_get_assignment_upload>();

            //foreach (proc_ass_get_assignment_upload a in assignments)
            //{
            //    proc_ass_get_assignment_upload nn = new proc_ass_get_assignment_upload();
            //    nn.assignment = a.assignment;
            //    foreach (assignmentparticipant p in a.participant)
            //    {
            //        nn.participant
            //        var uu = p.uploadvalue.Where(o => o.taau_type == "756156A2-1539-4270-B713-5BAE12645911");
            //    }
            //}

            //List<proc_ass_get_assignment_upload> filterassignments = new List<proc_ass_get_assignment_upload>();
            //var ttt = assignments.SelectMany(x => x.participant).SelectMany(z=>z.uploadvalue).Where(y => y.taau_type.ToUpper().Contains("9A14C631-7222-48C0-9315-F7A972A60351")).ToList();
            //filterassignments.Add(new proc_ass_get_assignment_upload { assignment = assignments.FirstOrDefault().assignment, participant = ttt.ToArray() });
            //assignments = filterassignments;


            // 


            //*****



            //Filter data before



            return Ok(assignments);
        }

        [HttpPut]
        [Route("api/Update_assignment_upload_comment")]
        public IActionResult Update_assignment_upload_comment(string uploadid, [FromBody] AssignmentUploadComments comment)
        {
            AssignmentBL ADB = new AssignmentBL(_configuration);
            bool isupdated= ADB.Update_Upload_Commment(uploadid, comment);

            return Ok(isupdated);
        }
        [HttpPost]
        [Route("api/InsertComment")]
        public IActionResult InsertComment([FromBody] AssignmentComment a)
        {
            AssignmentBL ADB = new AssignmentBL(_configuration);
           bool result= ADB.Insert_Commment(a);

            return Ok(result);
        }

        [HttpPost]
        [Route("api/UploadAssignment")]
        public IActionResult UploadAssignment([FromBody] AssignmentUpload a)
        {
            AssignmentBL ADB = new AssignmentBL(_configuration);
            bool issave= ADB.Upload_assignment(a);
            return Ok(issave);
        }


        [HttpGet]
        [Route("api/AssignmentQuestion")]
        public IActionResult AssignmentQuestion(string assignmentid)
        {
            AssignmentDB ADB=new AssignmentDB(_configuration);  
            List<Assignment> assignments = new List<Assignment>();
            assignments = ADB.Get_Assignment_Data(assignmentid);
            List<AssignmentQuestions> ABL = assignments.FirstOrDefault().AssignmentQuestionsMarks;
            return Ok(ABL);
        }
        [HttpGet]
        [Route("api/Get_Assignment_Question_Validation")]
        public IActionResult Get_Assignment_Question_Validation(string assignmentid,string participantid)
        {
            AssignmentDB ADB = new AssignmentDB(_configuration);
            Assignment_Question_Valuation assignments = new Assignment_Question_Valuation();
            assignments = ADB.Get_assignment_Question_Validation(assignmentid, participantid);
            return Ok(assignments);
        }

        [HttpPost]
        [Route("api/Save_Assignmant_Valuation")]
        public IActionResult Save_Assignmant_Valuation([FromBody] Assignment_Question_Valuation a)
        {
            AssignmentBL ADB = new AssignmentBL(_configuration);
            bool result = ADB.Save_Assignmant_Valuation(a);

            return Ok(result);
        }
        [HttpGet]
        [Route("api/Get_Valuation_Summary")]
        public IActionResult Get_Valuation_Summary(string assignmentid)
        {
            Assignment_Valuation_Summary vw = new Assignment_Valuation_Summary();
            AssignmentDB ADB = new AssignmentDB(_configuration);
            vw = ADB.Valuation_Summary(assignmentid);
            return Ok(vw);
        }
    }
}
