# Authentication and Cookie Hardening

## Overview

The API authentication flow now supports both:

- `Authorization: Bearer <token>`
- The `Auth_token` HTTP-only cookie

Bearer authentication takes precedence when both are present. Existing successful
login response bodies still include `authToken`, so the frontend response contract
has not changed.

## API Changes

| Area | Previous behavior | New behavior | Impact |
| --- | --- | --- | --- |
| JWT lifetime | 30 days | 8 hours | Users must authenticate again after eight hours. |
| JWT validation | Signature and expiry | Signature, expiry, issuer, and audience | Tokens created before deployment will no longer be accepted because they do not contain the required issuer and audience. |
| Authentication cookie | Inconsistent settings | `HttpOnly`, `Secure`, `SameSite=Lax`, `Path=/`, host-only, eight-hour expiry | JavaScript cannot read the cookie. It is sent only through HTTPS. |
| Authentication selection | Bearer and cookie | Bearer first, cookie fallback | Existing Bearer clients continue to work. |
| `UserInfo` | Anonymous token creation | Authentication required; username must match the token identity | The frontend must not use this endpoint as an anonymous login operation. |
| `UserInfo_wk` | Anonymous token creation | Authentication required; username must match the token identity | Calls without a valid login token return `401`; another username returns `403`. |
| `GenerateOAuthToken` | Token generated from the supplied username | Authentication and matching identity required | Arbitrary user impersonation is blocked. |
| Activity token APIs | Accepted a supplied user ID | Authentication required; supplied user ID must match the authenticated user | Another user ID returns `403`. |
| Logout | No centralized cookie logout | `POST /api/Authentication/api/Logout` expires `Auth_token` | React should call this endpoint during logout. |
| Cookie-authenticated writes | No origin validation | `POST`, `PUT`, `PATCH`, and `DELETE` require an approved `Origin` | Requests from unapproved websites return `403`. |
| CORS | Wildcard-looking configuration values | Exact origins only, with credentials enabled | Every React origin must be explicitly configured. |
| Request logging | Query/body values could be logged | Sensitive query fields are redacted and request bodies are not logged | Authentication secrets are not written to application request logs. |

## React Changes

### 1. Use the Login Response

Continue logging in through:

```text
POST /api/Authentication/api/GetToken
```

The response still contains `authToken`. The response also sets the protected
`Auth_token` cookie.

Do not call `UserInfo_wk` to create the initial authentication token. It is now
an authenticated compatibility endpoint.

### 2. Configure Axios

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
  "/Authentication/api/GetToken",
  loginRequest
);

setAccessToken(response.data.authToken);
```

Bearer is used when available. If it is absent, the API can authenticate using
the HTTP-only cookie.

### 3. Configure `fetch`

Applications using `fetch` must include credentials:

```javascript
const response = await fetch(
  `${apiUrl}/Authentication/api/GetToken`,
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
await fetch(`${apiUrl}/GetDesignations`, {
  credentials: "include",
  headers: {
    Authorization: `Bearer ${authToken}`,
  },
});
```

### 4. Update Logout

Call the API logout endpoint before clearing React authentication state:

```javascript
export async function logout() {
  await api.post("/Authentication/api/Logout");
  setAccessToken(null);
  // Clear the application's user/profile state, then redirect to login.
}
```

React cannot directly delete `Auth_token` because it is HTTP-only.

### 5. Handle Authentication Failures

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

### 6. Update Compatibility Calls

When calling:

```text
GET /api/Authentication/api/UserInfo_wk?username=...
GET /api/Authentication/api/UserInfo?username=...
GET /api/Authentication/api/GenerateOAuthToken?username=...
```

the username must be the logged-in user's username, email, mobile number, or user
ID represented in the current token.

The activity-token APIs must use the logged-in user's ID:

```text
GET /api/Authentication/api/GenerateActivityToken
GET /api/Authentication/api/GenerateActivityToken_wk
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
```

The signing key must contain at least 32 bytes. Production secrets should not be
committed to source control.

Because the cookie is `Secure`, both the React application and API must use
HTTPS. Local development should use `https://localhost` URLs where cookie
authentication is being tested.

## Deployment Impact

1. All users will need to log in again immediately after deployment.
2. Sessions expire after eight hours.
3. React origins missing from `CORSHost` will fail CORS checks.
4. Cookie-authenticated write requests from an unapproved or missing browser
   origin return `403`.
5. Anonymous calls to token-producing compatibility endpoints return `401`.
6. Calls requesting a different user's identity return `403`.

Deploy the API and React changes together if the React application currently
uses `UserInfo_wk` as its anonymous token-generation step.

## Verification Checklist

- Login returns `200`, includes `authToken`, and sets `Auth_token`.
- Browser cookie attributes show `HttpOnly`, `Secure`, `SameSite=Lax`, and an
  expiry approximately eight hours in the future.
- A Bearer-authenticated API request returns `200`.
- A cookie-authenticated API request returns `200`.
- A protected request without either credential returns `401`.
- `UserInfo_wk` with a different username returns `403`.
- An unsafe cookie request from an unapproved origin returns `403`.
- Logout returns `200` and removes the cookie.
- After logout, remove any React Bearer token before verifying that protected
  requests return `401`.
