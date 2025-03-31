using CyberVault.Server.DTO.Directory;

namespace CyberVault.Server.DTO.BlobFile;

public class BlobFileListDto
{
    public List<DirectoryArrayDto>? DirectoryPathArray { get; set; }
    public List<BlobFileDto>? Items { get; set; }
}