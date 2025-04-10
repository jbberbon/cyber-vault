/*using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;

namespace CyberVault.Server.DTO.BlobFile;

public class DownloadFileDto
{
    [StringLengthRange]
    public required string FileName { get; set; }
    [RegularExpression(@"^(?!\s)(?!.*\s$)[A-Fa-f0-9-]{36}$",
        ErrorMessage = "ID must be a valid GUID and cannot have leading or trailing spaces.")]
    public string ParentDirectoryId { get; set; } = "";
    public bool ForPreview { get; set; }
    public bool ForDownload { get; set; }
    
    // Use the dynamic validation attribute
    [OnlyOneTrue("ForPreview", "ForDownload", ErrorMessage = "Only one of ForPreview or ForDownload can be true at a time.")]
    public bool ValidateForPreviewAndDownload => true;  // Dummy property to trigger validation
}


public class ForDownloadOrPreview
{
    public bool ForPreview { get; set; }
    public bool ForDownload { get; set; }
}*/