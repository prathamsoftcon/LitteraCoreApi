# QA Auth Flow Instructions

## Purpose

This guide describes how to test the updated authentication flow after JWT
generation was moved into the login and OTP verification endpoints.

## What To Test

### 1. Password login

Use the password login endpoint:

```http
POST /api/Login
```

Validate that:

- a valid username/password returns `200 OK`
- the response includes the normal token payload
- the `Auth_token` cookie is set
- the request does not call `/api/GetToken`

### 2. OTP login

Use the OTP verification endpoint:

```http
GET /api/VerifyOTP?username=<username>&otp=<otp>
```

Validate that:

- a valid OTP returns `200 OK`
- the response includes the normal token payload
- the `Auth_token` cookie is set
- the OTP is not returned in the response body

If the environment still uses `VerifyOTP_wk` in a legacy flow, test that route
only if the build or environment specifically depends on it.

### 3. Invalid credentials

Validate that:

- an invalid password returns `401 Unauthorized`
- an invalid OTP returns `401 Unauthorized`
- an unknown user for password login returns the existing user-not-found
  response

### 4. Deprecated token endpoint

Validate that:

- `/api/GetToken` is not used by the frontend
- Swagger does not show `/api/GetToken` as a supported public auth route
- direct calls to `/api/GetToken` fail or are not routed as an API operation

### 5. Cookie and session behavior

Validate that:

- browser requests are sent with credentials enabled
- the login response sets `Auth_token`
- authenticated requests continue to work using the cookie or bearer token
- logout clears the auth state as expected

## Suggested Test Sequence

1. Open the login page.
2. Perform password login with valid credentials.
3. Confirm the success response and cookie creation.
4. Log out.
5. Perform OTP login with a valid OTP.
6. Confirm the success response, token payload, and cookie creation.
7. Try invalid password and invalid OTP cases.
8. Confirm `/api/GetToken` is not part of the active login flow.

## Notes

- Do not expect the OTP to appear in any successful response body.
- If a failure occurs after OTP delivery, request a new OTP and retry.
- Report any response that still exposes OTP data as a blocking security issue.
