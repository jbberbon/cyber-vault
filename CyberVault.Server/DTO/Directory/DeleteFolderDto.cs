using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.Directory;

public class DeleteFolderDto
{
    [RegularExpression(@"^(?!\s)(?!.*\s$)[A-Fa-f0-9-]{36}$",
        ErrorMessage = "ID must be a valid GUID and cannot have leading or trailing spaces.")]
    public required string DirectoryId { get; set; }
}