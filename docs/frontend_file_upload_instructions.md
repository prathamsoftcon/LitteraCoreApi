# Frontend File Upload Instructions

## Purpose

Use this guide when a frontend needs to upload a physical file through the backend upload API.

This document reflects the current backend contract implemented in:

- `Controllers/UploadController.cs`
- `Models/PhysicalFileUpload.cs`

## Backend Endpoint

Accepted backend routes:

- `POST /api/Upload/UploadFile`
- `POST /Upload/UploadFile`

The endpoint uses:

- `[Authorize(Policy = "PublicApiKey")]`

So browser frontends should normally call it through a public proxy that injects the configured API key.

For the Diet React app, that means:

- browser route: `POST /public-api/Upload/UploadFile`
- request path with shared client: `/Upload/UploadFile`

## Request Type

- `multipart/form-data`

Do not send JSON.

Do not manually construct the multipart boundary.

## Required Frontend Inputs

The frontend must send these 3 form-data fields:

- `File`: the browser-selected file
- `Url`: the absolute HTTP/HTTPS base URL used to build the returned public file URL
- `Path`: the relative upload folder under the configured physical root

Example:

```javascript
const formData = new FormData();
formData.append("File", file, file.name);
formData.append("Url", "http://localhost:8888/");
formData.append("Path", "Training_Upload/Content");
```

## Same Contract For Both Upload Scenarios

Use the same request shape for both:

- upload into the API site's own storage
- upload for another site that serves files from a shared folder

The frontend still sends:

- `File`
- `Url`
- `Path`

The backend always:

- saves the file under `UploadSettings:PhysicalRootPath`
- appends the provided relative `Path`
- builds the returned public file URL from the provided absolute `Url`

## Backend Physical Path

The frontend does not send the physical filesystem root.

The backend reads it from config:

- `UploadSettings:PhysicalRootPath`

If `UploadSettings:PhysicalRootPath` is empty or missing, the upload is rejected with:

- `upload path is missing`

Then the backend combines:

- `PhysicalRootPath`
- `Path`
- generated unique file name

Example:

- `UploadSettings:PhysicalRootPath = C:\Projects\upload`
- `Path = Training_Upload/Content`

Saved file location:

- `C:\Projects\upload\Training_Upload\Content\{generated-file-name}`

## How To Use It

For same-site upload:

- `PhysicalRootPath` should point to the site's served upload root
- `Url` should be that site's public base upload URL
- `Path` should be the relative folder under that root

Example:

- `PhysicalRootPath = C:\inetpub\qaapp\wwwroot`
- `Url = https://qaapp.littera.in/`
- `Path = Training_Upload/Content`

For other-site upload:

- `PhysicalRootPath` should point to the shared folder used by that site
- `Url` should be the other site's public base upload URL
- `Path` should be the relative folder under that shared root

Example:

- `PhysicalRootPath = C:\Projects\upload`
- `Url = https://qa.littera.in/`
- `Path = Training_Upload/Content`

## Where `Url` and `Path` Usually Come From

In the Diet React frontend, these normally come from React app configuration values such as:

- `LITTERA_CDN_BASE_URL`
- `LITTERA_CONTENT_PATH`

Typical mapping:

- `Url = LITTERA_CDN_BASE_URL`
- `Path = LITTERA_CONTENT_PATH`

Example:

```json
{
  "LITTERA_CDN_BASE_URL": "http://localhost:8888/",
  "LITTERA_CONTENT_PATH": "Training_Upload/Content"
}
```

## Diet Frontend Recommendation

In the Diet repo, use a dedicated upload client instead of the JSON-oriented public API client.

Recommended client:

- `publicUploadApi` from `src/services/publicRegistrationApi.js`

Do not use `publicRegistrationApi` for file uploads unless you override its JSON content type, because that client forces:

- `Content-Type: application/json`

That header causes multipart upload requests to fail.

Recommended usage:

```javascript
import { publicUploadApi } from "../../services/publicRegistrationApi";

async function uploadContentFile(file, reactConfig) {
  const formData = new FormData();
  formData.append("File", file, file.name);
  formData.append("Path", reactConfig.LITTERA_CONTENT_PATH);
  formData.append(
    "Url",
    `${String(reactConfig.LITTERA_CDN_BASE_URL).replace(/\/+$/, "")}/`
  );

  const response = await publicUploadApi.post("/Upload/UploadFile", formData);
  return response.data;
}
```

## Fetch Example

```javascript
async function uploadContentFile(file, reactConfig) {
  const formData = new FormData();
  formData.append("File", file, file.name);
  formData.append("Path", reactConfig.LITTERA_CONTENT_PATH);
  formData.append(
    "Url",
    `${String(reactConfig.LITTERA_CDN_BASE_URL).replace(/\/+$/, "")}/`
  );

  const response = await fetch("/public-api/Upload/UploadFile", {
    method: "POST",
    body: formData,
    credentials: "include",
    headers: {
      Accept: "application/json, text/plain",
    },
  });

  if (!response.ok) {
    throw new Error("File upload failed.");
  }

  return response.json();
}
```

## Success Response

Example response shape:

```json
{
  "fileName": "digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "originalFileName": "digital.pdf",
  "relativePath": "Training_Upload/Content/digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "fileUrl": "http://localhost:8888/Training_Upload/Content/digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "fileSize": 123456
}
```

The API should not expose the server filesystem path in the frontend response.

## Validation Notes

- `File` is required
- `Url` is required
- `Path` is required
- `UploadSettings:PhysicalRootPath` must be configured
- `Path` should be a relative path, not a Windows drive path
- path traversal such as `..` is rejected
- only absolute `http` and `https` URLs are accepted for `Url`
- this endpoint requires the configured public API key policy

## Recommended Frontend Flow

1. Read upload-related config values such as `LITTERA_CDN_BASE_URL` and `LITTERA_CONTENT_PATH`.
2. Build `FormData` with `File`, `Path`, and `Url`.
3. Send the request as `multipart/form-data`.
4. Save the returned `fileName`, `relativePath`, or `fileUrl` as needed.
