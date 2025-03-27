using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;

namespace CyberVault.Server.Models;

public class Folder
{
    public Guid Id { get; set; }
    [StringLengthRange] public string? Name { get; set; }
    [Required] public string? OwnerId { get; set; }
    [Required] public string? Path { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public User? Owner { get; set; }
    public List<FolderPermission>? FolderPermissions { get; set; }
}