using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using CyberVault.Server.Miscs.Constants;

namespace CyberVault.Server.Services.AzureBlobService;

public class AzureBlobService : IAzureBlobService
{
    private readonly IConfiguration _config;
    private readonly string _storageAccount;
    private readonly string _key;
    public BlobContainerClient BlobContainerClient { get; }
    public DataLakeServiceClient DataLakeServiceClient { get; }

    public AzureBlobService(
        IConfiguration config
    )
    {
        _config = config;
        _storageAccount = _config[UserSecretKeys.BlobStorageAccount] ??
                          throw new ArgumentException("BlobStorageAccount configuration value is missing.");
        _key = _config[UserSecretKeys.BlobKey] ??
               throw new ArgumentException("BlobStorageKey configuration value is missing.");

        var credential = new StorageSharedKeyCredential(_storageAccount, _key);

        // Create blob service client
        var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        BlobContainerClient = blobServiceClient.GetBlobContainerClient("cybervault");

        // Create data lake service client
        var dataLakeUri = $"https://{_storageAccount}.dfs.core.windows.net";
        DataLakeServiceClient = new DataLakeServiceClient(new Uri(dataLakeUri), credential);
    }

    public string GenerateReadOnlySasToken(string fileName)
    {
        // Get a reference to the BlobClient
        var blobClient = BlobContainerClient.GetBlobClient(fileName);

        // Set the SAS token expiration time (e.g., 1 hour from now)
        var expirationTime = DateTimeOffset.UtcNow.AddHours(1);

        // Set the SAS token permissions (e.g., read and write permissions)
        var permissions = BlobSasPermissions.Read;

        // Generate the SAS token for the blob
        var sasToken = blobClient.GenerateSasUri(permissions, expirationTime).Query;
        sasToken = $"{blobClient.Uri}{sasToken}";
        return sasToken;
    }
}