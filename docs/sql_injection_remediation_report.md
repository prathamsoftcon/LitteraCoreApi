# SQL Injection Audit and Remediation Report

## Summary

| Metric | Count |
| --- | ---: |
| Total `SqlCommand` occurrences scanned (`.cs` and `.vb`) | 596 |
| Vulnerable commands found | 84 |
| Active C# commands fixed | 31 |
| Unresolved legacy VB commands | 53 |
| Critical | 2 |
| High | 75 |
| Medium | 7 |
| Low / already safe | 512 |

The active .NET 6 application no longer contains user values concatenated into executable
`SqlCommand` or `SqlDataAdapter` SQL. Full command-level evidence, including original and
refactored code descriptions, API traces, final line numbers, risk, and fixed status, is in
[`dynamic_sql_api_findings.csv`](dynamic_sql_api_findings.csv).

## Active Remediation

| File | Final lines | Vulnerability type | Risk | Fixed |
| --- | --- | --- | --- | --- |
| `DBContext/TrainingDB.cs` | 31, 36, 252, 427, 633, 1225 | Concatenated SELECT/function arguments | High/Medium | Yes |
| `DBContext/SessionDB.cs` | 35, 1193, 1198, 3319 | Concatenated SELECT predicates | High/Medium | Yes |
| `DBContext/ContentDB.cs` | 365, 397, 487, 492, 542 | Concatenated SELECT predicates | High | Yes |
| `DBContext/FeedbackDB.cs` | 26, 65 | Concatenated SELECT predicates | High | Yes |
| `DBContext/EvalDB.cs` | 322 | Concatenated SELECT predicate | High | Yes |
| `DBContext/ParticipantDB.cs` | 969 | Concatenated SELECT predicates | High | Yes |
| `DBContext/SupportDB.cs` | 669 | Concatenated SELECT subquery predicate | High | Yes |
| `DBContext/UserDB.cs` | 790 | Concatenated SELECT predicates | High | Yes |
| `Common/DMS/DMSDB.cs` | 93, 168, 173, 228, 621, 682, 697, 702, 838, 846 | Concatenated functions, filters, and `IN` list | High/Medium | Yes |

Refactoring used explicit `SqlDbType` values and bounded string sizes. Date strings in the
DMS dashboard query are validated with `DateTime.TryParse`. Certificate agency IDs and DMS
document lists are validated with `Guid.TryParse`. The DMS `IN` clause now contains only
internally generated parameter names and one `UniqueIdentifier` parameter per GUID.

## Legacy Findings

`DBContext/old/Datamanager.vb` is not compiled by the current project. The audit found:

| Finding | Lines | Risk | Fixed |
| --- | --- | --- | --- |
| 51 concatenated legacy SELECT/function commands | See CSV | High | No |
| `Get_DATA_FROM_TABLES`: caller-controlled table expression | 5211 | Critical | No |
| `Get_DATA_FROM_TABLE_NEW`: caller-controlled complete SQL command | 6613 | Critical | No |

SQL parameters cannot represent table identifiers or complete statements. Securing the two
Critical APIs requires an allowlist, retirement, or API contract change, so the legacy files
were intentionally left unchanged. All 53 legacy findings are individually listed in the CSV.

## Validation

- `dotnet build LitteraCore.sln --no-restore --nologo '-clp:ErrorsOnly;Summary'`
  completed with **0 errors** and 4,469 existing project/environment warnings.
- A multiline statement scan found no active value-concatenated `SqlCommand` or direct
  `SqlDataAdapter` SQL.
- The only remaining active interpolated SQL is the DMS `IN` clause that joins trusted,
  internally generated names such as `@DocId0`; all values are parameters.
- Stored-procedure commands and their `CommandType.StoredProcedure` behavior were unchanged.
- Controllers, routes, DTOs, mappings, response serialization, and public method signatures
  were unchanged.
