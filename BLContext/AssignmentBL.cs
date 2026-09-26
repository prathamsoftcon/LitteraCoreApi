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
            PL = PDB.Get_TRG_PARTICIPANT_Data(trainingid,null, branchid, "\"ParticipantId,ParticipantName,photopath,totalrecords,ttpai_id,is_approve\"");

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


        public decimal? Get_Participant_Marks(string assignmenid, string participantid, string branchid = null)
        {

            decimal? marks = 0;

            AssignmentDB ADB = new AssignmentDB(_configuration);
            

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

                vj= vj.Where(o=>o.participantid.ToString().ToUpper()== participantid.ToString().ToUpper()).ToList();
                if (vj.Count() > 0)
                {
                    marks = vj.FirstOrDefault().valuation;
                }
            }
            else
            {
                throw new Exception("Marks not Allotted");
            }

         



          
            return marks;
        }


        public assignment_session_mapping_data Get_Session_Assignment_Details(string sessionid)
        {
            assignment_session_mapping_data assignment = new assignment_session_mapping_data();
            AssignmentDB edb = new AssignmentDB(_configuration);
            assignment = edb.Get_Session_Assignment_Details(sessionid);

            return assignment;
        }

        // ------------------------------------------------------------------
        // Added 2026-09-26 - assignment creation on an EXISTING session.
        // Business rules decided with Nishith (see project doc
        // claude/assignment-creation-existing-session-analysis-2026-09-26.md):
        //  - session types allowed: all except Break(2), Assignment(6), Test(7);
        //    deleted sessions (status 9) not allowed
        //  - multiple assignments per session allowed
        //  - deadline passed separately (required unless open-ended)
        //  - session can be changed on edit only while status is 0 (Draft / Not started)
        //  - the session itself is never modified from here
        // Validation failures throw ArgumentException (-> 400 in the controller).
        // ------------------------------------------------------------------
        private static readonly int[] NotAllowedSessionTypes =
        {
            (int)Common.CommonEnum.SESSION_TYPE.Breaks,
            (int)Common.CommonEnum.SESSION_TYPE.Assignment,
            (int)Common.CommonEnum.SESSION_TYPE.Test
        };

        private static bool HasQuestionContent(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return false;
            if (html.IndexOf("<img", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            string text = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", " ");
            text = System.Net.WebUtility.HtmlDecode(text);
            return !string.IsNullOrWhiteSpace(text);
        }

        private List<string> Validate_Assignment_Fields(AssignmentSaveRequest a)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(a.assignmentname)) errors.Add("Enter assignment name.");
            if (string.IsNullOrWhiteSpace(a.assignmenttypeid)) errors.Add("Select assignment type.");
            if (string.IsNullOrWhiteSpace(a.instructions)) errors.Add("Enter instructions.");
            if (string.IsNullOrWhiteSpace(a.tag)) errors.Add("Enter tag.");
            if (!HasQuestionContent(a.assessmentquestion)) errors.Add("Enter assignment description.");
            if (a.maxmarks <= 0) errors.Add("Enter max marks.");
            if (a.minmarks != null && (a.minmarks < 0 || a.minmarks > a.maxmarks)) errors.Add("Min passing marks must be between 0 and max marks.");
            if (string.IsNullOrWhiteSpace(a.trainingid)) errors.Add("Training is required.");
            if (string.IsNullOrWhiteSpace(a.sessionid)) errors.Add("Select session.");
            if (string.IsNullOrWhiteSpace(a.createdby) || string.IsNullOrWhiteSpace(a.branchid)) errors.Add("User details are missing.");
            if (a.isopenended != 1 && a.enddatetime == null) errors.Add("Enter end date and end time.");

            if (a.questions != null)
            {
                for (int i = 0; i < a.questions.Count; i++)
                {
                    var q = a.questions[i];
                    if (!HasQuestionContent(q.description)) errors.Add("Question " + (i + 1) + ": enter question text or add an image.");
                    if (q.max_marks <= 0) errors.Add("Question " + (i + 1) + ": enter max marks.");
                }
            }
            return errors;
        }

        private Session Validate_Target_Session(AssignmentSaveRequest a, List<string> errors)
        {
            SessionDB sdb = new SessionDB(_configuration);
            Session s = sdb.Get_Session_Details(a.sessionid);
            if (s == null || string.IsNullOrWhiteSpace(s.ttttt_session_id))
            {
                errors.Add("Selected session was not found.");
                return null;
            }
            if (!string.Equals(Convert.ToString(s.trainingid), Convert.ToString(a.trainingid), StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Selected session does not belong to this training.");
            }
            if (Convert.ToString(s.ttttt_status) == "9")
            {
                errors.Add("Selected session is deleted.");
            }
            if (NotAllowedSessionTypes.Contains(s.ttttt_type))
            {
                errors.Add("Assignment cannot be attached to a Break, Test or Assignment session.");
            }
            return s;
        }

        private static void Validate_End_After_Session_Start(AssignmentSaveRequest a, Session s, List<string> errors)
        {
            if (a.isopenended == 1 || a.enddatetime == null || s == null) return;
            DateTime start;
            if (DateTime.TryParse(Convert.ToString(s.ttttt_session_dt) + " " + Convert.ToString(s.ttttt_session_time), out start))
            {
                if (a.enddatetime.Value <= start)
                {
                    errors.Add("End date/time must be after the session start (" + start.ToString("dd/MM/yyyy HH:mm") + ").");
                }
            }
        }

        public AssignmentSaveResult Save_Assignment(AssignmentSaveRequest a)
        {
            List<string> errors = Validate_Assignment_Fields(a);
            if (!string.IsNullOrWhiteSpace(a.sessionid) && !string.IsNullOrWhiteSpace(a.trainingid))
            {
                Session s = Validate_Target_Session(a, errors);
                Validate_End_After_Session_Start(a, s, errors);
            }
            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join("\n", errors));
            }

            a.assignmentid = null; // always a new id on create
            AssignmentDB adb = new AssignmentDB(_configuration);
            return adb.Save_Assignment(a);
        }

        public AssignmentSaveResult? Update_Assignment(AssignmentSaveRequest a)
        {
            if (string.IsNullOrWhiteSpace(a.assignmentid))
            {
                throw new ArgumentException("Assignment id is required.");
            }

            AssignmentDB adb = new AssignmentDB(_configuration);
            Assignment existing = adb.Get_Assignment_Data(a.assignmentid).FirstOrDefault();
            if (existing == null)
            {
                return null;
            }

            List<string> errors = Validate_Assignment_Fields(a);
            // Training can't be changed on edit.
            a.trainingid = existing.Trainingid;

            bool sessionChanged = !string.IsNullOrWhiteSpace(a.sessionid)
                && !string.Equals(a.sessionid, existing.ttttt_session_id, StringComparison.OrdinalIgnoreCase);
            Session s = null;
            if (sessionChanged)
            {
                if (existing.status != 0)
                {
                    errors.Add("Session can only be changed while the assignment is in Draft (Not started) status.");
                }
                s = Validate_Target_Session(a, errors);
            }
            else
            {
                // Unchanged (includes legacy assignments on their own type-6 session).
                a.sessionid = existing.ttttt_session_id;
                SessionDB sdb = new SessionDB(_configuration);
                s = sdb.Get_Session_Details(a.sessionid);
            }
            Validate_End_After_Session_Start(a, s, errors);

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join("\n", errors));
            }
            return adb.Update_Assignment(a);
        }

        public List<AssignmentType> Get_Assignment_Types()
        {
            AssignmentDB adb = new AssignmentDB(_configuration);
            return adb.Get_Assignment_Types();
        }

        public AssignmentType Save_Assignment_Type(AssignmentTypeSaveRequest t)
        {
            if (t == null || string.IsNullOrWhiteSpace(t.assignmenttype))
            {
                throw new ArgumentException("Enter assignment type.");
            }
            if (string.IsNullOrWhiteSpace(t.createdby))
            {
                throw new ArgumentException("User details are missing.");
            }
            AssignmentDB adb = new AssignmentDB(_configuration);
            bool exists = adb.Get_Assignment_Types()
                .Any(x => string.Equals((x.AssignmentTypename ?? "").Trim(), t.assignmenttype.Trim(), StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                throw new ArgumentException("Assignment type already exists.");
            }
            return adb.Save_Assignment_Type(t);
        }
    }
}
