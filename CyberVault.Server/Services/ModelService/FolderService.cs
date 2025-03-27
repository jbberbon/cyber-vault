using CyberVault.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CyberVault.Server.Services.ModelService;

public class FolderService : IFolderService
{
    private readonly AppDbContext _dbContext;

    public FolderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetFolderPathByIdAndOwnerAsync(Guid parentDirectoryId, string ownerId)
    {
        var folderPath = await _dbContext.Folders
            .Where(f =>
                f.Id == parentDirectoryId &&
                f.OwnerId == ownerId
            )
            .Select(f => f.Path)
            .FirstOrDefaultAsync();
        return folderPath ?? "";
    }
}