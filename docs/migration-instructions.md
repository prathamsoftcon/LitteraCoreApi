# ASP.NET Core 6 API Migration Instructions
## Legacy VB.NET → ASP.NET Core 6 Migration Guide

This project involves migrating legacy VB.NET APIs into ASP.NET Core 6 while preserving:
- existing API behavior
- stored procedure logic
- response structure
- frontend compatibility
- mobile app compatibility

The migration should modernize the codebase gradually without introducing unnecessary architectural complexity.

---

# PROJECT ARCHITECTURE

This project already uses the following structure:

```text
Controllers/
BLContext/
DBContext/
Models/
```

Example:

```text
Controllers/
    ContentController.cs

BLContext/
    ContentBL.cs

DBContext/
    ContentDB.cs

Models/
    Content.cs
```

Copilot MUST follow this existing architecture.

DO NOT introduce:
- unnecessary Repository folders
- Service folders
- DTO folders
- Clean Architecture layers
- CQRS/Mediator patterns

unless explicitly requested.

---

# LEGACY VB.NET BACKGROUND

Legacy APIs commonly used:

```text
Domain
isonline
TableIndex
APIKEY
DataSet
DataTable
JavaScriptSerializer
Get_Common_Data()
```

Example old API:

```text
/Folder/Get_Folder_Data?Domain=YOJNAACADEMY&isonline=1&TableIndex=0&APIKEY=XXX
```

Legacy flow:

```text
Controller
    ↓
Datamanager.vb
    ↓
Get_Common_Data()
    ↓
DataSet.Tables(TableIndex)
    ↓
JavaScriptSerializer
```

These patterns should NOT be recreated in ASP.NET Core.

---

# CORE MIGRATION PRINCIPLES

## Primary Goal

Modernize APIs gradually while preserving:
- business logic
- database behavior
- stored procedure calls
- response compatibility

---

# MIGRATION PRIORITY

Priority order during migration:

1. Convert VB.NET syntax to C#
2. Preserve API behavior
3. Preserve response structure
4. Preserve stored procedure calls
5. Remove obsolete VB.NET dependencies
6. Improve security
7. Improve async support
8. Improve architecture gradually

Avoid massive rewrites during initial migration.

---

# CONTROLLER RULES

## Location

```text
Controllers/
```

## Responsibilities

Controllers should:
- define API routes
- accept HTTP requests
- validate input
- call BLContext methods
- return IActionResult

Controllers must NOT:
- contain SQL
- contain DataTable logic
- contain business logic
- manipulate database objects directly

---

# CONTROLLER PATTERN

GOOD:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ContentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [Route("api/Get_Session_Avg_Learning_Time")]
    public IActionResult Get_Session_Avg_Learning_Time(
        string trainingid)
    {
        ContentBL CBL = new ContentBL(_configuration);

        var result =
            CBL.Avg_Learning_data_sessionwise(trainingid);

        return Ok(result);
    }
}
```

---

# BLContext RULES

## Location

```text
BLContext/
```

## Responsibilities

BLContext classes contain:
- business logic
- validation
- calculations
- aggregation
- filtering
- combining DB results

BLContext classes:
- may call DBContext
- may transform models
- may apply calculations

BLContext classes must NOT:
- execute inline SQL
- use Request/Response directly
- use HttpContext directly

---

# BLContext PATTERN

GOOD:

```csharp
public List<Avg_Learning_data_Sessionwise>
Avg_Learning_data_sessionwise(string trainingid)
{
    ContentDB CDB = new ContentDB(_configuration);

    var data =
        CDB.Get_trg_avg_learning_Time(trainingid);

    return data;
}
```

---

# DBContext RULES

## Location

```text
DBContext/
```

## Responsibilities

DBContext classes handle:
- database access
- stored procedure execution
- SQL execution
- data mapping

DBContext classes:
- may use ADO.NET
- may use Dapper
- must use parameterized queries

---

# STORED PROCEDURE RULES

This project is heavily dependent on SQL Server stored procedures.

Copilot should:
- preserve stored procedure names
- preserve parameter names
- preserve output structure
- preserve existing SP behavior

DO NOT rewrite stable stored procedures into:
- LINQ
- EF queries
- unnecessary ORM logic

unless explicitly requested.

GOOD:

```csharp
SqlCommand cmd = new SqlCommand(
    "Trainingplan.proc_tp_get_upload_session_attachement",
    con);

cmd.CommandType = CommandType.StoredProcedure;
```

---

# DATABASE ACCESS RULES

Preferred:
- Dapper

Allowed:
- ADO.NET

Avoid:
- DataSet-heavy architecture
- excessive DataTable filtering

---

# SQL SECURITY RULES

## NEVER USE INLINE SQL CONCATENATION

BAD:

```csharp
"where id='" + id + "'"
```

GOOD:

```csharp
cmd.Parameters.AddWithValue("@id", id);
```

OR

```csharp
new { id = id }
```

---

# MODEL RULES

## Location

```text
Models/
```

Models may contain:
- entity models
- request models
- response models

Example:

```csharp
public class Content
{
    public string ttsam_id { get; set; }

    public string ttsad_title { get; set; }
}
```

---

# RESPONSE COMPATIBILITY RULES

Preserve existing JSON property names whenever possible.

DO NOT rename fields unnecessarily.

Examples:

```text
ttsam_id
ttsad_title
GlobalFilePath
```

These fields may already be used by:
- React frontend
- mobile apps
- Android apps
- reporting systems
- existing integrations

---

# OLD VB.NET PARAMETER RULES

Legacy APIs used:

```text
Domain
isonline
TableIndex
APIKEY
```

DO NOT add these parameters in new ASP.NET Core APIs unless explicitly required for backward compatibility.

BAD:

```csharp
GetData(
    string domain,
    string isonline,
    int tableIndex,
    string apiKey)
```

GOOD:

```csharp
Get_Session_Avg_Learning_Time(
    string trainingid)
```

---

# API DESIGN RULES

New APIs must be:
- feature-specific
- strongly typed
- business-oriented
- async where possible

Avoid generic APIs like:

```text
Get_Common_Data
```

---

# RESPONSE RULES

DO NOT use:

```vb
Dictionary(Of String,Object)
JavaScriptSerializer
```

Use:
- typed models
- IActionResult
- automatic ASP.NET Core serialization

GOOD:

```csharp
return Ok(result);
```

---

# NULL SAFETY RULES

Always add null checks for:
- DataRow values
- DataTable rows
- request parameters
- string conversions

GOOD:

```csharp
Convert.ToString(row["name"] ?? "")
```

Avoid:

```csharp
row["name"].ToString()
```

---

# PAGINATION RULES

Preserve existing pagination structure.

Use existing:
- PaginationParam
- PagedResult<T>
- PagedList<T>

Avoid introducing new pagination libraries unless explicitly requested.

GOOD:

```csharp
PagedList<Content>.ToPagedList(
    data,
    param.PageNumber,
    param.PageSize
)
```

---

# ASYNC RULES

Preferred controller pattern:

```csharp
public async Task<IActionResult>
```

Preferred DB calls:

```csharp
await cmd.ExecuteReaderAsync();
await connection.QueryAsync<T>();
```

Avoid synchronous DB calls in new code where possible.

---

# CONFIGURATION RULES

Use:

```csharp
IConfiguration
```

DO NOT use:

```vb
ConfigurationManager
```

Store settings in:

```json
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "LitteraDatabase": ""
  }
}
```

---

# AUTHENTICATION RULES

Avoid:

```text
?APIKEY=XXXX
```

Preferred:
- JWT authentication
- middleware
- authorization filters

Example:

```csharp
[Authorize]
```

---

# LOGGING RULES

Use:

```csharp
ILogger<T>
```

Avoid using:
- log4net in new APIs

---

# ERROR HANDLING RULES

Avoid repetitive try/catch blocks in every API.

Preferred:
- centralized exception middleware

Use try/catch only where business handling is required.

---

# SERIALIZATION RULES

ASP.NET Core automatically serializes JSON responses.

DO NOT use:
- JavaScriptSerializer

---

# SWAGGER RULES

All APIs should include Swagger annotations.

GOOD:

```csharp
[SwaggerOperation(
    Summary = "Get Session Avg Learning Time",
    Description = "Returns session-wise learning statistics"
)]
```

---

# FILE/FOLDER SECURITY RULES

Never expose:
- physical server paths
- local disk paths

Always sanitize:
- filenames
- folder names
- uploaded file paths

---

# EXPECTED COPILOT OUTPUT

Copilot should generate:

1. Controller methods
2. BLContext methods
3. DBContext methods
4. Model classes
5. Swagger annotations
6. Parameterized SQL
7. Async support
8. Proper IActionResult responses
9. Null-safe code
10. Stored procedure-based DB logic

using ONLY the CURRENT project structure.

---

# FINAL GOAL

Modernize old VB.NET APIs into:

✅ ASP.NET Core 6 APIs  
✅ Secure APIs  
✅ Async APIs  
✅ Strongly Typed APIs  
✅ Swagger Documented APIs  
✅ Mobile-Friendly JSON APIs  
✅ Production-Ready APIs  

while preserving the existing architecture:

```text
Controllers/
BLContext/
DBContext/
Models/
```

without forcing unnecessary architecture changes.