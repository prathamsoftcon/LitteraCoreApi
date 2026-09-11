# Access and Refresh Token Implementation Plan

## Purpose

This document records the target authentication design for a future implementation.
It intentionally describes the desired access-token and refresh-token flow; it is
not the behaviour of the application today.

## Current State

The API currently creates one JWT (`authToken`) at login. It is returned in the
login response and written to the `Auth_token` HTTP-only cookie. Its configured
lifetime is eight hours. The API validates that same JWT from either the Bearer
header or the cookie. There is no refresh token, token rotation, token
revocation store, or refresh endpoint.

## Target Flow

```text
Login
  |
  v
Authentication API
  |----------------------------|
  v                            v
Short-lived access token       Long-lived refresh token
(15-30 minutes)                (for example, 7-30 days)
  |                            |
  v                            v
React -> .NET API              HTTP-only secure cookie
  |
  | access token expires (401)
  v
POST /api/Auth/Refresh
  |
  v
Rotated refresh token + new access token
```

The React application retries the original failed request once after a
successful refresh. If refresh fails, it clears local authentication state and
redirects to login.

## Token Design

### Access token

- Use a signed JWT with a 15-30 minute lifetime.
- Keep only identity and authorization claims required by API requests.
- Send it as `Authorization: Bearer <access-token>`.
- Keep it in React memory rather than local storage or session storage.
- Do not use it as the long-lived browser cookie credential.

### Refresh token

- Generate a cryptographically random opaque value; it must not be a JWT.
- Store it only in an HTTP-only, `Secure` cookie, scoped to
  `/api/Auth/Refresh` where practical.
- Store only a hash of the refresh token in the database. Never persist or log
  the raw value.
- Associate the record with user, token family/session ID, issue time, expiry,
  revocation time, and optional device/IP/user-agent audit information.
- Use a longer expiry appropriate to product policy (for example 7-30 days)
  plus an optional absolute session lifetime.
- Rotate it on every successful refresh: revoke the submitted value and issue a
  replacement in the same token family.

## Endpoint Contract

### Login and OTP verification

Existing successful login paths (`/api/Login`, `/api/VerifyOTP`, and supported
equivalents) should:

1. Validate credentials or OTP.
2. Create a short-lived access JWT.
3. Generate and persist a hashed refresh token.
4. Set the refresh-token cookie.
5. Return the access token and existing user-detail response fields.

The raw refresh token must not be included in JSON.

### `POST /api/Auth/Refresh`

The endpoint reads the refresh cookie and should:

1. Find its hashed record and validate expiry, revocation status, user status,
   and token-family session state.
2. Detect reuse of an already-rotated/revoked token. On detection, revoke the
   entire token family and require login again.
3. Revoke the submitted token, create its replacement, set the replacement
   cookie, and return a new access JWT.
4. Return `401 Unauthorized` for missing, expired, invalid, reused, or revoked
   refresh tokens. Do not disclose which condition occurred.

This endpoint must be explicitly anonymous to normal JWT authorization, since
an expired access token is expected. It must still have rate limiting, CSRF/origin
protection, and request logging that redacts cookies and tokens.

### `POST /api/Logout`

Logout should expire the refresh cookie and revoke the current refresh-token
record or its entire token family. The React app must also clear its in-memory
access token. A logout endpoint cannot invalidate a stateless access JWT already
issued; its short lifetime limits that exposure.

## React Behaviour

1. After login, retain only `authToken` in in-memory auth state.
2. Send API requests with the Bearer access token and `withCredentials: true`
   (or `credentials: "include"`) so the refresh cookie is available.
3. On a protected request returning `401`, queue concurrent failed requests and
   make one refresh call.
4. If refresh succeeds, update in-memory state and retry each queued request
   once with the new access token.
5. If refresh fails, reject queued requests, clear auth state, and redirect to
   login. Never retry refresh in a loop.

The access token should not be placed in `localStorage`, and JavaScript must not
attempt to read the HTTP-only refresh cookie.

## Security and Deployment Requirements

- Use HTTPS in every environment where this is tested; refresh cookies require
  `Secure` and `HttpOnly`.
- Set an explicit `SameSite` value compatible with the deployed React/API
  topology. Cross-site cookies require `SameSite=None; Secure` and robust CSRF
  protection; same-site deployments should prefer `Lax` or `Strict` when
  compatible.
- Keep CORS origins explicit and allow credentials only for trusted origins.
- Add rate limiting to login, OTP, and refresh routes.
- Use a database-backed refresh-token repository so logout/revocation works
  across API instances and restarts.
- Do not add raw tokens, authorization headers, cookie values, passwords, or
  OTPs to logs, telemetry, exception responses, or audit records.
- Define retention and cleanup for expired/revoked refresh-token records.

## Implementation Checklist

1. Add refresh-token database schema, migration, repository, and cleanup job.
2. Add token generation, hashing, rotation, revocation, and reuse-detection
   services with unit tests.
3. Split the current eight-hour JWT configuration into access-token and
   refresh-token lifetimes.
4. Update every successful login/OTP/registration path to issue both tokens.
5. Implement and protect `POST /api/Auth/Refresh`.
6. Update logout to revoke refresh sessions and clear the refresh cookie.
7. Update React authentication state and its one-time 401 refresh interceptor.
8. Add integration tests for expiry, rotation, reuse detection, logout,
   concurrent 401 handling, cookie attributes, CSRF/origin behaviour, and
   multi-instance persistence.
9. Deploy backend and React changes together, since the old single-token client
   behaviour will no longer represent the intended session lifecycle.

## Acceptance Criteria

- An access token expires after the configured short lifetime.
- A valid refresh cookie returns a new access token and replaces the refresh
  cookie.
- Reusing a rotated refresh token invalidates its token family and forces login.
- Logout prevents future refreshes for that session.
- An expired access token can be refreshed without prompting the user to log in.
- An invalid or expired refresh token results in a clean login redirect, with no
  retry loop and no sensitive token data exposed.
