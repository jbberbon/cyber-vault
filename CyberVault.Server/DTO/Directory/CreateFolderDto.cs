using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.Directory;

public class CreateFolderDto
{
    [RegularExpression(@"^[A-Fa-f0-9-]{36}$", ErrorMessage = "ID must be a valid GUID.")]
    public string? ParentDirectoryId { get; set; }
    
    [RegularExpression(@"^[^\/:*?""<>|]+$", ErrorMessage = "NewFolderName contains invalid characters.")]
    public required string FolderName { get; set; }
}