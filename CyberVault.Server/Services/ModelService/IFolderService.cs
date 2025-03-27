namespace CyberVault.Server.Services.ModelService;

public interface IFolderService
{
    public Task<string> GetFolderPathByIdAndOwnerAsync(Guid parentDirectoryId, string ownerId);
}