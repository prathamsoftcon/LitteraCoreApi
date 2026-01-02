namespace LitteraCore.Models
{
    public class Test
    {
        public string testquestionid { get; set; }

        public string testid { get; set; }

        public string testname { get; set; }

        public string assesmenttime { get; set; }
        public string skilltag { get; set; }

        public int isactive { get; set; }
        public string trainingid { get; set; }
        public string trainingcode { get; set; }

        public string trg_type { get; set; }

        public DateTime? createdon { get; set; }
        public string createdbyagencyid { get; set; }

        public string training_sponsortype { get; set; }

        public int noofquestion { get; set; }

        public string sessionid { get; set; }
        public string ttttt_content_desc { get; set; }

        public string ttttt_session_dt { get; set; }
        public string ttttt_session_time { get; set; }
        public string ttttt_session_duration { get; set; }
        public string ttpss_participant_id { get; set; }
        public string ttpss_session_id { get; set; }
        public string ttpss_onscreen_time { get; set; }

        public string ttpss_status { get; set; }
        public string ttpss_created_on { get; set; }
        public string participantstatus { get; set; }

        public string participantenrollstatus { get; set; }
        public int issessioncompleted { get; set; }

        public int tdds_status { get; set; }
        public decimal maxMarks { get; set; }
        public decimal mark_per_question { get; set; }

        public string type { get; set;}

        public string Training_category_name { get; set; }
        public string TrainingCategoryId { get; set; }
        public string QuestionDifficultyID { get; set; }
        public string Test_time { get; set; }

        public string ttttt_status { get; set; }

        public Trg_Setting? trg_Setting { get; set; }

        public int passing_marks_percentage { get; set; } = 33;
        public int max_attempt { get; set; } = 3;
        public int attempted { get; set; } = 0;
    }
    public class TEST_RESULT_DATA
    {
        public string trainingid { get; set; }
        public string training_code { get; set; }
        public string training_name { get; set; }
        public string sessionid { get; set; }
        public string session_subject { get; set; }
        public string session_desc { get; set; }
        public string testid { get; set; }
        public string testname { get; set; }
        public string participantid { get; set; }
        public string participant_name { get; set; }
        public string Questionid { get; set; }
        public string question_desc { get; set; }

        public decimal mark_per_question { get; set; }
        public int iscorrect { get; set; }
        public decimal mark_obtained { get; set; }

        public decimal mark_percentage { get; set; }

        public string TestParticipantid { get; set; }

        public string TestQuestionid { get; set; }
    }
    public class ResultPercentage
    {
        public string key { get; set; }
        public string name { get; set; }
        public double percentage { get; set; }

        public int GroupOn { get; set; }

        public string testQuestionid { get; set; }
        public string testparticipantid { get; set; }


    }



    public class user_session_test
    {
        public string branchid { get; set; }
        public string agencyid { get; set; }
        public int usertype { get; set; }

        public string skilltags { get; set; }

        public string trainingcategoryid { get; set; }

        public string ttpai_id { get; set; }

        public string trainingid { get; set; }

        public Test test { get; set; }

    }

    public class participant_test_result
    {
        public string participantid { get; set; }
        public string testparticipantid { get; set; }
        public string AgencyName { get; set; }
        public string ag_mobileno { get; set; }
        public string ag_email { get; set; }

        public int total_questions { get; set; }
        public int total_correct { get; set; }
        public int total_incorrect { get; set; }

        public int total_not_answered { get; set; }
        public int totalrecored { get; set; }

    }



    public class TEST_SESSION_MAPPING_DATA
    {
        public string testid { get; set; }
        public string trainingid { get; set; }
        public string sessionid { get; set; }
        public string TestQuestionID { get; set; }
       
    }
    public class session_test_details
    {
        public string skillSet { get; set; }
        public int questionCount { get; set; }

        public int? questionDifficutyID { get; set; }
        public string trainingCategory { get; set; }

        public decimal time_per_question { get; set; }

        public int application_type_id { get; set; }
        public int mark_per_question { get; set; }


    }



}

