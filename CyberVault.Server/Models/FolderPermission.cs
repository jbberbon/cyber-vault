using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.Models;

public class FolderPermission
{
    public Guid Id { get; set; }
    [Required] public Guid? FolderId { get; set; }
    [Required] public string? UserId { get; set; }
    [Required] public int PermissionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public Folder Folder { get; set; }
    public User User { get; set; }
    public Permission Permission { get; set; }
}