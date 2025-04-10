using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;

namespace CyberVault.Server.DTO.BlobFile;

public class DeleteFileDto
{
    [StringLengthRange]
    public required string FileName { get; set; }
    [RegularExpression(@"^(?!\s)(?!.*\s$)[A-Fa-f0-9-]{36}$",
        ErrorMessage = "ID must be a valid GUID and cannot have leading or trailing spaces.")]
    public string ParentDirectoryId { get; set; } = "";
}