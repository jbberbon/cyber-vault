using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.Models;

public class BlobFilePermission
{
    public Guid Id { get; set; }
    [Required] public Guid FileId { get; set; }
    [Required] public string? UserId { get; set; }
    [Required] public int PermissionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public User? User { get; set; }
    public BlobFile? BlobFile { get; set; }
    public Permission? Permission { get; set; }
}