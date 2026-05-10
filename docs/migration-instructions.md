# ASP.NET Core 6 API Migration Instructions
## Legacy VB.NET → Modern .NET Core API Conversion

This instruction file is specifically for migrating old VB.NET APIs like:

- Folder/Get_Folder_Data
- Generic DataSet APIs
- Training APIs
- Content APIs
- Learning Time APIs

into modern ASP.NET Core 6 architecture.

---

# Core Migration Principle

DO NOT blindly copy old VB.NET architecture.

The old system used:
- Domain
- isonline
- TableIndex
- APIKEY
- DataSet
- DataTable
- JavaScriptSerializer
- Generic Stored Procedure APIs

These patterns must NOT be recreated in ASP.NET Core.

---

# Old Legacy Pattern

Example old API:

```text
/Folder/Get_Folder_Data?Domain=YOJNAACADEMY&isonline=1&TableIndex=0&APIKEY=XXX
```

Legacy architecture:

Controller
    ↓
Datamanager.vb
    ↓
Get_Common_Data()
    ↓
DataSet.Tables(TableIndex)
    ↓
JavaScriptSerializer

Reference:
- Datamanager.vb
- FolderController.vb
- TRAININGAPIController.vb

---

# Modern ASP.NET Core 6 Architecture

Use:

Controllers/
Services/
Repositories/
Interfaces/
Models/
DTOs/

Architecture:

Controller
    ↓
Service
    ↓
Repository
    ↓
Database

---

# IMPORTANT MIGRATION RULES

## NEVER USE THESE IN NEW APIs

❌ Domain  
❌ isonline  
❌ TableIndex  
❌ APIKEY in query string  
❌ DataSet  
❌ DataTable  
❌ JavaScriptSerializer  
❌ ConfigurationManager  
❌ System.Web  
❌ HttpContext.Current  
❌ Session[]  
❌ Inline SQL concatenation  
❌ Generic Get_Common_Data APIs  

---

# NEW API DESIGN RULES

## APIs must be:

✅ Feature-specific  
✅ Strongly typed  
✅ Async  
✅ DTO based  
✅ Swagger documented  
✅ Dependency Injection based  
✅ Repository pattern based  
✅ Secure  

---

# Example Conversion

## OLD API

```vb
Public Function Get_Common_Data(
    Domain,
    IsOnline,
    ProcedureName,
    Parameters,
    TableIndex,
    APIKEY
)
```

## NEW API

```csharp
[HttpGet("GetSessionAvgLearningTime")]
public async Task<IActionResult> GetSessionAvgLearningTime(
    [FromQuery] string trainingid)
{
    var result = await _contentService
        .GetSessionAvgLearningTimeAsync(trainingid);

    return Ok(result);
}
```

---

# CONTENT API RULES

For APIs like:

```csharp
[Route("api/Get_Session_Avg_Learning_Time")]
```

DO NOT reintroduce:

```text
domain,
isonline,
tableIndex,
apiKey
```

The new API should ONLY accept business parameters:

GOOD:

```csharp
GetSessionAvgLearningTime(string trainingid)
```

BAD:

```csharp
GetSessionAvgLearningTime(
    string domain,
    string isonline,
    int tableIndex,
    string apiKey,
    string trainingid)
```

---

# CONTROLLER RULES

## Use:

```csharp
[ApiController]
[Route("api/[controller]")]
```

## Controllers should:

- contain no SQL
- contain no business logic
- call services only
- return IActionResult
- use async/await

Example:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IContentService _service;

    public ContentController(IContentService service)
    {
        _service = service;
    }

    [HttpGet("GetSessionAvgLearningTime")]
    public async Task<IActionResult> GetSessionAvgLearningTime(
        string trainingid)
    {
        var result =
            await _service.GetSessionAvgLearningTimeAsync(trainingid);

        return Ok(result);
    }
}
```

---

# SERVICE RULES

Services contain:
- business logic
- validation
- transformation
- aggregation

Services must NOT:
- execute inline SQL
- use DataTables
- use DataSets

---

# REPOSITORY RULES

Repositories contain:
- DB access
- stored procedure execution
- Dapper/ADO.NET code

Repositories must:
- use parameterized queries
- use async calls
- return typed DTOs

---

# DATABASE ACCESS RULES

Preferred:
- Dapper

Allowed:
- ADO.NET

Avoid:
- DataSet-heavy architecture

---

# SQL SECURITY RULES

## NEVER DO THIS

```csharp
"where id='" + id + "'"
```

## ALWAYS DO THIS

```csharp
new SqlParameter("@id", id)
```

OR

```csharp
new { id = id }
```

---

# RESPONSE RULES

## DO NOT RETURN

```vb
Dictionary(Of String,Object)
```

OR

```vb
rows.Add(row)
serializer.Serialize(rows)
```

---

# ALWAYS RETURN DTOs

Example:

```csharp
public class AvgLearningDto
{
    public string SessionId { get; set; }

    public decimal AvgLearning { get; set; }
}
```

---

# STANDARD RESPONSE FORMAT

Use:

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public T Data { get; set; }
}
```

---

# AUTHENTICATION RULES

## DO NOT USE

```text
?APIKEY=XXXX
```

Use:
- JWT authentication
OR
- API key middleware

Preferred:

```csharp
[Authorize]
```

---

# CONFIGURATION RULES

Move all settings into:

```json
appsettings.json
```

Examples:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },

  "ApiSettings": {
    "BaseUrl": ""
  }
}
```

---

# LOGGING RULES

Replace:

```vb
log4net
```

with:

```csharp
ILogger<T>
```

---

# ERROR HANDLING RULES

DO NOT use repetitive try/catch in every API.

Use:
- Global exception middleware

---

# SERIALIZATION RULES

ASP.NET Core automatically serializes JSON.

DO NOT use:
- JavaScriptSerializer

---

# ASYNC RULES

All APIs and repositories must use async methods.

GOOD:

```csharp
await connection.QueryAsync<T>()
```

BAD:

```csharp
connection.Query<T>()
```

---

# SWAGGER RULES

All APIs must include Swagger documentation.

Example:

```csharp
[SwaggerOperation(
    Summary = "Get Session Avg Learning Time",
    Description = "Returns session-wise learning statistics"
)]
```

---

# DTO NAMING RULES

Use:
- Request DTOs
- Response DTOs

Examples:

```text
GetSessionAvgLearningTimeRequest
GetSessionAvgLearningTimeResponse
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

# EXPECTED OUTPUT FROM COPILOT

Copilot should generate:

1. Controller
2. Service interface
3. Service implementation
4. Repository interface
5. Repository implementation
6. DTOs
7. Dependency Injection setup
8. Swagger annotations
9. Async DB calls
10. Parameterized queries
11. Standard API responses

---

# FINAL GOAL

Transform old VB.NET procedural APIs into:

✅ ASP.NET Core 6 APIs  
✅ Clean Architecture  
✅ Layered Structure  
✅ Secure APIs  
✅ Async APIs  
✅ Strongly Typed APIs  
✅ Mobile-Friendly JSON APIs  
✅ Swagger Documented APIs  
✅ Production-Ready APIs

# ASP.NET Core 6 Migration Instructions
## Existing Project Structure Based Migration

This project already uses the following architecture:

Controllers/
BLContext/
DBContext/
Models/

Example:

Controllers/
    ContentController.cs

BLContext/
    ContentBL.cs

DBContext/
    ContentDB.cs

Models/
    Content.cs

Copilot MUST follow this existing structure.
Do NOT introduce unnecessary Clean Architecture or Repository folders unless explicitly requested.

---

# EXISTING ARCHITECTURE RULES

## Controller Layer

Location:

```text
Controllers/
```

Responsibilities:
- API routes
- HTTP request handling
- validation
- calling BL layer
- returning IActionResult

Controllers must NOT:
- contain SQL
- contain DataTable logic
- contain business logic

Example:

```csharp
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
```

---

# BLContext RULES

Location:

```text
BLContext/
```

Responsibilities:
- business logic
- filtering
- aggregation
- calculations
- validation
- combining DB results

BL classes:
- may call DBContext
- may transform models
- may apply calculations

BL classes must NOT:
- execute inline SQL
- use HTTP objects
- use Request/Response directly

Example:

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

Location:

```text
DBContext/
```

Responsibilities:
- database access
- stored procedure execution
- SQL execution
- data mapping

DBContext classes:
- may use ADO.NET
- may use Dapper
- must use parameterized queries

---

# IMPORTANT SECURITY RULES

## NEVER USE INLINE SQL CONCATENATION

BAD:

```csharp
"where id='" + id + "'"
```

GOOD:

```csharp
cmd.Parameters.AddWithValue("@id", id);
```

---

# DATABASE ACCESS RULES

Preferred:
- Dapper

Allowed:
- ADO.NET

Avoid:
- DataSet-heavy architecture
- DataTable-heavy filtering

---

# MODEL RULES

Location:

```text
Models/
```

Models should contain:
- entity models
- response models
- request models

Example:

```csharp
public class Content
{
    public string ttsam_id { get; set; }

    public string ttsad_title { get; set; }
}
```

---

# OLD VB.NET PARAMETERS RULE

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
- async where possible
- business-oriented

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
- automatic JSON serialization

GOOD:

```csharp
return Ok(result);
```

---

# ASYNC RULES

Preferred:

```csharp
public async Task<IActionResult>
```

Preferred DB calls:

```csharp
await cmd.ExecuteReaderAsync();
```

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

---

# LOGGING RULES

Use:

```csharp
ILogger<T>
```

DO NOT use:
- log4net in new APIs

---

# AUTHENTICATION RULES

Avoid:

```text
?APIKEY=XXXX
```

Preferred:
- JWT
- middleware
- authorization filters

---

# ERROR HANDLING RULES

Use:
- try/catch only where needed
- centralized exception handling preferred

---

# SWAGGER RULES

All APIs should include:

```csharp
[SwaggerOperation]
```

Example:

```csharp
[SwaggerOperation(
    Summary = "Get Session Avg Learning Time"
)]
```

---

# SERIALIZATION RULES

ASP.NET Core automatically serializes JSON.

DO NOT use:
- JavaScriptSerializer

---

# FILE MIGRATION RULES

When migrating old VB.NET APIs:

1. Preserve business logic
2. Preserve stored procedure calls
3. Preserve DB structure
4. Convert syntax to C#
5. Remove obsolete VB.NET patterns
6. Simplify architecture gradually

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

using the CURRENT project structure only.

---

# FINAL GOAL

Modernize APIs while preserving existing architecture:

Controllers/
BLContext/
DBContext/
Models/

without forcing unnecessary architecture changes.

# STORED PROCEDURE RULES

This project is heavily dependent on SQL Server stored procedures.

Copilot should:

- preserve stored procedure names
- preserve parameter names
- preserve output structure
- avoid rewriting SP business logic unnecessarily

GOOD:

```csharp
SqlCommand cmd = new SqlCommand(
    "Trainingplan.proc_tp_get_upload_session_attachement",
    con);

cmd.CommandType = CommandType.StoredProcedure;
```

Avoid converting stable stored procedures into LINQ or EF queries unless explicitly requested.
# PAGINATION RULES

Preserve existing pagination structure.

Use:
- PaginationParam
- PagedResult<T>

Avoid introducing new pagination libraries unless requested.

Example:

```csharp
PagedList<Content>.ToPagedList(
    data,
    param.PageNumber,
    param.PageSize
)
```
# NULL SAFETY RULES

Always add null checks for:
- DataRow values
- DataTable rows
- request parameters
- string conversions

Preferred:

```csharp
Convert.ToString(row["name"] ?? "")
```

Avoid:

```csharp
row["name"].ToString()
```

# RESPONSE COMPATIBILITY RULES

Preserve existing JSON property names whenever possible.

Do NOT rename fields unnecessarily.

Example:

```csharp
ttsam_id
ttsad_title
GlobalFilePath
```

These fields may already be used by:
- mobile apps
- React frontend
- old Android apps
- reporting systems

# MIGRATION PRIORITY

Priority during migration:

1. Convert VB.NET syntax to C#
2. Preserve API behavior
3. Preserve response structure
4. Preserve stored procedure calls
5. Remove obsolete VB.NET dependencies
6. Improve security
7. Improve async support
8. Improve architecture gradually

Avoid massive rewrites during initial migration.