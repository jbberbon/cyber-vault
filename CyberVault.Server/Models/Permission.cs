using CyberVault.Server.Models.Validations;

namespace CyberVault.Server.Models;

public class Permission
{
    public int Id { get; set; }
    [StringLengthRange] public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Property
    public List<FolderPermission>? FolderPermissions { get; set; }
    public List<BlobFilePermission>? BlobFilePermissions { get; set; }
}