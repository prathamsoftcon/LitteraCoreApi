# Brief Release Summary: Changes After `74713f8`

## Scope

This summary covers 67 commits on `Tokenimplementation` after `74713f87437db715c99561cf33266578835d15da` (14 January 2026) through `fbf064c` (23 August 2026).

## Main changes

- **Authentication and API security** - Introduced bearer-token, API-key, and cookie-based authentication flows; public API route handling; logout/cookie-origin validation; OTP registration and login updates. OTP values were removed from API responses, and later delivery validation was strengthened.
- **Security and reliability** - Reworked dynamic database access to use safer parameter handling, added SQL-exception response handling, expanded audit logging, and added configuration/error-handling guidance.
- **Training Master migration** - Migrated Training Category, Course Title, Fees, Training Type, Payment Type, Sponsor/Paid By, and Participant Level capabilities from the legacy application to API endpoints.
- **Global Content Library** - Added APIs and data-layer support for global content, including its association with training content.
- **File upload and SpeakUp Junior** - Added the upload controller and physical-file model, plus SpeakUp Junior import/upload functionality and frontend integration instructions.
- **Interactive Player** - Added the Interactive Player API, business/data layers, activity response support, activity-status soft delete, and required database migrations.
- **Session and certificates** - Added session save/edit/delete support, check-in-related updates, and certificate-processing fixes.
- **Email and SMS** - Improved SMTP configuration precedence and error handling, bulk participant mail delivery, OTP channel routing, and protection against blank email/mobile destinations.
- **Client configuration** - Added a Play Store download key to client data.

## Important delivery considerations

- Apply the database migrations in `database/migrations/`, especially the July upload and Interactive Player migrations.
- Review the frontend authentication, upload, and cookie-origin configuration documents before deploying browser clients.
- Update clients that depended on an OTP being returned by an API; OTP endpoints now confirm delivery instead.
- Update bulk-mail consumers: `POST /TRG_SEND_PARTICIPANT_MAIL` returns `{ "sent": <count> }` instead of `true`.
- Configure SMTP application setting `7` and OTP setting `6`; verify the required Training Master stored procedures exist in the target database.

## Configuration and dependency updates

| Area | Update required | Dependency / validation |
| --- | --- | --- |
| Database connection | Set `ConnectionStrings:LitteraDatabase` and `ConnectionStrings:Masterconfig` for the target SQL Server. Do not use the repository values as production credentials. | The API, legacy-migration endpoints, and email/OTP settings depend on these databases. |
| JWT and API key | Set `JWT_SECRET`, `JWT_ISSUER`, `JWT_AUDIENCE`, and `API_KEY` as deployment secrets. Environment variables override `appsettings.json`; `JWT_SECRET` must be at least 32 bytes. | React proxy calls to `PublicApiKey` endpoints must send the same value in the `ApiKey` header. Do not expose it to browser bundles. |
| Browser origins | Set `CORSHost` to every exact HTTP(S) React origin. | Wildcards or invalid origins prevent API startup. Cookie-authenticated writes also require an approved browser origin; production must use HTTPS. |
| Reverse proxy | Replace `ForwardedHeaders:KnownProxies` with the IP addresses of the production load balancer/reverse proxy. | Required for correct forwarded client IP, host, and scheme processing. |
| Upload storage | Set `UploadSettings:PhysicalRootPath` to the server's writable upload root. | The application appends the client `Path`; grant the API process write permission only to this root. Upload requests fail when the setting is blank. |
| SMTP and OTP | Populate application setting `7` with SMTP `host`, `portno`, `login`, and `password`; populate setting `6` with the intended `OTP_ON_SMS` and `OTP_ON_MAIL` flags. | `wwwroot/Content/GlobalSetting/emailSetting.xml` remains a fallback for SMTP fields and supplies branding/template compatibility values. |
| Client branding | Review `wwwroot/Content/GlobalSetting/ClientData.xml`, especially `PLAY_STORE_LINK`, `DEFAULT_LOGO2`, application title, and logo paths. | Referenced logo files must exist under `wwwroot`; browser clients receive these values through client-data APIs. |
| Interactive Player database | Run the three July Interactive Player scripts: upload limits, activity responses, and activity status soft delete. | The upload-limit script contains a TODO: confirm the actual activity table name before execution. The response scripts expect `trainingplan.tbl_tp_ip_acvtivity_master`. |
| Audit database | Run `20260612_expand_audit_trail_ip.sql`. | Required for the extended audit-trail IP data. |
| Training Master database | Ensure the legacy TrainingPlan procedures used by the migrated endpoints are present, including participant-level, category, fees, training-type, sponsor-type, and course-title procedures. | The API calls these procedures directly; they are not created by a migration in this repository. |
| Build/runtime packages | Restore NuGet packages from `LitteraCore.csproj`; deployment includes `dlllib/YEncryptDecryptData.dll`. | New/updated dependencies include `Microsoft.AspNetCore.Authentication.JwtBearer`, `Google.Cloud.Storage.V1`, MailKit 4.8.0, and MimeKit 4.8.0. |

Production configuration should be provided via protected environment variables or the deployment secret store, not committed configuration files. The API writes error logs to `logs/error.log`; ensure that directory is writable if file logging is retained.

## Detailed August changes

For the 14-23 August portion of this range, including endpoint-level compatibility notes and verification checks, see [Release Notes: 14-23 August 2026](release-notes-2026-08-14-to-2026-08-23.md).

## Commit timeline

| Period | Areas delivered |
| --- | --- |
| 18 Jan-20 Feb | Visual Studio upgrade, bearer-token authentication, and build/JWT fixes |
| Apr-May | Documentation baseline, global file type/folder APIs, and client-logo configuration |
| Jun | API-key/cookie hardening, public registration, audit improvements, and SQL-injection remediation |
| Jul | Training Master and Global Content migrations; public API, upload/SpeakUp Junior, and Interactive Player work |
| Aug | Certificates, session/check-in changes, Play Store key, Training Master completion, email, and OTP fixes |
