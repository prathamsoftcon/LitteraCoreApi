using LitteraCore.Models;

namespace LitteraCore.Common
{
    public class CommonEnum
    {
        public static DateTime DefaultDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
        public static string Agency_Active_Status = "0,1,9,2";
        public static string PortalAdmin_Agencyid = "06076C68-EEDD-4373-B24E-602AA93E7421";
        public static string SuperAdmin_Agencyid = "6DA19908-EA60-4FBF-BBE0-60B07E4CC5D4";
        public static string Agencytype_Staff = "00008";
        public static string Discount_Ledgerid = "417119B3-18B6-43A1-8A52-064E99B6880E";
        public static string Branchid = "dff7c661-5b84-4a7e-8250-31c420dd9fcd";
        public static DateTime content_expiry = Convert.ToDateTime("2025/06/30");
        public enum UserType
        {
            Admin = 1,
            Department = 2,
            CD = 3,
            Faculty = 4,
            Participant = 5


        }
        public enum UserDefaultRole
        {
            ADMIN = 2052,
            CD = 2053,
            FACULTY = 2050,
            PARTICIPANT = 2049,
            ORGANISATION = 38

        }
        public enum TEST_RESULT_OPTIONS
        {
            Correct = 1,
            Incorrect = 0,
            Not_answered = 3,

        }
        public enum usertype
        {
            Admin = 1,
            DEPT = 2,
            CD = 3,
            FACULTY = 4,
            PARTICIPANT = 5,
        }
        public enum Rights
        {
            View = 1,
            Insert = 2,
            Update = 4,
            Delete = 8,
            Approve = 16,
            BulkUpload = 32,
            ChangeStatus = 64,
            Mail = 128,
            Share = 256,
            start = 512

        }

        public enum DMS_TAT_TYPE_ID
        {
            Participant_MAPPING = 122,
            Participant_REGIS = 110,
            Test = 115,
            Course_suit_item = 128,
            Global_Content_Id = 119,
            PROPOSAL = 88,
            TRAINING_CREATION = 93,
            TIME_TABLE = 87,

            SPORT_KIT_DEMAND_NOTE = 97,
            LEAVE_REQUEST = 91,
            LIBRARY_BOOKS_REQUISITION = 102,
            HALL_BOOKING = 90,
            LAPTOP_DEMAND_NOTE = 96,
            TRAINING_KIT_DEMAND_NOTE = 94,
            HONORARIUM_PAYMENT_ORDER = 92,
            FACULTY_REG_REQUEST = 89,
            DEPARTMENT_FUNCTIONS= 129,
            JOB_POSITION = 130,
            Activity = 131,
            Job_Role = 132

        }
        public enum Agencystatus
        {
            Pending = 0,
            Approved = 1,
            Suspended = 9,
            Approved_Without_Login = 2,
            Deleted = -1,


        }
        public enum CASTCATEGORY
        {
            General = 1,
            OBC = 2,
            SC = 3,
            ST = 4,
            Not_To_Disclose = 5
        }
        public enum ID_PROOF_TYPE
        {
            Aadhar = 1,
            VoterID = 2,
            Driving_Licence = 3,
            Pan = 4,
            Passport = 5
        }
        public enum SessionDurationType
        {
            Min = 1,
            Hr = 2

        }
        public enum AgencyType
        {
            Staff = 00008
           

        }
        public enum FunctionStatus
        {
            Prepared = 0,
            Approved = 1,
            Forwarded= 2,
            Sent_For_Approval=3,
            Cancelled = -1

        }



        public enum SESSION_LIST_DISPLAY_OPTIONS
        {
           Session_No = 1,
            Date = 2,
            Time = 3,
            Duration = 4,
            SessionIcon = 5,
            Progress = 6,
            Day = 7,
            Week = 8,
            Module = 9
        }


        public enum SESSION_TYPE
        {
            Personled = 1,
            Breaks = 2,
            Tour = 3,
            Group_discussion = 4,
            Presentation = 5,
            Assignment = 6,
            Test = 7,
            Physical_training = 8,
            Sport = 9,
            Practical = 10,
            Self_paced = 11,
        }

        public enum TRAINING_TYPE
        {

            SELF_PACED = 2,
            OTHER = 1

        }

        public static string GET_JSON_FOLDER()
        {
            return @"Content/GlobalSetting/";
        }

        public enum SessionGroup
        {
            Study_Group = 1,
            Sport_Group = 2,
            Evaluation_Group = 3,
            Breaks_Group = 4,
            Tours_Group = 5,
            Self_paced = 6



        }

        public enum ParticipantType
        {
            Individual = 1,
            Organisation = 2,
            Intern = 3,
            IndividualWithoutLogin = 4

        }
        public enum ParticipationType
        {
            AnyoneCanJoin = 1,
            InvitationOnly = 2

        }

        public enum Page_TYPE
        {
           LitteraRoom=1

        }
        public enum Gender
        {
            Male = 1,
            Female = 0,
            Not_to_disclose = 2,
        }
        public enum Participant_Enroll_Status
        {
            Pending = 0,
            Approved = 1,
            Suspended = 9,
            Paid = 2,
            Deleted = -1,
            Consent_Received = 4,
            Sent_For_Approval = 5

        }
        public enum Participant_Status
        {
            Pending = 0,
            Approved_with_login = 1,
            Approved_Without_Login = 2,
            Suspended = 9,
            Deleted = -1,
            ConsentGiven=3

        }
        public enum Cast
        {
            General = 1,
            OBC = 2,
            SC = 3,
            ST = 3,
            Not_to_disclose = 5,
        }

        public static string Page_Allowed_Session_Type(int pagetype)
        {
            string allowedType = "";
            if(pagetype == (int)Page_TYPE.LitteraRoom)
            {
                allowedType = Convert.ToString((int)SESSION_TYPE.Personled) +","+ Convert.ToString((int)SESSION_TYPE.Tour)+","+ Convert.ToString((int)SESSION_TYPE.Group_discussion)+"," + Convert.ToString((int)SESSION_TYPE.Presentation) +","+ Convert.ToString((int)SESSION_TYPE.Physical_training)+","+ Convert.ToString((int)SESSION_TYPE.Sport)+","+ Convert.ToString((int)SESSION_TYPE.Practical)+","+ Convert.ToString((int)SESSION_TYPE.Self_paced);
            }
            return allowedType;
        }

        public enum SessionEntryControl
        {
            Module = 1,
            Week = 2,
            Date = 3,
            Day = 4,
            SrNo = 5
            //Show only 1,2,4,5 for self paced and 1,2,3,4 for other than self paced type trainig
        }
        public enum TrainingStatus
        {
            Proposed = 0,
            Started = 1,
            Postpone = 2,
            Cancel = 3,
            Completed = 4,
            Started_Reg_Open = 5

        }
        public static int Get_Self_Paced_Trg(string trg_type)
        {
            string[] selfarr = { "2" };
            if (selfarr.Contains(trg_type) == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public enum Feedbackstatus
        {
            Inactive = 0,
            Active = 1,
            Closed = 8,
            Pause = 9,
            Deleted = -1,


        }
        public enum QuestionResponseType
        {
            Descriptive = 1,
            MCQ = 2,
            Rating = 3

        }
        public enum Session_Status
        {
            Delete = 9

        }
        public enum SessionType
        {
            Academic = 1,
            Break = 2,
            Tour = 3,
            GroupDiscussion = 4,
            Presentation = 5,
            Assignment = 6,
            Test = 7,
            PT = 8,
            Sports = 9,
            Practical = 10,
            SelfPaced = 11
        }

        public enum SESSION_LIST_ACTIONS
        {

            CREATE_MEETING = 1,
            START_MEETING = 2,
            JOIN_MEETING = 3,
            SHARE_MEETING = 4,
            LITTERA_ROOM = 5,
            SHARE_FEEDBACK = 6,
            CHECK_COMPETENCY = 7,
            GIVE_FEEDBACK = 8,
            EDIT_SESSION = 9,
            DELETE_SESSION = 10,
            CONTENT_LIBRARY = 11,
            RUN_TEST = 12,
            View_TEST_RESULT = 13,
            Complete_Session = 14

        }

      
        public static string Get_Session_type_Display_Icon(int sessiontype)
        {
            string icon = "";
            switch (sessiontype)
            {
                case 1:
                    icon = "fa fa-graduation-cap";
                    break;
                case 2:
                    icon = "fa fa-coffee";
                    break;
                case 3:
                    icon = "fa fa-cab";
                    break;
                case 4:
                    icon = "fa fa-wechat";
                    break;
                case 5:
                    icon = "fa fa-file-powerpoint-o";
                    break;
                case 6:
                    icon = "fa fa-file-text-o";
                    break;
                case 7:
                    icon = "glyphicon glyphicon-check";
                    break;
                case 8:
                    icon = "fa fa-child";
                    break;
                case 9:
                    icon = "fa fa-life-saver";
                    break;
                case 10:
                    icon = "fa fa-flask";
                    break;
                case 11:
                    icon = "fa fa-book";
                    break;
            }
            return icon;
        }

        public static List<Session> OrderSessionData(string sessionOrder, List<Session> S)
        {
            string orderdata = sessionOrder;//Get_Session_Display_Order_Setting(trg_type, trainingid);
            List<string> ol = orderdata.Split(",".ToCharArray()).ToList();
            List<string> ordercolumns = new List<string>();
            List<bool> ordertype = new List<bool>();
            foreach (string o in ol)
            {
                var ordername = Enum.GetName(typeof(SessionEntryControl), Convert.ToInt32(o));
                string columnname = get_session_entry_mapped_column(ordername);
                ordercolumns.Add(columnname);
                ordertype.Add(true);

            }
            S = OrderByMultipleColumns(S, ordercolumns, ordertype);

            return S;
        }

        public static string get_session_entry_mapped_column(string entryname)
        {
            string name = "";
            if (entryname.ToString().ToUpper() == "MODULE")
            {
                name = "module";
            }
            else if (entryname.ToString().ToUpper() == "WEEK")
            {
                name = "ttttt_session_week";
            }
            else if (entryname.ToString().ToUpper() == "DAY")
            {
                name = "ttttt_session_day";
            }
            else if (entryname.ToString().ToUpper() == "DATE")
            {
                name = "ttttt_session_dt";
            }
            else if (entryname.ToString().ToUpper() == "SRNO")
            {
                name = "ttttt_session_no";
            }


            return name;
        }

        static List<Session> OrderByMultipleColumns(List<Session> list, List<string> columnNames, List<bool> ascendings)
        {
            if (columnNames.Count != ascendings.Count)
            {
                throw new ArgumentException("The number of column names must match the number of ascending flags");
            }

            IOrderedEnumerable<Session> orderedList = null;

            // Order the list based on the first column
            var firstColumn = columnNames[0];
            var firstAscending = ascendings[0];
            orderedList = firstAscending ? list.OrderBy(x => x.GetType().GetProperty(firstColumn).GetValue(x, null)) :
                                           list.OrderByDescending(x => x.GetType().GetProperty(firstColumn).GetValue(x, null));

            // Then order by subsequent columns
            for (int i = 1; i < columnNames.Count; i++)
            {
                var columnName = columnNames[i];
                var ascending = ascendings[i];
                orderedList = ascending ? orderedList.ThenBy(x => x.GetType().GetProperty(columnName).GetValue(x, null)) :
                                          orderedList.ThenByDescending(x => x.GetType().GetProperty(columnName).GetValue(x, null));
            }

            return orderedList.ToList();
        }

        public static string Get_Default_USER_TAT_TYPE(int usertype, string agencytypeid)
        {
            string tattypeid = "";
            switch (usertype)
            {
                case 1:
                    if (agencytypeid == "00008")
                    {
                        tattypeid = "125";
                    }

                    break;
                case 2:
                    if (agencytypeid == "00064")
                    {
                        tattypeid = "118";
                    }
                    break;
                case 3:
                    if (agencytypeid == "00008")
                    {
                        tattypeid = "106";
                    }
                    break;
                case 4:
                    if (agencytypeid == "00054")
                    {
                        tattypeid = "112";
                    }
                    else if (agencytypeid == "00066")
                    {
                        tattypeid = "112";
                    }
                    else if (agencytypeid == "00067")
                    {
                        tattypeid = "112";
                    }
                    break;
                case 5:
                    if (agencytypeid == "00065")
                    {
                        tattypeid = "126";
                    }
                    else if (agencytypeid == "00051")
                    {
                        tattypeid = "110";
                    }
                    break;
                case 0: // for other than 1 to 5 user type
                    if (agencytypeid == "00053")
                    {
                        tattypeid = "114";
                    }
                    else if (agencytypeid == "00068")
                    {
                        tattypeid = "127";
                    }

                    break;
                case 99: // for other than 1 to 5 user type
                    if (agencytypeid == "00053")
                    {
                        tattypeid = "114";
                    }
                    else if (agencytypeid == "00068")
                    {
                        tattypeid = "127";
                    }

                    break;

            }
            return tattypeid;
        }

        public enum DASHBOARD_TRG_ACTIONS
        {

            SessionList = 1,
            ContentLibrary = 2,
            UpdateStatus = 3,
            Attendance = 4,
            Litteraroom = 5,
            TestList = 6,
            AssignmentList = 7,
            AddParticipant = 8,
            TrainingCalendar = 9,
            MeetingList = 10,
            Forum = 11,
            Trg_Expenses = 12,
            Feedback = 13,
            Course_Overview = 14,
            Transaction_Details = 15,


        }
        public enum MaskingColumn
        {
            MOBILENO = 0,
            EMAIL = 1,

        }

       

    }
}
