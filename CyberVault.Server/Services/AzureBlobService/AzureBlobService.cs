using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System;
using Azure.Storage;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.Miscs.Constants;

namespace CyberVault.Server.Services.AzureBlobService
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly IConfiguration _configuration;
        private readonly string _accountKey;

        public AzureBlobService(BlobContainerClient blobContainerClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _accountKey = _configuration[UserSecretKeys.BlobAccountKey] ?? throw new ArgumentNullException("BlobStorage:AccountKey");
            _blobContainerClient = blobContainerClient;
        }

        public BlobSasUrlDto GenerateReadOnlySasToken(string fullPath, ForDownloadOrPreview downloadOrPreview)
        {
            var result = new BlobSasUrlDto();
            
            // 01. Get a reference to the BlobClient using the full path (including any subdirectories)
            var blobClient = _blobContainerClient.GetBlobClient(fullPath);

            // 02. Set the SAS token expiration time
            var expirationTime = DateTimeOffset.UtcNow.AddHours(1);

            // 03. Set the SAS token permissions (e.g., read permissions)
            var permissions = BlobSasPermissions.Read;

            // 04. Create the SAS token builder
            var sasBuilder = new BlobSasBuilder(permissions, expirationTime)
            {
                BlobContainerName = _blobContainerClient.Name,
                BlobName = fullPath // Use full path here, not just the file name
            };
            
            // 05. Check if Preview Sas is requested
            if (downloadOrPreview.ForPreview)
            {
                // 05.01  the SAS token
                var previewToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_blobContainerClient.AccountName, _accountKey)).ToString();
                
                // 05.02 Store the preview URL in the result
                result.PreviewSas = $"{blobClient.Uri}?{previewToken}";
            }
            
            // 06. Check if Download Sas is requested
            if (downloadOrPreview.ForDownload)
            {
                // 06.01 Set the content-disposition header to force download
                sasBuilder.ContentDisposition = "attachment;";
                
                // 06.02 Generate the SAS token
                var downloadToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_blobContainerClient.AccountName, _accountKey)).ToString();
                
                // 06.03 Store the download URL in the result
                result.DownloadSas = $"{blobClient.Uri}?{downloadToken}";
            }
            
            // 07. Return the result
            return result;
        }
    }
}