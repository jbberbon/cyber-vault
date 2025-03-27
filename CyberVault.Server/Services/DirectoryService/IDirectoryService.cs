using CyberVault.Server.DTO.BlobFile;

namespace CyberVault.Server.Services.DirectoryService;

public interface IDirectoryService
{
    public Task<bool> DirectoryExistsAsync(string directoryPrefix);
    public Task<bool> FolderNameExistsAsync(string ownerId, string folderName, string directoryWithNoOwnerId);

    public Task<BlobFileResponseDto> CreateAsync(string ownerId, string newFolderName, string parentDirectoryId = "");
    public Task<BlobFileResponseDto> DeleteAsync(string ownerId, string parentDirectoryId);
}