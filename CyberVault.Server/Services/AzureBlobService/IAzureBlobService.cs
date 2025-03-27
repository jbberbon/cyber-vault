using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;

namespace CyberVault.Server.Services.AzureBlobService;

public interface IAzureBlobService
{
    BlobContainerClient BlobContainerClient { get; }
    DataLakeServiceClient DataLakeServiceClient { get; }
    string GenerateReadOnlySasToken(string fileName);

}