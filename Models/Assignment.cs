namespace LitteraCore.Models
{
    public class Assignment
    {
        public string AssignmentID { get; set; }

        public string Instructions { get; set; }

        public string Tag { get; set; }

        public string AssesmentQuestions { get; set; }
        public string FacultyID_Json { get; set; }

        public string AttachmentsID_Json { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public int GradeApplicable { get; set; }
        public string AssignmentName { get; set; }

        public DateTime? createdon { get; set; }

        public string createdby { get; set; }

        public int status { get; set; }

        public Session session { get; set; }

        public decimal MaxMarks { get; set; }

        public string uploadpath { get; set; }


        public Training training { get; set; }

        //Below parameters are extra parameters which not required in further process
        public string facultyname { get; set; }
        public string AssignmentTypeName { get; set; }
        public string TrainingCode { get; set; }
        public string Trainingid { get; set; }

        public DateTime? ttttt_session_dt { get; set; }

        public string ttttt_session_time { get; set; }

        public int ttttt_session_duration { get; set; }
        public DateTime? ttttt_session_end_time { get; set; }

        // Assignment's own deadline (Assessment.Schedule.EndDateTime, returned by
        // proc_get_assignment_list_data as assignmentEndDateTime). NULL for legacy
        // assignments that own a type-6 session - those use the session end time.
        public DateTime? assignmentEndDateTime { get; set; }

        // Deadline to use everywhere (overdue check, "Last Date" display).
        public DateTime? effectiveEndDateTime
        {
            get { return assignmentEndDateTime ?? ttttt_session_end_time; }
        }
        public string tdds_doc_no { get; set; }

        public string ttttt_session_id { get; set; }

        public int ttttt_session_day { get; set; }
        public int ttttt_session_week { get; set; }
        public int ttttt_session_no { get; set; }
        public string ttttt_session_module { get; set; }

        public int isOpenended { get; set; }

        public string taau_status { get; set; }
        public string taau_uploadid { get; set; }

        public List<AssignmentQuestions> AssignmentQuestionsMarks { get; set; }

        public Trg_Setting? trg_Setting { get; set; }

        public UserBranch userbranches { get; set; }
    }
    public class AssignmentType
    {
        public string AssignmentTypeID { get; set; }
        public string AssignmentTypename { get; set; }
        public DateTime? createdon { get; set; }

        public string createdby { get; set; }
    }
    public class AssignmentValuation
    {
        public string taav_id { get; set; }
        public string taav_assignmentid { get; set; }
        public List<valuation_json> taav_valuation_json { get; set; }
        public string taav_createdby { get; set; }
        public DateTime? taav_createdon { get; set; }
    }
    public class valuation_json
    {
        public string assignmentid { get; set; }
        public string participantid { get; set; }

        public string participantname { get; set; }

        public string participantphoto { get; set; }
        public decimal? valuation { get; set; }
        public DateTime? createdon { get; set; }

        public string createdby { get; set; }

        public string remark { get; set; }
    }
    public class proc_ass_get_assignment_comment
    {
        public string assignment { get; set; }

        public assignmentparticipant[] participant { get; set; }

    }
    public class assignmentparticipant
    {
        public string participantid { get; set; }

        public string participantname { get; set; }

        public string participant_mobileno { get; set; }
        public string participant_emailid { get; set; }

        public string path;
        public string participantphoto { get { return this.path; } set { path = get_path(value); } }

        public paticipant_assignment_comments[] comment { get; set; }

        public paticipant_assignment_Uploads[] uploadvalue { get; set; }

        public int valuation_status { get; set; }

        public decimal? marks_allocated { get; set; }

        public string get_path(string absouutepath)
        {
            string path = "";
            if (absouutepath != null)
            {
               
                path =  absouutepath;
            }
            else
            {

                return @"/theme/images/faculty.jpg";
            }
            return path;
        }
    }
    public class proc_ass_get_assignment_upload
    {
        public string assignment { get; set; }
        public int no_of_uploads { get; set; }

        public assignmentparticipant[] participant { get; set; }

        public int status { get; set; }
    }
    public class paticipant_assignment_comments
    {
        public string taac_commentid { get; set; }
        public string taac_commentedby { get; set; }

        public string taac_comment { get; set; }

        public DateTime taac_createdon { get; set; }

        public string commentedby { get; set; }

        public string path;
        public string commentedbyohoto { get { return this.path; } set { path = get_path(value); } }



        public string get_path(string absouutepath)
        {
            string path = "";
            if (absouutepath != null)
            {
              
                path =absouutepath;
            }
            else
            {
                return @"/theme/images/faculty.jpg";
            }
            return path;
        }
    }
    public class paticipant_assignment_Uploads
    {
        public string taau_uploadid { get; set; }
        public string taau_uploadedby { get; set; }

        public string taau_uploadpath { get; set; }

        public DateTime taau_createdon { get; set; }

        public string uploadedby { get; set; }



        public string taau_title { get; set; }



        public string taau_type { get; set; }

        public paticipant_assignment_Uploads_Comment[] comment { get; set; }


        public string uploadedbyphoto { get; set; }


    }
    public class paticipant_assignment_Uploads_Comment
    {
        public string taau_uploadid { get; set; }
        public string taau_commentedby { get; set; }


        public DateTime taau_createdon { get; set; }

        public string taau_comment { get; set; }



        public string commentedbyname { get; set; }
        public string commentedbyphoto { get; set; }


    }

    public class AssignmentUploadComments
    {
        public string taau_uploadid { get; set; }

        public string taau_commentedby { get; set; }

        public string taau_createdon { get; set; }

        public string taau_comment { get; set; }


    }

    public class AssignmentComment
    {
        public string taac_commentid { get; set; }
        public string taac_AssignmentID { get; set; }
        public string taac_Participantid { get; set; }
        public string taac_commentedby { get; set; }
        public string taac_comment { get; set; }

        public string taac_createdon { get; set; }


    }

    public class AssignmentUpload
    {
        public string taau_uploadid { get; set; }

        public string taau_uploadpath { get; set; }
        public string taau_AssignmentID { get; set; }

        public string taau_Participantid { get; set; }
        public string taau_uploadedby { get; set; }

        public string taau_title { get; set; }

        public string taau_remark { get; set; }

        public string taau_type { get; set; }

        public string taau_createdon { get; set; }

        public int taau_status { get; set; }

        public string branchid { get; set; }
        public string trainingid { get; set; }
        public string sessionid { get; set; }

        public List<AssignmentUploadComments> taau_comment_json { get; set; }
    }

    public class AssignmentQuestions
    {
        public string questionid { get; set; }
        public string Description { get; set; }
        public decimal max_marks { get; set; }
        public decimal? max_allocated { get; set; } = null;

        // Per-question tags (added 2026-09-26). Optional - older questions have none.
        public List<string>? tags { get; set; }

    }
    public class Assignment_Question_Valuation
    {
        public string taaqv_id { get; set; }
        public string taaqv_assessmentid { get; set; }
        public string taaqv_participantid { get; set; }
        public int taaqv_status { get; set; }
        public string createdby { get; set; }
        public string createdon { get; set; }
        public string createdby_name { get; set; }
        public AssignmentQuestions[] taaqv_valuation_json { get; set; }

    }
    public class Assignment_Valuation_Summary
    {
        public int total_participant { get; set; }
        public int assignment_submitted { get; set; }
        public int valuation_completed { get; set; }
      

    }
    public class assignment_session_mapping_data
    {
        public string assignmentid { get; set; }
        public string trainingid { get; set; }
        public string sessionid { get; set; }
    }

    // ------------------------------------------------------------------
    // Added 2026-09-26 - frm_assignment_creation.aspx -> React migration.
    // Assignment is now attached to an EXISTING session (no new type-6
    // session is created). See project doc
    // claude/assignment-creation-existing-session-analysis-2026-09-26.md
    // ------------------------------------------------------------------
    public class AssignmentSaveRequest
    {
        // Empty on create (a new GUID is generated); required on update.
        public string? assignmentid { get; set; }
        public string assignmentname { get; set; }
        public string assignmenttypeid { get; set; }
        public string instructions { get; set; }
        public string tag { get; set; }
        // Assignment description (rich HTML) - old "asswesmentquestion".
        public string assessmentquestion { get; set; }
        // Faculty ids; stored as JSON [{"id":"..."}] exactly like the old page.
        public List<string>? faculty { get; set; }
        public string? attachments { get; set; }
        public decimal maxmarks { get; set; }
        public decimal? minmarks { get; set; }
        public List<AssignmentQuestionInput>? questions { get; set; }

        public string trainingid { get; set; }
        // Existing session the assignment is attached to.
        public string sessionid { get; set; }
        // Deadline, passed separately from the session. Required unless open-ended.
        public DateTime? enddatetime { get; set; }
        public int isopenended { get; set; }

        public string createdby { get; set; }
        public string createdempid { get; set; }
        public string branchid { get; set; }
    }

    public class AssignmentQuestionInput
    {
        public string? questionid { get; set; }
        // Rich HTML (text, images, diagrams as images).
        public string description { get; set; }
        public decimal max_marks { get; set; }
        public List<string>? tags { get; set; }
    }

    public class AssignmentSaveResult
    {
        public bool success { get; set; }
        public string assignmentid { get; set; }
        public string? docno { get; set; }
        public string? message { get; set; }
    }

    public class AssignmentTypeSaveRequest
    {
        public string assignmenttype { get; set; }
        public string createdby { get; set; }
    }
}
