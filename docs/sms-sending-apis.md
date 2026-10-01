# SMS-Sending APIs

This document describes how SMS messages are sent by the LitteraCore API and where the SMS provider configuration comes from.

## SMS delivery implementation

SMS delivery is implemented by `Common/SmsService/SmsService.cs`, registered as `ISmsService` in `Program.cs`. The service does not use the `SMSAPI` value from application setting `8`.

When `SendSmsAsync` is called, it loads the provider configuration from:

```text
wwwroot/Content/GlobalSetting/SMSSetting.xml
```

The file is read at runtime from the application's current working directory:

```csharp
xmldoc.Load(Path.Combine(
    Directory.GetCurrentDirectory(),
    "wwwroot/Content/GlobalSetting",
    "SMSSetting.xml"));
```

The XML values are mapped as follows:

| XML element | Use in the provider request |
| --- | --- |
| `url` | SMS provider endpoint |
| `USERID` | Provider user ID |
| `PASSWORD` | Provider password |
| `SENDER_ID` | Sender ID |
| `PEID` | Principal Entity ID |
| `key`, `routeid` | Present in the XML/model, but not currently used when building the request URL |

`SendSmsAsync` builds a GET request to the configured `url`. It adds the provider credentials, recipient number, message, template ID, route, and `PEID` as query-string parameters. Hyphens are removed from the recipient mobile number before the request is sent.

The provider configuration contains credentials and must not be exposed through an API response or committed with real production secrets. Keep deployment-specific values in a protected configuration/secret-management process and ensure the XML file is protected on the server.

## SMS templates

Templates are loaded from:

```text
wwwroot/Content/GlobalSetting/SMSTemplate.xml
```

`GetTemplateMsg(templateType)` selects the `<Template>` whose `<ID>` matches the requested template type. It returns:

- `ID`: application template ID.
- `DLT_CT_ID`: provider template ID, passed to `SendSmsAsync` as `templateId`.
- `text`: message text.

The OTP workflow uses template type `1`. The OTP text is populated by the authentication controller before it calls `SendSmsAsync`.

## APIs and workflows

### `GET /api/SEND_SMS`

Controller: `ApplicationConfigController.SEND_SMS`

Query parameters:

```text
/api/SEND_SMS?mobileno=919999999999&templatetype=1
```

The endpoint loads the requested template and calls `SendSmsAsync`. The current implementation returns `401 Unauthorized` after the send call completes; this is the existing behavior of the endpoint and should be reviewed if callers require a success response.

### Authentication and OTP workflows

The authentication controller calls `ISmsService.SendSmsAsync` for OTP delivery when the application setting permits SMS delivery and the user has a mobile number. The OTP setting is loaded from application setting ID `6`:

```text
Training.sp_get_PortalSetting(@SettinguniqueID = '6')
```

Relevant values include `OTP_ON_SMS` and `OTP_ON_MAIL`. The provider URL and credentials still come from `SMSSetting.xml`; setting ID `6` does not supply the provider URL.

## Application setting ID `8`

Application setting ID `8` represents “SMS send by application”. Its model is `SMS_SEND_BY_APPLICATION`:

```json
{
  "IS_SMS_SEND": "0",
  "SMSAPI": ""
}
```

The value is read from the `SettingValue` column by `ApplicationConfigDB.Get_Application_Setting("8")`. For the public `Get_Application_Setting` response, `SMSAPI` is intentionally removed because it is legacy/credential-bearing configuration and is not used by `SmsService`.

Therefore, the configuration sources are separate:

| Concern | Source |
| --- | --- |
| Whether the application SMS setting is enabled | Application setting ID `8` (`IS_SMS_SEND`) |
| Whether OTP can be sent by SMS | Application setting ID `6` (`OTP_ON_SMS`) |
| SMS provider URL and credentials | `SMSSetting.xml` |
| SMS message and provider template ID | `SMSTemplate.xml` |

## Error handling

If the provider request fails, `SmsService` attempts to send an error notification by email. When `throwOnFailure` is `true`, it throws an `InvalidOperationException`; otherwise, the failure is swallowed after logging/notification. Authentication workflows use the service and handle failed SMS delivery alongside email delivery.

## Source references

- `Common/SmsService/SmsService.cs`
- `Common/SmsService/ISmsService.cs`
- `Controllers/ApplicationConfigController.cs`
- `Controllers/AuthenticationController.cs`
- `DBContext/ApplicationConfigDB.cs`
- `Models/ApplicationSetting.cs`
- `wwwroot/Content/GlobalSetting/SMSSetting.xml`
- `wwwroot/Content/GlobalSetting/SMSTemplate.xml`
