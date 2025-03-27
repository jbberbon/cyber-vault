using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.Directory;

public class DeleteFolderDto
{
    [RegularExpression(@"^[A-Fa-f0-9-]{36}$", ErrorMessage = "ID must be a valid GUID.")]
    public required string DirectoryId { get; set; }
}