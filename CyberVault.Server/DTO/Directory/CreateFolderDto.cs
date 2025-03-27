using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.Directory;

public class CreateFolderDto
{
    [RegularExpression(@"^(?!\s)(?!.*\s$)[A-Fa-f0-9-]{36}$",
        ErrorMessage = "ID must be a valid GUID and cannot have leading or trailing spaces.")]
    public string? ParentDirectoryId { get; set; }

    // This regex allows any character except / : * ? " < > | and also disallows leading/trailing spaces
    [RegularExpression(@"^(?!\s)(?!.*\s$)[^\/:*?""<>|]+$",
        ErrorMessage = "NewFolderName contains invalid characters or leading/trailing spaces.")]
    public required string FolderName { get; set; }
}