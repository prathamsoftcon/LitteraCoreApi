# Authentication JWT Flow Update

## Overview

JWT generation now happens only inside the active authentication flows:

- `POST /api/Login` handles password login and returns the JWT on success.
- `GET /api/VerifyOTP` handles OTP verification and returns the JWT on success.
- `GET /api/VerifyOTP_wk` follows the same OTP flow as the main endpoint.

The standalone public token endpoint is no longer exposed as an API route.

## What Changed

### Before

- `POST /api/GetToken` accepted login payloads and generated JWTs directly.
- Password login and OTP login both depended on the same public token endpoint.

### After

- Password validation and token issuance happen together inside `/api/Login`.
- OTP validation and token issuance happen together inside `/api/VerifyOTP`.
- The shared token creation and cookie write are handled by an internal helper method.
- `/api/GetToken` no longer has public routing attributes, so Swagger and external callers should not use it.

## Response Behavior

### Password login

Successful password login returns the normal token payload and sets the auth cookie.

### OTP login

Successful OTP verification returns the normal token payload and sets the auth cookie.

### Failure cases

- Invalid password returns `401 Unauthorized`.
- Invalid OTP returns `401 Unauthorized` with `Invalid Otp`.
- Missing user data or unknown user still returns the existing error response used by the controller.

## Client Impact

Clients should call the auth endpoint that matches the login method:

- Password login clients should call `/api/Login`.
- OTP login clients should call `/api/VerifyOTP`.

Do not call `/api/GetToken` directly.

## Notes

- The auth cookie behavior remains intact.
- The returned JWT payload shape is preserved so the frontend change should be minimal.
- If future cleanup is needed, the response contract can be tightened further without changing the token issuer flow.
