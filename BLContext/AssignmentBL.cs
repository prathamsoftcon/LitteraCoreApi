using LitteraCore.Common.DMS;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Abstractions;
using System.Data;

namespace LitteraCore.BLContext
{
    public class AssignmentBL
    {
        private readonly IConfiguration _configuration;
        public AssignmentBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Assignment> Get_Assignments(string trainingid, string usertype, string userid, DateTime fromdate, DateTime todate)
        {

            AssignmentDB ADB = new AssignmentDB(_configuration);
            List<Assignment> assignments = new List<Assignment>();
            assignments = ADB.Get_Assignment_Data();
            if (trainingid != null)
            {
                assignments = assignments.Where(x => x.Trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();
            }
            List<FilterUserTrg> userwise_assignments = new List<FilterUserTrg>();
            TrainingDB usertrg = new TrainingDB(_configuration);
            List<FilterUserTrg> FL = new List<FilterUserTrg>();
            if (usertype != null)
            {
                FL = assignments.ConvertAll(x => new FilterUserTrg { trainingid = x.Trainingid.ToString() });
                userwise_assignments = usertrg.Get_Users_Trg_Data(FL, usertype, userid, fromdate, todate);
                assignments = assignments.Where(x => userwise_assignments.Any(y => y.trainingid.ToString() == x.Trainingid.ToString())).ToList();
            }
           

            return assignments;
        }
        public List<Assignment> Get_Assignment_Data(string assignmentid = null)
        {

            AssignmentDB ADB = new AssignmentDB(_configuration);
            List<Assignment> assignments = new List<Assignment>();
            assignments= ADB.Get_Assignment_Data(assignmentid);
            return assignments;
        }
        public List<AssignmentValuation> Get_Valuation(string assignmenid, string participantid = null,string branchid=null)
        {



            AssignmentDB ADB = new AssignmentDB(_configuration);
            List<Assignment> assignments = new List<Assignment>();
            assignments = ADB.Get_Assignment_Data(assignmenid);
            //Get Traingingid from assignmment
            string trainingid = assignments.FirstOrDefault().Trainingid;

            List<Participant> PL = new List<Participant>();
            ParticipantDB PDB = new ParticipantDB(_configuration);
            //List of training all participant
            PL = PDB.Get_TRG_PARTICIPANT_Data(trainingid,null, branchid);

            List<AssignmentValuation> AV = new List<AssignmentValuation>();
            AV = ADB.Get_Valuation(assignmenid);

            List<AssignmentValuation> Final_AV = new List<AssignmentValuation>();
            string valuationid = null; string createdby = null; DateTime? createdon = null;
            List<valuation_json> vj = new List<valuation_json>();
            if (AV.Count > 0)
            {
                valuationid = AV.FirstOrDefault().taav_id;
                createdby = AV.FirstOrDefault().taav_createdby;
                createdon = AV.FirstOrDefault().taav_createdon;
                vj = AV.FirstOrDefault().taav_valuation_json;
            }
            AssignmentValuation m = new AssignmentValuation();
            m.taav_assignmentid = assignmenid;
            m.taav_id = valuationid;
            m.taav_createdby = createdby;
            m.taav_createdon = createdon;



            List<valuation_json> vj_new = new List<valuation_json>();
            foreach (Participant p in PL)
            {
                List<valuation_json> filterjson = new List<valuation_json>();
                filterjson = vj.Where(x => x.participantid.ToString().ToUpper() == p.ParticipantId.ToUpper()).ToList();
                if (filterjson.Count > 0)
                {
                    vj_new.Add(new valuation_json { assignmentid = m.taav_assignmentid, participantid = p.ParticipantId, participantname = p.ParticipantName, participantphoto = p.photopath_full, createdby = filterjson.FirstOrDefault().createdby, createdon = filterjson.FirstOrDefault().createdon, valuation = filterjson.FirstOrDefault().valuation, remark = filterjson.FirstOrDefault().remark });
                }
                else
                {
                    vj_new.Add(new valuation_json { assignmentid = m.taav_assignmentid, participantid = p.ParticipantId, participantname = p.ParticipantName, participantphoto = p.photopath_full, createdby = null, createdon = null, valuation = null, remark = null }); ;
                }

            }

            m.taav_valuation_json = vj_new;

            Final_AV.Add(m);
            return Final_AV;
        }
        public List<proc_ass_get_assignment_comment> Get_Comment_Data(string assignmentid, string participantid)
        {
            AssignmentDB PDB = new AssignmentDB(_configuration);
            List<proc_ass_get_assignment_comment> li = PDB.Get_assignment_Comments(assignmentid, participantid);
            return li;
        }
        public List<proc_ass_get_assignment_upload> Get_Upload_Data(string assignmentid, string participantid)
        {
            AssignmentDB PDB = new AssignmentDB(_configuration);
            List<proc_ass_get_assignment_upload> li = PDB.Get_assignment_Uploads(assignmentid, participantid);
            return li;
        }

        public bool Update_Upload_Commment(string uploadid, AssignmentUploadComments a)
        {
            AssignmentDB ADB = new AssignmentDB(_configuration);
            ADB.Update_Assignment_Upload_Comment(uploadid, a);
            return true;
        }

        public bool Insert_Commment(AssignmentComment a)
        {
            AssignmentDB ADB = new AssignmentDB(_configuration);
            ADB.Insert_Comment(a);
            return true;
        }
        public bool Upload_assignment(AssignmentUpload a)
        {
            AssignmentDB ADB = new AssignmentDB(_configuration);
            if (ADB.Upload_Document(a) == true)
            {
                if (a.taau_status == 1)
                {
                    SessionDB sdb = new SessionDB(_configuration);
                    bool issaved = sdb.Update_Session_Status(a.taau_Participantid, a.trainingid, a.sessionid, "00:00", a.branchid, 1);
                }
               
            }
           
            return true;
        }

        public List<AssignmentUpload> Get_Participant_Uploaded_Assignments(string participantid,string assignmentid=null)
        {
            AssignmentDB PDB = new AssignmentDB(_configuration);
            List<AssignmentUpload> li = PDB.Get_Participant_Assignment_Uploads(participantid,assignmentid );
            return li;
        }
        public Assignment_Question_Valuation Get_assignment_Question_Validation(string assignmentid, string participantid)
        {
            AssignmentDB PDB = new AssignmentDB(_configuration);
            Assignment_Question_Valuation li = PDB.Get_assignment_Question_Validation(assignmentid, participantid);
            return li;
        }

        public bool Save_Assignmant_Valuation(Assignment_Question_Valuation a)
        {
            AssignmentDB ADB = new AssignmentDB(_configuration);
            ADB.Save_Assignmant_Valuation(a);
            return true;
        }

        public Assignment_Valuation_Summary Valuation_Summary(string assignmentid)
        {
            Assignment_Valuation_Summary vw=new Assignment_Valuation_Summary();
            AssignmentDB PDB = new AssignmentDB(_configuration);
            vw = PDB.Valuation_Summary(assignmentid);
            return vw;
        }

        public List<Assignment_Question_Valuation> Get_assignment_All_Valuation(string assignmentid)
        {
            AssignmentDB PDB = new AssignmentDB(_configuration);
            List<Assignment_Question_Valuation> li = PDB.Get_assignment_All_Valuation(assignmentid);
            return li;
        }

        public bool update_Assignment_status(DMS d)
        {
            bool is_saved = false;
            AssignmentDB edb = new AssignmentDB(_configuration);
            is_saved = edb.update_Assignment_status(d);

            return is_saved;
        }
    }
}
