# Global Exception Handling Plan

## Objective

Introduce centralized API exception handling in the backend so unhandled errors return a consistent JSON response shape, while keeping frontend changes limited and predictable.

## Scope

- Backend: add global exception-handling middleware for ASP.NET Core APIs.
- Frontend: align error parsing with the new consistent JSON error contract.
- Non-goal: change successful response payloads.

## Backend Changes

### Current state

- Unhandled exceptions are logged centrally in `Program.cs`.
- Error responses are not centralized.
- Some controllers handle exceptions locally, but the behavior is inconsistent.
- SQL exceptions are not mapped through one reusable API response format.

### Planned changes

1. Create a dedicated middleware, for example `GlobalExceptionHandlingMiddleware`.
2. Register it early in the ASP.NET Core pipeline in `Program.cs`.
3. Catch exceptions in one place and return consistent JSON.
4. Keep centralized logging in the middleware.
5. Remove duplicated controller-level `try/catch` blocks where they are only translating exceptions into generic HTTP responses.

### Recommended response shape

```json
{
  "success": false,
  "message": "Human-readable error message",
  "errorCode": "SQL_ERROR",
  "details": "Optional low-level detail"
}
```

### Recommended status mapping

- `SqlException` -> `400 Bad Request` for known validation/business/procedure issues, otherwise `500 Internal Server Error`
- `InvalidOperationException` -> `400 Bad Request`
- `UnauthorizedAccessException` -> `401 Unauthorized`
- `KeyNotFoundException` -> `404 Not Found`
- fallback `Exception` -> `500 Internal Server Error`

### SQL exception handling guidance

- Prefer a small shared formatter method for SQL messages before writing the response.
- Avoid exposing raw database internals unless the message is already safe for UI display.
- Log the full exception server-side even if the response message is simplified.

### Rollout notes

- Keep success payloads unchanged.
- Start by handling only unhandled exceptions globally.
- For endpoints with custom business responses, review before removing local `try/catch`.
- Test at least one `SqlException`, one validation-style exception, and one unexpected exception.

## Frontend Changes Instructions

### Expected impact

- Successful API calls should not require changes.
- Error-handling code may need updates if it depends on endpoint-specific response shapes.

### Frontend contract

Frontend should read API failures from a common structure:

```json
{
  "success": false,
  "message": "Human-readable error message",
  "errorCode": "SQL_ERROR"
}
```

### Frontend implementation instructions

1. Continue treating non-2xx responses as failures.
2. Read the error message from `response.data.message` first.
3. Optionally use `response.data.errorCode` for special UI handling.
4. Fall back to a generic message when `message` is missing.
5. Do not assume all failures are `404`; use both status code and message.

### Suggested frontend fallback logic

```javascript
const message =
  error?.response?.data?.message ||
  error?.response?.data?.title ||
  "Something went wrong. Please try again.";
```

### Frontend testing checklist

- Verify SQL/procedure failures show a readable toast or inline error.
- Verify unknown server failures show a generic safe message.
- Verify current success flows remain unchanged.
- Verify pages do not rely on raw HTML or plain-text error bodies.

## Suggested Order Of Work

1. Add backend middleware and JSON error contract.
2. Validate a few existing endpoints against the new contract.
3. Update frontend shared API error parsing if needed.
4. Remove redundant local exception handling only after verification.

## Risk Notes

- Some existing frontend screens may depend on old ad hoc error shapes.
- Some controller actions may intentionally return custom non-2xx responses.
- SQL exception messages may need sanitization before exposing them to users.
