using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;
using Microsoft.AspNetCore.Identity;

namespace CyberVault.Server.Models;

public class Role : IdentityRole
{
    [StringLengthRange] public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required] public DateTime UpdatedAt { get; set; }
}