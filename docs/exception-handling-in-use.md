# Exception Handling In Use

## Purpose

Describe the backend exception-handling pattern that is currently in use in this codebase today, before the planned global exception middleware is introduced.

## Current Pattern

The backend currently uses a mixed approach:

- Some controller actions catch `SqlException` locally and convert it into a `400 Bad Request`.
- Other actions do not catch exceptions locally, so failures bubble up as unhandled exceptions.
- Unhandled exceptions are logged centrally in `Program.cs`, but they are not yet converted into one shared JSON API error contract.

## SQL Exception Pattern In Use

Several controller actions use a local `try/catch` like this:

```csharp
try
{
    // business/database call
    return Ok(result);
}
catch (SqlException ex)
{
    return SqlExceptionResponseHelper.CreateBadRequest(ex);
}
```

Examples:

- `TrainingController.GetTrainings`
- `TrainingController.RCVP_Trg_Type_Save_Data`
- multiple actions in `ContentController`

## Shared Helper In Use

`Common/SqlExceptionResponseHelper.cs` is the shared formatter used by controllers that follow the SQL exception pattern.

Current response shape:

```json
{
  "success": false,
  "errorCode": "SQL_ERROR",
  "message": "Database or procedure message"
}
```

Behavior:

- Returns HTTP `400 Bad Request`
- Uses the last non-empty message from `SqlException.Errors`
- Falls back to `SqlException.Message`
- Falls back again to a generic database error message if needed

## Unhandled Exception Logging In Use

Unhandled exceptions are logged in `Program.cs`.

Current behavior:

- The request pipeline catches any uncaught exception
- It logs file and line information when available
- It logs the exception as `Unhandled exception`
- It rethrows the exception

Important limitation:

- Logging is centralized, but error responses are not centralized yet
- Endpoints without a local `try/catch` can still return framework-default `500` responses

## Example Of A Gap

`TrainingController.RCVP_Training_Type_Save_Data` currently does not wrap its database call in a `try/catch`.

Current result:

- success path returns `200 OK` with `true`
- SQL/procedure failure bubbles up as an unhandled exception
- the error is logged centrally
- the client likely receives a generic `500` response instead of the shared SQL error JSON shape

## Practical Guidance For New Or Updated Endpoints

Until global exception middleware is implemented, follow the existing local SQL exception convention for endpoints that directly or indirectly execute SQL procedures:

1. Wrap the controller action body in `try/catch`
2. Catch `SqlException`
3. Return `SqlExceptionResponseHelper.CreateBadRequest(ex)`
4. Leave unexpected non-SQL exceptions to centralized logging unless the endpoint has a specific business reason to handle them differently

## Known Limitations

- Error behavior is inconsistent between endpoints
- Most controllers do not validate all bad-input cases before calling the BL/DB layer
- Unhandled exceptions are logged, but clients do not always get a predictable JSON error payload
- SQL exception messages may expose raw database/procedure text if the procedure message is not UI-safe

## Related Document

For the planned future direction, see `docs/global_exception_handling_plan.md`.
