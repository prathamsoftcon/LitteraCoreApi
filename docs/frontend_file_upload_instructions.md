# Frontend File Upload Instructions

## Purpose

Use this guide from the React frontend to upload a physical file through the backend upload API.

## API Endpoint

- `POST /api/Upload/UploadFile`

Legacy-compatible route also works:

- `POST /Upload/UploadFile`

## Request Type

- `multipart/form-data`

## Frontend Inputs

The frontend should send only these values:

- `file`: the browser-selected file
- `url`: the site base URL from React config
- `path`: the relative upload folder from React config

## Backend Physical Path

The frontend does **not** send the physical server path.

The backend reads the physical root from:

- `appsettings.json`
- `UploadSettings:PhysicalRootPath`

Then it combines:

- `PhysicalRootPath`
- `path`
- generated unique file name

Example:

- `UploadSettings:PhysicalRootPath = C:\Projects\upload`
- `path = Training_Upload/Content`

Saved file location:

- `C:\Projects\upload\Training_Upload\Content\{generated-file-name}`

## Where `url` and `path` come from

The frontend should first call:

- `GET /api/GET_REACT_APP_CONFIGURATION`

Relevant values returned by that API:

- `LITTERA_CDN_BASE_URL`
- `LITTERA_CONTENT_PATH`

Use them like this:

- `url = LITTERA_CDN_BASE_URL`
- `path = LITTERA_CONTENT_PATH`

Example config values:

```json
{
  "LITTERA_CDN_BASE_URL": "http://localhost:8888/",
  "LITTERA_CONTENT_PATH": "Training_Upload/Content/"
}
```

## Example Request

If the user selects:

- `C:\digital.pdf`

and React config returns:

- `url = http://localhost:8888/`
- `path = Training_Upload/Content/`

then the frontend uploads with form-data:

- `file = digital.pdf`
- `url = http://localhost:8888/`
- `path = Training_Upload/Content/`

## Example Using Fetch

```javascript
async function uploadContentFile(file, reactConfig, token) {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("url", reactConfig.LITTERA_CDN_BASE_URL);
  formData.append("path", reactConfig.LITTERA_CONTENT_PATH);

  const response = await fetch("/api/Upload/UploadFile", {
    method: "POST",
    headers: {
      "Content-Type": "multipart/form-data"
    },
    body: formData
  });

  if (!response.ok) {
    throw new Error("File upload failed.");
  }

  return response.json();
}
```

## Example Using Axios

```javascript
import axios from "axios";

async function uploadContentFile(file, reactConfig) {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("url", reactConfig.LITTERA_CDN_BASE_URL);
  formData.append("path", reactConfig.LITTERA_CONTENT_PATH);

  const response = await axios.post("/api/Upload/UploadFile", formData, {
    headers: {
      "Content-Type": "multipart/form-data"
    }
  });

  return response.data;
}
```

## Success Response

Example response shape:

```json
{
  "fileName": "digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "originalFileName": "digital.pdf",
  "relativePath": "Training_Upload/Content/digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "physicalPath": "C:\\Projects\\upload\\Training_Upload\\Content\\digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "fileUrl": "http://localhost:8888/Training_Upload/Content/digital_20260720123000123_a1b2c3d4e5f6.pdf",
  "fileSize": 123456
}
```

## Validation Notes

- `file` is required
- this upload API is public and does not require a bearer token
- send `Content-Type: multipart/form-data`
- `path` should be a relative path, not a Windows drive path
- path traversal such as `..` is rejected
- only `http` and `https` absolute URLs are accepted for `url`

## Recommended Frontend Flow

1. Call `GET /api/GET_REACT_APP_CONFIGURATION`.
2. Read `LITTERA_CDN_BASE_URL` and `LITTERA_CONTENT_PATH`.
3. Send the selected file to `POST /api/Upload/UploadFile`.
4. Save the returned `fileName`, `relativePath`, or `fileUrl` as needed.
5. send `Content-Type: multipart/form-data`
