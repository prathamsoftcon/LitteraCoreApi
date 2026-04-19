# Review Comments & Suggestions

## Quick Fixes
1. **Logger type**
   - Replace `ILogger<AgencyController>` with `ILogger<DashboardController>`.

2. **Null-safety for `FirstOrDefault()`**
   - Example:
     - Before: `d.total_enrollments = p.FirstOrDefault().totalrecords;`
     - After: `d.total_enrollments = p.FirstOrDefault()?.totalrecords ?? 0;`

3. **Participant status lookup**
   - Guard against missing participant info and DMS status:
     - Lookup `PAI.FirstOrDefault(...)` and only read `ttpai_id` when not null.
     - Use `dms?.doc_status` when assigning `participantstatus`.

## Refactor Suggestions
- Extract shared training query logic into a `DashboardService` (or similar) with methods like:
  - `GetDashboardData(...)`
  - `GetAllTrainingData(...)`
  - `GetUpcomingEvents(...)`
- Keep controller actions thin and orchestration-only.

## Configuration
- Move `Config.json` loading to startup and inject via `IOptions<T>`.
- Avoid per-request file IO in controller actions.

## Consistency
- Replace string usertype checks (e.g., `"5"`) with enum-based checks.
- Prefer consistent casing and avoid repeated `ToUpper()` comparisons where possible.
