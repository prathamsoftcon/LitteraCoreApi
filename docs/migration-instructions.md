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