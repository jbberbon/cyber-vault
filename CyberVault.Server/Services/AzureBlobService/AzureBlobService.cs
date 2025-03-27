using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;
using CyberVault.Server.Miscs;

namespace CyberVault.Server.Services;

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
}