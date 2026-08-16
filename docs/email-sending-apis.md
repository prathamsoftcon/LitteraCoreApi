# Email-Sending APIs

This document lists the active API endpoints that send email in the LitteraCore API. All of them ultimately use `SmtpEmailService`, which sends through MailKit SMTP.

## Direct Mail API

### `POST /api/Send_Mail`

Sends one HTML email to a specified recipient.

- Controller: `ApplicationConfigController.Send_Mail`
- Authorization: `PublicApiKey` policy required.
- Request body:

```json
{
  "recipientEmail": "user@example.com",
  "subject": "Subject text",
  "message": "<p>HTML message body</p>"
}
```

- Validation: `recipientEmail`, `subject`, and `message` are required.
- Success response: `200 OK` with `true`.
- Delivery failure: `502 Bad Gateway` with a generic error message. SMTP failures are logged.

## Bulk Participant Mail API

### `POST /TRG_SEND_PARTICIPANT_MAIL`

Sends the same HTML message either to an explicit list of recipients or to all participants in a training.

- Controller: `ApplicationConfigController.TRG_SEND_PARTICIPANT_MAIL`
- Query parameters:
  - `participantttype=2`: send only to addresses in `recipientEmail`.
  - Any other value: send to participants returned for `trainngid`.
  - `trainngid`: training identifier when sending to training participants.
- Request body:

```json
{
  "recipientEmail": ["user1@example.com", "user2@example.com"],
  "subject": "Subject text",
  "message": "%3Cp%3EHTML%20message%20body%3C%2Fp%3E"
}
```

The endpoint URL-decodes `message`, validates that at least one recipient is available, and waits for all sends to complete. It returns `200 OK` with the recipient count on success, `400 Bad Request` for invalid input, and `502 Bad Gateway` when the SMTP provider rejects a message.

## Workflow APIs That Send Mail

| Method and route | Email sent |
| --- | --- |
| `POST /api/CreateUser` | Registration credentials and an optional configured copy email. |
| `GET /api/GenerateOTP` | OTP details when the identifier is an email address. |
| `GET /api/GenerateOTP_wk` | OTP details for the legacy-compatible workflow when the identifier is an email address. |
| `GET /api/Send_OTP` | OTP details for the password/OTP workflow. |
| `GET /api/Send_General_OTP` | General OTP details. |
| `GET /api/TRG_SHARE_CONTENT_MAIL` | A shared training-content link to one or more supplied recipients. |
| `GET /api/Generate_ALL_Certificate` | Certificate-generation completion notification, with a JSON attachment when participants are ineligible. |
| `GET /api/Download_All_Certificate` | Certificate-download processing completion notification, with a JSON attachment when applicable. |

## Mail Delivery Implementation

`SmtpEmailService.SendEmailAsync` builds an HTML `MimeMessage` and sends it using MailKit with STARTTLS. SMTP host, port, username, and password are read from application setting `7`; values in `wwwroot/Content/GlobalSetting/emailSetting.xml` are used only when a setting `7` value is unavailable.

- Setting ID `6`: OTP email configuration.
- Setting ID `7`: application email configuration.

The service also reads `wwwroot/Content/GlobalSetting/emailSetting.xml` for branding and compatibility fallback values. Templates used by the registration workflow are read from `wwwroot/Content/GlobalSetting/emailTemplate.xml`.

For normal mail sends, delivery failures are recorded in the application error log and are surfaced as `EmailDeliveryException` by default. Request-time APIs return a generic `502 Bad Gateway` response. Background certificate and registration notifications explicitly use best-effort sending; their failures are recorded in the error log without changing the already-completed workflow result.

## Source References

- `Controllers/ApplicationConfigController.cs`
- `Controllers/AuthenticationController.cs`
- `Controllers/ContentController.cs`
- `Controllers/TrainingController.cs`
- `Controllers/UserController.cs`
- `BLContext/ContentBL.cs`
- `Common/EmailService/SmtpEmailService.cs`
