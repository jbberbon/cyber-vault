using System.ComponentModel.DataAnnotations;
using CyberVault.Server.Models.Validations;

namespace CyberVault.Server.DTO.BlobFile;

public class DownloadFileDto
{
    [StringLengthRange]
    public required string FileName { get; set; }
    [RegularExpression(@"^(?!\s)(?!.*\s$)[A-Fa-f0-9-]{36}$",
        ErrorMessage = "ID must be a valid GUID and cannot have leading or trailing spaces.")]
    public string ParentDirectoryId { get; set; } = "";
    public ForDownloadOrPreview DownloadOrPreview { get; set; } = new ForDownloadOrPreview();
}


public class ForDownloadOrPreview
{
    public bool ForPreview { get; set; }
    public bool ForDownload { get; set; }
    
    [AtLeastOneTrue("ForPreview", "ForDownload", ErrorMessage = "One of ForPreview or ForDownload must be true.")]
    public bool ValidateForPreviewAndDownload => true;  // Dummy property to trigger validation
}