using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace LitteraCore.Controllers
{
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UploadController> _logger;
        private readonly IConfiguration _configuration;

        public UploadController(
            IWebHostEnvironment environment,
            ILogger<UploadController> logger,
            IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Policy = "PublicApiKey")]
        [Route("api/Upload/UploadFile")]
        [Route("Upload/UploadFile")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation("To physically upload a file to the requested url/path location.")]
        public async Task<IActionResult> UploadFile([FromForm] PhysicalFileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "File is required." });
            }

            try
            {
                var relativeDirectory = ResolveRequestedDirectory(request.Url, request.Path);
                if (string.IsNullOrWhiteSpace(relativeDirectory))
                {
                    return BadRequest(new { message = "A valid path is required." });
                }

                var storageRoot = ResolveStorageRoot();
                var targetDirectory = ResolveSafeDirectory(storageRoot, relativeDirectory);
                Directory.CreateDirectory(targetDirectory);

                var uniqueFileName = BuildUniqueFileName(request.File.FileName);
                var physicalFilePath = Path.Combine(targetDirectory, uniqueFileName);

                await using (var stream = new FileStream(
                    physicalFilePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None))
                {
                    await request.File.CopyToAsync(stream);
                }

                var relativeFilePath = CombineSegments(relativeDirectory, uniqueFileName);
                var response = new PhysicalFileUploadResponse
                {
                    FileName = uniqueFileName,
                    OriginalFileName = request.File.FileName,
                    RelativePath = relativeFilePath,
                    FileUrl = BuildFileUrl(request.Url, relativeFilePath),
                    FileSize = request.File.Length
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file to physical storage.");
                return StatusCode(500, new { message = "File upload failed." });
            }
        }

        private string ResolveStorageRoot()
        {
            var configuredPhysicalRootPath =
                _configuration["UploadSettings:PhysicalRootPath"]?.Trim();
            if (!string.IsNullOrWhiteSpace(configuredPhysicalRootPath))
            {
                if (!Path.IsPathRooted(configuredPhysicalRootPath))
                {
                    throw new ArgumentException(
                        "UploadSettings:PhysicalRootPath must be an absolute filesystem path.");
                }

                return Path.GetFullPath(configuredPhysicalRootPath);
            }

            var webRoot = string.IsNullOrWhiteSpace(_environment.WebRootPath)
                ? Path.Combine(_environment.ContentRootPath, "wwwroot")
                : _environment.WebRootPath;

            return Path.GetFullPath(webRoot);
        }

        private static string ResolveRequestedDirectory(string? url, string? path)
        {
            var relativePath = NormalizeRelativePath(path);
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                return relativePath;
            }

            return ExtractUrlRelativeBase(url);
        }

        private static string ExtractUrlRelativeBase(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri))
            {
                if (absoluteUri.Scheme != Uri.UriSchemeHttp
                    && absoluteUri.Scheme != Uri.UriSchemeHttps)
                {
                    throw new ArgumentException("Only HTTP/HTTPS urls are supported.");
                }

                return NormalizeRelativePath(absoluteUri.AbsolutePath);
            }

            return NormalizeRelativePath(RemoveQueryAndFragment(url));
        }

        private static string NormalizeRelativePath(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var sanitized = RemoveQueryAndFragment(value)
                .Replace('\\', '/')
                .Trim();

            if (sanitized.StartsWith("~/", StringComparison.Ordinal))
            {
                sanitized = sanitized[2..];
            }

            sanitized = sanitized.Trim('/');
            if (string.IsNullOrWhiteSpace(sanitized))
            {
                return string.Empty;
            }

            var segments = sanitized
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var segment in segments)
            {
                if (segment == "." || segment == "..")
                {
                    throw new ArgumentException("Path traversal is not allowed.");
                }

                if (segment.Contains(':'))
                {
                    throw new ArgumentException("Absolute filesystem paths are not allowed.");
                }

                if (segment.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    throw new ArgumentException("The supplied url/path contains invalid characters.");
                }
            }

            return string.Join('/', segments);
        }

        private static string ResolveSafeDirectory(string rootPath, string relativeDirectory)
        {
            var fullRootPath = Path.GetFullPath(rootPath);
            var fullTargetPath = Path.GetFullPath(Path.Combine(fullRootPath, relativeDirectory));
            var comparison = OperatingSystem.IsWindows()
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
            var rootWithSeparator =
                fullRootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            if (!fullTargetPath.StartsWith(rootWithSeparator, comparison))
            {
                throw new ArgumentException("The resolved upload path is outside the application root.");
            }

            return fullTargetPath;
        }

        private static string BuildUniqueFileName(string originalFileName)
        {
            var safeName = SanitizeFileName(Path.GetFileNameWithoutExtension(originalFileName));
            var safeExtension = SanitizeExtension(Path.GetExtension(originalFileName));

            return $"{safeName}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{safeExtension}";
        }

        private static string SanitizeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "file";
            }

            var cleaned = new string(value
                .Where(ch => !Path.GetInvalidFileNameChars().Contains(ch))
                .ToArray())
                .Trim();

            return string.IsNullOrWhiteSpace(cleaned) ? "file" : cleaned;
        }

        private static string SanitizeExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return string.Empty;
            }

            var cleaned = new string(extension
                .Where(ch => char.IsLetterOrDigit(ch) || ch == '.')
                .ToArray());

            return cleaned.StartsWith('.') ? cleaned : $".{cleaned}";
        }

        private string BuildFileUrl(string? url, string relativeFilePath)
        {
            var normalizedFilePath = relativeFilePath.Replace('\\', '/').TrimStart('/');

            if (Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri)
                && (absoluteUri.Scheme == Uri.UriSchemeHttp
                    || absoluteUri.Scheme == Uri.UriSchemeHttps))
            {
                var basePath = NormalizeRelativePath(absoluteUri.AbsolutePath);
                var builder = new UriBuilder(absoluteUri)
                {
                    Path = "/" + CombineSegments(basePath, normalizedFilePath)
                };

                return builder.Uri.ToString();
            }

            var requestBaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            var requestBaseUri = new Uri(requestBaseUrl, UriKind.Absolute);
            return new Uri(requestBaseUri, normalizedFilePath).ToString();
        }

        private static string CombineSegments(string? first, string? second)
        {
            var segments = new[] { first, second }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!.Replace('\\', '/').Trim('/'))
                .Where(value => !string.IsNullOrWhiteSpace(value));

            return string.Join('/', segments);
        }

        private static string RemoveQueryAndFragment(string value)
        {
            var withoutFragment = value.Split('#', 2)[0];
            return withoutFragment.Split('?', 2)[0];
        }
    }
}
