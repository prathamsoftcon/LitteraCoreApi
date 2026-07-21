namespace LitteraCore.Models
{
    public class PhysicalFileUploadRequest
    {
        public IFormFile? File { get; set; }
        public string? Url { get; set; }
        public string? Path { get; set; }
    }

    public class PhysicalFileUploadResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
}
