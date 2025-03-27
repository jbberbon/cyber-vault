using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.DTO.BlobFile;

public class GetListItemsDto
{
    [RegularExpression(@"^[A-Fa-f0-9-]{36}$", ErrorMessage = "ID must be a valid GUID.")]
    public string DirectoryId { get; set; } = "";
}