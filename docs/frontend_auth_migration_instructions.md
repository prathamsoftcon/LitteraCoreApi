# Frontend Auth Migration Instructions
#C:\Projects\LitteraCoreReactAPI\docs\authentication_jwt_flow_update.md
## Purpose

This note describes the frontend changes required after moving JWT issuance into
the actual authentication endpoints.

## What Changed

- Password login now uses `POST /api/Login`.
- OTP login now uses `GET /api/VerifyOTP`.
- The public `/api/GetToken` route is no longer part of the supported API flow.
- Both auth paths now issue the JWT and set the `Auth_token` cookie server-side.

## Frontend Actions

### 1. Update password login calls

Replace any call to `/api/GetToken` used for password sign-in with:

```http
POST /api/Login
```

Keep sending the same login payload the backend already expects.

### 2. Update OTP login calls

Replace any call to `/api/GetToken` used for OTP sign-in with:

```http
GET /api/VerifyOTP?username=<username>&otp=<otp>
```

If the existing frontend uses `VerifyOTP_wk`, keep using that only if the code
path already depends on it. New work should prefer the main `VerifyOTP`
endpoint.

### 3. Preserve credentials handling

The backend still sets the HTTP-only `Auth_token` cookie. Make sure browser
requests include credentials:

- `fetch`: `credentials: "include"`
- `axios`: `withCredentials: true`

### 4. Consume the auth response

After a successful login, keep using the returned `authToken` value if the app
stores a bearer token in memory or state.

Typical flow:

1. Call `/api/Login` or `/api/VerifyOTP`.
2. Read `response.data.authToken` when present.
3. Store the token in frontend auth state if needed.
4. Rely on the cookie for subsequent requests when supported.

## Response Expectations

Successful auth responses should continue to include the normal token payload.

Expected behavior:

- `200 OK` on success
- `401 Unauthorized` for invalid password or OTP
- `404 Not Found` when the backend reports an unknown user for password login

## Do Not Do

- Do not call `/api/GetToken` from frontend code.
- Do not try to read `Auth_token` from JavaScript; it is HTTP-only.
- Do not move JWT creation into the browser.

## Suggested Verification

- Confirm password login still redirects or loads the authenticated area.
- Confirm OTP login still completes without exposing the OTP in the response.
- Confirm logout still clears local auth state after the backend cookie is removed.
- Confirm requests continue to succeed when `withCredentials` / `credentials`
  are enabled.
