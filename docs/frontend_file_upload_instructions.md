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

The frontend must always send these 3 values:

- `file`: the browser-selected file
- `url`: the absolute HTTP/HTTPS base URL that should be used to build the returned file URL
- `path`: the relative upload folder under the configured physical root

## Same Contract For Both Upload Scenarios

Use the same request shape for both:

- upload into the API site's own storage
- upload for another site that serves files from a shared folder

The frontend still sends:

- `file`
- `url`
- `path`

The backend always:

- saves the file under `UploadSettings:PhysicalRootPath`
- appends the provided relative `path`
- builds the returned public file URL from the provided absolute `url`

## Backend Physical Path

The frontend does **not** send the physical server path.

The backend reads the physical root from:

- `appsettings.json`
- `UploadSettings:PhysicalRootPath`

If `UploadSettings:PhysicalRootPath` is empty or missing, the upload is rejected with:

- `upload path is missing`

Then it combines:

- `PhysicalRootPath`
- `path`
- generated unique file name

Example:

- `UploadSettings:PhysicalRootPath = C:\Projects\upload`
- `path = Training_Upload/Content`

Saved file location:

- `C:\Projects\upload\Training_Upload\Content\{generated-file-name}`

## How To Use It

For same-site upload:

- `PhysicalRootPath` should point to the API site's served upload root
- `url` should be that site's public base upload URL
- `path` should be the relative folder under that root

Example:

- `PhysicalRootPath = C:\inetpub\qaapp\wwwroot`
- `url = https://qaapp.littera.in/`
- `path = Training_Upload/Content`

For other-site upload:

- `PhysicalRootPath` should point to the shared folder used by that site
- `url` should be the other site's public base upload URL
- `path` should be the relative folder under that shared root

Example:

- `PhysicalRootPath = C:\Projects\upload`
- `url = https://qa.littera.in/`
- `path = Training_Upload/Content`

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
- `url` is required
- `path` is required
- `UploadSettings:PhysicalRootPath` must be configured
- this upload API requires the configured public API key policy
- send `Content-Type: multipart/form-data`
- `path` should be a relative path, not a Windows drive path
- path traversal such as `..` is rejected
- only absolute `http` and `https` URLs are accepted for `url`

## Recommended Frontend Flow

1. Call `GET /api/GET_REACT_APP_CONFIGURATION`.
2. Read `LITTERA_CDN_BASE_URL` and `LITTERA_CONTENT_PATH`.
3. Send the selected file to `POST /api/Upload/UploadFile`.
4. Save the returned `fileName`, `relativePath`, or `fileUrl` as needed.
5. send `Content-Type: multipart/form-data`
