# Documentation

## Backend Notes

- [`exception-handling-in-use.md`](exception-handling-in-use.md)
- [`global_exception_handling_plan.md`](global_exception_handling_plan.md)

## Auth Notes

- [`authentication_cookie_hardening.md`](authentication_cookie_hardening.md)
- [`authentication_jwt_flow_update.md`](authentication_jwt_flow_update.md)
- [`frontend_file_upload_instructions.md`](frontend_file_upload_instructions.md)
- [`frontend_auth_migration_instructions.md`](frontend_auth_migration_instructions.md)
- [`qa_auth_flow_instructions.md`](qa_auth_flow_instructions.md)

## `DashboardController.Dashboard_Data`

### Purpose
Returns paged training dashboard data for a user, including status-based filtering, search criteria filtering, session completion percentages, ratings, and action info.

### Data Sources
- `TrainingDB.Get_VW_Training_calendar(startdate, enddate, filter_status, filter_cd, filter_acd)`
- `TrainingDB.Get_Users_Trg_Data(...)`
- `DashboardBL.Get_Trg_Feedback_Data(finyear)`
- `SessionDB.Get_Session_Status_summary(...)`
- `SessionDB.Get_Session_Data(startdate, enddate)`
- `ParticipantDB.Get_Participant_Additional_info(...)` (participant-only)
- `DMSBL.GET_DMS_STATUS_DATA_FOR_SELECTED_DOCID(...)` (participant-only)

### Database Objects
- Not directly referenced in this method. Access is encapsulated by `TrainingDB`, `SessionDB`, `ParticipantDB`, and `DashboardBL`.

### Parameters
- `usertype` (`string`): User type identifier (e.g., participant, admin).
- `userid` (`string`): Current user identifier.
- `startdate` (`DateTime`): Start date for training range.
- `enddate` (`DateTime`): End date for training range.
- `param` (`PaginationParam`): Paging parameters from query string.
- `searchCriterias` (`SearchParam?`): Search filters posted in request body.
- `filter_status` (`string`): Comma-separated training status filter.
- `filter_cd` (`string`): Course director filter.
- `filter_acd` (`string`): Associate course director filter.
- `branchid` (`string?`): Optional branch filter.

### Return values
- `200 OK`: A paged response containing filtered training items with completion percentage, rating, and action info.

### Example usage
`POST /api/Dashboard_Data?usertype=5&userid=USER_ID&startdate=2024-04-01&enddate=2024-04-30&filter_status=1,5`

