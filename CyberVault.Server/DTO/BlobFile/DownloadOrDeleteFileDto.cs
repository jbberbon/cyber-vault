using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;

namespace CyberVault.Server.DTO.BlobFile;

public class DownloadOrDeleteFileDto
{
    [StringLengthRange]
    public required string FileName { get; set; }
    [RegularExpression(@"^[A-Fa-f0-9-]{36}$", ErrorMessage = "ID must be a valid GUID.")]
    public string ParentDirectoryId { get; set; } = "";
}