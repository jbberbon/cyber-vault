using CyberVault.Server.DTO.BlobFile;

namespace CyberVault.Server.Services.FilesService;

public interface IFilesService
{
    public Task<BlobFileResponseDto> ListAsync(string ownerId, string directoryId);
    public Task<BlobFileResponseDto> UploadAsync(string ownerId, IFormFile file, string parentDirectoryId);
    public Task<BlobFileResponseDto> DownloadAsync(DownloadFileServiceDto request);
    public Task<BlobFileResponseDto> DeleteAsync(string ownerId, string fileName, string parentDirectoryId);
}