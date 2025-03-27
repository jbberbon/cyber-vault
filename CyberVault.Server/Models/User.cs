using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;
using Microsoft.AspNetCore.Identity;

namespace CyberVault.Server.Models;

public class User : IdentityUser
{
    [StringLengthRange] public string? FirstName { get; set; }
    [StringLengthRange] public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }
    [Required] public DateTime UpdatedAt { get; set; }

    public string GetFullName()
    {
        // Use null-coalescing operator to handle null values and avoid having extra spaces
        string firstName = string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName;
        string lastName = string.IsNullOrEmpty(LastName) ? string.Empty : LastName;

        // If both are empty, return an empty string, else concatenate the names with a space
        return $"{firstName} {lastName}".Trim();
    }

    // Navigation properties
    public List<BlobFile>? BlobFiles { get; set; }
    public List<Folder>? Folders { get; set; }
    public List<BlobFilePermission>? BlobFilePermissions { get; set; }
    public List<FolderPermission>? FolderPermissions { get; set; }
}