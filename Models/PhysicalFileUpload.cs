using System.ComponentModel.DataAnnotations;

namespace LitteraCore.Models
{
    public class PhysicalFileUploadRequest
    {
        [Required]
        public IFormFile? File { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? Url { get; set; }

        [Required(AllowEmptyStrings = false)]
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
