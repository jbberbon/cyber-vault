using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.BlobFile;

public class UploadFileDto
{
    public required IFormFile File { get; set; }

    [RegularExpression(@"^[A-Fa-f0-9-]{36}$", ErrorMessage = "ID must be a valid GUID.")]
    public string? ParentFolderId { get; set; } = "";
}