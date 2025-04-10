using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;
using CyberVault.Server.DTO.BlobFile;

namespace CyberVault.Server.Services.AzureBlobService;

public interface IAzureBlobService
{
    BlobSasUrlDto GenerateReadOnlySasToken(string fileName, ForDownloadOrPreview downloadOrPreview);

}