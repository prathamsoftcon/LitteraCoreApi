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

The endpoint URL-decodes `message` before sending. It queues mail work and returns `200 OK` with `true`; this response does not confirm that every recipient accepted the email.

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

`SmtpEmailService.SendEmailAsync` builds an HTML `MimeMessage` and sends it using MailKit with STARTTLS. The configured SMTP host, port, username, and password are read from application settings:

- Setting ID `6`: OTP email configuration.
- Setting ID `7`: application email configuration.

The service also reads `wwwroot/Content/GlobalSetting/emailSetting.xml` while loading configuration. Templates used by the registration workflow are read from `wwwroot/Content/GlobalSetting/emailTemplate.xml`.

For normal mail sends, delivery failures are recorded in the application error log. Callers can pass `throwOnFailure: true`, which is used by `POST /api/Send_Mail` so that the endpoint can return a failure response.

## Source References

- `Controllers/ApplicationConfigController.cs`
- `Controllers/AuthenticationController.cs`
- `Controllers/ContentController.cs`
- `Controllers/TrainingController.cs`
- `Controllers/UserController.cs`
- `BLContext/ContentBL.cs`
- `Common/EmailService/SmtpEmailService.cs`
