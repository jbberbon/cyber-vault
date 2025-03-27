using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.BlobFile;

public class UploadFileDto
{
    public required IFormFile File { get; set; }

    [RegularExpression(@"^(?!\s)(?!.*\s$)[A-Fa-f0-9-]{36}$",
        ErrorMessage = "ID must be a valid GUID and cannot have leading or trailing spaces.")]
    public string? ParentFolderId { get; set; } = "";
}