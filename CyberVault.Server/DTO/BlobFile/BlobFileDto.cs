namespace CyberVault.Server.DTO.BlobFile;

public class BlobFileDto
{
    public string? ServerAssignedId { get; set; }
    public string? Uri { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }
    public Stream? Content { get; set; }
}