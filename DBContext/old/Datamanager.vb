Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Xml
Imports DataSecurityLibrary
Imports Newtonsoft.Json
Imports WebYojna
Imports log4net
Imports Newtonsoft.Json.Linq
Imports System.Threading.Tasks

Public Class Datamanager

    Dim con As New SqlConnection
    Dim cmd As New SqlCommand
    Public Shared Companyname As String = "Anon Global Foundation"
    Public Shared DefaultPassword As String = "12345678@"

    Public Shared CENSUSCODE_NAME As String = "CENSUS CODE"
    Public Shared Salutation_ColumnName As String = "SALUTATION"
    Public Shared FirstName_ColumnName As String = "FIRST NAME"
    Public Shared HFirstName_ColumnName As String = "H FIRST NAME"
    Public Shared MiddleName_ColumnName As String = "MIDDLE NAME"
    Public Shared HMiddleName_ColumnName As String = "H MIDDLE NAME"
    Public Shared LastName_ColumnName As String = "LAST NAME"
    Public Shared HLastName_ColumnName As String = "H LAST NAME"
    Public Shared AadharNo_ColumnName As String = "AADHAR NO"
    Public Shared PRINTWITHMADHYAPRADESH_ColumnName As String = "PRINT WITH MADHYAPRADESH"

    Public Shared CENSUS_CODE_ColumnId As String = "00001"
    Public Shared USERCODE_ColumnId As String = "00002"
    Public Shared ADDRESS1_ColumnId As String = "00008"
    Public Shared ADDRESS2_ColumnId As String = "00009"
    Public Shared ADDRESS3_ColumnId As String = "00058"
    Public Shared PINCODE_ColumnId As String = "00025"
    Public Shared TELEPHONENO_ColumnId As String = "00026"
    Public Shared CATEGORY_ColumnId As String = "00315"
    ' Public Shared DDONAME_ColumnId As String = "00053"
    Public Shared DEPT_HEAD_ColumnId As String = "00057"
    Public Shared EMAIL_ColumnId As String = "00111"

    Public Shared PARMANENTPHONE_ColumnId As String = "00117"
    Public Shared LOCALPHONE_ColumnId As String = "00118"
    Public Shared DESIGNATION_ColumnId As String = "00021"
    Public Shared FATHER_ColumnId As String = "00036"
    Public Shared CASTE_ColumnId As String = "00037"
    Public Shared GENDER_ColumnId As String = "00038"
    Public Shared GENDER_Columnname As String = "Gender"
    Public Shared DOB_ColumnId As String = "00039"
    Public Shared DOB_Columnname As String = "00039"

    Public Shared DEPARTMENT_ColumnId As String = "00054"
    Public Shared LOCATION_ColumnId As String = "00052"
    Public Shared LOCATION_TypeId As String = "00051"

    Public Shared PRINTWITHMADHYAPRADESH_ColumnId As String = "00059"
    Public Shared INHOUSEFACULTY_ColumnId As String = "00060"
    '  Public Shared TOADDRESS_ColumnId As String = "00050"
    Public Shared FACULTY_OFFICE_ADDRESS_ColumnId As String = "00056"

    Public Shared COMPANYNAME_ColumnId As String = "00213"
    Public Shared TRAINING_EXP_UNITID As String = "00006"
    Public Shared DEBTER_AGENCYTYPEID As String = "00015"



    Public Shared Salutation_ColumnId As String = "00329"
    Public Shared FirstName_ColumnId As String = "00119"
    Public Shared HFirstName_ColumnId As String = "00330"
    Public Shared MiddleName_ColumnId As String = "00120"
    Public Shared HMiddleName_ColumnId As String = "00331"
    Public Shared LastName_ColumnId As String = "00121"
    Public Shared HLastName_ColumnId As String = "00332"
    Public Shared AadharNo_ColumnId As String = "00321"

    Public Shared BANK_ColumnId As String = "00010"
    Public Shared BRANCH_ColumnId As String = "00011"
    Public Shared IFSC_ColumnId As String = "00055"
    Public Shared ACCOUNT_ColumnId As String = "00012"

    Public Shared EMPLOYEE_ID As String = "00019"

    Public Shared BriefDesc_ColumnId As String = "00041"
    Public Shared Medium_ColumnId As String = "00115"
    '***********Need to decide*******
    Public Shared honor_ColumnId As String = "00135"
    Public Shared phdTitle_ColumnId As String = "00136"
    Public Shared ReasearchPaper_ColumnId As String = "00137    "
    Public Shared TOT_ColumnId As String = "00138"
    Public Shared EXP_ColumnId As String = "00139"
    Public Shared ForeignTraining_ColumnId As String = "00140"
    Public Shared CaseStudy_ColumnId As String = "00141"
    '***********Need to decide*******
    Public Shared Introduction_ColumnId As String = "00056"
    Public Shared GSTIN As String = "00212"
    Public Shared GSTIN_ColumnName As String = "GSTIN No"


    '************************Columns for Sponsor Representative
    Public Shared Rep_Name_ColumnId As String = "00119"
    Public Shared Rep_Designation_ColumnId As String = "00021"
    Public Shared Rep_mobileno_ColumnId As String = "00118"
    Public Shared Rep_officeno_ColumnId As String = "00117"
    Public Shared Rep_email_ColumnId As String = "00111"
    Public Shared Rep_email1_ColumnId As String = "00130"
    Public Shared Rep_email2_ColumnId As String = "00203"
    Public Shared Rep_department_ColumnId As String = "00054"



    Public Shared Rep_Name_columnname As String = "Name"
    Public Shared Rep_Designation_columnname As String = "Designation"
    Public Shared Rep_mobileno_columnname As String = "Mobile No"
    Public Shared Rep_officeno_columnname As String = "Office No"
    Public Shared Rep_email_columnname As String = "Email Id"
    Public Shared Rep_email1_columnname As String = "Alternatee Email 1"
    Public Shared Rep_email2_columnname As String = "Alternatee Email 1"
    Public Shared Rep_department_columnname As String = "Department"



    Public Shared BriefDesc_ColumnName As String = "Brief Description (Posting & Qualification)"
    Public Shared Medium_ColumnName As String = "Medium"
    Public Shared honor_ColumnName As String = "Award / Honour"
    Public Shared phdTitle_ColumnName As String = "Title Of PHD/M.Phil"
    Public Shared ReasearchPaper_ColumnName As String = "Research Paper"
    Public Shared TOT_ColumnName As String = "TOT Of DOPT"
    Public Shared EXP_ColumnName As String = "Experience"
    Public Shared ForeignTraining_ColumnName As String = "Foreign Trainings"
    Public Shared CaseStudy_ColumnName As String = "Case Study"
    Public Shared Introduction_ColumnName As String = "Introduction Of Faculty"


    Public Shared TRAINING_SSA_ADMIN_DEPARTMENT As String = "29EC6B3F-1AEF-45EE-A1F9-8C70EDC233B9"
    Public Shared TRAINING_SSA_GENERAL_DIVISION As String = "86B11FBF-FA9D-4271-BA4B-82E9179D53F7"
    Public Shared CONFIG_ID_Assistant As String = "0A0DEB11-57F1-4FC4-9DBC-3BF554968E1A"
    Public Shared TRAINING_SSA_DIRECTOR_DESIGNATION As String = "7EC22688-783C-4CF5-B1D2-497B38CE3241"
    Public Shared FacultyName_ColumnId As String = "00119"
    Public Shared FacultyHindiName_ColumnId As String = "00330"

    Public Shared CONFIG_ID_TrainingSection As String = "065A0022-9736-49DB-AC39-D365EC4249B0"
    Public Shared CONFIG_ID_TrainingOfficer As String = "F68039DA-1086-4590-960B-35FAA52A695D"
    Public Shared CONFIG_ID_Superident As String = "8267E0D3-6FAF-4C2F-8F03-7FC7F2EAA670"

    Public Shared CONFIG_ID_Notesheet_Joint_Dept As String = "7C9F4F1C-03D8-4188-8B67-49B4950720F8"

    Public Shared USERCODE_ColumnName As String = "USER CODE"
    Public Shared ADDRESS1_ColumnName As String = "ADDRESS 1"
    Public Shared ADDRESS2_ColumnName As String = "ADDRESS 2"
    Public Shared PINCODE_ColumnName As String = "PIN CODE"
    Public Shared TELEPHONENO_ColumnName As String = "TELEPHONE NO"
    Public Shared FacultyName_ColumnName As String = "Faculty Name"
    Public Shared FacultyHindiName_ColumnName As String = "Faculty Hindi Name"

    Public Shared DEPT_HEAD_ColumnName As String = "DEPT_HEAD"

    Public Shared PARMANENTPHONE_ColumnName As String = "PARMANENT PHONE"



    Public Shared LOCATION_ColumnName As String = "LOCATION"
    Public Shared LOCATION_Type_ColumnName As String = "LOCATIONTYPE"
    Public Shared ADDRESS3_ColumnName As String = "ADDRESS 3"
    Public Shared EMAIL_ColumnName As String = "EMAIL"
    Public Shared LOCALPHONE_ColumnName As String = "LOCAL PHONE"
    Public Shared DESIGNATION_ColumnName As String = "DESIGNATION"
    Public Shared COMPANYNAME_ColumnName As String = "COMPANY NAME"
    Public Shared FACULTY_OFFICE_ADDRESS_ColumnName As String = "FACULTY_OFFICE_ADDRESS"
    Public Shared INHOUSEFACULTY_ColumnName As String = "PRINT WITH MADHYAPRADESH" '1: FOR INHOUSE,2: FOR 




    Public Shared BANK_ColumnName As String = "Bank"
    Public Shared BRANCH_ColumnName As String = "Branch"
    Public Shared IFSC_ColumnName As String = "IFSC Code"
    Public Shared ACCOUNT_ColumnName As String = "Account No"
    Public Shared EMPLOYEE_COLUMN_NAME As String = "EMPLOYEEs"



    Public Shared COURSEDIR_FORMROLEID As Integer = 8

    Public Shared LEDGER_HONORARIUM_EXPENCES As String = "A2DC2EFD-EF08-4149-B30E-2FE490680812" '1
    Public Shared LEDGER_FOLDER_PEN_WRITING_PAD As String = "DB825E89-33CD-405B-BCDF-D2D052E2C6DD" '2
    Public Shared LEDGER_WORKING_LUNCH As String = "C09CCDF1-AA5E-4C8A-A967-3BC4EA19F6FC" '3
    Public Shared LEDGER_CLASS_ROOM_REFRESHMENT As String = "628788FD-BD7C-4C77-B3B0-E56116F655D7" '4
    Public Shared LEDGER_TRAVEL_ALLOWANCE As String = "46311CA8-204E-42C1-9880-B7735D62F5CC" '5
    Public Shared LEDGER_INAUGRATION_SESSION As String = "41C6A1C0-8C6D-47A8-84D4-7CC271E139FC" '6
    Public Shared LEDGER_VALIDATION_SESSION As String = "862BC18C-59B0-4BB9-A64B-C2E99E3A3262" '7
    Public Shared LEDGER_PHOTO_SESSION As String = "8588DD78-C38F-4D44-874A-C89FE239BBF5" '8
    Public Shared LEDGER_LOCAL_TRAVELLING_EXPENDITURE As String = "F2F2C38E-3AD1-4395-AC88-2F882F66E862" '9
    Public Shared LEDGER_FIELD_VISIT As String = "2D1F1836-4330-4F96-8498-2124191319F5" '10
    Public Shared LEDGER_OTHER_EXPENDITURE As String = "FEDB0943-9B42-4818-A195-268D99C78947" '11

    Public Shared LEDGER_READING_MATERIAL As String = "B729B38E-5189-4AD4-8B21-4D957148BF3D" '12
    Public Shared LEDGER_PHOTOCOPY_EXPENSES As String = "1145974A-156E-4E34-9CC8-2587E307AC89" '13
    Public Shared LEDGER_CLASS_ROOM_ELECTRICITY_WATER As String = "EFF31239-CF01-4826-81BE-ABF5BB907E49" '14
    Public Shared LEDGER_CLERICAL_EXPENSES As String = "04B0FD13-15E4-4B68-BC3D-A107F9A7AD1D" '15
    Public Shared LEDGER_LIBRARY_CHARGES As String = "78C3A394-1D28-4158-A9E8-56B73E938B52" '16


    Public Shared LEDGER_EQUIPMENT_CHARGES As String = "08173DD4-DD9F-45D8-9468-DD9D2A297526" '17
    Public Shared LEDGER_OTHER_OVERHEAD_CHARGES As String = "8629350B-6A90-4EE5-95F2-3A28F285B5ED" '18
    Public Shared LEDGER_CLOTH_WASHIG_EXPENSES As String = "564783E3-C9C9-49E2-8757-27BC6C7B3413" '19
    Public Shared LEDGER_HOSTEL_MAINTENANCE As String = "ADA9FCEF-1CAF-4727-A583-63B8394A01B9" '20
    Public Shared LEDGER_SPORTS_EXP As String = "E69965FD-0956-49FC-B0EA-A7EA180E73F9" '21
    Public Shared DEVELOPMENT_FUND As String = "89DA0ABA-881D-4E94-B827-99DA82018841" '21

    Public Shared CASH_IN_HAND As String = "A3719B05-B8B9-44BC-87CA-943C14F7F627"
    Public Shared VoucherTypeId_ReceiptType As String = "AB08C3A4-D60E-4434-8236-7CEBCCA7B171"
    '-----CR-------------------
    Public Shared CategoryId_TRAINING As String = "1F5DC703-5916-4CCD-8DE3-678E0424F525" ', '
    Public Shared LedgerId_EXPENSES_PAYABLE As String = "BDB68405-E1F5-486C-B9AB-CBCB58113044" ' EXPENSES PAYABLE'
    Public Shared LedgerId_COURSE_SAVING As String = "6AC0D968-E080-4761-B6AF-508B40715D4D" ',@LedgerName=N'COURSE SAVING'
    '------------Desk_Assistant_Action--------------------------------------------
    Public Shared CONFIG_ID_FeedBackReport_Joint_Dept As String = "F63E8531-C202-4F4D-92D8-E162BBD48B0C"
    Public Shared CONFIG_ID_FeedBacksummary_Joint_Dept As String = "8BE96E50-E464-46C4-B3D9-A3BA603DEE9F"
    Public Shared CONFIG_ID_FeedBackGOI_Joint_Dept As String = "9E6A8634-9E7D-4C63-A982-2BB09495CDC0"

    Public Shared InteractionwithtraineesId As String = "FE7E5525-FD97-4331-A18F-98470C56546C"
    Public Shared feeDbackid1 As String = "93642884-218B-43AD-9E5B-93172F9E8B4E"
    Public Shared feeDbackid2 As String = "67A65FFD-32E3-49E6-ACA7-CAFAC6FEC734"
    Public Shared Intro_TopicID As String = "F94BE516-F550-40DA-9389-0A06E4947E5E" '/*पंजीयन एंव परिचय */,
    Public Shared Feedback_TopicID As String = "6EA423B7-AFB3-42D1-8AF6-4AF2BA0B41C3" '/*फीडबैक एवं समापन */,    

    Public Shared FacultybyPresentationId As String = "7A6BB4FF-D8E7-4BC2-91F1-1AF50B1CB041"
    Public Shared FacultybySubjectKnowledgeId As String = "71872B1C-8A31-49CE-A15E-2EAB8A9EF0F9"
    Public Shared feedbacktype3Id As String = "4DF1A52E-F767-4056-B29C-120AB08DCF1A"
    Public Shared Desk_Value_For_TimeTable_Print As Integer = "1"
    Public Shared Desk_Value_For_TraineeLetter_Print As Integer = "2"

    Public Shared Desk_Value_For_FacultyLetter_Print As Integer = "3"


    Public Shared Desk_Value_For_TraineeAttendence_print As Integer = "4"
    Public Shared Desk_Value_For_TraineeAttendence_Register_Print As Integer = "5"

    Public Shared Desk_Value_For_KitDistribution_Print As Integer = "6"
    Public Shared Desk_Value_For_HonoraruimLetter_Print As Integer = "7"
    Public Shared TRAINING_BRANCHID As String = "DFF7C661-5B84-4A7E-8250-31C420DD9FCD"
    Public Shared CONFIG_ID_Kitdistribution_Joint_Dept As String = "721EB10C-DC90-4A98-B1F6-BDDED5945682"
    Public Shared CONFIG_ID_attendanceReport_Joint_Dept As String = "3909C0CA-3C02-4EEA-8A7E-EF4B9A92E900"
    Public Shared CONFIG_ID_FacultyLetter_Joint_Dept As String = "5BB7FA83-4758-49EE-942C-B133B1CFB648"

    Public Shared PROFORMA_BILLTYPE As Byte = 1
    Public Shared FINAL_BILLTYPE As Byte = 2
    Public Shared SUNDRY_DEBTORS_LEDGER As String = "91CA7D72-9B02-4916-96B7-64BCA2F674D4"
    Public Shared CONSULTANCY_INCOME_LEDGER As String = "3B264D36-E564-4460-B82F-5EC6B61C2BC4" 'CONSULTANCYINCOME
    Public Shared DEBTORS_LEDGERTYPEID As String = "A50F1622-1E7F-47F0-89E0-D474A3313A32"
    Public Shared TRAINING_ADVANCELEDGERID As String = "B25890E7-2E5C-406F-9DC0-789A21DB73AF"
    Public Shared TRAINING_EXPENSES_LEDGERTYPEID As String = "0B961DB1-2F84-458F-BF36-6D14DAF365DB"
    Public Shared VoucherTypeId_BillType As String = "17D34460-BC60-46F9-B7EC-DB885D2FAA55"

    Public Shared PRITNwithMP_HINDI As String = "मध्यप्रदेश शासन"
    Public Shared PRITNwithINDIA_HINDI As String = "भारत शासन"
    Public Shared HTRAINING_AMOUNT_PAYABLE As String = "महानिदेशक आर.सी.वी.पी.नरोन्हा प्रशासन एवं प्रबंधकीय अकादमी मध्यप्रदेश,भोपाल"
    Public Shared API_KEY As String = "EQZBGPECQEIUUDQTBXCHBGEQZBGPECQEIUUDQTBXCHBGEQZBGPECQEIUUDQTBXCHBGECQEIUUDQTBXCHBGECQEIUUDQTBXCHBGEQZBGP"
    Public Shared API_KEY_VALIDITY As String = "10/04/2022"
    Public Shared API_TEST_KEY As String = "1234"
    Public Shared API_TEST_KEY_VALIDITY As String = "12/04/2018"

    Public Shared TRG_COURSE_MASTER_FORM_ID As String = "503"
    Public Shared TRG_SPONSOR_DEPARTMENT_MASTER_FORM_ID As String = "9368"                            'Added by Sarvjeet on 23.05.2018
    Public Shared TRG_SPONSOR_MASTER_FORM_ID As String = "511"                                        'Added by Sarvjeet on 23.05.2018
    Public Shared TRG_HONORARIUM_RATE_FORM_ID As String = "530"                                       'Added by Sarvjeet on 23.05.2018
    Public Shared TRG_ITEM_MASTER_FORM_ID As String = "9432"                                          'Added by Sarvjeet on 24.05.2018
    Public Shared TRG_ROOM_MASTER_FORM_ID As String = "8940"                                          'Added by Sarvjeet on 25.05.2018
    Public Shared TRG_HOSTEL_MASTER_FORM_ID As String = "504"                                         'Added by Sarvjeet on 25.05.2018
    Public Shared TRG_TRAINING_HALL_FORM_ID As String = "501"                                         'Added by Sarvjeet on 25.05.2018
    Public Shared TRG_EXP_NOTESHEET_FORM_ID As String = "545"                                         'Added by Sarvjeet on 25.05.2018
    Public Shared TRG_HONORARIUM_PAYMENT_ORDER_FORM_ID = "9378"                                       'Added by Sarvjeet on 28.05.2018
    Public Shared TRG_ROOM_RESERVATION_FORM_ID = "2020"                                               'Added by Sarvjeet on 29.05.2018
    Public Shared TRG_RECEIPT = "2020"

    Public Shared TRG_CANCEL_ROOM_RESERVATION_FORM_ID = "2021"                                          'Added by Sarvjeet on 06.06.2018
    Public Shared TRG_TRAINING_CALENDAR_FORM_ID = "514"                                                 'Added by Sarvjeet on 05.06.2018
    Public Shared TRG_TRAINING_NODAL_OFFICER_FORM_ID = "8902"                                           'Added by Sarvjeet on 06.06.2018
    Public Shared TRG_PROPOSAL_COURSE_FORM_ID = "585"                                                   'Added by Sarvjeet on 08.06.2018
    Public Shared TRG_DELEGATED_DEPARTMENT_FORM_ID = "510"                                              'Added by Sarvjeet on 08.06.2018
    Public Shared TRG_TRAINING_PROPOSAL_FORM_ID = "9363"                                                'Added by Sarvjeet on 09.06.2018
    Public Shared TRG_GUEST_FACULTY_FORM_ID = "505"                                                     'Added by Sarvjeet on 09.06.2018
    Public Shared TRG_ROOM_CHECKIN_FORM_ID = "2022"                                                     'Added by Sarvjeet on 10.06.2018
    Public Shared TRG_ROOM_CHECKOUT_FORM_ID = "2009"                                                    'Added by Sarvjeet on 13.06.2018
    Public Shared TRG_BILL_CANCELLATION_FORM_ID = "570"                                                 'Added by Sarvjeet on 13.06.2018
    Public Shared TRG_RECEIPT_CANCEL_FORM_ID = "571"                                                    'Added by Sarvjeet on 13.06.2018
    Public Shared TRG_CANCEL_TRANSFERRED_RECEIPT_AMOUNT_FORM_ID = "2024"                                'Added by Sarvjeet on 14.06.2018
    Public Shared TRG_SHOW_PARTICIPANT_FORM_ID = "565"                                                  'Added by Sarvjeet on 14.06.2018
    Public Shared TRG_CERTIFICATE_NOTESHEET_FORM_ID = "576"                                             'Added by Sarvjeet on 14.06.2018
    Public Shared TRG_ITEM_GROUP_MASTER_FORM_ID = "9434"                                                'Added by Sarvjeet on 15.06.2018
    Public Shared TRG_ROOM_TYPE_FORM_ID = "508"                                                         'Added by Sarvjeet on 15.06.2018
    Public Shared TRG_TRAINING_TYPE_FORM_ID = "513"                                                     'Added by Sarvjeet on 15.06.2018
    Public Shared TRG_REQUISITIONSLIP_FOR_TRAINING_KIT_FORM_ID = "9425"                                 'Added by Sarvjeet on 18.06.2018
    Public Shared TRG_REQSLIP_FOR_ELEC_ITEM_FORM_ID = "9426"                                            'Added by Sarvjeet on 18.06.2018
    Public Shared TRG_TRAINING_TOUR_PROPOSAL_FORM_ID = "9429"                                           'Added by Sarvjeet on 18.06.2018
    Public Shared TRG_NOMINATION_APPROVAL_DASHBOARD_FORM_ID = "8906"                                    'Added by Sarvjeet on 19.06.2018
    Public Shared TRG_TRAINEE_ATTENDANCE_FORM_ID = "2003"                                               'Added by Sarvjeet on 19.06.2018
    Public Shared TRG_HONORARIUM_PAYMENT_PROFORMA_FORM_ID = "527"                                       'Added by Sarvjeet on 19.06.2018
    Public Shared TRG_TRAINEE_ATTENDACE_REGISTER_FORM_ID = "9397"                                       'Added by Sarvjeet on 19.06.2018
    Public Shared TRG_TRANSFER_RECEIPT_AMOUNT_FORM_ID = "2023"                                          'Added by Sarvjeet on 20.06.2018
    Public Shared TRG_BILL_REMINDER_FORM_ID = "552"                                                     'Added by Sarvjeet on 20.06.2018
    Public Shared TRG_UPDATE_RECEIPT_FORM_ID = "593"                                                    'Added by Sarvjeet on 20.06.2018
    Public Shared TRG_COLLECTION_REGISTER_FORM_ID = "2017"                                              'Added by Sarvjeet on 21.06.2018
    Public Shared TRG_CANCEL_HALL_RESERVATION_FORM_ID = "582"                                           'Added by Sarvjeet on 22.06.2018
    Public Shared TRG_HALL_CHECKIN_CHECKOUT_FORM_ID = "9413"                                            'Added by Sarvjeet on 23.06.2018
    Public Shared TRG_HALL_RESERVATION_FORM_ID = "9415"                                                 'Added by Sarvjeet on 23.06.2018
    'Public Shared TRG_SPORTS_KIT_FORM_ID = "9422"                                                      'Added by Sarvjeet on 25.06.2018
    Public Shared TRG_DAILY_ROOM_STATUS_FORM_ID = "590"                                                 'Added by Sarvjeet on 26.06.2018
    Public Shared TRG_TRAINEE_REGISTRATION_FORM_ID = "591"                                              'Added by Sarvjeet on 26.06.2018
    Public Shared TRG_HONORARIUM_RECEIPT_REPRINT_FORM_ID = "2002"                                       'Added by Sarvjeet on 26.06.2018
    Public Shared TRG_TRAINEE_ATTENDANCE_REPORT_FORM_ID = "9384"                                        'Added by Sarvjeet on 26.06.2018
    Public Shared TRG_WEEKLY_TRAINING_CALENDAR_FORM_ID = "546"
    Public Shared TRG_DAYS_IN_WEEK = 7
    Public Shared TRG_DAYS_IN_MONTH = 30
    Public Shared SuperAdmin_Agencyid = "6DA19908-EA60-4FBF-BBE0-60B07E4CC5D4"
    ' Public Shared PortalAdmin_Agencyid = "7AA59FB2-9524-4A17-9AA2-51FDB9FD0603"
    Public Shared PortalAdmin_Agencyid = "06076C68-EEDD-4373-B24E-602AA93E7421"
    Public Shared DefaultDate As DateTime = Convert.ToDateTime("1900-01-01 00:00:00.000")


    Public Function Get_Common_Data(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable) As DataSet
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Dim dsItemGroup As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.CommandText = ProcedureName
        If Not dtparameterlist Is Nothing Then
            Dim j As Integer
            For i = 0 To dtparameterlist.Rows.Count - 1
                For j = 0 To dtparameterlist.Columns.Count - 1
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                        If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString = "NULL" Then
                            cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                        Else
                            cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString)
                        End If

                    End If
                Next
            Next
        End If


        logger.Error("DataManager - Get_Common_Data->Start Connection " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dsItemGroup)
        logger.Error("DataManager - Get_Common_Data-> End Connection " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
        con.Close()
        Return dsItemGroup
    End Function
    '    Public Function LoadTrainingPlanData(ByVal Domain As String, ByVal isOnline As String, ByVal createdby As String, _
    'ByVal AgencyTypeId As String, _
    'ByVal CourseId As String, _
    'ByVal LocationId As String, _
    'ByVal Tilldate As DateTime, _
    'ByVal TrainingCategoryId As String, _
    'ByVal HallID As String, _
    'ByVal HostelID As String, _
    'ByVal topicID As String, _
    'ByVal FacultyID As String) As DataSet
    '        Dim ds As New DataSet
    '        con = Get_Connection_String(Domain, IsOnline)
    '        con.Open()
    '        cmd = New SqlCommand("TrainingPlan.TP_LoadTrainingPlanData", con)
    '        cmd.CommandType = CommandType.StoredProcedure
    '        cmd.CommandTimeout = 5000
    '        If createdby Is Nothing Then
    '            cmd.Parameters.AddWithValue("@createdby", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@createdby", createdby)
    '        End If

    '        If AgencyTypeId Is Nothing Then
    '            cmd.Parameters.AddWithValue("@AgencyTypeId", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@AgencyTypeId", AgencyTypeId)
    '        End If
    '        If CourseId Is Nothing Then
    '            cmd.Parameters.AddWithValue("@CourseId", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@CourseId", CourseId)
    '        End If
    '        If LocationId Is Nothing Then
    '            cmd.Parameters.AddWithValue("@LocationId", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@LocationId", LocationId)
    '        End If
    '        If Tilldate = Nothing Then
    '            cmd.Parameters.AddWithValue("@Tilldate", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@Tilldate", Tilldate)
    '        End If
    '        If TrainingCategoryId Is Nothing Then
    '            cmd.Parameters.AddWithValue("@TrainingCategoryId", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@TrainingCategoryId", TrainingCategoryId)
    '        End If
    '        If HallID Is Nothing Then
    '            cmd.Parameters.AddWithValue("@HallID", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@HallID", HallID)
    '        End If
    '        If HostelID Is Nothing Then
    '            cmd.Parameters.AddWithValue("@HostelID", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@HostelID", HostelID)
    '        End If
    '        If topicID Is Nothing Then
    '            cmd.Parameters.AddWithValue("@topicID", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@topicID", topicID)
    '        End If
    '        If FacultyID Is Nothing Then
    '            cmd.Parameters.AddWithValue("@FacultyID", DBNull.Value)
    '        Else
    '            cmd.Parameters.AddWithValue("@FacultyID", FacultyID)
    '        End If

    '        Dim daKit As SqlDataAdapter = New SqlDataAdapter(cmd)
    '        daKit.Fill(ds)
    '        con.Close()
    '        Return ds
    '    End Function
    '    Public Function GetCategory(ByVal Domain As String, ByVal isOnline As String, ByVal SchemeId As String) As DataTable
    '        Dim dtSchemes As New DataTable
    '        con = Get_Connection_String(Domain, IsOnline)
    '        con.Open()
    '        cmd = New SqlCommand("[TrainingPlan].GetTrainingCategory", con)
    '        cmd.CommandType = CommandType.StoredProcedure
    '        cmd.Connection = con
    '        cmd.CommandTimeout = 5000
    '        cmd.Parameters.AddWithValue("@SchemeId", SchemeId)
    '        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
    '        daSchemes.Fill(dtSchemes)
    '        con.Close()
    '        Return dtSchemes
    '    End Function
    Public Function GetTraningCode(ByVal Domain As String, ByVal isonline As String, ByVal TrainingDate As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("Select TrainingPlan.[F_GETTrainingCode] (@TrainingDate)", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.Add("@TrainingDate", SqlDbType.DateTime).Value = TrainingDate
        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function
    Public Function CheckTraningCourseCode(ByVal Domain As String, ByVal isonline As String, ByVal TrainingCode As String, ByVal TrainingDate As String) As Boolean
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim isExists As Boolean
        cmd = New SqlCommand("Select TrainingPlan.[F_CheckTrainingCourseCode] (@TrainingCode, @TrainingDate) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingCode", TrainingCode)
        cmd.Parameters.Add("@TrainingDate", SqlDbType.DateTime).Value = TrainingDate
        isExists = cmd.ExecuteScalar()
        con.Close()
        Return isExists
    End Function
    Public Function GetTrainingAmount(ByVal Domain As String, ByVal isonline As String, ByVal TrainingCategoryId As String, ByVal courseid As String, ByVal TrainingType As String, ByVal ProposedCandidate As String, ByVal TrainingId As String, ByVal EffectiveDate As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingAmount As New DataTable
        cmd = New SqlCommand("Select * from TrainingPlan.F_GetTrainingAmount_new (@TrainingCategoryId, @courseid, @TrainingType,@ProposedCandidate,@TrainingId,@EffectiveDate) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingCategoryId", TrainingCategoryId)
        cmd.Parameters.AddWithValue("@courseid", courseid)
        cmd.Parameters.AddWithValue("@TrainingType", TrainingType)
        cmd.Parameters.AddWithValue("@ProposedCandidate", ProposedCandidate)

        If TrainingId Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        End If
        cmd.Parameters.AddWithValue("@EffectiveDate", EffectiveDate)
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtTrainingAmount)
        con.Close()
        Return dtTrainingAmount
    End Function
    'Public Function InsUpdTainingPlan(ByVal Domain As String, ByVal isonline As String, ByVal TrainingPlanId As String, ByVal RevisedPlanOf As String, ByVal TrainingPlanno As String, ByVal LedgerId As String, ByVal PlaceId As String, ByVal ExecutingAgency As String, ByVal TrainingName As String, ByVal TrainingDetails As String, ByVal StartDate As String, ByVal CompletionPeriod As String, ByVal CompletionType As String, ByVal Contingency As String, ByVal AdminSno As String, ByVal AdminSanctionDate As String, ByVal AdminSanctionAmount As String, ByVal AdminSanctionXml As String, ByVal TechSno As String, ByVal TechnicalSanctionDate As String, ByVal TechnicalSanctionAmount As String, ByVal TechnicalSanctionXml As String, ByVal FinancialSno As String, ByVal FinancialSanctionDate As String, ByVal FinancialSanctionAmount As String, ByVal FinancialSanctionXml As String, ByVal SponsorID As String, ByVal SponsorName As String, ByVal SponsoredAmt As String, ByVal CourseId As String, ByVal ParticipantLevel As String, ByVal ParticipantCategoryID As String, ByVal PhyUnit As String, ByVal CourseDirector As String, ByVal AssociateDirector As String, ByVal HallType As String, ByVal HallNo As String, ByVal Rent As String, ByVal TrainingType As String, ByVal HostelNo As String, ByVal ManualFileNo As String, ByVal TrainingScheduleXml As String, ByVal CreatedBy As String, ByVal BranchId As String, ByVal TrainingExpendData As String, ByVal TrainingDisbursementData As String, ByVal TrainingCategoryid As String, ByVal TrainingCourseCode As String, ByVal DepartmentReferenceNo As String, ByVal ClosingDate As String, ByVal CategoryDetailID As String, ByVal DeptXML As String, ByVal Training_SponsorType As String, ByVal ProposalID As String, ByVal fixvariableexpenditure As String) As Boolean
    '    con = Get_Connection_String(Domain, isonline)
    '    con.Open()
    '    cmd = New SqlCommand("TrainingPlan.InsUpdTrainingPlan", con)
    '    cmd.Parameters.AddWithValue("@TrainingPlanId ", TrainingPlanId)
    '    If TrainingPlanno Is Nothing Then
    '        cmd.Parameters.AddWithValue("@TrainingPlanno ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@TrainingPlanno ", TrainingPlanno)
    '    End If
    '    If ProposalID Is Nothing Then
    '        cmd.Parameters.AddWithValue("@TrainingProposalid ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@TrainingProposalid ", ProposalID)
    '    End If

    '    If TrainingPlanno Is Nothing Then
    '        cmd.Parameters.AddWithValue("@TrainingCode ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@TrainingCode ", TrainingCourseCode)
    '    End If

    '    If LedgerId Is Nothing Then
    '        cmd.Parameters.AddWithValue("@LedgerId ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@LedgerId ", LedgerId)
    '    End If
    '    If PlaceId Is Nothing Then
    '        cmd.Parameters.AddWithValue("@PlaceId ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@PlaceId", PlaceId)
    '    End If
    '    If ExecutingAgency Is Nothing Then
    '        cmd.Parameters.AddWithValue("@ExecutingAgency", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@ExecutingAgency", ExecutingAgency)
    '    End If

    '    cmd.Parameters.AddWithValue("@TrainingName", TrainingName)
    '    cmd.Parameters.AddWithValue("@TrainingDetails", TrainingDetails)
    '    cmd.Parameters.AddWithValue("@StartDate", StartDate)
    '    cmd.Parameters.AddWithValue("@ClosingDate", ClosingDate)
    '    cmd.Parameters.AddWithValue("@CompletionPeriod", CompletionPeriod)
    '    cmd.Parameters.AddWithValue("@CompletionType ", CompletionType)
    '    cmd.Parameters.AddWithValue("@Contingency ", Contingency)
    '    cmd.Parameters.AddWithValue("@AdminSno", AdminSno)

    '    If Not AdminSanctionDate = "#12:00:00 AM#" AndAlso Not AdminSanctionDate = Nothing Then
    '        cmd.Parameters.Add(New SqlParameter("@AdminSanctionDate", AdminSanctionDate))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@AdminSanctionDate", DBNull.Value))
    '    End If
    '    If AdminSanctionAmount = 0 Then
    '        cmd.Parameters.Add(New SqlParameter("@AdminSanctionAmount", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@AdminSanctionAmount", AdminSanctionAmount))
    '    End If
    '    If AdminSanctionXml Is Nothing Then
    '        cmd.Parameters.Add(New SqlParameter("@AdminSanctionXml", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@AdminSanctionXml", AdminSanctionXml))
    '    End If


    '    cmd.Parameters.AddWithValue("@TechSno ", TechSno)
    '    cmd.Parameters.AddWithValue("@Training_SponsorType ", Training_SponsorType)


    '    If Not TechnicalSanctionDate = "#12:00:00 AM#" And Not TechnicalSanctionDate = Nothing Then
    '        cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionDate", TechnicalSanctionDate))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionDate", DBNull.Value))
    '    End If
    '    If TechnicalSanctionAmount = 0 Then
    '        cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionAmount", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionAmount", TechnicalSanctionAmount))
    '    End If
    '    'cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionAmount", TechAmount))
    '    If TechnicalSanctionXml Is Nothing Then
    '        cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionXml", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionXml", TechnicalSanctionXml))
    '    End If
    '    If CategoryDetailID Is Nothing OrElse CategoryDetailID.ToString = "" Then
    '        cmd.Parameters.Add(New SqlParameter("@CategoryDetailID", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@CategoryDetailID", CategoryDetailID))
    '    End If

    '    cmd.Parameters.AddWithValue("@FinancialSno", FinancialSno)

    '    If Not FinancialSanctionDate = "#12:00:00 AM#" AndAlso Not FinancialSanctionDate = Nothing Then
    '        cmd.Parameters.Add(New SqlParameter("@FinancialSanctionDate", FinancialSanctionDate))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@FinancialSanctionDate", DBNull.Value))
    '    End If
    '    If FinancialSanctionAmount = 0 Then
    '        cmd.Parameters.Add(New SqlParameter("@FinancialSanctionAmount", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@FinancialSanctionAmount", FinancialSanctionAmount))
    '    End If
    '    'cmd.Parameters.Add(New SqlParameter("@FinancialSanctionAmount", FinancialAmount))
    '    If FinancialSanctionXml Is Nothing Then
    '        cmd.Parameters.Add(New SqlParameter("@FinancialSanctionXml", DBNull.Value))
    '    Else
    '        cmd.Parameters.Add(New SqlParameter("@FinancialSanctionXml", FinancialSanctionXml))
    '    End If
    '    cmd.Parameters.Add(New SqlParameter("@DataColXML", TrainingExpendData))
    '    cmd.Parameters.Add(New SqlParameter("@DataColXML1", TrainingDisbursementData))
    '    cmd.Parameters.AddWithValue("@PhyUnit ", PhyUnit)
    '    If SponsorID Is Nothing Then
    '        cmd.Parameters.AddWithValue("@SponsorID ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@SponsorID ", SponsorID)
    '    End If
    '    If SponsorID Is Nothing Then
    '        cmd.Parameters.AddWithValue("@SponsorName ", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@SponsorName ", SponsorName)
    '    End If
    '    If DeptXML Is Nothing Then
    '        cmd.Parameters.AddWithValue("@TrainingDepartmentXml", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@TrainingDepartmentXml", DeptXML)
    '    End If

    '    cmd.Parameters.AddWithValue("@SponsoredAmt ", SponsoredAmt)
    '    cmd.Parameters.AddWithValue("@CourseId ", CourseId)
    '    cmd.Parameters.AddWithValue("@ParticipantCategoryId", ParticipantCategoryID)
    '    cmd.Parameters.AddWithValue("@ParticipantLevel", ParticipantLevel)
    '    cmd.Parameters.AddWithValue("@DepartmentReferenceNo", DepartmentReferenceNo)
    '    cmd.Parameters.AddWithValue("@CourseDirector", CourseDirector)
    '    cmd.Parameters.AddWithValue("@AssociateDirector", AssociateDirector)
    '    If HallNo Is Nothing Then
    '        cmd.Parameters.AddWithValue("@HallNo", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@HallNo", HallNo)
    '    End If
    '    cmd.Parameters.AddWithValue("@HallType", HallType)
    '    cmd.Parameters.AddWithValue("@Rent ", Rent)
    '    cmd.Parameters.AddWithValue("@TrainingType ", TrainingType)
    '    If HostelNo Is Nothing Then
    '        cmd.Parameters.AddWithValue("@HostelNo", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@HostelNo ", HostelNo)
    '    End If
    '    If TrainingCategoryid Is Nothing Then
    '        cmd.Parameters.AddWithValue("@TrainingCategoryid", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@TrainingCategoryid", TrainingCategoryid)
    '    End If
    '    cmd.Parameters.AddWithValue("@ManualFileNo", ManualFileNo)
    '    cmd.Parameters.AddWithValue("@TrainingScheduleXml ", TrainingScheduleXml)
    '    cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
    '    cmd.Parameters.AddWithValue("@BranchId", BranchId)
    '    cmd.Parameters.AddWithValue("@exptype", fixvariableexpenditure)
    '    cmd.CommandType = CommandType.StoredProcedure
    '    cmd.CommandTimeout = 5000
    '    cmd.ExecuteNonQuery()
    '    con.Close()
    '    Return True
    'End Function
    Public Function GetTraningPlanNo(ByVal Domain As String, ByVal isonline As String, ByVal TrainingDate As DateTime, ByVal CategoryId As String, ByVal BranchId As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("Select TrainingPlan.[GenTrainingNo] (@TrainingDate, @CategoryId, @BranchId) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.Add("@TrainingDate", SqlDbType.DateTime).Value = TrainingDate
        cmd.Parameters.AddWithValue("@CategoryId", CategoryId)
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo

    End Function
    Public Function Save_Common_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString)
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function
    Public Function GetDataTable(ByVal str As String) As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        'str = "[{PARTICIPANTID:2d5e07bd-2bd8-32c5-35ec-6744baa0bce7,PARTICIPANTNAME:PRINCE  ,HPARTICIPANTNAME:PRINCE  ,SALUTATION:1,F_NAME:PRINCE,L_NAME:,M_NAME:,GENDER:1,AGE:50,DESIGNATIONNAME:se,CURRENTLOCATION:bhopal,MOBILENO:343344424,EMAIL:p@gmail.com,WITHCHILD:0,NOOFPERSON:1,IS_BHOPAL:0},{PARTICIPANTID:8de44179-1aff-ca45-50ac-2ce08eee7bfc,PARTICIPANTNAME:RAJKUMAR KUMAR ,HPARTICIPANTNAME:RAJKUMAR KUMAR ,SALUTATION:2,F_NAME:RAJKUMAR,L_NAME:,M_NAME:KUMAR,GENDER:0,AGE:20,DESIGNATIONNAME:pe,CURRENTLOCATION:indore,MOBILENO:3435543345,EMAIL:g@gmail.com,WITHCHILD:1,NOOFPERSON:2,IS_BHOPAL:0},{PARTICIPANTID:75617866-7e22-ac9d-aaff-9ea93c8bb2f4,PARTICIPANTNAME:RAJA  JAIN,HPARTICIPANTNAME:RAJA  JAIN,SALUTATION:1,F_NAME:RAJA,L_NAME:JAIN,M_NAME:,GENDER:1,AGE:40,DESIGNATIONNAME:de,CURRENTLOCATION:jbp,MOBILENO:3453454353,EMAIL:l@gmail.com,WITHCHILD:0,NOOFPERSON:1,IS_BHOPAL:0}]"

        ' str = "[{PARTICIPANTID:b4497fe1-35c0-424a-9e7f-0006bd81203a$PARTICIPANTNAME:PRINCC,E  $HPARTICIPANTNAME:PRINCC,E  $SALUTATION:1$F_NAME:PRINCC,E$L_NAME:$M_NAME:$GENDER:1$AGE:30$DESIGNATIONNAME:43$CURRENTLOCATION:34$MOBILENO:5845454545$EMAIL:p@gmail.com$WITHCHILD:0$NOOFPERSON:1$IS_BHOPAL:0}$01]"
        If (str.Contains("$") = True) Then
            str = str.Replace(",", "~")
            str = str.Replace("$", ",")
        End If

        Dim sptstr() As String = str.Split("},")
        For i = 0 To sptstr.Length - 1

            sptstr(i) = sptstr(i).Replace("{", "")
            sptstr(i) = sptstr(i).Replace("[", "")
            sptstr(i) = sptstr(i).Replace("}", "")
            sptstr(i) = sptstr(i).Replace("]", "")

            If sptstr(i) = "" Then
                Continue For
            End If
            Dim sptField() As String = sptstr(i).Split(",")
            If i = 0 Then
                For j = 0 To sptField.Length - 1
                    Dim sptcol = sptField(j).Split(":")
                    dt.Columns.Add(sptcol(0).ToString())
                Next

            End If
            dr = dt.NewRow
            For j = 0 To sptField.Length - 1
                If sptField(j) = "" Then
                    Continue For
                End If

                Dim sptcol = sptField(j).Split(":")
                If sptcol(1).ToString() <> "" Then
                    dr(sptcol(0).ToString()) = sptcol(1).Replace("~", ",")
                Else
                    dr(sptcol(0).ToString()) = DBNull.Value
                End If


            Next
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
    Public Function Get_CreatedOn_Server(ByVal Domain As String, ByVal Isonline As String) As String
        Dim Createdon_New As String          '' To hold Createdon where time in 24 hours format.
        Dim SplitStartDate_New() As String  '' To hold split current date as array
        Dim T_StartDate_New As Date         '' To hold current date without time
        Dim Hours As Integer                '' To hold only hours of time.
        Dim Str As String = "00"            '' To hold value if time(Hour) in 12 AM
        Dim GetOnlyTime() As String         '' To hold split current Time as array

        SplitStartDate_New = getDateTime.ToString.Split(" ")
        T_StartDate_New = SplitStartDate_New(0)
        If SplitStartDate_New.Length > 2 Then
            If SplitStartDate_New(2).ToString.ToUpper = "PM" Then
                GetOnlyTime = SplitStartDate_New(1).Split(":")
                Hours = Convert.ToInt32(GetOnlyTime(0))
                If Hours <> 12 Then
                    Hours = (12 + Hours)
                End If
                Createdon_New = (Get_DB_Date(T_StartDate_New.ToString("dd/MM/yyyy"))).ToString("yyyy/MM/dd") + " " + Convert.ToString(Hours) + ":" + GetOnlyTime(1) + ":" + GetOnlyTime(2)
            Else
                GetOnlyTime = SplitStartDate_New(1).Split(":")
                Hours = Convert.ToInt32(GetOnlyTime(0))
                Createdon_New = (Get_DB_Date(T_StartDate_New.ToString("dd/MM/yyyy"))).ToString("yyyy/MM/dd") + " " + Convert.ToString(Hours) + ":" + GetOnlyTime(1) + ":" + GetOnlyTime(2)
                If Hours = 12 Then
                    Createdon_New = (Get_DB_Date(T_StartDate_New.ToString("dd/MM/yyyy"))).ToString("yyyy/MM/dd") + " " + Str + ":" + GetOnlyTime(1) + ":" + GetOnlyTime(2)
                End If
            End If
        Else
            Createdon_New = (Get_DB_Date(T_StartDate_New.ToString("dd/MM/yyyy"))).ToString("yyyy/MM/dd") + " " + SplitStartDate_New(1)
        End If
        Return Createdon_New
    End Function
    Public Shared Function Get_DB_Date(ByVal DtToCheck As String) As Date
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try

            Dim dDate As Date
            Dim gb As CultureInfo
            Dim Fdate As String
            Fdate = FormatDate(DtToCheck.Trim())
            gb = New CultureInfo("en-GB")
            dDate = DateTime.Parse(Fdate, gb).Date
            'dDate = Format(dDate, "yyyy/MM/dd")
            'added and commented by pradeep u to get date in YYYY-MM-dd format in place of YYYY/MM/dd
            dDate = Format(dDate, "yyyy-MM-dd")
            Return dDate
            ''Dim dDate As Date
            ''Dim gb As CultureInfo
            ''gb = New CultureInfo("fr-FR")
            ''dDate = DateTime.Parse(DtToCheck.Trim(), gb).Date
            ''Return dDate

        Catch ex As Exception
            logger.Error("DataManager - Get_DB_Date" + ex.Message)
            If DtToCheck.ToString <> "" Then
                'Throw New Exception("Incorrect Date Format Date Must be in (dd/mm/yyyy) Format.")

                Throw New Exception("1")
            End If

        End Try

#Disable Warning BC42353 ' Function 'Get_DB_Date' doesn't return a value on all code paths. Are you missing a 'Return' statement?
    End Function
#Enable Warning BC42353 ' Function 'Get_DB_Date' doesn't return a value on all code paths. Are you missing a 'Return' statement?
    Public Shared Function FormatDate(ByVal RequestString As String) As String
        Dim ResultDateString As String = Nothing
        Dim str() As String
        Dim car As String
        car = "/"

        If (RequestString.Contains(car)) Then
            str = RequestString.Split(car)
        Else
            car = "-"
            str = RequestString.Split("-")
        End If

        If Not str.Length = 0 Then
            If (str(0).Length = 4) Then
                If (Convert.ToInt16(str(1).ToString() > 12)) Then
                    ResultDateString = str(1).ToString() + car + str(2).ToString() + car + str(0).ToString()
                Else
                    ResultDateString = str(2).ToString() + car + str(1).ToString() + car + str(0).ToString()
                End If
            Else
                If (Convert.ToInt16(str(1).ToString() > 12)) Then
                    ResultDateString = str(1).ToString() + car + str(0).ToString() + car + str(2).ToString()
                Else
                    ResultDateString = str(0).ToString() + car + str(1).ToString() + car + str(2).ToString()
                End If

            End If

        End If


        Return ResultDateString
    End Function

    Public Shared ReadOnly Property getDateTime() As DateTime
        Get
            'Return DateTime.UtcNow.AddHours(5).AddMinutes(30)
            Dim dDate As Date
            dDate = DateTime.UtcNow.AddHours(5).AddMinutes(30)
            'Dim gb As CultureInfo
            'gb = New CultureInfo("pt-BR")
            'dDate = DateTime.Parse(dDate, gb).Date
            'dDate = DateTime.ParseExact(dDate.ToString(), "dd/MM/yyyy", DBNull.Value)
            'dDate1 = Format(dDate, "dd/MM/yyyy") 'cb swapnil becoz here  we dont  want set format in mm/dd/yyyy.  as per told azhar
            Return dDate

        End Get
    End Property
    Public Function GetUsercode(ByVal Domain As String, ByVal IsOnline As String, ByVal AgencyTypeId As String, ByVal BranchID As String) As String

        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand(" SELECT YUser.[F_GetUserCode]  ('" & AgencyTypeId & "','" & BranchID & "') ", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim Usercode As String = cmd.ExecuteScalar.ToString()
        con.Close()
        Return Usercode
    End Function
    Public Function InsUpdTainingPlan(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingPlanId As String, ByVal RevisedPlanOf As String, ByVal TrainingPlanno As String, ByVal LedgerId As String, ByVal PlaceId As String, ByVal ExecutingAgency As String, ByVal TrainingName As String, ByVal TrainingDetails As String, ByVal StartDate As String, ByVal CompletionPeriod As String, ByVal CompletionType As String, ByVal Contingency As String, ByVal AdminSno As String, ByVal AdminSanctionDate As String, ByVal AdminSanctionAmount As String, ByVal AdminSanctionXml As String, ByVal TechSno As String, ByVal TechnicalSanctionDate As String, ByVal TechnicalSanctionAmount As String, ByVal TechnicalSanctionXml As String, ByVal FinancialSno As String, ByVal FinancialSanctionDate As String, ByVal FinancialSanctionAmount As String, ByVal FinancialSanctionXml As String, ByVal SponsorID As String, ByVal SponsorName As String, ByVal SponsoredAmt As String, ByVal CourseId As String, ByVal ParticipantLevel As String, ByVal ParticipantCategoryID As String, ByVal PhyUnit As String, ByVal CourseDirector As String, ByVal AssociateDirector As String, ByVal HallType As String, ByVal HallNo As String, ByVal Rent As String, ByVal TrainingType As String, ByVal HostelNo As String, ByVal ManualFileNo As String, ByVal TrainingScheduleXml As String, ByVal CreatedBy As String, ByVal BranchId As String, ByVal TrainingExpendData As String, ByVal TrainingDisbursementData As String, ByVal TrainingCategoryid As String, ByVal TrainingCourseCode As String, ByVal DepartmentReferenceNo As String, ByVal ClosingDate As String, ByVal CategoryDetailID As String, ByVal DeptXML As String, ByVal Training_SponsorType As String, ByVal ProposalID As String, ByVal fixvariableexpenditure As String, ByVal createdbyempid As String, ByVal forwardedempid As String, ByVal tat_typeid As String, ByVal docstatus As String, ByVal procedurefor As String) As DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim dttrainingdetail As New DataTable
        cmd = New SqlCommand("TrainingPlan.InsUpdTrainingPlan", con)
        cmd.Parameters.AddWithValue("@TrainingPlanId ", TrainingPlanId)
        If TrainingPlanno Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingPlanno ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingPlanno ", TrainingPlanno)
        End If
        If ProposalID Is Nothing Then
            If System.Web.HttpContext.Current.Session("proposalid_on_TP") Is Nothing Then
                cmd.Parameters.AddWithValue("@TrainingProposalid ", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@TrainingProposalid ", System.Web.HttpContext.Current.Session("proposalid_on_TP"))
            End If

        Else
            cmd.Parameters.AddWithValue("@TrainingProposalid ", ProposalID)
        End If

        If TrainingCourseCode Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingCode ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingCode ", TrainingCourseCode)
        End If

        If LedgerId Is Nothing Then
            cmd.Parameters.AddWithValue("@LedgerId ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@LedgerId ", LedgerId)
        End If
        If PlaceId Is Nothing Then
            cmd.Parameters.AddWithValue("@PlaceId ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@PlaceId", PlaceId)
        End If
        If ExecutingAgency Is Nothing Then
            cmd.Parameters.AddWithValue("@ExecutingAgency", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ExecutingAgency", ExecutingAgency)
        End If

        cmd.Parameters.AddWithValue("@TrainingName", TrainingName)
        cmd.Parameters.AddWithValue("@TrainingDetails", TrainingDetails)
        cmd.Parameters.AddWithValue("@StartDate", StartDate)
        cmd.Parameters.AddWithValue("@ClosingDate", ClosingDate)
        cmd.Parameters.AddWithValue("@CompletionPeriod", CompletionPeriod)
        cmd.Parameters.AddWithValue("@CompletionType ", CompletionType)
        cmd.Parameters.AddWithValue("@Contingency ", Contingency)
        cmd.Parameters.AddWithValue("@AdminSno", AdminSno)
        If Not AdminSanctionDate = "#12:00:00 AM#" AndAlso Not AdminSanctionDate = Nothing Then
            cmd.Parameters.Add(New SqlParameter("@AdminSanctionDate", AdminSanctionDate))
        Else
            cmd.Parameters.Add(New SqlParameter("@AdminSanctionDate", DBNull.Value))
        End If
        If AdminSanctionAmount = 0 Then
            cmd.Parameters.Add(New SqlParameter("@AdminSanctionAmount", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@AdminSanctionAmount", AdminSanctionAmount))
        End If
        If AdminSanctionXml Is Nothing Then
            cmd.Parameters.Add(New SqlParameter("@AdminSanctionXml", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@AdminSanctionXml", AdminSanctionXml))
        End If
        cmd.Parameters.AddWithValue("@TechSno ", TechSno)
        cmd.Parameters.AddWithValue("@Training_SponsorType ", Training_SponsorType)
        If Not TechnicalSanctionDate = "#12:00:00 AM#" And Not TechnicalSanctionDate = Nothing Then
            cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionDate", TechnicalSanctionDate))
        Else
            cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionDate", DBNull.Value))
        End If
        If TechnicalSanctionAmount = 0 Then
            cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionAmount", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionAmount", TechnicalSanctionAmount))
        End If

        If TechnicalSanctionXml Is Nothing Then
            cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionXml", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@TechnicalSanctionXml", TechnicalSanctionXml))
        End If
        If CategoryDetailID Is Nothing OrElse CategoryDetailID.ToString = "" Then
            cmd.Parameters.Add(New SqlParameter("@CategoryDetailID", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@CategoryDetailID", CategoryDetailID))
        End If

        cmd.Parameters.AddWithValue("@FinancialSno", FinancialSno)

        If Not FinancialSanctionDate = "#12:00:00 AM#" AndAlso Not FinancialSanctionDate = Nothing Then
            cmd.Parameters.Add(New SqlParameter("@FinancialSanctionDate", FinancialSanctionDate))
        Else
            cmd.Parameters.Add(New SqlParameter("@FinancialSanctionDate", DBNull.Value))
        End If
        If FinancialSanctionAmount = 0 Then
            cmd.Parameters.Add(New SqlParameter("@FinancialSanctionAmount", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@FinancialSanctionAmount", FinancialSanctionAmount))
        End If

        If FinancialSanctionXml Is Nothing Then
            cmd.Parameters.Add(New SqlParameter("@FinancialSanctionXml", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@FinancialSanctionXml", FinancialSanctionXml))
        End If
        cmd.Parameters.Add(New SqlParameter("@DataColXML", TrainingExpendData))
        cmd.Parameters.Add(New SqlParameter("@DataColXML1", TrainingDisbursementData))
        cmd.Parameters.AddWithValue("@PhyUnit ", PhyUnit)
        If SponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorID ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorID ", SponsorID)
        End If
        If SponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorName ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorName ", SponsorName)
        End If
        If DeptXML Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingDepartmentXml", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingDepartmentXml", DeptXML)
        End If

        cmd.Parameters.AddWithValue("@SponsoredAmt ", SponsoredAmt)
        cmd.Parameters.AddWithValue("@CourseId ", CourseId)
        cmd.Parameters.AddWithValue("@ParticipantCategoryId", ParticipantCategoryID)
        cmd.Parameters.AddWithValue("@ParticipantLevel", ParticipantLevel)
        cmd.Parameters.AddWithValue("@DepartmentReferenceNo", DepartmentReferenceNo)
        cmd.Parameters.AddWithValue("@CourseDirector", CourseDirector)
        cmd.Parameters.AddWithValue("@AssociateDirector", AssociateDirector)
        If HallNo Is Nothing Then
            cmd.Parameters.AddWithValue("@HallNo", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@HallNo", HallNo)
        End If
        cmd.Parameters.AddWithValue("@HallType", HallType)
        cmd.Parameters.AddWithValue("@Rent ", Rent)
        cmd.Parameters.AddWithValue("@TrainingType ", TrainingType)
        If HostelNo Is Nothing Then
            cmd.Parameters.AddWithValue("@HostelNo", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@HostelNo ", HostelNo)
        End If
        If TrainingCategoryid Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingCategoryid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingCategoryid", TrainingCategoryid)
        End If
        cmd.Parameters.AddWithValue("@ManualFileNo", ManualFileNo)
        cmd.Parameters.AddWithValue("@TrainingScheduleXml ", TrainingScheduleXml)
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        cmd.Parameters.AddWithValue("@exptype", fixvariableexpenditure)

        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", forwardedempid)
        cmd.Parameters.AddWithValue("@tat_type_id", tat_typeid)
        cmd.Parameters.AddWithValue("@doc_status", docstatus)
        cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dttrainingdetail)
        con.Close()
        Return dttrainingdetail
    End Function

    Public Function Get_Notesheet_data_new(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal Procedurefor As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_trainingexp_notesheet_dynamic]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        cmd.Parameters.AddWithValue("@procedurefor", Procedurefor)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function SaveTrainingExpNotesheet(ByVal Domain As String, ByVal isOnline As String, ByVal Branchid As String, ByVal createdby As String, ByVal ExpNotesheetXML As String) As Boolean
        'already save = 1 and new=0
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[SaveTrainingExpNotesheet]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@BranchId", Branchid)
        cmd.Parameters.AddWithValue("@CreatedBy", createdby)
        cmd.Parameters.AddWithValue("@ExpNote_XML", ExpNotesheetXML)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function CheckExpenditureNotesheet(ByVal Domain As String, ByVal isonline As String, ByVal TrainingId As String) As Boolean
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim isExists As Boolean
        cmd = New SqlCommand("Select TrainingPlan.F_CheckExpenditureNotesheet (@TrainingId) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        isExists = cmd.ExecuteScalar()
        con.Close()
        Return isExists
    End Function

    Public Function encryptPassword(ByVal password As String) As String
        Dim encPwd As Byte() = Encoding.UTF8.GetBytes(password)
        Dim sha1 As HashAlgorithm = HashAlgorithm.Create("SHA1")
        Dim pp As Byte() = sha1.ComputeHash(encPwd)
        Dim sb As StringBuilder = New StringBuilder()

        For Each b As Byte In pp
            sb.Append(b.ToString("x2"))
        Next

        Return sb.ToString()
    End Function
    Protected Function hashGenerator(ByVal Username As String, ByVal sender_id As String, ByVal message As String, ByVal secure_key As String) As String
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append(Username).Append(sender_id).Append(message).Append(secure_key)
        Dim genkey As Byte() = Encoding.UTF8.GetBytes(sb.ToString())
        Dim sha1 As HashAlgorithm = HashAlgorithm.Create("SHA512")
        Dim sec_key As Byte() = sha1.ComputeHash(genkey)
        Dim sb1 As StringBuilder = New StringBuilder()

        For i As Integer = 0 To sec_key.Length - 1
            sb1.Append(sec_key(i).ToString("x2"))
        Next

        Return sb1.ToString()
    End Function
    'Public Function sendsingleSMS(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileno As String, ByVal message As String, ByVal securekey As String) As String
    '    Dim datastream As Stream
    '    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12


    '    Dim request As HttpWebRequest = CType(WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest"), HttpWebRequest)
    '    request.ProtocolVersion = HttpVersion.Version10
    '    request.KeepAlive = False
    '    request.ServicePoint.ConnectionLimit = 1
    '    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"
    '    request.Method = "POST"
    '    Dim encryptedpassword As String = encryptPassword(password)
    '    Dim NewsecureKey As String = HashGenerator(username.Trim(), senderid.Trim(), message.Trim(), securekey.Trim())
    '    Dim smsservicetype As String = "singlemsg"
    '    Dim query As String = "username=" & HttpUtility.UrlEncode(username.Trim()) & "$password=" + HttpUtility.UrlEncode(encryptedpassword) & "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) & "&content=" + HttpUtility.UrlEncode(message.Trim()) & "&mobileno=" + HttpUtility.UrlEncode(mobileno) & "senderid=" + HttpUtility.UrlEncode(senderid.Trim()) & "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim())
    '    Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)
    '    request.ContentType = "application/x-www-form-urlencoded"
    '    request.ContentLength = byteArray.Length
    '    datastream = request.GetRequestStream()
    '    datastream.Write(byteArray, 0, byteArray.Length)
    '    datastream.Close()
    '    Dim response As WebResponse = request.GetResponse()
    '    Dim Status As String = (CType(response, HttpWebResponse)).StatusDescription
    '    datastream = response.GetResponseStream()
    '    Dim reader As StreamReader = New StreamReader(datastream)
    '    Dim responseFromServer As String = reader.ReadToEnd()
    '    reader.Close()
    '    datastream.Close()
    '    response.Close()
    '    Return responseFromServer

    'End Function

    Public Function CHECK_REGISTERED_MOBILE_NO(ByVal Domain As String, ByVal isonline As String, ByVal Mobileno As String, ByVal sponsorid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If sponsorid = "" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_get_tp_sponosr_ahuthorised_person_rmn] ('" + Mobileno + "',null) select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_get_tp_sponosr_ahuthorised_person_rmn] ('" + Mobileno + "','" + sponsorid + "') select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function chk_reg_mob_no(ByVal Domain As String, ByVal isonline As String, ByVal mobileno As String, ByVal sponsorid As String, ByVal trainingid As String, ByVal participantid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If participantid = "" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_get_participant_rmn] ('" + mobileno + "','" + trainingid + "','" + sponsorid + "',null) select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_get_participant_rmn] ('" + mobileno + "','" + trainingid + "','" + sponsorid + "','" + participantid + "') select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function
    Public Function getparticipants(ByVal Domain As String, ByVal isonline As String, ByVal trainingplanid As String, ByVal ParticipantId As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Or trainingplanid = "" Then
            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingid", trainingplanid)
        End If
        If ParticipantId Is Nothing Or ParticipantId = "" Then
            cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ParticipantId", ParticipantId)
        End If

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        '***********Process to add participant salutation name
        Dim dt1 As DataTable = dsSchemes.Tables(0)
        dt1.Columns.Add("ts_name")
        dt1.Columns.Add("ts_hname")
        Dim dtsalutation As DataTable
        dtsalutation = Get_Salutation(Domain, isonline)
        For Each dr As DataRow In dt1.Rows
            Dim dtfiltersal As DataTable
            If (dr("salutation").ToString <> "") Then
                dtsalutation.DefaultView.RowFilter = "ts_id='" + dr("salutation").ToString + "'"
                dtfiltersal = dtsalutation.DefaultView.ToTable
                If dtfiltersal.Rows.Count > 0 Then
                    dr("ts_name") = dtfiltersal.Rows(0)("ts_name").ToString()
                    dr("ts_hname") = dtfiltersal.Rows(0)("ts_hname").ToString()
                End If
            Else
                dr("ts_name") = ""
                dr("ts_hname") = ""
            End If

        Next




        Return dsSchemes
    End Function



    Public Function GetConfigurationDetail(ByVal Domain As String, ByVal isonline As String, ByVal CONFIGURATIONID As String, ByVal Branchid As String, ByVal Currentdate As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetconfigurationSettings", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If CONFIGURATIONID Is Nothing Then
            cmd.Parameters.AddWithValue("@CONFIGURATIONID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CONFIGURATIONID", CONFIGURATIONID)
        End If
        cmd.Parameters.AddWithValue("@Branchid", Branchid)

        If Currentdate = "" Then
            cmd.Parameters.AddWithValue("@Currentdate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Currentdate", Currentdate)
        End If
        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function getCertificate(ByVal Domain As String, ByVal isonline As String, ByVal ParticipantId As String, ByVal Trainingid As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.TP_GetCErtificate", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If ParticipantId Is Nothing Then
            cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ParticipantId", ParticipantId)
        End If
        cmd.Parameters.AddWithValue("@trainingid", Trainingid)
        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function

    Public Function GetTrainingCalenderFroPrint(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal CourseDirector As String, ByVal CourseID As String, ByVal SponsorId As String, ByVal orderby As Byte, ByVal TrainingStatus As String, ByVal TrainingCategory As String) As DataTable
        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetTrainingCalendar", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        If CourseID Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseID ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseID", CourseID)
        End If
        If CourseDirector Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirector ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirector", CourseDirector)
        End If
        If SponsorId Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorId ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorId", SponsorId)
        End If
        If TrainingStatus Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingStatus", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingStatus", TrainingStatus)
        End If
        If TrainingCategory Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingCategory", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingCategory", TrainingCategory)
        End If



        cmd.Parameters.AddWithValue("@orderby", orderby)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function GetTrainingCalender(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal CourseDirector As String, ByVal CourseID As String, ByVal SponsorId As String, ByVal Procedure As String, ByVal sortby As String) As DataTable

        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetTrainingCalendar", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        If CourseID = "" Then
            cmd.Parameters.AddWithValue("@CourseID ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseID", CourseID)
        End If
        If CourseDirector = "" Then
            cmd.Parameters.AddWithValue("@CourseDirector ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirector", CourseDirector)
        End If
        If SponsorId = "" Then
            cmd.Parameters.AddWithValue("@SponsorId ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorId", SponsorId)
        End If

        If Procedure = "" Then
            cmd.Parameters.AddWithValue("@Procedurefor", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Procedurefor", Procedure)
        End If
        If sortby = "" Then
            cmd.Parameters.AddWithValue("@Orderby ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Orderby", sortby)
        End If

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function
    Public Function CheckCourse(ByVal Domain As String, ByVal isonline As String, ByVal CourseCode As String, ByVal CourseName As String, ByVal HCourseName As String, ByVal duration As Integer, ByVal durationtype As Byte, ByVal coursecategory As String) As DataTable

        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingPlanno As New DataTable
        Dim da As New SqlDataAdapter("Trainingplan.TP_CheckCourse", con)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 5000
        da.SelectCommand.Parameters.AddWithValue("@CourseName", CourseName)
        da.SelectCommand.Parameters.AddWithValue("@CourseCode", CourseCode)
        da.SelectCommand.Parameters.AddWithValue("@HCourseName", HCourseName)
        da.SelectCommand.Parameters.AddWithValue("@duration", duration)
        da.SelectCommand.Parameters.AddWithValue("@durationtype", durationtype)
        da.SelectCommand.Parameters.AddWithValue("@coursecategory", coursecategory)
        da.Fill(dtTrainingPlanno)
        con.Close()
        Return dtTrainingPlanno
    End Function

    Public Function InsUpdAgency(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyName As String, ByVal HAgencyName As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal RoleID As String, ByVal ColumnValXML As String, ByVal UserCode As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("YUser.InsUpdAgencyNew", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyId", AgencyId)
        cmd.Parameters.AddWithValue("@AgencyName", AgencyName)
        cmd.Parameters.AddWithValue("@HAgencyName", HAgencyName)
        cmd.Parameters.AddWithValue("@AgencyTypeID", AgencyTypeID)
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@ParentID", ParentID)
        cmd.Parameters.AddWithValue("@RoleID", RoleID)
        cmd.Parameters.AddWithValue("@UserCode", UserCode)
        cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function GetTypeColumns(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyTypeId As String) As DataTable
        Dim dtTypeColumns As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("YUser.GetAgencyTypeDetail", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyTypeId", AgencyTypeId)
        cmd.Parameters.AddWithValue("@ColTy", "M")
        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dtTypeColumns)
        con.Close()
        Return dtTypeColumns
    End Function
    Public Function MakeDataTableforXML(ByVal DtAgencyTypeCOL As DataTable, ByVal TypeColId As String, ByVal ColValue As String) As DataTable
        Dim Dr As DataRow
        If DtAgencyTypeCOL Is Nothing Then
            DtAgencyTypeCOL = New DataTable
            DtAgencyTypeCOL.Columns.Add("AGENCYTYPECOLUMNID")
            DtAgencyTypeCOL.Columns.Add("COLUMNVALUE")
        End If
        Dr = DtAgencyTypeCOL.NewRow
        Dr.Item("AGENCYTYPECOLUMNID") = TypeColId
        Dr.Item("COLUMNVALUE") = ColValue
        DtAgencyTypeCOL.Rows.Add(Dr)
        Return DtAgencyTypeCOL
    End Function
    Public Function MakeDataTableforXML_Registration(ByVal DtAgencyTypeCOL As DataTable, ByVal TypeColId As String, ByVal ColValue As String) As DataTable
        Dim Dr As DataRow
        If DtAgencyTypeCOL Is Nothing Then
            DtAgencyTypeCOL = New DataTable
            DtAgencyTypeCOL.Columns.Add("TTFRD_AGENCY_TYPE_COL_ID")
            DtAgencyTypeCOL.Columns.Add("TTFRD_AGENCY_TYPE_COL_VAL")
        End If
        Dr = DtAgencyTypeCOL.NewRow
        Dr.Item("TTFRD_AGENCY_TYPE_COL_ID") = TypeColId
        Dr.Item("TTFRD_AGENCY_TYPE_COL_VAL") = ColValue
        DtAgencyTypeCOL.Rows.Add(Dr)
        Return DtAgencyTypeCOL
    End Function

    Public Function UpdateTrainingStatus(ByVal Domain As String, ByVal Isonline As String, ByVal TrainingId As String, ByVal TrainingStatus As String, ByVal StatusUpdateDate As String, ByVal createdby As String, ByVal branchid As String, ByVal StatusReason As String) As Boolean
        con = Get_Connection_String(Domain, Isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_UpdTrainingStatus", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.Parameters.AddWithValue("@TrainingStatus", TrainingStatus)
        cmd.Parameters.AddWithValue("@StatusUpdateDate", StatusUpdateDate)
        cmd.Parameters.AddWithValue("@CreatedBy", createdby)
        If StatusReason = "" Then
            cmd.Parameters.AddWithValue("@StatusReason", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@StatusReason", StatusReason)
        End If

        cmd.Parameters.AddWithValue("@BranchId", branchid)

        cmd.ExecuteNonQuery()
        con.Close()

        Return True


    End Function
    Public Function CheckDocumentStatus(ByVal Domain As String, ByVal Isonline As String, ByVal DocumentStatus As Byte, ByVal Trainingid As String, ByVal Actiondate As Date) As Boolean
        con = Get_Connection_String(Domain, Isonline)
        con.Open()
        cmd = New SqlCommand(" SELECT [TrainingPlan].[F_CheckDocumentStatus] ('" & Trainingid & "','" & DocumentStatus & "','" & Actiondate & "') ", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim Isexists As Boolean = cmd.ExecuteScalar.ToString()
        con.Close()
        Return Isexists
    End Function
    Public Function SaveNotesheet(ByVal Domain As String, ByVal Isonline As String, ByVal NotesheetId As String, ByVal TrainingId As String, ByVal NotesheetNo As String, ByVal Subject As String, ByVal NotesheetDescription As String, ByVal EmployeeId As String, ByVal DepartmentId As String, ByVal DesignationId As String, ByVal SectionId As String, ByVal DateOfSubmission As Date, ByVal CreatedBy As String, ByVal BranchId As String, ByVal DocumentType As Byte, ByVal HonorariumPayments_Xml As String) As Boolean
        con = Get_Connection_String(Domain, Isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.SaveNotesheet", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@NotesheetId", NotesheetId)
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.Parameters.AddWithValue("@NotesheetNo", NotesheetNo)
        cmd.Parameters.AddWithValue("@Subject", Subject)
        cmd.Parameters.AddWithValue("@NotesheetDescription", NotesheetDescription)
        cmd.Parameters.AddWithValue("@DocumentType", DocumentType)
        cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId)
        cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId)
        cmd.Parameters.AddWithValue("@DesignationId", DesignationId)
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        cmd.Parameters.AddWithValue("@SectionId", SectionId)
        cmd.Parameters.AddWithValue("@DateOfSubmission", DateOfSubmission)
        If HonorariumPayments_Xml Is Nothing Then
            cmd.Parameters.AddWithValue("@HonorariumPayments_Xml", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@HonorariumPayments_Xml", HonorariumPayments_Xml)
        End If

        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function GetTrainingCategory(ByVal Domain As String, ByVal Isonline As String, ByVal TrainingCategoryId As String) As DataSet
        Dim dsCourse As New DataSet
        con = Get_Connection_String(Domain, Isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetTrainingCategory", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If TrainingCategoryId Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingCategoryId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingCategoryId", TrainingCategoryId)
        End If
        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dsCourse)
        con.Close()
        Return dsCourse
    End Function
    Public Function GetTrainingPlanDetails(ByVal Domain As String, ByVal isOnline As String, TrainingId As String, ByVal WithParticipantDetails As String, ByVal WithExpenditureDetails As String) As DataSet
        Dim dsTrainingDetails As New DataSet
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_training_details", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        'cmd.Parameters.AddWithValue("@WithParticipantDetails", WithParticipantDetails)
        'cmd.Parameters.AddWithValue("@WithExpenditureDetails", WithExpenditureDetails)
        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dsTrainingDetails)
        con.Close()
        Return dsTrainingDetails
    End Function

    Public Function GetTrainingStatus(ByVal Domain As String, ByVal isOnline As String, ByVal sponsorID As String, ByVal CourseDirId As String, ByVal StatusreportOrder As String, ByVal Fromdate As String, ByVal todate As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_RptGetTrainingStatus", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If sponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorID", sponsorID)
        End If
        If CourseDirId Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirId", CourseDirId)
        End If
        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", todate)
        cmd.Parameters.AddWithValue("@StatusreportOrder", StatusreportOrder)
        da.Fill(dt)
        con.Close()
        Return dt
    End Function
    Public Function GetFees_due_collected_balance_report(ByVal Domain As String, ByVal isOnline As String, ByVal sponsorID As String, ByVal CourseDirId As String, ByVal StatusreportOrder As Byte, ByVal Fromdate As String, ByVal todate As String, ByVal ReportOrder As String) As DataSet

        ' Dim dt As New DataTable
        Dim ds As New DataSet

        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_Get_Due_Collected_balance_report", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If sponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorID", sponsorID)
        End If
        If CourseDirId Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirId", CourseDirId)
        End If
        cmd.Parameters.AddWithValue("@StatusreportOrder", StatusreportOrder)
        cmd.Parameters.AddWithValue("@Fromdate", Fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@ReportOrder", ReportOrder)
        '   da.Fill(dt)
        da.Fill(ds)
        con.Close()
        'Return dt
        Return ds
    End Function

    Public Function GetFeesStatus(ByVal Domain As String, ByVal isOnline As String, ByVal sponsorID As String, ByVal CourseDirId As String, ByVal StatusreportOrder As String, ByVal Fromdate As String, ByVal todate As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetBillStatusReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If sponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorID", sponsorID)
        End If
        If CourseDirId Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirId", CourseDirId)
        End If
        cmd.Parameters.AddWithValue("@Purpose", 2)
        cmd.Parameters.AddWithValue("@StatusreportOrder", StatusreportOrder)
        cmd.Parameters.AddWithValue("@Fromdate", Fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)

        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function GETtrainingParticipantReport(ByVal Domain As String, ByVal IsOnline As String, ByVal fromdt As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.Pro_get_TrainingParticipantDetail", con)
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdt", fromdt)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function Project_Receipt_And_Exp_details(ByVal Domain As String, ByVal IsOnline As String, ByVal sponsorID As String, ByVal CourseDirId As String, ByVal StatusreportOrder As String, ByVal Fromdate As String, ByVal todate As String, ByVal ReportOrder As String, ByVal schemeid As String, ByVal billtype As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_get_project_receipt_and_Exp_details", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If sponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorID", sponsorID)
        End If
        If CourseDirId Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirId", CourseDirId)
        End If

        cmd.Parameters.AddWithValue("@StatusreportOrder", StatusreportOrder)
        cmd.Parameters.AddWithValue("@Fromdate", Fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)

        cmd.Parameters.AddWithValue("@ReportOrder", ReportOrder)
        cmd.Parameters.AddWithValue("@schemeid", schemeid)
        cmd.Parameters.AddWithValue("@BillType", billtype)

        '---------------------------------------------------------------


        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetRoomStatus(ByVal Domain As String, ByVal IsOnline As String, ByVal statusdate As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim da As New SqlDataAdapter("TrainingPlan.TP_Proc_Rpt_DailyRoomStatus", con)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 5000
        da.SelectCommand.Parameters.Add(New SqlParameter("@Statusdate", statusdate))
        da.SelectCommand.CommandTimeout = 0
        da.Fill(dt)
        con.Close()
        Return dt


    End Function
    Public Function CheckBillStatus(ByVal Domain As String, ByVal IsOnline As String, ByVal Trainingid As String, ByVal Sponsorid As String) As Boolean
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        If Sponsorid Is Nothing Then
            cmd = New SqlCommand(" SELECT [TrainingPlan].[F_CheckBillStatus] ('" & Trainingid & "',NULL )", con)
        Else
            cmd = New SqlCommand(" SELECT [TrainingPlan].[F_CheckBillStatus] ('" & Trainingid & "','" & Sponsorid & "') ", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim Isexists As Boolean = cmd.ExecuteScalar.ToString()
        con.Close()
        Return Isexists
    End Function

    Public Function GetProposalDetails(ByVal Domain As String, ByVal IsOnline As String, ByVal Proposalid As String, ByVal forWhich As Byte, ByVal fromdate As Date, ByVal todate As Date) As DataTable
        Dim dtProposal As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[TP_GetProposalInfo]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Proposalid Is Nothing Then
            cmd.Parameters.AddWithValue("@ProposalID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ProposalID", Proposalid)
        End If
        If fromdate = Nothing Then
            cmd.Parameters.AddWithValue("@Fromdate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Fromdate", fromdate)
        End If
        If todate = Nothing Then
            cmd.Parameters.AddWithValue("@todate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@todate", todate)
        End If
        cmd.Parameters.AddWithValue("@forWhich", forWhich)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dtProposal)
        con.Close()
        Return dtProposal
    End Function

    Public Function GetPrintBill(ByVal Domain As String, ByVal IsOnline As String, ByVal SponsorBillid As String, ByVal BillType As Byte) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.TP_GetPrintBill", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If SponsorBillid Is Nothing OrElse SponsorBillid.Trim = "" Then
            cmd.Parameters.AddWithValue("@SponsorBillid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorBillid", SponsorBillid)
        End If
        cmd.Parameters.AddWithValue("@BillType", BillType)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetWorkReport(ByVal Domain As String, ByVal IsOnline As String, ByVal allotmentdate As String) As DataTable
        Dim dtWork As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.tp_getworkreport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daWorkMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
        cmd.Parameters.AddWithValue("@allotmentdate", allotmentdate)
        daWorkMaster.Fill(dtWork)
        con.Close()
        Return dtWork
    End Function

    Public Function Show_Error_Message(ByVal Domain As String, ByVal isOnline As String, ByVal sqlex As String, ByVal language As Int16) As String
        Dim strmessage As String = Nothing
        Dim dterrormsg As DataTable
        Dim dterrortbl As DataTable = Get_Error_msg_table(Domain, isOnline)
        If sqlex.Length <= 5 Then
            dterrortbl.DefaultView.RowFilter = "tem_id='" & sqlex & "'"
            dterrormsg = dterrortbl.DefaultView.ToTable()
            If dterrormsg.Rows.Count > 0 Then

                Select Case language
                    Case LanguageType.English
                        strmessage = dterrormsg.Rows(0).Item("tem_error").ToString()
                    Case LanguageType.Hindi
                        strmessage = dterrormsg.Rows(0).Item("tem_error").ToString()
                End Select

            Else
                'chk errorpage rights
                strmessage = sqlex
            End If
        Else
            'chk errorpage rights
            strmessage = sqlex
        End If
        Return strmessage
    End Function
    Public Function Show_Error_Message_Front_end(ByVal Domain As String, ByVal isOnline As String, ByVal exception As String, ByVal language As Int16) As String
        Dim strmessage As String = Nothing
        Dim dterrormsg As DataTable
        Dim dterrortbl As DataTable = Get_Error_msg_table(Domain, isOnline)

        If exception.Length <= 5 Then
            dterrortbl.DefaultView.RowFilter = "tem_id='" & exception & "'"
            dterrormsg = dterrortbl.DefaultView.ToTable()
            If dterrormsg.Rows.Count > 0 Then

                Select Case language
                    Case LanguageType.English
                        strmessage = "Error Code - " + exception.ToString + " : " + dterrormsg.Rows(0).Item("tem_error").ToString()
                    Case LanguageType.Hindi
                        strmessage = "एरर कोड - " + exception.ToString + " : " + dterrormsg.Rows(0).Item("tem_Herror").ToString()
                End Select


            Else
                dterrortbl.DefaultView.RowFilter = "tem_id='-1'"
                dterrormsg = dterrortbl.DefaultView.ToTable()
                If dterrormsg.Rows.Count > 0 Then
                    Select Case language
                        Case LanguageType.English
                            strmessage = "Error Code - " + exception.ToString + " : " + dterrormsg.Rows(0).Item("tem_error").ToString()
                        Case LanguageType.Hindi
                            strmessage = "एरर कोड - " + exception.ToString + " : " + dterrormsg.Rows(0).Item("tem_Herror").ToString()
                    End Select
                Else
                    strmessage = "Error Code - " + exception.ToString
                End If


            End If
        Else
            'chk errorpage rights
            strmessage = "Error Code - " + exception.ToString
        End If
        Return strmessage
    End Function
    Public Function Get_Error_msg_table(ByVal Domain As String, ByVal IsOnline As String) As DataTable

        Dim dt As New DataTable
        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "yuser_Proc_Get_Error_Table"
            cmd.Connection = con
            con.Open()
            Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dt)
            da.Dispose()
            con.Close()
        Else
            Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
            cnn.Open()

            Dim cmd As New SqlCommand("YUser.Proc_Get_Error_Table", cnn)

            cmd.CommandTimeout = 6000
            cmd.CommandType = CommandType.StoredProcedure


            Dim adp As New SqlDataAdapter(cmd)
            adp.Fill(dt)
            cnn.Close()
        End If

        Return dt
    End Function
    Public Function GetTraining_TTDetail(ByVal Domain As String, ByVal isOnline As String, ByVal Trainingcode As String, ByVal Purpose As String, ByVal fromdate As String, ByVal todate As String, ByVal Isforwarded As String) As DataSet
        Dim dsCourse As New DataSet
        con = Get_Connection_String(Domain, isOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.TP_GetTrainingPlanDetail", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Trainingcode Is Nothing Then
            cmd.Parameters.AddWithValue("@Trainingcode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Trainingcode", Trainingcode)
        End If
        cmd.Parameters.AddWithValue("@Purpose", Purpose)
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@Isforwarded", Isforwarded)

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dsCourse)
        con.Close()
        Return dsCourse
    End Function


    Public Function SEARCH_TRAINING_FACULTIES(ByVal Domain As String, ByVal isonline As String, ByVal AgencyTYpeid As String, ByVal searchtxt As String, ByVal SearchType As String) As DataTable
        'con = Get_Connection_String(Domain, isonline)
        'con.Open()
        Dim dt As New DataTable
        dt = GET_USER_AGENCY_DATA(Domain, isonline, "00054", Nothing)
        Dim dtfinal As New DataTable
        dtfinal.Columns.Add("GuestFacultyId")
        dtfinal.Columns.Add("GuestFacultyName")
        dtfinal.Columns.Add("HGuestFacultyName")
        dtfinal.Columns.Add("AgencyTypeId")
        dtfinal.Columns.Add("Per_Address")
        dtfinal.Columns.Add("Corr_Address")
        dtfinal.Columns.Add("Address2")
        dtfinal.Columns.Add("Address3")
        dtfinal.Columns.Add("MobileNo")
        dtfinal.Columns.Add("PhoneNo")
        dtfinal.Columns.Add("AAdharNo")
        dtfinal.Columns.Add("Subjectexpert")
        dtfinal.Columns.Add("Bank")
        dtfinal.Columns.Add("Branch")
        dtfinal.Columns.Add("accountno")
        dtfinal.Columns.Add("ifsc")
        dtfinal.Columns.Add("hsalutation")
        dtfinal.Columns.Add("salutation")
        dtfinal.Columns.Add("email")
        dtfinal.Columns.Add("salutationid")
        dtfinal.Columns.Add("isdisable")
        dtfinal.Columns.Add("staffid")
        For Each dr As DataRow In dt.Rows
            Dim dr1 As DataRow = dtfinal.NewRow()
            dr1("GuestFacultyId") = dr("agencyid")
            dr1("GuestFacultyName") = dr("AgencyName")
            dr1("HGuestFacultyName") = dr("HAgencyName")
            dr1("AgencyTypeId") = dr("tyaam_typeid")
            dr1("Per_Address") = dr("Ag_Address")
            dr1("Corr_Address") = dr("Ag_Address1")
            dr1("Address2") = dr("Ag_Address1")
            dr1("Address3") = dr("Ag_Address1")
            dr1("MobileNo") = dr("ag_mobileno")
            dr1("PhoneNo") = dr("ag_phone")
            dr1("AAdharNo") = dr("ag_aadhar")
            ' dr1("Subjectexpert") = dr("agencyid")
            'dr1("Bank") = dr("agencyid")
            'dr1("Branch") = dr("agencyid")
            'dr1("accountno") = dr("agencyid")
            ' dr1("ifsc") = dr("agencyid")
            dr1("hsalutation") = dr("agencyid")
            dr1("salutation") = dr("agencyid")
            dr1("email") = dr("agencyid")
            dr1("salutationid") = dr("ag_salutation")
            dr1("isdisable") = "0"
            dtfinal.Rows.Add(dr1)

        Next
        If searchtxt <> "NULL" Then
            dtfinal.DefaultView.RowFilter = "GuestFacultyName like '%" + searchtxt + "%'"
            dtfinal = dtfinal.DefaultView.ToTable
        End If

        'If searchtxt = "NULL" Then
        '    cmd = New SqlCommand("select * from [TrainingPlan].[F_search_faculty]  ('" + AgencyTYpeid + "',NULL,'" + SearchType + "') ", con)
        'Else
        '    cmd = New SqlCommand("select * from [TrainingPlan].[F_search_faculty]  ('" + AgencyTYpeid + "',N'" + searchtxt + "','" + SearchType + "') ", con)
        'End If

        'cmd.CommandType = CommandType.Text
        'cmd.CommandTimeout = 5000

        'Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        'daCourse.Fill(dt)
        'con.Close()
        Return dtfinal
    End Function

    Public Function GetHonoraruiamRecieptForPrint(ByVal Domain As String, ByVal IsOnline As String, ByVal HonPayRegId As String) As DataTable
        Dim dtCourse As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_Hono_Receipts_reprint", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If HonPayRegId Is Nothing Then
            cmd.Parameters.AddWithValue("@HonPayRegId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@HonPayRegId", HonPayRegId)
        End If

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dtCourse)
        con.Close()
        Return dtCourse
    End Function
    Public Function SEARCH_TRAINING_Subject(ByVal Domain As String, ByVal isonline As String, ByVal searchtxt As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dt As New DataTable
        If searchtxt = "NULL" Then
            cmd = New SqlCommand("select * from [TrainingPlan].[F_search_subject]  (NULL) ", con)
        Else
            cmd = New SqlCommand("select * from [TrainingPlan].[F_search_subject]  (N'" + searchtxt + "') ", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function CheckFacultyExist(ByVal Domain As String, ByVal IsOnline As String, ByVal Trainingid As String, ByVal FacultyId As String) As Boolean

        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand(" SELECT [TrainingPlan].[F_CheckFaculty] (@Trainingid,@FacultyId) ", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@Trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Trainingid", Trainingid)
        End If
        If FacultyId Is Nothing Then
            cmd.Parameters.AddWithValue("@FacultyId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@FacultyId", FacultyId)
        End If
        Dim Isexists As Boolean = cmd.ExecuteScalar.ToString()
        con.Close()
        Return Isexists
    End Function


    Public Function InsUpdAgency_Faculty(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyName As String, ByVal HAgencyName As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal RoleID As String, ByVal ColumnValXML As String, ByVal UserCode As String, ByVal isdisabled As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("YUser.InsUpdAgencyNew", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyId", AgencyId)
        cmd.Parameters.AddWithValue("@AgencyName", AgencyName)
        If HAgencyName.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@HAgencyName", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@HAgencyName", HAgencyName)
        End If

        cmd.Parameters.AddWithValue("@AgencyTypeID", AgencyTypeID)
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@ParentID", ParentID)
        cmd.Parameters.AddWithValue("@RoleID", RoleID)
        cmd.Parameters.AddWithValue("@UserCode", UserCode)
        cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        cmd.Parameters.AddWithValue("@isdisable", isdisabled)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function InsUpdFacultyCourses(ByVal Domain As String, ByVal isOnline As String, ByVal FacultyId As String, ByVal CourseDetails As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_InsUpdFacultyCourses", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@FacultyId", FacultyId)
        If Not CourseDetails Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDetails", CourseDetails)
        Else
            cmd.Parameters.AddWithValue("@CourseDetails", DBNull.Value)
        End If

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function MakeDt(ByVal DtAgencyCOL As DataTable, ByVal colID As String, ByVal ColName As String, ByVal ColValue As String) As DataTable
        Dim Dr As DataRow
        If DtAgencyCOL Is Nothing Then
            DtAgencyCOL = New DataTable
            DtAgencyCOL.Columns.Add("COLUMNID")
            DtAgencyCOL.Columns.Add("COLUMNName")
            DtAgencyCOL.Columns.Add("COLUMNVALUE")
        End If
        Dr = DtAgencyCOL.NewRow
        Dr.Item("COLUMNID") = colID
        Dr.Item("COLUMNName") = ColName
        Dr.Item("COLUMNVALUE") = ColValue
        DtAgencyCOL.Rows.Add(Dr)

        Return DtAgencyCOL
    End Function
    Public Function InsUpdCourseDetails(ByVal Domain As String, ByVal isOnline As String, ByVal CourseId As String, ByVal CourseName As String, ByVal HCourseName As String, ByVal CourseCode As String, ByVal CourseDuration As String, ByVal CreatedBy As String, ByVal BranchId As String, ByVal CourseDetails As String, ByVal DurationType As String, ByVal isActive As String, ByVal coursecategory As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[TP_InsUpdCourse]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@CourseId", CourseId)
        cmd.Parameters.AddWithValue("@CourseName", CourseName)
        cmd.Parameters.AddWithValue("@HCourseName", HCourseName)
        If CourseCode Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseCode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseCode", CourseCode)
        End If
        If Not CourseDuration Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDuration", CourseDuration)
        End If
        If Not DurationType Is Nothing Then
            cmd.Parameters.AddWithValue("@DurationType", DurationType)
        End If

        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        If CourseDetails Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDetails", DBNull.Value)
        Else

            cmd.Parameters.AddWithValue("@CourseDetails", CourseDetails)
        End If

        cmd.Parameters.AddWithValue("@isactive", isActive)

        cmd.Parameters.AddWithValue("@coursecategory", coursecategory)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function GetFacultyData(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyTypeId As String, ByVal withStandardColumns As Byte, ByVal Agencyid As String, ByVal isDisableRequired As String) As DataSet
        Dim dsAgencies As New DataSet
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("YUser.GetDynamicAgencyReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Agencyid.ToString.ToUpper = "NULL" Then
            cmd.Parameters.AddWithValue("@AgencyId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@AgencyId", Agencyid)
        End If


        cmd.Parameters.AddWithValue("@AgencyTypeId", AgencyTypeId)
        cmd.Parameters.AddWithValue("@withStandardColumns", withStandardColumns)
        cmd.Parameters.AddWithValue("@reqdisable", isDisableRequired)
        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dsAgencies)
        con.Close()
        Return dsAgencies
    End Function



    Public Function GET_TRAINING_Details(ByVal Domain As String, ByVal isonline As String, ByVal trainingcode As String, ByVal employeeid As String, ByVal Doctypeid As String, ByVal isForwarded As String, ByVal FromDate As String, ByVal ToDate As String, ByVal BranchID As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dt As New DataTable

        cmd = New SqlCommand("Select * from  [TRAININGPLAN].[f_tp_get_training_no]  (N'" + trainingcode + "','" + employeeid + "'," + Doctypeid + "," + isForwarded + ",N'" + FromDate + "',N'" + ToDate + "','" + BranchID + "') ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function CHECK_Mobile_No_For_Agency(ByVal Domain As String, ByVal isonline As String, ByVal Mobileno As String, ByVal AgencyTypeID As String, ByVal columnid As String, ByVal Agencyid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If Agencyid.ToString = "NULL" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_check_mobile_no_for_agencytype] ('" + Mobileno + "','" + AgencyTypeID + "','" + columnid + "',null) select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_check_mobile_no_for_agencytype] ('" + Mobileno + "','" + AgencyTypeID + "','" + columnid + "','" + Agencyid + "') select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function TRG_SEARCH_COURSE(ByVal Domain As String, ByVal isonline As String, ByVal searchtxt As String, ByVal coursecategory As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dt As New DataTable
        If searchtxt = "NULL" Then
            cmd = New SqlCommand("select * from [TrainingPlan].[F_search_coursename] (NULL,NULL) ", con)
        Else
            If coursecategory = "NULL" Then
                cmd = New SqlCommand("select * from [TrainingPlan].[F_search_coursename] (N'" + searchtxt + "',NULL) ", con)
            Else
                cmd = New SqlCommand("select * from [TrainingPlan].[F_search_coursename] (N'" + searchtxt + "','" + coursecategory + "') ", con)
            End If

        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function TRG_getparticipants(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingplanid As String, ByVal ParticipantId As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Then
            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingid", trainingplanid)
        End If
        If ParticipantId Is Nothing Then
            cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ParticipantId", ParticipantId)
        End If

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function

    Public Function GetSponsorWiseDetail(ByVal Domain As String, ByVal IsOnline As String, ByVal sponsarid As String, ByVal fdate As String, ByVal tdate As String, ByVal isCancel As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_get__bill_details", con)
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        If sponsarid = "" Then
            cmd.Parameters.AddWithValue("@sponsorid", DBNull.Value)

        Else
            cmd.Parameters.AddWithValue("@sponsorid", sponsarid)
        End If

        ' cmd.Parameters.AddWithValue("@sponsorid", sponsarid)
        cmd.Parameters.AddWithValue("@billfdate", fdate)
        cmd.Parameters.AddWithValue("@billtdate", tdate)
        cmd.Parameters.AddWithValue("@iscancel", isCancel)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dt)
        con.Close()
        Return dt
    End Function


    Public Function GetweeklyDetail(ByVal Domain As String, ByVal IsOnline As String, ByVal fromdate As String) As DataSet
        Dim dsTrainingDetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("Trainingplan.proc_get_weekly_training_details", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)

        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dsTrainingDetails)
        con.Close()
        Return dsTrainingDetails
    End Function



    Public Function Save_TT_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal xmldata As String, ByVal CreatedonParam As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString)
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        'Dim sptXMLName() As String = XMLName.Split(",")

        'For i = 0 To dtXMLparameterlist.Rows.Count - 1
        '    For j = 0 To dtXMLparameterlist.Columns.Count - 1
        '        Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
        '        If xmlpara.ToString = "" Then
        '            Continue For
        '        End If

        '        Dim dttempxml As DataTable
        '        Dim xmldata As String
        '        If xmlpara IsNot Nothing Then
        '            'dttempxml = DerializeDataTable(xmlpara)
        '            dttempxml = GetDataTable(xmlpara)
        '            If dttempxml.Rows.Count > 0 Then
        '                Dim wr As New StringWriter
        '                dttempxml.TableName = sptXMLName(j)
        '                dttempxml.WriteXml(wr)
        '                xmldata = wr.ToString()
        '            End If

        '        Else
        '            xmldata = Nothing
        '        End If
        '        cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
        '    Next
        'Next
        cmd.Parameters.AddWithValue("@TrainingScheduleXML", xmldata)

        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function

    Public Function GetTraining_TIME_TABLE(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal timetableid As String) As DataSet


        Dim ds As New Data.DataSet
        Dim cnn As SqlConnection = Get_Connection_String(Domain, isOnline)
        cnn.Open()
        Dim da As New SqlDataAdapter("[TrainingPlan].[proc_tp_print_trg_time_table]", cnn)
        da.SelectCommand.CommandType = Data.CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 8000
        If Not trainingid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", trainingid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", DBNull.Value)
        End If
        If Not timetableid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@timetableid", timetableid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@timetableid", DBNull.Value)
        End If

        da.Fill(ds)
        cnn.Close()
        Return ds



    End Function
    Public Function CheckHonorariumRate(ByVal Domain As String, ByVal isOnline As String, ByVal HonorariumRateID As String) As Boolean
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("Select TrainingPlan.[F_CheckHonorariumRate] (@HonorariumRateID)", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@HonorariumRateID", HonorariumRateID)
        Dim IsExists As Boolean = cmd.ExecuteScalar()
        con.Close()
        Return IsExists
    End Function

    Public Function GetweeklyCalender(ByVal Domain As String, ByVal isOnline As String, ByVal fromDate As String, ByVal toDate As String, ByVal CourseDirectorID As String, ByVal SponsorId As String) As DataSet
        Dim dsTrainingDetails As New DataSet

        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetWeeklyTrainingCalendar", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromDate", fromDate)
        cmd.Parameters.AddWithValue("@toDate", toDate)
        If CourseDirectorID Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirector", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirector", CourseDirectorID)
        End If
        If SponsorId Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorId", SponsorId)
        End If

        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dsTrainingDetails)
        con.Close()
        Return dsTrainingDetails
    End Function

    Public Function GetVehcialMaintanceReport(ByVal Domain As String, ByVal isOnline As String, ByVal VehicleId As String, ByVal Branchid As String, ByVal Month As Integer, ByVal F_year As String) As DataTable
        Dim dtVehical As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.[TP_GetVehcileMaintance]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If VehicleId Is Nothing Then
            cmd.Parameters.AddWithValue("@VehicleId", DBNull.Value)

        Else
            cmd.Parameters.AddWithValue("@VehicleId", VehicleId)

        End If
        cmd.Parameters.AddWithValue("@Branchid", Branchid)
        cmd.Parameters.AddWithValue("@Month", Month)
        cmd.Parameters.AddWithValue("@F_year", F_year)
        cmd.Parameters.AddWithValue("@ForReport", 1)

        Dim daWorkMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
        daWorkMaster.Fill(dtVehical)
        con.Close()
        Return dtVehical
    End Function

    Public Function GetWeeklyTrainingReport(ByVal Domain As String, ByVal isOnline As String, ByVal finyear As String, ByVal month As String) As DataSet

        Dim dsWeeklyTraining As New DataSet

        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("yuser.proc_getWeekErpData", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@finyear", finyear)
        If month = Nothing Then
            cmd.Parameters.AddWithValue("@month", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@month", month)

        End If




        Dim dareport As SqlDataAdapter = New SqlDataAdapter(cmd)

        dareport.Fill(dsWeeklyTraining)
        con.Close()

        Return dsWeeklyTraining
    End Function

    Public Function GetVehcial_fuelExpensesReport(ByVal Domain As String, ByVal isOnline As String, ByVal VehicleId As String, ByVal Month As Integer, ByVal F_year As String) As DataTable
        Dim dtVehical As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.[TP_GetFuelExpensesReport]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If VehicleId Is Nothing Then
            cmd.Parameters.AddWithValue("@VehicleId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@VehicleId", VehicleId)

        End If

        cmd.Parameters.AddWithValue("@Month", Month)
        cmd.Parameters.AddWithValue("@F_year", F_year)

        Dim daWorkMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
        daWorkMaster.Fill(dtVehical)
        con.Close()
        Return dtVehical
    End Function

    Public Function Get_chequeRegister(ByVal Domain As String, ByVal isOnline As String, ByVal fromdate As String, ByVal Todate As String, ByVal RptType As String, ByVal sponsorid As String) As DataTable
        Dim dtFee As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetChequeRegister", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daWorkMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
        cmd.Parameters.AddWithValue("@Fromdt", fromdate)
        cmd.Parameters.AddWithValue("@Todt", Todate)
        cmd.Parameters.AddWithValue("@RptType", RptType)

        If sponsorid Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorID", sponsorid)

        End If


        daWorkMaster.Fill(dtFee)
        con.Close()
        Return dtFee
    End Function

    Public Function GetFee_cashRegister(ByVal Domain As String, ByVal isOnline As String, ByVal fromdate As String, ByVal Todate As String, ByVal sponsorid As String) As DataTable
        Dim dtFee As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetFee_CashRegister", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daWorkMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
        cmd.Parameters.AddWithValue("@Fromdt", fromdate)
        cmd.Parameters.AddWithValue("@Todt", Todate)
        If sponsorid Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorID", sponsorid)

        End If
        daWorkMaster.Fill(dtFee)
        con.Close()
        Return dtFee
    End Function

    Public Function GetTrainingExpenditureReport(ByVal Domain As String, ByVal isOnline As String, ByVal TrainingId As String, ByVal Fromdate As String, ByVal todate As String, ByVal BranchId As String, ByVal CourseDirId As String, ByVal sponsorID As String) As DataSet
        Dim ds As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_GetTrainingExpDetails", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If TrainingId Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        End If
        If CourseDirId Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDirector", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseDirector", CourseDirId)
        End If
        cmd.Parameters.AddWithValue("@FDate", Fromdate)
        cmd.Parameters.AddWithValue("@TDate", todate)


        If BranchId Is Nothing Then
            cmd.Parameters.AddWithValue("@Branchid", DBNull.Value)

        Else
            cmd.Parameters.AddWithValue("@Branchid", BranchId)

        End If


        If sponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorId", DBNull.Value)

        Else
            cmd.Parameters.AddWithValue("@SponsorId", sponsorID)

        End If

        da.Fill(ds)
        con.Close()
        Return ds

    End Function

    Public Function Get_DRAD_Receipts(ByVal Domain As String, ByVal IsOnline As String, ByVal SponsorId As String, ByVal SponsorToId As String, ByVal Financialyear As String, ByVal AgencyId As String, ByVal procedurefor As String) As DataTable
        Dim dt As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_get_receipt_transfered_data", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If SponsorId Is Nothing OrElse SponsorId.Trim = "" Then
            cmd.Parameters.AddWithValue("@RM_Trfby_Sponsorid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@RM_Trfby_Sponsorid", SponsorId)
        End If
        If SponsorToId Is Nothing OrElse SponsorToId.Trim = "00000" Then
            cmd.Parameters.AddWithValue("@TrfTo_Sponsorid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrfTo_Sponsorid", SponsorToId)
        End If
        cmd.Parameters.AddWithValue("@finyear", Financialyear)
        cmd.Parameters.AddWithValue("@branchid", AgencyId)
        cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function GetHallBookingReport(ByVal Domain As String, ByVal isOnline As String, ByVal Month As String, ByVal Year As String, ByVal FromDate As String, ByVal Todate As String, ByVal Branchid As String) As DataTable
        Dim dtNo As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.GetHallBookingReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daWorkMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
        If Month = "NULL" Then
            cmd.Parameters.AddWithValue("@month", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@month", Month)
        End If

        If Year = "NULL" Then
            cmd.Parameters.AddWithValue("@F_year", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@F_year", Year)
        End If

        cmd.Parameters.AddWithValue("@from_date", FromDate)
        cmd.Parameters.AddWithValue("@to_date", Todate)
        cmd.Parameters.AddWithValue("@branchid", Branchid)
        daWorkMaster.Fill(dtNo)
        con.Close()
        Return dtNo
    End Function

    Public Function getcoursedetail(ByVal Domain As String, ByVal isOnline As String, ByVal trainingplanid As String) As DataTable
        Dim dsSchemes As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetTrainingCourseDetails", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Then
            'cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TraingingId", trainingplanid)
        End If
        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes.Tables(0)
    End Function

    Public Function GetFeedbackDetail(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingplanid As String, ByVal participantid As String, ByVal procedurefor As String, ByVal status As String, ByVal weekid As String) As DataSet
        Dim dsFeedback As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cmd = New SqlCommand("TrainingPlan.proc_get_Participant_Feedback", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Then
            'cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingId", trainingplanid)
        End If
        cmd.Parameters.AddWithValue("@participantId", participantid)
        cmd.Parameters.AddWithValue("@feedbackstatus", status)
        cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        If weekid.ToString.Trim <> "" And weekid.ToString().ToUpper <> "NULL" Then
            cmd.Parameters.AddWithValue("@weekid", weekid)
        Else
            cmd.Parameters.AddWithValue("@weekid", DBNull.Value)
        End If

        Dim daFeedback As SqlDataAdapter = New SqlDataAdapter(cmd)
        daFeedback.Fill(dsFeedback)
        con.Close()
        Return dsFeedback
    End Function

    Public Function GetTrainingPlanCode(ByVal Domain As String, ByVal isOnline As String, ByVal ParentId As String, ByVal BranchID As String, ByVal SchemeId As String, ByVal ForAcademy As String, ByVal Fromdate As String, ByVal Todate As String) As DataTable

        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        Dim dtTrainingPlanno As New DataTable
        Dim da As New SqlDataAdapter("TrainingPlan.GetTrainingPlanNo", con)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 5000
        da.SelectCommand.Parameters.AddWithValue("@ParentId", ParentId)
        da.SelectCommand.Parameters.AddWithValue("@BranchId", BranchID)
        da.SelectCommand.Parameters.AddWithValue("@ForAcademy", 1)
        If Not SchemeId = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@SchemeId", UCase(SchemeId))
        End If
        If Fromdate = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@Fromdate", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
        End If
        If Todate = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@Todate", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@Todate", Todate)
        End If

        da.Fill(dtTrainingPlanno)
        con.Close()
        Return dtTrainingPlanno
    End Function

    Public Function getFeedback_GOI(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingId As String) As DataSet
        Dim ds As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim cmd As New SqlCommand("TrainingPlan.getFeedback_GOI", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.Parameters.Add(New SqlParameter("@TrainingId", TrainingId))

        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetCourseDetails(ByVal Domain As String, ByVal IsOnline As String, ByVal CourseId As String) As DataSet
        Dim dsCourse As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetCourse", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If CourseId Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CourseId", CourseId)
        End If
        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dsCourse)
        con.Close()
        Return dsCourse
    End Function

    'Public Function GetGuestFaculties(ByVal Domain As String, ByVal IsOnline As String, ByVal CourseID As String, ByVal topicID As String, ByVal FacultyID As String) As DataTable
    '    Dim dtGuestFaculties As New DataTable
    '    Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
    '    con.Open()
    '    cmd = New SqlCommand("TrainingPlan.TP_GetGuestFaculties", con)
    '    cmd.CommandType = CommandType.StoredProcedure
    '    cmd.Connection = con
    '    cmd.CommandTimeout = 5000
    '    If CourseID Is Nothing Then
    '        cmd.Parameters.AddWithValue("@CourseID", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@CourseID", CourseID)
    '    End If
    '    If topicID Is Nothing Then
    '        cmd.Parameters.AddWithValue("@topicID", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@topicID", topicID)
    '    End If
    '    If FacultyID Is Nothing Then
    '        cmd.Parameters.AddWithValue("@FacultyID", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@FacultyID", FacultyID)
    '    End If
    '    Dim daGuestFaculties As SqlDataAdapter = New SqlDataAdapter(cmd)
    '    daGuestFaculties.Fill(dtGuestFaculties)
    '    con.Close()
    '    Return dtGuestFaculties
    'End Function

    Public Function GetHonorariumPaymentReport(ByVal Domain As String, ByVal IsOnline As String, ByVal FromDate As String, ByVal ToDate As String, ByVal facultyid As String, ByVal Trainingid As String) As DataTable
        Dim dtTraining As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.GetHonorariumPaymentReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If FromDate = Nothing Then
            cmd.Parameters.AddWithValue("@FromDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@FromDate", FromDate)
        End If
        If ToDate = Nothing Then
            cmd.Parameters.AddWithValue("@ToDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ToDate", ToDate)
        End If
        If facultyid Is Nothing Then
            cmd.Parameters.AddWithValue("@facultyid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Facultyid", facultyid)
        End If
        If Trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingid", Trainingid)
        End If
        Dim daTraining As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTraining.Fill(dtTraining)
        con.Close()
        Return dtTraining
    End Function


    Private Function AddFacultyFeedback(ByVal drfacultyFeedback As DataRow) As String
        Dim sbtype3 As New StringBuilder
        If Not (drfacultyFeedback Is Nothing) Then
            sbtype3.Append("<td align='center'>")
            sbtype3.Append(drfacultyFeedback.Item("PercentagePerformance").ToString)
            sbtype3.Append("</td>")
        Else
            sbtype3.Append("<td align='center'>0</td>")
        End If
        Return sbtype3.ToString
    End Function

    Public Function GetAgencies(ByVal Domain As String, ByVal IsOnline As String, ByVal AgencyTypeId As String, ByVal withStandardColumns As Byte, ByVal isDiableRequired As String) As DataSet
        Dim dsAgencies As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("YUser.GetDynamicAgencyReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyTypeId", AgencyTypeId)
        cmd.Parameters.AddWithValue("@withStandardColumns", withStandardColumns)
        cmd.Parameters.AddWithValue("@reqdisable", isDiableRequired)
        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dsAgencies)
        con.Close()
        Return dsAgencies
    End Function

    '    End Function
    Public Function CHK_SPONSER_USED(ByVal Domain As String, ByVal isonline As String, ByVal sponsorid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand(" SELECT [TrainingPlan].[TP_F_CheckSponsor] (@SponsorID) ", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        If sponsorid Is Nothing Then
            cmd.Parameters.AddWithValue("@SponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorID", sponsorid)
        End If
        Dim Isexists As Boolean = cmd.ExecuteScalar.ToString()
        con.Close()
        Return Isexists
    End Function

    Public Function TrainingProposalReport(ByVal Domain As String, ByVal IsOnline As String, ByVal proposalid As String, ByVal Branchid As String, ByVal FDate As String, ByVal TDate As String, ByVal IsForwarded As String, ByVal employeeid As String, ByVal dmsconf As String, ByVal procedurefor As String) As DataSet
        Dim dtTraining As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_trg_proposal]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        If proposalid = "NULL" Then
            cmd.Parameters.AddWithValue("@ttp_id", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ttp_id", proposalid)
        End If

        If Branchid = "NULL" Then
            cmd.Parameters.AddWithValue("@BranchID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@BranchID", Branchid)
        End If

        If FDate = "NULL" Then
            cmd.Parameters.AddWithValue("@FromDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@FromDate", FDate)
        End If
        If TDate = "NULL" Then
            cmd.Parameters.AddWithValue("@ToDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ToDate", TDate)
        End If

        If IsForwarded = "NULL" Then
            cmd.Parameters.AddWithValue("@Isforwarded", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Isforwarded", IsForwarded)
        End If
        If employeeid = "NULL" Then
            cmd.Parameters.AddWithValue("@employeeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@employeeid", employeeid)
        End If

        If dmsconf = "NULL" Then
            cmd.Parameters.AddWithValue("@DMSConf", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@DMSConf", dmsconf)
        End If

        If procedurefor = "NULL" Then
            cmd.Parameters.AddWithValue("@procedurefor", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        End If
        cmd.Parameters.AddWithValue("@choice", System.Web.HttpContext.Current.Session("choice"))
        Dim daTraining As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTraining.Fill(dtTraining)
        con.Close()
        Return dtTraining
    End Function

    Public Function CHK_CATEGORY_IN_USE(ByVal Domain As String, ByVal isonline As String, ByVal categorydetailid As String) As String
        Dim con As SqlConnection = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("Select TrainingPlan.[F_CheckTrainingCategoryRate] (@CategoryDetailID)", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@CategoryDetailID", categorydetailid)
        Dim IsExists As Boolean = cmd.ExecuteScalar()
        con.Close()
        Return IsExists
    End Function

    Public Function GetFeedbackDetailNew(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingplanid As String, ByVal ProcedureFor As Integer, ByVal status As String) As DataSet
        Dim dsFeedback As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cmd = New SqlCommand("TrainingPlan.proc_get_training_wise_feedback", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Then
            'cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ttwf_trainingid", trainingplanid)
        End If
        cmd.Parameters.AddWithValue("@ttwf_status", status)
        cmd.Parameters.AddWithValue("@Procedurefor", ProcedureFor)
        Dim daFeedback As SqlDataAdapter = New SqlDataAdapter(cmd)
        daFeedback.Fill(dsFeedback)
        con.Close()
        Return dsFeedback
    End Function

    Public Function GetFeedbackStandards(ByVal Domain As String, ByVal IsOnline As String) As DataTable
        Dim dtFeedbackStandard As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetFeedbackStandards", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        ' cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        ' cmd.Parameters.AddWithValue("@WithParticipantDetails", WithParticipantDetails)
        Dim daFeedback As SqlDataAdapter = New SqlDataAdapter(cmd)
        daFeedback.Fill(dtFeedbackStandard)
        con.Close()
        Return dtFeedbackStandard
    End Function

    Public Function GetCollectionRegisterDetails(ByVal Domain As String, ByVal IsOnline As String, ByVal collectiondate As String, ByVal todate As String, ByVal schemeid As String, ByVal sponsorId As String) As DataSet
        Dim dsCollectionRegister As New DataSet

        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetCollectionRegister", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        ' 
        cmd.Parameters.AddWithValue("@_FDate", collectiondate)
        cmd.Parameters.AddWithValue("@_TDate", todate)

        cmd.Parameters.AddWithValue("@Schemeid", schemeid)
        If Not sponsorId = Nothing Then
            cmd.Parameters.AddWithValue("@sponsorId", sponsorId)
        Else
            cmd.Parameters.AddWithValue("@sponsorId", DBNull.Value)
        End If


        Dim dacollectionregister As SqlDataAdapter = New SqlDataAdapter(cmd)

        dacollectionregister.Fill(dsCollectionRegister)
        con.Close()

        Return dsCollectionRegister
    End Function

    Public Function FillBank(ByVal Domain As String, ByVal IsOnline As String, ByVal CreatedBy As String, ByVal BranchID As String, ByVal SchemeId As String, ByVal OnlyBanks As String, ByVal GroupId As String, ByVal LedgerTypeXML As String)


        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)

        con.Open()
        Dim dset As New DataTable
        Dim ddl As DataTable
#Disable Warning BC42024 ' Unused local variable: 'i'.
        Dim i As Integer
#Enable Warning BC42024 ' Unused local variable: 'i'.
        dset.Clear()
        Dim da As New SqlDataAdapter("YUser.getLedger", con)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 5000
        da.SelectCommand.Parameters.AddWithValue("@BranchId", BranchID)
        da.SelectCommand.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        If GroupId Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@GroupId", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@GroupId", GroupId)
        End If
        If LedgerTypeXML Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@LedgerTypeXML", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@LedgerTypeXML", LedgerTypeXML)
        End If
        da.SelectCommand.Parameters.AddWithValue("@OnlyBanks", OnlyBanks)

        If Not SchemeId = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@SchemeId", UCase(SchemeId))
        End If

        da.Fill(dset)
        con.Close()
        dset.DefaultView.RowFilter = "LedgerId <> '3fb3ffc9-9cbe-4829-a680-cc09f56e7a36'"

        ddl = dset.DefaultView.ToTable()



        'ddl.DataValueField = "LedgerId"
        'If choice = 1 Then
        '    ddl.DataTextField = "LedgerName"
        'Else
        '    ddl.DataTextField = "LedgerName"
        'End If
        'ddl.DataBind()

        dset.Dispose()
        Return ddl
    End Function





    Public Function Get_TRG_FOR_Feedback(ByVal Domain As String, ByVal isonline As String, ByVal mobileno As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingAmount As New DataTable
        cmd = New SqlCommand("select * from  [TrainingPlan].[f_tp_trgid_for_feedback] ('" + mobileno + "') ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtTrainingAmount)
        con.Close()
        Return dtTrainingAmount
    End Function

    Public Function Register_Faculty_Application(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal createdOn As String, ByVal AgencyXML As String, ByVal SpecilizationXML As String,
                                                 ByVal createdby As String, ByVal docremark As String, ByVal docdate As String, ByVal empid As String, ByVal fwddate As String, ByVal typeid As String,
                                                ByVal docstatus As String, ByVal procfor As String, ByVal uploaddoc As String, ByVal uploaddocname As String, ByVal BranchID As String) As DataTable
        Dim dtFacultyreg As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_ins_tbl_tp_faculty_registration]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ttfrm_id", AgencyId)
        cmd.Parameters.AddWithValue("@ttfrm_date", createdOn)
        cmd.Parameters.AddWithValue("@ttfrm_xml", AgencyXML)

        If SpecilizationXML Is Nothing Or SpecilizationXML = "" Then
            cmd.Parameters.AddWithValue("@ttfrs_xml", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ttfrs_xml", SpecilizationXML)
        End If

        If createdby Is Nothing Or createdby = "" Then
            cmd.Parameters.AddWithValue("@created_by", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@created_by", createdby)
        End If

        If docremark Is Nothing Or docremark = "" Then
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docremark", docremark)
        End If

        If docdate Is Nothing Or docdate = "" Then
            cmd.Parameters.AddWithValue("@docdate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docdate", docdate)
        End If

        If BranchID Is Nothing Or BranchID = "" Then
            cmd.Parameters.AddWithValue("@branchid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@branchid", BranchID)
        End If
        If empid Is Nothing Or empid = "" Then
            cmd.Parameters.AddWithValue("@created_by_empid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@created_by_empid", empid)
        End If
        If fwddate Is Nothing Or fwddate = "" Then
            cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@fwd_empid", fwddate)
        End If

        cmd.Parameters.AddWithValue("@tat_type_id", typeid)
        cmd.Parameters.AddWithValue("@doc_status", docstatus)
        cmd.Parameters.AddWithValue("@procedurefor", procfor)

        If uploaddoc Is Nothing Or uploaddoc = "" Then
            cmd.Parameters.AddWithValue("@uploaded_doc", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@uploaded_doc", uploaddoc)
        End If


        If uploaddocname Is Nothing Or uploaddocname = "" Then
            cmd.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@uploaded_doc_name", uploaddocname)
        End If


        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtFacultyreg)
        con.Close()
        Return dtFacultyreg
    End Function


    Public Function Get_Mob_User_Data(ByVal Domain As String, ByVal isonline As String, ByVal mobileno As String) As DataTable
        Dim dtFacultyreg As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("[YUSER].[proc_yuser_chk_user_mobile_no]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@user_mobile_no", mobileno)

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtFacultyreg)
        con.Close()
        Return dtFacultyreg
    End Function

    Public Function Get_Faculty_Registration(ByVal Domain As String, ByVal IsOnline As String, ByVal registrationid As String) As DataSet
        Dim dsFacultyRegistration As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_tbl_tp_faculty_registration]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ttfrm_id", registrationid)
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsFacultyRegistration)
        con.Close()
        Return dsFacultyRegistration
    End Function

    Public Function CHECK_EXISTING_Columnvalue(ByVal Domain As String, ByVal isonline As String, ByVal Searchvalue As String, ByVal agencytypeid As String, ByVal columnid As String, ByVal agencyid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If agencyid <> "NULL" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [YUSER].[fs_yuser_check_duplicate_agency_column_value] ('" + Searchvalue + "','" + agencytypeid + "','" + columnid + "','" + agencyid + "') select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [YUSER].[fs_yuser_check_duplicate_agency_column_value] ('" + Searchvalue + "','" + agencytypeid + "','" + columnid + "',null) select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function



    Public Function ResetPassword(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal UserId As String, ByVal OldPassword As String, ByVal NewPassword As String, ByVal Type As Boolean) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        If cnn.State = ConnectionState.Closed Then
            cnn.Open()
        Else
            cnn.Close()
            cnn.Open()
        End If
        Dim cmd As New SqlCommand("YUser.ChangePassword", cnn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add(New SqlParameter("@UserId", UserId))
        If Not OldPassword Is Nothing Then
            cmd.Parameters.Add(New SqlParameter("@OldPassword", OldPassword))
        Else
            cmd.Parameters.Add(New SqlParameter("@OldPassword", DBNull.Value))
        End If

        cmd.Parameters.Add(New SqlParameter("@NewPassword", NewPassword))
        cmd.Parameters.Add(New SqlParameter("@ty", Type))
        cmd.ExecuteNonQuery()
        cnn.Close()
        cmd.Dispose()

        Return True

    End Function


    Public Function CHECK_EXISTING_Columnvalue_Reg(ByVal Domain As String, ByVal isonline As String, ByVal Searchvalue As String, ByVal agencytypeid As String, ByVal columnid As String, ByVal agencyid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If agencyid <> "NULL" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [YUser].[fs_yuser_check_duplicate_value_on_faculty_reg] ('" + Searchvalue + "','" + agencytypeid + "','" + columnid + "','" + agencyid + "') select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [YUser].[fs_yuser_check_duplicate_value_on_faculty_reg] ('" + Searchvalue + "','" + agencytypeid + "','" + columnid + "',null) select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function


    Public Function Save_Proposal_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal proposXML As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("~", ","))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        'Dim sptXMLName() As String = XMLName.Split(",")

        'For i = 0 To dtXMLparameterlist.Rows.Count - 1
        '    For j = 0 To dtXMLparameterlist.Columns.Count - 1
        '        Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
        '        If xmlpara.ToString = "" Then
        '            Continue For
        '        End If

        '        Dim dttempxml As DataTable
        '        Dim xmldata As String
        '        If xmlpara IsNot Nothing Then
        '            'dttempxml = DerializeDataTable(xmlpara)
        '            dttempxml = GetDataTable_For_Proposal(xmlpara)
        '            If dttempxml.Rows.Count > 0 Then
        '                Dim wr As New StringWriter
        '                dttempxml.TableName = sptXMLName(j)
        '                dttempxml.WriteXml(wr)
        '                xmldata = wr.ToString()
        '            End If

        '        Else
        '            xmldata = Nothing
        '        End If
        '        cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
        '    Next
        'Next
        If proposXML.ToString.ToUpper <> "NULL" Then
            cmd.Parameters.AddWithValue("@proposalxml", proposXML)
        Else
            cmd.Parameters.AddWithValue("@proposalxml", DBNull.Value)
        End If


        Dim dtProposal As New DataTable
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtProposal)
        con.Close()
        Return dtProposal
    End Function
    Public Function GetRoomCategory(ByVal Domain As String, ByVal isOnline As String, ByVal TrainingCategoryId As String, ByVal category As String, ByVal isallData As String, ByVal effectivedate As String) As DataSet
        Dim dsRoomCategory As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetRoomType", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If TrainingCategoryId Is Nothing Then
            cmd.Parameters.AddWithValue("@RoomTypeID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@RoomTypeID", TrainingCategoryId)
        End If
        If category Is Nothing Then
            cmd.Parameters.AddWithValue("@category", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@category", category)
        End If
        cmd.Parameters.AddWithValue("@isall", isallData)

        If effectivedate.ToString.Trim <> "" Then
            cmd.Parameters.AddWithValue("@effectivedate", effectivedate)
        Else
            cmd.Parameters.AddWithValue("@effectivedate", DBNull.Value)
        End If
        Dim daRoomCategory As SqlDataAdapter = New SqlDataAdapter(cmd)
        daRoomCategory.Fill(dsRoomCategory)
        con.Close()
        Return dsRoomCategory
    End Function

    Public Function Get_MAIL_SMS_TEXT(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal applicationtypeid As String, ByVal applicationid As String, ByVal status As String, Optional ByVal procedurefor As String = Nothing, Optional ByVal lettertype As String = Nothing) As DataSet
        Dim dsItemGroup As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.CommandText = "[TrainingPlan].[proc_tp_send_mail_sms_data]"
        cmd.Parameters.AddWithValue("@doc_typeid", applicationtypeid)
        cmd.Parameters.AddWithValue("@docid", applicationid)
        cmd.Parameters.AddWithValue("@staus", status)
        If Not procedurefor Is Nothing Then
            cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        Else
            cmd.Parameters.AddWithValue("@procedurefor", DBNull.Value)
        End If
        If Not lettertype Is Nothing Then
            cmd.Parameters.AddWithValue("@tttds_letter_type", lettertype)
        Else
            cmd.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
        End If

        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dsItemGroup)
        con.Close()
        Return dsItemGroup
    End Function
    Public Function Get_NOTESHEET_DATA_For_PRINT(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal applicationid As String, ByVal docdate As String) As DataSet
        Dim dsNotesheet As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.CommandText = "[TrainingPlan].[proc_tp_get_doc_remark]"
        cmd.Parameters.AddWithValue("@docid", applicationid)
        cmd.Parameters.AddWithValue("@date", docdate)


        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dsNotesheet)
        con.Close()
        Return dsNotesheet
    End Function

    Public Function GetLedgerTypeColumnId(ByVal Domain As String, ByVal IsOnline As String, ByVal LedgerTypeid As String, ByVal columnID As String) As DataTable

        Dim dt As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        'Dim da As New SqlDataAdapter("Select LedgerTypeColumnID from YUser.LedgerTypeColumn where LedgerTypeID = '" & LedgerTypeid & "' and ColumnID = '" & columnID & "'", con)

        Dim da As New SqlDataAdapter("yuser.proc_get_ledgertypecolumnid", con)


        da.SelectCommand.CommandTimeout = 8000
        ' da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.Parameters.AddWithValue("@LedgerTypeID", LedgerTypeid)


        da.SelectCommand.Parameters.AddWithValue("@ColumnID", columnID)

        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function CancelReceiptVoucher(ByVal Domain As String, ByVal IsOnline As String, ByVal IsUpd As String, ByVal VDate As String, ByVal SchemeId As String,
                             ByVal VType As String, ByVal TotalDr As String, ByVal TotalCr As String,
                             ByVal Narration As String, ByVal CreatedBy As String, ByVal BranchId As String,
                             ByVal DTbl As Data.DataTable, ByVal VSp As String, ByVal VoucherNo As String,
                             ByVal VoucherId As String, ByVal VoucherTypeId As String,
                             ByVal FundTranId As String, ByVal VoucherGUID As String, ByVal receiptno As String, Optional ByVal FndTxFile As String = Nothing, Optional ByVal Functionaryid As String = Nothing, Optional ByVal voucherrefXML As String = Nothing) As DataSet

        Dim VDtls As String
        Dim Para As New SqlParameter
        ' Dim dr As SqlDataReader = Nothing
        'Dim Dt As New Data.DataTable("PrintData")
        Dim wr As New System.IO.StringWriter

        If Not DTbl Is Nothing Then
            DTbl.TableName = "temp"
            DTbl.WriteXml(wr)
            VDtls = wr.ToString()
            wr.Dispose()
            VDtls = Strings.Replace(VDtls, ",", "")
            VSp = Strings.Replace(VSp, ",", "")

        End If

        'Dim cmd As New SqlCommand
        'If IsUpd = False Then
        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()

        'cmd.Connection = cnn
        Dim da As SqlDataAdapter
        Dim ds As New Data.DataSet
        da = New SqlDataAdapter("Trainingplan.TP_CancelReceipt", cnn)
        'Else
        '    cmd.CommandText = "YUser.UpdVoucherMaster"
        '    cmd.Parameters.Add(New SqlParameter("@VoucherID", VoucherId))
        '    cmd.Parameters.Add(New SqlParameter("@VoucherNo", VoucherNo))
        'End If
        'Dim cnn As New SqlConnection(ConfigurationManager.AppSettings.Item("CnStr"))
        'Dim cnn As New SqlConnection(Me.Session("ConnectionString"))

        da.SelectCommand.CommandType = Data.CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 10000

        If VDate = Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@VDate", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@VDate", VDate))
        End If
        If SchemeId Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@SchemeId", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@SchemeId", SchemeId))

        End If

        If VType Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@VType", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@VType", VType))

        End If

        If TotalDr = Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@TotalDrAmount", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@TotalDrAmount", TotalDr))
        End If

        If TotalCr = Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@TotalCrAmount", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@TotalCrAmount", TotalCr))

        End If

        If Narration Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@Narration", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@Narration", Narration))


        End If

        If CreatedBy Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@CreatedBy", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@CreatedBy", CreatedBy))

        End If

        If receiptno Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@Receiptno", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@Receiptno", receiptno))
        End If


        'If Me.Session.Item("@AgencyId") <> "00000" Then
        '    cmd.Parameters.Add(New SqlParameter("@BranchId", Me.Session.Item("@AgencyId")))
        'End If
        If BranchId <> "00000" Then

            If BranchId Is Nothing Then
                da.SelectCommand.Parameters.Add(New SqlParameter("@BranchId", DBNull.Value))

            Else
                da.SelectCommand.Parameters.Add(New SqlParameter("@BranchId", BranchId))

            End If
        End If

#Disable Warning BC42104 ' Variable 'VDtls' is used before it has been assigned a value. A null reference exception could result at runtime.
        If VDtls Is Nothing Then
#Enable Warning BC42104 ' Variable 'VDtls' is used before it has been assigned a value. A null reference exception could result at runtime.
            da.SelectCommand.Parameters.Add(New SqlParameter("@DataXML", DBNull.Value))

        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@DataXML", VDtls))
        End If


        If VSp Is Nothing Then

            da.SelectCommand.Parameters.Add(New SqlParameter("@DataColXML", DBNull.Value))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@DataColXML", VSp))

        End If

        If VoucherTypeId <> Nothing Then

            da.SelectCommand.Parameters.Add(New SqlParameter("@VTypeId", VoucherTypeId))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@VTypeId", DBNull.Value))
        End If
        If FundTranId IsNot Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@FundTranID", FundTranId))

        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@FundTranID", DBNull.Value))
        End If

        If FndTxFile IsNot Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@FndTx", FndTxFile))

        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@FndTx", DBNull.Value))

        End If


        If Functionaryid Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@Functionaryid", DBNull.Value))

        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@Functionaryid", Functionaryid))
        End If
        If Not voucherrefXML Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@voucherrefXML", voucherrefXML))

        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@voucherrefXML", DBNull.Value))
        End If
        If VoucherGUID Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@VoucherGUID", DBNull.Value))

        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@VoucherGUID", VoucherGUID))
        End If



        da.SelectCommand.Parameters.Add(New SqlParameter("@IsPrint", True))


        da.Fill(ds)
        cnn.Close()
        Return ds
    End Function

    Public Function UpdateReceiptMaster(ByVal Domain As String, ByVal IsOnline As String, ByVal VoucherGuid As String, ByVal ReceiptidXML As String) As Boolean
        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_UpdateRECEIPTMASTER", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@VoucherGUID", VoucherGuid)
        cmd.Parameters.AddWithValue("@RECEIPTIDXML", ReceiptidXML)

        cmd.ExecuteNonQuery()
        con.Close()
        cmd.Dispose()
        Return True

    End Function

    Public Function Get_Proposal_Cost(ByVal Domain As String, ByVal isonline As String, ByVal isjoint As String, ByVal isresidential As String, ByVal proposedcandidate As String, ByVal duration As String, ByVal nooftrainings As String, ByVal effectivedate As String) As DataTable
        Dim dtcost As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        cmd = New SqlCommand("Select * from Trainingplan.F_GetTrainingAmount_for_proposal (N'" + isjoint + "'," + isresidential + "," + proposedcandidate + ",'" + duration + "','" + nooftrainings + "','" + effectivedate + "')", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dtcost)
        Return dtcost
    End Function
    Public Function GetDataTable_For_Proposal(ByVal str As String) As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sptstr() As String = str.Split("},")
        For i = 0 To sptstr.Length - 1

            sptstr(i) = sptstr(i).Replace("{", "")
            sptstr(i) = sptstr(i).Replace("[", "")
            sptstr(i) = sptstr(i).Replace("}", "")
            sptstr(i) = sptstr(i).Replace("]", "")

            If sptstr(i) = "" Then
                Continue For
            End If
            Dim sptField() As String = sptstr(i).Split(",")
            If i = 0 Then
                For j = 0 To sptField.Length - 1
                    Dim sptcol = sptField(j).Split(":")
                    dt.Columns.Add(sptcol(0).ToString())
                Next

            End If
            dr = dt.NewRow
            For j = 0 To sptField.Length - 1
                If sptField(j) = "" Then
                    Continue For
                End If

                Dim sptcol = sptField(j).Split(":")
                If sptcol(1).ToString() <> "" Then
                    dr(sptcol(0).ToString()) = sptcol(1).Replace("~", ",")
                Else
                    dr(sptcol(0).ToString()) = DBNull.Value
                End If


            Next
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
    Public Function Save_Nodel_Officer_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal password As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString)
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue("@ttsap_user_password", password)
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function


    Public Function CHECK_REGISTERED_MAIL_ID(ByVal Domain As String, ByVal isonline As String, ByVal mailid As String, ByVal sponsorid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If sponsorid = "" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_get_tp_sponosr_ahuthorised_person_email] ('" + mailid + "',null) select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[f_tp_get_tp_sponosr_ahuthorised_person_email] ('" + mailid + "','" + sponsorid + "') select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function


    Public Function CHECK_USERNAME_MOBILE_MAIL_IN_USER(ByVal Domain As String, ByVal isonline As String, ByVal checkvalue As String, ByVal userid As String, ByVal type As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        If userid = "" Then
            cmd = New SqlCommand("declare @isexi int select  @isexi =   [yuser].[fs_yuser_check_username_rmn_email] ('" + checkvalue + "',null,'" + type + "') select @isexi", con)
        Else
            cmd = New SqlCommand("declare @isexi int select  @isexi =    [yuser].[fs_yuser_check_username_rmn_email] ('" + checkvalue + "','" + userid + "','" + type + "') select @isexi", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function GET_NOTESHEET_TEMPLATE(ByVal Domain As String, ByVal IsOnline As String, ByVal documenttypeid As String, ByVal documentid As String, ByVal status As String, ByVal procedurefor As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("[TrainingPlan].[proc_tp_doc_template_data]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)

        cmd.Parameters.AddWithValue("@staus", status)
        cmd.Parameters.AddWithValue("@doc_typeid", documenttypeid)
        cmd.Parameters.AddWithValue("@docid", documentid)
        cmd.Parameters.AddWithValue("@procedurefor", procedurefor)



        '---------------------------------------------------------------


        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function CheckRoom(ByVal Domain As String, ByVal IsOnline As String, ByVal RoomID As String) As Boolean
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Select [TrainingPlan].[F_CheckRoom] (@RoomID)", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@RoomID", RoomID)
        Dim IsExists As Boolean = cmd.ExecuteScalar()
        con.Close()
        Return IsExists
    End Function

    Public Function Save_Proposal(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal docremark As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        If (docremark = "NULL") Then
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docremark", docremark)
        End If

        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function



    Public Function Save_Proposal_Data_For_Enquiry(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal proposXML As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("~", ","))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable_For_Proposal(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next
        If proposXML.ToString.ToUpper <> "NULL" Then
            cmd.Parameters.AddWithValue("@proposalxml", proposXML)
        Else
            cmd.Parameters.AddWithValue("@proposalxml", DBNull.Value)
        End If


        Dim dtProposal As New DataTable
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtProposal)
        con.Close()
        Return dtProposal
    End Function

    Public Function GET_HallDashboard_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal hallid As String, ByVal halltypeid As String, ByVal frmdate As String, ByVal todate As String, ByVal procedure As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_hall_occupancy", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If hallid = "NULL" Then
            cmd.Parameters.AddWithValue("@hallid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hallid", hallid)
        End If

        If halltypeid = "NULL" Then
            cmd.Parameters.AddWithValue("@halltypeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@halltypeid", halltypeid)
        End If

        cmd.Parameters.AddWithValue("@FromDate", frmdate)
        cmd.Parameters.AddWithValue("@ToDate", todate)
        cmd.Parameters.AddWithValue("@procedurefor", procedure)

        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetPrintFacultyLetter(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingPlanid As String, ByVal Timetableid As String, ByVal Facultyid As String, ByVal Choice As String) As DataTable

        Dim dt As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()

        Dim da As New SqlDataAdapter("[TrainingPlan].[proc_tp_print_faculty_letter]", con)
        da.SelectCommand.CommandTimeout = 8000

        da.SelectCommand.CommandType = CommandType.StoredProcedure

        If TrainingPlanid = "NULL" Then
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", TrainingPlanid)

        End If

        If Timetableid = "NULL" Then
            da.SelectCommand.Parameters.AddWithValue("@timetableid", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@timetableid", Timetableid)
        End If

        If Facultyid = "NULL" Then
            da.SelectCommand.Parameters.AddWithValue("@facultyid", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@facultyid", Facultyid)
        End If


        If Choice = "NULL" Then
            da.SelectCommand.Parameters.AddWithValue("@choice", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@choice", Choice)
        End If

        da.Fill(dt)
        con.Close()
        Return dt
    End Function


    Public Function Get_Nomination_Summery_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal departmentid As String, ByVal trainingsFor As String, ByVal periodtype As String, ByVal periodvalue As String, ByVal periodyear As String, ByVal fromdt As String, ByVal toDT As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_get_tbl_tp_participant_nomination", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@procedurefor", "3")

        If (periodtype <> "NULL") Then
            cmd.Parameters.AddWithValue("@periodtype", periodtype)
        Else
            cmd.Parameters.AddWithValue("@periodtype", DBNull.Value)
        End If
        If periodvalue <> "NULL" Then
            cmd.Parameters.AddWithValue("@periodvalue", periodvalue)
        Else
            cmd.Parameters.AddWithValue("@periodvalue", DBNull.Value)
        End If
        If periodyear <> "NULL" Then
            cmd.Parameters.AddWithValue("@periodyear", periodyear)
        Else
            cmd.Parameters.AddWithValue("@periodyear", DBNull.Value)
        End If

        If fromdt <> "NULL" Then
            cmd.Parameters.AddWithValue("@fromdt", fromdt)
        Else
            cmd.Parameters.AddWithValue("@fromdt", DBNull.Value)
        End If

        If toDT <> "NULL" Then
            cmd.Parameters.AddWithValue("@todt", toDT)
        Else
            cmd.Parameters.AddWithValue("@todt", DBNull.Value)
        End If
        If trainingid <> "" Then
            cmd.Parameters.AddWithValue("@training_id", trainingid)
        End If
        If departmentid <> "" Then
            cmd.Parameters.AddWithValue("@sponsor_id", departmentid)
        End If



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function GET_TRAINEE_LETTER_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal TrainingId As String, ByVal WithParticipantDetails As Byte, ByVal WithExpenditureDetails As Byte) As DataSet
        Dim dsTrainingDetails As New DataSet
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.GetTrainingPlanDetails", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.Parameters.AddWithValue("@WithParticipantDetails", WithParticipantDetails)
        cmd.Parameters.AddWithValue("@WithExpenditureDetails", WithExpenditureDetails)
        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dsTrainingDetails)
        con.Close()
        Return dsTrainingDetails
    End Function

    Public Function GetNotesheetInfo(ByVal Domain As String, ByVal isOnline As String, ByVal TrainingId As String, ByVal DocumentType As Byte) As DataTable
        Dim dtnotesheet As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetNotesheetInfo", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.Parameters.AddWithValue("@DocumentType", DocumentType)
        cmd.CommandTimeout = 5000
        Dim danotesheet As SqlDataAdapter = New SqlDataAdapter(cmd)
        danotesheet.Fill(dtnotesheet)
        con.Close()
        Return dtnotesheet
    End Function
    Public Function GetKitDistributionReport(ByVal Domain As String, ByVal isOnline As String, ByVal TrainingId As String) As DataTable
        Dim dtReport As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetKitDistributionReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dtReport)
        con.Close()
        Return dtReport
    End Function
    Public Function GetConfigurationDetail_For_Kit_Dist(ByVal Domain As String, ByVal isOnline As String, ByVal CONFIGURATIONID As String, ByVal Branchid As String, ByVal Currentdate As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetconfigurationSettings", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If CONFIGURATIONID Is Nothing Then
            cmd.Parameters.AddWithValue("@CONFIGURATIONID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@CONFIGURATIONID", CONFIGURATIONID)
        End If
        cmd.Parameters.AddWithValue("@Branchid", Branchid)

        If Currentdate = "" Then
            cmd.Parameters.AddWithValue("@Currentdate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Currentdate", Currentdate)
        End If
        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function Save_hall_Reservation_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next

        Dim dthallReservt As New DataTable
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dthallReservt)
        cnn.Close()
        Return dthallReservt
    End Function
    Public Function Get_NOTESHEET_DATA_For_PRINT_New_Format(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal applicationid As String, ByVal docdate As String, ByVal doctype As String) As DataSet
        Dim dsNotesheet As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.CommandText = "[TrainingPlan].[proc_tp_get_doc_remark]"
        cmd.Parameters.AddWithValue("@docid", applicationid)
        cmd.Parameters.AddWithValue("@date", docdate)
        cmd.Parameters.AddWithValue("@tttds_doc_type", doctype)

        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dsNotesheet)
        con.Close()
        Return dsNotesheet
    End Function

    Public Function Save_HALL_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal docremark As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        If (docremark = "NULL") Then
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docremark", docremark)
        End If

        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function

    Public Function Get_HALL_BOOKING_Data(ByVal Domain As String, ByVal isonline As String, ByVal bookingid As String, ByVal branchid As String, ByVal Fromdate As String, ByVal todate As String) As DataSet

        Dim dtDocdata As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, isonline)
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.CommandText = "[TrainingPlan].[proc_tp_get_hall_booking_data]"
        If bookingid.ToString.ToUpper = "NULL" Then
            cmd.Parameters.AddWithValue("@tthbm_id", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@tthbm_id", bookingid)
        End If

        cmd.Parameters.AddWithValue("@branchid", branchid)
        If Fromdate.ToString.ToUpper = "NULL" Then
            cmd.Parameters.AddWithValue("@fromdt", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@fromdt", Fromdate)
        End If
        If todate.ToString.ToUpper = "NULL" Then
            cmd.Parameters.AddWithValue("@todate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@todate", todate)
        End If


        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dtDocdata)
        con.Close()
        Return dtDocdata
    End Function

    Public Function CHECK_REGISTERED_MOBILE_NO_FOR_FEEDBACK(ByVal Domain As String, ByVal isonline As String, ByVal Mobileno As String) As DataTable
        Dim dttrainingdata As New DataTable

        con = Get_Connection_String(Domain, isonline)
        con.Open()

        cmd = New SqlCommand("select  *  from [TrainingPlan].[ft_tp_get_participant_rmn] ('" + Mobileno + "')", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dttrainingdata)
        con.Close()
        con.Close()
        Return dttrainingdata
    End Function
    Public Function Save_HALL_ORDER_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal orderhtml As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("~", ","))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        cmd.Parameters.AddWithValue("@tthbo_order_html", orderhtml)


        Dim dtProposal As New DataTable
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtProposal)
        con.Close()
        Return dtProposal
    End Function


    Public Function Save_Feedback_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal feedbackdataXML As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("~", ","))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        cmd.Parameters.AddWithValue("@ParticipantsFeedbackXml", feedbackdataXML)



        Dim dtProposal As New DataTable
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtProposal)
        con.Close()
        Return dtProposal
    End Function

    Public Function GetTrainingForBill(ByVal Domain As String, ByVal IsOnline As String, ByVal SponsorId As String, ByVal ForBill As String, ByVal Fromdate As String, ByVal Todate As String, ByVal BillType As String, ByVal SchemeId As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.TP_GetTrainingNos", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If SponsorId Is Nothing OrElse SponsorId.Trim = "" Then
            cmd.Parameters.AddWithValue("@SponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorID", SponsorId)
        End If
        If SponsorId Is Nothing OrElse SponsorId.Trim = "" Then
            cmd.Parameters.AddWithValue("@ForBill", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ForBill", ForBill)
        End If
        If Fromdate = Nothing Then
            cmd.Parameters.AddWithValue("@Fromdate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Fromdate", Fromdate)
        End If
        If Todate = Nothing Then
            cmd.Parameters.AddWithValue("@Todate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Todate", Todate)
        End If
        cmd.Parameters.AddWithValue("@BillType", BillType)
        If SchemeId Is Nothing Then
            cmd.Parameters.AddWithValue("@Schemeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Schemeid", SchemeId)
        End If
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetTraineeAttendanceReport(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingId As String, ByVal fromdate As Date, ByVal todate As Date) As DataTable
        Dim dtReport As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetTrainingAttendanceReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.Parameters.AddWithValue("@FromDateOfPresence", fromdate)
        cmd.Parameters.AddWithValue("@ToDateOfPresence", todate)
        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dtReport)
        con.Close()
        Return dtReport
    End Function

    Public Function Save_Notesheet_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal editorhtml As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue("@printtext", editorhtml)
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function
    Public Function Save_Horonium_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal horoniumxml As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue("@honorariumxml", horoniumxml)
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        'cmd.ExecuteNonQuery()
        'cnn.Close()
        Dim dthoorder As New DataTable
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dthoorder)
        con.Close()
        Return dthoorder
    End Function
    Public Function Save_Horonium_Payment(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal horoniumxml As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue("@honorariumpaymentxml", horoniumxml)
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        Dim dthoorder As New DataTable
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dthoorder)
        con.Close()

        Return dthoorder

    End Function


    Public Function Save_Common_Data_With_Single_XML(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal xmlparam As String, ByVal Xmlvalue As String, ByVal CreatedonParam As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue(xmlparam, Xmlvalue)




        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function

    Public Function GetParticipantsAttendance(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingId As String) As DataSet
        Dim dtKit As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetParticipantsAttendance", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.CommandTimeout = 5000
        Dim daKit As SqlDataAdapter = New SqlDataAdapter(cmd)
        daKit.Fill(dtKit)
        con.Close()

        Return dtKit
    End Function
    Public Function GetTraineeAttendance_Register(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingId As String) As DataTable
        Dim dtReport As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)

        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dtReport)
        con.Close()
        Return dtReport
    End Function


    Public Function GetTraningBILLNO(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingDate As String, ByVal BranchId As String, ByVal BillType As String) As String
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("Select TrainingPlan.[F_getBillNo] (@Date, @BranchId,@BillType) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = TrainingDate
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        cmd.Parameters.AddWithValue("@BillType", BillType)
        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function
    Public Function InsupdSponsorbill(ByVal Domain As String, ByVal IsOnline As String, ByVal SponsorBillid As String, ByVal SponsorId As String,
ByVal Billamount As String, ByVal Grossamount As String, ByVal ServiceTaxamount As String, ByVal Advanceamount As String, ByVal Billtype As String, ByVal Billdate As String, ByVal BillNo As String, ByVal CreatedBy As String, ByVal BranchID As String, ByVal ExpXML As String, ByVal VoucherGUID As String, ByVal Schemeid As String, ByVal receiptwisebillamountXML As String, ByVal employeeid As String) As Boolean
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim returnBooleanvalue As Boolean
        cmd = New SqlCommand("[TrainingPlan].[TP_InsUpdSponsorBill]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@SponsorBillid", SponsorBillid)
        cmd.Parameters.AddWithValue("@SponsorId", SponsorId)
        cmd.Parameters.AddWithValue("@Billamount", Billamount)
        cmd.Parameters.AddWithValue("@Grossamount", Grossamount)
        cmd.Parameters.AddWithValue("@ServiceTaxamount", ServiceTaxamount)
        cmd.Parameters.AddWithValue("@Advanceamount", Advanceamount)
        cmd.Parameters.AddWithValue("@Billtype", Billtype)
        cmd.Parameters.AddWithValue("@Billdate", Billdate)
        cmd.Parameters.AddWithValue("@BillNo", BillNo)
        If VoucherGUID Is Nothing Then
            cmd.Parameters.AddWithValue("@VoucherGUID", VoucherGUID)
        Else
            cmd.Parameters.AddWithValue("@VoucherGUID", VoucherGUID)
        End If
        If Schemeid Is Nothing Then
            cmd.Parameters.AddWithValue("@Schemeid ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Schemeid ", Schemeid)
        End If
        If receiptwisebillamountXML Is Nothing Then
            cmd.Parameters.AddWithValue("@ADVANCEAMOUNTXML ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ADVANCEAMOUNTXML ", receiptwisebillamountXML)
        End If
        cmd.Parameters.AddWithValue("@ExpXML", ExpXML)
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@BranchId", BranchID)

        cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        cmd.Parameters.AddWithValue("@docdate", Billdate)
        cmd.Parameters.AddWithValue("@CreatedBy_empid", employeeid)
        cmd.Parameters.AddWithValue("@fwd_empid", employeeid)
        cmd.Parameters.AddWithValue("@tat_type_id", "104")
        cmd.Parameters.AddWithValue("@doc_status", "1")
        cmd.Parameters.AddWithValue("@procedurefor", "1")
        cmd.Parameters.AddWithValue("@uploaded_doc", DBNull.Value)
        cmd.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
        cmd.Parameters.AddWithValue("@doctype", "1")
        cmd.Parameters.AddWithValue("@draftletter", DBNull.Value)
        cmd.Parameters.AddWithValue("@tttds_is_final", DBNull.Value)
        cmd.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)

        returnBooleanvalue = cmd.ExecuteNonQuery()
        con.Close()
        Return returnBooleanvalue
    End Function

    Public Function SaveVoucher(ByVal Domain As String, ByVal IsOnline As String, ByVal IsUpd As String, ByVal VDate As String, ByVal SchemeId As String,
                                ByVal VType As String, ByVal TotalDr As String, ByVal TotalCr As String,
                                ByVal Narration As String, ByVal CreatedBy As String, ByVal BranchId As String,
                                ByVal DTbl As Data.DataTable, ByVal VSp As String, ByVal VoucherNo As String,
                                ByVal VoucherId As String, ByVal VoucherTypeId As String,
                                ByVal FundTranId As String, ByVal VoucherGUID As String, Optional ByVal FndTxFile As String = Nothing, Optional ByVal Functionaryid As String = Nothing, Optional ByVal voucherrefXML As String = Nothing) As DataSet
        'ByVal FundTranId As String, ByVal VoucherGUID As String, Optional ByVal FndTxFile As String = Nothing, Optional ByVal Functionaryid As String = Nothing, Optional ByVal voucherrefXML As String = Nothing, Optional ByVal strVDetail As String = Nothing, Optional ByVal strColvalueDetail As String = Nothing) As DataSet
        ''cb pradeep u on 14 sep 2012 this is not required for this function this is used only on insupdvouchernew function

        Dim VDtls As String
        Dim Para As New SqlParameter
        ' Dim dr As SqlDataReader = Nothing
        'Dim Dt As New Data.DataTable("PrintData")
        Dim wr As New System.IO.StringWriter
        DTbl.TableName = "temp"
        DTbl.WriteXml(wr)
        VDtls = wr.ToString()
        wr.Dispose()
        VDtls = Strings.Replace(VDtls, ",", "")
        VSp = Strings.Replace(VSp, ",", "")

        'Dim cmd As New SqlCommand
        'If IsUpd = False Then
        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()

        'cmd.Connection = cnn
        Dim da As SqlDataAdapter
        Dim ds As New Data.DataSet
        da = New SqlDataAdapter("YUser.InsVoucherMaster", cnn)
        'Else
        '    cmd.CommandText = "YUser.UpdVoucherMaster"
        '    cmd.Parameters.Add(New SqlParameter("@VoucherID", VoucherId))
        '    cmd.Parameters.Add(New SqlParameter("@VoucherNo", VoucherNo))
        'End If
        'Dim cnn As New SqlConnection(ConfigurationManager.AppSettings.Item("CnStr"))
        'Dim cnn As New SqlConnection(Me.Session("ConnectionString"))

        da.SelectCommand.CommandType = Data.CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 10000

        'If VoucherId = 0 And VoucherId = Nothing Then
        '    da.SelectCommand.Parameters.Add(New SqlParameter("@VoucherId", Nothing))
        'Else
        '    da.SelectCommand.Parameters.Add(New SqlParameter("@VoucherId", VoucherId))
        'End If
        'da.SelectCommand.Parameters.Add(New SqlParameter("@VoucherNo", VoucherNo))
        da.SelectCommand.Parameters.Add(New SqlParameter("@VDate", VDate))
        da.SelectCommand.Parameters.Add(New SqlParameter("@SchemeId", SchemeId))
        da.SelectCommand.Parameters.Add(New SqlParameter("@VType", VType))
        da.SelectCommand.Parameters.Add(New SqlParameter("@TotalDrAmount", TotalDr))
        da.SelectCommand.Parameters.Add(New SqlParameter("@TotalCrAmount", TotalCr))
        da.SelectCommand.Parameters.Add(New SqlParameter("@Narration", Narration))
        da.SelectCommand.Parameters.Add(New SqlParameter("@CreatedBy", CreatedBy))
        'If Me.Session.Item("@AgencyId") <> "00000" Then
        '    cmd.Parameters.Add(New SqlParameter("@BranchId", Me.Session.Item("@AgencyId")))
        'End If
        If BranchId <> "00000" Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@BranchId", BranchId))
        End If
        da.SelectCommand.Parameters.Add(New SqlParameter("@DataXML", VDtls))
        If Not VSp Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@DataColXML", VSp))
        End If
        If VoucherTypeId <> Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@VTypeId", VoucherTypeId))
        End If
        If FundTranId IsNot Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@FundTranID", FundTranId))
        End If
        If FndTxFile IsNot Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@FndTx", FndTxFile))
        End If
        If Not Functionaryid Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@Functionaryid", Functionaryid))
        Else
            da.SelectCommand.Parameters.Add(New SqlParameter("@Functionaryid", DBNull.Value))
        End If
        If Not voucherrefXML Is Nothing Then
            da.SelectCommand.Parameters.Add(New SqlParameter("@voucherrefXML", voucherrefXML))
        End If

        da.SelectCommand.Parameters.Add(New SqlParameter("@VoucherGUID", VoucherGUID))
        da.SelectCommand.Parameters.Add(New SqlParameter("@IsPrint", True))
        'If Not strVDetail Is Nothing Then
        '    da.SelectCommand.Parameters.Add(New SqlParameter("@strVdetail", strVDetail))
        'End If
        'If Not strColvalueDetail Is Nothing Then
        '    da.SelectCommand.Parameters.Add(New SqlParameter("@strColvalueDetail", strColvalueDetail))
        'End If
        'cb pradeep u on 14-sep-2012 these parameters are not required these are required to be added for function InsVoucherMasterNEW
        da.Fill(ds)
        cnn.Close()
        Return ds
    End Function

    Public Function GetPrintBill(ByVal Domain As String, ByVal IsOnline As String, ByVal SponsorBillid As String, ByVal BillType As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.TP_GetPrintBill", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If SponsorBillid Is Nothing OrElse SponsorBillid.Trim = "" Then
            cmd.Parameters.AddWithValue("@SponsorBillid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorBillid", SponsorBillid)
        End If
        cmd.Parameters.AddWithValue("@BillType", BillType)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetTraining_TIME_TABLE_FOR_WEEK(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal timetableid As String, ByVal week As String) As DataSet


        Dim ds As New Data.DataSet
        Dim cnn As SqlConnection = Get_Connection_String(Domain, isOnline)
        cnn.Open()
        Dim da As New SqlDataAdapter("[TrainingPlan].[proc_tp_print_trg_time_table]", cnn)
        da.SelectCommand.CommandType = Data.CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 8000
        If Not trainingid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", trainingid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", DBNull.Value)
        End If
        If Not timetableid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@timetableid", timetableid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@timetableid", DBNull.Value)
        End If
        If Not week Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@week", week)
        Else
            da.SelectCommand.Parameters.AddWithValue("@week", DBNull.Value)
        End If

        da.Fill(ds)
        cnn.Close()
        Return ds



    End Function

    Public Function GET_ROOM_Dashboard_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal hostalid As String, ByVal roomtypeid As String, ByVal frmdate As String, ByVal todate As String, ByVal procedure As String, ByVal Isapk As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_room_occupancy", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If hostalid = "NULL" Then
            cmd.Parameters.AddWithValue("@hostelid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hostelid", hostalid)
        End If

        If roomtypeid = "NULL" Then
            cmd.Parameters.AddWithValue("@roomtypeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@roomtypeid", roomtypeid)
        End If

        cmd.Parameters.AddWithValue("@FromDate", frmdate)
        cmd.Parameters.AddWithValue("@ToDate", todate)
        cmd.Parameters.AddWithValue("@procedurefor", procedure)
        cmd.Parameters.AddWithValue("@isapk", Isapk)

        da.Fill(ds)
        con.Close()
        Return ds
    End Function


    Public Function UpdateReceipt(ByVal Domain As String, ByVal IsOnline As String, ByVal mptcxml As String) As Boolean
        Dim returnBooleanvalue As Boolean
        con = Get_Connection_String(Domain, IsOnline)
        cmd = New SqlCommand("TRAININGPLAN.TP_Update_receipt", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@mptcxml", mptcxml)
        returnBooleanvalue = cmd.ExecuteNonQuery()
        con.Close()
        Return returnBooleanvalue
    End Function

    Public Function GETINSTRUMENTDETAIL(ByVal Domain As String, ByVal IsOnline As String, ByVal Sponsorid As String, ByVal Fromdt As String, ByVal todt As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TRAININGPLAN.TP_GETINSTRUMENTDETAIL", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Sponsorid Is Nothing Then
            cmd.Parameters.AddWithValue("@Sponsorid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Sponsorid", Sponsorid)
        End If

        cmd.Parameters.AddWithValue("@Fromdt", Fromdt)
        cmd.Parameters.AddWithValue("@todt", todt)
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dt)
        con.Close()
        Return dt
    End Function


    Public Function Save_Reminder(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal Branchid As String) As String

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        Dim BillReminderno As String = GetTraningBILLNO(Domain, IsOnline, Common.getDateTime.Date, Branchid, 2)
        cmd.Parameters.AddWithValue("@Reminderno", BillReminderno)
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return BillReminderno

    End Function
    Public Function GetBillReminder(ByVal Domain As String, ByVal IsOnline As String, ByVal sponsorID As String, ByVal Billtype As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetBillStatusReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If sponsorID Is Nothing Or sponsorID = "NULL" Then
            cmd.Parameters.AddWithValue("@sponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorID", sponsorID)
        End If
        cmd.Parameters.AddWithValue("@Purpose", 1)
        cmd.Parameters.AddWithValue("@Billtype", Billtype)
        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function Get_DATA_FROM_TABLES(ByVal Domain As String, ByVal IsOnline As String, ByVal tablename As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "select * from " + tablename

        Dim ds As New DataTable()
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(ds)
        cnn.Close()

        Return ds
    End Function



    Public Function TRG_SAVE_FACULTY_CONTENT(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal sessionid As String, ByVal contentstring As String, ByVal filetype As String, ByVal usertype As String, ByVal userid As String, ByVal mobno As String, ByVal facultyid As String) As Boolean
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim dttrainingdetail As New DataTable
        cmd = New SqlCommand("trainingplan.proc_tp_apk_upload_time_table_content", con)
        cmd.Parameters.AddWithValue("@TrainingId", trainingid)
        If sessionid Is Nothing Then
            cmd.Parameters.AddWithValue("@sessionid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sessionid ", sessionid)
        End If
        If contentstring Is Nothing Then
            cmd.Parameters.AddWithValue("@contentstring", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@contentstring ", contentstring)
        End If

        If filetype Is Nothing Then
            cmd.Parameters.AddWithValue("@contentfiletype", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@contentfiletype ", filetype)
        End If

        If usertype Is Nothing Then
            cmd.Parameters.AddWithValue("@usertype", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@usertype ", usertype)
        End If
        If userid Is Nothing Then
            cmd.Parameters.AddWithValue("@userid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@userid", userid)
        End If
        If mobno Is Nothing Then
            cmd.Parameters.AddWithValue("@mobileno", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@mobileno", mobno)
        End If
        cmd.Parameters.AddWithValue("@facultyid", facultyid)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function Save_Transfer_Detail_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String) As DataSet
        Dim dsSponserDetail As New DataSet
        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dsSponserDetail)
        'cmd.ExecuteNonQuery()
        cnn.Close()

        Return dsSponserDetail

    End Function


    Public Function TRG_GET_DOCUMENT_STATUS(ByVal Domain As String, ByVal isonline As String, ByVal procedurefor As String, ByVal doctypeid As String, ByVal documentid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("Declare @s int  Select @s =  [TRAININGPLAN].[fs_tp_get_training_document_status]  ('" + procedurefor + "','" + doctypeid + "',N'" + documentid + "')  Select @s  ", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        If IsDBNull(cmd.ExecuteScalar()) Then
            sTrainingPlanNo = ""
        Else
            sTrainingPlanNo = cmd.ExecuteScalar()
        End If

        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function Get_EMPLOYEE_DATA_TO_SEND_SMS_MAIL(ByVal Domain As String, ByVal IsOnline As String, ByVal employeeid As String) As DataTable

        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim dt As New DataTable
        cmd = New SqlCommand("select * from [Yuser].[F_yuser_get_employee_mobile_email] ('" + employeeid + "')", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function


    Public Function Save_Training_plan_data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal docremark As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure
        Dim isFinalDraft As String = "0"
        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1

                If dtparameterlist.Columns(j).ColumnName.ToString().ToUpper = "@UPLOADED_DOC" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then

                        If Not System.Web.HttpContext.Current.Session("Dept_Letter_Path") Is Nothing Then
                            If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                            Else
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                            End If
                            isFinalDraft = "1"
                        Else
                            If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                            Else
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                            End If
                        End If

                    End If

                Else
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                        If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                            cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                        Else
                            cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                        End If
                    End If
                End If






            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue("@tttds_is_final", isFinalDraft)
        cmd.Parameters.AddWithValue("@tttds_letter_type", System.Web.HttpContext.Current.Session("Dept_Letter_Type"))
        If (docremark = "NULL") Then
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docremark", docremark)
        End If

        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next
        cmd.Parameters.AddWithValue("@draftletter", System.Web.HttpContext.Current.Session("Dept_Letter_Draft"))

        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function

    Public Function CheckCourse_New(ByVal Domain As String, ByVal isonline As String, ByVal CourseCode As String, ByVal CourseName As String, ByVal HCourseName As String, ByVal duration As Integer, ByVal durationtype As Byte, ByVal coursecategory As String, ByVal courseid As String) As DataTable

        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingPlanno As New DataTable
        Dim da As New SqlDataAdapter("Trainingplan.TP_CheckCourse", con)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 5000
        da.SelectCommand.Parameters.AddWithValue("@CourseName", CourseName)
        da.SelectCommand.Parameters.AddWithValue("@CourseCode", CourseCode)
        da.SelectCommand.Parameters.AddWithValue("@HCourseName", HCourseName)
        da.SelectCommand.Parameters.AddWithValue("@duration", duration)
        da.SelectCommand.Parameters.AddWithValue("@durationtype", durationtype)
        da.SelectCommand.Parameters.AddWithValue("@coursecategory", coursecategory)
        da.SelectCommand.Parameters.AddWithValue("@courseid", courseid)
        da.Fill(dtTrainingPlanno)
        con.Close()
        Return dtTrainingPlanno
    End Function
    Public Function Check_TRG_CODE_EXIST(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal code As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String = ""
        cmd = New SqlCommand("select trainingid from [TrainingPlan].[f_tp_training_detail_from_code]  (Null,'" + fromdate + "','" + todate + "','" + code + "',0,null)", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        If cmd.ExecuteScalar() Is Nothing Then
            Return False
        Else
            Return True
        End If

        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function gettrainingbycode(ByVal Domain As String, ByVal isOnline As String, ByVal proposaldetailid As String, ByVal departmentid As String, ByVal code As String) As Boolean
        Dim dsSchemes As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_chk_code_on_proposal", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ttpd_id", proposaldetailid)
        cmd.Parameters.AddWithValue("@deptid", departmentid)
        cmd.Parameters.AddWithValue("@code", code)
        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        If (dsSchemes.Tables(0).Rows.Count > 0) Then
            If dsSchemes.Tables(0).Rows(0)("trainingid").ToString = "" Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function

    Public Function GET_CANCEL_HALL_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal Orderid As String, ByVal Procedurefor As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_hall_reservation_data_to_cancel", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)

        cmd.Parameters.AddWithValue("@orderid", Orderid)
        cmd.Parameters.AddWithValue("@procedurefor", Procedurefor)

        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function Save_Hall_CheckINOUT(ByVal Domain As String, ByVal isOnline As String, ByVal ReservationOrderId As String, ByVal CheckStatus As String, ByVal Checkouttime As String, ByVal CreatedBy As String, ByVal Hallid As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_hall_check_inout]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@tthbm_id", ReservationOrderId)
        cmd.Parameters.AddWithValue("@checkinout", CheckStatus)
        cmd.Parameters.AddWithValue("@checkinouttime", Checkouttime)
        cmd.Parameters.AddWithValue("@createby", CreatedBy)
        cmd.Parameters.AddWithValue("@hallid", Hallid)

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function



    Public Function SAVE_RESERVATION_BILL(ByVal Domain As String, ByVal IsOnline As String, ByVal orderid As String, ByVal chargexml As String, ByVal billdate As String, ByVal createdby As String, ByVal isFinal As String) As Boolean
        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        cmd = New SqlCommand("Trainingplan.proc_tp_save_hall_charges_bill", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Connection = cnn
        cmd.Parameters.AddWithValue("@orderid", orderid)
        cmd.Parameters.AddWithValue("@charges", chargexml)
        cmd.Parameters.AddWithValue("@billdate", billdate)
        cmd.Parameters.AddWithValue("@cretedby", createdby)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
        cmd.Parameters.AddWithValue("@isfinal", isFinal)

        cmd.ExecuteNonQuery()
        cnn.Close()
        cmd.Dispose()
        Return True

    End Function

    Public Function InsUpdAgency_STAFF(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyName As String, ByVal HAgencyName As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal RoleID As String, ByVal ColumnValXML As String, ByVal UserCode As String, ByVal isdisabled As String, ByVal password As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_trg_ins_upd_staff", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyId", AgencyId)
        cmd.Parameters.AddWithValue("@AgencyName", AgencyName)
        If HAgencyName.Trim = "" Then
            cmd.Parameters.AddWithValue("@HAgencyName", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@HAgencyName", HAgencyName)
        End If

        cmd.Parameters.AddWithValue("@AgencyTypeID", AgencyTypeID)
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@ParentID", ParentID)
        cmd.Parameters.AddWithValue("@RoleID", RoleID)
        If (UserCode Is Nothing) Then
            cmd.Parameters.AddWithValue("@UserCode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@UserCode", UserCode)
        End If

        cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        cmd.Parameters.AddWithValue("@isdisable", isdisabled)

        cmd.Parameters.AddWithValue("@uploadpath", DBNull.Value)
        'cmd.Parameters.AddWithValue("@uploadpath", "2")
        If Not password Is Nothing Then
            cmd.Parameters.AddWithValue("@password", password)
        Else
            cmd.Parameters.AddWithValue("@password", DBNull.Value)
        End If

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function GET_TRG_DATA(ByVal Domain As String, ByVal isonline As String, ByVal Trainingid As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dt As New DataTable
        Dim sTrainingPlanNo As String = ""
        cmd = New SqlCommand("select * from [TrainingPlan].[f_tp_training_detail_from_code]  (Null,Null,Null,Null,0,'" + Trainingid + "')", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function GET_ROOM_Dashboard_Data_With_TRG(ByVal Domain As String, ByVal IsOnline As String, ByVal hostalid As String, ByVal roomtypeid As String, ByVal frmdate As String, ByVal todate As String, ByVal procedure As String, ByVal Isapk As String, ByVal trgid As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_room_occupancy", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If hostalid = "NULL" Then
            cmd.Parameters.AddWithValue("@hostelid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hostelid", hostalid)
        End If

        If roomtypeid = "NULL" Then
            cmd.Parameters.AddWithValue("@roomtypeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@roomtypeid", roomtypeid)
        End If

        cmd.Parameters.AddWithValue("@FromDate", frmdate)
        cmd.Parameters.AddWithValue("@ToDate", todate)
        cmd.Parameters.AddWithValue("@procedurefor", procedure)
        cmd.Parameters.AddWithValue("@isapk", Isapk)
        If (trgid.ToString <> "") Then
            cmd.Parameters.AddWithValue("@trgid", trgid)
        End If


        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GET_ROOM_Reservation_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal hostalid As String, ByVal roomtypeid As String, ByVal frmdate As String, ByVal todate As String, ByVal procedure As String, ByVal Isapk As String, ByVal trgid As String, ByVal roomid As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_room_occupancy", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If roomid = "NULL" Then
            cmd.Parameters.AddWithValue("@roomid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@roomid", roomid)
        End If

        If hostalid = "NULL" Then
            cmd.Parameters.AddWithValue("@hostelid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hostelid", hostalid)
        End If

        If roomtypeid = "NULL" Then
            cmd.Parameters.AddWithValue("@roomtypeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@roomtypeid", roomtypeid)
        End If

        cmd.Parameters.AddWithValue("@FromDate", frmdate)
        cmd.Parameters.AddWithValue("@ToDate", todate)
        cmd.Parameters.AddWithValue("@procedurefor", procedure)
        cmd.Parameters.AddWithValue("@isapk", Isapk)
        If (trgid.ToString <> "") Then
            cmd.Parameters.AddWithValue("@trgid", trgid)
        End If


        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function GetTrainingPlanDetails_From_PROPOSAL(ByVal Domain As String, ByVal isOnline As String, proposaldetaillid As String) As DataSet
        Dim dsTrainingDetails As New DataSet
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].proc_tp_get_proposal_info", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ttpd_id", proposaldetaillid)
        cmd.Parameters.AddWithValue("@Fromdate", DBNull.Value)
        cmd.Parameters.AddWithValue("@todate", DBNull.Value)
        cmd.Parameters.AddWithValue("@procedurefor", "0")
        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dsTrainingDetails)
        con.Close()
        Return dsTrainingDetails
    End Function


    Public Function gettrainingbycode_On_Proposal(ByVal Domain As String, ByVal isOnline As String, ByVal proposaldetailid As String, ByVal code As String) As String
        Dim dsSchemes As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_chk_code_on_proposal", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ttpd_id", proposaldetailid)
        cmd.Parameters.AddWithValue("@code", code)
        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        If (dsSchemes.Tables(0).Rows.Count > 0) Then
            Return dsSchemes.Tables(0).Rows(0)("trainingid").ToString
        Else
            Return ""
        End If

    End Function

    Public Function GetTrainingPlanCode_New(ByVal Domain As String, ByVal isOnline As String, ByVal ParentId As String, ByVal BranchID As String, ByVal SchemeId As String, ByVal ForAcademy As String, ByVal Fromdate As String, ByVal Todate As String, ByVal procedurefor As String) As DataTable

        Dim con As SqlConnection = Get_Connection_String(Domain, isOnline)
        con.Open()
        Dim dtTrainingPlanno As New DataTable
        Dim da As New SqlDataAdapter("TrainingPlan.GetTrainingPlanNo", con)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 5000
        da.SelectCommand.Parameters.AddWithValue("@ParentId", ParentId)
        da.SelectCommand.Parameters.AddWithValue("@BranchId", BranchID)
        da.SelectCommand.Parameters.AddWithValue("@ForAcademy", 1)
        If Not SchemeId = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@SchemeId", UCase(SchemeId))
        End If
        If Fromdate = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@Fromdate", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@Fromdate", Fromdate)
        End If
        If Todate = Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@Todate", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@Todate", Todate)
        End If
        da.SelectCommand.Parameters.AddWithValue("@procedurefor", procedurefor)
        da.Fill(dtTrainingPlanno)
        con.Close()
        Return dtTrainingPlanno
    End Function

    Public Function CHECK_HONORARIUM_STATUS(ByVal Domain As String, ByVal isonline As String, ByVal docid As String, ByVal procedurefor As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("declare @isexi int select  @isexi =   [TrainingPlan].[fs_tp_get_honorarium_payment_status] ('" + docid + "','" + procedurefor + "') select @isexi", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function Save_Data_REF(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If

        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        'cmd.ExecuteNonQuery()
        'cnn.Close()
        Dim dthoorder As New DataTable
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dthoorder)
        con.Close()
        Return dthoorder
    End Function

    Public Function CHECK_REGISTERED_MOBILE_NO_FOR_PARTICIPANT(ByVal Domain As String, ByVal isonline As String, ByVal Mobileno As String) As DataTable
        Dim dtparticipant As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        cmd = New SqlCommand("select * from [TrainingPlan].[f_tp_chk_rmn_for_feedback] ('" + Mobileno + "')", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtparticipant)
        con.Close()
        Return dtparticipant
    End Function

    Public Function GET_REQUEST_ROOM_Reservation_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal hostalid As String, ByVal roomtypeid As String, ByVal frmdate As String, ByVal todate As String, ByVal procedure As String, ByVal Isapk As String, ByVal trgid As String, ByVal roomid As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_room_occupancy", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If roomid = "NULL" Then
            cmd.Parameters.AddWithValue("@roomid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@roomid", roomid)
        End If

        If hostalid = "NULL" Then
            cmd.Parameters.AddWithValue("@hostelid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hostelid", hostalid)
        End If

        If roomtypeid = "NULL" Then
            cmd.Parameters.AddWithValue("@roomtypeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@roomtypeid", roomtypeid)
        End If

        cmd.Parameters.AddWithValue("@FromDate", frmdate)
        cmd.Parameters.AddWithValue("@ToDate", todate)
        cmd.Parameters.AddWithValue("@procedurefor", procedure)
        cmd.Parameters.AddWithValue("@isapk", Isapk)
        If trgid = "NULL" Then
            cmd.Parameters.AddWithValue("@trgid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trgid", trgid)
        End If

        da.Fill(ds)
        con.Close()
        Return ds
    End Function

    Public Function Get_Participant_data(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingplanid As String, ByVal ParticipantId As String, ByVal Procedurefor As String, ByVal weekid As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Then
            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingid", trainingplanid)
        End If
        If ParticipantId Is Nothing Then
            cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ParticipantId", ParticipantId)
        End If

        cmd.Parameters.AddWithValue("@procedurefor", Procedurefor)
        cmd.Parameters.AddWithValue("@weekid", weekid)
        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function

    Public Function GetProposal_Report(ByVal Domain As String, ByVal IsOnline As String, ByVal Proposalid As String, ByVal FrDate As String, ByVal TDate As String, ByVal Procedurefor As String, ByVal SponsorId As String, ByVal Coursedirectorid As String, ByVal Status As String) As DataTable
        Dim dsTrainingDetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].proc_tp_get_proposal_info", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Proposalid = "NULL" Then
            cmd.Parameters.AddWithValue("@ttpd_id", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ttpd_id", Proposalid)
        End If
        cmd.Parameters.AddWithValue("@Fromdate", FrDate)
        cmd.Parameters.AddWithValue("@todate", TDate)
        cmd.Parameters.AddWithValue("@procedurefor", Procedurefor)
        If SponsorId = "NULL" Then
            cmd.Parameters.AddWithValue("@sponsorid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorid", SponsorId)
        End If
        If Coursedirectorid = "NULL" Then
            cmd.Parameters.AddWithValue("@coursedirectorid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@coursedirectorid", Coursedirectorid)
        End If

        If Status = "NULL" Then
            cmd.Parameters.AddWithValue("@status", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@status", Status)
        End If

        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dsTrainingDetails)
        con.Close()
        Return dsTrainingDetails
    End Function

    Public Function GET_TRAINING_MINIMUM_PARTICIPANT(ByVal Domain As String, ByVal isonline As String, ByVal TrainingCategory As String, ByVal Duration As String, ByVal DurationType As String, ByVal effectivedate As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dt As New DataTable
        cmd = New SqlCommand("select * from Trainingplan.[ft_tp_get_min_participant]  ('" + TrainingCategory + "'," + Duration + ",'" + DurationType + "','" + effectivedate + "') ", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function GetBillReminder_New(ByVal Domain As String, ByVal IsOnline As String, ByVal sponsorID As String, ByVal Billtype As String, ByVal Reminderid As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetBillStatusReport", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
        If sponsorID Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorID", sponsorID)
        End If
        cmd.Parameters.AddWithValue("@Purpose", 3)
        cmd.Parameters.AddWithValue("@Billtype", Billtype)
        If Reminderid Is Nothing Then
            cmd.Parameters.AddWithValue("@reminderid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@reminderid", Reminderid)
        End If

        da.Fill(ds)
        con.Close()
        Return ds
    End Function


#Enable Warning BC42105 ' Function 'SendMail_With_CC' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.
    Public Function TRAINING_STATUS_REPORTS(ByVal Domain As String, ByVal IsOnline As String, ByVal financialyear As String, ByVal coursedirectorid As String, ByVal sponsortypeid As String, ByVal sponsorid As String, ByVal procedurefor As String, ByVal week As String) As DataSet
        Dim dtTraining As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_monthly_status]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        cmd.Parameters.AddWithValue("@financialyear", financialyear)
        If week = "NULL" Or week = "00000" Then
            cmd.Parameters.AddWithValue("@week", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@week", week)
        End If

        If coursedirectorid = "NULL" Or coursedirectorid = "00000" Then
            cmd.Parameters.AddWithValue("@coursedirectorid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@coursedirectorid", coursedirectorid)
        End If

        If sponsorid = "NULL" Or sponsorid = "00000" Then
            cmd.Parameters.AddWithValue("@sponsorrid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@sponsorrid", sponsorid)
        End If

        If sponsortypeid = "NULL" Or sponsortypeid = "00000" Then
            cmd.Parameters.AddWithValue("@SponsorType", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@SponsorType", sponsortypeid)
        End If

        If procedurefor = "NULL" Then
            cmd.Parameters.AddWithValue("@procedurefor", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        End If



        Dim daTraining As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTraining.Fill(dtTraining)
        con.Close()
        Return dtTraining
    End Function

    Public Function Save_Common_Data_With_Single_XML_RefNo(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal xmlparam As String, ByVal Xmlvalue As String, ByVal CreatedonParam As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue(xmlparam, Xmlvalue)




        'cmd.ExecuteNonQuery()
        'cnn.Close()

        Dim dthoorder As New DataTable
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dthoorder)
        con.Close()
        Return dthoorder

    End Function

    Public Function GET_TRAINING_FEEDBACK_STANDARD_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal effectivedate As String, ByVal trainingid As String, ByVal participantid As String) As DataSet
        Dim dtFeedbackStandard As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetFeedbackStandards", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@EffectiveDate", effectivedate)
        If trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@trainingplanid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingplanid", trainingid)
        End If
        If participantid Is Nothing Then
            cmd.Parameters.AddWithValue("@participantid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@participantid", participantid)
        End If

        Dim daFeedback As SqlDataAdapter = New SqlDataAdapter(cmd)
        daFeedback.Fill(dtFeedbackStandard)
        con.Close()
        Return dtFeedbackStandard
    End Function

    Public Function Save_Common_Data_With_DSC(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal Encrypted_Hash_A As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        If Not Encrypted_Hash_A Is Nothing Then
            cmd.Parameters.AddWithValue("@docremarkenc", Encrypted_Hash_A)
        Else
            cmd.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
        End If


        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function

    Public Function GET_DSC_INFO(ByVal Domain As String, ByVal IsOnline As String, ByVal userid As String) As DataSet
        Dim ds As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()

        cmd = New SqlCommand("Yuser.proc_get_tbl_yuser_dsc_info", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)

        cmd.Parameters.AddWithValue("@tydi_user_id", userid)


        da.Fill(ds)
        con.Close()
        Return ds
    End Function
    Public Function Save_Common_Data_With_DSC_AND_LETTER(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal SignDoc As String, ByVal SignLetter As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        If Not SignDoc Is Nothing Then
            cmd.Parameters.AddWithValue("@docremarkenc", SignDoc)
        Else
            cmd.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
        End If
        If Not SignLetter Is Nothing Then
            cmd.Parameters.AddWithValue("@draftletterenc", SignLetter)
        Else
            cmd.Parameters.AddWithValue("@draftletterenc", DBNull.Value)
        End If

        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function



    Public Function Save_Training_plan_data_With_DSC(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal docremark As String, ByVal signDoc As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure
        Dim isFinalDraft As String = "0"
        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1

                If dtparameterlist.Columns(j).ColumnName.ToString().ToUpper = "@UPLOADED_DOC" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then

                        If Not System.Web.HttpContext.Current.Session("Dept_Letter_Path") Is Nothing Then
                            If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                            Else
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                            End If
                            isFinalDraft = "1"
                        Else
                            If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                            Else
                                cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                            End If
                        End If

                    End If

                Else
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                        If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                            cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                        Else
                            cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                        End If
                    End If
                End If






            Next
        Next
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If



        cmd.Parameters.AddWithValue("@tttds_is_final", isFinalDraft)
        cmd.Parameters.AddWithValue("@tttds_letter_type", System.Web.HttpContext.Current.Session("Dept_Letter_Type"))
        If (docremark = "NULL") Then
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docremark", docremark)
        End If

        If Not signDoc Is Nothing Then
            cmd.Parameters.AddWithValue("@docremarkenc", signDoc)
        Else
            cmd.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
        End If
        If Not System.Web.HttpContext.Current.Session("Dept_Letter_Draft_enc") Is Nothing Then
            cmd.Parameters.AddWithValue("@draftletterenc", System.Web.HttpContext.Current.Session("Dept_Letter_Draft_enc"))
        Else
            cmd.Parameters.AddWithValue("@draftletterenc", DBNull.Value)
        End If


        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next
        cmd.Parameters.AddWithValue("@draftletter", System.Web.HttpContext.Current.Session("Dept_Letter_Draft"))

        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function



    Public Function Get_CD_Availability_data(ByVal Domain As String, ByVal IsOnline As String, ByVal cdid As String, ByVal fromdate As String, ByVal Todate As String, ByVal Searchdate As String, ByVal Procedurefor As String) As DataSet
        Dim dtcddta As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_cd_availability]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not cdid Is Nothing Then
            cmd.Parameters.AddWithValue("@cdid", cdid)
        Else
            cmd.Parameters.AddWithValue("@cdid", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@FromDate", fromdate)
        cmd.Parameters.AddWithValue("@ToDate", Todate)
        cmd.Parameters.AddWithValue("@procedurefor", Procedurefor)

        If Not Searchdate Is Nothing Then
            cmd.Parameters.AddWithValue("@searchdate", Searchdate)
        Else
            cmd.Parameters.AddWithValue("@searchdate", DBNull.Value)
        End If
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcddta)
        con.Close()
        Return dtcddta
    End Function
    Public Function Get_Rights(ByVal Domain As String, ByVal IsOnline As String, ByVal formroleid As String, ByVal formid As String, ByVal userid As String) As DataSet
        Dim dtcddta As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("YUser.GetFormRights", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@FormID", formid)
        cmd.Parameters.AddWithValue("@FormRoleID", formroleid)
        cmd.Parameters.AddWithValue("@userid", userid)
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcddta)
        con.Close()
        Return dtcddta
    End Function

    Public Function GetTraningReceiptNO(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingDate As DateTime, ByVal BranchId As String) As String
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("Select TrainingPlan.[F_getReceiptNo] (@Date, @BranchId) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = TrainingDate
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        sTrainingPlanNo = cmd.ExecuteScalar()
        con.Close()
        Return sTrainingPlanNo
    End Function

    Public Function SaveReceipt(ByVal ReceiptId As String, ByVal ReceiptDate As Date, ByVal ReceiptNo As String,
ByVal SponsorId As String, ByVal PaymentType As Byte, ByVal Letterno As String, ByVal Createdby As String, ByVal Branchid As String, ByVal Paymentxml As String,
ByVal Instrumentxml As String, ByVal Remark As String, ByVal Receipttype As Byte, ByVal Schemeid As String, ByVal LetterReceivedDate As Date, ByVal tald_head_id As String, ByVal ReceiptNo_Auto As String, ByVal tald_ledger_id As String,
ByVal Docremark As String, ByVal DocDate As String, ByVal EmpId As String, ByVal ForwardEmpId As String, ByVal TypeId As String, ByVal DocStatus As String, ByVal Procedurefor As String, ByVal UploadDoc As String, ByVal UploadDocName As String, ByVal DocType As String, ByVal Draftletter As String, ByVal IsFinal As String, ByVal LetterType As String, ByVal Domain As String, ByVal IsOnline As String) As Boolean
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[TP_SaveReceipt]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ReceiptId", ReceiptId)

        If ReceiptDate = Nothing Then
            cmd.Parameters.AddWithValue("@ReceiptDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ReceiptDate", ReceiptDate)
        End If


        If ReceiptNo Is Nothing Then
            cmd.Parameters.AddWithValue("@ReceiptNo", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo)
        End If


        cmd.Parameters.AddWithValue("@SponsorId", SponsorId)
        cmd.Parameters.AddWithValue("@PaymentType", PaymentType)
        cmd.Parameters.AddWithValue("@Letterno", Letterno)
        cmd.Parameters.AddWithValue("@Createdby", Createdby)
        cmd.Parameters.AddWithValue("@Branchid", Branchid)
        If Instrumentxml Is Nothing Then
            cmd.Parameters.AddWithValue("@Instrumentxml", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Instrumentxml", Instrumentxml)
        End If

        cmd.Parameters.AddWithValue("@Paymentxml", Paymentxml)
        cmd.Parameters.AddWithValue("@Remark", Remark)
        cmd.Parameters.AddWithValue("@Receipttype", Receipttype)
        If Schemeid Is Nothing Then
            cmd.Parameters.AddWithValue("@Schemeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Schemeid", Schemeid)
        End If
        cmd.Parameters.AddWithValue("@LetterReceivedDate", LetterReceivedDate)

        If tald_head_id Is Nothing Then
            cmd.Parameters.AddWithValue("@tald_head_id ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@tald_head_id ", tald_head_id)
        End If

        If tald_ledger_id Is Nothing Then
            cmd.Parameters.AddWithValue("@tald_ledger_id ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@tald_ledger_id ", tald_ledger_id)
        End If


        cmd.Parameters.AddWithValue("@Receiptno_auto", ReceiptNo_Auto)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function Get_EMPLOYEE_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal agencyid As String, ByVal tillDate As String, ByVal isWithLeftRequired As String) As DataTable

        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim dt As New DataTable
        If agencyid Is Nothing Then
            cmd = New SqlCommand("select * from YUSER.[HR_GetEmployeeDetails] (null,'" + tillDate + "','" + isWithLeftRequired + "')", con)
        Else
            cmd = New SqlCommand("select * from YUSER.[HR_GetEmployeeDetails] ('" + agencyid + "','" + tillDate + "','" + isWithLeftRequired + "')", con)
        End If


        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daCourse As SqlDataAdapter = New SqlDataAdapter(cmd)
        daCourse.Fill(dt)
        con.Close()
        Return dt
    End Function


    Public Function get_Payment_Status_Data(ByVal Domain As String, ByVal isonline As String, ByVal trainingplanid As String, ByVal facultyid As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_honorarium_payment_statu_data]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Then
            cmd.Parameters.AddWithValue("@trg_id", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trg_id", trainingplanid)
        End If
        If facultyid Is Nothing Then
            cmd.Parameters.AddWithValue("@facultyid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@facultyid", facultyid)
        End If

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function


    Public Function Get_DATA_FROM_TABLE_NEW(ByVal Domain As String, ByVal IsOnline As String, ByVal tablename As String) As DataTable

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = tablename

        Dim ds As New DataTable()
        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(ds)
        cnn.Close()

        Return ds
    End Function



    Public Function getHonorariumReceipt(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String, ByVal fromdate As String, ByVal todate As String, ByVal employeeid As String, ByVal honorariumid As String, ByVal procedurefor As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_honorarium_data]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingid Is Nothing Or trainingid = "" Then
            cmd.Parameters.AddWithValue("@trainingplanid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingplanid", trainingid)
        End If
        If fromdate Is Nothing Or fromdate = "" Then
            cmd.Parameters.AddWithValue("@fromdt", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@fromdt", fromdate)
        End If

        If todate Is Nothing Or todate = "" Then
            cmd.Parameters.AddWithValue("@todt", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@todt", todate)
        End If

        If employeeid Is Nothing Or employeeid = "" Then
            cmd.Parameters.AddWithValue("@employeeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@employeeid", employeeid)
        End If
        If honorariumid Is Nothing Or honorariumid = "" Then
            cmd.Parameters.AddWithValue("@ttshdm_id", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ttshdm_id", honorariumid)
        End If

        cmd.Parameters.AddWithValue("@procedurefor", procedurefor)

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function


    Public Function get_faculty_feedback_Data(ByVal Domain As String, ByVal isonline As String, ByVal facultyid As String, ByVal fromdate As String, ByVal todate As String, ByVal status As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_Get_faculty_feedback]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@facultyid", facultyid)
        cmd.Parameters.AddWithValue("@fromdt", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@feedbackstatus", status)


        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function
    Public Function GET_CD_CHARGE_Data(ByVal Domain As String, ByVal IsOnline As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[YUser].[proc_yuser_get_hr_delegated_department]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@thdd_emp_id", DBNull.Value)
        cmd.Parameters.AddWithValue("@procedurefor", "2")

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function CHECK_REGISTERED_MOBILE_NO_OR_CODE_FOR_VIRTUAL_CLASS(ByVal Domain As String, ByVal isonline As String, ByVal checkval As String, ByVal type As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingAmount As New DataTable
        cmd = New SqlCommand("declare @isexi int select  @isexi =  [TrainingPlan].[f_tp_chk_duplicate_entry_for_virtual_class] ('" + checkval + "','" + type + "') select @isexi ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000



        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtTrainingAmount)
        con.Close()
        Return dtTrainingAmount
    End Function
    Public Function SaveSchemeDetails(ByVal Domain As String, ByVal IsOnline As String, ByVal Schemeid As String, ByVal SchemeName As String, ByVal HSchemeName As String, ByVal IsFCRA As String, ByVal createdBy As String, ByVal ForAllLedger As String, ByVal RoleID As String, ByVal UmbrelaSchemeid As String, ByVal SchemeAlias As String, ByVal SchemeAliasHindi As String, ByVal sponsortype As String, ByVal fromyeardate As String, ByVal toyeardate As String, ByVal ConvergedScheme As String, ByVal schemecoverage As String, ByVal Isschemecoverage As Integer) As Boolean
        Dim flag As Boolean = False
        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.CommandText = "yuser_insupdscheme"
            cmd.Parameters.AddWithValue("v_SchemeID", Schemeid)
            cmd.Parameters.AddWithValue("v_SchemeName", SchemeName)
            If HSchemeName Is Nothing Then
                cmd.Parameters.AddWithValue("v_HSchemeName", Nothing)
            Else
                cmd.Parameters.AddWithValue("v_HSchemeName", HSchemeName)
            End If

            cmd.Parameters.AddWithValue("v_IsFCRA", IsFCRA)
            cmd.Parameters.AddWithValue("v_CreatedBy", createdBy)
            cmd.Parameters.AddWithValue("v_ForAllLedger", ForAllLedger)
            cmd.Parameters.AddWithValue("v_RoleId", RoleID)
            cmd.Parameters.AddWithValue("v_umbrelaScheme", UmbrelaSchemeid)
            cmd.Parameters.AddWithValue("v_alias", SchemeAlias)
            cmd.Parameters.AddWithValue("v_halias", SchemeAliasHindi)
            cmd.Parameters.AddWithValue("v_sponsortype", sponsortype)
            cmd.Parameters.AddWithValue("v_fromyeardate", fromyeardate)
            cmd.Parameters.AddWithValue("v_toyeardate", toyeardate)
            cmd.Parameters.AddWithValue("v_ConvergedScheme", ConvergedScheme)
            cmd.Parameters.AddWithValue("v_schemecoverage", schemecoverage)
            cmd.Parameters.AddWithValue("v_Isconvergence", Isschemecoverage)
            cmd.Parameters.AddWithValue("v_agencyid", Nothing)
            cmd.Connection = con
            con.Open()
            flag = cmd.ExecuteNonQuery()
            con.Close()
        Else
            Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
            cnn.Open()
            Dim cmd As New SqlCommand("yuser.insupdscheme", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@SchemeID", Schemeid)
            cmd.Parameters.AddWithValue("@SchemeName", SchemeName)
            If HSchemeName Is Nothing Then
                cmd.Parameters.AddWithValue("@HSchemeName", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@HSchemeName", HSchemeName)
            End If

            cmd.Parameters.AddWithValue("@IsFCRA", IsFCRA)
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy)
            cmd.Parameters.AddWithValue("@ForAllLedger", ForAllLedger)
            cmd.Parameters.AddWithValue("@RoleId", RoleID)
            cmd.Parameters.AddWithValue("@umbrelaScheme", UmbrelaSchemeid)
            cmd.Parameters.AddWithValue("@alias", SchemeAlias)
            cmd.Parameters.AddWithValue("@halias", SchemeAliasHindi)

            cmd.Parameters.AddWithValue("@sponsortype", sponsortype)
            cmd.Parameters.AddWithValue("@fromyeardate", fromyeardate)
            cmd.Parameters.AddWithValue("@toyeardate", toyeardate)
            cmd.Parameters.AddWithValue("@ConvergedScheme", ConvergedScheme)
            cmd.Parameters.AddWithValue("@schemecoverage", schemecoverage)
            cmd.Parameters.AddWithValue("@Isconvergence", Isschemecoverage)
            flag = cmd.ExecuteNonQuery()
            cnn.Close()
        End If
        Return flag

    End Function
    Public Function GetSchemeDetails(ByVal Domain As String, ByVal IsOnline As String, ByVal Schemeid As String) As DataSet
        Dim dsSchemeMaster As New DataSet
        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.CommandText = "yuser_proc_get_particular_scheme_data"
            cmd.Parameters.AddWithValue("v_schemeid", Schemeid)
            cmd.Connection = con
            con.Open()
            Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dsSchemeMaster)
            con.Close()
        Else
            Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
            cnn.Open()
            Dim cmd As New SqlCommand("yuser.proc_get_particular_scheme_data", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.Parameters.AddWithValue("@schemeid", Schemeid)
            Dim daTaxMaster As SqlDataAdapter = New SqlDataAdapter(cmd)
            daTaxMaster.Fill(dsSchemeMaster)
            cnn.Close()
        End If
        Return dsSchemeMaster
    End Function

    Public Function GetRole(ByVal Domain As String, ByVal IsOnline As String, ByVal id As String, ByVal RoleID As String, ByVal Type As String) As DataSet
        Dim dsRole As New DataSet

        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.CommandText = "yuser_checkledgerRole"
            If id = "" Or id = Nothing Then
                cmd.Parameters.AddWithValue("v_Id", Nothing)
            Else
                cmd.Parameters.AddWithValue("v_Id", id)
            End If
            If RoleID = "" Or RoleID = Nothing Then
                cmd.Parameters.AddWithValue("v_RoleID", Nothing)
            Else
                cmd.Parameters.AddWithValue("v_RoleID", RoleID)
            End If

            If Type = "" Or Type = Nothing Then
                cmd.Parameters.AddWithValue("v_type", Nothing)
            Else
                cmd.Parameters.AddWithValue("v_type", Type)
            End If
            cmd.Connection = con
            con.Open()
            Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dsRole)
            con.Close()
        Else
            Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
            con.Open()
            Dim cmd As New SqlCommand("yuser.checkledgerRole", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = con
            cmd.CommandTimeout = 5000
            If id = "" Or id = Nothing Then
                cmd.Parameters.AddWithValue("@Id", Nothing)
            Else
                cmd.Parameters.AddWithValue("@Id", id)
            End If
            If RoleID = "" Or RoleID = Nothing Then
                cmd.Parameters.AddWithValue("@RoleID", Nothing)
            Else
                cmd.Parameters.AddWithValue("@RoleID", RoleID)
            End If

            If Type = "" Or Type = Nothing Then
                cmd.Parameters.AddWithValue("@type", Nothing)
            Else
                cmd.Parameters.AddWithValue("@type", Type)
            End If

            Dim daAgencyType As SqlDataAdapter = New SqlDataAdapter(cmd)
            daAgencyType.Fill(dsRole)
            con.Close()
        End If
        Return dsRole
    End Function

    Public Function DeleteSchemeDetails(ByVal Domain As String, ByVal IsOnline As String, ByVal Schemeid As String, ByVal RoleID As String) As Boolean
        Dim flag As Boolean
        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(General.GetSessionValue("Domain"), General.GetSessionValue("IsOnline"))
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.CommandText = "yuser_delete_scheme"
            cmd.Parameters.AddWithValue("v_SchemeID", Schemeid)
            cmd.Parameters.AddWithValue("v_RoleId", RoleID)
            cmd.Connection = con
            con.Open()
            flag = cmd.ExecuteNonQuery()
            con.Close()
        Else
            Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
            con.Open()
            Dim cmd As New SqlCommand("yuser.delete_scheme", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@SchemeID", Schemeid)
            cmd.Parameters.AddWithValue("@RoleId", RoleID)
            flag = cmd.ExecuteNonQuery()
            con.Close()
        End If
        Return flag
    End Function
    Public Function TRG_getparticipants_Datewise(ByVal Domain As String, ByVal IsOnline As String, ByVal trg_date As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_Get_datewise_participant", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trg_date", trg_date)
        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function


    Public Function Get_Training_StakeHolder_Mail(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingPlanid As String, ByVal Sessionid As String) As DataTable

        Dim dt As New DataTable
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()

        Dim da As New SqlDataAdapter("[TrainingPlan].[trg_lms_get_training_contact_details]", con)
        da.SelectCommand.CommandTimeout = 8000

        da.SelectCommand.CommandType = CommandType.StoredProcedure

        If TrainingPlanid = "NULL" Then
            da.SelectCommand.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@trainingid", TrainingPlanid)

        End If

        If Sessionid = "NULL" Then
            da.SelectCommand.Parameters.AddWithValue("@sessionid", DBNull.Value)
        Else
            da.SelectCommand.Parameters.AddWithValue("@sessionid", Sessionid)
        End If


        da.Fill(dt)
        con.Close()
        Return dt
    End Function



    Public Function TRG_CHECK_FEES_STATUS(ByVal Domain As String, ByVal isonline As String, ByVal feesid As String) As String
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim sTrainingPlanNo As String
        cmd = New SqlCommand("Declare @s int  Select @s =  [TRAININGPLAN].[F_proc_tp_check_used_fees]  ('" + feesid + "')  Select @s  ", con)

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        If IsDBNull(cmd.ExecuteScalar()) Then
            sTrainingPlanNo = ""
        Else
            sTrainingPlanNo = cmd.ExecuteScalar()
        End If

        con.Close()
        Return sTrainingPlanNo
    End Function


    Public Function chk_reg_mob_no_new(ByVal Domain As String, ByVal isonline As String, ByVal mobileno As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim da As New SqlDataAdapter("select  * from  [TrainingPlan].[f_tp_participant_data_by_rmn] ('" + mobileno + "')", con)
        da.SelectCommand.CommandTimeout = 8000

        da.SelectCommand.CommandType = CommandType.Text
        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function chk_reg_mailid(ByVal Domain As String, ByVal isonline As String, ByVal mailid As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim da As New SqlDataAdapter("select  * from  [TrainingPlan].[f_tp_participant_data_by_email] ('" + mailid + "')", con)
        da.SelectCommand.CommandTimeout = 8000

        da.SelectCommand.CommandType = CommandType.Text
        da.Fill(dt)
        con.Close()
        Return dt
    End Function



    Public Function Save_User_Reg_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal password As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()
        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.Replace("'", ""))
                    End If


                End If
            Next
        Next
        cmd.Parameters.AddWithValue("@password", password)
        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function

    Public Function GetTrainingCalender_LMS(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal loginuserid As String, ByVal usertype As String, ByVal orderby As String) As DataTable

        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_lms_get_training_calendar", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        cmd.Parameters.AddWithValue("@Usertype", usertype)
        cmd.Parameters.AddWithValue("@orderby", orderby)



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function



    Public Function GET_FEEDBACK_GRAPH_DATA(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal loginuserid As String, ByVal usertype As String, ByVal duration As String, ByVal branchid As String) As DataTable

        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_feedback_chart_data", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        cmd.Parameters.AddWithValue("@Usertype", usertype)
        cmd.Parameters.AddWithValue("@branchid", branchid)
        cmd.Parameters.AddWithValue("@duration", duration)



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function



    Public Function GET_PROG_DATA(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal loginuserid As String, ByVal usertype As String, ByVal orderby As String, ByVal procedurefor As String, ByVal duration As String) As DataSet

        Dim dsTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_lms_get_training_calendar", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        cmd.Parameters.AddWithValue("@Usertype", usertype)
        cmd.Parameters.AddWithValue("@orderby", orderby)
        cmd.Parameters.AddWithValue("@Procedurefor", procedurefor)
        cmd.Parameters.AddWithValue("@duration", duration)



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsTrainingCalender)
        con.Close()
        Return dsTrainingCalender
    End Function

    Public Function InsUpdTainingPlan_NEW(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingPlanId As String, ByVal RevisedPlanOf As String, ByVal TrainingPlanno As String, ByVal LedgerId As String,
                                          ByVal TrainingName As String, ByVal TrainingDetails As String, ByVal StartDate As String, ByVal CompletionPeriod As String, ByVal CompletionType As String,
                                          ByVal SponsoredAmt As String, ByVal CourseId As String, ByVal ParticipantCategoryID As String, ByVal PhyUnit As String, ByVal CourseDirector As String,
                                          ByVal AssociateDirector As String, ByVal HallType As String,
                                          ByVal HallNo As String, ByVal Rent As String,
                                          ByVal sponsortype As String, ByVal CreatedBy As String, ByVal BranchId As String, ByVal TrainingExpendData As String, ByVal TrainingCategoryid As String, ByVal TrainingCourseCode As String,
                                          ByVal ClosingDate As String, ByVal CategoryDetailID As String, ByVal DeptXML As String,
                                          ByVal Training_SponsorType As String, ByVal ProposalID As String,
                                          ByVal fixvariableexpenditure As String, ByVal createdbyempid As String,
                                          ByVal forwardedempid As String, ByVal tat_typeid As String, ByVal residenttype As String, ByVal docstatus As String, ByVal procedurefor As String, ByVal benificiary As String,
 ByVal prerequisite As String, ByVal learnersaccompolish As String, ByVal Trgnature As String, ByVal imgpath As String, ByVal validtill As String, ByVal participantleveltxt As String, ByVal participation_type As String, ByVal participattype As String, ByVal feedbacktype As String, ByVal checklisttype As String, ByVal sessionmappingrequired As String, ByVal proposaldetailid As String, Optional trg_setting As String = Nothing) As DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim dttrainingdetail As New DataTable
        cmd = New SqlCommand("TrainingPlan.proc_tp_ins_upd_training", con)
        cmd.Parameters.AddWithValue("@TrainingPlanId ", TrainingPlanId)
        If TrainingPlanno Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingPlanno ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingPlanno ", TrainingPlanno)
        End If
        'If ProposalID Is Nothing Then
        '    If System.Web.HttpContext.Current.Session("proposalid_on_TP") Is Nothing Then
        '        cmd.Parameters.AddWithValue("@TrainingProposalid ", DBNull.Value)
        '    Else
        '        cmd.Parameters.AddWithValue("@TrainingProposalid ", System.Web.HttpContext.Current.Session("proposalid_on_TP"))
        '    End If

        'Else
        '    cmd.Parameters.AddWithValue("@TrainingProposalid ", ProposalID)
        'End If
        cmd.Parameters.AddWithValue("@TrainingProposalid ", ProposalID)
        If TrainingCourseCode Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingCode ", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingCode ", TrainingCourseCode)
        End If


        cmd.Parameters.AddWithValue("@TrainingName", TrainingName)
        cmd.Parameters.AddWithValue("@TrainingDetails", TrainingDetails)
        cmd.Parameters.AddWithValue("@StartDate", StartDate)
        cmd.Parameters.AddWithValue("@ClosingDate", ClosingDate)
        cmd.Parameters.AddWithValue("@CompletionPeriod", CompletionPeriod)
        cmd.Parameters.AddWithValue("@CompletionType", CompletionType)
        cmd.Parameters.AddWithValue("@tttf_sponsor_type", Training_SponsorType)

        If CategoryDetailID Is Nothing OrElse CategoryDetailID.ToString = "" Then
            cmd.Parameters.Add(New SqlParameter("@tttf_id", DBNull.Value))
        Else
            cmd.Parameters.Add(New SqlParameter("@tttf_id", CategoryDetailID))
        End If


        cmd.Parameters.Add(New SqlParameter("@DataColXML", TrainingExpendData))

        cmd.Parameters.AddWithValue("@PhyUnit ", PhyUnit)


        If DeptXML Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingDepartmentXml", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingDepartmentXml", DeptXML)
        End If

        cmd.Parameters.AddWithValue("@SponsoredAmt ", SponsoredAmt)
        cmd.Parameters.AddWithValue("@CourseId ", CourseId)
        cmd.Parameters.AddWithValue("@ParticipantCategoryId", ParticipantCategoryID)
        cmd.Parameters.AddWithValue("@ParticipantLevel", participantleveltxt)


        cmd.Parameters.AddWithValue("@CourseDirector", CourseDirector)
        cmd.Parameters.AddWithValue("@AssociateDirector", AssociateDirector)

        cmd.Parameters.AddWithValue("@ResidentType ", residenttype)

        If TrainingCategoryid Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingCategoryid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingCategoryid", TrainingCategoryid)
        End If

        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@BranchId", BranchId)
        cmd.Parameters.AddWithValue("@exptype", fixvariableexpenditure)

        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", forwardedempid)
        cmd.Parameters.AddWithValue("@tat_type_id", tat_typeid)
        cmd.Parameters.AddWithValue("@doc_status", docstatus)
        cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        If benificiary <> "" Then
            cmd.Parameters.AddWithValue("@benefitted", benificiary)
        Else
            cmd.Parameters.AddWithValue("@benefitted", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@prerequiste", prerequisite)
        If learnersaccompolish <> "" Then
            cmd.Parameters.AddWithValue("@objective", learnersaccompolish)
        Else
            cmd.Parameters.AddWithValue("@objective", DBNull.Value)
        End If
        If imgpath <> "" Then
            cmd.Parameters.AddWithValue("@img_path", imgpath)
        Else
            cmd.Parameters.AddWithValue("@img_path", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@trg_type", Trgnature)

        If validtill <> "" Then
            cmd.Parameters.AddWithValue("@trg_validity", validtill)
        Else
            cmd.Parameters.AddWithValue("@trg_validity", DBNull.Value)
        End If
        cmd.Parameters.AddWithValue("@participation_type", participation_type)
        cmd.Parameters.AddWithValue("@participant_type", participattype)

        If feedbacktype <> "" Then
            cmd.Parameters.AddWithValue("@FeedbackType", feedbacktype)
        Else
            cmd.Parameters.AddWithValue("@FeedbackType", DBNull.Value)
        End If
        If checklisttype <> "" Then
            cmd.Parameters.AddWithValue("@ChcekListType", checklisttype)
        Else
            cmd.Parameters.AddWithValue("@ChcekListType", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@participant_seession_required", sessionmappingrequired)

        If trg_setting <> Nothing Then
            cmd.Parameters.AddWithValue("@trg_setting", trg_setting)
        End If

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dttrainingdetail)
        con.Close()

        'Comment on 08-March-24 due to discussion because of alrady called in training save procedure.
        'If Not proposaldetailid Is Nothing Then
        '    If proposaldetailid <> "" Then
        '        Update_Proposal_on_trg(Domain, IsOnline, ProposalID, proposaldetailid, TrainingPlanId, dttrainingdetail.Rows(0)("TrainingPlanno").ToString, createdbyempid)
        '    End If

        'End If



        Return dttrainingdetail
    End Function



    Public Function GetTrainingAmount_New(ByVal Domain As String, ByVal isonline As String, ByVal feesid As String, ByVal trgtype As String, ByVal sponsortype As String, ByVal startdate As String, ByVal durationtype As String, ByVal duration As String, ByVal residenttype As String, ByVal ProposedCandidate As String, ByVal TrainingId As String, ByVal EffectiveDate As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingAmount As New DataTable
        cmd = New SqlCommand("Select * from TrainingPlan.ft_tp_lms_get_trg_amt (@TrainingfeesId,@tttf_trg_type,@tttf_sponsor_type,@ResidentType,@ProposedCandidate,@TrainingId,@trgstartdt,@DurationType,@Duration) ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        If Not feesid Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingfeesId", feesid)
        Else
            cmd.Parameters.AddWithValue("@TrainingfeesId", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@tttf_trg_type", trgtype)
        cmd.Parameters.AddWithValue("@tttf_sponsor_type", sponsortype)
        cmd.Parameters.AddWithValue("@ResidentType", residenttype)

        cmd.Parameters.AddWithValue("@ProposedCandidate", ProposedCandidate)

        cmd.Parameters.AddWithValue("@trgstartdt", startdate)
        cmd.Parameters.AddWithValue("@DurationType", durationtype)
        cmd.Parameters.AddWithValue("@Duration", duration)

        If TrainingId Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        End If
        cmd.Parameters.AddWithValue("@EffectiveDate", EffectiveDate)


        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtTrainingAmount)
        con.Close()
        Return dtTrainingAmount
    End Function



    Public Function Get_Trg_Data_New(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_training_details]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingid", trainingid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function



    Public Function Get_TRG_Session_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal timetableid As String, ByVal branchid As String, ByVal fromdt As String, ByVal todt As String, ByVal trgcode As String, ByVal empid As String, ByVal dmsconfi As String, ByVal day As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_trg_time_table]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingid Is Nothing Or trainingid = "" Then
            cmd.Parameters.AddWithValue("@TrainingPlanId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingPlanId", trainingid)
        End If

        If timetableid Is Nothing Or timetableid = "" Then
            cmd.Parameters.AddWithValue("@timetableid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@timetableid", timetableid)
        End If

        If branchid Is Nothing Or branchid = "" Then
            cmd.Parameters.AddWithValue("@BranchID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@BranchID", branchid)
        End If

        If fromdt Is Nothing Or fromdt = "" Then
            cmd.Parameters.AddWithValue("@FromDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@FromDate", fromdt)
        End If

        If todt Is Nothing Or todt = "" Then
            cmd.Parameters.AddWithValue("@ToDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ToDate", todt)
        End If

        If trgcode Is Nothing Or trgcode = "" Then
            cmd.Parameters.AddWithValue("@Trainingcode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Trainingcode", trgcode)
        End If
        cmd.Parameters.AddWithValue("@Isforwarded", "0")
        If empid Is Nothing Or empid = "" Then
            cmd.Parameters.AddWithValue("@employeeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@employeeid", empid)
        End If
        If dmsconfi Is Nothing Or dmsconfi = "" Then
            cmd.Parameters.AddWithValue("@DMSConf", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@DMSConf", dmsconfi)
        End If

        If day Is Nothing Or day = "" Then
            cmd.Parameters.AddWithValue("@day", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@day", day)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function



    Public Function Get_TRG_IMPORT_Session_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal timetableid As String, ByVal branchid As String, ByVal fromdt As String, ByVal todt As String, ByVal trgcode As String, ByVal empid As String, ByVal dmsconfi As String, ByVal trgimportcode As String, ByVal createdby As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_import_trg_time_table]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingid Is Nothing Or trainingid = "" Then
            cmd.Parameters.AddWithValue("@TrainingPlanId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@TrainingPlanId", trainingid)
        End If

        If timetableid Is Nothing Or timetableid = "" Then
            cmd.Parameters.AddWithValue("@timetableid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@timetableid", timetableid)
        End If

        If branchid Is Nothing Or branchid = "" Then
            cmd.Parameters.AddWithValue("@BranchID", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@BranchID", branchid)
        End If

        If fromdt Is Nothing Or fromdt = "" Then
            cmd.Parameters.AddWithValue("@FromDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@FromDate", fromdt)
        End If

        If todt Is Nothing Or todt = "" Then
            cmd.Parameters.AddWithValue("@ToDate", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ToDate", todt)
        End If

        If trgcode Is Nothing Or trgcode = "" Then
            cmd.Parameters.AddWithValue("@Trainingcode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Trainingcode", trgcode)
        End If

        If empid Is Nothing Or empid = "" Then
            cmd.Parameters.AddWithValue("@employeeid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@employeeid", empid)
        End If
        If dmsconfi Is Nothing Or dmsconfi = "" Then
            cmd.Parameters.AddWithValue("@DMSConf", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@DMSConf", dmsconfi)
        End If

        If trgimportcode Is Nothing Or trgimportcode = "" Then
            cmd.Parameters.AddWithValue("@Trainingimportcode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Trainingimportcode", trgimportcode)
        End If
        cmd.Parameters.AddWithValue("@createdby", createdby)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function



    Public Function GetTraningCodeBy_Time_table(ByVal Domain As String, ByVal isonline As String, ByVal timetableid As String) As DataTable
        Dim dtcode As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        ' cmd = New SqlCommand("select distinct trainingcode from trainingplan.Vw_tp_trg_time_table where ttttt_timetableid='" + timetableid + "'", con)
        cmd = New SqlCommand("select distinct ttttt_trainingid,bd.TrainingCode from TrainingPlan.tbl_tp_trg_time_table  ttt
inner join trainingplan.trainingbasicdetails bd on bd.trainingid=ttt.ttttt_trainingid
 where ttttt_timetableid='" + timetableid + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcode)
        con.Close()
        Return dtcode
    End Function



    Public Function GetTraningCodeBy_TRG(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String) As DataTable
        Dim dtcode As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        ' cmd = New SqlCommand("select distinct trainingcode from trainingplan.Vw_tp_trg_time_table where ttttt_timetableid='" + timetableid + "'", con)
        cmd = New SqlCommand("select distinct trainingid,trainingcode from [TrainingPlan].[Vw_tp_trg_time_table]  where trainingid='" + trainingid + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcode)
        con.Close()
        Return dtcode
    End Function

    Public Function Get_Fees_Details(ByVal Domain As String, ByVal IsOnline As String, ByVal feesid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_get_training_fees_data]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@tttf_id", feesid)
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function chk_reg_mailid_New(ByVal Domain As String, ByVal isonline As String, ByVal mailid As String) As DataTable
        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim da As New SqlDataAdapter("select  * from  [TrainingPlan].[ft_tp_get_participant_by_regemail] ('" + mailid + "')", con)
        da.SelectCommand.CommandTimeout = 8000

        da.SelectCommand.CommandType = CommandType.Text
        da.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function InsUpd_Faculty_New(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyName As String, ByVal HAgencyName As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal RoleID As String, ByVal ColumnValXML As String, ByVal UserCode As String, ByVal isdisabled As String, ByVal coursedetail As String, ByVal uploadpath As String, ByVal agencystatus As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_insupd_guest_faculty", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@facultyid", AgencyId)
        cmd.Parameters.AddWithValue("@facultyname", AgencyName)
        If HAgencyName.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@hfacultyname", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hfacultyname", HAgencyName)
        End If


        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        cmd.Parameters.AddWithValue("@CourseDetails", coursedetail)
        cmd.Parameters.AddWithValue("@isdisable", isdisabled)
        cmd.Parameters.AddWithValue("@uploadpath", uploadpath)
        cmd.Parameters.AddWithValue("@agencystatus", agencystatus)

        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@branchid", branchid)
        If createdbyempid <> "" Then
            cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        Else
            cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
            cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
        End If




        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function Get_TRG_DETAIL(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_training_details]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", trainingid)
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function


    Public Function GetTraining_Session_Details(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal branchid As String, ByVal timetableid As String, ByVal fromdate As String, ByVal todate As String, ByVal trainingcode As String, ByVal employeeid As String) As DataSet


        Dim ds As New Data.DataSet
        Dim cnn As SqlConnection = Get_Connection_String(Domain, isOnline)
        cnn.Open()
        Dim da As New SqlDataAdapter("[TrainingPlan].[proc_tp_get_trg_time_table]", cnn)
        da.SelectCommand.CommandType = Data.CommandType.StoredProcedure
        da.SelectCommand.CommandTimeout = 8000
        If Not trainingid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", trainingid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@TrainingPlanId", DBNull.Value)
        End If
        If Not timetableid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@timetableid", timetableid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@timetableid", DBNull.Value)
        End If
        da.SelectCommand.Parameters.AddWithValue("@BranchID", branchid)
        If Not fromdate Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@FromDate", fromdate)
        Else
            da.SelectCommand.Parameters.AddWithValue("@FromDate", DBNull.Value)
        End If
        If Not todate Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@ToDate", todate)
        Else
            da.SelectCommand.Parameters.AddWithValue("@ToDate", DBNull.Value)
        End If
        If Not trainingcode Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@Trainingcode", trainingcode)
        Else
            da.SelectCommand.Parameters.AddWithValue("@Trainingcode", DBNull.Value)
        End If
        If Not employeeid Is Nothing Then
            da.SelectCommand.Parameters.AddWithValue("@employeeid", employeeid)
        Else
            da.SelectCommand.Parameters.AddWithValue("@employeeid", DBNull.Value)
        End If
        da.Fill(ds)
        cnn.Close()
        Return ds



    End Function



    Public Function Save_Clone_Session_Details(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal timetableid As String, ByVal createdby As String, ByVal branchid As String, ByVal sessionid As String, ByVal facultyid As String, ByVal contentdesc As String, ByVal subject As String, ByVal type As String, ByVal sessiondate As String, ByVal sessionday As String, ByVal sessionweek As String, ByVal sessionduration As String, ByVal rowno As String, ByVal isjointsession As String, ByVal sessionno As String, ByVal sessiontime As String, ByVal endtime As String, ByVal status As String, ByVal remark As String, ByVal tag As String, ByVal durationtype As String, ByVal isComplementory As String) As Boolean
        'already save = 1 and new=0
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_ins_upd_session]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@Trainingid", trainingid)
        cmd.Parameters.AddWithValue("@Timetableid", timetableid)
        cmd.Parameters.AddWithValue("@createdby", createdby)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))

        cmd.Parameters.AddWithValue("@branchid", branchid)
        cmd.Parameters.AddWithValue("@ttttt_session_id", sessionid)
        If facultyid = "" Then
            cmd.Parameters.AddWithValue("@ttttt_facultyid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ttttt_facultyid", facultyid)
        End If

        cmd.Parameters.AddWithValue("@ttttt_content_desc", contentdesc)

        cmd.Parameters.AddWithValue("@ttttt_subject", subject)
        cmd.Parameters.AddWithValue("@ttttt_type", type)
        cmd.Parameters.AddWithValue("@ttttt_session_dt", sessiondate)
        cmd.Parameters.AddWithValue("@ttttt_session_day", sessionday)

        cmd.Parameters.AddWithValue("@ttttt_session_week", sessionweek)
        cmd.Parameters.AddWithValue("@ttttt_session_duration", sessionduration)
        cmd.Parameters.AddWithValue("@ttttt_session_row_no", rowno)
        cmd.Parameters.AddWithValue("@ttttt_is_joint_session", isjointsession)

        cmd.Parameters.AddWithValue("@ttttt_session_no", sessionno)
        cmd.Parameters.AddWithValue("@ttttt_session_time", sessiontime)

        cmd.Parameters.AddWithValue("@ttttt_session_end_time", endtime)

        cmd.Parameters.AddWithValue("@ttttt_status", status)
        cmd.Parameters.AddWithValue("@ttttt_remark", remark)

        cmd.Parameters.AddWithValue("@TTTTT_SESSION_DURATION_TYPE", durationtype)
        cmd.Parameters.AddWithValue("@tag", tag)
        cmd.Parameters.AddWithValue("@iscomplimentory", isComplementory)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function Get_Session_DETAIL(ByVal Domain As String, ByVal IsOnline As String, ByVal sessionid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_upload_session_attachement]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@sessionid", sessionid)
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function Save_Session_Attachment(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal categoryid As String, ByVal sessionid As String, ByVal branchid As String, ByVal title As String, ByVal completiontype As String, ByVal attachmenttype As String, ByVal attachmentid As String, ByVal attachmentpath As String, ByVal attachmenttext As String, ByVal createdby As String, ByVal parentid As String, ByVal ischild As String, ByVal tag As String, ByVal permission As String, ByVal usertype As String, ByVal createdbyempid As String, ByVal fwdedempid As String, ByVal globalfilename As String, ByVal globalcontentfolderid As String, ByVal status As String, ByVal globalcontentid As String) As Boolean
        'already save = 1 and new=0
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_upload_session_attachement]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trgid", trainingid)

        cmd.Parameters.AddWithValue("@sessionid", sessionid)
        cmd.Parameters.AddWithValue("@title", title)

        cmd.Parameters.AddWithValue("@GlobalContentyTypeID", attachmenttype)
        cmd.Parameters.AddWithValue("@attachementid", attachmentid)
        If Not attachmentpath Is Nothing Then
            If attachmentpath = "" Then
                cmd.Parameters.AddWithValue("@GlobalFilePath", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@GlobalFilePath", attachmentpath)
            End If

        Else
            cmd.Parameters.AddWithValue("@GlobalFilePath", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@GlobalWysiwagText", attachmenttext)
        cmd.Parameters.AddWithValue("@createdby", createdby)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))


        cmd.Parameters.AddWithValue("@ttsad_tag", tag)
        cmd.Parameters.AddWithValue("@permission", permission)
        cmd.Parameters.AddWithValue("@usertype", usertype)

        cmd.Parameters.AddWithValue("@tat_type_id", "119")
        cmd.Parameters.AddWithValue("@fwd_empid", fwdedempid)
        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@branchid", branchid)
        cmd.Parameters.AddWithValue("@GlobalFileName", globalfilename)
        cmd.Parameters.AddWithValue("@GlobalContentFolderID", globalcontentfolderid)
        cmd.Parameters.AddWithValue("@status", status)
        cmd.Parameters.AddWithValue("@globalcontentid", globalcontentid)
        cmd.Parameters.AddWithValue("@procfor", DBNull.Value)


        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function Get_Higher_Authority_To_Set_Data(ByVal dtusers As DataTable) As String
        Dim usertype As String = ""
        Dim dtadmin As DataTable
        dtusers.DefaultView.RowFilter = "usertype='1'"
        dtadmin = dtusers.DefaultView.ToTable
        Dim dtCD As DataTable
        dtusers.DefaultView.RowFilter = "usertype='3'"
        dtCD = dtusers.DefaultView.ToTable
        Dim dtFaculty As DataTable
        dtusers.DefaultView.RowFilter = "usertype='4'"
        dtFaculty = dtusers.DefaultView.ToTable
        Dim dtParticipant As DataTable
        dtusers.DefaultView.RowFilter = "usertype='5'"
        dtParticipant = dtusers.DefaultView.ToTable
        Dim dtDept As DataTable
        dtusers.DefaultView.RowFilter = "usertype='2'"
        dtDept = dtusers.DefaultView.ToTable

        If dtadmin.Rows.Count > 0 Then   'If Found Admin then set Admin type
            usertype = dtadmin.Rows(0)("usertype").ToString
        ElseIf dtCD.Rows.Count > 0 Then  'If Admin not found and CD found then set Cd type
            usertype = dtCD.Rows(0)("usertype").ToString
        ElseIf dtFaculty.Rows.Count > 0 Then  'If Admin and CD not found But Faculty found then set Faculty type
            usertype = dtFaculty.Rows(0)("usertype").ToString
        ElseIf dtParticipant.Rows.Count > 0 Then 'If Admin,CD,Faculty not found But Participant found then set Participant type
            usertype = dtParticipant.Rows(0)("usertype").ToString
        ElseIf dtDept.Rows.Count > 0 Then  'If Admin,CD,Faculty and participant not found But Dept found then set Dept type
            usertype = dtDept.Rows(0)("usertype").ToString
        Else  'If Admin,CD,Faculty,participant and Dept not found then set First Return User Type
            usertype = dtusers.Rows(0)("usertype").ToString
        End If

        Return usertype
    End Function


    Public Function GET_TRG_DETAILS_BY_TRG(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal trainingid As String) As DataSet

        Dim dsTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_lms_get_training_calendar", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not fromdate Is Nothing Then
            cmd.Parameters.AddWithValue("@fromdate", fromdate)
        Else
            cmd.Parameters.AddWithValue("@fromdate", DBNull.Value)
        End If
        If Not todate Is Nothing Then
            cmd.Parameters.AddWithValue("@todate", todate)
        Else
            cmd.Parameters.AddWithValue("@todate", DBNull.Value)
        End If
        cmd.Parameters.AddWithValue("@trainingId", trainingid)




        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsTrainingCalender)
        con.Close()
        Return dsTrainingCalender
    End Function

    Public Function Get_Tax_Rates(ByVal Domain As String, ByVal isOnline As String) As DataTable
        Dim dtexpdetails As New DataTable
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].GetExpenses", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        Dim daTrainingDetails As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingDetails.Fill(dtexpdetails)
        con.Close()
        Return dtexpdetails
    End Function




    Public Function Get_Trg_Bill_Details(ByVal Domain As String, ByVal IsOnline As String, ByVal Billid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_edit_bill]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@SponsorBillid", Billid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function


    Public Function Get_Agency_Details(ByVal Domain As String, ByVal IsOnline As String, ByVal agencyid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[GetAgencyDetail]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyID", agencyid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function


    Public Function GetSponsorData(ByVal Domain As String, ByVal IsOnline As String, ByVal agencyid As String) As DataSet
        Dim dsAgencies As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("YUser.proc_yuser_get_sponsor_data", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        If Not agencyid Is Nothing Then
            cmd.Parameters.AddWithValue("@AgencyId", agencyid)
            'Else
            '    cmd.Parameters.AddWithValue("@AgencyId", DBNull.Value)
        End If
        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dsAgencies)
        con.Close()
        Return dsAgencies
    End Function


    Public Function InsUpdAgency_Representative(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyName As String, ByVal HAgencyName As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal RoleID As String, ByVal ColumnValXML As String, ByVal UserCode As String, ByVal isdisabled As String, ByVal upload As String, ByVal agencystatus As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal branchid As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_insupd_sponsor_rep", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@repid", AgencyId)
        cmd.Parameters.AddWithValue("@repname", AgencyName)
        If HAgencyName.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@hrepname", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hrepname", HAgencyName)
        End If

        cmd.Parameters.AddWithValue("@agencystatus", agencystatus)
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        'cmd.Parameters.AddWithValue("@UserCode", UserCode)
        cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        cmd.Parameters.AddWithValue("@isdisable", isdisabled)
        If upload <> "" Then
            cmd.Parameters.AddWithValue("@uploadpath", upload)
        Else
            cmd.Parameters.AddWithValue("@uploadpath", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@branchid", branchid)

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function GET_EMAIL_USER_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal usertypeid As String, ByVal emailid As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_get_user_data_from_email]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@email", emailid)
        cmd.Parameters.AddWithValue("@usertype", usertypeid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function
    Public Function InsUpdAgency_Sponsor(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyName As String, ByVal HAgencyName As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal ColumnValXML As String, ByVal status As String, ByVal isdisabled As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal upload As String, ByVal otheragencytype As String, ByVal participantid As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_insupd_sponsor", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        If Not AgencyId Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorid", AgencyId)
        Else
            cmd.Parameters.AddWithValue("@sponsorid", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@sposnosrname", AgencyName)
        If HAgencyName.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@hsposnosrname", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hsposnosrname", HAgencyName)
        End If


        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

        cmd.Parameters.AddWithValue("@uploadpath", upload)
        cmd.Parameters.AddWithValue("@agencystatus", status)
        cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        cmd.Parameters.AddWithValue("@isdisable", isdisabled)

        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@branchid", branchid)
        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        cmd.Parameters.AddWithValue("@is_other_type", otheragencytype)

        If Not participantid Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorprid", participantid)
        Else
            cmd.Parameters.AddWithValue("@sponsorprid", DBNull.Value)
        End If
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function


    Public Function GetDMS_Last_Status(ByVal Domain As String, ByVal isonline As String, ByVal docid As String) As DataTable
        Dim dtdata As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("select * from DMS.VW_dms_doc_last_status where tdds_doc_id='" + docid + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dtdata)
        con.Close()

        Return dtdata
    End Function

    Public Function GetDMS_Last_Status_By_Type(ByVal Domain As String, ByVal isonline As String, ByVal typeid As String) As DataTable
        Dim dtdata As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("select tdds_tat_type_id,tdds_doc_id,tdds_status from DMS.VW_dms_doc_last_status where tdds_tat_type_id='" + typeid + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dtdata)
        con.Close()

        Return dtdata
    End Function


    Public Function InsUpdAgency_Sponsor_new(ByVal Domain As String, ByVal isOnline As String, ByVal AgencyId As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal docpath As String, ByVal photopath As String, ByVal status As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal pincode As String, ByVal salutation As String, ByVal fname As String, ByVal mname As String, ByVal lname As String, ByVal hfname As String, ByVal hmname As String, ByVal hlname As String, ByVal gender As String, ByVal age As String, ByVal dob As String, ByVal phone As String, ByVal altPhone As String, ByVal mobile As String, ByVal altMobile As String, ByVal email As String, ByVal altEmail As String, ByVal aadhar As String, ByVal panNo As String, ByVal gstinNo As String, ByVal orgXML As String, ByVal partXML As String, ByVal password As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_insupd_sponsor", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        If Not AgencyId Is Nothing Then
            cmd.Parameters.AddWithValue("@sponsorid", AgencyId)
        Else
            cmd.Parameters.AddWithValue("@sponsorid", DBNull.Value)
        End If
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@branchid", branchid)
        If docpath.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@docpath", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@docpath", docpath)
        End If
        If photopath.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@photopath", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@photopath", photopath)
        End If
        cmd.Parameters.AddWithValue("@agencystatus", status)

        'If sponsorstatus.ToString.Trim = "" Then
        '    cmd.Parameters.AddWithValue("@00053status", DBNull.Value)
        'Else
        '    cmd.Parameters.AddWithValue("@00053status", sponsorstatus)
        'End If
        'If participantstatus.ToString.Trim = "" Then
        '    cmd.Parameters.AddWithValue("@00065status", DBNull.Value)
        'Else
        '    cmd.Parameters.AddWithValue("@00065status", participantstatus)
        'End If
        cmd.Parameters.AddWithValue("@Agencytype", AgencyTypeID)



        If address.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Ag_Address", address)
        End If
        If city.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_address_city", city)
        End If
        If state.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_address_state", state)
        End If
        If pincode.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_pincode", pincode)
        End If
        If salutation.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_salutation", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_salutation", salutation)
        End If
        If fname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_first_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_first_name", fname)
        End If
        If mname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_m_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_m_name", mname)
        End If
        If lname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_l_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_l_name", lname)
        End If
        If hfname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hfirst_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hfirst_name", hfname)
        End If
        If hmname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hm_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hm_name", hmname)
        End If
        If hlname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hl_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hl_name", hlname)
        End If
        If gender.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gender", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gender", gender)
        End If
        If age.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_age", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_age", age)
        End If
        If dob.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_dob", dob)
        End If
        If phone.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_phone", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_phone", phone)
        End If
        If altPhone.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_alternative_phone", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_phone", altPhone)
        End If
        If mobile.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_mobileno", mobile)
        End If
        If altMobile.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", altMobile)
        End If
        If email.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_email", email)
        End If
        If altEmail.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_alternative_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_email", altEmail)
        End If
        If aadhar.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_aadhar", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_aadhar", aadhar)
        End If
        If panNo.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_pan", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_pan", panNo)
        End If
        If gstinNo.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gstn", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gstn", gstinNo)
        End If

        If Not orgXML Is Nothing Then
            cmd.Parameters.AddWithValue("@OrgXML", orgXML)
        Else
            cmd.Parameters.AddWithValue("@OrgXML", DBNull.Value)
        End If

        If Not partXML Is Nothing Then
            cmd.Parameters.AddWithValue("@OrgPartXML", partXML)
        Else
            cmd.Parameters.AddWithValue("@OrgPartXML", DBNull.Value)
        End If
        If Not password Is Nothing Then
            cmd.Parameters.AddWithValue("@password", password)
        Else
            cmd.Parameters.AddWithValue("@password", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function GET_Sponsor_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal agencytype As String, ByVal agencyid As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_get_sponsor_data]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", agencytype)
        If Not agencyid Is Nothing Then
            cmd.Parameters.AddWithValue("@agencyid", agencyid)
        Else
            cmd.Parameters.AddWithValue("@agencyid", DBNull.Value)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function




    Public Function CHECK_EMAILDATA(ByVal Domain As String, ByVal IsOnline As String, ByVal agencytype As String, ByVal agencyid As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_get_sponsor_data]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", agencytype)
        If Not agencyid Is Nothing Then
            cmd.Parameters.AddWithValue("@agencyid", agencyid)
        Else
            cmd.Parameters.AddWithValue("@agencyid", DBNull.Value)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function


    Public Function SAVE_PARTICIPANT_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal participantid As String, ByVal trainingid As String, ByVal agencystatus As String, ByVal CreatedBy As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal docpath As String, ByVal photopath As String, ByVal address As String, ByVal pincode As String, ByVal salutation As String, ByVal fname As String, ByVal mname As String, ByVal lname As String, ByVal hfname As String, ByVal hmname As String, ByVal hlname As String, ByVal gender As String, ByVal age As String, ByVal dob As String, ByVal phone As String, ByVal altPhone As String, ByVal mobile As String, ByVal altMobile As String, ByVal email As String, ByVal altEmail As String, ByVal aadhar As String, ByVal panNo As String, ByVal gstinNo As String, ByVal partXML As String, procedurefor As String, ByVal isdelete As String, ByVal password As String, ByVal roleid As String, ByVal agencytype As String, ByVal sponsorid As String, ByVal city As String, ByVal state As String, ByVal username As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_ins_upd_participant", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@participantid", participantid)
        If Not sponsorid Is Nothing Then
            If sponsorid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@sponsorid", sponsorid)
            Else
                cmd.Parameters.AddWithValue("@sponsorid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@sponsorid", DBNull.Value)
        End If
        If Not trainingid Is Nothing Then
            If trainingid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@Trainingid", trainingid)
            Else
                cmd.Parameters.AddWithValue("@Trainingid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@Trainingid", DBNull.Value)
        End If


        cmd.Parameters.AddWithValue("@Createdby", CreatedBy)
        cmd.Parameters.AddWithValue("@Branchid", branchid)
        If Not partXML Is Nothing Then
            cmd.Parameters.AddWithValue("@PartXML", partXML)
        Else
            cmd.Parameters.AddWithValue("@PartXML", DBNull.Value)
        End If
        If Not docpath Is Nothing Then
            If docpath.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@docpath", docpath)
            End If

        End If

        If Not photopath Is Nothing Then
            If photopath.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@photopath", photopath)
            End If
        End If

        If Not agencystatus Is Nothing Then
            If agencystatus.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@agencystatus", agencystatus)
            End If
        End If
        If Not procedurefor Is Nothing Then
            If procedurefor.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
            End If
        End If




        cmd.Parameters.AddWithValue("@isdelete", isdelete)
        If Not password Is Nothing Then
            If password.ToString <> "" Then
                cmd.Parameters.AddWithValue("@password", password)
            End If
        End If

        cmd.Parameters.AddWithValue("@roleid", roleid)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@Agencytype", agencytype)




        If salutation.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_salutation", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_salutation", salutation)
        End If
        If fname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_first_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_first_name", fname)
        End If
        If mname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_m_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_m_name", mname)
        End If
        If lname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_l_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_l_name", lname)
        End If

        If gender.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gender", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gender", gender)
        End If

        If dob.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_dob", dob)
        End If

        If mobile.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_mobileno", mobile)
        End If

        If email.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_email", email)
        End If

        If Not createdbyempid Is Nothing Then
            If createdbyempid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            Else
                cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
        End If

        If Not fwdempid Is Nothing Then
            If fwdempid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
            Else
                cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
        End If
        If Not address Is Nothing Then
            If address <> "" Then
                cmd.Parameters.AddWithValue("@Ag_Address", address)
            Else
                cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
        End If
        If Not pincode Is Nothing Then
            If pincode <> "" Then
                cmd.Parameters.AddWithValue("@ag_pincode", pincode)
            Else
                cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
        End If
        If Not city Is Nothing Then
            If city <> "" Then
                cmd.Parameters.AddWithValue("@ag_address_city", city)
            Else
                cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
        End If
        If Not state Is Nothing Then
            If state <> "" Then
                cmd.Parameters.AddWithValue("@ag_address_state", state)
            Else
                cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
        End If
        If Not altMobile Is Nothing Then
            If altMobile <> "" Then
                cmd.Parameters.AddWithValue("@ag_alternative_mobileno", altMobile)
            Else
                cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
        End If


        If Not username Is Nothing Then
            If username.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@username", username)
            End If
        End If

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function







    Public Function SAVE_FAC_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal facultyid As String, ByVal agencystatus As String, ByVal CreatedBy As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal docpath As String, ByVal photopath As String, ByVal address As String, ByVal pincode As String, ByVal salutation As String, ByVal fname As String, ByVal mname As String, ByVal lname As String, ByVal hfname As String, ByVal hmname As String, ByVal hlname As String, ByVal gender As String, ByVal dob As String, ByVal phone As String, ByVal mobile As String, ByVal altMobile As String, ByVal email As String, ByVal aadhar As String, ByVal roleid As String, ByVal agencytype As String, ByVal city As String, ByVal state As String, ByVal FacXML As String, ByVal CourseXML As String, ByVal password As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_insupd_guest_faculty", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@facultyid", facultyid)
        cmd.Parameters.AddWithValue("@Createdby", CreatedBy)
        If Not docpath Is Nothing Then
            If docpath.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@docpath", docpath)
            End If
        End If

        If Not photopath Is Nothing Then
            If photopath.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@photopath", photopath)
            End If
        End If
        If Not agencystatus Is Nothing Then
            If agencystatus.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@agencystatus", agencystatus)
            End If
        End If

        cmd.Parameters.AddWithValue("@roleid", roleid)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@Agencytype", agencytype)

        cmd.Parameters.AddWithValue("@Branchid", branchid)
        If Not FacXML Is Nothing Then
            cmd.Parameters.AddWithValue("@otherDetails", FacXML)
        Else
            cmd.Parameters.AddWithValue("@otherDetails", DBNull.Value)
        End If
        If Not CourseXML Is Nothing Then
            cmd.Parameters.AddWithValue("@CourseDetails", CourseXML)
        Else
            cmd.Parameters.AddWithValue("@CourseDetails", DBNull.Value)
        End If







        If salutation.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_salutation", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_salutation", salutation)
        End If

        If fname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_first_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_first_name", fname)
        End If
        If mname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_m_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_m_name", mname)
        End If
        If lname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_l_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_l_name", lname)
        End If

        If hfname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hfirst_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hfirst_name", hfname)
        End If
        If hmname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hm_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hm_name", hmname)
        End If
        If hlname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hl_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hl_name", hlname)
        End If

        If gender.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gender", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gender", gender)
        End If
        If Not dob Is Nothing Then
            If dob.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_dob", dob)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
        End If

        If Not mobile Is Nothing Then
            If mobile.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_mobileno", mobile)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
        End If

        If Not altMobile Is Nothing Then
            If altMobile.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_alternative_mobileno", altMobile)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
        End If

        If Not aadhar Is Nothing Then
            If aadhar.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_aadhar", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_aadhar", aadhar)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_aadhar", DBNull.Value)
        End If

        If Not phone Is Nothing Then
            If phone.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_phone", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_phone", phone)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_phone", DBNull.Value)
        End If



        If email.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_email", email)
        End If

        If Not address Is Nothing Then
            If address.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@Ag_Address", address)
            End If
        Else
            cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
        End If
        If Not city Is Nothing Then
            If city.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_address_city", city)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
        End If

        If Not city Is Nothing Then
            If state.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_address_state", state)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
        End If
        If Not city Is Nothing Then
            If pincode.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_pincode", pincode)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
        End If


        If Not createdbyempid Is Nothing Then
            If createdbyempid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            Else
                cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
        End If

        If Not fwdempid Is Nothing Then
            If fwdempid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
            Else
                cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
        End If

        If Not password Is Nothing Then
            cmd.Parameters.AddWithValue("@password", password)
        Else
            cmd.Parameters.AddWithValue("@password", DBNull.Value)
        End If


        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function


    Public Function GET_EMAIL_AGENCY_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal type As String, ByVal chkval As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_check_value_in_agency_master]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@value", chkval)
        cmd.Parameters.AddWithValue("@type", type)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function


    'Public Function GetInstructorData(ByVal Domain As String, ByVal IsOnline As String, ByVal agencytype As String, ByVal instructorid As String) As DataSet
    '    Dim dsGuestFaculties As New DataSet
    '    Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
    '    con.Open()
    '    cmd = New SqlCommand("TrainingPlan.TP_GetGuestFaculties", con)
    '    cmd.CommandType = CommandType.StoredProcedure
    '    cmd.Connection = con
    '    cmd.CommandTimeout = 5000

    '    If instructorid Is Nothing Then
    '        cmd.Parameters.AddWithValue("@FacultyID", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@FacultyID", instructorid)
    '    End If
    '    If agencytype Is Nothing Then
    '        cmd.Parameters.AddWithValue("@Agencytype", DBNull.Value)
    '    Else
    '        cmd.Parameters.AddWithValue("@Agencytype", agencytype)
    '    End If
    '    Dim daGuestFaculties As SqlDataAdapter = New SqlDataAdapter(cmd)
    '    daGuestFaculties.Fill(dsGuestFaculties)
    '    con.Close()
    '    Return dsGuestFaculties
    'End Function


    Public Function GET_AGENCY_DATA_NEW(ByVal Domain As String, ByVal IsOnline As String, ByVal agencytypeid As String, ByVal agencyid As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_get_agency]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", agencytypeid)
        If Not agencyid Is Nothing Then
            cmd.Parameters.AddWithValue("@AgencyId", agencyid)
        Else
            cmd.Parameters.AddWithValue("@AgencyId", DBNull.Value)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)

        '*************Extra Code to increase tyaam_val in first table to Manage old procedure data

        dsuserdetails.Tables(0).Columns.Add("tyaam_val")

        For Each dr In dsuserdetails.Tables(0).Rows
            Dim dtfilterdata As DataTable
            dtfilterdata = dsuserdetails.Tables(1)
            dtfilterdata.DefaultView.RowFilter = "tyaam_typeid='" + dr("tyaam_typeid").ToString + "'"
            dtfilterdata = dtfilterdata.DefaultView.ToTable()
            If dtfilterdata.Rows.Count > 0 Then
                dr("tyaam_val") = dtfilterdata.Rows(0)("tyaam_val")
            End If
        Next




        '*****************

        con.Close()
        Return dsuserdetails
    End Function


    Public Function GENERATE_NEW_DMS_DOC_NO(ByVal Domain As String, ByVal IsOnline As String, ByVal docdate As String, ByVal BranchId As String, ByVal tat_type_id As String, ByVal repeattype As String, ByVal docprefix As String) As String
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim docno As String
        cmd = New SqlCommand("select [DMS].[f_dms_doc_ref_no]('" + docdate + "','" + BranchId + "','" + tat_type_id + "','" + docprefix + "','" + repeattype + "') ", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        docno = cmd.ExecuteScalar()
        con.Close()
        Return docno
    End Function


    Public Function Get_Trg_All_User_Details(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String) As DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        Dim dtTrainingAmount As New DataTable
        If Not trainingid Is Nothing Then
            cmd = New SqlCommand("select * from trainingplan.Vw_tp_trg_all_user where trainingid='" + trainingid + "'", con)
        Else
            cmd = New SqlCommand("select * from trainingplan.Vw_tp_trg_all_user", con)
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtTrainingAmount)
        con.Close()
        Return dtTrainingAmount
    End Function


    Public Function GET_SESSION_MEETING_DETAILS(ByVal Domain As String, ByVal IsOnline As String, ByVal sessionid As String) As DataTable
        Dim dtmeetingdetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[trainingplan].[proc_tp_lms_get_meeting]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not sessionid Is Nothing Then
            cmd.Parameters.AddWithValue("@session_id", sessionid)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtmeetingdetails)
        con.Close()
        Return dtmeetingdetails
    End Function

    Public Function Get_Agency_Data_For_Login(ByVal Domain As String, ByVal isonline As String, ByVal AgencyTypeId As String, ByVal Agencyid As String) As DataTable
        Dim dtAgencies As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("YUser.proc_yuser_get_agency", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", AgencyTypeId)
        cmd.Parameters.AddWithValue("@AgencyId", Agencyid)
        Dim daAgencyTypes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daAgencyTypes.Fill(dtAgencies)
        con.Close()
        Return dtAgencies
    End Function

    '**********************Mail/SMS Code
    Public Function Send_OTP(ByVal mobNo As String, ByVal emailid As String) As String
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Dim str As String = ""
        Dim OTPID As String = ""
        Dim Msg As String = ""
        ' declare array string to generate random string with combination of small,capital letters and numbers
        Dim charArr As Char() = "0123456789".ToCharArray()
        Dim strrandom As String = String.Empty
        Dim objran As New Random()
        Dim noofcharacters As Integer = 6
        For i As Integer = 0 To noofcharacters - 1
            'It will not allow Repetation of Characters
            Dim pos As Integer = objran.[Next](1, charArr.Length)
            If Not strrandom.Contains(charArr.GetValue(pos).ToString()) Then
                strrandom += charArr.GetValue(pos)
            Else
                i -= 1
            End If
        Next
        str = strrandom
        OTPID = Guid.NewGuid.ToString.Substring(0, 4).ToUpper

        Dim smstext As String = ""
        Dim sptmsgdata = Get_SMS_TEXT("1").Split("*")
        Dim templateid As String = sptmsgdata(1)
        smstext = sptmsgdata(0)
        smstext = smstext.Replace("(#otpid#)", OTPID)
        smstext = smstext.Replace("(#otp#)", str)
        'Msg = (Convert.ToString("Your OTP for id " + OTPID + " is ") & str) + " Please don't share with anyone."

        'Commented due to SMS mail not required in case of ANON
        Try

            sendOTPSms(mobNo, smstext, templateid, 1)

        Catch ex As Exception

        End Try



        If (Not emailid Is Nothing) And emailid <> "" Then

            Dim ClientUrl As String = ""
            Dim ClientName As String = ""
            Dim login As String = ""
            Dim Password As String = ""
            Dim portno As String = ""
            Dim host As String = ""
            Dim header As String = ""
            Dim Footer As String = ""




            'Dim smtpServer As New SmtpClient()

            'Dim es As New EmailConfiguration
            'es = Get_Mail_Setting()
            'ClientUrl = es.clienturl
            'ClientName = es.clientname
            'login = es.login
            'Password = es.password
            'portno = es.portno
            'host = es.host
            'header = es.header



            Dim otpsetting As ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING
            Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
            Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
            Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=6")
            Dim response = request.GetResponse()
            Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
            Dim j As JObject = JObject.Parse(responseString)
            otpsetting = j.ToObject(Of ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING)()


            If (otpsetting.OTP_ON_MAIL = "1") Then
                Send_Mail("1", emailid, "OTP Details from " + ClientName + "", smstext.Replace(". Pratham Softcon", "").Replace("PSCON", ""), 1)
            End If



        End If

        '    Else
        '        Send_Mail("1", "mpacademy@nic.in", "OTP Details", Msg)
        '    End If
        'If ConfigurationManager.AppSettings("IS_OTP_MAIL").ToString = "1" Then
        '    If (Not emailid Is Nothing) Then
        '        Send_Mail("1", emailid, "OTP Details", Msg)

        '    Else
        '        Send_Mail("1", "mpacademy@nic.in", "OTP Details", Msg)
        '    End If
        'End If


        System.Web.HttpContext.Current.Session("OTPSentTime") = System.DateTime.Now
        System.Web.HttpContext.Current.Session("OTP") = str
        Return OTPID
    End Function
    Public Function sendSingleSMS(ByVal mobileNo As String, ByVal message As String, ByVal templateid As String, Optional isOTP As Int16 = 0) As String
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try

            'This Function rewrite with commenting old code to setting with xml now its decided by application setting

            'Dim url As String = ""
            'Dim key As String = ""
            'Dim routeid As String = ""
            'Dim PEID As String = ""
            'Dim senderid As String = ""
            'Dim userid As String = ""
            'Dim password As String = ""

            'Dim es As New SMSSetting
            'es = Get_SMS_Setting()

            'url = es.url
            'key = es.key
            'routeid = es.routeid
            'PEID = es.PEID
            'senderid = es.SENDER_ID
            'userid = es.userid
            'password = es.password


            'Dim isSMSReq = "0"

            'Dim ess As New SMSSetting
            'ess = Get_SMS_Setting()

            'If es.key <> "" Then
            '    isSMSReq = "1"
            'End If



            'If isSMSReq = "1" Then
            '    ' Dim finalurl = "" + url + "?key=" + key + "&routeid=" + routeid + "&type=text&contacts=" + mobileNo.Replace("-", "") + "&senderid=" + senderid + "&msg=" + message + "&tlv={""PE_ID"":""" + PEID + """,""Template_ID"":""" + templateid + """}"

            '    Dim finalurl = "" + url + "?user=" + userid + "&password=" + password + "&senderid=" + senderid + "&channel=Trans&DCS=8&flashsms=0&number=" + mobileNo.Replace("-", "") + "&text=" + message + "&DLTTemplateId=" + templateid + "&route=5&PEId=" + PEID + ""

            '    Dim request As WebRequest = WebRequest.Create(finalurl)
            '    ' request.UseDefaultCredentials

            '    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            '    Dim response As WebResponse = request.GetResponse()
            '    Throw New Exception(finalurl)
            'End If

            Dim isSMSReq = "0"
            Dim smsapistr As String = ""
            If isOTP = 1 Then

                Dim otpsetting As ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING
                Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
                Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
                Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=6")
                Dim response = request.GetResponse()
                Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
                Dim j As JObject = JObject.Parse(responseString)
                otpsetting = j.ToObject(Of ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING)()
                smsapistr = otpsetting.SMSAPI

                If (otpsetting.OTP_ON_SMS = "1") Then
                    isSMSReq = "1"
                Else
                    isSMSReq = "0"
                End If

            Else
                Dim otpsetting As ApplicationSetting.SMS_SEND_BY_APPLICATION
                Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
                Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
                Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=8")
                Dim response = request.GetResponse()
                Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
                Dim j As JObject = JObject.Parse(responseString)
                otpsetting = j.ToObject(Of ApplicationSetting.SMS_SEND_BY_APPLICATION)()
                smsapistr = otpsetting.SMSAPI
                If otpsetting.IS_SMS_SEND = 1 Then
                    isSMSReq = "1"
                Else
                    isSMSReq = "0"
                End If
            End If

            If isSMSReq = "1" Then
                Dim finalurl = smsapistr.Replace("##MOBILE##", mobileNo.Replace("-", "")).Replace("##MSG##", message).Replace("##TEMPLATEID##", templateid)

                Dim request As WebRequest = WebRequest.Create(finalurl)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

                Dim response As WebResponse = request.GetResponse()
                Throw New Exception(finalurl)
            End If


        Catch ex As Exception
            logger.Error("TrainingAPI -sendSingleSMS  " + ex.Message)
        End Try

        Return ""
    End Function
    Function Get_SMS_TEXT(msgtype) As String
        Dim templateid As String = ""
        Dim text As String = ""

        If msgtype = "1" Then

            Dim otpsetting As ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING
            Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
            Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
            Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=6")
            Dim response = request.GetResponse()
            Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
            Dim j As JObject = JObject.Parse(responseString)
            otpsetting = j.ToObject(Of ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING)()
            templateid = otpsetting.smstemplate.DLT_CT_ID
            text = otpsetting.smstemplate.text
        Else

            Dim messagetype As String = "0"

            Dim es As New SMSTemplate
            es = Get_SMS_TEMPLATE(msgtype)
            messagetype = es.ID
            templateid = es.DLT_CT_ID
            text = es.text

        End If


        Return text + "*" + templateid

    End Function
    Public Function sendOTPSms(ByVal mobileNumber As String, ByVal message As String, ByVal Optional templateid As String = "1", Optional isOTP As Int16 = 0) As String
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try


            Dim returndata As String = sendSingleSMS(mobileNumber, message, templateid, isOTP)
            'Return returndata

        Catch ex As SystemException
            logger.Error("DataManager - sendOTPSms" + ex.Message)
            Return ex.ToString
        End Try
    End Function
    Public Function Send_Mail(ByVal isOnline As String, ByVal emailid As String, ByVal Subject As String, ByVal MailText As String, Optional isOTP As Int16 = 0)
        'Commented due to SMS mail not required in case of ANON

        If isOnline = "1" Then

            SendMail_With_XML(emailid, Subject, MailText, "", Nothing, Nothing, Nothing, 1)
            ' General.SendMail_For_RCVP(emailid, Subject, MailText)
        End If
        Return True
    End Function
    Public Shared Sub SendMail_With_Attachment(ByVal UserId As String, ByVal Subject As String, ByVal Body As String, ByVal Attachstring As String, ByVal AttachmentName As String)
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try
            'Commented due to SMS mail not required in case of ANON
            '            Dim msg As New MailMessage()
            '            Dim smtpServer As New SmtpClient()
            '            Dim mailid As String = MailCredential.Mailcredential.Get_Mailid_For_rcvponline()
            '            Dim pwd As String = MailCredential.Mailcredential.Get_Pwd_For_rcvponline()
            '            smtpServer.Credentials = New Net.NetworkCredential(mailid, pwd)

            '            smtpServer.Port = 587  
            '            smtpServer.Host = "smtp.gmail.com"
            '            smtpServer.EnableSsl = True
            '            msg.To.Add(UserId)
            '            'msg.To.Add(txtTo.Tex)
            '            'msg.From = New MailAddress("prathamsoftware@prathamsoft.com", "Pratham", System.Text.Encoding.UTF8)
            '            msg.From = New MailAddress(mailid, "M.P. Prashasan Academy", System.Text.Encoding.UTF8)
            '            msg.Subject = Subject
            '            msg.Body = Body
            '            msg.IsBodyHtml = True
            '            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure

            '#Disable Warning BC40000 ' 'Public Overloads Property ReplyTo As MailAddress' is obsolete: 'ReplyTo is obsoleted for this type.  Please use ReplyToList instead which can accept multiple addresses. http://go.microsoft.com/fwlink/?linkid=14202'.
            '            msg.ReplyTo = New MailAddress(mailid)
            '#Enable Warning BC40000 ' 'Public Overloads Property ReplyTo As MailAddress' is obsolete: 'ReplyTo is obsoleted for this type.  Please use ReplyToList instead which can accept multiple addresses. http://go.microsoft.com/fwlink/?linkid=14202'.

            '            Dim sptAttachment() As String = Attachstring.Split("$")
            '            Dim sptName() As String = AttachmentName.Split("$")
            '            For i = 0 To sptAttachment.Length - 1
            '                If sptAttachment(i).ToString <> "" Then
            '                    msg.Attachments.Add(Attachment.CreateAttachmentFromString(sptAttachment(i), sptName(i)))
            '                    msg.Attachments.Last().ContentDisposition.FileName = sptName(i)
            '                End If
            '            Next


            '            smtpServer.Send(msg)
            '            msg = Nothing
        Catch ex As Exception
            logger.Error("DataManager - SendMail_With_Attachment" + ex.Message)
        End Try
    End Sub
    Public Function SendMail_With_CC(ByVal UserId As String, ByVal Subject As String, ByVal Body As String, ByVal ccid As String)
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try
            'Commented due to SMS mail not required in case of ANON
            '            Dim msg As New MailMessage()
            '            Dim smtpServer As New SmtpClient()
            '            Dim mailid As String = MailCredential.Mailcredential.Get_Mailid_For_Mail_I()
            '            Dim pwd As String = MailCredential.Mailcredential.Get_Pwd_For_Mail_I()




            '            'smtpServer.Credentials = New Net.NetworkCredential("prathamsoftware@prathamsoft.com", "pratham#56")
            '            smtpServer.Credentials = New Net.NetworkCredential(mailid, pwd)

            '            smtpServer.Port = 587
            '            smtpServer.Host = "smtp.gmail.com"
            '            smtpServer.EnableSsl = True
            '            msg.To.Add(UserId)
            '            'msg.To.Add(txtTo.Tex)
            '            'msg.From = New MailAddress("prathamsoftware@prathamsoft.com", "Pratham", System.Text.Encoding.UTF8)
            '            msg.From = New MailAddress(mailid, "Pratham", System.Text.Encoding.UTF8)
            '            msg.Subject = Subject
            '            msg.Body = Body
            '            msg.IsBodyHtml = True
            '            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            '            'msg.ReplyTo = New MailAddress("prathamsoftware@prathamsoft.com")
            '            msg.ReplyTo = New MailAddress(mailid)
            '            smtpServer.Send(msg)
            '            msg = Nothing

            '#Disable Warning BC40000 ' 'Public Overloads Property ReplyTo As MailAddress' is obsolete: 'ReplyTo is obsoleted for this type.  Please use ReplyToList instead which can accept multiple addresses. http://go.microsoft.com/fwlink/?linkid=14202'.
            '            msg.ReplyTo = New MailAddress(mailid)
            '#Enable Warning BC40000 ' 'Public Overloads Property ReplyTo As MailAddress' is obsolete: 'ReplyTo is obsoleted for this type.  Please use ReplyToList instead which can accept multiple addresses. http://go.microsoft.com/fwlink/?linkid=14202'.
            '            smtpServer.Send(msg)
            '            msg = Nothing

        Catch ex As Exception
            logger.Error("DataManager - SendMail_With_CC" + ex.Message)
        End Try
#Disable Warning BC42105 ' Function 'SendMail_With_CC' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.
    End Function



    Public Function CHECK_FEEDBACK_STATUS(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal trgid As String, ByVal sessionid As String, ByVal ipaddress As String) As DataSet
        Dim dsNotesheet As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        If con.State = ConnectionState.Closed Then
            con.Open()
        Else
            con.Close()
            con.Open()
        End If
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.CommandText = "[TrainingPlan].[proc_tp_chk_feedback_status]"
        cmd.Parameters.AddWithValue("@trainingplanid", trgid)
        cmd.Parameters.AddWithValue("@sessionid", sessionid)
        cmd.Parameters.AddWithValue("@ipaddress", ipaddress)

        Dim adp As New SqlDataAdapter(cmd)
        adp.Fill(dsNotesheet)
        con.Close()
        Return dsNotesheet
    End Function
    Public Function InsUpdAgency_Representative_New(ByVal Domain As String, ByVal isOnline As String, ByVal repid As String, ByVal repname As String, ByVal hrepname As String, ByVal uploadpath As String, ByVal photopath As String, ByVal Address As String, ByVal Address1 As String, ByVal city As String, ByVal state As String, ByVal pincode As String, ByVal salutaion As String, ByVal fname As String, ByVal mname As String, ByVal lname As String, ByVal hfname As String, ByVal hmname As String, ByVal hlname As String, ByVal gender As String, ByVal age As String, ByVal dob As String, ByVal phone As String, ByVal altphone As String, ByVal mobileno As String, ByVal altmobileno As String, ByVal email As String, ByVal altemail As String, ByVal agaadhar As String, ByVal pan As String, ByVal gstn As String, ByVal AgencyTypeID As String, ByVal CreatedBy As String, ByVal ParentID As String, ByVal RoleID As String, ByVal ColumnValXML As String, ByVal isdisabled As String, ByVal upload As String, ByVal agencystatus As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal branchid As String, ByVal password As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_insupd_sponsor_rep", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        If Not repid Is Nothing Then
            If repid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@repid", repid)
            Else
                cmd.Parameters.AddWithValue("@repid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@repid", DBNull.Value)
        End If
        If Not ParentID Is Nothing Then
            If ParentID.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@parentid", ParentID)
            Else
                cmd.Parameters.AddWithValue("@parentid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@parentid", DBNull.Value)
        End If



        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@isdisable", isdisabled)
        cmd.Parameters.AddWithValue("@agencystatus", agencystatus)
        cmd.Parameters.AddWithValue("@roleid", RoleID)
        cmd.Parameters.AddWithValue("@branchid", branchid)
        cmd.Parameters.AddWithValue("@Agencytype", AgencyTypeID)
        If repname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@repname", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@repname", repname)
        End If

        If hrepname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@hrepname", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@hrepname", hrepname)
        End If

        If ColumnValXML <> Nothing Then
            cmd.Parameters.AddWithValue("@ColumnValXML", ColumnValXML)
        Else
            cmd.Parameters.AddWithValue("@ColumnValXML", DBNull.Value)
        End If

        If uploadpath.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@uploadpath", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@uploadpath", uploadpath)
        End If
        If photopath.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@photopath", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@photopath", photopath)
        End If

        If Address.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Ag_Address", Address)
        End If
        If Address1.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@Ag_Address1", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@Ag_Address1", Address1)
        End If

        If city.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_address_city", city)
        End If
        If state.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_address_state", state)
        End If
        If pincode.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_pincode", pincode)
        End If
        If salutaion.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_salutation", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_salutation", salutaion)
        End If
        If fname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_first_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_first_name", fname)
        End If
        If mname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_m_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_m_name", mname)
        End If
        If lname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_l_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_l_name", lname)
        End If
        If hfname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hfirst_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hfirst_name", hfname)
        End If
        If hmname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hm_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hm_name", hmname)
        End If
        If hlname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hl_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hl_name", hlname)
        End If
        If gender.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gender", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gender", gender)
        End If
        If age.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_age", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_age", age)
        End If
        If dob.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_dob", dob)
        End If
        If phone.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_phone", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_phone", phone)
        End If
        If altphone.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_alternative_phone", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_phone", altphone)
        End If
        If mobileno.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_mobileno", mobileno)
        End If
        If altmobileno.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", altmobileno)
        End If
        If email.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_email", email)
        End If
        If altemail.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_alternative_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_email", altemail)
        End If
        If agaadhar.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_aadhar", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_aadhar", agaadhar)
        End If
        If pan.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_pan", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_pan", pan)
        End If
        If gstn.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gstn", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gstn", gstn)
        End If




        If Not createdbyempid Is Nothing Then
            If createdbyempid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            Else
                cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
        End If

        If Not fwdempid Is Nothing Then
            If fwdempid.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
            Else
                cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
        End If

        If Not password Is Nothing Then
            cmd.Parameters.AddWithValue("@password", password)
        Else
            cmd.Parameters.AddWithValue("@password", DBNull.Value)
        End If


        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function TRG_CHECK_TOKEN(ByVal APIKEY As String) As Boolean
        If APIKEY Is Nothing Then
            Return False
        End If
        If APIKEY.ToString.ToUpper = Datamanager.API_TEST_KEY.ToUpper Then
            Dim currentdate As Date = General.Get_DB_Date(Common.getDateTime().ToString("dd/MM/yyyy"))
            Dim ValidtityDate As Date = General.Get_DB_Date(Datamanager.API_TEST_KEY_VALIDITY)
            'If currentdate <= ValidtityDate Then
            '    Return True
            'Else
            '    Return False
            'End If
            Return True
        Else
            If APIKEY.ToString.ToUpper = Datamanager.API_KEY.ToUpper Then
                Dim currentdate As Date = General.Get_DB_Date(Common.getDateTime().ToString("dd/MM/yyyy"))
                Dim ValidtityDate As Date = General.Get_DB_Date(Datamanager.API_KEY_VALIDITY)
                'If currentdate <= ValidtityDate Then
                '    Return True
                'Else
                '    Return False
                'End If
                Return True
            Else
                Return False
            End If

        End If

        ' Return True
    End Function
    Public Function DerializeDataTable(ByVal json As String) As DataTable
        Dim dt As DataTable
        dt = JsonConvert.DeserializeObject(Of DataTable)(json)
        Return dt
    End Function

    Public Function SAVE_REC_SESSION(ByVal Domain As String, ByVal isOnline As String, ByVal trainingplanid As String, ByVal timetableid As String, ByVal CreatedBy As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal docremark As String, ByVal docdate As String, ByVal docstatus As String, ByVal sessionxml As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_ins_trg_session", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingPlanId", trainingplanid)
        cmd.Parameters.AddWithValue("@Timetableid", timetableid)

        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
        cmd.Parameters.AddWithValue("@branchid", branchid)
        If Not docremark Is Nothing Then
            cmd.Parameters.AddWithValue("@docremark", docremark)
        Else
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@docdate", docdate)
        cmd.Parameters.AddWithValue("@createdby_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        cmd.Parameters.AddWithValue("@tat_type_id", "87")
        cmd.Parameters.AddWithValue("@doc_status", docstatus)
        cmd.Parameters.AddWithValue("@uploaded_doc", DBNull.Value)
        cmd.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@SessionXML", sessionxml)


        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function Insupd_Folder_Data(ByVal Domain As String, ByVal Isonline As String, ByVal folderid As String, ByVal foldername As String, ByVal createdby As String) As Boolean
        con = Get_Connection_String(Domain, Isonline)
        con.Open()
        cmd = New SqlCommand("content.tbl_Content_FolderInsert", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        If Not folderid Is Nothing Then
            cmd.Parameters.AddWithValue("@GlobalContentFolderID", folderid)
        End If

        cmd.Parameters.AddWithValue("@GlobalContentFolderName", foldername)
        cmd.Parameters.AddWithValue("@CreatedbyAgencyID", createdby)
        cmd.Parameters.AddWithValue("@CreatedOn", Get_CreatedOn_Server(Domain, Isonline))


        cmd.ExecuteNonQuery()
        con.Close()

        Return True


    End Function

    Public Function GET_Folder_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal Folderid As String) As DataTable
        Dim dtfolders As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[Content].[tbl_Content_FolderSelect]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        ' cmd.Parameters.AddWithValue("@GlobalContentFolderID", Folderid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtfolders)
        con.Close()
        Return dtfolders
    End Function


    Public Function Ins_Global_Content(ByVal Domain As String, ByVal Isonline As String, ByVal contentid As String, ByVal contenttypeid As String, ByVal contenttitle As String, ByVal tags As String, ByVal folderid As String, ByVal contenttext As String, ByVal filepath As String, ByVal filename As String, ByVal createdby As String, ByVal createdempid As String, ByVal fwdempid As String, ByVal branchid As String, ByVal docdate As String, ByVal createdbyempid As String, ByVal tat_id As String, ByVal status As String, ByVal remark As String, ByVal ThumbnailPath As String, ByVal tcm_content_reading_time As String) As Boolean
        con = Get_Connection_String(Domain, Isonline)
        con.Open()
        cmd = New SqlCommand("content.sp_insert_tbl_ContentMaster", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        cmd.Parameters.AddWithValue("@p_GlobalContentID", contentid)
        If Not contenttypeid Is Nothing Then
            cmd.Parameters.AddWithValue("@p_GlobalContentyTypeID", contenttypeid)
        Else
            cmd.Parameters.AddWithValue("@p_GlobalContentyTypeID", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@p_GlobalContentTitle", contenttitle)
        cmd.Parameters.AddWithValue("@p_GlobalContentTag", tags)
        If Not folderid Is Nothing Then
            cmd.Parameters.AddWithValue("@p_GlobalContentFolderID", folderid)
        Else
            cmd.Parameters.AddWithValue("@p_GlobalContentFolderID", DBNull.Value)
        End If


        If Not contenttext Is Nothing Then
            cmd.Parameters.AddWithValue("@p_GlobalWysiwagText", contenttext)
        Else
            cmd.Parameters.AddWithValue("@p_GlobalWysiwagText", DBNull.Value)
        End If

        If Not filepath Is Nothing Then
            cmd.Parameters.AddWithValue("@p_GlobalFilePath", filepath)
        Else
            cmd.Parameters.AddWithValue("@p_GlobalFilePath", DBNull.Value)
        End If
        If Not filename Is Nothing Then
            cmd.Parameters.AddWithValue("@p_GlobalFileName", filename)
        Else
            cmd.Parameters.AddWithValue("@p_GlobalFileName", DBNull.Value)
        End If
        If Not remark Is Nothing Then
            cmd.Parameters.AddWithValue("@tttds_info_desc", remark)
        Else
            cmd.Parameters.AddWithValue("@tttds_info_desc", DBNull.Value)
        End If


        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, Isonline))
        cmd.Parameters.AddWithValue("@createdby", createdby)
        cmd.Parameters.AddWithValue("@branchid", branchid)

        If Not remark Is Nothing Then
            cmd.Parameters.AddWithValue("@docremark", remark)
        Else
            cmd.Parameters.AddWithValue("@docremark", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@docdate", docdate)
        cmd.Parameters.AddWithValue("@actiondate", Get_CreatedOn_Server(Domain, Isonline))
        cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
        cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        cmd.Parameters.AddWithValue("@tat_type_id", tat_id)
        cmd.Parameters.AddWithValue("@doc_status", status)

        If (Not tcm_content_reading_time Is Nothing) Then
            cmd.Parameters.AddWithValue("@p_tcm_content_reading_time", tcm_content_reading_time)
        Else
            cmd.Parameters.AddWithValue("@p_tcm_content_reading_time", "0")
        End If


        If Not ThumbnailPath Is Nothing Then
            cmd.Parameters.AddWithValue("@p_Global_Thumbnail_FilePath", ThumbnailPath)
        Else
            cmd.Parameters.AddWithValue("@p_Global_Thumbnail_FilePath", DBNull.Value)
        End If


        cmd.ExecuteNonQuery()
        con.Close()

        Return True


    End Function

    Public Function GET_FILE_TYPE_Data(ByVal Domain As String, ByVal IsOnline As String) As DataTable
        Dim dtfolders As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[Content].[tbl_GlobalContentTypeSelect]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        ' cmd.Parameters.AddWithValue("@GlobalContentFolderID", Folderid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtfolders)
        con.Close()
        Return dtfolders
    End Function
    Public Function Get_Agency_Sessions_Details(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal agencyid As String) As DataTable
        Dim dtagencydetails As New DataTable

        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.CommandText = "YUser_Proc_Get_Agency_Session_Details"
            cmd.Parameters.AddWithValue("v_agencyid", agencyid)
            cmd.Connection = con
            con.Open()
            Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dtagencydetails)
            con.Close()
        Else
            Dim cnn As SqlConnection
            Dim cmd As SqlCommand
            Dim oDataAccess As New dataAccess
            cnn = Get_Connection_String(Domain, IsOnline)
            cnn.Open()

            cmd = New SqlCommand("YUser.Proc_Get_Agency_Session_Details", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = cnn
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@agencyid", agencyid)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dtagencydetails)
            cnn.Close()
        End If

        Return dtagencydetails

    End Function  ' Need to be discussed






    Public Function Get_User_Test_List(ByVal Domain As String, ByVal isonline As String, ByVal usertype As String, ByVal loginuserid As String) As DataTable
        Dim dtcode As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        ' cmd = New SqlCommand("select distinct trainingcode from trainingplan.Vw_tp_trg_time_table where ttttt_timetableid='" + timetableid + "'", con)
        cmd = New SqlCommand("eval.GetTestListWithUserType", con)
        cmd.Parameters.AddWithValue("@userid", loginuserid)
        cmd.Parameters.AddWithValue("@usertype", usertype)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcode)
        con.Close()
        Return dtcode
    End Function



    Public Function Get_QUES_SKILL_List(ByVal Domain As String, ByVal isonline As String, ByVal trainingcategoryid As String) As DataTable
        Dim dtcode As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        'cmd = New SqlCommand("select distinct SkillTag from eval.Questions where [Training.TrainingCategoryID]='" + trainingcategoryid + "'", con)
        '        cmd = New SqlCommand("select SkillTag,[Training.TrainingCategoryID],count(*) as noofquestions  from eval.Questions
        'where [Training.TrainingCategoryID]='" + trainingcategoryid + "'
        'group by SkillTag,[Training.TrainingCategoryID]
        '", con)
        cmd = New SqlCommand("SELECT distinct value as SkillTag FROM eval.Questions CROSS APPLY STRING_SPLIT(SkillTag, ',') where [Training.TrainingCategoryID]= '" + trainingcategoryid + "'
", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcode)
        con.Close()
        Return dtcode
    End Function




    Public Function Get_TRG_SESS_TEST_List(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String) As DataTable
        Dim dtcode As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        ' cmd = New SqlCommand("select distinct trainingcode from trainingplan.Vw_tp_trg_time_table where ttttt_timetableid='" + timetableid + "'", con)
        cmd = New SqlCommand("select * from eval.TestQuestions where [Training.TrainingID]='" + trainingid + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcode)
        con.Close()
        Return dtcode
    End Function


    Public Function Get_TEST_Result_List(ByVal Domain As String, ByVal isonline As String, ByVal testquestionid As String, ByVal pageno As String, ByVal pagesize As String, ByVal participantid As String) As DataTable
        Dim dtcode As New DataTable
        'con = Get_Connection_String(Domain, isonline)
        'con.Open()
        '' cmd = New SqlCommand("select distinct trainingcode from trainingplan.Vw_tp_trg_time_table where ttttt_timetableid='" + timetableid + "'", con)
        'cmd = New SqlCommand("Eval.GetPrarticipantResultbyTestQuestionID", con)
        'cmd.CommandType = CommandType.StoredProcedure

        'cmd.Parameters.AddWithValue("@TestQuestionId", testquestionid)
        'If Not pageno Is Nothing Then
        '    cmd.Parameters.AddWithValue("@PageNo", pageno)
        'Else
        '    cmd.Parameters.AddWithValue("@PageNo", DBNull.Value)
        'End If


        'If Not pagesize Is Nothing Then
        '    cmd.Parameters.AddWithValue("@PageSize", pagesize)
        'Else
        '    cmd.Parameters.AddWithValue("@PageSize", DBNull.Value)
        'End If
        'If Not participantid Is Nothing Then
        '    cmd.Parameters.AddWithValue("@participantID", participantid)
        'Else
        '    cmd.Parameters.AddWithValue("@participantID", DBNull.Value)
        'End If



        'cmd.CommandTimeout = 5000
        'cmd.Connection = con
        'Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        'daTrainingCalender.Fill(dtcode)
        'con.Close()
        'Return dtcode

        Dim cnn As SqlConnection
        Dim cmd As SqlCommand
        Dim oDataAccess As New dataAccess
        cnn = Get_Connection_String(Domain, isonline)
        cnn.Open()

        cmd = New SqlCommand("eval.GetPrarticipantResultbyTestQuestionID", cnn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = cnn
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TestQuestionId", testquestionid)
        If Not pageno Is Nothing Then
            cmd.Parameters.AddWithValue("@PageNo", pageno)

        End If


        If Not pagesize Is Nothing Then
            cmd.Parameters.AddWithValue("@PageSize", pagesize)

        End If
        If Not participantid Is Nothing Then
            cmd.Parameters.AddWithValue("@participantID", participantid)

        End If

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtcode)
        cnn.Close()

        Return dtcode
    End Function



    Public Function SendMail_With_XML(ByVal usermailid As String, ByVal Subject As String, ByVal Body As String, ByVal userdisplayname As String, ByVal Optional attachment As String = Nothing, ByVal Optional ccto As String = Nothing, ByVal Optional bccto As String = Nothing, Optional isOTP As Int16 = 0)
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try
            'Commented due to SMS mail not required in case of ANON


            Dim isMailRequired As String = ""
            'Commented on 20.11.2023 due to now Is mail required checked by Mail setting on Applicationsetting 
            'Also this is not checked in case of OTP if OTP required true then direct send otp from here
            'isMailRequired = GET_GLOBAL_SETTING("IS_MAIL_REQUIRED")

            If isOTP = 1 Then
                isMailRequired = "1"
            Else
                Dim otpsetting As ApplicationSetting.EMAIL_SEND_BY_APPLICATION

                Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
                Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
                Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=7")
                Dim response = request.GetResponse()
                Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
                Dim j As JObject = JObject.Parse(responseString)
                otpsetting = j.ToObject(Of ApplicationSetting.EMAIL_SEND_BY_APPLICATION)()
                If otpsetting.IS_MAIL_SEND = 1 Then
                    isMailRequired = "1"
                Else
                    isMailRequired = "0"
                End If
            End If



            If isMailRequired = "1" Then
                Dim ClientUrl As String = ""
                Dim ClientName As String = ""
                Dim login As String = ""
                Dim Password As String = ""
                Dim portno As String = ""
                Dim host As String = ""
                Dim header As String = ""
                Dim Footer As String = ""



                Dim msg As New MailMessage()
                Dim smtpServer As New SmtpClient()

                Dim es As New EmailConfiguration
                es = Get_Mail_Setting(1)
                ClientUrl = es.clienturl
                ClientName = es.clientname
                login = es.login
                Password = es.password
                portno = es.portno
                host = es.host
                header = es.header


                header = header.Replace("(#todaydate#)", CDate(Common.getDateTime()).ToString("dd/MM/yyyy"))


                Dim mailid As String = login
                Dim pwd As String = Password


                Body = header.ToString + Body.ToString + Footer.ToString
                Body = Body.Replace("(#regname#)", userdisplayname)
                smtpServer.Credentials = New Net.NetworkCredential(mailid, pwd)

                smtpServer.Port = portno
                smtpServer.Host = host
                smtpServer.EnableSsl = True
                'smtpServer.UseDefaultCredentials = True

                msg.To.Add(usermailid)
                msg.From = New MailAddress(mailid, ClientName, System.Text.Encoding.UTF8)
                msg.Subject = Subject
                msg.Body = Body
                msg.IsBodyHtml = True
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                msg.ReplyTo = New MailAddress(mailid)

                If Not ccto Is Nothing Then
                    msg.CC.Add(ccto)
                End If
                If Not bccto Is Nothing Then
                    msg.Bcc.Add(ccto)
                End If



                If Not attachment Is Nothing Then
                    'Dim AttachChallan As Attachment = New Attachment(HttpContext.Current.Server.MapPath(attachment), System.Net.Mime.MediaTypeNames.Application.Pdf)
                    Dim AttachChallan As Attachment = New Attachment(HttpContext.Current.Server.MapPath(attachment))
                    msg.Attachments.Add(AttachChallan)
                End If
                logger.Error("DataManager - SendMail_With_XML->Start Sending Mail " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
                smtpServer.Send(msg)
                logger.Error("DataManager - SendMail_With_XML->End Sending Mail " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
            End If



        Catch ex As Exception
            logger.Error("DataManager - SendMail_With_XML " + ex.Message)
        End Try
    End Function


    Public Function SendMail_With_XML_BACKGROUND(ByVal usermailid As String, ByVal Subject As String, ByVal Body As String, ByVal userdisplayname As String, ByVal isMailRequired As String, ByVal emailsetting As EmailConfiguration, ByVal Optional attachment As String = Nothing, ByVal Optional ccto As String = Nothing, ByVal Optional bccto As String = Nothing)
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try
            'Commented due to SMS mail not required in case of ANON

            logger.Error("Start mail")
            If isMailRequired = "1" Then
                logger.Error("in is mail mail")
                Dim ClientUrl As String = ""
                Dim ClientName As String = ""
                Dim login As String = ""
                Dim Password As String = ""
                Dim portno As String = ""
                Dim host As String = ""
                Dim header As String = ""
                Dim Footer As String = ""



                Dim msg As New MailMessage()
                Dim smtpServer As New SmtpClient()
                logger.Error("Start email setting mail")
                Dim es As New EmailConfiguration
                es = emailsetting
                ClientUrl = es.clienturl
                ClientName = es.clientname
                login = es.login
                Password = es.password
                portno = es.portno
                host = es.host
                header = es.header
                logger.Error("read setting mail")

                header = header.Replace("(#todaydate#)", CDate(Common.getDateTime()).ToString("dd/MM/yyyy"))


                Dim mailid As String = login
                Dim pwd As String = Password


                Body = header.ToString + Body.ToString + Footer.ToString
                Body = Body.Replace("(#regname#)", userdisplayname)
                smtpServer.Credentials = New Net.NetworkCredential(mailid, pwd)

                smtpServer.Port = portno
                smtpServer.Host = host
                smtpServer.EnableSsl = True
                'smtpServer.UseDefaultCredentials = True

                msg.To.Add(usermailid)
                msg.From = New MailAddress(mailid, ClientName, System.Text.Encoding.UTF8)
                msg.Subject = Subject
                msg.Body = Body
                msg.IsBodyHtml = True
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                msg.ReplyTo = New MailAddress(mailid)

                If Not ccto Is Nothing Then
                    msg.CC.Add(ccto)
                End If
                If Not bccto Is Nothing Then
                    msg.Bcc.Add(ccto)
                End If


                logger.Error("set data")
                logger.Error("attachment" + attachment)
                If Not attachment Is Nothing Then
                    'Dim AttachChallan As Attachment = New Attachment(HttpContext.Current.Server.MapPath(attachment), System.Net.Mime.MediaTypeNames.Application.Pdf)
                    Dim AttachChallan As Attachment = New Attachment(attachment)
                    msg.Attachments.Add(AttachChallan)
                End If
                logger.Error("set attachment")
                logger.Error("DataManager - SendMail_With_XML->Start Sending Mail " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
                smtpServer.Send(msg)
                logger.Error("DataManager - SendMail_With_XML->End Sending Mail " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
            End If



        Catch ex As Exception
            logger.Error("DataManager - SendMail_With_XML " + ex.Message)
        End Try
    End Function

    Public Function Get_Meeting_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal meetingid As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_lms_get_meeting_data", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@tttlm_id", meetingid)



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function



    Public Function Get_User_Data(ByVal Domain As String, ByVal isonline As String, ByVal username As String) As DataTable
        Dim dtFacultyreg As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("[YUSER].[uservalidate]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@UserName", username)

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtFacultyreg)
        con.Close()
        Return dtFacultyreg
    End Function


    Public Function Get_OTP() As Hashtable
        Dim ht As New Hashtable
        Dim charArr As Char() = "0123456789".ToCharArray()
        Dim strrandom As String = String.Empty
        Dim objran As New Random()
        Dim noofcharacters As Integer = 6
        For i As Integer = 0 To noofcharacters - 1
            'It will not allow Repetation of Characters
            Dim pos As Integer = objran.[Next](1, charArr.Length)
            If Not strrandom.Contains(charArr.GetValue(pos).ToString()) Then
                strrandom += charArr.GetValue(pos)
            Else
                i -= 1
            End If
        Next

        ht.Add("OTPID", Guid.NewGuid.ToString.Substring(0, 4).ToUpper)
        ht.Add("OTP", strrandom)

        System.Web.HttpContext.Current.Session("OTPSentTime") = System.DateTime.Now
        System.Web.HttpContext.Current.Session("OTP") = strrandom
        Return ht
    End Function


    Public Function GET_GLOBAL_SETTING(ByVal Tag As String) As String

        Dim sb As New StringBuilder
        Dim dtTableData As New DataTable
        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object) = Nothing
        Dim keyvalue As String = ""
        Dim gs As New GlobalSetting
        gs = Get_Global_XML_Setting()

        If Tag.ToString().ToUpper = "SHARE_CONTENT_ON_GOOGLE_DRIVE" Then
            keyvalue = gs.share_content_on_google_drive
        ElseIf Tag.ToString().ToUpper = "ADMIN_MAIL_ID" Then
            keyvalue = gs.Admin_Mail_Id
        ElseIf Tag.ToString().ToUpper = "IS_MAIL_REQUIRED" Then
            keyvalue = gs.IS_MAIL_REQUIRED
        ElseIf Tag.ToString().ToUpper = "REGISTRATION_COMPULSORY_FIELD" Then
            keyvalue = gs.REGISTRATION_COMPULSORY_FIELD
        ElseIf Tag.ToString().ToUpper = "IS_SMS_REQUIRED" Then
            keyvalue = gs.IS_SMS_REQUIRED
        ElseIf Tag.ToString().ToUpper = "IS_PAID_TRG_REQUIRED" Then
            keyvalue = gs.IS_PAID_TRG_REQUIRED
        End If


        Return keyvalue


    End Function







    Public Function SAVE_CD_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal facultyid As String, ByVal agencystatus As String, ByVal CreatedBy As String, ByVal branchid As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal docpath As String, ByVal photopath As String, ByVal address As String, ByVal pincode As String, ByVal salutation As String, ByVal fname As String, ByVal mname As String, ByVal lname As String, ByVal hfname As String, ByVal hmname As String, ByVal hlname As String, ByVal gender As String, ByVal dob As String, ByVal phone As String, ByVal mobile As String, ByVal altMobile As String, ByVal email As String, ByVal aadhar As String, ByVal roleid As String, ByVal agencytype As String, ByVal city As String, ByVal state As String, ByVal FacXML As String, ByVal CourseXML As String, ByVal password As String, ByVal agencyname As String, ByVal hagencyname As String, ByVal isdisable As String, ByVal usertype As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_trg_ins_upd_staff", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@AgencyId", facultyid)
        cmd.Parameters.AddWithValue("@AgencyName", agencyname)
        cmd.Parameters.AddWithValue("@HAgencyName", hagencyname)
        cmd.Parameters.AddWithValue("@AgencyTypeID", agencytype)
        cmd.Parameters.AddWithValue("@Createdby", CreatedBy)
        cmd.Parameters.AddWithValue("@Branchid", branchid)
        ' cmd.Parameters.AddWithValue("@roleid", DBNull.Value)

        If Not docpath Is Nothing Then
            If docpath.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@docpath", docpath)
            End If
        End If
        If Not photopath Is Nothing Then
            If photopath.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@photopath", photopath)
            End If
        End If
        If Not agencystatus Is Nothing Then
            If agencystatus.ToString() <> "" Then
                cmd.Parameters.AddWithValue("@agencystatus", agencystatus)
            End If
        End If
        If Not address Is Nothing Then
            If address.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@Ag_Address", address)
            End If
        Else
            cmd.Parameters.AddWithValue("@Ag_Address", DBNull.Value)
        End If

        If Not city Is Nothing Then
            If city.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_address_city", city)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_address_city", DBNull.Value)
        End If
        If Not city Is Nothing Then
            If state.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_address_state", state)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_address_state", DBNull.Value)
        End If
        If Not city Is Nothing Then
            If pincode.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_pincode", pincode)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_pincode", DBNull.Value)
        End If
        If salutation.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_salutation", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_salutation", salutation)
        End If
        If fname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_first_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_first_name", fname)
        End If
        If mname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_m_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_m_name", mname)
        End If
        If lname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_l_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_l_name", lname)
        End If
        If hfname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hfirst_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hfirst_name", hfname)
        End If
        If hmname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hm_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hm_name", hmname)
        End If
        If hlname.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_hl_name", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_hl_name", hlname)
        End If
        If gender.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_gender", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_gender", gender)
        End If


        If Not dob Is Nothing Then
            If dob.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_dob", dob)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_dob", DBNull.Value)
        End If
        If Not phone Is Nothing Then
            If phone.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_phone", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_phone", phone)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_phone", DBNull.Value)
        End If

        If Not mobile Is Nothing Then
            If mobile.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_mobileno", mobile)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_mobileno", DBNull.Value)
        End If
        If Not altMobile Is Nothing Then
            If altMobile.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_alternative_mobileno", altMobile)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_alternative_mobileno", DBNull.Value)
        End If
        If email.ToString.Trim = "" Then
            cmd.Parameters.AddWithValue("@ag_email", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ag_email", email)
        End If


        If Not aadhar Is Nothing Then
            If aadhar.ToString.Trim = "" Then
                cmd.Parameters.AddWithValue("@ag_aadhar", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ag_aadhar", aadhar)
            End If
        Else
            cmd.Parameters.AddWithValue("@ag_aadhar", DBNull.Value)
        End If


        If Not password Is Nothing Then
            cmd.Parameters.AddWithValue("@password", password)
        Else
            cmd.Parameters.AddWithValue("@password", DBNull.Value)
        End If

        If Not FacXML Is Nothing Then
            cmd.Parameters.AddWithValue("@ColumnValXML", FacXML)
        Else
            cmd.Parameters.AddWithValue("@ColumnValXML", DBNull.Value)
        End If

        If createdbyempid <> "" Then
            cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)
        Else
            cmd.Parameters.AddWithValue("@CreatedBy_empid", DBNull.Value)
            cmd.Parameters.AddWithValue("@fwd_empid", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@usertype", usertype)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function


    Public Function Get_User_Pwd_Details(ByVal Domain As String, ByVal IsOnline As String, ByVal username As String) As DataTable
        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("yuser.proc_check_and_login_user", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@UserName", username)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function


    Public Function GET_USER_AGENCY_MAPPING_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal usertype As String, ByVal agencytype As String) As DataTable
        Dim dtuserdetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[YUser].[proc_yuser_get_user_agency_mapping]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@usertype", usertype)
        cmd.Parameters.AddWithValue("@agencytype", agencytype)


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtuserdetails)
        con.Close()
        Return dtuserdetails
    End Function




    Public Function GET_TRG_LOGIN_USER_SESSIONS(ByVal Domain As String, ByVal isonline As String, ByVal usertype As String, ByVal loginuserid As String, ByVal branchid As String, ByVal trainingid As String, ByVal weekid As String) As DataSet

        Dim dsTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_loginuser_sessions", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not usertype Is Nothing Then
            cmd.Parameters.AddWithValue("@Usertype", usertype)
        Else
            If usertype <> "" Then
                cmd.Parameters.AddWithValue("@Usertype", usertype)
            Else
                cmd.Parameters.AddWithValue("@Usertype", DBNull.Value)
            End If

        End If
        If loginuserid <> "" Then
            cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        Else
            cmd.Parameters.AddWithValue("@loginuserid", DBNull.Value)
        End If
        If branchid <> "" Then
            cmd.Parameters.AddWithValue("@branchid", branchid)
        Else
            cmd.Parameters.AddWithValue("@branchid", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@trainingID", trainingid)
        If Not weekid Is Nothing Then
            If weekid = "" Then
                cmd.Parameters.AddWithValue("@week", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@week", weekid)
            End If

        Else
            cmd.Parameters.AddWithValue("@week", DBNull.Value)
        End If

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsTrainingCalender)
        con.Close()
        Return dsTrainingCalender
    End Function


    Public Function GET_CHECK_VALUE_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal type As String, ByVal chkval As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_check_value_in_agency_master]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@value", chkval)
        cmd.Parameters.AddWithValue("@type", type)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function
    Public Function GET_SESSION_PARTICIPANT_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal SessionId As String, ByVal participantid As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[Trainingplan].[proc_tp_get_participant_session]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        cmd.Parameters.AddWithValue("@sessionid", SessionId)
        If Not participantid Is Nothing Then
            cmd.Parameters.AddWithValue("@participantid", participantid)
        Else
            cmd.Parameters.AddWithValue("@participantid", DBNull.Value)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function



    Public Function DELETE_SESSION_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal sessionid As String, ByVal facultyid As String, ByVal createdby As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_delete_session", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", trainingid)
        cmd.Parameters.AddWithValue("@Sessionid", sessionid)
        If Not facultyid Is Nothing Then
            If facultyid <> "" Then
                cmd.Parameters.AddWithValue("@facultyid", facultyid)
            Else
                cmd.Parameters.AddWithValue("@facultyid", DBNull.Value)
            End If
        Else
            cmd.Parameters.AddWithValue("@facultyid", DBNull.Value)
        End If
        cmd.Parameters.AddWithValue("@createdby", createdby)
        cmd.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, isOnline))

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function


    Public Function SAVE_PARTICIPANT_ENROLL_DATES(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal participantid As String, ByVal startdate As String, ByVal enddate As String, ByVal isspecific As String, ByVal createdby As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_ins_upd_participant_joining_dt", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@Trainingid", trainingid)
        cmd.Parameters.AddWithValue("@Createdby", createdby)
        cmd.Parameters.AddWithValue("@CreatedOn", Get_CreatedOn_Server(Domain, isOnline))
        cmd.Parameters.AddWithValue("@Participantid", participantid)
        cmd.Parameters.AddWithValue("@startdate", startdate)
        cmd.Parameters.AddWithValue("@enddate", enddate)
        cmd.Parameters.AddWithValue("@isspecific", isspecific)

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function


    Public Function Extend_Training_DATES(ByVal Domain As String, ByVal isOnline As String, ByVal trainingid As String, ByVal closingdate As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.proc_tp_extend_trg_date", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        cmd.Parameters.AddWithValue("@t_closingdate", closingdate)

        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function GET_STATE_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal countryid As String) As DataTable
        Dim dtdata As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[YUser].[proc_yuser_get_state]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not countryid Is Nothing Then
            cmd.Parameters.AddWithValue("@tysm_tycom_id", countryid)
        End If

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtdata)
        con.Close()
        Return dtdata
    End Function
    Public Function GET_CITY_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal stateid As String) As DataTable
        Dim dtdata As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[YUser].[proc_yuser_get_city]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not stateid Is Nothing Then
            cmd.Parameters.AddWithValue("@tycm_tysm_id", stateid)
        End If

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtdata)
        con.Close()
        Return dtdata
    End Function


    Public Function GET_BILL_ADVICE_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal billid As String) As DataSet
        Dim dsdata As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[Trainingplan].[proc_tp_get_adjustment_advice_id]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@billid", billid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsdata)
        con.Close()
        Return dsdata
    End Function



    Public Function Get_ADJUSTMENT_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal billid As String, ByVal adjustmentid As String) As DataSet
        Dim dsTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_balance_bill_amt]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If billid <> "" Then
            cmd.Parameters.AddWithValue("@billid", billid)
        End If
        If adjustmentid <> "" Then
            cmd.Parameters.AddWithValue("@bill_adjustment_id", adjustmentid)
        End If

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsTrainingCalender)
        con.Close()
        Return dsTrainingCalender
    End Function

    Public Function Get_Connection_String(ByVal Domain As String, ByVal isonline As String) As SqlClient.SqlConnection
        Dim Sqlconnectionstring As New SqlClient.SqlConnection
        Sqlconnectionstring.ConnectionString = ConfigurationManager.ConnectionStrings.Item("constr").ToString
        Return Sqlconnectionstring
    End Function
    Enum LanguageType
        Hindi = 0
        English = 1
    End Enum
    Public Function GET_Sponsor_Category(ByVal Domain As String, ByVal IsOnline As String) As DataTable
        Dim dtcategory As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_get_tbl_sponsor_type]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcategory)
        con.Close()
        Return dtcategory
    End Function

    Public Function GET_Sponsor_Group(ByVal Domain As String, ByVal IsOnline As String) As DataTable
        Dim dtcategory As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[trainingplan].[proc_tp_get_tbl_umbrella_dept]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcategory)
        con.Close()
        Return dtcategory
    End Function

    Public Function Save_WYSIBAG_CONTENT(ByVal Domain As String, ByVal IsOnline As String, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable, ByVal dtXMLparameterlist As DataTable, ByVal XMLName As String, ByVal CreatedonParam As String, ByVal contentdata As String) As Boolean

        Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
        cnn.Open()

        Dim cmd As New SqlCommand(ProcedureName, cnn)

        cmd.CommandTimeout = 5000
        cmd.CommandType = CommandType.StoredProcedure

        Dim j As Integer
        For i = 0 To dtparameterlist.Rows.Count - 1
            For j = 0 To dtparameterlist.Columns.Count - 1
                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                    If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString.ToUpper() = "NULL" Then
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString)
                    End If


                End If
            Next
        Next

        If CreatedonParam <> "" Then
            cmd.Parameters.AddWithValue(CreatedonParam, Get_CreatedOn_Server(Domain, IsOnline))
        End If
        cmd.Parameters.AddWithValue("@GlobalWysiwagText", contentdata)
        Dim sptXMLName() As String = XMLName.Split(",")

        For i = 0 To dtXMLparameterlist.Rows.Count - 1
            For j = 0 To dtXMLparameterlist.Columns.Count - 1
                Dim xmlpara As String = dtXMLparameterlist.Rows(i)(dtXMLparameterlist.Columns(j).ColumnName).ToString
                If xmlpara.ToString = "" Then
                    Continue For
                End If

                Dim dttempxml As DataTable
                Dim xmldata As String
                If xmlpara IsNot Nothing Then
                    'dttempxml = DerializeDataTable(xmlpara)
                    dttempxml = GetDataTable(xmlpara)
                    If dttempxml.Rows.Count > 0 Then
                        Dim wr As New StringWriter
                        dttempxml.TableName = sptXMLName(j)
                        dttempxml.WriteXml(wr)
                        xmldata = wr.ToString()
                    End If

                Else
                    xmldata = Nothing
                End If
#Disable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
                cmd.Parameters.AddWithValue(dtXMLparameterlist.Columns(j).ColumnName, xmldata)
#Enable Warning BC42104 ' Variable 'xmldata' is used before it has been assigned a value. A null reference exception could result at runtime.
            Next
        Next


        cmd.ExecuteNonQuery()
        cnn.Close()

        Return True

    End Function



    Public Function getAllparticipantdata(ByVal Domain As String, ByVal isonline As String, ByVal procedurefor As String, ByVal pageno As String, ByVal pagesize As String, ByVal searchcolumn As String, ByVal searchvalue As String, ByVal filtername As String, ByVal filtervalue As String, ByVal filtername1 As String, ByVal filtervalue1 As String, ByVal trainingid As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        'cmd.CommandTimeout = 5000
        'cmd.Parameters.AddWithValue("@procedurefor", procedurefor)
        'If Not pageno Is Nothing Then
        '    cmd.Parameters.AddWithValue("@PageNo", pageno)

        'End If
        'If Not pagesize Is Nothing Then
        '    cmd.Parameters.AddWithValue("@PageSize", pagesize)

        'End If
        'If Not searchcolumn Is Nothing Then
        '    cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn)
        'Else
        '    cmd.Parameters.AddWithValue("@SearchColumn", DBNull.Value)
        'End If
        'If Not searchvalue Is Nothing Then
        '    cmd.Parameters.AddWithValue("@SearchValue", searchvalue)
        'Else
        '    cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value)
        'End If
        'If Not filtername Is Nothing Then
        '    cmd.Parameters.AddWithValue("@filtername", filtername)
        'Else
        '    cmd.Parameters.AddWithValue("@filtername", DBNull.Value)
        'End If
        'If Not filtervalue Is Nothing Then
        '    cmd.Parameters.AddWithValue("@filtervalue", filtervalue)
        'Else
        '    cmd.Parameters.AddWithValue("@filtervalue", DBNull.Value)
        'End If
        'If Not filtername1 Is Nothing Then
        '    cmd.Parameters.AddWithValue("@filtername1", filtername1)
        'Else
        '    cmd.Parameters.AddWithValue("@filtername1", DBNull.Value)
        'End If
        'If Not filtervalue1 Is Nothing Then
        '    cmd.Parameters.AddWithValue("@filtervalue1", filtervalue1)
        'Else
        '    cmd.Parameters.AddWithValue("@filtervalue1", DBNull.Value)
        'End If

        If Not trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingId", trainingid)

        End If





        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function



    Public Function GET_TRAINING_FACULTY_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String) As DataTable
        Dim dtuserdetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_trg_faculty]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", trainingid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtuserdetails)
        con.Close()
        Return dtuserdetails
    End Function
    Public Function GET_FEEDBACK_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal sessionid As String) As DataTable
        Dim dtuserdetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_trg_faculty]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@TrainingId", trainingid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtuserdetails)
        con.Close()
        Return dtuserdetails
    End Function


    Public Function GET_TRG_FEEDBACK_DATA_NEW(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal sessionid As String, ByVal feedbackdate As String) As DataSet
        Dim dsGuestFaculties As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_feedback_report", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        If feedbackdate Is Nothing Then
            cmd.Parameters.AddWithValue("@date", DBNull.Value)
        Else
            If feedbackdate <> "" Then
                cmd.Parameters.AddWithValue("@date", feedbackdate)
            Else
                cmd.Parameters.AddWithValue("@date", DBNull.Value)
            End If
        End If

        If sessionid Is Nothing Then
            cmd.Parameters.AddWithValue("@sharefeedbackid", DBNull.Value)
        Else
            If sessionid <> "" Then
                cmd.Parameters.AddWithValue("@sharefeedbackid", sessionid)
            Else
                cmd.Parameters.AddWithValue("@sharefeedbackid", DBNull.Value)
            End If
        End If

        Dim daGuestFaculties As SqlDataAdapter = New SqlDataAdapter(cmd)
        daGuestFaculties.Fill(dsGuestFaculties)
        con.Close()
        Return dsGuestFaculties
    End Function



    Public Function GET_APPLICATION_SETTING_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal settinguniqueid As String) As DataSet
        Dim dsGuestFaculties As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.sp_get_PortalSetting", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        cmd.Parameters.AddWithValue("@SettinguniqueID", settinguniqueid)
        Dim daGuestFaculties As SqlDataAdapter = New SqlDataAdapter(cmd)
        daGuestFaculties.Fill(dsGuestFaculties)
        con.Close()
        Return dsGuestFaculties
    End Function



    Public Function INSERT_APP_SETTING_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal settingname As String, ByVal settingvalue As String, ByVal settinguniqueid As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.sp_insert_PortalSetting", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@p_SettingName", settingname)
        cmd.Parameters.AddWithValue("@p_SettingValue", settingvalue)
        cmd.Parameters.AddWithValue("@p_SettinguniqueID", settinguniqueid)


        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function


    Public Function UPDATE_APP_SETTING_DATA(ByVal Domain As String, ByVal isOnline As String, ByVal settingname As String, ByVal settingvalue As String, ByVal settingid As String) As Boolean
        con = Get_Connection_String(Domain, isOnline)
        con.Open()
        cmd = New SqlCommand("trainingplan.sp_update_PortalSetting", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@p_SettingName", settingname)
        cmd.Parameters.AddWithValue("@p_SettingValue", settingvalue)
        cmd.Parameters.AddWithValue("@w_SettingID", settingid)


        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function
    Public Function GET_COUNTRY_CODE(ByVal Domain As String, ByVal IsOnline As String) As DataSet
        Dim dsGuestFaculties As New DataSet
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("yuser.prop_yuser_get_country_code", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000


        Dim daGuestFaculties As SqlDataAdapter = New SqlDataAdapter(cmd)
        daGuestFaculties.Fill(dsGuestFaculties)
        con.Close()
        Return dsGuestFaculties
    End Function

    Public Sub SEND_SMS_BY_PORTAL_SETTING(ByVal Domain As String, ByVal isonline As String, mobileNumber As String, message As String)
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Try
            Dim dtportalsetting As DataTable = GET_APPLICATION_SETTING_DATA(Domain, isonline, TRAININGAPIController.APPLICATIONSETTING.SMS).Tables(0)
            Dim Apipath As String = ""

            If dtportalsetting.Rows.Count > 0 Then
                Dim arr = Newtonsoft.Json.Linq.JObject.Parse(dtportalsetting.Rows(0)("SettingValue"))
                Apipath = arr("SMSAPI")
                If Apipath <> "" Then
                    Apipath.Replace("##Mobile##", mobileNumber)
                    Apipath.Replace("##Msg##", message)
                    Dim request As WebRequest = WebRequest.Create(Apipath)
                    Dim response As WebResponse = request.GetResponse()
                End If

            End If


        Catch ex As SystemException
            logger.Error("TrainingAPI -Get_Data  " + ex.Message)
        End Try
    End Sub


    Public Function GetAllFormRight(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal CreatedBy As String) As DataTable

        Dim dt As New DataTable
        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "YUser_GetForms"
            cmd.Parameters.AddWithValue("v_CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("v_FormRoleId", Nothing)
            cmd.Connection = con
            con.Open()
            Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dt)
        Else
            Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
            If cnn.State = ConnectionState.Closed Then
                cnn.Open()
            Else
                cnn.Close()
                cnn.Open()
            End If
            Dim cmd As New SqlCommand("YUser.GetForms", cnn)

            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 5000

            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
            da.Fill(dt)
            cnn.Close()
        End If
        Return dt
    End Function
    Public Function GetFormRoleName(ByVal Domain As String, ByVal IsOnline As String, ByVal CreatedBy As String, ByVal Type As String, ByVal procedurefor As String) As DataTable
        Dim dt As New DataTable


        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 8000
            cmd.CommandText = "YUser_GetRoleName"
            cmd.Parameters.AddWithValue("v_CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("v_type", Type)
            cmd.Parameters.AddWithValue("v_procedurefor", procedurefor)

            cmd.Connection = con
            con.Open()
            Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dt)
            con.Close()
        Else
            Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
            If cnn.State = ConnectionState.Closed Then
                cnn.Open()
            Else
                cnn.Close()
                cnn.Open()
            End If

            Dim cmd As New SqlCommand("YUser.GetRoleName", cnn)


            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("@type", Type)
            cmd.Parameters.AddWithValue("@procedurefor", procedurefor)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 5000
            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
            da.Fill(dt)
            cnn.Close()
        End If

        Return dt
    End Function
    Public Function InsUpdUserLog(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal Userid As String, ByVal Logoff As Byte, ByVal ipaddres As String) As Boolean
        Dim Flag As Boolean
        If HttpContext.Current.Session("IsMySQL") = "1" Then
            Dim con As New MySql.Data.MySqlClient.MySqlConnection
            con = SecureData.GetMysqlConnection(Domain, IsOnline)

            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "yuser_InsUpdUserLog"
            cmd.Parameters.AddWithValue("v_UserID", Userid)
            cmd.Parameters.AddWithValue("v_LogOff", Logoff)
            cmd.Parameters.AddWithValue("v_ip", ipaddres)
            cmd.Connection = con
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

        Else
            Dim cnn As SqlConnection = Get_Connection_String(Domain, IsOnline)
            cnn.Open()
            Dim cmd As SqlCommand
            cmd = New SqlCommand("[YUser].[InsUpdUserLog]", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@Userid", Userid)
            cmd.Parameters.AddWithValue("@Logoff", Logoff)
            'adding a new parameter   ipaddress
            cmd.Parameters.AddWithValue("@ip", ipaddres)

            Flag = cmd.ExecuteNonQuery()
            cnn.Close()



        End If

        Return Flag
    End Function




    Public Function Get_Session_ATTACHMENT_Detail(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal sessionid As String, ByVal attachmentid As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_upload_session_attachement]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@trainingid", trainingid)
        End If
        If Not sessionid Is Nothing Then
            cmd.Parameters.AddWithValue("@sessionid", sessionid)
        End If
        If Not attachmentid Is Nothing Then
            cmd.Parameters.AddWithValue("@attachementid", attachmentid)
        End If



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    'Public Function GET_SETTING_XML_FOLDER() As String
    '    Dim Foldername As String = "GlobalSetting"

    '    Return Foldername
    'End Function


    Public Function Get_Training_Payment_Type(ByVal Domain As String, ByVal IsOnline As String) As DataTable
        Dim dttype As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_sponsor_type", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dttype)
        con.Close()
        Return dttype
    End Function
    Public Function Get_Training_Paid_Amt(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String) As Decimal
        Dim dt As New DataTable
        Dim GrandTotal As Decimal = 0.00
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_get_training_exp_details", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        con.Close()

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dt)
        GrandTotal = LMS_CALCULAT_PAID_AMT(dt)


        Return GrandTotal
    End Function


    Function LMS_CALCULAT_PAID_AMT(ByVal dt As DataTable) As String
        Dim finalamt As Decimal = 0.00
        Dim totalamt As Decimal = 0.00
        Dim discount As Decimal = 0.00
        Dim GST As Decimal = 0.00
        Dim GatewayCharges As Decimal = 0.00
        Dim GrandTotal As Decimal = 0.00
        For i = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("T_Rate").ToString() <> "" Then
                If dt.Rows(i)("T_ExpHeadId").ToString().ToUpper() = "417119B3-18B6-43A1-8A52-064E99B6880E" Then
                    discount = discount + CDbl(dt.Rows(i)("T_Rate").ToString())
                End If
                If dt.Rows(i)("T_ExpHeadId").ToString().ToUpper() = "037A7299-2E07-444B-91AB-3B8E0565FABC" Then
                    GST = GST + CDbl(dt.Rows(i)("T_Rate").ToString())
                End If
                If dt.Rows(i)("T_ExpHeadId").ToString().ToUpper() = "0A4F1798-44B4-4C17-807A-8C83EFF7DA1D" Then
                    GatewayCharges = GatewayCharges + CDbl(dt.Rows(i)("T_Rate").ToString())
                End If
                If dt.Rows(i)("T_ExpHeadId").ToString().ToUpper() = "417119B3-18B6-43A1-8A52-064E99B6880E" Or dt.Rows(i)("T_ExpHeadId").ToString().ToUpper() = "037A7299-2E07-444B-91AB-3B8E0565FABC" Or dt.Rows(i)("T_ExpHeadId").ToString().ToUpper() = "0A4F1798-44B4-4C17-807A-8C83EFF7DA1D" Then
                    Continue For
                End If

                totalamt = totalamt + CDbl(dt.Rows(i)("T_Rate").ToString())
            End If
        Next

        Dim discountamt As Decimal = (CDbl(totalamt) * CDbl(discount)) / 100
        Dim totalafterdiscount = CDbl(totalamt) - CDbl(discountamt)

        Dim gstamount As Decimal = (CDbl(totalafterdiscount) * CDbl(GST)) / 100
        GrandTotal = CDbl(totalamt) - CDbl(discountamt) + CDbl(gstamount)
        Return GrandTotal
    End Function
    Public Function Get_Training_Signatory_Details(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String) As DataTable
        Dim dttype As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_certificate_signatory", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@ttcs_trainingid", trainingid)
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dttype)
        con.Close()
        Return dttype
    End Function



    Public Function Get_participant_Upcoming_Trg(ByVal Domain As String, ByVal IsOnline As String,
                                                  ByVal fromdate As String, ByVal todate As String, ByVal loginuserid As String, ByVal usertype As String, ByVal orderby As String, ByVal filtername As String, ByVal filterval As String, ByVal pageno As String, ByVal pagesize As String, ByVal searchcolumn As String, ByVal searchval As String) As DataSet
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[proc_tp_get_training_for_participant]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        cmd.Parameters.AddWithValue("@Usertype", usertype)
        cmd.Parameters.AddWithValue("@orderby", orderby)

        cmd.Parameters.AddWithValue("@PageNo", pageno)
        cmd.Parameters.AddWithValue("@PageSize", pagesize)
        If searchcolumn <> "" Then
            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn)
        End If
        If searchval <> "" Then
            cmd.Parameters.AddWithValue("@SearchValue", searchval)
        End If

        If filtername <> "" Then
            cmd.Parameters.AddWithValue("@filtername", filtername)
        End If
        If filterval <> "" Then
            cmd.Parameters.AddWithValue("@filtervalue", filterval)
        End If




        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function


    Public Function Save_Receipt_Rollback(ByVal Domain As String, ByVal IsOnline As String,
                                                ByVal receiptid As String, ByVal receiptdate As String, ByVal payeeid As String, ByVal createdby As String, ByVal branchid As String, ByVal paymentxml As String, ByVal rec_remark As String, ByVal receipttype As String, ByVal schemeid As String, ByVal receiptno As String, ByVal trainingid As String, ByVal litteraorderid As String,
                                                ByVal docremark As String, ByVal docdate As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal rec_tat_typeid As String, ByVal rec_docstatus As String, ByVal rec_procedurefor As String, ByVal rec_uploaded_doc As String, ByVal rec_uploaded_doc_name As String, ByVal rec_doctype As String, ByVal rec_draftletter As String, ByVal rec_is_final As String,
                                          ByVal rec_doc_lettertype As String, ByVal rec_docremarkenc As String, ByVal rec_draftletterenc As String, ByVal littera_tat_typeid As String, ByVal littera_status As String, ByVal littera_procedurefor As String, ByVal littera_uploaded_doc As String, ByVal littera_uploaded_doc_name As String, ByVal littera_doctype As String, ByVal littera_draftletter As String, ByVal littera_is_final As String, ByVal littera_doc_lettertype As String,
                                           ByVal littera_docremarkenc As String, ByVal littera_draftletterenc As String) As Boolean
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim transaction As SqlTransaction = con.BeginTransaction


        Try
            Dim cmd As New SqlCommand
            cmd = New SqlCommand("[TrainingPlan].[proc_tp_save_receipt]", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = con
            cmd.Transaction = transaction
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@ReceiptId", receiptid)
            cmd.Parameters.AddWithValue("@ReceiptDate", receiptdate)
            cmd.Parameters.AddWithValue("@SponsorId", payeeid)
            cmd.Parameters.AddWithValue("@Createdby", createdby)
            cmd.Parameters.AddWithValue("@Branchid", branchid)
            cmd.Parameters.AddWithValue("@Paymentxml", paymentxml)
            If rec_remark <> "" And rec_remark <> "NULL" Then
                cmd.Parameters.AddWithValue("@Remark", rec_remark)
            Else
                cmd.Parameters.AddWithValue("@Remark", DBNull.Value)
            End If
            If receipttype <> "" And receipttype <> "NULL" Then
                cmd.Parameters.AddWithValue("@Receipttype", receipttype)
            Else
                cmd.Parameters.AddWithValue("@Receipttype", DBNull.Value)
            End If

            cmd.Parameters.AddWithValue("@Schemeid", schemeid)
            cmd.Parameters.AddWithValue("@Receiptno_auto", receiptno)
            cmd.Parameters.AddWithValue("@trainingid", trainingid)
            cmd.Parameters.AddWithValue("@paymentorderid", litteraorderid)
            cmd.Parameters.AddWithValue("@docremark", docremark)
            cmd.Parameters.AddWithValue("@docdate", docdate)
            cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)

            cmd.Parameters.AddWithValue("@tat_type_id", rec_tat_typeid)
            cmd.Parameters.AddWithValue("@doc_status", rec_docstatus)
            cmd.Parameters.AddWithValue("@procedurefor", rec_procedurefor)
            cmd.Parameters.AddWithValue("@uploaded_doc", rec_uploaded_doc)
            If rec_uploaded_doc_name <> "" And rec_uploaded_doc_name <> "NULL" Then
                cmd.Parameters.AddWithValue("@uploaded_doc_name", rec_uploaded_doc_name)
            Else
                cmd.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
            End If
            If rec_doctype <> "" And rec_doctype <> "NULL" Then
                cmd.Parameters.AddWithValue("@doctype", rec_doctype)
            Else
                cmd.Parameters.AddWithValue("@doctype", DBNull.Value)
            End If
            If rec_draftletter <> "" And rec_draftletter <> "NULL" Then
                cmd.Parameters.AddWithValue("@draftletter", rec_draftletter)
            Else
                cmd.Parameters.AddWithValue("@draftletter", DBNull.Value)
            End If
            If rec_draftletter <> "" And rec_is_final <> "NULL" Then
                cmd.Parameters.AddWithValue("@tttds_is_final", rec_is_final)
            Else
                cmd.Parameters.AddWithValue("@tttds_is_final", DBNull.Value)
            End If
            If rec_doc_lettertype <> "" And rec_doc_lettertype <> "NULL" Then
                cmd.Parameters.AddWithValue("@tttds_letter_type", rec_doc_lettertype)
            Else
                cmd.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
            End If
            If rec_docremarkenc <> "" And rec_docremarkenc <> "NULL" Then
                cmd.Parameters.AddWithValue("@docremarkenc", rec_docremarkenc)
            Else
                cmd.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
            End If

            If rec_draftletterenc <> "" And rec_draftletterenc <> "NULL" Then
                cmd.Parameters.AddWithValue("@draftletterenc", rec_draftletterenc)
            Else
                cmd.Parameters.AddWithValue("@draftletterenc", DBNull.Value)
            End If

            cmd.ExecuteNonQuery()



            'Litter order entry
            Dim cmd1 As New SqlCommand
            cmd1 = New SqlCommand("[TrainingPlan].[sp_insert_litteraorder]", con)
            cmd1.CommandType = CommandType.StoredProcedure
            cmd1.Connection = con
            cmd1.Transaction = transaction
            cmd1.CommandTimeout = 5000
            cmd1.Parameters.AddWithValue("@p_LitteraOrderID", litteraorderid)
            cmd1.Parameters.AddWithValue("@p_LitteraOrderDateandtime", Get_CreatedOn_Server(Domain, IsOnline))
            cmd1.Parameters.AddWithValue("@p_ParticipantId", payeeid)
            cmd1.Parameters.AddWithValue("@p_Createdby", createdby)
            cmd1.Parameters.AddWithValue("@p_createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd1.Parameters.AddWithValue("@p_Branchid", branchid)
            cmd1.Parameters.AddWithValue("@p_Schemeid", schemeid)
            cmd1.Parameters.AddWithValue("@p_Razorpay_order_ID", receiptno)
            cmd1.Parameters.AddWithValue("@p_trainingid", trainingid)
            cmd1.Parameters.AddWithValue("@p_PAYMENTXML", paymentxml)
            cmd1.Parameters.AddWithValue("@docremark", docremark)
            cmd1.Parameters.AddWithValue("@docdate", docdate)
            cmd1.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            cmd1.Parameters.AddWithValue("@fwd_empid", fwdempid)
            cmd1.Parameters.AddWithValue("@tat_type_id", littera_tat_typeid)
            cmd1.Parameters.AddWithValue("@doc_status", littera_status)

            cmd1.Parameters.AddWithValue("@procedurefor", littera_procedurefor)
            If littera_uploaded_doc <> "" And littera_uploaded_doc <> "NULL" Then
                cmd1.Parameters.AddWithValue("@uploaded_doc", littera_uploaded_doc)
            Else
                cmd1.Parameters.AddWithValue("@uploaded_doc", DBNull.Value)
            End If
            If littera_uploaded_doc_name <> "" And littera_uploaded_doc_name <> "NULL" Then
                cmd1.Parameters.AddWithValue("@uploaded_doc_name", littera_uploaded_doc_name)
            Else
                cmd1.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
            End If
            If littera_doctype <> "" And littera_doctype <> "NULL" Then
                cmd1.Parameters.AddWithValue("@doctype", littera_doctype)
            Else
                cmd1.Parameters.AddWithValue("@doctype", DBNull.Value)
            End If
            If littera_draftletter <> "" And littera_draftletter <> "NULL" Then
                cmd1.Parameters.AddWithValue("@draftletter", littera_draftletter)
            Else
                cmd1.Parameters.AddWithValue("@draftletter", DBNull.Value)
            End If
            If littera_is_final <> "" And littera_is_final <> "NULL" Then
                cmd1.Parameters.AddWithValue("@tttds_is_final", littera_is_final)
            Else
                cmd1.Parameters.AddWithValue("@tttds_is_final", DBNull.Value)
            End If
            If littera_doc_lettertype <> "" And littera_doc_lettertype <> "NULL" Then
                cmd1.Parameters.AddWithValue("@tttds_letter_type", littera_doc_lettertype)
            Else
                cmd1.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
            End If
            If littera_docremarkenc <> "" And littera_docremarkenc <> "NULL" Then
                cmd1.Parameters.AddWithValue("@docremarkenc", littera_docremarkenc)
            Else
                cmd1.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
            End If
            If littera_draftletterenc <> "" And littera_draftletterenc <> "NULL" Then
                cmd1.Parameters.AddWithValue("@draftletterenc", littera_draftletterenc)
            Else
                cmd1.Parameters.AddWithValue("@draftletterenc", DBNull.Value)
            End If


            cmd1.ExecuteNonQuery()

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return True
    End Function


    Public Function Get_Test_Report_Data(ByVal Domain As String, ByVal IsOnline As String,
                                                  ByVal trainingid As String, ByVal participantid As String) As DataTable
        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[Eval].[GetTestDetailsforParticipant]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingId", trainingid)
        cmd.Parameters.AddWithValue("@participantID", participantid)

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function


    Public Function GetParticipantsAttendancedata(ByVal Domain As String, ByVal IsOnline As String, ByVal TrainingId As String, ByVal sessionid As String, ByVal fordate As String) As DataSet
        Dim dtKit As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.TP_GetParticipantsAttendance", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@TrainingId", TrainingId)
        cmd.Parameters.AddWithValue("@date", fordate)
        cmd.CommandTimeout = 5000
        Dim daKit As SqlDataAdapter = New SqlDataAdapter(cmd)
        daKit.Fill(dtKit)
        con.Close()

        Return dtKit
    End Function

    Public Function getparticipants_New(ByVal Domain As String, ByVal isonline As String, ByVal trainingplanid As String, ByVal sessionid As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Or trainingplanid = "" Then
            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingid", trainingplanid)
        End If
        'If sessionid Is Nothing Or sessionid = "" Then
        '    cmd.Parameters.AddWithValue("@sessionid", DBNull.Value)
        'Else
        '    cmd.Parameters.AddWithValue("@sessionid", sessionid)
        'End If

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function

    Public Function getAttendance_New(ByVal Domain As String, ByVal isonline As String, ByVal trainingplanid As String, ByVal sessionid As String, branchid As String, ByVal adate As String) As DataSet
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_attendance", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@trainingid", trainingplanid)
        cmd.Parameters.AddWithValue("@sessionid", sessionid)
        cmd.Parameters.AddWithValue("@BranchId", branchid)
        cmd.Parameters.AddWithValue("@date", adate)

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Return dsSchemes
    End Function
    Public Function Get_PARTICULAR_SESSION_DETAILS(ByVal Domain As String, ByVal IsOnline As String, ByVal Sessionid As String) As DataTable
        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Select * from Trainingplan.Vw_tp_trg_time_table where ttttt_session_id='" + Sessionid + "' ", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function Get_QUES_SKILL_QUESTIONS(ByVal Domain As String, ByVal isonline As String, ByVal trainingcategoryid As String, ByVal tags As String) As DataTable
        Dim dtcode As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        'cmd = New SqlCommand("select distinct SkillTag from eval.Questions where [Training.TrainingCategoryID]='" + trainingcategoryid + "'", con)
        '        cmd = New SqlCommand("select SkillTag,[Training.TrainingCategoryID],count(*) as noofquestions  from eval.Questions
        'where [Training.TrainingCategoryID]='" + trainingcategoryid + "'
        'group by SkillTag,[Training.TrainingCategoryID]
        '", con)
        cmd = New SqlCommand("select value as tags,QuestionID,[Training.TrainingCategoryID]  from eval.Questions CROSS APPLY STRING_SPLIT(SkillTag, ',')
where [Training.TrainingCategoryID]='" + trainingcategoryid + "'
and value in(select Val from YUser.Btbl('" + tags + "'))", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtcode)
        con.Close()
        Return dtcode
    End Function



    Public Function Save_Assignment_Rollback(ByVal Domain As String, ByVal IsOnline As String,
                                                ByVal assignmentid As String, ByVal assignmentname As String, ByVal instructions As String, ByVal tag As String, ByVal assessmentquestion As String, ByVal faculty As String, ByVal attachments As String, ByVal assignmenttype As String, ByVal gradeapplicable As String, ByVal Grademarks As String, ByVal createdby As String, ByVal branchid As String, ByVal sessiontype As String, ByVal assignmentstartdate As String, ByVal assignmentenddate As String, ByVal assignmentstarttime As String, ByVal assignmentendtime As String, ByVal sessionday As String, ByVal sessionweek As String, ByVal assignmentduration As String, ByVal sessionno As String, ByVal timetableid As String, ByVal trainingid As String, ByVal isopeneded As String, ByVal dtfaculy As DataTable, ByVal createdempid As String, ByVal fwdempid As String, ByVal dmsstatus As String, ByVal MaxMarks As String, ByVal sessionmodule As String, Optional minmarks As String = Nothing, Optional QuestionMarks As String = Nothing, Optional iscomplementory As String = Nothing) As Boolean
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim transaction As SqlTransaction = con.BeginTransaction


        Try
            Dim cmd As New SqlCommand
            cmd = New SqlCommand("[Assessment].[sp_insert_Assignment]", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = con
            cmd.Transaction = transaction
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@p_AssignmentID", assignmentid)
            cmd.Parameters.AddWithValue("@p_Instructions", instructions)
            cmd.Parameters.AddWithValue("@p_Tag", tag)
            cmd.Parameters.AddWithValue("@p_AssesmentQuestions", assessmentquestion)
            cmd.Parameters.AddWithValue("@p_FacultyID_Json", faculty)
            If Not attachments Is Nothing Then
                If attachments <> "" Then
                    cmd.Parameters.AddWithValue("@p_AttachmentsID_Json", attachments)
                Else
                    cmd.Parameters.AddWithValue("@p_AttachmentsID_Json", DBNull.Value)
                End If
            Else
                cmd.Parameters.AddWithValue("@p_AttachmentsID_Json", DBNull.Value)
            End If


            If QuestionMarks Is Nothing Or QuestionMarks = "" Then
                cmd.Parameters.AddWithValue("@question_max_marks", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@question_max_marks", QuestionMarks)
            End If

            cmd.Parameters.AddWithValue("@p_AssignmentTypeID", assignmenttype)
            cmd.Parameters.AddWithValue("@p_GradeApplicable", gradeapplicable)
            cmd.Parameters.AddWithValue("@p_AssignmentName", assignmentname)
            cmd.Parameters.AddWithValue("@p_createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd.Parameters.AddWithValue("@p_createdby", createdby)
            cmd.Parameters.AddWithValue("@MaxMarks", MaxMarks)
            If (minmarks <> Nothing) Then
                cmd.Parameters.AddWithValue("@min_passing_marks", minmarks)
            End If

            cmd.ExecuteNonQuery()



            ''Save Grade Data
            Dim cmd1 As New SqlCommand
            cmd1 = New SqlCommand("[Assessment].[sp_insert_Grade]", con)
            cmd1.CommandType = CommandType.StoredProcedure
            cmd1.Connection = con
            cmd1.Transaction = transaction
            cmd1.CommandTimeout = 5000
            cmd1.Parameters.AddWithValue("@p_AssignmentID", assignmentid)
            cmd1.Parameters.AddWithValue("@p_GradeID", Guid.NewGuid().ToString())
            If Grademarks <> "" And Not Grademarks Is Nothing Then
                cmd1.Parameters.AddWithValue("@p_Marks", Grademarks)
            Else
                cmd1.Parameters.AddWithValue("@p_Marks", DBNull.Value)
            End If

            cmd1.Parameters.AddWithValue("@p_GradeCritaria", DBNull.Value)
            'cmd1.Parameters.AddWithValue("@p_GradeCritaria", "1")
            cmd1.Parameters.AddWithValue("@p_RatingID", DBNull.Value)
            cmd1.Parameters.AddWithValue("@p_createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd1.Parameters.AddWithValue("@p_createdby", createdby)
            cmd1.ExecuteNonQuery()


            ''Save Session Data
            Dim sessionid As String = Guid.NewGuid.ToString()
            Dim cmd2 As New SqlCommand
            cmd2 = New SqlCommand("[TrainingPlan].[proc_tp_ins_upd_session]", con)
            cmd2.CommandType = CommandType.StoredProcedure
            cmd2.Connection = con
            cmd2.Transaction = transaction
            cmd2.CommandTimeout = 5000
            cmd2.Parameters.AddWithValue("@Trainingid", trainingid)
            cmd2.Parameters.AddWithValue("@Timetableid", timetableid)
            cmd2.Parameters.AddWithValue("@createdby", createdby)
            cmd2.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd2.Parameters.AddWithValue("@branchid", branchid)
            cmd2.Parameters.AddWithValue("@ttttt_session_id", sessionid)
            Dim sessionfac As String = ""
            Dim isjointsession As String = "0"
            If dtfaculy.Rows.Count > 1 Then
                isjointsession = "1"
            End If
            For i = 0 To dtfaculy.Rows.Count - 1
                sessionfac = sessionfac + dtfaculy.Rows(i)("id").ToString + ","
            Next

            If sessionfac <> "" Then
                cmd2.Parameters.AddWithValue("@ttttt_facultyid", sessionfac)
            Else
                cmd2.Parameters.AddWithValue("@ttttt_facultyid", DBNull.Value)
            End If
            'Code to calculate assignment duration and duration type
            Dim durationtype As String = ""
            Dim sessionstartdatetime As String = assignmentstartdate + " " + assignmentstarttime
            Dim sessionenddatetime As String = assignmentenddate + " " + assignmentendtime
            Dim date1 As DateTime = CDate(sessionstartdatetime)
            Dim date2 As DateTime = CDate(sessionenddatetime)

            'Dim difference As Int64 = DateDiff(DateInterval.Minute, date1, date2)
            'If difference <= 120 Then
            '    durationtype = "1"
            'ElseIf difference <= 480 Then
            '    durationtype = "2"
            'ElseIf difference > 480 Then
            '    durationtype = "3"
            'End If
            'assignmentduration = difference
            'As per task no 25 change done to send duration 0 in case of assignment
            durationtype = "1"
            assignmentduration = "0"


            cmd2.Parameters.AddWithValue("@ttttt_content_desc", assignmentname)
            cmd2.Parameters.AddWithValue("@ttttt_subject", assessmentquestion)
            cmd2.Parameters.AddWithValue("@ttttt_type", sessiontype)
            cmd2.Parameters.AddWithValue("@ttttt_session_dt", assignmentstartdate)
            cmd2.Parameters.AddWithValue("@ttttt_session_day", sessionday)
            cmd2.Parameters.AddWithValue("@ttttt_session_week", sessionweek)
            cmd2.Parameters.AddWithValue("@ttttt_session_duration", assignmentduration)
            cmd2.Parameters.AddWithValue("@ttttt_session_row_no", "1")
            cmd2.Parameters.AddWithValue("@ttttt_is_joint_session", isjointsession)
            cmd2.Parameters.AddWithValue("@ttttt_session_no", sessionno)

            cmd2.Parameters.AddWithValue("@ttttt_session_time", assignmentstarttime)
            cmd2.Parameters.AddWithValue("@ttttt_session_end_time", sessionenddatetime)
            cmd2.Parameters.AddWithValue("@ttttt_status", "0")
            cmd2.Parameters.AddWithValue("@ttttt_remark", "")
            cmd2.Parameters.AddWithValue("@TTTTT_SESSION_DURATION_TYPE", durationtype)
            cmd2.Parameters.AddWithValue("@tag", tag)
            cmd2.Parameters.AddWithValue("@iscomplimentory", iscomplementory)
            If sessionmodule <> "" Then
                cmd2.Parameters.AddWithValue("@ttttt_module_no", sessionmodule)
            End If

            cmd2.ExecuteNonQuery()


            ''Save Schedule Data

            Dim cmd3 As New SqlCommand
            cmd3 = New SqlCommand("[Assessment].[sp_insert_Schedule]", con)
            cmd3.CommandType = CommandType.StoredProcedure
            cmd3.Connection = con
            cmd3.Transaction = transaction
            cmd3.CommandTimeout = 5000
            cmd3.Parameters.AddWithValue("@p_AssignmentId", assignmentid)
            cmd3.Parameters.AddWithValue("@p_DeadlineType", isopeneded)
            cmd3.Parameters.AddWithValue("@p_SessionID", sessionid)
            cmd3.Parameters.AddWithValue("@p_trainingID", trainingid)
            cmd3.Parameters.AddWithValue("@p_createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd3.Parameters.AddWithValue("@p_createdby", createdby)

            cmd3.ExecuteNonQuery()


            ''Save DMS Data
            Dim docno As String = ""
            Dim cmd5 = New SqlCommand("Declare @UserCode Nvarchar(500) Select @UserCode= [DMS].[f_dms_doc_ref_no](getdate(),'" + branchid + "','123','$$','Year') select @UserCode as docno", con)
            cmd5.CommandType = CommandType.Text
            cmd5.CommandTimeout = 5000
            cmd5.Transaction = transaction
            cmd5.Connection = con
            docno = cmd5.ExecuteScalar()



            Dim cmd4 As New SqlCommand
            cmd4 = New SqlCommand("[DMS].[proc_dms_Ins_upd_doc_status]", con)
            cmd4.CommandType = CommandType.StoredProcedure
            cmd4.Connection = con
            cmd4.Transaction = transaction
            cmd4.CommandTimeout = 5000
            cmd4.Parameters.AddWithValue("@doc_no", docno)
            cmd4.Parameters.AddWithValue("@tttds_info_desc", DBNull.Value)
            cmd4.Parameters.AddWithValue("@doc_id", assignmentid)
            cmd4.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd4.Parameters.AddWithValue("@createdby", createdby)
            cmd4.Parameters.AddWithValue("@branchid", branchid)
            cmd4.Parameters.AddWithValue("@docremark", DBNull.Value)
            cmd4.Parameters.AddWithValue("@docdate", Get_CreatedOn_Server(Domain, IsOnline))
            cmd4.Parameters.AddWithValue("@actiondate", Get_CreatedOn_Server(Domain, IsOnline))
            cmd4.Parameters.AddWithValue("@CreatedBy_empid", createdempid)
            cmd4.Parameters.AddWithValue("@fwd_empid", fwdempid)
            cmd4.Parameters.AddWithValue("@tat_type_id", "123")
            cmd4.Parameters.AddWithValue("@doc_status", dmsstatus)
            cmd4.Parameters.AddWithValue("@attached_doc", DBNull.Value)
            cmd4.Parameters.AddWithValue("@attached_doc_name", DBNull.Value)
            cmd4.Parameters.AddWithValue("@doctype", "1")
            cmd4.Parameters.AddWithValue("@draftletter", DBNull.Value)
            cmd4.Parameters.AddWithValue("@tttds_is_final", "1")
            cmd4.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
            cmd4.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
            cmd4.Parameters.AddWithValue("@draftletterenc", DBNull.Value)

            cmd4.ExecuteNonQuery()


            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return True
    End Function

    Public Function Update_Assignment_Rollback(ByVal Domain As String, ByVal IsOnline As String,
                                                ByVal assignmentid As String, ByVal assignmentname As String, ByVal instructions As String, ByVal tag As String, ByVal assessmentquestion As String, ByVal faculty As String, ByVal attachments As String, ByVal assignmenttype As String, ByVal gradeapplicable As String, ByVal Grademarks As String, ByVal createdby As String, ByVal branchid As String, ByVal sessiontype As String, ByVal assignmentstartdate As String, ByVal assignmentenddate As String, ByVal assignmentstarttime As String, ByVal assignmentendtime As String, ByVal sessionday As String, ByVal sessionweek As String, ByVal assignmentduration As String, ByVal sessionno As String, ByVal timetableid As String, ByVal trainingid As String, ByVal isopeneded As String, ByVal dtfaculy As DataTable, ByVal createdempid As String, ByVal fwdempid As String, ByVal dmsstatus As String, ByVal assignmentno As String, ByVal sessionid As String, ByVal sessionstatus As String, ByVal MaxMarks As String, ByVal sessionmodule As String, Optional minmarks As String = Nothing, Optional QuestionMarks As String = Nothing, Optional iscomplementory As String = Nothing) As Boolean
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim transaction As SqlTransaction = con.BeginTransaction


        Try
            Dim cmd As New SqlCommand
            cmd = New SqlCommand("[Assessment].[sp_update_Assignment]", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = con
            cmd.Transaction = transaction
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@p_AssignmentID", assignmentid)
            cmd.Parameters.AddWithValue("@p_Instructions", instructions)
            cmd.Parameters.AddWithValue("@p_Tag", tag)
            cmd.Parameters.AddWithValue("@p_AssesmentQuestions", assessmentquestion)
            cmd.Parameters.AddWithValue("@p_FacultyID_Json", faculty)
            If Not attachments Is Nothing Then
                If attachments <> "" Then
                    cmd.Parameters.AddWithValue("@p_AttachmentsID_Json", attachments)
                Else
                    cmd.Parameters.AddWithValue("@p_AttachmentsID_Json", DBNull.Value)
                End If
            Else
                cmd.Parameters.AddWithValue("@p_AttachmentsID_Json", DBNull.Value)
            End If



            cmd.Parameters.AddWithValue("@p_AssignmentTypeID", assignmenttype)
            cmd.Parameters.AddWithValue("@p_GradeApplicable", gradeapplicable)
            cmd.Parameters.AddWithValue("@p_AssignmentName", assignmentname)
            cmd.Parameters.AddWithValue("@MaxMarks", MaxMarks)
            If (minmarks <> Nothing) Then
                cmd.Parameters.AddWithValue("@min_passing_marks", minmarks)
            End If

            If QuestionMarks Is Nothing Or QuestionMarks = "" Then
                cmd.Parameters.AddWithValue("@question_max_marks", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@question_max_marks", QuestionMarks)
            End If

            cmd.ExecuteNonQuery()



            ''Save Grade Data
            Dim cmd1 As New SqlCommand
            cmd1 = New SqlCommand("[Assessment].[sp_update_Grade]", con)
            cmd1.CommandType = CommandType.StoredProcedure
            cmd1.Connection = con
            cmd1.Transaction = transaction
            cmd1.CommandTimeout = 5000
            cmd1.Parameters.AddWithValue("@p_AssignmentID", assignmentid)
            If Grademarks <> "" And Not Grademarks Is Nothing Then
                cmd1.Parameters.AddWithValue("@p_Marks", Grademarks)
            Else
                cmd1.Parameters.AddWithValue("@p_Marks", DBNull.Value)
            End If

            cmd1.Parameters.AddWithValue("@p_GradeCritaria", DBNull.Value)
            cmd1.Parameters.AddWithValue("@p_RatingID", DBNull.Value)
            cmd1.ExecuteNonQuery()


            ''Save Session Data

            Dim cmd2 As New SqlCommand
            cmd2 = New SqlCommand("[TrainingPlan].[proc_tp_ins_upd_session]", con)
            cmd2.CommandType = CommandType.StoredProcedure
            cmd2.Connection = con
            cmd2.Transaction = transaction
            cmd2.CommandTimeout = 5000
            cmd2.Parameters.AddWithValue("@Trainingid", trainingid)
            cmd2.Parameters.AddWithValue("@Timetableid", timetableid)
            cmd2.Parameters.AddWithValue("@createdby", createdby)
            cmd2.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd2.Parameters.AddWithValue("@branchid", branchid)
            cmd2.Parameters.AddWithValue("@ttttt_session_id", sessionid)
            Dim sessionfac As String = ""
            Dim isjointsession As String = "0"
            If dtfaculy.Rows.Count > 1 Then
                isjointsession = "1"
            End If
            For i = 0 To dtfaculy.Rows.Count - 1
                sessionfac = sessionfac + dtfaculy.Rows(i)("id").ToString + ","
            Next

            If sessionfac <> "" Then
                cmd2.Parameters.AddWithValue("@ttttt_facultyid", sessionfac)
            Else
                cmd2.Parameters.AddWithValue("@ttttt_facultyid", DBNull.Value)
            End If
            'Code to calculate assignment duration and duration type
            Dim durationtype As String = ""
            Dim sessionstartdatetime As String = assignmentstartdate + " " + assignmentstarttime
            Dim sessionenddatetime As String = assignmentenddate + " " + assignmentendtime
            Dim date1 As DateTime = CDate(sessionstartdatetime)
            Dim date2 As DateTime = CDate(sessionenddatetime)
            'Dim difference As Int64 = DateDiff(DateInterval.Minute, date1, date2)
            'If difference <= 120 Then
            '    durationtype = "1"
            'ElseIf difference <= 480 Then
            '    durationtype = "2"
            'ElseIf difference > 480 Then
            '    durationtype = "3"
            'End If
            'assignmentduration = difference
            durationtype = "1"
            assignmentduration = "0"


            cmd2.Parameters.AddWithValue("@ttttt_content_desc", assessmentquestion)
            cmd2.Parameters.AddWithValue("@ttttt_subject", assessmentquestion)
            cmd2.Parameters.AddWithValue("@ttttt_type", sessiontype)
            cmd2.Parameters.AddWithValue("@ttttt_session_dt", assignmentstartdate)
            cmd2.Parameters.AddWithValue("@ttttt_session_day", sessionday)
            cmd2.Parameters.AddWithValue("@ttttt_session_week", sessionweek)
            cmd2.Parameters.AddWithValue("@ttttt_session_duration", assignmentduration)
            cmd2.Parameters.AddWithValue("@ttttt_session_row_no", "1")
            cmd2.Parameters.AddWithValue("@ttttt_is_joint_session", isjointsession)
            cmd2.Parameters.AddWithValue("@ttttt_session_no", sessionno)

            cmd2.Parameters.AddWithValue("@ttttt_session_time", assignmentstarttime)
            cmd2.Parameters.AddWithValue("@ttttt_session_end_time", sessionenddatetime)
            cmd2.Parameters.AddWithValue("@ttttt_status", sessionstatus)
            cmd2.Parameters.AddWithValue("@ttttt_remark", "")
            cmd2.Parameters.AddWithValue("@TTTTT_SESSION_DURATION_TYPE", durationtype)
            cmd2.Parameters.AddWithValue("@tag", tag)
            cmd2.Parameters.AddWithValue("@iscomplimentory", iscomplementory)
            If sessionmodule <> "" Then
                cmd2.Parameters.AddWithValue("@ttttt_module_no", sessionmodule)
            End If

            cmd2.ExecuteNonQuery()


            ''Save Schedule Data

            Dim cmd3 As New SqlCommand
            cmd3 = New SqlCommand("[Assessment].[sp_update_Schedule]", con)
            cmd3.CommandType = CommandType.StoredProcedure
            cmd3.Connection = con
            cmd3.Transaction = transaction
            cmd3.CommandTimeout = 5000
            cmd3.Parameters.AddWithValue("@p_AssignmentId", assignmentid)
            cmd3.Parameters.AddWithValue("@p_DeadlineType", isopeneded)
            cmd3.Parameters.AddWithValue("@p_SessionID", sessionid)
            cmd3.Parameters.AddWithValue("@p_trainingID", trainingid)

            cmd3.ExecuteNonQuery()


            ''Save DMS Data
            Dim docno As String = assignmentno

            Dim cmd4 As New SqlCommand
            cmd4 = New SqlCommand("[DMS].[proc_dms_Ins_upd_doc_status]", con)
            cmd4.CommandType = CommandType.StoredProcedure
            cmd4.Connection = con
            cmd4.Transaction = transaction
            cmd4.CommandTimeout = 5000
            cmd4.Parameters.AddWithValue("@doc_no", docno)
            cmd4.Parameters.AddWithValue("@tttds_info_desc", DBNull.Value)
            cmd4.Parameters.AddWithValue("@doc_id", assignmentid)
            cmd4.Parameters.AddWithValue("@createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd4.Parameters.AddWithValue("@createdby", createdby)
            cmd4.Parameters.AddWithValue("@branchid", branchid)
            cmd4.Parameters.AddWithValue("@docremark", DBNull.Value)
            cmd4.Parameters.AddWithValue("@docdate", Get_CreatedOn_Server(Domain, IsOnline))
            cmd4.Parameters.AddWithValue("@actiondate", Get_CreatedOn_Server(Domain, IsOnline))
            cmd4.Parameters.AddWithValue("@CreatedBy_empid", createdempid)
            cmd4.Parameters.AddWithValue("@fwd_empid", fwdempid)
            cmd4.Parameters.AddWithValue("@tat_type_id", "123")
            cmd4.Parameters.AddWithValue("@doc_status", dmsstatus)
            cmd4.Parameters.AddWithValue("@attached_doc", DBNull.Value)
            cmd4.Parameters.AddWithValue("@attached_doc_name", DBNull.Value)
            cmd4.Parameters.AddWithValue("@doctype", "1")
            cmd4.Parameters.AddWithValue("@draftletter", DBNull.Value)
            cmd4.Parameters.AddWithValue("@tttds_is_final", "1")
            cmd4.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
            cmd4.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
            cmd4.Parameters.AddWithValue("@draftletterenc", DBNull.Value)

            cmd4.ExecuteNonQuery()


            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return True
    End Function
    Public Function Get_assignment_list(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String) As DataSet
        Dim dscode As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        ' cmd = New SqlCommand("select distinct trainingcode from trainingplan.Vw_tp_trg_time_table where ttttt_timetableid='" + timetableid + "'", con)
        cmd = New SqlCommand("Assessment.proc_get_assignment_list_data", con)
        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dscode)
        con.Close()
        Return dscode
    End Function
    Public Function GET_SURVEY_TAKER_DETAILS(ByVal Domain As String, ByVal IsOnline As String, ByVal mobileno As String) As DataSet
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[Survey360].[proc_get_survey_taker_details]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@mobileno", mobileno)


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function

    Public Function Get_Received_Amt(ByVal Domain As String, ByVal isonline As String) As DataTable
        Dim dtamt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        cmd = New SqlCommand("Select * from TrainingPlan.[VW_TotalFees_byReceiptID]", con)
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 5000
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dtamt)
        con.Close()
        Return dtamt
    End Function
    Public Function Get_Payment_Details(ByVal Domain As String, ByVal IsOnline As String, ByVal trainingid As String, ByVal participantid As String, ByVal fromdate As String, ByVal todate As String, ByVal usertype As String, ByVal loginuserid As String, ByVal pageno As String, ByVal pagesize As String, ByVal searchcolumn As String, ByVal searchcolumnvalue As String, ByVal sortcolumn As String, ByVal sortorder As String, ByVal filtername As String, ByVal filtervalue As String) As DataTable
        Dim dsuserdetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[TrainingPlan].[TP_GetPaymentForParticipant]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not participantid Is Nothing And participantid <> "" Then
            cmd.Parameters.AddWithValue("@ParticipantId", participantid)
        End If
        If Not fromdate Is Nothing And fromdate <> "" Then
            cmd.Parameters.AddWithValue("@fromdate", fromdate)
        End If
        If Not todate Is Nothing And todate <> "" Then
            cmd.Parameters.AddWithValue("@todate", todate)
        End If
        If Not usertype Is Nothing And usertype <> "" Then
            cmd.Parameters.AddWithValue("@Usertype", usertype)
        End If
        If Not loginuserid Is Nothing And loginuserid <> "" Then
            cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        End If
        If Not pageno Is Nothing And pageno <> "" Then
            cmd.Parameters.AddWithValue("@PageNo", pageno)
        End If
        If Not pagesize Is Nothing And pagesize <> "" Then
            cmd.Parameters.AddWithValue("@PageSize", pagesize)
        End If
        If Not searchcolumn Is Nothing And searchcolumn <> "" Then
            cmd.Parameters.AddWithValue("@SearchColumn", searchcolumn)
        End If
        If Not searchcolumnvalue Is Nothing And searchcolumnvalue <> "" Then
            cmd.Parameters.AddWithValue("@SearchValue", searchcolumnvalue)
        End If
        If Not sortcolumn Is Nothing And sortcolumn <> "" Then
            cmd.Parameters.AddWithValue("@SortColumn", sortcolumn)
        End If
        If Not sortorder Is Nothing And sortorder <> "" Then
            cmd.Parameters.AddWithValue("@SortOrder", sortorder)
        End If
        If Not filtername Is Nothing And filtername <> "" Then
            cmd.Parameters.AddWithValue("@filtername", filtername)
        End If
        If Not filtervalue Is Nothing And filtervalue <> "" Then
            cmd.Parameters.AddWithValue("@filtervalue", filtervalue)
        End If
        If Not trainingid Is Nothing And trainingid <> "" Then
            cmd.Parameters.AddWithValue("@trainingid", trainingid)
        End If



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)
        con.Close()
        Return dsuserdetails
    End Function

    Public Function GET_ADJUSTMENT_DETAILS(ByVal Domain As String, ByVal isonline As String, ByVal receiptid As String, ByVal fromdate As String, ByVal todate As String, ByVal usertype As String, ByVal loginuserid As String, ByVal pageno As String, ByVal pagesize As String, ByVal searchcol As String, ByVal searchval As String, ByVal searchorder As String, ByVal filtername As String, ByVal filterval As String, ByVal trainingid As String) As DataTable

        Dim dtTrainingCalender As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_receipt_adjustment", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If Not receiptid Is Nothing Then
            cmd.Parameters.AddWithValue("@ReceiptId", receiptid)
        Else
            cmd.Parameters.AddWithValue("@ReceiptId", DBNull.Value)
        End If
        If Not fromdate Is Nothing Then
            cmd.Parameters.AddWithValue("@fromdate", fromdate)
        Else
            cmd.Parameters.AddWithValue("@fromdate", DBNull.Value)
        End If
        If Not todate Is Nothing Then
            cmd.Parameters.AddWithValue("@todate", todate)
        Else
            cmd.Parameters.AddWithValue("@todate", DBNull.Value)
        End If

        If Not usertype Is Nothing Then
            cmd.Parameters.AddWithValue("@Usertype", usertype)
        Else
            cmd.Parameters.AddWithValue("@Usertype", DBNull.Value)
        End If
        If Not loginuserid Is Nothing Then
            cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        Else
            cmd.Parameters.AddWithValue("@loginuserid", DBNull.Value)
        End If
        If Not pageno Is Nothing Then
            cmd.Parameters.AddWithValue("@PageNo", pageno)
        Else
            cmd.Parameters.AddWithValue("@PageNo", pageno)
        End If
        If Not pagesize Is Nothing Then
            cmd.Parameters.AddWithValue("@PageSize", pagesize)
        Else
            cmd.Parameters.AddWithValue("@PageSize", pagesize)
        End If
        If Not searchcol Is Nothing Then
            cmd.Parameters.AddWithValue("@SearchColumn", searchcol)
        Else
            cmd.Parameters.AddWithValue("@SearchColumn", searchcol)
        End If
        If Not searchval Is Nothing Then
            cmd.Parameters.AddWithValue("@SearchValue", searchval)
        Else
            cmd.Parameters.AddWithValue("@SearchValue", searchval)
        End If
        If Not searchorder Is Nothing Then
            cmd.Parameters.AddWithValue("@SortOrder", searchorder)
        Else
            cmd.Parameters.AddWithValue("@SortOrder", searchorder)
        End If
        If Not filtername Is Nothing Then
            cmd.Parameters.AddWithValue("@filtername", filtername)
        Else
            cmd.Parameters.AddWithValue("@filtername", filtername)
        End If
        If Not filterval Is Nothing Then
            cmd.Parameters.AddWithValue("@filtervalue", filterval)
        Else
            cmd.Parameters.AddWithValue("@filtervalue", filterval)
        End If
        If Not trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@trainingid", trainingid)
        Else
            cmd.Parameters.AddWithValue("@trainingid", trainingid)
        End If






        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtTrainingCalender)
        con.Close()
        Return dtTrainingCalender
    End Function

    Public Function GET_CD_PROG_GRAPH_DATA(ByVal Domain As String, ByVal isonline As String, ByVal fromdate As String, ByVal todate As String, ByVal loginuserid As String, ByVal usertype As String, ByVal orderby As String, ByVal procedurefor As String, ByVal duration As String) As DataSet

        Dim dsTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.proc_tp_get_burn_graph", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@fromdate", fromdate)
        cmd.Parameters.AddWithValue("@todate", todate)
        cmd.Parameters.AddWithValue("@loginuserid", loginuserid)
        cmd.Parameters.AddWithValue("@Usertype", usertype)
        'cmd.Parameters.AddWithValue("@orderby", orderby)
        cmd.Parameters.AddWithValue("@Procedurefor", procedurefor)
        'cmd.Parameters.AddWithValue("@duration", duration)



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsTrainingCalender)
        con.Close()
        Return dsTrainingCalender
    End Function


    Public Function Get_Mail_Setting(Optional settingforotp As Int16 = 0) As EmailConfiguration
        ' Dim es As New EmailConfiguration

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/emailsetting?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of EmailConfiguration)()

        Dim EC As New EmailConfiguration

        Dim xmldoc As New XmlDocument
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/emailSetting.xml"))
        Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/EmailConfiguration")
        For Each node As XmlNode In Nodes
            For Each node1 As XmlNode In node.ChildNodes
                If node1.Name = "clienturl" Then
                    EC.clienturl = node1.InnerText
                End If
                If node1.Name = "clientname" Then
                    EC.clientname = node1.InnerText
                End If
                'If node1.Name = "login" Then
                '    EC.login = node1.InnerText
                'End If
                'If node1.Name = "password" Then
                '    EC.password = node1.InnerText
                'End If
                'If node1.Name = "portno" Then
                '    EC.portno = node1.InnerText
                'End If
                'If node1.Name = "host" Then
                '    EC.host = node1.InnerText
                'End If
                If node1.Name = "header" Then
                    EC.header = node1.InnerText
                End If
                If node1.Name = "footer" Then
                    EC.footer = node1.InnerText
                End If
            Next


        Next
        'New Code to get setting from Application Setting
        If settingforotp = 1 Then
            Dim otpsetting As ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING

            Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
            Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
            Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=6")
            Dim response = request.GetResponse()
            Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
            Dim j As JObject = JObject.Parse(responseString)
            otpsetting = j.ToObject(Of ApplicationSetting.OTP_LOGIN_REQUIRED_SETTING)()

            EC.host = otpsetting.EMAILSETTING.HOST
            EC.login = otpsetting.EMAILSETTING.EMAILID
            EC.password = otpsetting.EMAILSETTING.PWD
            EC.portno = otpsetting.EMAILSETTING.PORT



        Else
            Dim otpsetting As ApplicationSetting.EMAIL_SEND_BY_APPLICATION

            Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
            Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
            Dim request = WebRequest.Create(BaseUrl + "api/ApplicationSettting?APIKEY=" + Littera_APIKEY + "&settingtype=7")
            Dim response = request.GetResponse()
            Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
            Dim j As JObject = JObject.Parse(responseString)
            otpsetting = j.ToObject(Of ApplicationSetting.EMAIL_SEND_BY_APPLICATION)()

            EC.host = otpsetting.EMAILSETTING.HOST
            EC.login = otpsetting.EMAILSETTING.EMAILID
            EC.password = otpsetting.EMAILSETTING.PWD
            EC.portno = otpsetting.EMAILSETTING.PORT
        End If






        Return EC
    End Function


    Public Function Get_Global_XML_Setting() As GlobalSetting
        Dim setting As New GlobalSetting

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/GlobalSetting?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of GlobalSetting)()

        Dim dtsetting As New DataTable
        con = Get_Connection_String("YojnaAcademy", "1")
        con.Open()
        cmd = New SqlCommand("Trainingplan.proc_get_tbl_tp_Global_Setting", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        'cmd.Parameters.AddWithValue("@ttgs_key_name", fromdate)




        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtsetting)
        con.Close()


        For Each dr As DataRow In dtsetting.Rows
            If dr("ttgs_key_name") = "share_content_on_google_drive" Then
                setting.share_content_on_google_drive = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "Admin_Mail_Id" Then
                setting.Admin_Mail_Id = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "IS_MAIL_REQUIRED" Then
                setting.IS_MAIL_REQUIRED = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "REGISTRATION_COMPULSORY_FIELD" Then
                setting.REGISTRATION_COMPULSORY_FIELD = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "IS_SMS_REQUIRED" Then
                setting.IS_SMS_REQUIRED = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "IS_PAID_TRG_REQUIRED" Then
                setting.IS_PAID_TRG_REQUIRED = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "MONTH_DAYS" Then
                setting.MONTH_DAYS = dr("ttgs_key_value")
            End If
            If dr("ttgs_key_name") = "WEEK_DAYS" Then
                setting.WEEK_DAYS = dr("ttgs_key_value")
            End If
        Next



        'Dim xmldoc As New XmlDocument
        'Dim Foldername As String = GET_SETTING_XML_FOLDER()
        'xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/GlobalSetting.xml"))
        'Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/GlobalSetting/setting")
        'For Each node As XmlNode In Nodes
        '    If node.SelectSingleNode("key").InnerText = "share_content_on_google_drive" Then
        '        setting.share_content_on_google_drive = node.SelectSingleNode("value").InnerText
        '    End If
        '    If node.SelectSingleNode("key").InnerText = "Admin_Mail_Id" Then
        '        setting.Admin_Mail_Id = node.SelectSingleNode("value").InnerText
        '    End If
        '    If node.SelectSingleNode("key").InnerText = "IS_MAIL_REQUIRED" Then
        '        setting.IS_MAIL_REQUIRED = node.SelectSingleNode("value").InnerText
        '    End If
        '    If node.SelectSingleNode("key").InnerText = "REGISTRATION_COMPULSORY_FIELD" Then
        '        setting.REGISTRATION_COMPULSORY_FIELD = node.SelectSingleNode("value").InnerText
        '    End If
        '    If node.SelectSingleNode("key").InnerText = "IS_SMS_REQUIRED" Then
        '        setting.IS_SMS_REQUIRED = node.SelectSingleNode("value").InnerText
        '    End If
        '    If node.SelectSingleNode("key").InnerText = "IS_PAID_TRG_REQUIRED" Then
        '        setting.IS_PAID_TRG_REQUIRED = node.SelectSingleNode("value").InnerText
        '    End If
        'Next



        Return setting
    End Function

    Public Function Get_Payment_Setting() As GatewayDetails
        Dim GD As New GatewayDetails

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/GatewayDetail?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of GatewayDetails)()

        Dim xmldoc As New XmlDocument
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/GatewayDetail.xml"))
        Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/PaymentConfiguration")
        For Each node As XmlNode In Nodes
            For Each node1 As XmlNode In node.ChildNodes
                If node1.Name = "img" Then
                    GD.img = node1.InnerText
                End If
                If node1.Name = "key" Then
                    GD.key = node1.InnerText
                End If
                If node1.Name = "secret" Then
                    GD.secret = node1.InnerText
                End If

            Next

        Next

        Return GD
    End Function
    Public Function Get_SMS_Setting() As SMSSetting
        Dim ss As New SMSSetting

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/SMSSetting?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of SMSSetting)()


        Dim xmldoc As New XmlDocument
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/SMSSetting.xml"))
        Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/SMSConfiguration")
        For Each node As XmlNode In Nodes
            For Each node1 As XmlNode In node.ChildNodes
                If node1.Name = "url" Then
                    ss.url = node1.InnerText
                End If
                If node1.Name = "key" Then
                    ss.key = node1.InnerText
                End If
                If node1.Name = "routeid" Then
                    ss.routeid = node1.InnerText
                End If
                If node1.Name = "SENDER_ID" Then
                    ss.SENDER_ID = node1.InnerText
                End If
                If node1.Name = "PEID" Then
                    ss.PEID = node1.InnerText
                End If
                If node1.Name = "USERID" Then
                    ss.userid = node1.InnerText
                End If
                If node1.Name = "PASSWORD" Then
                    ss.password = node1.InnerText
                End If
            Next
        Next

        Return ss
    End Function

    Public Function Get_EMAIL_TEMPLATE(ByVal id As String) As EmailTemplate
        Dim es As New List(Of EmailTemplate)

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/emailTemplate?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of List(Of EmailTemplate))()
        'es = es.Where(Function(o) o.ID.ToString().ToUpper() = id.ToString().ToUpper())

        Dim xmldoc As New XmlDocument
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/emailTemplate.xml"))
        Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/EmailTemplate/Template")
        For Each node As XmlNode In Nodes
            Dim temp As New EmailTemplate
            For Each node1 As XmlNode In node.ChildNodes
                If node1.Name = "ID" Then
                    temp.ID = node1.InnerText
                End If
                If node1.Name = "subject" Then
                    temp.subject = node1.InnerText
                End If
                If node1.Name = "text" Then
                    temp.text = node1.InnerText
                End If

            Next
            es.Add(temp)
        Next

        es = es.Where(Function(o) o.ID.ToString().ToUpper() = id.ToString().ToUpper()).ToList()


        Return es.FirstOrDefault()
    End Function
    Public Function Get_SMS_TEMPLATE(ByVal id As String) As SMSTemplate
        Dim es As New List(Of SMSTemplate)

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/SMSSetting?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of List(Of SMSTemplate))()
        'es = es.Where(Function(o) o.ID.ToString().ToUpper() = id.ToString().ToUpper())

        Dim xmldoc As New XmlDocument
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/SMSTemplate.xml"))
        Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/SMSTemplate/Template")
        For Each node As XmlNode In Nodes
            Dim temp As New SMSTemplate
            For Each node1 As XmlNode In node.ChildNodes

                If node1.Name = "ID" Then

                    temp.ID = node1.InnerText
                End If
                If node1.Name = "DLT_CT_ID" Then
                    temp.DLT_CT_ID = node1.InnerText
                End If
                If node1.Name = "text" Then
                    temp.text = node1.InnerText
                End If
            Next
            es.Add(temp)
        Next
        es = es.Where(Function(o) o.ID.ToString().ToUpper() = id.ToString().ToUpper()).ToList()

        Return es.FirstOrDefault()
    End Function

    Public Function Get_Client_Data() As Login
        Dim l As New Login

        'Dim BaseUrl As String = ConfigurationManager.AppSettings("API").ToString()
        'Dim Littera_APIKEY As String = ConfigurationManager.AppSettings("APIKEY").ToString()
        'Dim request = WebRequest.Create(BaseUrl + "api/SMSSetting?APIKEY=" + Littera_APIKEY + "")
        'Dim response = request.GetResponse()
        'Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
        'Dim j As JObject = JObject.Parse(responseString)
        'es = j.ToObject(Of List(Of SMSTemplate))()
        'es = es.Where(Function(o) o.ID.ToString().ToUpper() = id.ToString().ToUpper())

        Dim xmldoc As New XmlDocument
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/ClientData.xml"))
        Dim Nodes As XmlNodeList = xmldoc.DocumentElement.SelectNodes("/ClientData/setting")

        Dim cinfo As New Login.cinfo

        For Each node As XmlNode In Nodes
            If node.SelectSingleNode("key").InnerText = "LOGIN_LOGO" Then
                cinfo.logo = node.SelectSingleNode("value").InnerText
                cinfo.LOGIN_LOGO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CERTIFICATE_TXT" Then
                cinfo.CERTIFICATE_TXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_FIRST_BOX_HEADER" Then
                cinfo.ACHIEVEMENT_FIRST_BOX_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_FIRST_BOX_VALUE" Then
                cinfo.ACHIEVEMENT_FIRST_BOX_VALUE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_SECOND_BOX_HEADER" Then
                cinfo.ACHIEVEMENT_SECOND_BOX_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_SECOND_BOX_VALUE" Then
                cinfo.ACHIEVEMENT_SECOND_BOX_VALUE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_THIRD_BOX_HEADER" Then
                cinfo.ACHIEVEMENT_THIRD_BOX_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_THIRD_BOX_VALUE" Then
                cinfo.ACHIEVEMENT_THIRD_BOX_VALUE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_FOURTH_BOX_HEADER" Then
                cinfo.ACHIEVEMENT_FOURTH_BOX_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHIEVEMENT_FOURTH_BOX_VALUE" Then
                cinfo.ACHIEVEMENT_FOURTH_BOX_VALUE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "QR_IMG" Then
                cinfo.QR_IMG = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "FB_LINK" Then
                cinfo.FB_LINK = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "TWT_LINK" Then
                cinfo.TWT_LINK = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "LINKEDIN_LINK" Then
                cinfo.LINKEDIN_LINK = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "SLIDER_1_IMG" Then
                cinfo.SLIDER_1_IMG = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "SLIDER_2_IMG" Then
                cinfo.SLIDER_2_IMG = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "SLIDER_3_IMG" Then
                cinfo.SLIDER_3_IMG = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "WELCOME_TXT_HEADER" Then
                cinfo.WELCOME_TXT_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "WELCOME_TXT_DETAILS" Then
                cinfo.WELCOME_TXT_DETAILS = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CERTIFIED_FACULTY_TEXT" Then
                cinfo.CERTIFIED_FACULTY_TEXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CERTIFICATION_TEXT" Then
                cinfo.CERTIFICATION_TEXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_VIDEO" Then
                cinfo.ABT_US_VIDEO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_VIDEO_IMG" Then
                cinfo.ABT_US_VIDEO_IMG = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_MAIN_TXT" Then
                cinfo.ABT_US_MAIN_TXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_WELCOME_HEADER" Then
                cinfo.ABT_US_WELCOME_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_WELCOME_DETAIL" Then
                cinfo.ABT_US_WELCOME_DETAIL = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_CONCEPT_HEADER" Then
                cinfo.ABT_US_CONCEPT_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_CONCEPT_DETAIL" Then
                cinfo.ABT_US_CONCEPT_DETAIL = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_MISSION_HEADER" Then
                cinfo.ABT_US_MISSION_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_MISSION_DETAILS" Then
                cinfo.ABT_US_MISSION_DETAILS = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_VISION_HEADER" Then
                cinfo.ABT_US_VISION_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ABT_US_VISION_DETAIL" Then
                cinfo.ABT_US_VISION_DETAIL = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHEIVEMENT_TEXT" Then
                cinfo.ACHEIVEMENT_TEXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CONTACTUS_ADDRESS" Then
                cinfo.CONTACTUS_ADDRESS = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CONTACTUS_PHONENO" Then
                cinfo.CONTACTUS_PHONENO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "ACHEIVEMENT_IMG" Then
                cinfo.ACHEIVEMENT_IMG = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CONTACTUS_EMAIL" Then
                cinfo.CONTACTUS_EMAIL = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "READ_MORE_PAGE_HEADER" Then
                cinfo.READ_MORE_PAGE_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "READ_MORE_PAGE_DETAILS" Then
                cinfo.READ_MORE_PAGE_DETAILS = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "READ_MORE_PAGE_VIDEO" Then
                cinfo.READ_MORE_PAGE_VIDEO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "READ_MORE_PAGE_GOALS" Then
                cinfo.READ_MORE_PAGE_GOALS = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "PRIVACY_PAGE_HEADER" Then
                cinfo.PRIVACY_PAGE_HEADER = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "PRIVACY_PAGE_CONTENT" Then
                cinfo.PRIVACY_PAGE_CONTENT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "SLIDER_READ_MORE_DISPLAY" Then
                cinfo.SLIDER_READ_MORE_DISPLAY = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "LITTERA_SITE_TAGLINE" Then
                cinfo.LITTERA_SITE_TAGLINE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "LITTERA_LOGO" Then
                cinfo.LITTERA_LOGO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "DASHBOARD_LOGO" Then
                cinfo.DASHBOARD_LOGO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "APP_TITLE" Then
                cinfo.APP_TITLE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "APP_TITLE" Then
                cinfo.title = node.SelectSingleNode("value").InnerText
                cinfo.APP_TITLE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "FAV_ICON" Then
                cinfo.FAV_ICON = node.SelectSingleNode("value").InnerText
                cinfo.icon = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "TRENDING_TRG_Header" Then
                cinfo.TRENDING_TRG_Header = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "TRG_CALENDAR_TEXT" Then
                cinfo.TRG_CALENDAR_TEXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "TRENDING_TRG_TEXT" Then
                cinfo.TRENDING_TRG_TEXT = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "PLATFORM_NAME" Then
                cinfo.PLATFORM_NAME = node.SelectSingleNode("value").InnerText

            ElseIf node.SelectSingleNode("key").InnerText = "APP_LOGIN_LOGO" Then
                cinfo.APP_LOGIN_LOGO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "APP_OTP_LOGO" Then
                cinfo.APP_OTP_LOGO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "APP_LOGIN_TITLE" Then
                cinfo.APP_LOGIN_TITLE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "APP_OTP_TITLE" Then
                cinfo.APP_OTP_TITLE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "APP_DASHBOARD_TITLE" Then
                cinfo.APP_DASHBOARD_TITLE = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "TRENDING_CALENDAR_Header" Then
                cinfo.TRENDING_CALENDAR_Header = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CERTIFIED_FAC_Header" Then
                cinfo.CERTIFIED_FAC_Header = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CERTIFICATION_Header" Then
                cinfo.CERTIFICATION_Header = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "CERTIFICATE_LOGO" Then
                cinfo.CERTIFICATE_LOGO = node.SelectSingleNode("value").InnerText
            ElseIf node.SelectSingleNode("key").InnerText = "PLAY_STORE_LINK" Then
                cinfo.PLAY_STORE_LINK = node.SelectSingleNode("value").InnerText
            End If

        Next
        l.Clientinfo = cinfo

        Return l
    End Function
    Public Function GET_SETTING_XML_FOLDER() As String
        Return "Content/GlobalSetting/"
    End Function

    Public Function Get_Training_Trg_Types(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String) As DataTable

        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("select TrainingId,trg_type from TrainingPlan.VW_Training_calendar where TrainingId='" + trainingid + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dt)
        con.Close()
        Return dt
    End Function
    Public Function Get_Training_Trg_Types_By_Code(ByVal Domain As String, ByVal isonline As String, ByVal code As String) As DataTable

        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("select TrainingId,trg_type from TrainingPlan.VW_Training_calendar where Trainingcode='" + code + "'", con)
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dt)
        con.Close()
        Return dt
    End Function

    Public Function Update_Session_Status(ByVal Domain As String, ByVal IsOnline As String, ByVal Participantid As String, ByVal SessionId As String, ByVal Onscreentime As String, ByVal status As String, ByVal trainingid As String, ByVal Branchid As String) As Boolean

        Dim dt As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("Trainingplan.proc_update_participant_session_status", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@ttpss_participant_id", Participantid)
        cmd.Parameters.AddWithValue("@ttpss_session_id", SessionId)
        cmd.Parameters.AddWithValue("@ttpss_onscreen_time", Onscreentime)
        cmd.Parameters.AddWithValue("@ttpss_status", status)
        cmd.Parameters.AddWithValue("@trainingid", trainingid)
        cmd.Parameters.AddWithValue("@BranchId", Branchid)
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    Public Function Get_CAST(ByVal Domain As String, ByVal isonline As String) As DataTable

        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("yuser.proc_get_person_cast", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dt)
        con.Close()
        Return dt
    End Function


    Public Function Update_Proposal_on_trg(ByVal Domain As String, ByVal isonline As String, ByVal proposalid As String, ByVal proposaldetailid As String, ByVal trainingid As String, ByVal trainincode As String, ByVal createdby As String) As Boolean


        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_update_trg_proposal_code", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@ttp_id", proposalid)
        cmd.Parameters.AddWithValue("@ttpd_id", proposaldetailid)
        cmd.Parameters.AddWithValue("@ttpc_trg_id", trainingid)
        cmd.Parameters.AddWithValue("@ttpc_created_by", createdby)
        If trainincode.ToString() <> "" Then
            cmd.Parameters.AddWithValue("@code", trainincode)
        End If

        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.ExecuteNonQuery()
        con.Close()
        Return True
    End Function

    'Public Function Get_User_Detail(ByVal Domain As String, ByVal isonline As String, ByVal userid As String) As DataTable
    '    Dim dtFacultyreg As New DataTable
    '    con = Get_Connection_String(Domain, isonline)
    '    con.Open()
    '    cmd = New SqlCommand("[YUSER].[proc_yuser_get_user_detail]", con)
    '    cmd.CommandType = CommandType.StoredProcedure
    '    cmd.CommandTimeout = 5000
    '    cmd.Parameters.AddWithValue("@userid", userid)

    '    Dim da As New SqlDataAdapter(cmd)
    '    da.Fill(dtFacultyreg)
    '    con.Close()
    '    Return dtFacultyreg
    'End Function

    Public Function Save_Payment_Order_Receipt(ByVal Domain As String, ByVal IsOnline As String,
                                              ByVal receiptid As String, ByVal receiptdate As String, ByVal payeeid As String, ByVal createdby As String, ByVal branchid As String, ByVal paymentxml As String, ByVal rec_remark As String, ByVal receipttype As String, ByVal schemeid As String, ByVal receiptno As String, ByVal trainingid As String, ByVal litteraorderid As String,
                                              ByVal docremark As String, ByVal docdate As String, ByVal createdbyempid As String, ByVal fwdempid As String, ByVal rec_tat_typeid As String, ByVal rec_docstatus As String, ByVal rec_procedurefor As String, ByVal rec_uploaded_doc As String, ByVal rec_uploaded_doc_name As String, ByVal rec_doctype As String, ByVal rec_draftletter As String, ByVal rec_is_final As String,
                                        ByVal rec_doc_lettertype As String, ByVal rec_docremarkenc As String, ByVal rec_draftletterenc As String, ByVal littera_tat_typeid As String, ByVal littera_status As String, ByVal littera_procedurefor As String, ByVal littera_uploaded_doc As String, ByVal littera_uploaded_doc_name As String, ByVal littera_doctype As String, ByVal littera_draftletter As String, ByVal littera_is_final As String, ByVal littera_doc_lettertype As String,
                                         ByVal littera_docremarkenc As String, ByVal littera_draftletterenc As String) As Boolean
        Dim dtTrainingCalender As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        Dim transaction As SqlTransaction = con.BeginTransaction


        Try
            Dim cmd As New SqlCommand
            cmd = New SqlCommand("[TrainingPlan].[proc_tp_save_receipt]", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = con
            cmd.Transaction = transaction
            cmd.CommandTimeout = 5000
            cmd.Parameters.AddWithValue("@ReceiptId", receiptid)
            cmd.Parameters.AddWithValue("@ReceiptDate", receiptdate)
            cmd.Parameters.AddWithValue("@SponsorId", payeeid)
            cmd.Parameters.AddWithValue("@Createdby", createdby)
            cmd.Parameters.AddWithValue("@Branchid", branchid)
            cmd.Parameters.AddWithValue("@Paymentxml", paymentxml)
            If rec_remark <> "" And rec_remark <> "NULL" Then
                cmd.Parameters.AddWithValue("@Remark", rec_remark)
            Else
                cmd.Parameters.AddWithValue("@Remark", DBNull.Value)
            End If
            If receipttype <> "" And receipttype <> "NULL" Then
                cmd.Parameters.AddWithValue("@Receipttype", receipttype)
            Else
                cmd.Parameters.AddWithValue("@Receipttype", DBNull.Value)
            End If

            cmd.Parameters.AddWithValue("@Schemeid", schemeid)
            cmd.Parameters.AddWithValue("@Receiptno_auto", receiptno)
            cmd.Parameters.AddWithValue("@trainingid", trainingid)
            cmd.Parameters.AddWithValue("@paymentorderid", litteraorderid)
            cmd.Parameters.AddWithValue("@docremark", docremark)
            cmd.Parameters.AddWithValue("@docdate", docdate)
            cmd.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            cmd.Parameters.AddWithValue("@fwd_empid", fwdempid)

            cmd.Parameters.AddWithValue("@tat_type_id", rec_tat_typeid)
            cmd.Parameters.AddWithValue("@doc_status", rec_docstatus)
            cmd.Parameters.AddWithValue("@procedurefor", rec_procedurefor)
            cmd.Parameters.AddWithValue("@uploaded_doc", rec_uploaded_doc)
            If rec_uploaded_doc_name <> "" And rec_uploaded_doc_name <> "NULL" Then
                cmd.Parameters.AddWithValue("@uploaded_doc_name", rec_uploaded_doc_name)
            Else
                cmd.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
            End If
            If rec_doctype <> "" And rec_doctype <> "NULL" Then
                cmd.Parameters.AddWithValue("@doctype", rec_doctype)
            Else
                cmd.Parameters.AddWithValue("@doctype", DBNull.Value)
            End If
            If rec_draftletter <> "" And rec_draftletter <> "NULL" Then
                cmd.Parameters.AddWithValue("@draftletter", rec_draftletter)
            Else
                cmd.Parameters.AddWithValue("@draftletter", DBNull.Value)
            End If
            If rec_draftletter <> "" And rec_is_final <> "NULL" Then
                cmd.Parameters.AddWithValue("@tttds_is_final", rec_is_final)
            Else
                cmd.Parameters.AddWithValue("@tttds_is_final", DBNull.Value)
            End If
            If rec_doc_lettertype <> "" And rec_doc_lettertype <> "NULL" Then
                cmd.Parameters.AddWithValue("@tttds_letter_type", rec_doc_lettertype)
            Else
                cmd.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
            End If
            If rec_docremarkenc <> "" And rec_docremarkenc <> "NULL" Then
                cmd.Parameters.AddWithValue("@docremarkenc", rec_docremarkenc)
            Else
                cmd.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
            End If

            If rec_draftletterenc <> "" And rec_draftletterenc <> "NULL" Then
                cmd.Parameters.AddWithValue("@draftletterenc", rec_draftletterenc)
            Else
                cmd.Parameters.AddWithValue("@draftletterenc", DBNull.Value)
            End If

            cmd.ExecuteNonQuery()



            'Litter order entry
            Dim cmd1 As New SqlCommand
            cmd1 = New SqlCommand("[TrainingPlan].[sp_insert_litteraorder]", con)
            cmd1.CommandType = CommandType.StoredProcedure
            cmd1.Connection = con
            cmd1.Transaction = transaction
            cmd1.CommandTimeout = 5000
            cmd1.Parameters.AddWithValue("@p_LitteraOrderID", litteraorderid)
            cmd1.Parameters.AddWithValue("@p_LitteraOrderDateandtime", Get_CreatedOn_Server(Domain, IsOnline))
            cmd1.Parameters.AddWithValue("@p_ParticipantId", payeeid)
            cmd1.Parameters.AddWithValue("@p_Createdby", createdby)
            cmd1.Parameters.AddWithValue("@p_createdon", Get_CreatedOn_Server(Domain, IsOnline))
            cmd1.Parameters.AddWithValue("@p_Branchid", branchid)
            cmd1.Parameters.AddWithValue("@p_Schemeid", schemeid)
            cmd1.Parameters.AddWithValue("@p_Razorpay_order_ID", receiptno)
            cmd1.Parameters.AddWithValue("@p_trainingid", trainingid)
            cmd1.Parameters.AddWithValue("@p_PAYMENTXML", paymentxml)
            cmd1.Parameters.AddWithValue("@docremark", docremark)
            cmd1.Parameters.AddWithValue("@docdate", docdate)
            cmd1.Parameters.AddWithValue("@CreatedBy_empid", createdbyempid)
            cmd1.Parameters.AddWithValue("@fwd_empid", fwdempid)
            cmd1.Parameters.AddWithValue("@tat_type_id", littera_tat_typeid)
            cmd1.Parameters.AddWithValue("@doc_status", littera_status)

            cmd1.Parameters.AddWithValue("@procedurefor", littera_procedurefor)
            If littera_uploaded_doc <> "" And littera_uploaded_doc <> "NULL" Then
                cmd1.Parameters.AddWithValue("@uploaded_doc", littera_uploaded_doc)
            Else
                cmd1.Parameters.AddWithValue("@uploaded_doc", DBNull.Value)
            End If
            If littera_uploaded_doc_name <> "" And littera_uploaded_doc_name <> "NULL" Then
                cmd1.Parameters.AddWithValue("@uploaded_doc_name", littera_uploaded_doc_name)
            Else
                cmd1.Parameters.AddWithValue("@uploaded_doc_name", DBNull.Value)
            End If
            If littera_doctype <> "" And littera_doctype <> "NULL" Then
                cmd1.Parameters.AddWithValue("@doctype", littera_doctype)
            Else
                cmd1.Parameters.AddWithValue("@doctype", DBNull.Value)
            End If
            If littera_draftletter <> "" And littera_draftletter <> "NULL" Then
                cmd1.Parameters.AddWithValue("@draftletter", littera_draftletter)
            Else
                cmd1.Parameters.AddWithValue("@draftletter", DBNull.Value)
            End If
            If littera_is_final <> "" And littera_is_final <> "NULL" Then
                cmd1.Parameters.AddWithValue("@tttds_is_final", littera_is_final)
            Else
                cmd1.Parameters.AddWithValue("@tttds_is_final", DBNull.Value)
            End If
            If littera_doc_lettertype <> "" And littera_doc_lettertype <> "NULL" Then
                cmd1.Parameters.AddWithValue("@tttds_letter_type", littera_doc_lettertype)
            Else
                cmd1.Parameters.AddWithValue("@tttds_letter_type", DBNull.Value)
            End If
            If littera_docremarkenc <> "" And littera_docremarkenc <> "NULL" Then
                cmd1.Parameters.AddWithValue("@docremarkenc", littera_docremarkenc)
            Else
                cmd1.Parameters.AddWithValue("@docremarkenc", DBNull.Value)
            End If
            If littera_draftletterenc <> "" And littera_draftletterenc <> "NULL" Then
                cmd1.Parameters.AddWithValue("@draftletterenc", littera_draftletterenc)
            Else
                cmd1.Parameters.AddWithValue("@draftletterenc", DBNull.Value)
            End If


            cmd1.ExecuteNonQuery()

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return True
    End Function
    Function GET_COMPETENCY_CONFIG(msgtype) As String

        Dim messagetype As String = "0"
        Dim templateid As String = ""
        Dim text As String = ""
        Dim es As New SMSTemplate
        es = Get_SMS_TEMPLATE(msgtype)
        messagetype = es.ID
        templateid = es.DLT_CT_ID
        text = es.text
        Return text + "*" + templateid

    End Function

    Public Function Get_COMPETENCY_Configuration_values() As CompetencyConfiguration
        Dim es As New CompetencyConfiguration
        Dim Foldername As String = GET_SETTING_XML_FOLDER()
        Dim jsontxt = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/Competency_Config.json"))
        es = JsonConvert.DeserializeObject(Of CompetencyConfiguration)(jsontxt)


        Return es
    End Function
    Public Function GET_USER_AGENCY_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal agencytype As String, ByVal agencyid As String) As DataTable
        Dim dtuserdetails As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("yuser.proc_yuser_get_agency_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", agencytype)
        If Not agencyid Is Nothing Then
            cmd.Parameters.AddWithValue("@agencyid", agencyid)
        End If



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtuserdetails)
        con.Close()
        Return dtuserdetails
    End Function



    Public Function Get_Participant_New(ByVal Domain As String, ByVal isonline As String, ByVal trainingplanid As String, ByVal ParticipantId As String) As DataTable
        Dim dsSchemes As New DataSet
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_training_participants_vr1", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        If trainingplanid Is Nothing Or trainingplanid = "" Then
            cmd.Parameters.AddWithValue("@trainingid", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@trainingid", trainingplanid)
        End If
        If ParticipantId Is Nothing Or ParticipantId = "" Then
            cmd.Parameters.AddWithValue("@ParticipantId", DBNull.Value)
        Else
            cmd.Parameters.AddWithValue("@ParticipantId", ParticipantId)
        End If

        Dim daSchemes As SqlDataAdapter = New SqlDataAdapter(cmd)
        daSchemes.Fill(dsSchemes)
        con.Close()
        Dim dtfinal As DataTable = dsSchemes.Tables(0)
        dtfinal.Columns.Add("sposnorname")
        Dim dtagency As DataTable = GET_USER_AGENCY_DATA(Domain, isonline, "00053", Nothing)

        For Each dr As DataRow In dtfinal.Rows
            If dr("SponsorID").ToString() <> "" Then
                Dim dtfilteragency As New DataTable
                dtagency.DefaultView.RowFilter = "AgencyId='" + dr("SponsorID").ToString() + "'"
                dtfilteragency = dtagency.DefaultView.ToTable
                If dtfilteragency.Rows.Count > 0 Then
                    dr("sposnorname") = dtfilteragency.Rows(0)("AgencyName").ToString()
                End If
            End If

        Next



        Return dtfinal
    End Function



    Public Function GET_FACULTY_DATA_NEW(ByVal Domain As String, ByVal IsOnline As String, ByVal agencyid As String) As DataTable
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_get_agency]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", "00054")
        If Not agencyid Is Nothing Then
            If agencyid <> "" Then
                cmd.Parameters.AddWithValue("@AgencyId", agencyid)
            End If

        Else
            cmd.Parameters.AddWithValue("@AgencyId", DBNull.Value)
        End If


        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)



        '*************Extra Code to increase tyaam_val in first table to Manage old procedure data

        dsuserdetails.Tables(0).Columns.Add("tyaam_val")

        For Each dr In dsuserdetails.Tables(0).Rows
            Dim dtfilterdata As DataTable
            dtfilterdata = dsuserdetails.Tables(1)
            dtfilterdata.DefaultView.RowFilter = "tyaam_typeid='" + dr("tyaam_typeid").ToString + "'"
            dtfilterdata = dtfilterdata.DefaultView.ToTable()
            If dtfilterdata.Rows.Count > 0 Then
                dr("tyaam_val") = dtfilterdata.Rows(0)("tyaam_val")
            End If
        Next

        '*****************

        con.Close()
        'Return dsuserdetails.Tables(0)
        Dim dtNewAgency As New DataTable
        dtNewAgency.Columns.Add("totalrecord")
        dtNewAgency.Columns.Add("GuestFacultyId")
        dtNewAgency.Columns.Add("GuestFacultyName")
        dtNewAgency.Columns.Add("HGuestFacultyName")
        dtNewAgency.Columns.Add("isdisable")
        dtNewAgency.Columns.Add("CourseId")
        dtNewAgency.Columns.Add("CourseName")
        dtNewAgency.Columns.Add("HCourseName")
        dtNewAgency.Columns.Add("TopicId")
        dtNewAgency.Columns.Add("TopicExperience")
        dtNewAgency.Columns.Add("TopicName")
        dtNewAgency.Columns.Add("HTopicName")
        dtNewAgency.Columns.Add("TopicContent")
        dtNewAgency.Columns.Add("HTopicContent")
        dtNewAgency.Columns.Add("Per_Address")
        dtNewAgency.Columns.Add("Corr_Address")
        dtNewAgency.Columns.Add("Address2")
        dtNewAgency.Columns.Add("Address3")
        dtNewAgency.Columns.Add("Designation")
        dtNewAgency.Columns.Add("MobileNo")
        dtNewAgency.Columns.Add("PhoneNo")
        dtNewAgency.Columns.Add("officephone")
        dtNewAgency.Columns.Add("email")
        dtNewAgency.Columns.Add("pincode")
        dtNewAgency.Columns.Add("state")
        dtNewAgency.Columns.Add("city")
        dtNewAgency.Columns.Add("aadharno")
        dtNewAgency.Columns.Add("currentorganization")
        dtNewAgency.Columns.Add("bank")
        dtNewAgency.Columns.Add("branch")
        dtNewAgency.Columns.Add("IFSC")
        dtNewAgency.Columns.Add("accountno")
        dtNewAgency.Columns.Add("isinhousefaculty")
        dtNewAgency.Columns.Add("code")
        dtNewAgency.Columns.Add("saluatationId")
        dtNewAgency.Columns.Add("H_F_Name")
        dtNewAgency.Columns.Add("H_M_Name")
        dtNewAgency.Columns.Add("H_L_Name")

        dtNewAgency.Columns.Add("F_Name")
        dtNewAgency.Columns.Add("M_Name")
        dtNewAgency.Columns.Add("L_Name")
        dtNewAgency.Columns.Add("ts_name")
        dtNewAgency.Columns.Add("ts_hname")
        dtNewAgency.Columns.Add("subspecilization")
        dtNewAgency.Columns.Add("uploadpath")
        dtNewAgency.Columns.Add("ag_photo_path")
        dtNewAgency.Columns.Add("agencystatus")
        dtNewAgency.Columns.Add("DOB")
        dtNewAgency.Columns.Add("Gender")
        dtNewAgency.Columns.Add("tyaam_val")
        dtNewAgency.Columns.Add("tyaam_typeid")

        For Each dr As DataRow In dsuserdetails.Tables(0).Rows
            Dim dr1 As DataRow = dtNewAgency.NewRow
            dr1("totalrecord") = dsuserdetails.Tables(0).Rows.Count.ToString()
            dr1("GuestFacultyId") = dr("AgencyId")
            dr1("GuestFacultyName") = dr("AgencyName")
            dr1("HGuestFacultyName") = dr("HAgencyName")
            dr1("isdisable") = "0"
            dr1("CourseId") = Nothing
            dr1("CourseName") = Nothing
            dr1("HCourseName") = Nothing
            dr1("TopicId") = Nothing
            dr1("TopicExperience") = Nothing
            dr1("TopicName") = Nothing
            dr1("HTopicName") = Nothing
            dr1("TopicContent") = Nothing
            dr1("HTopicContent") = Nothing
            dr1("Per_Address") = dr("Ag_Address")
            dr1("Corr_Address") = dr("Ag_Address1")
            dr1("Address2") = Nothing
            dr1("Address3") = Nothing
            dr1("Designation") = ""
            dr1("MobileNo") = dr("ag_mobileno")
            dr1("PhoneNo") = dr("ag_phone")
            dr1("officephone") = dr("ag_phone")
            dr1("email") = dr("ag_email")
            dr1("pincode") = dr("ag_pincode")
            dr1("state") = dr("ag_address_state")
            dr1("city") = dr("ag_address_city")
            dr1("aadharno") = dr("ag_aadhar")
            dr1("currentorganization") = Nothing
            dr1("bank") = Nothing
            dr1("branch") = Nothing
            dr1("IFSC") = Nothing
            dr1("accountno") = Nothing
            dr1("isinhousefaculty") = Nothing
            dr1("code") = dr("UserCode")
            dr1("saluatationId") = dr("ag_salutation")
            dr1("H_F_Name") = dr("ag_first_name")
            dr1("F_Name") = dr("ag_first_name")
            dr1("M_Name") = dr("ag_m_name")
            dr1("L_Name") = dr("ag_l_name")
            dr1("ts_name") = Nothing
            dr1("ts_hname") = Nothing
            dr1("subspecilization") = Nothing

            dr1("uploadpath") = dr("uploadpath")
            dr1("ag_photo_path") = dr("ag_photo_path")
            dr1("agencystatus") = dr("tyaam_status")
            dr1("DOB") = dr("ag_dob")
            dr1("Gender") = dr("ag_gender")
            dr1("tyaam_val") = dr("tyaam_val")
            dr1("tyaam_typeid") = dr("tyaam_typeid")
            dtNewAgency.Rows.Add(dr1)
        Next

        dtNewAgency.DefaultView.RowFilter = "agencystatus=1"
        dtNewAgency = dtNewAgency.DefaultView.ToTable

        Return dtNewAgency
    End Function


    Public Function GET_EMPLOYEE_DATA(ByVal Domain As String, ByVal IsOnline As String, ByVal agencyid As String) As DataTable
        Dim dsuserdetails As New DataSet
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_yuser_get_agency]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000
        cmd.Parameters.AddWithValue("@agencytype", "00008")
        If Not agencyid Is Nothing Then
            If agencyid <> "" Then
                cmd.Parameters.AddWithValue("@AgencyId", agencyid)
            End If

        Else
            cmd.Parameters.AddWithValue("@AgencyId", DBNull.Value)
        End If

        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dsuserdetails)


        '*****************

        con.Close()

        Return dsuserdetails.Tables(0)
    End Function



    Public Function Get_Salutation(ByVal Domain As String, ByVal IsOnline As String) As DataTable
        Dim dtsalutation As New DataTable
        con = Get_Connection_String(Domain, IsOnline)
        con.Open()
        cmd = New SqlCommand("[yuser].[proc_tp_get_salutation]", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000



        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dtsalutation)


        '*****************

        con.Close()

        Return dtsalutation
    End Function

    Public Function Get_Session_Completion_Data(ByVal Domain As String, ByVal isonline As String, ByVal trainingid As String, ByVal usertype As String, ByVal userid As String, ByVal fromdt As String, ByVal todt As String) As DataTable

        'Dim dt As New DataTable
        'con = Get_Connection_String(Domain, isonline)
        'con.Open()
        'cmd = New SqlCommand("TrainingPlan.proc_tp_get_session_completion_status", con)
        'cmd.CommandType = CommandType.StoredProcedure
        'cmd.Connection = con
        'cmd.CommandTimeout = 5000
        'If Not trainingid Is Nothing Then
        '    cmd.Parameters.AddWithValue("@TrainingId", trainingid)
        'Else
        '    cmd.Parameters.AddWithValue("@TrainingId", DBNull.Value)
        'End If

        'cmd.Parameters.AddWithValue("@loginusertype", usertype)
        'cmd.Parameters.AddWithValue("@frmdt", fromdt)
        'cmd.Parameters.AddWithValue("@todt", todt)
        'cmd.Parameters.AddWithValue("@loginuserid", userid)
        'Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        'daTrainingCalender.Fill(dt)
        'con.Close()
        'Return dt

        Dim dt As New DataTable()
        con = Get_Connection_String(Domain, isonline)
        con.Open()

        cmd = New SqlCommand("TrainingPlan.proc_tp_get_session_completion_status", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 5000

        If Not trainingid Is Nothing Then
            cmd.Parameters.AddWithValue("@TrainingId", trainingid)
        Else
            cmd.Parameters.AddWithValue("@TrainingId", DBNull.Value)
        End If

        cmd.Parameters.AddWithValue("@loginusertype", usertype)
        cmd.Parameters.AddWithValue("@frmdt", fromdt)
        cmd.Parameters.AddWithValue("@todt", todt)
        cmd.Parameters.AddWithValue("@loginuserid", userid)

        ' Use SqlDataReader for more efficient data retrieval
        Dim reader As SqlDataReader = cmd.ExecuteReader()


        dt.Columns.Add("tttttm_training_id", GetType(String))
        dt.Columns.Add("ttttt_session_id", GetType(String))
        dt.Columns.Add("iscompleted", GetType(Int32))
        dt.Columns.Add("percentcomplete", GetType(Decimal))
        dt.Columns.Add("totalparticipant", GetType(Int32))
        ' Populate the DataTable with the data from the SqlDataReader
        Dim dr As DataRow

        dt.BeginLoadData()

        While reader.Read()
            ' Create a new DataRow
            dr = dt.NewRow()

            ' Assign values from the reader to the DataRow
            dr("tttttm_training_id") = If(reader("tttttm_training_id") IsNot DBNull.Value, reader("tttttm_training_id").ToString(), String.Empty)
            dr("ttttt_session_id") = If(reader("ttttt_session_id") IsNot DBNull.Value, reader("ttttt_session_id").ToString(), String.Empty)

            If reader("iscompleted") IsNot DBNull.Value Then
                dr("iscompleted") = Convert.ToInt64(reader("iscompleted"))
            Else
                dr("iscompleted") = 0
            End If

            If reader("percentcomplete") IsNot DBNull.Value Then
                dr("percentcomplete") = Convert.ToDecimal(reader("percentcomplete"))
            Else
                dr("percentcomplete") = 0
            End If

            If reader("totalparticipant") IsNot DBNull.Value Then
                dr("totalparticipant") = Convert.ToInt64(reader("totalparticipant"))
            Else
                dr("totalparticipant") = 0
            End If

            ' Add the DataRow to the DataTable
            dt.Rows.Add(dr)
        End While

        ' End batch insert mode
        dt.EndLoadData()

        reader.Close()
        con.Close()

        Return dt
    End Function



    Public Function Get_Common_DB_Data(ByVal Domain As String, ByVal IsOnline As Boolean, ByVal ProcedureName As String, ByVal dtparameterlist As DataTable) As DataSet
        Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
        Dim dsItemGroup As New DataSet

        ' Get the database connection
        Dim con As SqlConnection = Get_Connection_String(Domain, IsOnline)

        ' Using the connection within the Using block ensures that it is automatically closed when done
        Using con
            ' Open the connection if it is closed
            If con.State = ConnectionState.Closed Then con.Open()

            ' Setup the SqlCommand and SqlDataAdapter
            Using cmd As New SqlCommand(ProcedureName, con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandTimeout = 5000

                ' Adding parameters from the DataTable only if it is not Nothing
                If Not dtparameterlist Is Nothing Then
                    Dim j As Integer
                    For i = 0 To dtparameterlist.Rows.Count - 1
                        For j = 0 To dtparameterlist.Columns.Count - 1
                            If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString <> "" Then
                                If dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString = "NULL" Then
                                    cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, DBNull.Value)
                                Else
                                    cmd.Parameters.AddWithValue(dtparameterlist.Columns(j).ColumnName, dtparameterlist.Rows(i)(dtparameterlist.Columns(j).ColumnName).ToString)
                                End If

                            End If
                        Next
                    Next
                End If

                ' Logging before executing the command
                logger.Error("DataManager - Get_Common_Data->Start Connection " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))

                ' Execute the command and fill the dataset
                Using adp As New SqlDataAdapter(cmd)
                    adp.Fill(dsItemGroup)
                End Using

                ' Logging after execution
                logger.Error("DataManager - Get_Common_Data->End Connection " + Common.getDateTime.ToString("dd/MM/yyyy hh:mm:ss"))
            End Using
        End Using

        ' Return the filled dataset
        Return dsItemGroup
    End Function

    Public Function Get_Session_Completion_Summary(ByVal Domain As String, ByVal isonline As String, ByVal branchid As String, ByVal usertype As String, ByVal userid As String, ByVal fromdt As String, ByVal todt As String) As DataTable

        Dim dt As New DataTable
        con = Get_Connection_String(Domain, isonline)
        con.Open()
        cmd = New SqlCommand("TrainingPlan.proc_tp_get_course_progress_summary", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = con
        cmd.CommandTimeout = 5000


        cmd.Parameters.AddWithValue("@loginuserid", userid)
        cmd.Parameters.AddWithValue("@loginusertype", usertype)
        cmd.Parameters.AddWithValue("@frmdt", fromdt)
        cmd.Parameters.AddWithValue("@todt", todt)
        If Not branchid Is Nothing Then
            cmd.Parameters.AddWithValue("@branchid", branchid)
        Else
            cmd.Parameters.AddWithValue("@branchid", DBNull.Value)
        End If
        Dim daTrainingCalender As SqlDataAdapter = New SqlDataAdapter(cmd)
        daTrainingCalender.Fill(dt)
        con.Close()



        Return dt
    End Function

End Class
