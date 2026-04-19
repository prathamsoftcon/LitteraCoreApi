# Review — `Controllers/DashboardController.cs`

## Summary
- Large controller with business logic mixed into actions.
- Repeated code patterns and potential null-reference risks.
- Logger type mismatch and direct configuration file reads.

## Key Issues
1. Logger type mismatch: `_logger` should be `ILogger<DashboardController>`.
2. NullReference risk: `FirstOrDefault()` results used without null checks.
3. Duplicate patterns: repeated filtering/paging logic across multiple actions.
4. Config read in controller: `Config.json` loaded directly with no caching or error handling.
5. Magic values: string/numeric user type values used in many comparisons.

## Recommendations
- Fix logger generic type and constructor injection.
- Guard all `FirstOrDefault()` usages with null checks or `?.`.
- Extract training/dashboard logic into services for reuse and testability.
- Load `Config.json` at startup and inject via options.
- Use enums or typed constants for user types and statuses.
