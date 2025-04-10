namespace CyberVault.Server.DTO.BlobFile;

public class BlobFileResponseDto
{
    /*public BlobFileResponseDto()
    {
        BlobFile = new BlobFileDto();
    }*/
    
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
    public int ErrorCode { get; set; } = 0;
    public BlobFileDto BlobFile { get; set; } = new BlobFileDto();
    public BlobFileListDto BlobFileList { get; set; } = new BlobFileListDto();
    public BlobSasUrlDto SasUrl { get; set; } = new BlobSasUrlDto();
}