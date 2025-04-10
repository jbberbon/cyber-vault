namespace CyberVault.Server.DTO.BlobFile;

public class DownloadFileServiceDto
{
    public required string OwnerId { get; set; }
    public required string FileName { get; set; }
    public string? ParentDirectoryId { get; set; }
    public ForDownloadOrPreview DownloadOrPreview { get; set; } = new ForDownloadOrPreview();
}