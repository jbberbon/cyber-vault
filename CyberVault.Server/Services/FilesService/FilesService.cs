using Azure.Storage.Blobs;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities;
using CyberVault.Server.Services.AzureBlobService;
using CyberVault.Server.Services.DirectoryService;
using CyberVault.Server.Services.ModelService;

namespace CyberVault.Server.Services.FilesService;

public class FilesService : IFilesService
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly IDirectoryService _directoryService;
    private readonly ILogger<FilesService> _logger;
    private readonly IFolderService _folderService;
    private readonly IAzureBlobService _azureBlobService;

    public FilesService(
        IAzureBlobService azureBlobService,
        IDirectoryService directoryService,
        ILogger<FilesService> logger,
        IFolderService folderService
    )
    {
        _azureBlobService = azureBlobService;
        _blobContainerClient = azureBlobService.BlobContainerClient;
        _directoryService = directoryService;
        _logger = logger;
        _folderService = folderService;
    }

    public async Task<BlobFileResponseDto> ListAsync(string ownerId, string directoryId)
    {
        var response = new BlobFileResponseDto();
        var items = new List<BlobFileDto>();

        try
        {
            // 01. Prepare fullPath
            string fullPath;
            if (string.IsNullOrEmpty(directoryId)) // Root folder
            {
                fullPath = $"{ownerId}/";
            }
            else // Subdirectory
            {
                var retrievedPath = await _folderService
                    .GetFolderPathByIdAndOwnerAsync(
                        Guid.Parse(directoryId),
                        ownerId
                    );
                if (string.IsNullOrEmpty(retrievedPath))
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCodes.NotFound;
                    return response;
                }

                fullPath = $"{ownerId}/{retrievedPath}/";
            }

            // 02. Check if directory exists
            var dirExists = await _directoryService.DirectoryExistsAsync(fullPath);
            if (!dirExists)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCodes.NotFound;
                return response;
            }

            // 03. Retrieve list.
            await foreach (
                var item in _blobContainerClient.GetBlobsByHierarchyAsync(
                    prefix: fullPath,
                    delimiter: "/"
                )
            )
            {
                // 04. Extract the last element of the child item path
                var path = item.IsPrefix ? item.Prefix : item.Blob.Name;
                path = item.IsPrefix ? path.Remove(path.Length - 1) : path; // Remove the Tailing Slash ('/')
                var pathLastElement = DirectoryUtilities.PluckLastDirectoryElement(path);

                // 05. Remove UserID Root folder from the URI
                var sanitizedPath = DirectoryUtilities.RemoveRootDirectory(
                    ownerId,
                    item.IsPrefix ? item.Prefix : item.Blob.Name
                );

                // 06. Process Virtual Folders
                if (item.IsPrefix)
                {
                    // 06.01 Extract FolderName
                    var (extractedName, extractedId) = DirectoryUtilities.SplitNameAndServerAssignedId(pathLastElement);

                    items.Add(new BlobFileDto
                    {
                        ServerAssignedId = extractedId,
                        Uri = sanitizedPath,
                        Name = extractedName,
                        ContentType = "directory"
                    });
                }
                // 07. Process Blobs
                else
                {
                    items.Add(new BlobFileDto
                    {
                        /*ServerAssignedId = extractedId,*/
                        Uri = sanitizedPath,
                        Name = pathLastElement,
                        ContentType = item.Blob.Properties.ContentType
                    });
                }
            }

            response.IsSuccess = true;
            response.BlobFileList = items;
            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while retrieving blobs. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while retrieving blobs. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while retrieving blobs."];
            return response;
        }
    }

    public async Task<BlobFileResponseDto> UploadAsync(string ownerId, IFormFile file, string parentDirectoryId)
    {
        var response = new BlobFileResponseDto();
        try
        {
            // 01. Prepare BlobName
            string blobName;
            if (string.IsNullOrEmpty(parentDirectoryId)) // Root Directory
            {
                blobName = $"{ownerId}/{file.FileName}";
            }
            else // Subdirectory
            {
                var retrievedPath = await _folderService
                    .GetFolderPathByIdAndOwnerAsync(
                        Guid.Parse(parentDirectoryId),
                        ownerId
                    );

                if (string.IsNullOrEmpty(retrievedPath))
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCodes.NotFound;
                    return response;
                }

                blobName = $"{ownerId}/{retrievedPath}/{file.FileName}";
            }

            // 02. Upload the file
            BlobClient client = _blobContainerClient.GetBlobClient(blobName);
            await using (Stream data = file.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            // 03. Success
            response.IsSuccess = true;
            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while uploading file. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while uploading blob. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while uploading blob."];
            return response;
        }
    }

    public async Task<BlobFileResponseDto> DownloadAsync(string ownerId, string fileName, string parentDirectoryId)
    {
        var response = new BlobFileResponseDto();
        try
        {
            // 01. Prepare fullPath
            string fullPath;
            if (string.IsNullOrEmpty(parentDirectoryId)) // Root folder
            {
                fullPath = $"{ownerId}/{fileName}";
            }
            else // Subdirectory
            {
                var retrievedPath = await _folderService
                    .GetFolderPathByIdAndOwnerAsync(
                        Guid.Parse(parentDirectoryId),
                        ownerId
                    );
                if (string.IsNullOrEmpty(retrievedPath))
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCodes.NotFound;
                    return response;
                }

                fullPath = $"{ownerId}/{retrievedPath}/{fileName}";
            }

            // 02. Prepare BlobClient
            BlobClient file = _blobContainerClient.GetBlobClient(fullPath);

            // 03. Check if file exists first
            var fileExists = await file.ExistsAsync();
            if (!fileExists.Value)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCodes.NotFound;
                response.Errors = ["File not found."];
                return response;
            }

            // 04. Get the download URL from Azure
            var sasUrl = _azureBlobService.GenerateReadOnlySasToken(fullPath);

            // 05. Success
            response.IsSuccess = true;
            response.SasUrl = sasUrl;

            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while downloading file. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while downloading blob. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while downloading blob."];
            return response;
        }
    }

    public async Task<BlobFileResponseDto> DeleteAsync(string ownerId, string fileName, string parentDirectoryId)
    {
        var response = new BlobFileResponseDto();
        try
        {
            // 01. Prepare fullPath
            string fullPath;
            if (string.IsNullOrEmpty(parentDirectoryId)) // Root folder
            {
                fullPath = $"{ownerId}/{fileName}";
            }
            else // Subdirectory
            {
                var retrievedPath = await _folderService
                    .GetFolderPathByIdAndOwnerAsync(
                        Guid.Parse(parentDirectoryId),
                        ownerId
                    );
                if (string.IsNullOrEmpty(retrievedPath))
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCodes.NotFound;
                    return response;
                }

                fullPath = $"{ownerId}/{retrievedPath}/{fileName}";
            }

            // 02. Prepare BlobClient
            BlobClient file = _blobContainerClient.GetBlobClient(fullPath);

            // 03. Check if file exists first
            var fileExists = await file.ExistsAsync();
            if (!fileExists.Value)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCodes.NotFound;
                response.Errors = ["File not found."];
                return response;
            }

            // 04. Delete file 
            await file.DeleteAsync();

            // 05. Success
            response.IsSuccess = true;
            return response;
        }
        catch (Azure.RequestFailedException e)
        {
            _logger.LogError("Something went wrong while deleting file. Errors: {Errors}",
                string.Join(", ", e.Message));
            response.IsSuccess = false;
            response.ErrorCode = e.Status;
            response.Errors = [e.ErrorCode ?? "500"];
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while deleting file. Errors: {Errors}", string.Join(", ", e));
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = ["Something went wrong while deleting file."];
            return response;
        }
    }
}