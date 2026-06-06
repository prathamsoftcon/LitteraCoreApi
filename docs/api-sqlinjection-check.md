# Codex Instruction File – SQL Injection Audit and Remediation (.NET 6 / SQL Server)

## Objective

Audit the codebase for SQL Injection vulnerabilities and automatically refactor vulnerable database access code to use parameterized queries.

---

## Scope

Analyze all `.cs`, `.vb`, repository, BL, DB, controller, service, and helper files.

Focus on:

* `SqlCommand`
* `SqlConnection`
* `SqlDataAdapter`
* `ExecuteReader`
* `ExecuteScalar`
* `ExecuteNonQuery`
* `CommandType.Text`
* Dynamic SQL generation
* Stored procedure execution
* Raw ADO.NET code

---

## High-Risk Patterns

Identify all code matching patterns similar to:

```csharp
"SELECT * FROM Users WHERE UserID='" + userId + "'"

"UPDATE Users SET Name='" + name + "'"

"DELETE FROM Users WHERE UserID='" + id + "'"

"EXEC proc_test '" + value + "'"

string.Format(
    "SELECT * FROM Table WHERE Code='{0}'",
    code
)

$"SELECT * FROM Users WHERE UserID='{userId}'"
```

Also detect:

```csharp
query += condition;
sql += userInput;
```

and any concatenation involving:

```csharp
+
string.Format
$"..."
StringBuilder
Append
AppendLine
```

---

## Example Vulnerability

### Vulnerable

```csharp
SqlCommand cmd = new SqlCommand(
    "SELECT * FROM trainingplan.VW_Training_calendar " +
    "WHERE trainingno='" + trainingcode + "'",
    con
);
```

### Secure

```csharp
SqlCommand cmd = new SqlCommand(
    @"SELECT *
      FROM trainingplan.VW_Training_calendar
      WHERE trainingno = @trainingcode",
    con
);

cmd.Parameters.Add(
    "@trainingcode",
    SqlDbType.VarChar,
    100
).Value = trainingcode ?? string.Empty;
```

---

## Required Refactoring Rules

### Rule 1

Never concatenate user input into SQL.

Replace:

```csharp
"... WHERE Id='" + id + "'"
```

with:

```csharp
"... WHERE Id=@Id"

cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;
```

---

### Rule 2

Convert all text-based SQL commands to parameterized SQL.

---

### Rule 3

When data type is known, use explicit SQL types.

Prefer:

```csharp
cmd.Parameters.Add("@TrainingId",
    SqlDbType.UniqueIdentifier).Value = trainingId;
```

instead of:

```csharp
AddWithValue()
```

where practical.

---

### Rule 4

Validate inputs before executing queries.

Examples:

```csharp
Guid.TryParse(...)
int.TryParse(...)
DateTime.TryParse(...)
```

Apply validation whenever possible.

---

### Rule 5

Keep business logic unchanged.

Do not modify:

* Returned data
* Mapping logic
* DTOs
* Models
* JSON serialization
* Existing API contracts

Only remove SQL injection risks.

---

## Special Cases

### Stored Procedures

Safe:

```csharp
cmd.CommandType = CommandType.StoredProcedure;
cmd.CommandText = "proc_user_get";

cmd.Parameters.Add(
    "@UserId",
    SqlDbType.VarChar
).Value = userId;
```

Unsafe:

```csharp
cmd.CommandText =
    "EXEC proc_user_get '" + userId + "'";
```

Refactor unsafe versions.

---

### Dynamic Filters

Unsafe:

```csharp
sql += " AND Name='" + name + "'";
```

Refactor to:

```csharp
sql += " AND Name=@Name";

cmd.Parameters.Add(
    "@Name",
    SqlDbType.VarChar
).Value = name;
```

---

## Output Requirements

Generate a report:

| File       | Line | Vulnerability Type | Risk | Fixed |
| ---------- | ---- | ------------------ | ---- | ----- |
| Example.cs | 145  | SQL Injection      | High | Yes   |

Include:

* File path
* Method name
* Original vulnerable code
* Refactored code
* Risk rating

---

## Risk Classification

### Critical

User input reaches:

* UPDATE
* DELETE
* INSERT
* MERGE
* EXEC

through string concatenation.

### High

User input reaches:

* SELECT
* Dynamic WHERE clauses

through string concatenation.

### Medium

Dynamic SQL with partial validation.

### Low

Stored procedures already using parameters.

---

## Final Validation

After refactoring:

1. Project must compile.
2. No SQL syntax changes.
3. No API contract changes.
4. No mapping logic changes.
5. All user-controlled values must be parameterized.
6. Generate summary:

```text
Total SQL commands scanned:
Total vulnerable commands found:
Total fixed:
Critical:
High:
Medium:
Low:
```

---

## Additional Investigation

Search entire solution for:

```text
SqlCommand(
CommandType.Text
ExecuteReader(
ExecuteScalar(
ExecuteNonQuery(
string.Format(
$"
Append(
AppendLine(
SELECT
UPDATE
DELETE
INSERT
EXEC
EXECUTE
```

Review every occurrence manually if automatic remediation is uncertain.
