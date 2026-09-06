# Participant Data Stored-Procedure Endpoint Map

This inventory lists active API actions that reach `TrainingPlan.proc_tp_training_participants_vr2`, whether directly or through a `ParticipantDB`, BL, or DB helper. Commented-out code is excluded.

## Scope

- Active endpoint count: **23**.
- The application uses JWT as its default and fallback authorization policy.
- `/api/TRG_PARTICIPANT_DETAILS_wk` and `/api/TrgSessions` explicitly use the `PublicApiKey` policy.
- No route in this inventory is marked `AllowAnonymous`.

## Procedure access methods

| `ParticipantDB` method | Procedure parameters supplied | Notes |
|---|---|---|
| `Get_TRG_PARTICIPANT_Data` | `@trainingid`, `@ParticipantId`, `@ColumnList`, `@branchid` | Executes the procedure only when `trainingid` is non-null. |
| `Get_Search_Participant` | `@trainingid`, `@ParticipantId`, `@SearchColumn`, `@SearchValue`, `@ColumnList`, `@branchid` | Executes the procedure only when `trainingid` is non-null. |
| `Get_Trg_Participant_List` | `@trainingid`, `@ParticipantId`, search/sort/filter values, paging, certificate flag, `@ColumnList`, `@branchid` | Always executes the procedure; a null `trainingid` is sent as `DBNull.Value`. |

All three active methods configure a 5,000-second command timeout.

## Endpoints

### TrainingController

| Verb | Route | Parameters | Call path | Participant-data use |
|---|---|---|---|---|
| GET | `/api/TRG_PARTICIPANT_DETAILS` | `trainingid`, `participantid?`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Returns participant detail(s) for a training. |
| GET | `/api/TRG_PARTICIPANT_DETAILS_wk` | `trainingid`, `participantid?`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Same lookup; uses `PublicApiKey`. |
| GET | `/api/Generate_Certificate` | `trainingid`, `participantid`, `branchid`, `APPURL`, `Logo_Path`, `loginuserid?` | Direct: `Get_Trg_Participant_List` | Looks up `ttpai_id` before generating certificate HTML. |
| GET | `/api/Generate_Certificate_New` | `trainingid`, `participantid`, `branchid`, `APPURL`, `Logo_Path`, `loginuserid?` | Direct: `Get_Trg_Participant_List` | New certificate-generation variant; looks up `ttpai_id`. |
| GET | `/api/Generate_ALL_Certificate` | `trainingid`, `branchid`, `APPURL`, `Logo_Path`, `loginuserid?` | Direct: `Get_Trg_Participant_List` | Background generation for all training participants. |
| GET | `/api/Generate_And_Download_ALL_Certificate` | `trainingid`, `branchid`, `APPURL`, `Logo_Path`, `loginuserid?` | Direct: `Get_Trg_Participant_List` | Background generate-and-download flow for all participants. |
| GET | `/api/Download_All_Certificate` | `trainingid`, `branchid`, `APPURL`, `Logo_Path`, `loginuserid?` | Direct: `Get_Trg_Participant_List` | Loads participants for pre-generated certificate download. |
| GET | `/api/Generate_Certificate_All` | `trainingid`, `branchid`, `APPURL`, `Logo_Path` | Direct: `Get_TRG_PARTICIPANT_Data` | Builds combined certificate HTML for all participants. |
| POST | `/api/Get_Trg_Participant_List` | `trainingid?`, `participantid?`, `branchid?`, `searchcolumn?`, `searchvalue?`, `sortcolumn?`, `sortvalue?`, `filtername?`, `filtervalue?`, `pageno=1`, `pagesize=-1`, `is_certificate_generated=2` | `TrgBL.Get_Trg_Participant_List` → `ParticipantDB.Get_Trg_Participant_List` | Returns a paged training-participant list. |
| GET | `/api/Generate_Certificate_BR` | `trainingid`, `participantid`, `branchid`, `APPURL`, `Logo_Path`, `loginuserid?` | Direct: `Get_Trg_Participant_List` | Business-rule certificate generation; looks up `ttpai_id`. |

### SessionController

| Verb | Route | Parameters | Call path | Participant-data use |
|---|---|---|---|---|
| GET | `/api/TrgSessions` | `trainingid`, `pagetype=0`, `usertype?`, `userid?`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | When `usertype` is Participant (`5`), derives approval status. Uses `PublicApiKey`. |
| GET | `/api/GET_PARTICIPANT_NEXT_SESSION` | `trainingid`, `participantid`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Checks participant approval/status while finding the next session. |
| GET | `/api/CHECK_PREVIOUS_SESSION_FOR_COMPLETION` | `trainingid`, `sessionid`, `pagetype=0`, `usertype?`, `userid?`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Conditional participant-status lookup when `usertype` is Participant (`5`). |
| GET | `/api/TrgSessions_with_content` | `trainingid`, `pagetype=0`, `usertype?`, `userid?`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Conditional participant-status lookup when `usertype` is Participant (`5`). |

### ParticipantController and EvalTestController

| Verb | Route | Parameters | Call path | Participant-data use |
|---|---|---|---|---|
| GET | `/api/TRG_PARTICIPANT_ACTION` | `usertype`, `userid`, `trainingid`, `participantid`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Uses the participant record to build available actions. |
| GET | `/api/Check_Test_Eligibility` | `userid`, `trainingid`, `testid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Checks whether training status allows a test. |
| GET | `/api/TRAINING_TEST_ANALYTIC_DATA` | `usertype`, `userid`, `fromdate`, `todate`, `trainingid?`, `sessionid?`, `participantid?`, `groupOn=1`, `testtype=3`, `testid?`, `branchid?` | `EvalTestController` → `EvalBL` → `EvalDB` → `Get_TRG_PARTICIPANT_Data` | Loads participant data only when `trainingid` is supplied; enriches analytics. |

### AssignmentController

| Verb | Route | Parameters | Call path | Participant-data use |
|---|---|---|---|---|
| GET | `/api/Get_Assignment_Valuation` | `assignmentid`, `participantid?` | `AssignmentBL.Get_Valuation` → `Get_TRG_PARTICIPANT_Data` | Combines assignment valuation with the training participant list. |
| POST | `/api/Get_Comments` | `assignmentid`, `trainingid`, pagination query, `SearchParam` body, `participantid?`, `branchid?` | Direct: `Get_TRG_PARTICIPANT_Data` | Joins assignment comments to the participant list. |
| POST | `/api/Get_Assignment_Upload_Status` | `trainingid`, `assignmentid`, `PaginationParam`, `SearchParam` body?, `branchid?`, `status=2` | Direct: `Get_TRG_PARTICIPANT_Data` | Compares assignment uploads against every participant. |

### SupportController and ApplicationConfigController

| Verb | Route | Parameters | Call path | Participant-data use |
|---|---|---|---|---|
| POST | `/api/SearchParticipant` | `trainingid?`, `searchcolumn?`, `searchvalue?`, `branchid?`, `PaginationParam` query? | Direct: `Get_Search_Participant` | Searches participants and returns paged results. |
| POST | `/api/GET_ENROLLMENT_SUMMARY` | `TrainingList` body, `branchid`, `PaginationParam` query? | Direct: `Get_Trg_Participant_List` | Gets one paged participant row per training for enrollment totals. |
| POST | `/TRG_SEND_PARTICIPANT_MAIL` | `bulk_maildetails`, `participantttype`, `trainngid?` | Direct: `Get_Trg_Participant_List` | When `participantttype` is not `"2"`, obtains participant emails for the training. |

## Source locations reviewed

| File | Relevant responsibility |
|---|---|
| `DBContext/ParticipantDB.cs` | Active methods at lines 22, 332, and 549; procedure commands at lines 41, 351, and 570. |
| `Controllers/TrainingController.cs` | Participant detail, certificate, and participant-list routes. |
| `Controllers/SessionController.cs` | Training session and participant progression routes. |
| `Controllers/ParticipantController.cs`, `Controllers/EvalTestController.cs` | Participant actions and test eligibility/analytics. |
| `Controllers/AssignmentController.cs`, `BLContext/AssignmentBL.cs` | Comments, upload status, and valuation enrichment. |
| `Controllers/SupportController.cs`, `Controllers/ApplicationConfigController.cs` | Search, enrollment summary, and participant-mail recipient lookup. |
| `DBContext/EvalDB.cs`, `BLContext/TrgBL.cs`, `Program.cs` | Analytics call chain, participant-list delegation, and authorization defaults. |

## Notes

- This is a source-level inventory, not an HTTP runtime trace. Routes noted as conditional reach the procedure only when their stated condition is true.
- Routes using `Get_Search_Participant` or `Get_Trg_Participant_List` are included because those methods execute the same stored procedure even though they do not call `Get_TRG_PARTICIPANT_Data` by name.
