using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;

namespace CyberVault.Server.Services;

public interface IAzureBlobService
{
    BlobContainerClient BlobContainerClient { get; }
    DataLakeServiceClient DataLakeServiceClient { get; }

}