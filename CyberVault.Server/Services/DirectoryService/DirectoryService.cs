using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using CyberVault.Server.Data;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities;
using CyberVault.Server.Models;
using CyberVault.Server.Services.AzureBlobService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CyberVault.Server.Services.DirectoryService;

public class DirectoryService : IDirectoryService
{
    /*private readonly IAzureBlobService _azureBlobService;*/
    private readonly BlobContainerClient _blobContainerClient;
    private readonly DataLakeServiceClient _dataLakeServiceClient;
    private readonly ILogger<DirectoryService> _logger;
    private readonly AppDbContext _dbContext;

    public DirectoryService(
        IAzureBlobService azureBlobService,
        ILogger<DirectoryService> logger,
        AppDbContext dbContext
    )
    {
        _blobContainerClient = azureBlobService.BlobContainerClient;
        _dataLakeServiceClient = azureBlobService.DataLakeServiceClient;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<bool> DirectoryExistsAsync(string directory)
    {
        string directoryPrefix = directory.TrimEnd('/');

        // 01. Get the filesystem client
        var fileSystemClient = _dataLakeServiceClient.GetFileSystemClient(_blobContainerClient.Name);

        // 02. Get the directory client
        var directoryClient = fileSystemClient.GetDirectoryClient(directoryPrefix);

        // 03. Check if the directory exists by trying to get its properties
        var response = await directoryClient.ExistsAsync();

        // 04. Success
        return response.Value;
    }

    public async Task<bool> FolderNameExistsAsync(string ownerId, string folderName, string parentPathWithNoUserId)
    {
        Boolean isExisting;
        // 01. Check on Root Directory
        if (string.IsNullOrEmpty(parentPathWithNoUserId))
        {
            isExisting = await _dbContext.Folders
                .AnyAsync(folder =>
                    folder.OwnerId == ownerId &&
                    folder.Name == folderName &&
                    folder.Path != null &&
                    !EF.Functions.Like(folder.Path, "%/%"));

            return isExisting;
        }

        // 02. Check on Subdirectories
        var tempPath = $"{parentPathWithNoUserId}/{folderName}+";
        isExisting = await _dbContext.Folders
            .AnyAsync(folder =>
                folder.OwnerId == ownerId &&
                folder.Name == folderName &&
                folder.Path != null &&
                folder.Path.StartsWith(tempPath));

        return isExisting;
    }

    public async Task<BlobFileResponseDto> CreateRootAsync(string ownerId)
    {
        var response = new BlobFileResponseDto();
        try
        {
            // 01. Create the Directory in Azure blob
            var fileSystemClient = _dataLakeServiceClient.GetFileSystemClient(_blobContainerClient.Name);
            await fileSystemClient.CreateDirectoryAsync(ownerId);

            // 02. Success
            response.IsSuccess = true;
            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while creating user root directory. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while creating user root directory. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while creating folder."];
            return response;
        }
    }


    public async Task<BlobFileResponseDto> CreateAsync(
        string ownerId,
        string newFolderName,
        string parentDirectoryId = ""
    )
    {
        var response = new BlobFileResponseDto();
        try
        {
            // 01. Prepare fullPath
            var retrievedPath = "";
            string fullPath;
            if (string.IsNullOrEmpty(parentDirectoryId)) // User's root folder
            {
                fullPath = $"{ownerId}/{newFolderName}";
            }
            else // Subdirectory
            {
                var parsedId = Guid.Parse(parentDirectoryId);
                retrievedPath = await _dbContext.Folders
                    .Where(f => f.Id == parsedId)
                    .Select(f => f.Path)
                    .FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(retrievedPath))
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCodes.NotFound;
                    return response;
                }

                fullPath = $"{ownerId}/{retrievedPath}/{newFolderName}";
            }

            // 02. Check if folder name is taken in Database
            var exists = await FolderNameExistsAsync(ownerId, newFolderName, retrievedPath);
            if (exists)
            {
                response.ErrorCode = ErrorCodes.AlreadyExists;
                return response;
            }

            // 03. Assign UUID to the folderName
            var uuid = Guid.NewGuid();
            fullPath = $"{fullPath}+{uuid}";

            // 04. Store new Folder to DB
            var newFolder = new Folder
            {
                Id = uuid,
                Name = newFolderName,
                OwnerId = ownerId,
                Path = DirectoryUtilities.RemoveRootDirectory(ownerId, fullPath),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _dbContext.Folders.AddAsync(newFolder);

            // 05. Create the Directory in Azure blob
            var fileSystemClient = _dataLakeServiceClient.GetFileSystemClient(_blobContainerClient.Name);
            await fileSystemClient.CreateDirectoryAsync(fullPath);

            // 06. Commit changes to DB if nothing happened while adding blob to Azure
            await _dbContext.SaveChangesAsync();

            // 07. Success
            response.IsSuccess = true;
            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while creating directory / folder. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while creating user root directory. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while creating folder."];
            return response;
        }
    }

    public async Task<BlobFileResponseDto> DeleteAsync(string ownerId, string parentDirectoryId)
    {
        var response = new BlobFileResponseDto();
        try
        {
            // 01. Retrieve path from DB
            var parsedId = Guid.Parse(parentDirectoryId);
            var retrievedPath = await _dbContext.Folders
                .Where(f => f.Id == parsedId)
                .Select(f => f.Path)
                .FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(retrievedPath))
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCodes.NotFound;
                return response;
            }

            // 02. Prepare fullPath
            string fullPath = $"{ownerId}/{retrievedPath}";

            // 03. Check if directory exists in Azure
            var exists = await DirectoryExistsAsync(fullPath);
            if (!exists)
            {
                response.ErrorCode = ErrorCodes.NotFound;
                return response;
            }

            // 04. Find folders to delete based on OwnerId and Path containing the retrievedPath
            var foldersToDelete = await _dbContext.Folders
                .Where(f => f.OwnerId == ownerId && f.Path != null && f.Path.Contains(retrievedPath))
                .ToListAsync();

            // 05. Perform DB Delete
            if (foldersToDelete.Count > 0)
            {
                _dbContext.Folders.RemoveRange(foldersToDelete);
                await _dbContext.SaveChangesAsync();
            }

            // 06. Delete all blobs and subdirectories recursively in Azure 
            await DeleteRecursivelyAsync(fullPath);

            // 07. Delete the directory itself on Azure
            var fileSystemClient = _dataLakeServiceClient.GetFileSystemClient(_blobContainerClient.Name);
            var directoryClient = fileSystemClient.GetDirectoryClient(fullPath.TrimEnd('/'));
            try
            {
                await directoryClient.DeleteAsync();
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                // Directory doesn't exist or was already deleted, which is fine
            }

            // 08. Success
            response.IsSuccess = true;
            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while deleting directory / folder. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while deleting directory. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while deleting folder."];
            return response;
        }
    }

    private async Task DeleteRecursivelyAsync(string prefix)
    {
        // 01. Get all items in the current directory
        List<BlobHierarchyItem> items = new List<BlobHierarchyItem>();
        await foreach (var item in _blobContainerClient.GetBlobsByHierarchyAsync(delimiter: "/", prefix: prefix))
        {
            items.Add(item);
        }

        // 02. Process each item
        foreach (var item in items)
        {
            if (item.IsPrefix)
            {
                // 03. Recursively delete sub-directories
                await DeleteRecursivelyAsync(item.Prefix);
            }
            else if (item.Blob != null)
            {
                // 04. Delete blob
                BlobClient blobClient = _blobContainerClient.GetBlobClient(item.Blob.Name);
                await blobClient.DeleteIfExistsAsync();
            }
        }
    }
}