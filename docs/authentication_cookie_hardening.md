# Authentication, API Key, and Cookie Hardening

## Overview

The API now uses three explicit access models:

- User APIs require `Authorization: Bearer <token>` or the `Auth_token`
  HTTP-only cookie.
- Selected proxy-facing APIs require `ApiKey: <key>` and do not require JWT.
- Only actions explicitly marked `[AllowAnonymous]` accept requests without
  either credential.

Bearer authentication takes precedence when both are present. Existing successful
login response bodies still include `authToken`, so the frontend response contract
has not changed.

The API key must remain on the trusted React server/proxy. It must not be included
in browser JavaScript, public environment variables, or responses sent to the
browser.

## API Changes

| Area | Previous behavior | New behavior | Impact |
| --- | --- | --- | --- |
| Default API authorization | Global MVC authorization filter plus path-based API-key middleware | JWT/cookie fallback authorization policy | Unmarked actions require a valid user token. |
| Proxy API authorization | Path substring exclusions in middleware | Explicit `PublicApiKey` authorization policy | Only actions carrying the policy accept API-key authentication. |
| API-key validation | Middleware accepted direct or Base64-decoded values | Dedicated `ApiKey` authentication scheme with fixed-time direct comparison | Send the configured key exactly as stored. |
| Swagger authentication | Bearer applied globally | Per-operation Bearer, API key, or anonymous metadata | Swagger sends the correct credential for each action. |
| JWT lifetime | 30 days | 8 hours | Users must authenticate again after eight hours. |
| JWT validation | Signature and expiry | Signature, expiry, issuer, and audience | Tokens created before deployment will no longer be accepted because they do not contain the required issuer and audience. |
| Authentication cookie | Inconsistent settings | `HttpOnly`, `Secure`, `SameSite=Lax`, `Path=/`, host-only, eight-hour expiry | JavaScript cannot read the cookie. It is sent only through HTTPS. |
| Authentication selection | Bearer and cookie | Bearer first, cookie fallback | Existing Bearer clients continue to work. |
| `UserInfo` | Anonymous token creation | Authentication required; username must match the token identity | The frontend must not use this endpoint as an anonymous login operation. |
| `UserInfo_wk` | Anonymous token creation | Authentication required; username must match the token identity | Calls without a valid login token return `401`; another username returns `403`. |
| `GenerateOAuthToken` | Token generated from the supplied username | Authentication and matching identity required | Arbitrary user impersonation is blocked. |
| Activity token APIs | Accepted a supplied user ID | Authentication required; supplied user ID must match the authenticated user | Another user ID returns `403`. |
| Logout | No centralized cookie logout | `POST /api/Logout` expires `Auth_token` | React should call this endpoint during logout. |
| Cookie-authenticated writes | No origin validation | `POST`, `PUT`, `PATCH`, and `DELETE` require an approved `Origin` | Requests from unapproved websites return `403`. |
| CORS | Wildcard-looking configuration values | Exact origins only, with credentials enabled | Every React origin must be explicitly configured. |
| Request logging | Query/body values could be logged | Sensitive query fields are redacted and request bodies are not logged | Authentication secrets are not written to application request logs. |

## API Access Inventory

This inventory reflects the controller attributes at the time of this update.
All routes not listed below require JWT/cookie authentication through the default
fallback policy.

### API-Key-Only Routes

These routes require:

```http
ApiKey: <configured-key>
```

A JWT alone does not authorize them.

| Method | Route |
| --- | --- |
| `GET` | `/api/Agency` |
| `GET` | `/api/Salutation` |
| `GET` | `/api/Branches` |
| `GET` | `/api/trainingplan` |
| `GET` | `/api/GetClientData` |
| `GET` | `/api/country` |
| `GET` | `/api/Finacial_year` |
| `GET` | `/api/Get_Application_Setting` |
| `GET` | `/api/Check_Payment_Gateway_Available` |
| `POST` | `/api/Save_Audit_Trail_wk` |
| `POST` | `/api/GetToken` |
| `GET` | `/api/GenerateOTP` |
| `GET` | `/api/GenerateOTP_wk` |
| `GET` | `/api/VerifyOTP` |
| `GET` | `/api/VerifyOTP_wk` |
| `GET` | `/api/GET_REACT_APP_CONFIGURATION` |
| `GET` | `/api/GET_REACT_APP_CONFIGURATION_wk` |
| `GET` | `/api/Send_OTP` |
| `GET` | `/api/Get_Activity_Token_Info` |
| `GET` | `/api/Send_General_OTP` |
| `GET` | `/api/VerifyOTPWithLogin` |
| `POST` | `/api/SAVE_USER_LOG_wk` |
| `GET` | `/api/User_Session_Details` |
| `POST` | `/api/Learning_Time_wk` |
| `GET` | `/api/GET_CONTENT_DETAILS_wk` |
| `GET` | `/api/check_content_learning_exist_wk` |
| `GET` | `/api/Littera_Events` |
| `GET` | `/api/TrgSessions` |
| `POST` | `/api/Update_Session_Status_wk` |
| `GET` | `/api/Check_First_Login_wk` |
| `GET` | `/api/Training_Details` |
| `GET` | `/api/TRG_SPONSOR` |
| `GET` | `/api/TRG_PARTICIPANT_DETAILS_wk` |
| `GET` | `/api/Participants_training_wk` |

### Credential-Free Anonymous Routes

These are the only live controller actions currently marked
`[AllowAnonymous]`. They require neither JWT nor API key:

| Method | Route |
| --- | --- |
| `POST` | `/api/Login_Fail_Entry` |
| `POST` | `/api/Upcoming_Events` |

## React Changes

### 1. Call API-Key Routes Through the Server Proxy

The browser calls a trusted React backend/proxy. The proxy reads the API key from
server-side configuration and adds it to the ASP.NET Core request:

```http
ApiKey: <configured-key>
```

Do not set this header in browser-side Axios or `fetch` code.

### 2. Use the Login Response

Login is now an API-key-only proxy route:

```text
POST /api/GetToken
```

The proxy adds the API key. The response still contains `authToken` and the API
also sets the protected `Auth_token` cookie.

Do not call `UserInfo_wk` to create the initial authentication token. It is now
an authenticated compatibility endpoint.

### 3. Configure Axios

For the least frontend disruption, keep Bearer support and allow the browser to
send the cookie:

```javascript
import axios from "axios";

export const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
  withCredentials: true,
});

export function setAccessToken(token) {
  if (token) {
    api.defaults.headers.common.Authorization = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common.Authorization;
  }
}
```

After login:

```javascript
const response = await api.post(
  "/api/GetToken",
  loginRequest
);

setAccessToken(response.data.authToken);
```

Bearer is used when available. If it is absent, the API can authenticate using
the HTTP-only cookie.

### 4. Configure `fetch`

Applications using `fetch` must include credentials:

```javascript
const response = await fetch(
  `${proxyUrl}/api/GetToken`,
  {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(loginRequest),
  }
);
```

For an authenticated Bearer request:

```javascript
await fetch(`${apiUrl}/api/GetDesignations`, {
  credentials: "include",
  headers: {
    Authorization: `Bearer ${authToken}`,
  },
});
```

### 5. Update Logout

Call the API logout endpoint before clearing React authentication state:

```javascript
export async function logout() {
  await api.post("/api/Logout");
  setAccessToken(null);
  // Clear the application's user/profile state, then redirect to login.
}
```

React cannot directly delete `Auth_token` because it is HTTP-only.

### 6. Handle Authentication Failures

Add a response interceptor that returns the user to login on `401`:

```javascript
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      setAccessToken(null);
      window.location.assign("/login");
    }

    return Promise.reject(error);
  }
);
```

Treat `403` differently: the user is authenticated, but the requested identity
or request origin is not permitted.

### 7. Update Compatibility Calls

When calling:

```text
GET /api/UserInfo_wk?username=...
GET /api/UserInfo?username=...
GET /api/GenerateOAuthToken?username=...
```

the username must be the logged-in user's username, email, mobile number, or user
ID represented in the current token.

The activity-token APIs must use the logged-in user's ID:

```text
GET /api/GenerateActivityToken
GET /api/GenerateActivityToken_wk
```

## Deployment Configuration

Configure each React origin as an exact value:

```json
"CORSHost": [
  "https://app.example.com",
  "https://admin.example.com"
]
```

Do not use entries such as:

```text
https://*.example.com
```

The API fails during startup when a wildcard or invalid origin is configured.

Provide these values through deployment configuration:

```text
JWT_SECRET
JWT_ISSUER
JWT_AUDIENCE
API_KEY
```

The signing key must contain at least 32 bytes. Production secrets should not be
committed to source control.

`API_KEY` overrides the `ApiKey` value in `appsettings.json`. The React server
proxy and ASP.NET Core API must use the same value. The key value is
case-sensitive; the HTTP header name is not.

Because the cookie is `Secure`, both the React application and API must use
HTTPS. Local development should use `https://localhost` URLs where cookie
authentication is being tested.

## Deployment Impact

1. All users will need to log in again immediately after deployment.
2. Sessions expire after eight hours.
3. React origins missing from `CORSHost` will fail CORS checks.
4. Cookie-authenticated write requests from an unapproved or missing browser
   origin return `403`.
5. API-key routes return `401` when the key is missing or invalid.
6. Calls requesting a different user's identity return `403`.
7. React browser bundles must not contain the API key.

Deploy the API and React changes together if the React application currently
uses `UserInfo_wk` as its anonymous token-generation step.

## Verification Checklist

- Login returns `200`, includes `authToken`, and sets `Auth_token`.
- Browser cookie attributes show `HttpOnly`, `Secure`, `SameSite=Lax`, and an
  expiry approximately eight hours in the future.
- A Bearer-authenticated API request returns `200`.
- A cookie-authenticated API request returns `200`.
- A protected request without either credential returns `401`.
- An API-key route with the configured `ApiKey` header returns `200`.
- An API-key route without the header, with an invalid key, or with only JWT
  returns `401`.
- Swagger marks API-key routes with the `ApiKey` security scheme and includes
  the header in generated requests.
- `/api/Login_Fail_Entry` and `/api/Upcoming_Events` remain callable without
  credentials.
- `UserInfo_wk` with a different username returns `403`.
- An unsafe cookie request from an unapproved origin returns `403`.
- Logout returns `200` and removes the cookie.
- After logout, remove any React Bearer token before verifying that protected
  requests return `401`.
